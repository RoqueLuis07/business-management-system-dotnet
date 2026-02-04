using System.Linq;
using BusinessManagementSystem.Domain.Enums;

namespace BusinessManagementSystem.Domain.Entities
{
    public class WorkOrder
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        // Identificador de negocio (talonario). Único global.
        public string WorkOrderNumber { get; private set; }

        public Client Client { get; private set; }
        public WorkOrderDiagnosis? Diagnosis { get; private set; }
        public Equipment Equipment { get; private set; }

        // Lo que el cliente pide / el problema reportado
        public string RequestedWorkDescription { get; private set; }

        public WorkOrderStatus Status { get; private set; } = WorkOrderStatus.Ingresada;

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        // Garantía corre desde ENTREGA (regla de negocio)
        public DateTime? DeliveredAtLocal { get; private set; }
        public int WarrantyDays { get; private set; } = 30;

        // Asignación simple por ahora (luego lo vinculamos a User)
        public Guid? AssignedMechanicUserId { get; private set; }
        public WorkOrderQuote? Quote { get; private set; }

        // Accesorios (lo que trae / no trae al ingreso)
        public IReadOnlyCollection<WorkOrderAccessory> Accessories => _accessories.AsReadOnly();
        private readonly List<WorkOrderAccessory> _accessories = new();
        public IReadOnlyCollection<WorkOrderPart> Parts => _parts.AsReadOnly();
        private readonly List<WorkOrderPart> _parts = new();

        public WorkOrder(string workOrderNumber, Client client, Equipment equipment, string requestedWorkDescription)
        {
            if (string.IsNullOrWhiteSpace(workOrderNumber))
                throw new ArgumentException("El número de Orden de Trabajo (talonario) es obligatorio.", nameof(workOrderNumber));
            if (client is null)
                throw new ArgumentNullException(nameof(client));
            if (equipment is null)
                throw new ArgumentNullException(nameof(equipment));
            if (string.IsNullOrWhiteSpace(requestedWorkDescription))
                throw new ArgumentException("La descripción del trabajo solicitado es obligatoria.", nameof(requestedWorkDescription));

            WorkOrderNumber = workOrderNumber.Trim(); // conserva ceros a la izquierda
            Client = client;
            Equipment = equipment;
            RequestedWorkDescription = requestedWorkDescription.Trim();
        }

        // -----------------------
        // Comportamiento / reglas
        // -----------------------

        public void AddAccessory(string name, bool isPresent, string? condition)
        {
            EnsureNotDelivered();
            _accessories.Add(new WorkOrderAccessory(name, isPresent, condition));
        }
        public void AddPart(string partName, int quantity)
        {
            EnsureNotDelivered();
            _parts.Add(new WorkOrderPart(partName, quantity));
        }
        public void AssignMechanic(Guid mechanicUserId)
        {
            EnsureNotDelivered();

            if (mechanicUserId == Guid.Empty)
                throw new ArgumentException("El mecánico asignado no es válido.", nameof(mechanicUserId));

            AssignedMechanicUserId = mechanicUserId;

            if (Status == WorkOrderStatus.Ingresada)
                Status = WorkOrderStatus.Asignada;
        }

        public void PricePart(Guid workOrderPartId, decimal unitPrice, Guid? catalogItemId = null)
        {
            EnsureNotDelivered();

            var part = _parts.FirstOrDefault(p => p.Id == workOrderPartId);
            if (part is null)
                throw new InvalidOperationException("No se encontró el repuesto dentro de la OT.");

            part.SetPricing(unitPrice, catalogItemId);
        }

        public void StartDiagnosis()
        {
            EnsureNotDelivered();
            EnsureStatus(WorkOrderStatus.Ingresada, WorkOrderStatus.Asignada);
            Status = WorkOrderStatus.EnDiagnostico;
        }
        public void SetDiagnosis(string findings, string recommendedWork, string? notes, Guid mechanicUserId)
        {
            EnsureNotDelivered();
            EnsureStatus(WorkOrderStatus.EnDiagnostico);

            Diagnosis = new WorkOrderDiagnosis(findings, recommendedWork, notes, mechanicUserId);
        }
        public void CreateOrUpdateQuote(decimal laborCost, string? notes, Guid createdByUserId)
        {
            EnsureNotDelivered();

            // El presupuesto suele nacer tras diagnóstico (o durante),
            // así que permitimos EnDiagnostico o EsperandoAprobacion.
            EnsureStatus(WorkOrderStatus.EnDiagnostico, WorkOrderStatus.EsperandoAprobacion);

            // Para presupuestar bien, necesitamos que los repuestos tengan precio asignado.
            // Si alguno no tiene UnitPrice, no se puede calcular el total.
            var unpriced = _parts.Where(p => p.UnitPrice is null).ToList();
            if (unpriced.Any())
                throw new InvalidOperationException("Hay repuestos sin precio. No se puede generar el presupuesto.");

            var partsTotal = _parts.Sum(p => p.LineTotal ?? 0m);

            if (Quote is null)
                Quote = new WorkOrderQuote(laborCost, partsTotal, notes, createdByUserId);
            else
                Quote.Update(laborCost, partsTotal, notes);

            // Si creamos/actualizamos el presupuesto, pasa a esperar aprobación del cliente
            Status = WorkOrderStatus.EsperandoAprobacion;
        }

        public void MarkWaitingForApproval()
        {
            EnsureNotDelivered();
            EnsureStatus(WorkOrderStatus.EnDiagnostico);
            Status = WorkOrderStatus.EsperandoAprobacion;
        }

        public void Approve()
        {
            EnsureNotDelivered();
            EnsureStatus(WorkOrderStatus.EsperandoAprobacion);
            Status = WorkOrderStatus.Aprobada;
        }

        public void StartRepair()
        {
            EnsureNotDelivered();
            EnsureStatus(WorkOrderStatus.Aprobada, WorkOrderStatus.EnDiagnostico);
            Status = WorkOrderStatus.EnReparacion;
        }

        public void MarkFinished()
        {
            EnsureNotDelivered();
            EnsureStatus(WorkOrderStatus.EnReparacion);
            Status = WorkOrderStatus.Terminada;
        }

        public void MarkReadyForDelivery()
        {
            EnsureNotDelivered();
            EnsureStatus(WorkOrderStatus.Terminada);
            Status = WorkOrderStatus.ListaParaEntrega;
        }

        public void MarkDelivered(DateTime deliveredAtLocal)
        {
            EnsureNotDelivered();
            EnsureStatus(WorkOrderStatus.ListaParaEntrega);

            if (deliveredAtLocal == default)
                throw new ArgumentException("La fecha de entrega no es válida.", nameof(deliveredAtLocal));

            DeliveredAtLocal = deliveredAtLocal;
            Status = WorkOrderStatus.Entregada;
        }

        public bool IsUnderWarranty(DateTime nowLocal)
        {
            if (DeliveredAtLocal is null) return false;
            return nowLocal <= DeliveredAtLocal.Value.AddDays(WarrantyDays);
        }

        public void SetWarrantyDays(int days)
        {
            EnsureNotDelivered();

            if (days < 1 || days > 365)
                throw new ArgumentOutOfRangeException(nameof(days), "La garantía debe estar entre 1 y 365 días.");

            WarrantyDays = days;
        }

        private void EnsureNotDelivered()
        {
            if (Status == WorkOrderStatus.Entregada)
                throw new InvalidOperationException("La OT ya fue entregada/cerrada. No se puede modificar.");
        }

        private void EnsureStatus(params WorkOrderStatus[] allowed)
        {
            if (!allowed.Contains(Status))
                throw new InvalidOperationException($"Operación no permitida en el estado actual: {Status}.");
        }
    }
}

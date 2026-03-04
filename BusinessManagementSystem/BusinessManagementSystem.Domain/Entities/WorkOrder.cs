using System.Linq;
using BusinessManagementSystem.Domain.Enums;

namespace BusinessManagementSystem.Domain.Entities
{
    public class WorkOrder
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        // Identificador de negocio (talonario). �nico global.
        public string WorkOrderNumber { get; private set; }

        public Client Client { get; private set; }
        public Guid ClientId { get; private set; }

        public Equipment Equipment { get; private set; }
        public Guid EquipmentId { get; private set; }

        public WorkOrderDiagnosis? Diagnosis { get; private set; }
        public WorkOrderQuote? Quote { get; private set; }

        public string? QuoteRejectionReason { get; private set; }
        public Guid? QuoteRejectedByUserId { get; private set; }
        public DateTime? QuoteRejectedAtUtc { get; private set; }

        public WorkOrderServiceReport? ServiceReport { get; private set; }

        // Cancelaci�n
        public string? CancellationReason { get; private set; }
        public Guid? CancelledByUserId { get; private set; }
        public DateTime? CancelledAtUtc { get; private set; }

        // Lo que el cliente pide / el problema reportado
        public string RequestedWorkDescription { get; private set; }

        public WorkOrderStatus Status { get; private set; } = WorkOrderStatus.Ingresada;

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

        // Garant�a corre desde ENTREGA (regla de negocio)
        public DateTime? DeliveredAtLocal { get; private set; }
        public int WarrantyDays { get; private set; } = 30;
        public Guid? WarrantyOriginalWorkOrderId { get; private set; }

        // Asignaci�n simple por ahora (luego lo vinculamos a User)
        public Guid? AssignedMechanicUserId { get; private set; }

        // Accesorios (lo que trae / no trae al ingreso)
        public IReadOnlyCollection<WorkOrderAccessory> Accessories => _accessories.AsReadOnly();
        private readonly List<WorkOrderAccessory> _accessories = new();

        // Garant�as registradas (v�nculos)
        public IReadOnlyCollection<WarrantyClaim> WarrantyClaims => _warrantyClaims.AsReadOnly();
        private readonly List<WarrantyClaim> _warrantyClaims = new();

        // Repuestos (mec�nico carga; admin pone precio)
        public IReadOnlyCollection<WorkOrderPart> Parts => _parts.AsReadOnly();
        private readonly List<WorkOrderPart> _parts = new();
        // Parameterless constructor required by EF Core for materialization
        private WorkOrder()
        {
            // for EF
        }
        public WorkOrder(string workOrderNumber, Client client, Equipment equipment, string requestedWorkDescription)
        {
            if (string.IsNullOrWhiteSpace(workOrderNumber))
                throw new ArgumentException("El n�mero de Orden de Trabajo (talonario) es obligatorio.", nameof(workOrderNumber));
            if (client is null)
                throw new ArgumentNullException(nameof(client));
            if (equipment is null)
                throw new ArgumentNullException(nameof(equipment));
            if (string.IsNullOrWhiteSpace(requestedWorkDescription))
                throw new ArgumentException("La descripci�n del trabajo solicitado es obligatoria.", nameof(requestedWorkDescription));

            WorkOrderNumber = workOrderNumber.Trim();
            Client = client;
            ClientId = client.Id;
            Equipment = equipment;
            EquipmentId = equipment.Id;
            RequestedWorkDescription = requestedWorkDescription.Trim();
        }

        // -----------------------
        // Comportamiento / reglas
        // -----------------------

        public void AddAccessory(string name, bool isPresent, string? condition)
        {
            EnsureNotClosed();
            _accessories.Add(new WorkOrderAccessory(name, isPresent, condition));
        }

        public void UpdateAccessory(Guid accessoryId, bool isPresent, string? condition)
        {
            EnsureNotClosed();

            var accessory = _accessories.FirstOrDefault(a => a.Id == accessoryId);
            if (accessory is null)
                throw new InvalidOperationException("No se encontr� el accesorio en la OT.");

            accessory.UpdateCondition(isPresent, condition);
        }

        public void RemoveAccessory(Guid accessoryId)
        {
            EnsureNotClosed();

            var accessory = _accessories.FirstOrDefault(a => a.Id == accessoryId);
            if (accessory is null)
                throw new InvalidOperationException("No se encontr� el accesorio en la OT.");

            _accessories.Remove(accessory);
        }

        public void AddPart(string partName, int quantity)
        {
            EnsureNotClosed();
            _parts.Add(new WorkOrderPart(partName, quantity));
        }

        public void UpdatePartQuantity(Guid workOrderPartId, int quantity)
        {
            EnsureNotClosed();

            var part = _parts.FirstOrDefault(p => p.Id == workOrderPartId);
            if (part is null)
                throw new InvalidOperationException("No se encontr� el repuesto dentro de la OT.");

            part.UpdateQuantity(quantity);

            InvalidateQuoteIfAny();
        }

        public void RemovePart(Guid workOrderPartId)
        {
            EnsureNotClosed();

            var part = _parts.FirstOrDefault(p => p.Id == workOrderPartId);
            if (part is null)
                throw new InvalidOperationException("No se encontr� el repuesto dentro de la OT.");

            _parts.Remove(part);

            InvalidateQuoteIfAny();
        }

        public void PricePart(Guid workOrderPartId, decimal unitPrice, Guid? catalogItemId = null)
        {
            EnsureNotClosed();

            var part = _parts.FirstOrDefault(p => p.Id == workOrderPartId);
            if (part is null)
                throw new InvalidOperationException("No se encontr� el repuesto dentro de la OT.");

            part.SetPricing(unitPrice, catalogItemId);

            InvalidateQuoteIfAny();
        }

        public void AssignMechanic(Guid mechanicUserId)
        {
            EnsureNotClosed();

            if (mechanicUserId == Guid.Empty)
                throw new ArgumentException("El mec�nico asignado no es v�lido.", nameof(mechanicUserId));

            AssignedMechanicUserId = mechanicUserId;

            if (Status == WorkOrderStatus.Ingresada)
                Status = WorkOrderStatus.Asignada;
        }

        public void StartDiagnosis()
        {
            EnsureNotClosed();
            EnsureStatus(WorkOrderStatus.Ingresada, WorkOrderStatus.Asignada);
            Status = WorkOrderStatus.EnDiagnostico;
        }

        public void SetDiagnosis(string findings, string recommendedWork, string? notes, Guid mechanicUserId)
        {
            EnsureNotClosed();
            EnsureStatus(WorkOrderStatus.EnDiagnostico);

            Diagnosis = new WorkOrderDiagnosis(findings, recommendedWork, notes, mechanicUserId);
        }

        public void CreateOrUpdateQuote(decimal laborCost, string? notes, Guid createdByUserId)
        {
            EnsureNotClosed();
            EnsureStatus(WorkOrderStatus.EnDiagnostico, WorkOrderStatus.EsperandoAprobacion, WorkOrderStatus.PresupuestoRechazado);

            var unpriced = _parts.Where(p => p.UnitPrice is null).ToList();
            if (unpriced.Any())
                throw new InvalidOperationException("Hay repuestos sin precio. No se puede generar el presupuesto.");

            var partsTotal = _parts.Sum(p => p.LineTotal ?? 0m);

            if (Quote is null)
                Quote = new WorkOrderQuote(laborCost, partsTotal, notes, createdByUserId);
            else
                Quote.Update(laborCost, partsTotal, notes);

            // Nuevo presupuesto => limpiamos rechazo anterior (si exist�a)
            QuoteRejectionReason = null;
            QuoteRejectedByUserId = null;
            QuoteRejectedAtUtc = null;

            Status = WorkOrderStatus.EsperandoAprobacion;
        }

        public void Approve()
        {
            EnsureNotClosed();
            EnsureStatus(WorkOrderStatus.EsperandoAprobacion);

            if (Quote is null)
                throw new InvalidOperationException("No existe presupuesto para aprobar.");

            Status = WorkOrderStatus.Aprobada;
        }

        public void RejectQuote(string reason, Guid rejectedByUserId)
        {
            EnsureNotClosed();
            EnsureStatus(WorkOrderStatus.EsperandoAprobacion);

            if (Quote is null)
                throw new InvalidOperationException("No existe presupuesto para rechazar.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("El motivo de rechazo es obligatorio.", nameof(reason));

            if (rejectedByUserId == Guid.Empty)
                throw new ArgumentException("El usuario no es v�lido.", nameof(rejectedByUserId));

            QuoteRejectionReason = reason.Trim();
            QuoteRejectedByUserId = rejectedByUserId;
            QuoteRejectedAtUtc = DateTime.UtcNow;

            Status = WorkOrderStatus.PresupuestoRechazado;
        }

        public void Cancel(string reason, Guid cancelledByUserId)
        {
            EnsureNotClosed();

            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException("El motivo de cancelaci�n es obligatorio.", nameof(reason));

            if (cancelledByUserId == Guid.Empty)
                throw new ArgumentException("El usuario no es v�lido.", nameof(cancelledByUserId));

            CancellationReason = reason.Trim();
            CancelledByUserId = cancelledByUserId;
            CancelledAtUtc = DateTime.UtcNow;

            Status = WorkOrderStatus.Cancelada;
        }

        public void StartRepair()
        {
            EnsureNotClosed();
            EnsureStatus(WorkOrderStatus.Aprobada);
            Status = WorkOrderStatus.EnReparacion;
        }

        public void SetServiceReport(string workPerformed, string? recommendations, string? notes, Guid mechanicUserId)
        {
            EnsureNotClosed();
            EnsureStatus(WorkOrderStatus.EnReparacion, WorkOrderStatus.Terminada);

            ServiceReport = new WorkOrderServiceReport(workPerformed, recommendations, notes, mechanicUserId);
        }

        public void MarkFinished()
        {
            EnsureNotClosed();
            EnsureStatus(WorkOrderStatus.EnReparacion);

            if (ServiceReport is null)
                throw new InvalidOperationException("No se puede finalizar la OT sin cargar el trabajo realizado.");

            Status = WorkOrderStatus.Terminada;
        }

        public void MarkReadyForDelivery()
        {
            EnsureNotClosed();
            EnsureStatus(WorkOrderStatus.Terminada);
            Status = WorkOrderStatus.ListaParaEntrega;
        }

        public void MarkDelivered(DateTime deliveredAtLocal)
        {
            EnsureNotClosed();
            EnsureStatus(WorkOrderStatus.ListaParaEntrega);

            if (deliveredAtLocal == default)
                throw new ArgumentException("La fecha de entrega no es v�lida.", nameof(deliveredAtLocal));

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
            EnsureNotClosed();

            if (days < 1 || days > 365)
                throw new ArgumentOutOfRangeException(nameof(days), "La garant�a debe estar entre 1 y 365 d�as.");

            WarrantyDays = days;
        }

        public void MarkAsWarrantyClaimOf(WorkOrder originalWorkOrder, string reason, Guid createdByUserId, DateTime nowLocal)
        {
            EnsureNotClosed();

            if (originalWorkOrder is null)
                throw new ArgumentNullException(nameof(originalWorkOrder));

            if (this.Id == originalWorkOrder.Id)
                throw new InvalidOperationException("Una OT no puede ser garant�a de s� misma.");

            if (originalWorkOrder.Status != WorkOrderStatus.Entregada)
                throw new InvalidOperationException("La OT original debe estar entregada para aplicar garant�a.");

            if (!originalWorkOrder.IsUnderWarranty(nowLocal))
                throw new InvalidOperationException("La OT original est� fuera del per�odo de garant�a.");

            if (Client.Id != originalWorkOrder.Client.Id)
                throw new InvalidOperationException("La garant�a debe pertenecer al mismo cliente.");

            WarrantyOriginalWorkOrderId = originalWorkOrder.Id;

            _warrantyClaims.Add(new WarrantyClaim(originalWorkOrder.Id, this.Id, reason, createdByUserId));
        }

        private void EnsureNotClosed()
        {
            if (Status == WorkOrderStatus.Entregada)
                throw new InvalidOperationException("La OT ya fue entregada/cerrada. No se puede modificar.");

            if (Status == WorkOrderStatus.Cancelada)
                throw new InvalidOperationException("La OT est� cancelada. No se puede modificar.");
        }

        private void EnsureStatus(params WorkOrderStatus[] allowed)
        {
            if (!allowed.Contains(Status))
                throw new InvalidOperationException($"Operaci�n no permitida en el estado actual: {Status}.");
        }

        private void InvalidateQuoteIfAny()
        {
            if (Quote is null) return;

            Quote = null;

            // Si est� en negociaci�n o esperando, volvemos a diagn�stico para recalcular
            if (Status == WorkOrderStatus.EsperandoAprobacion || Status == WorkOrderStatus.PresupuestoRechazado)
                Status = WorkOrderStatus.EnDiagnostico;
        }
    }
}

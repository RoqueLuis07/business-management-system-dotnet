# ? QUICK START LOCAL - 5 MINUTOS

**Ejecutar tu sistema ahora mismo de forma local**

---

## ?? Lo que necesitas

- ? .NET 8 SDK (ya lo tienes)
- ? PostgreSQL 15 (descarga 5 min)
- ? Tu proyecto clonado (ya lo tienes)

---

## 1?? INSTALAR POSTGRESQL (5 minutos)

### Descarga

?? **https://www.postgresql.org/download/windows/**

O directo: **https://www.enterprisedb.com/downloads/postgres-postgresql-downloads**

### Instalar

```
1. Ejecuta el .exe
2. Next ? Next ? Next
3. Contraseña: postgres123 (anota esto)
4. Next ? Install
```

### Verificar que funciona

```powershell
psql --version
# Resultado: psql (PostgreSQL) 15.x
```

---

## 2?? CREAR BASE DE DATOS (2 minutos)

### Opción A: pgAdmin (Más visual)

```
1. Abre navegador ? http://localhost:5050
2. Usuario: postgres, Contraseña: postgres123
3. Right-click Servers ? Database ? Create
4. Nombre: ayr_servicio
5. Click Create
```

### Opción B: Terminal (Más rápido)

```powershell
psql -U postgres

postgres=# CREATE DATABASE ayr_servicio;
postgres=# \q
```

---

## 3?? VERIFICAR BASE DE DATOS (1 minuto)

```powershell
psql -U postgres -l

# Deberías ver:
# ayr_servicio | postgres | UTF8
```

---

## 4?? CONFIGURAR PROYECTO

### Crear `appsettings.json` en raíz

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ayr_servicio;Username=postgres;Password=postgres123;"
  }
}
```

---

## 5?? PRÓXIMAS FASES (Guía completa)

?? **Lee: SETUP_LOCAL_BD.md**

Para:
- Crear Infrastructure project (con EF Core)
- Generar Migrations
- Crear Repositories
- Ejecutar tests

---

## ? LISTO

Tu sistema está conectado a PostgreSQL.

**Próximo:** Crear Infrastructure project (ver SETUP_LOCAL_BD.md paso a paso)


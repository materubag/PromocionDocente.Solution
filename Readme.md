# Proyecto Promoción Docente

Este proyecto está compuesto por una API y un cliente desarrollados en **.NET 9**, utilizando **Blazor** y **ASP.NET Core**, y está diseñado para ser ejecutado y mantenido en **Visual Studio 2022**. Permite la gestión y promoción de docentes, integrando múltiples fuentes de datos.

---

## Requisitos Previos

- **Visual Studio 2022** (con soporte para .NET 9, Blazor y ASP.NET Core)
- **SQL Server** instalado y en funcionamiento.
- **.NET 9 SDK** instalado en el sistema.

- Archivos `.sql` de las bases de datos del proyecto.
- Acceso a los proyectos `PromocionDocentesAPI` y `Client`.

---

## Estructura de Bases de Datos

El sistema utiliza las siguientes bases de datos, cuyos scripts de creación se encuentran en archivos `.sql` dentro del proyecto:

- `PROMOCIONDOCENTE`
- `TTHH`
- `DAC`
- `DIDE`
- `DITIC`

> **IMPORTANTE:**  
> Los nombres de las bases de datos deben ser exactamente como se indica arriba, ya que las cadenas de conexión y la estructura del proyecto dependen de estos nombres.

---

## Configuración Inicial

### 1. Crear las Bases de Datos

Ejecuta los archivos `.sql` correspondientes a cada base de datos en tu instancia de SQL Server utilizando SQL Server Management Studio (SSMS) o una herramienta compatible.

**Pasos:**
1. Abre SSMS y conéctate a tu instancia de SQL Server.
2. Abre cada archivo `.sql` y ejecútalo.
3. Verifica que las bases de datos se han creado correctamente con los nombres exactos:  
   `PROMOCIONDOCENTE`, `TTHH`, `DAC`, `DIDE`, `DITIC`.

---

### 2. Configurar las Cadenas de Conexión

Edita el archivo `appsettings.json` de la API para asegurarte de que el servidor y los nombres de base de datos coinciden con tu entorno local.

Ejemplo de sección de conexiones:

```json
"ConnectionStrings": {
  "PROMOCION_DOCENTE": "Server=TU_SERVIDOR;Database=PROMOCIONDOCENTE;Trusted_Connection=True;TrustServerCertificate=True;",
  "TTHH": "Server=TU_SERVIDOR;Database=TTHH;Trusted_Connection=True;TrustServerCertificate=True;",
  "DAC": "Server=TU_SERVIDOR;Database=DAC;Trusted_Connection=True;TrustServerCertificate=True;",
  "DIDE": "Server=TU_SERVIDOR;Database=DIDE;Trusted_Connection=True;TrustServerCertificate=True;",
  "DITIC": "Server=TU_SERVIDOR;Database=DITIC;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
> **Reemplaza** `TU_SERVIDOR` por el nombre de tu instancia de SQL Server.

---

### 3. Utilización de Metadata

La metadata de cada fuente de datos está definida en la sección `DataSources` del archivo de configuración.  
Ejemplo:

```json
"DataSources": {
  "TTHH": {
    "Fields": {
      "IdContrato": "ID_CONTRATO",
      "CedDoc": "CED_DOC",
      "NombreDocente": "NOMBRE_DOCENTE",
      "NivelDocente": "NIVEL_DOCENTE",
      "FechaContratacion": "FECHA_CONTRATACION",
      "FechaUltimoAscenso": "FECHA_ULTIMO_ASCENSO",
      "PdfContrato": "PDF_CONTRATO",
      "EstadoContrato": "ESTADO_CONTRATO"
    }
  },
  // ... otras fuentes según tu configuración
}
```
**No modifiques los nombres de los campos ni de las bases de datos.**

---

## Ejecución del Proyecto

### 1. Abrir la Solución en Visual Studio 2022

1. Abre **Visual Studio 2022**.
2. Selecciona "Abrir proyecto o solución" y elige el archivo `.sln` del proyecto.

---

### 2. Iniciar la API (PromocionDocentesAPI)

- Haz clic derecho sobre el proyecto de la API y selecciona **Establecer como proyecto de inicio**.
- Presiona **F5** o haz clic en **Iniciar depuración** para ejecutar la API.
- Verifica que la API arranque correctamente y pueda conectarse a todas las bases de datos.

---

### 3. Iniciar el Cliente (Blazor)

- Haz clic derecho sobre el proyecto `Client` y selecciona **Establecer como proyecto de inicio**.
- Presiona **F5** o haz clic en **Iniciar depuración**.
- Luego vuelve a ejecutar el proyecto Blazor en Visual Studio 2022.

---

### 4. Revisión de Metadata

Accede a la API y/o al cliente Blazor para consultar los endpoints o interfaces relacionadas con la metadata y verificar que la conexión y visualización de datos es correcta.

---

## Observaciones Finales

- Si necesitas cambiar el servidor de base de datos, edita la propiedad `Server` en todas las cadenas de conexión del `appsettings.json`.
- Si se agregan nuevas fuentes de datos o se modifican campos, actualiza la sección correspondiente en `DataSources` y en los scripts `.sql`.
- Asegúrate de tener los permisos necesarios en SQL Server para crear bases de datos y tablas.

---

## Soporte

Para dudas o problemas, consulta la documentación interna del proyecto o contacta al equipo de desarrollo.
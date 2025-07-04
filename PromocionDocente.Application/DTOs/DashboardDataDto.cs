using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Application.DTOs
{
    public class DashboardDataDto
    {
        public DashboardEstadisticasDto Estadisticas { get; set; }
        public List<SolicitudDocenteDto> Solicitudes { get; set; }
    }

    public class DashboardEstadisticasDto
    {
        public int TotalDocentes { get; set; }
        public int EvaluacionesPendientes { get; set; }
        public int SolicitudesFinalizadas { get; set; }
        public int SolicitudesRechazadas { get; set; } 
        public int ProcesosActivos { get; set; }
    }


    public class SolicitudDocenteDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string NombreDocente { get; set; }
        public string Nivel { get; set; }
        public DateOnly Fecha { get; set; }
        public string TiempoEspera { get; set; }
        public string Estado { get; set; }
    }

    public class ValidacionDto
    {
        public string Usuario { get; set; }
    }
}

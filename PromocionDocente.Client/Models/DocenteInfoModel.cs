namespace PromocionDocente.Client.Models
{
    public class DocenteInfoModel
    {
        public string NombreCompleto { get; set; }
        public string Codigo { get; set; }
        public string CargoActual { get; set; }
        public string CargoSolicitado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string TiempoEnProceso { get; set; }
        public string Estado { get; set; }
    }
}

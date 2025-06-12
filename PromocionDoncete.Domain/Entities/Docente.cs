namespace PromocionDocente.Domain.Entities
{
    public class Docente
    {
        public string CedDoc { get; set; } = string.Empty;
        public string NomDoc { get; set; } = string.Empty;
        public string ApeDoc { get; set; } = string.Empty;
        public string TelDoc { get; set; } = string.Empty;
        public string UrlCedDoc { get; set; } = string.Empty;
        public DateTime FecIng { get; set; }
        public DateTime FecNac { get; set; }
        public string TitAcaMax { get; set; } = string.Empty;
        public string IdCar { get; set; } = string.Empty;
    }
}

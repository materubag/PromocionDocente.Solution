namespace PromocionDocente.Client.Models
{
    public class DocumentoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Estado { get; set; } = "Pendiente";
        public bool Evaluado { get; set; } = false;
    }
}

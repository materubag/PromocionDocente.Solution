namespace PromocionDocente.Client.Models
{
    public class DocumentoModel
    {
       
            public int Id { get; set; }
            public string Nombre { get; set; } = "";
            public string Estado { get; set; } = "Pendiente"; // Estado inicial
            public bool Evaluado { get; set; } = false;
        
    }
}

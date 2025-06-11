namespace PromocionDocente.Client.Models
{

        public class Obra
        {
            public int Id { get; set; }
            public string Titulo { get; set; } = "";
            public string Autor { get; set; } = "";
            public string Tipo { get; set; } = "";
            public DateTime FechaPublicacion { get; set; }
            public string Estado { get; set; } = "";
            public bool Seleccionado { get; set; }
        }

        public class Docente_Info
        {
            public string Nombre { get; set; } = "";
            public string Cargo { get; set; } = "";
            public string Avatar { get; set; } = "";
        }
    
}

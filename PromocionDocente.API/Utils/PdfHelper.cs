using System;
using System.IO;

namespace PromocionDocente.API.Utils
{
    public static class PdfHelper
    {
        public static void GuardarPdfDesdeBinario(byte[] contenido, string nombreArchivo)
        {
            string ruta = Path.Combine(Directory.GetCurrentDirectory(), "Pdfs", nombreArchivo);
            File.WriteAllBytes(ruta, contenido);
        }

        public static byte[] LeerPdfComoBinario(string nombreArchivo)
        {
            string ruta = Path.Combine(Directory.GetCurrentDirectory(), "Pdfs", nombreArchivo);
            return File.ReadAllBytes(ruta);
        }
    }
}

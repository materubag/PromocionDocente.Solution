using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Infrastructure.Utils
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

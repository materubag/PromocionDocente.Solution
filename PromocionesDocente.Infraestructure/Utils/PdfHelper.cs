using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PromocionDocente.Infrastructure.Utils
{
    public static class PdfHelper
    {
        public static byte[] ConvertirBase64ABinario(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        public static string GuardarBinarioComoPdf(byte[] binario, string cedula, string titulo, string subcarpeta)
        {
            try
            {
                string baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Pdfs", subcarpeta);

                if (!Directory.Exists(baseFolder))
                    Directory.CreateDirectory(baseFolder);

                string nombreLimpio = $"{cedula}_{SanitizarNombre(titulo)}.pdf";
                string rutaCompleta = Path.Combine(baseFolder, nombreLimpio);

                if (File.Exists(rutaCompleta))
                {
                    byte[] existente = File.ReadAllBytes(rutaCompleta);
                    if (!binario.SequenceEqual(existente))
                    {
                        File.WriteAllBytes(rutaCompleta, binario);
                    }
                }
                else
                {
                    File.WriteAllBytes(rutaCompleta, binario);
                }

                return $"https://localhost:7088/Pdfs/{subcarpeta}/{nombreLimpio}";
            }
            catch
            {
                return "";
            }
        }

        private static string SanitizarNombre(string nombre)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                nombre = nombre.Replace(c, '_');
            }

            nombre = nombre.Replace(" ", "_");
            nombre = Regex.Replace(nombre, @"[^\p{L}\p{N}_\.]", "_");

            return nombre;
        }
    }
}

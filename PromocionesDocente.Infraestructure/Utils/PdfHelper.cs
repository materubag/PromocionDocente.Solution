using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using static System.Net.Mime.MediaTypeNames;

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

        public static byte[] GenerarPdfPostulacion(string cedula, string nombreDocente,
        string categoriaAnterior, string categoriaNueva, DateOnly fechaAprobacion, string usuario)
        {
            // En lugar de generar un PDF real, creamos un documento de texto en memoria
            // que representa la información que iría en el PDF
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(ms))
            {
                writer.WriteLine("CERTIFICADO DE PROMOCIÓN DOCENTE");
                writer.WriteLine("================================");
                writer.WriteLine();
                writer.WriteLine($"Fecha: {fechaAprobacion.ToString("dd/MM/yyyy")}");
                writer.WriteLine();
                writer.WriteLine("Por medio del presente documento se certifica que:");
                writer.WriteLine();
                writer.WriteLine($"El/La docente {nombreDocente} con cédula {cedula} ha sido " +
                    $"promovido/a de la categoría {categoriaAnterior} a la categoría {categoriaNueva}, " +
                    $"cumpliendo con todos los requisitos establecidos para dicha promoción.");
                writer.WriteLine();
                writer.WriteLine($"La promoción ha sido aprobada con fecha {fechaAprobacion.ToString("dd/MM/yyyy")}.");
                writer.WriteLine();
                writer.WriteLine("Este documento ha sido generado automáticamente por el " +
                    "Sistema de Promoción Docente.");
                writer.WriteLine($"Revisor: {usuario}");

                writer.Flush();

                // Convertir a base64 y luego a binario para simular el proceso
                return ms.ToArray();
            }
        }
    }
}

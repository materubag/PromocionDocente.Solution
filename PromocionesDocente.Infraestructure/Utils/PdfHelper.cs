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
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using static System.Net.Mime.MediaTypeNames;
using Document = QuestPDF.Fluent.Document;


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
            // Configurar QuestPDF (solo necesario una vez en la aplicación)
            QuestPDF.Settings.License = LicenseType.Community;

            // Generar documento
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(header =>
                    {
                        header.AlignCenter().Text("CERTIFICADO DE PROMOCIÓN DOCENTE")
                            .FontSize(16)
                            .Bold();
                    });

                    page.Content().Element(content =>
                    {
                        content.Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Text($"Fecha: {fechaAprobacion:dd/MM/yyyy}");

                            column.Item().Text("Por medio del presente documento se certifica que:");
                            column.Item().Text(text =>
                            {
                                text.Span($"El/La docente {nombreDocente} con cédula {cedula} ha sido ");
                                text.Span($"promovido/a de la categoría {categoriaAnterior} a la categoría {categoriaNueva}, ");
                                text.Span($"cumpliendo con todos los requisitos establecidos para dicha promoción.");
                            });

                            column.Item().Text($"La promoción ha sido aprobada con fecha {fechaAprobacion:dd/MM/yyyy}.");

                            column.Item().Text(text =>
                            {
                                text.Span("Este documento ha sido generado automáticamente por el ");
                                text.Span("Sistema de Promoción Docente.");
                            });

                            column.Item().Text($"Revisor: {usuario}");
                        });
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Página ").FontSize(10);
                        x.CurrentPageNumber().FontSize(10);
                        x.Span(" de ").FontSize(10);
                        x.TotalPages().FontSize(10);
                    });
                });
            });

            // Generar y devolver PDF como array de bytes
            return document.GeneratePdf();
        }
    }
}

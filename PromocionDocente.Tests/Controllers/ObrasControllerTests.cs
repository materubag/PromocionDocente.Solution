using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.API.Controllers;
using PromocionDocente.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace PromocionDocente.Tests.Controllers
{
    [TestClass]
    public class ObrasControllerTests
    {
        private PromocionDocenteContext _context;
        private ObrasController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PromocionDocenteContext>()
                .UseInMemoryDatabase("TestDb_Obras")
                .Options;

            _context = new PromocionDocenteContext(options);
            _controller = new ObrasController(_context, null); // Servicio puede ser null si no se usa

            // Datos de prueba
            _context.Obras.Add(new Obra
            {
                IdObra = 1,
                CedDoc = "1234567890",
                Titulo = "Investigación sobre IA",
                TipoObra = "Libro",
                FechaPublicacion = DateOnly.FromDateTime(DateTime.Today),
                Estado = "PENDIENTE"
            });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task GetObras_ReturnsList()
        {
            var result = await _controller.GetObras();
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, result.Value.Count);
        }

        [TestMethod]
        public async Task GetObraPorId_ReturnsObra_WhenExists()
        {
            var result = await _controller.GetObraPorId(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("Investigación sobre IA", result.Value.Titulo);
        }

        [TestMethod]
        public async Task GetObraPorId_ReturnsNotFound_WhenMissing()
        {
            var result = await _controller.GetObraPorId(999);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteObra_RemovesObra_WhenExists()
        {
            var response = await _controller.DeleteObra(1);
            Assert.IsInstanceOfType(response, typeof(NoContentResult));

            var obraEliminada = await _context.Obras.FindAsync(1);
            Assert.IsNull(obraEliminada);
        }
    }
}

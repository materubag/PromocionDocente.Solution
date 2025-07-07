using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.API.Controllers;
using PromocionDocente.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromocionDocente.Tests.Controllers
{
    [TestClass]
    public class DocentesControllerTests
    {
        private PromocionDocenteContext _context;
        private DocentesController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PromocionDocenteContext>()
                .UseInMemoryDatabase("TestDb_Docentes")
                .Options;

            _context = new PromocionDocenteContext(options);
            _controller = new DocentesController(_context);

            // Datos de prueba
            _context.Docentes.Add(new Docente
            {
                CedDoc = "1234567890",
                Nom1Doc = "Juan",
                Ape1Doc = "Pérez"
            });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task GetDocentes_ReturnsList()
        {
            var result = await _controller.GetDocentes();
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, result.Value.Count);
        }

        [TestMethod]
        public async Task GetDocente_ReturnsOk_WhenExists()
        {
            var result = await _controller.GetDocente("1234567890");
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("Juan", result.Value.Nom1Doc);
        }

        [TestMethod]
        public async Task GetDocente_ReturnsNotFound_WhenMissing()
        {
            var result = await _controller.GetDocente("0000000000");
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
    }
}

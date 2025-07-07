using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.API.Controllers;
using PromocionDocente.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace PromocionDocente.Tests.Controllers
{
    [TestClass]
    public class CursosCapacitacionsControllerTests
    {
        private PromocionDocenteContext _context;
        private CursosCapacitacionsController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PromocionDocenteContext>()
                .UseInMemoryDatabase("TestDb_Cursos")
                .Options;

            _context = new PromocionDocenteContext(options);
            _controller = new CursosCapacitacionsController(_context, null); // El servicio puede ser null para pruebas básicas

            // Agregar curso de prueba
            _context.CursosCapacitacions.Add(new CursosCapacitacion
            {
                IdCurso = 1,
                CedDoc = "1234567890",
                NombreCurso = "Docker Básico",
                FechaCurso = DateOnly.FromDateTime(DateTime.Today),
                Horas = 20,
                Estado = "PENDIENTE"
            });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task GetCursosCapacitacions_ReturnsAll()
        {
            var result = await _controller.GetCursosCapacitacions();
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(1, result.Value.Count);
        }

        [TestMethod]
        public async Task GetCursoById_ReturnsCorrectCourse()
        {
            var result = await _controller.GetCursosCapacitacion(1);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("Docker Básico", result.Value.NombreCurso);
        }

        [TestMethod]
        public async Task GetCursoById_ReturnsNotFound_WhenMissing()
        {
            var result = await _controller.GetCursosCapacitacion(99);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteCurso_RemovesSuccessfully()
        {
            var result = await _controller.DeleteCursosCapacitacion(1);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            var curso = await _context.CursosCapacitacions.FindAsync(1);
            Assert.IsNull(curso);
        }
    }
}

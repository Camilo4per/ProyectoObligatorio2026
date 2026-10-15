using Dominio.Excepciones;
using DTOs.DTOs;
using LogicaAplicacion.InterfacesDeCasoDeUso.Capitulos;
using LogicaAplicacion.InterfacesDeCasoDeUso.Historias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Presentacion.Controllers
{
    public class CapituloController : Controller
    {
        private IAgregarCapitulo _agregarCapituloCU;
        private IObtenerHistoriaPorId _obtenerHistoriaPorIdCU;

        public CapituloController(IAgregarCapitulo agregarCapituloCU, IObtenerHistoriaPorId obtenerHistoriaPorIdCU)
        {
            _agregarCapituloCU = agregarCapituloCU;
            _obtenerHistoriaPorIdCU = obtenerHistoriaPorIdCU;
        }

        // GET: Capitulo/Create?idHistoria=5
        public ActionResult Create(int idHistoria)
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            if (idHistoria <= 0)
            {
                return NotFound();
            }

            HistoriaDTO? historia = _obtenerHistoriaPorIdCU.Ejecutar(idHistoria);
            if (historia == null)
            {
                return NotFound();
            }

            CargarDatosVista(historia, null);

            CapituloDTO nuevoCapitulo = new CapituloDTO
            {
                idHistoria = idHistoria,
                Tipo = 0
            };

            return View(nuevoCapitulo);
        }

        // POST: Capitulo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CapituloDTO capitulo)
        {
            string? rolUsuario = HttpContext.Session.GetString("Rol");
            if (rolUsuario != "Administrador")
            {
                return RedirectToAction("Login", "Usuario");
            }

            try
            {
                _agregarCapituloCU.Ejecutar(capitulo, capitulo.idCapituloAntecedente ?? 0, capitulo.TextoOpcion ?? "");
                return RedirectToAction("Index", "Historia");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                if (capitulo.idHistoria.HasValue)
                {
                    HistoriaDTO? historia = _obtenerHistoriaPorIdCU.Ejecutar(capitulo.idHistoria.Value);
                    if (historia != null)
                    {
                        CargarDatosVista(historia, capitulo.idCapituloAntecedente);
                    }
                }

                return View(capitulo);
            }
        }

        private void CargarDatosVista(HistoriaDTO historia, int? idAntecedenteSeleccionado)
        {
            ViewBag.TituloHistoria = historia.Titulo;

            List<CapituloDTO> capitulosIntermedios = new List<CapituloDTO>();
            if (historia.Capitulos != null)
            {
                foreach (CapituloDTO cap in historia.Capitulos)
                {
                    if (cap.Tipo == 0)
                    {
                        capitulosIntermedios.Add(cap);
                    }
                }
            }

            ViewBag.CapitulosAntecedentes = new SelectList(capitulosIntermedios, "Id", "Titulo", idAntecedenteSeleccionado);
        }
    }
}

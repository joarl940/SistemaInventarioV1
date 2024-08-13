using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventarioV1.Areas.Admin.Controllers
{
    [Area("Admin")]// se debe piner el atributo de area para poder visualizar 
    [Authorize(Roles =DS.Role_Admin)]  //obligar al loggeo
    public class BodegaController : Controller
    {
        //referenciar la unidad de trabajo que ya es un servicio
        private readonly IUnidadTrabajo _unidadTrabajo;

        //ctor + tab + tab para crear el controlador
        public BodegaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert (int? id)
        {
            Bodega bodega = new Bodega();
            if (id == null)
            {
                //crear una nueva bodega
                bodega.Estado = true;
                return View(bodega);
            }
            //actualizamos bodega
            bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());
            if (bodega == null)
            {
                return NotFound();
            }
            return View(bodega); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//sirve para la falsificacion de solicitudes
        public async Task<IActionResult>Upsert (Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if (bodega.Id == 0)
                {
                    await _unidadTrabajo.Bodega.Agregar(bodega);
                    TempData[DS.Exitosa] = "Bodega Creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Bodega.Actualizar(bodega);
                    TempData[DS.Exitosa] = "Bodega Actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardad();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al crear bodega";
            return View(bodega);
        }

        //crear region para un metodo API que se va trabajar dese un java script 
        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new { data = todos });// se va a referenciar el nombre data desde el jav script

        }
        [HttpPost]
        public async Task<IActionResult>Delete(int id)
        {
            var bodegaDB = await _unidadTrabajo.Bodega.Obtener(id);
            if (bodegaDB == null)
            {
                return Json(new { success = false, Message = "Error al borrar Bodega" });
            }
            _unidadTrabajo.Bodega.Remover(bodegaDB);
            await _unidadTrabajo.Guardad();
            return Json(new { success = true, Message = "Bodega borrada exitosamente" });
        }
        [ActionName ("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Bodega.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim()&& b.Id!=id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }
        #endregion
    }
}


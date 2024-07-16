using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventarioV1.Areas.Admin.Controllers
{
    [Area("Admin")]// se debe piner el atributo de area para poder visualizar 
    public class MarcaController : Controller
    {
        //referenciar la unidad de trabajo que ya es un servicio
        private readonly IUnidadTrabajo _unidadTrabajo;

        //ctor + tab + tab para crear el controlador
        public MarcaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert (int? id)
        {
            Marca marca = new Marca();
            if (id == null)
            {
                //crear una nueva marca
                marca.Estado = true;
                return View(marca);
            }
            //actualizamos marca
            marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//sirve para la falsificacion de solicitudes
        public async Task<IActionResult>Upsert (Marca marca)
        {
            if (ModelState.IsValid)
            {
                if (marca.Id == 0)
                {
                    await _unidadTrabajo.Marca.Agregar(marca);
                    TempData[DS.Exitosa] = "Marca Creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Marca.Actualizar(marca);
                    TempData[DS.Exitosa] = "Marca Actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardad();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al crear Marca";
            return View(marca);
        }

        //crear region para un metodo API que se va trabajar dese un java script 
        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Marca.ObtenerTodos();
            return Json(new { data = todos });// se va a referenciar el nombre data desde el jav script

        }
        [HttpPost]
        public async Task<IActionResult>Delete(int id)
        {
            var marcaDB = await _unidadTrabajo.Marca.Obtener(id);
            if (marcaDB == null)
            {
                return Json(new { success = false, Message = "Error al borrar Marca" });
            }
            _unidadTrabajo.Marca.Remover(marcaDB);
            await _unidadTrabajo.Guardad();
            return Json(new { success = true, Message = "Marca borrada exitosamente" });
        }
        [ActionName ("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Marca.ObtenerTodos();
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


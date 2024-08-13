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
    [Authorize(Roles = DS.Role_Admin)]  //obligar al loggeo
    public class CategoriaController : Controller
    {
        //referenciar la unidad de trabajo que ya es un servicio
        private readonly IUnidadTrabajo _unidadTrabajo;

        //ctor + tab + tab para crear el controlador
        public CategoriaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert (int? id)
        {
            Categoria categoria = new Categoria();
            if (id == null)
            {
                //crear una nueva categoria
                categoria.Estado = true;
                return View(categoria);
            }
            //actualizamos categoria
            categoria = await _unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//sirve para la falsificacion de solicitudes
        public async Task<IActionResult>Upsert (Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                if (categoria.Id == 0)
                {
                    await _unidadTrabajo.Categoria.Agregar(categoria);
                    TempData[DS.Exitosa] = "Categoria Creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Categoria.Actualizar(categoria);
                    TempData[DS.Exitosa] = "Categoria Actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardad();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al crear Categoria";
            return View(categoria);
        }

        //crear region para un metodo API que se va trabajar dese un java script 
        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Categoria.ObtenerTodos();
            return Json(new { data = todos });// se va a referenciar el nombre data desde el jav script

        }
        [HttpPost]
        public async Task<IActionResult>Delete(int id)
        {
            var categoriaDB = await _unidadTrabajo.Categoria.Obtener(id);
            if (categoriaDB == null)
            {
                return Json(new { success = false, Message = "Error al borrar Categoria" });
            }
            _unidadTrabajo.Categoria.Remover(categoriaDB);
            await _unidadTrabajo.Guardad();
            return Json(new { success = true, Message = "Categoria borrada exitosamente" });
        }
        [ActionName ("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Categoria.ObtenerTodos();
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


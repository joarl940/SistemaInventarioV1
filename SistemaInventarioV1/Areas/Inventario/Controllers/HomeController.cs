using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.Especificaciones;
using SistemaInventario.Modelos.ViewModels;
using System.Diagnostics;

namespace SistemaInventarioV1.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidad_trabajo;

        public HomeController(ILogger<HomeController> logger,IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidad_trabajo = unidadTrabajo;
        }
  
        public IActionResult Index(int pageNumber=1, string busqueda="", string busquedaActual="")
        {
            if (!String.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
            }
            else
            {
                busqueda = busquedaActual;
            }
            ViewData["BusquedaActual"] = busqueda;
            if (pageNumber<1)
            {
                pageNumber = 1;
            }
            Parmetros parametros = new Parmetros()
            {
                PageNumber = pageNumber,
                PageSize = 2
            };
            var resultado = _unidad_trabajo.Producto.ObtenerTodosPaginado(parametros);
            if (!String.IsNullOrEmpty(busqueda))
            {
                resultado = _unidad_trabajo.Producto.ObtenerTodosPaginado(parametros,p=>p.Descripcion.Contains(busqueda));
            }
            ViewData["TotalPaginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["PageSize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disable";//clase css para desactivar el boton
            ViewData["Siguiente"] = "";

            if (pageNumber>1)
            {
                ViewData["Previo"] = "";
            }
            if (resultado.MetaData.TotalPages>=pageNumber)
            {
                ViewData["Siguiente"] = "disabled";
            }
            return View(resultado);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

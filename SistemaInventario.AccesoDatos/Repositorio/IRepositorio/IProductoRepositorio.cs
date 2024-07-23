using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        void Actualizar(Producto producto);// se maneja de forma individual porque cada objeto tiene sus propias porpiedades

        IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);

    }


}

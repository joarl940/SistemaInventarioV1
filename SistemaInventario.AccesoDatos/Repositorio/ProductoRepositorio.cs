using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventarioV1.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext _db;

            public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Producto producto)
        {
            //capturo el dato para procesarlo
            var ProductoBD = _db.Productos.FirstOrDefault(b => b.Id == producto.Id);
            if (ProductoBD != null)
            {
                if (producto.ImagenUrl!=null)
                {
                    ProductoBD.ImagenUrl = producto.ImagenUrl;
                }
                ProductoBD.NumeroSerie = producto.NumeroSerie;
                ProductoBD.Descripcion = producto.Descripcion;
                ProductoBD.Precio = producto.Precio;
                ProductoBD.Costo = producto.Costo;
                ProductoBD.CategoriaId = producto.CategoriaId;
                ProductoBD.MarcaId = producto.MarcaId;
                ProductoBD.PadreId = producto.PadreId;
                ProductoBD.Estado = producto.Estado;

                _db.SaveChanges();
            }
        }

        //Metodo encargado de llenar las vistas 
        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            if (obj=="Categoria")
            {
                return _db.Categorias.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text=c.Nombre,
                    Value=c.Id.ToString()
                });
            }
            if (obj == "Marca")
            {
                return _db.Marcas.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Producto")
            {
                return _db.Productos.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }
    }

}

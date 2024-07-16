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
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _db;

            public CategoriaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Categoria categoria)
        {
            //capturo el dato para procesarlo
            var CategoriaBD = _db.Categorias.FirstOrDefault(b => b.Id == categoria.Id);
            if (CategoriaBD != null)
            {
                CategoriaBD.Nombre = categoria.Nombre;
                CategoriaBD.Descripcion = categoria.Descripcion;
                CategoriaBD.Estado = categoria.Estado;
                _db.SaveChanges();
            }
        }
    }

}

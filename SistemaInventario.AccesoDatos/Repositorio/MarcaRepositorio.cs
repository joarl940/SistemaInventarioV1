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
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext _db;

            public MarcaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Marca marca)
        {
            //capturo el dato para procesarlo
            var MarcaBD = _db.Marcas.FirstOrDefault(b => b.Id == marca.Id);
            if (MarcaBD != null)
            {
                MarcaBD.Nombre = marca.Nombre;
                MarcaBD.Descripcion = marca.Descripcion;
                MarcaBD.Estado = marca.Estado;
                _db.SaveChanges();
            }
        }
    }

}

using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio : IRepositorio<Categoria>
    {
        void Actualizar(Categoria categoria);// se maneja de forma individual porque cada objeto tiene sus propias porpiedades
    }
}

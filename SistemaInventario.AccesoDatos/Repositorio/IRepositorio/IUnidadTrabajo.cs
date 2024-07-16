using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable // liberar objetos que no se esten usando o esten consumiendo recursos
    {
        IBodegaRepositorio Bodega { get; }
        ICategoriaRepositorio Categoria { get; }
        IMarcaRepositorio Marca { get; }


        Task Guardad();
    }
}

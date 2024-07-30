using SistemaInventario.Modelos.Especificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IRepositorio<T>where T : class // se hace la clase generica para poder trabajar con varios objetos
    {
        //task para ponerlos ascincronos
        Task <T> Obtener(int id);

        Task <IEnumerable<T>> ObtenerTodos(
            Expression<Func<T,bool>> filtro =null,
            Func<IQueryable<T>,IOrderedQueryable<T>> orderBy = null,
            string incluirPropiedades=null,
            bool isTraking=true
            );
        PagedList<T> ObtenerTodosPaginado(Parmetros parametros,
            Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string incluirPropiedades = null,
            bool isTraking = true);


        Task<T> ObtenerPrimero(
            Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null,
            bool isTraking = true
            );

        Task Agregar(T entidad);

        //no se puede poner ascincrono por se metodos de eliminacion
        void Remover(T entidad);

        void RemoverRango(IEnumerable<T> entidad);
    }
}

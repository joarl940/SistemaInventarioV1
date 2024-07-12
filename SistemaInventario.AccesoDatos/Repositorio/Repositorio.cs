using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV1.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        //constructor
        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet=_db.Set<T>();
        }
           
        public async Task Agregar(T entidad) // se pone el async porque se definio el metodo task
        {
            await dbSet.AddAsync(entidad); // equivale a un insert into table
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id); // equivale a un select * from (solo por id)
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTraking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro); // es igual a un select * from where ..... condiciones
            }
            if (incluirPropiedades != null)
            {
                //recorrer los filtros que vienen convirtiendolos a tipo char
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); // ejemplo "categoria, marca"
                }
            }
            if (!isTraking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTraking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query=query.Where(filtro); // es igual a un select * from where ..... condiciones
            }
            if (incluirPropiedades != null)
            {
                //recorrer los filtros que vienen convirtiendolos a tipo char
                foreach(var incluirProp in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); // ejemplo "categoria, marca"
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!isTraking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }

        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);
        }
    }
}

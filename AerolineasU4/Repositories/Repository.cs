using Microsoft.EntityFrameworkCore;

namespace AerolineasU4.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
        }

        public DbSet<T> Get()
        {
            return context.Set<T>();
        }

        public T? Get(object index)
        {
            return context.Find<T>(index);
        }

        public void Insert(T entidad)
        {
            context.Add(entidad);
            context.SaveChanges();
        }

        public void Update(T entidad)
        {
            context.Update(entidad);
            context.SaveChanges();
        }

        public void Delete(T entidad)
        {
            context.Remove(entidad);
            context.SaveChanges();
        }
    }
}

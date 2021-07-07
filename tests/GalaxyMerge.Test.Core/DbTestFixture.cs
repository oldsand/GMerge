using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Test.Core
{
    public abstract class DbTestFixture<T> where T : DbContext
    {
        protected DbTestFixture(DbContextOptions<T> contextOptions)
        {
            ContextOptions = contextOptions;
        }
        protected DbContextOptions<T> ContextOptions { get; }

        protected virtual void Seed()
        {
        }
    }
}
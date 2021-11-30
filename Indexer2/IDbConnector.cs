using Microsoft.EntityFrameworkCore;

namespace Indexer
{
    public interface IDbConnector
    {
        [System.Runtime.Versioning.RequiresPreviewFeatures]
        public abstract static void InjectDbContext(DbContextOptionsBuilder opt);
    }
}

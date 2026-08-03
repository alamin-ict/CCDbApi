using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface INewsContentRepository : IGenericRepository<NewsContent>
    {

    }
    public class NewsContentRepository : GenericRepository<NewsContent>, INewsContentRepository
    {
        public NewsContentRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

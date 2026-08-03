using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
   
    public interface IPagePostRepository : IGenericRepository<PagePost>
    {

    }
    public class PagePostRepository : GenericRepository<PagePost>, IPagePostRepository
    {
        public PagePostRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

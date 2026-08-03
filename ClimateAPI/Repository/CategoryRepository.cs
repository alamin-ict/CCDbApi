using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {

    }
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ClimateDbContext context) : base(context)
        {

        }
    }
   
}

using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
  
    public interface IPostCategoryMappingRepository : IGenericRepository<PostCategoryMapping>
    {

    }
    public class PostCategoryMappingRepository : GenericRepository<PostCategoryMapping>, IPostCategoryMappingRepository
    {
        public PostCategoryMappingRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

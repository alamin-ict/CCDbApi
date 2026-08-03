using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface IPostTagsMappingRepository : IGenericRepository<PostTagsMapping>
    {

    }
    public class PostTagsMappingRepository : GenericRepository<PostTagsMapping>, IPostTagsMappingRepository
    {
        public PostTagsMappingRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface ITagsRepository : IGenericRepository<Tags>
    {

    }
    public class TagsRepository : GenericRepository<Tags>, ITagsRepository
    {
        public TagsRepository(ClimateDbContext context) : base(context)
        {

        }
    }
   
}

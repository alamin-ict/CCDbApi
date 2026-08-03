using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
   
    public interface IMediaRepository : IGenericRepository<Media>
    {

    }
    public class MediaRepository : GenericRepository<Media>, IMediaRepository
    {
        public MediaRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

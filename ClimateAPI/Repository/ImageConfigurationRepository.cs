using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
   
    public interface IImageConfigurationRepository : IGenericRepository<ImageConfiguration>
    {

    }
    public class ImageConfigurationRepository : GenericRepository<ImageConfiguration>, IImageConfigurationRepository
    {
        public ImageConfigurationRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

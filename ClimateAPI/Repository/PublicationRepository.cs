using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface IPublicationRepository : IGenericRepository<Publication>
    {

    }
    public class PublicationRepository : GenericRepository<Publication>, IPublicationRepository
    {
        public PublicationRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

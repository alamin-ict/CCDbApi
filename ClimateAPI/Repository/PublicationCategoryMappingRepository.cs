using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface IPublicationCategoryMappingRepository : IGenericRepository<PublicationCategoryMapping>
    {

    }
    public class PublicationCategoryMappingRepository : GenericRepository<PublicationCategoryMapping>, IPublicationCategoryMappingRepository
    {
        public PublicationCategoryMappingRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

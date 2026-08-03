using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{

    public interface ISocialContactRepository : IGenericRepository<SocialContact>
    {

    }
    public class SocialContactRepository : GenericRepository<SocialContact>, ISocialContactRepository
    {
        public SocialContactRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

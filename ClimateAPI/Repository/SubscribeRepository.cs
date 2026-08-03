using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{

    public interface ISubscribeRepository : IGenericRepository<Subscribe>
    {

    }
    public class SubscribeRepository : GenericRepository<Subscribe>, ISubscribeRepository
    {
        public SubscribeRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

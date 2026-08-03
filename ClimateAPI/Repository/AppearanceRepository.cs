using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{

    public interface IAppearanceRepository : IGenericRepository<Appearance>
    {

    }
    public class AppearanceRepository : GenericRepository<Appearance>, IAppearanceRepository
    {
        public AppearanceRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

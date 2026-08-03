using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
   
    public interface IGeneralSettingsRepository : IGenericRepository<GeneralSettings>
    {

    }
    public class GeneralSettingsRepository : GenericRepository<GeneralSettings>, IGeneralSettingsRepository
    {
        public GeneralSettingsRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{

    public interface ITrainingInfoRepository : IGenericRepository<TrainingInfo>
    {

    }
    public class TrainingInfoRepository : GenericRepository<TrainingInfo>, ITrainingInfoRepository
    {
        public TrainingInfoRepository(ClimateDbContext context) : base(context)
        {

        }
    }

}

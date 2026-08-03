using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
 
    public interface ITraineeInfoRepository : IGenericRepository<TraineeInfo>
    {

    }
    public class TraineeInfoRepository : GenericRepository<TraineeInfo>, ITraineeInfoRepository
    {
        public TraineeInfoRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

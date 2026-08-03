using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
  
    
    public interface IUserRepository : IGenericRepository<User>
    {

    }
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

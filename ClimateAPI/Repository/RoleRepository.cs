using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    

        public interface IRoleRepository : IGenericRepository<Role>
        {

        }
        public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
            public RoleRepository(ClimateDbContext context) : base(context)
            {

            }
        }
    }

using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
   
        public interface IPartnerRepository : IGenericRepository<Partner>
        {

        }
        public class PartnerRepository : GenericRepository<Partner>, IPartnerRepository
    {
            public PartnerRepository(ClimateDbContext context) : base(context)
            {

            }
        }
    }

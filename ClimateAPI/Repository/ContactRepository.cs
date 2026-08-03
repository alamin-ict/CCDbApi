using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface IContactRepository : IGenericRepository<Contact>
    {

    }
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}


using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {

    }
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {

    }
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

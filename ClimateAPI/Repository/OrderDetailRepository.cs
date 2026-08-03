using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {

    }
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

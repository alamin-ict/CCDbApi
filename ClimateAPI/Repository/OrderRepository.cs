using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
   
    public interface IOrderRepository : IGenericRepository<Order>
    {

    }
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ClimateDbContext context) : base(context)
        {

        }
    }


    
   
   


   
}

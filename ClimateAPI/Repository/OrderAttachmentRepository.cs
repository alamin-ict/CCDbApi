using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    public interface IOrderAttachmentRepository : IGenericRepository<OrderAttachment>
    {

    }
    public class OrderAttachmentRepository : GenericRepository<OrderAttachment>, IOrderAttachmentRepository
    {
        public OrderAttachmentRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
  
    public interface ICommentRepository : IGenericRepository<Comment>
    {

    }
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ClimateDbContext context) : base(context)
        {

        }
    }
}

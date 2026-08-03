using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{


        public interface ISliderImageRepository : IGenericRepository<SliderImage>
        {

        }
        public class SliderImageRepository : GenericRepository<SliderImage>, ISliderImageRepository
        {
            public SliderImageRepository(ClimateDbContext context) : base(context)
            {

            }
        }
    }

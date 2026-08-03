using CCDbApi.Model;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi.Repository
{
    
    public interface ISliderRepository : IGenericRepository<Slider>
    {

    }
    public class SliderRepository : GenericRepository<Slider>, ISliderRepository
    {
        public SliderRepository(ClimateDbContext context) : base(context)
        {

        }
    }

}

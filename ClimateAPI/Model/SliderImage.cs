
namespace CCDbApi.Model
{
    public class SliderImage : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string SliderType { get; set; } //Mainslider, SecondRowSlider/Highlights, ProductImage
        public string SliderMainText { get; set; }//Climate Change Development Bangladesh
        public string? SliderDetailText { get; set; }//Empowering Bangladesh Against Climate Change
        public int SliderOrder { get; set; }
        public string DescriptionLink { get; set; }
        
        public string UserId {  get; set; } 
    }
}

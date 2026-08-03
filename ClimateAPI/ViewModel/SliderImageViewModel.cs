namespace CCDbApi.ViewModel
{
    public class SliderImageViewModel
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string SliderType { get; set; } //Mainslider, SecondRowSlider, ProductImage
        public string SliderMainText { get; set; }
        public string SliderDetailText { get; set; }
        public string DescriptionLink { get; set; }
        public int SliderOrder { get; set; }
    }
}

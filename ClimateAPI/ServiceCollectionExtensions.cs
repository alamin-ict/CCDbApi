using CCDbApi.Repository;
using CCDbApi.Service;
using IYogaBackendMicroservice.Repository;

namespace CCDbApi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register your application services here
            // Register the generic repository and the specific repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Register specific repositories
            services.AddScoped<IUserRepository, UserRepository>(); 
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<INewsContentRepository, NewsContentRepository>();  
            services.AddScoped<IPartnerRepository, PartnerRepository>();    
            services.AddScoped<IImageConfigurationRepository, ImageConfigurationRepository>();  
            services.AddScoped<ISliderImageRepository, SliderImageRepository>();    
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<ISubscribeRepository,SubscribeRepository>();



            services.AddScoped<ITagsRepository, TagsRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPublicationRepository, PublicationRepository>();
            services.AddScoped<IPagePostRepository, PagePostRepository>();
            services.AddScoped<IImageConfigurationRepository, ImageConfigurationRepository>();
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<ISocialContactRepository, SocialContactRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();


            services.AddScoped<IPostCategoryMappingRepository, PostCategoryMappingRepository>();
            services.AddScoped<IPostTagsMappingRepository, PostTagsMappingRepository>();
            services.AddScoped<IPublicationCategoryMappingRepository, PublicationCategoryMappingRepository>();
            services.AddScoped<IGeneralSettingsRepository, GeneralSettingsRepository>();
            services.AddScoped<IAppearanceRepository, AppearanceRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IOrderAttachmentRepository, OrderAttachmentRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();


            //Register the specific services
            services.AddScoped<IClimateService, ClimateService>();
            services.AddScoped<ITrainingInfoRepository, TrainingInfoRepository>();
            services.AddScoped<ITraineeInfoRepository, TraineeInfoRepository>();
            services.AddTransient<EmailService>();
            services.AddScoped<ICcdvService,CcdvService>();  
            services.AddScoped<ISettingsService, SettingsService>();   
            services.AddScoped<IOrderService, OrdersService>();

            return services;
        }
    }
}



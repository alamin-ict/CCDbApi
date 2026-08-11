
using CCDbApi.Model;
using Microsoft.EntityFrameworkCore;
namespace IYogaBackendMicroservice.Repository
{
    public class ClimateDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ClimateDbContext(DbContextOptions<ClimateDbContext> options, IConfiguration configuration)
       : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<TraineeInfo> TraineeInfos { get; set; }

        public DbSet<TrainingInfo> TrainingInfos { get; set; }
        public DbSet<Subscribe> Subscribe { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<PagePost> PagePosts { get; set; }
        public DbSet<PostCategoryMapping> PostCategoriesMappings { get; set; }
        public DbSet<PublicationCategoryMapping> PublicationCategoryMappings { get; set; }
        public DbSet<PostTagsMapping> PostTagsMappings { get; set; }
        public DbSet<Appearance> Appearance { get; set; }
        public DbSet<GeneralSettings> GeneralSettings { get; set; }   
        public DbSet<SocialContact> SocialContacts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderAttachment> OrderAttachments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ImageConfiguration> ImageConfigurations { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<NewsContent> NewsContents { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ensure OnConfiguring is not used if options are passed in constructor
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }
}

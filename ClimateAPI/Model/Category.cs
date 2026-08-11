using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CCDbApi.Model
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Description { get; set; }
        public string? ParentId { get; set; }
        public string UserId { get; set; }

    }
    public class Tags : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string UserId { get; set; }
    }
    public class Publication : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public DateTime Date { get; set; }
        public string? FullContent
        {
            get; set;
        }
        public Status Status { get; set; }
        public string Author { get; set; }
        public string? AuthorId { get; set; } = string.Empty;
        public string CoverImage { get; set; } = string.Empty;
        public string DownloadUrl { get; set; }
        public decimal Price { get; set; }
        public int PageSize { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public string PublisherId { get; set; } = string.Empty;
        public int Year { get; set; }
        public string UserId { get; set; }
    }
    public class PostCategoryMapping : BaseEntity
    {
        public string PostId { get; set; }
        public string CategoryId { get; set; }
    }
    public class PublicationCategoryMapping : BaseEntity
    {
        public string PublicationId { get; set; }
        public string CategoryId { get; set; }
    }
    public class PostTagsMapping : BaseEntity
    {
        public string PostId { get; set; }
        public string TagsId { get; set; }
    }
    public class PagePost : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Permalink { get; set; }
        public string FullContent
        {
            get; set;
        }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public string Author { get; set; }
        public string? AuthorId { get; set; } = string.Empty;
        public string CoverImage { get; set; } = string.Empty;
        public bool? IsSellInStore { get; set; }
        public decimal? Price { get; set; }
        public int? Year { get; set; }
        public string? DownloadUrl { get; set; }
        public int? PageSize { get; set; }
        public string? Publisher { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }  // post, page

    }
    public class GeneralSettings : BaseEntity
    {
        public string SiteTitle { get; set; }
        public string SiteUrl { get; set; }
        public string Tagline { get; set; }
        public int PostsPerPage { get; set; }
        public string AdminEmail { get; set; }
        public string UserId { get; set; }
    }
    public class Appearance : BaseEntity
    {
        public string DonateUrl { get; set; }
        public string DonateButtonLabel { get; set; }
        public string FooterTagline { get; set; }
        public bool IsShowGalleryOnHomePage { get; set; }
        public int HeroOverleyOpacity { get; set; }
        public string UserId { get; set; }
    }
    public class Slider : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Heading { get; set; }
        public string Subheading { get; set; }
        public string CTALabel { get; set; }
        public string CTAUrl { get; set; }
        public string UserId { get; set; }
    }
    public class SocialContact : BaseEntity
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string? GithubUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string InstragramUrl { get; set; }
        public string UserId { get; set; }

    }
    public class Media : BaseEntity
    {
        public string MediaUrl { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Size { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
    }
    public class Comment : BaseEntity
    {
        public string Name { get; set; }
        public CommentStatus Status { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }
    public class Order : BaseEntity
    {
        public string OrderNo { get; set; } = string.Empty;

        public string UserId { get; set; }

        public string CustomerId { get; set; }

        public string? Title { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public string PropertyAddress { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
    public class OrderDetail : BaseEntity
    {
        public string PublicationId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public string OrderId { get; set; }
    }
    public class OrderAttachment : BaseEntity
    {
        public string OrderId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;
        public string DownloadUrl { get; set; }
    }
    public class Payment : BaseEntity
    {
        public string OrderId { get; set; }

        public string PaymentNo { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string? TransactionId { get; set; }

        public DateTime PaymentDate { get; set; }

        public string? Remarks { get; set; }
        public string UserId { get; set; }
        //public virtual Order Order { get; set; }
    }

    public class Invoice : BaseEntity
    {
        public string OrderId { get; set; }

        public string InvoiceNo { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal Tax { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime? DueDate { get; set; }
        public string UserId { get; set; }
        //public virtual Order Order { get; set; }
    }


}


using CCDbApi.Model;

namespace CCDbApi.ViewModel
{

    // ================= Publication DTOs =================

    public class CreatePublicationDto
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public DateTime? Date { get; set; }
        public string? FullContent { get; set; }
        public Status Status { get; set; }
        public string Author { get; set; }
        public string? AuthorId { get; set; }
        public string CoverImage { get; set; }
        public string DownloadUrl { get; set; }
        public decimal Price { get; set; }
        public int PageSize { get; set; }
        public string Publisher { get; set; }
        public string PublisherId { get; set; }
        public int Year { get; set; }
        public List<string>? CategoryIds { get; set; }
    }



    public class PublicationResponseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public DateTime Date { get; set; }
        public string? FullContent { get; set; }
        public Status Status { get; set; }
        public string Author { get; set; }
        public string CoverImage { get; set; }
        public string DownloadUrl { get; set; }
        public decimal Price { get; set; }
        public int PageSize { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }

        public List<CategoryDto>? Categories { get; set; }
    }


    // ================= Category Mapping DTOs =================

    public class CreatePublicationCategoryMappingDto
    {
        public string PublicationId { get; set; }
        public string CategoryId { get; set; }
    }


    public class PublicationCategoryMappingResponseDto
    {
        public string Id { get; set; }
        public string PublicationId { get; set; }
        public string CategoryId { get; set; }
    }


    // ================= Post Category Mapping DTOs =================

    public class CreatePostCategoryMappingDto
    {
        public string PostId { get; set; }
        public string CategoryId { get; set; }
    }


    public class PostCategoryMappingResponseDto
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string CategoryId { get; set; }
    }


    // ================= Post Tags Mapping DTOs =================

    public class CreatePostTagsMappingDto
    {
        public string PostId { get; set; }
        public string TagsId { get; set; }
    }


    public class PostTagsMappingResponseDto
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string TagsId { get; set; }
    }


    // ================= Page Post DTOs =================

    public class CreatePagePostDto
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Permalink { get; set; }
        public string FullContent { get; set; }
        public Status Status { get; set; }
        public string Author { get; set; }
        public string? AuthorId { get; set; }
        public string Type { get; set; }
        public string CoverImage { get; set; }
        public bool? IsSellInStore { get; set; }
        public decimal? Price { get; set; }
        public int? Year { get; set; }
        public string? DownloadUrl { get; set; }
        public int? PageSize { get; set; }
        public string? Publisher { get; set; }
        public List<string>? CategoryIds { get; set; }
        public List<string>? TagsIds { get; set; }
    }
    public class CreatePageDto
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Permalink { get; set; }
        public string FullContent { get; set; }
        public Status Status { get; set; }
        public string Author { get; set; }
        public string? AuthorId { get; set; }
        public string Type { get; set; }
        public string CoverImage { get; set; }

    }


    public class PagePostResponseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Permalink { get; set; }
        public string FullContent { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public string Author { get; set; }
        public string CoverImage { get; set; }
        public bool? IsSellInStore { get; set; }
        public decimal? Price { get; set; }
        public int? Year { get; set; }
        public string? DownloadUrl { get; set; }
        public int? PageSize { get; set; }
        public string? Publisher { get; set; }
        public string Type { get; set; }  // post, page
        public List<CategoryDto>? Categories { get; set; }
        public List<TagsDto>? Tags { get; set; }
    }

    public class PageResponseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Permalink { get; set; }
        public string FullContent { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public string Author { get; set; }
        public string CoverImage { get; set; }

    }

    // ================= General Settings DTOs =================

    public class CreateGeneralSettingsDto
    {
        //public string? Id { get; set; }
        public string SiteTitle { get; set; }
        public string SiteUrl { get; set; }
        public string Tagline { get; set; }
        public int PostsPerPage { get; set; }
        public string AdminEmail { get; set; }
    }

    public class GeneralSettingsResponseDto
    {
        public string Id { get; set; }
        public string SiteTitle { get; set; }
        public string SiteUrl { get; set; }
        public string Tagline { get; set; }
        public int PostsPerPage { get; set; }
        public string AdminEmail { get; set; }
    }


    // ================= Appearance DTOs =================

    public class CreateAppearanceDto
    {
        //public string? Id { get; set; }
        public string DonateUrl { get; set; }
        public string DonateButtonLabel { get; set; }
        public string FooterTagline { get; set; }
        public bool IsShowGalleryOnHomePage { get; set; }
        public int HeroOverleyOpacity { get; set; }
    }

    public class AppearanceResponseDto
    {
        public string Id { get; set; }
        public string DonateUrl { get; set; }
        public string DonateButtonLabel { get; set; }
        public string FooterTagline { get; set; }
        public bool IsShowGalleryOnHomePage { get; set; }
        public int HeroOverleyOpacity { get; set; }
    }


    // ================= Slider DTOs =================

    public class CreateSliderDto
    {
        public string? Id { get; set; }
        public string ImageUrl { get; set; }
        public string Heading { get; set; }
        public string Subheading { get; set; }
        public string CTALabel { get; set; }
        public string CTAUrl { get; set; }
    }

    public class SliderResponseDto
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Heading { get; set; }
        public string Subheading { get; set; }
        public string CTALabel { get; set; }
        public string CTAUrl { get; set; }
    }


    // ================= Social Contact DTOs =================

    public class CreateSocialContactDto
    {
        public string? Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string? GithubUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string InstragramUrl { get; set; }
    }

    public class SocialContactResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string? GithubUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string InstragramUrl { get; set; }
    }


    // ================= Media DTOs =================

    public class CreateMediaDto
    {
        public string MediaUrl { get; set; }
    }

    public class MediaResponseDto
    {
        public string? Id { get; set; }
        public string MediaUrl { get; set; }
    }


    // ================= Comment DTOs =================

    public class CreateCommentDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PostId { get; set; }

        public CommentStatus? Status { get; set; }

        public string Description { get; set; }
    }

    public class CommentResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
    }
}

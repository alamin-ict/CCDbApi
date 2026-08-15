using CCDbApi.Model;
using CCDbApi.Repository;
using CCDbApi.ViewModel;
using System.Linq;

namespace CCDbApi.Service
{
    public interface ICcdvService
    {

        Task<Media> AddMediaAsync(Media tags);
        Task<Media> UpdateMediaAsync(Media tags);
        Task<Media> DeleteMediaAsync(Media tags);
        Task<Media> GetMediaAsync(string id);
        Task<List<Media>> GetAllMediaAsync();

        // Tags
        Task<Tags> AddTagsAsync(Tags tags);
        Task<Tags> UpdateTagsAsync(Tags tags);
        Task<Tags> DeleteTagsAsync(Tags tags);
        Task<Tags> GetTagsAsync(string id);
        Task<List<Tags>> GetAllTagsAsync();

        // Categories
        Task<Category> AddCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<Category> DeleteCategoryAsync(Category category);
        Task<Category> GetCategoryAsync(string id);
        Task<List<Category>> GetAllCategoriesAsync();
        // Publications
        Task<Publication> AddPublicationAsync(Publication publication);
        Task<Publication> UpdatePublicationAsync(Publication publication);
        Task<Publication> DeletePublicationAsync(Publication publication);
        Task<Publication> GetPublicationAsync(string id);
        Task<List<PublicationResponseDto>> GetAllPublicationsAsync();
        Task<List<PublicationCategoryMapping>> AddPublicationCategoryMappingAsync(List<PublicationCategoryMapping> publicationCategoryMapping);
        Task<List<CategoryDto?>> GetPublicationCategoryMappingsAsync(string publicationId);


        // Page Post
        Task<PagePost> AddPagePostAsync(PagePost pagePost);
        Task<PagePost> UpdatePagePostAsync(PagePost pagePost);
        Task<PagePost> DeletePagePostAsync(PagePost pagePost);
        Task<PagePost> GetPagePostAsync(string id);
        Task<List<PagePostResponseDto>> GetAllPagePostsAsync();
        Task<bool> AddTagAndCategoryMappingWithPagePostAsync(PagePost post, List<string>? catIds,
            List<string>? TagIds);
        Task<List<CategoryDto>> GetAllCategoriesByPostIdAsync(string postId);
        Task<List<TagsDto>> GetAllTagsByPostIdAsync(string postId);
        //comment
        Task<Comment> AddCommentAsync(Comment pagePost);
        Task<Comment> UpdateCommentAsync(Comment pagePost);
        Task<Comment> DeleteCommentAsync(Comment pagePost);
        Task<Comment> GetCommentAsync(string id);
        Task<List<Comment>> GetAllCommentsAsync();
        Task<List<Comment>> GetAllPublicCommentsAsync();
        Task<List<Comment>> GetAllUserCommentsAsync(string userId);

    }
    public class CcdvService : ICcdvService
    {
        private readonly ITagsRepository _tagsRepo;
        private readonly IMediaRepository _mediaRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IPublicationRepository _publicationRepo;
        private readonly IPagePostRepository _pagePostRepo;
        private readonly IPostCategoryMappingRepository _postMappingRepo;
        private readonly IPostTagsMappingRepository _postTagsMappingRepo;
        private readonly IPublicationCategoryMappingRepository _publicationCategoryMappingRepo;
        private readonly ICommentRepository _commentRepo;
        public CcdvService(ICommentRepository commentRepository,
            ITagsRepository tagsRepo, IPostCategoryMappingRepository pageTagsMappingRepo,
            IPostTagsMappingRepository tagsMappingRepository, IMediaRepository media,
            ICategoryRepository categoryRepo, IPublicationRepository publicationRepo,
            IPagePostRepository pagePostRepo, IPublicationCategoryMappingRepository publicationCategoryMappingRepo)
        {
            _postMappingRepo = pageTagsMappingRepo;
            _mediaRepo = media;
            _commentRepo = commentRepository;
            _postTagsMappingRepo = tagsMappingRepository;
            _tagsRepo = tagsRepo;
            _categoryRepo = categoryRepo;
            _publicationRepo = publicationRepo;
            _pagePostRepo = pagePostRepo;
            _publicationCategoryMappingRepo = publicationCategoryMappingRepo;
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            var added = await _categoryRepo.AddAsync(category);
            if (added == 1)
            {
                return category;
            }
            return null;
        }

        public async Task<Tags> AddTagsAsync(Tags tags)
        {
            var added = await _tagsRepo.AddAsync(tags);
            if (added == 1)
            {
                return tags;
            }
            return null;
        }

        public async Task<Category> DeleteCategoryAsync(Category category)
        {
            var deleted = await _categoryRepo.RemoveAsync(category);
            if (deleted == 1)
            {
                return category;
            }
            return null;
        }

        public async Task<Tags> DeleteTagsAsync(Tags tags)
        {
            var deleted = await _tagsRepo.RemoveAsync(tags);
            if (deleted == 1)
            {
                return tags;
            }
            return null;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {

            var data = await _categoryRepo.GetAllAsync();
            if (data == null)
            {
                return null;
            }
            return data.ToList();
        }

        public async Task<List<Tags>> GetAllTagsAsync()
        {
            var data = await _tagsRepo.GetAllAsync();
            if (data == null)
            {
                return null;
            }
            return data.ToList();
        }

        public async Task<Category> GetCategoryAsync(string id)
        {
            var data = await _categoryRepo.FindAsync(a => a.Id.ToString() == id);
            if (data == null)
            {
                return null;
            }
            return data.FirstOrDefault();
        }

        public async Task<Tags> GetTagsAsync(string id)
        {
            var data = await _tagsRepo.FindAsync(a => a.Id.ToString() == id);
            if (data == null)
            {
                return null;
            }
            return data.FirstOrDefault();
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var added = await _categoryRepo.UpdateAsync(category);
            if (added == 1)
            {
                return category;
            }
            return null;
        }

        public async Task<Tags> UpdateTagsAsync(Tags tags)
        {
            var added = await _tagsRepo.UpdateAsync(tags);
            if (added == 1)
            {
                return tags;
            }
            return null;
        }
        // Publications

        public async Task<Publication> AddPublicationAsync(Publication publication)
        {
            var added = await _publicationRepo.AddAsync(publication);

            if (added == 1)
            {
                return publication;
            }

            return null;
        }


        public async Task<Publication> UpdatePublicationAsync(Publication publication)
        {
            var updated = await _publicationRepo.UpdateAsync(publication);

            if (updated == 1)
            {
                return publication;
            }

            return null;
        }


        public async Task<Publication> DeletePublicationAsync(Publication publication)
        {
            var deleted = await _publicationRepo.RemoveAsync(publication);

            if (deleted == 1)
            {
                return publication;
            }

            return null;
        }


        public async Task<Publication> GetPublicationAsync(string id)
        {
            var data = await _publicationRepo.FindAsync(
                x => x.Id.ToString() == id
            );

            if (data == null)
            {
                return null;
            }

            return data.FirstOrDefault();
        }


        public async Task<List<PublicationResponseDto>> GetAllPublicationsAsync()
        {
            var publications = new List<PublicationResponseDto>();

            // Get all categories
            var categories = await _categoryRepo.GetAllAsync();

            // Get all publications
            var data = await _publicationRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return publications;
            }

            // Get publication IDs from actual publication data
            var publicationIds = data
                .Select(a => a.Id.ToString())
                .ToList();

            // Get all category mappings for these publications
            var mappings = await _publicationCategoryMappingRepo
                .FindAsync(a => publicationIds.Contains(a.PublicationId));

            foreach (var pub in data)
            {
                // Get mappings for current publication
                var publicationMappings = mappings?
                    .Where(a => a.PublicationId == pub.Id.ToString())
                    .ToList();

                // Get category IDs for this publication
                var categoryIds = publicationMappings?
                    .Select(x => x.CategoryId)
                    .ToList() ?? new List<string>();

                // Get actual category objects
                var publicationCategories = categories?
                    .Where(c => categoryIds.Contains(c.Id.ToString()))
                    .ToList();

                // Convert categories to CategoryDto
                var categoryDtos = publicationCategories?
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id.ToString(),
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        ParentId = c.ParentId?.ToString()
                    })
                    .ToList() ?? new List<CategoryDto>();

                var publication = new PublicationResponseDto
                {
                    Id = pub.Id.ToString(),

                    Status = pub.Status,
                    Author = pub.Author,

                    Categories = categoryDtos,

                    CoverImage = pub.CoverImage,
                    Date = pub.Date,
                    Description = pub.Description,
                    DownloadUrl = pub.DownloadUrl,
                    FullContent = pub.FullContent,
                    PageSize = pub.PageSize,
                    Price = pub.Price,
                    Publisher = pub.Publisher,
                    Slug = pub.Slug,
                    Title = pub.Title,
                    Year = pub.Year
                };

                publications.Add(publication);
            }

            return publications;
        }
        //comment
        public async Task<PagePost> AddPagePostAsync(PagePost pagePost)
        {
            var added = await _pagePostRepo.AddAsync(pagePost);

            if (added == 1)
            {
                return pagePost;
            }

            return null;
        }
        public async Task<PagePost> UpdatePagePostAsync(PagePost pagePost)
        {
            var updated = await _pagePostRepo.UpdateAsync(pagePost);

            if (updated == 1)
            {
                return pagePost;
            }

            return null;
        }

        public async Task<PagePost> DeletePagePostAsync(PagePost pagePost)
        {
            var deleted = await _pagePostRepo.RemoveAsync(pagePost);

            if (deleted == 1)
            {
                return pagePost;
            }

            return null;
        }
        public async Task<PagePost> GetPagePostAsync(string id)
        {
            var data = await _pagePostRepo.FindAsync(
                x => x.Id.ToString() == id
            );

            if (data == null)
            {
                return null;
            }

            return data.FirstOrDefault();
        }
        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            var data = await _commentRepo.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            return data.ToList();
        }
        public async Task<List<Comment>> GetAllPublicCommentsAsync()
        {
            var data = await _commentRepo.FindAsync(a=>a.Status==CommentStatus.Approved);
            if (data == null)
            {
                return null;
            }

            return data.ToList();
        }
        public async Task<List<Comment>> GetAllUserCommentsAsync(string userId)
        {
            var data = await _commentRepo.FindAsync(a=>a.UserId==userId|| a.Status == CommentStatus.Approved);

            if (data == null)
            {
                return null;
            }

            return data.ToList();
        }
        // Page Post

        public async Task<Comment> AddCommentAsync(Comment pagePost)
        {
            var added = await _commentRepo.AddAsync(pagePost);

            if (added == 1)
            {
                return pagePost;
            }

            return null;
        }
        public async Task<Comment> UpdateCommentAsync(Comment pagePost)
        {
            var updated = await _commentRepo.UpdateAsync(pagePost);

            if (updated == 1)
            {
                return pagePost;
            }

            return null;
        }

        public async Task<Comment> DeleteCommentAsync(Comment pagePost)
        {
            var deleted = await _commentRepo.RemoveAsync(pagePost);

            if (deleted == 1)
            {
                return pagePost;
            }

            return null;
        }
        public async Task<Comment> GetCommentAsync(string id)
        {
            var data = await _commentRepo.FindAsync(
                x => x.Id.ToString() == id
            );

            if (data == null)
            {
                return null;
            }

            return data.FirstOrDefault();
        }
        public async Task<List<PagePostResponseDto>> GetAllPagePostsAsync()
        {
            var data = await _pagePostRepo.GetAllAsync();

            if (data == null || !data.Any())
            {
                return new List<PagePostResponseDto>();
            }

            var posts = data.ToList();

            // Only posts need category/tag mappings
            var postItems = posts
                .Where(x => x.Type != null &&
                            x.Type.Equals("post", StringComparison.OrdinalIgnoreCase))
                .ToList();

            List<PostCategoryMapping> categoryMappings = new();
            List<Category> categories = new();

            List<PostTagsMapping> tagMappings = new();
            List<Tags> tags = new();

            if (postItems.Any())
            {
                var postIds = postItems
                    .Select(x => x.Id.ToString())
                    .ToList();

                // Category mappings
                var categoryMappingData = await _postMappingRepo
                    .FindAsync(x => postIds.Contains(x.PostId));

                categoryMappings = categoryMappingData?.ToList()
                    ?? new List<PostCategoryMapping>();

                var categoryIds = categoryMappings
                    .Where(x => !string.IsNullOrEmpty(x.CategoryId))
                    .Select(x => x.CategoryId)
                    .Distinct()
                    .ToList();

                if (categoryIds.Any())
                {
                    var categoryData = await _categoryRepo
                        .FindAsync(x => categoryIds.Contains(x.Id.ToString()));

                    categories = categoryData?.ToList()
                        ?? new List<Category>();
                }

                // Tag mappings
                var tagMappingData = await _postTagsMappingRepo
                    .FindAsync(x => postIds.Contains(x.PostId));

                tagMappings = tagMappingData?.ToList()
                    ?? new List<PostTagsMapping>();

                var tagIds = tagMappings
                    .Where(x => !string.IsNullOrEmpty(x.TagsId))
                    .Select(x => x.TagsId)
                    .Distinct()
                    .ToList();

                if (tagIds.Any())
                {
                    var tagData = await _tagsRepo
                        .FindAsync(x => tagIds.Contains(x.Id.ToString()));

                    tags = tagData?.ToList()
                        ?? new List<Tags>();
                }
            }

            // Map response
            var result = posts.Select(post =>
            {
                var postId = post.Id.ToString();

                List<CategoryDto> postCategories = new();
                List<TagsDto> postTags = new();

                // Only attach mappings for "post"
                if (post.Type != null &&
                    post.Type.Equals("post", StringComparison.OrdinalIgnoreCase))
                {
                    var categoryIdsForPost = categoryMappings
                        .Where(x => x.PostId == postId)
                        .Select(x => x.CategoryId)
                        .ToHashSet();

                    postCategories = categories
                        .Where(x => categoryIdsForPost.Contains(x.Id.ToString()))
                        .Select(x => new CategoryDto
                        {
                            Id = x.Id.ToString(),
                            Name = x.Name,
                            Slug = x.Slug,
                            Description = x.Description,
                            ParentId = x.ParentId?.ToString()
                        })
                        .ToList();

                    var tagIdsForPost = tagMappings
    .Where(x => x.PostId == postId)
    .Select(x => x.TagsId)
    .Where(x => !string.IsNullOrEmpty(x))
    .ToHashSet();

                    postTags = tags
                        .Where(x => tagIdsForPost.Contains(x.Id.ToString()))
                        .Select(x => new TagsDto
                        {
                            Id = x.Id.ToString(),
                            Name = x.Name,
                            Slug = x.Slug
                        })
                        .ToList();
                }

                return new PagePostResponseDto
                {
                    Id = postId,
                    Author = post.Author,
                    Categories = postCategories,
                    CoverImage = post.CoverImage,
                    Date = post.Date,
                    Description = post.Description,
                    FullContent = post.FullContent,
                    Permalink = post.Permalink,
                    Status = post.Status,
                    Tags = postTags,
                    Year = post.Year,
                    Publisher = post.Publisher,
                    Price = post.Price,
                    PageSize = post.PageSize,
                    IsSellInStore = post.IsSellInStore,
                    DownloadUrl = post.DownloadUrl,
                    Type = post.Type,
                    Title = post.Title
                };
            }).ToList();

            return result;
        }

        public async Task<List<PublicationCategoryMapping>> AddPublicationCategoryMappingAsync(List<PublicationCategoryMapping> publicationCategoryMapping)
        {
            var publication = publicationCategoryMapping.FirstOrDefault();
            var existing = await _publicationCategoryMappingRepo.FindAsync(a => a.PublicationId == publication.PublicationId);
            if (existing != null)
            {
                await _publicationCategoryMappingRepo.RemoveRangeAsync(existing);
            }
            await _publicationCategoryMappingRepo.AddRangeAsync(publicationCategoryMapping);
            return publicationCategoryMapping;
        }

        public async Task<List<CategoryDto?>> GetPublicationCategoryMappingsAsync(string publicationId)
        {
            var existing = await _publicationCategoryMappingRepo.FindAsync(a => a.PublicationId == publicationId);

            if (existing == null || !existing.Any())
            {
                return new List<CategoryDto>();
            }

            var data = new List<CategoryDto>();
            var categoriesId = existing.Select(a => a.CategoryId).Distinct().ToList();
            var categories = await _categoryRepo.FindAsync(a => categoriesId.Contains(a.Id.ToString()));
            foreach (var category in categories)
            {
                data.Add(new CategoryDto()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Description = category.Description,
                    Slug = category.Slug,
                    ParentId = category.ParentId,
                });
            }
            return data;

        }

        public async Task<bool> AddTagAndCategoryMappingWithPagePostAsync(PagePost post, List<string>? catIds, List<string>? TagIds)
        {
            var postId = post.Id.ToString();
            var cates = await _postMappingRepo.FindAsync(a => a.PostId == postId);
            if (cates != null)
            {
                await _postMappingRepo.RemoveRangeAsync(cates);
            }
            var tags = await _postTagsMappingRepo.FindAsync(a => a.PostId == postId);
            if (tags != null)
            {
                await _postTagsMappingRepo.RemoveRangeAsync(tags);
            }
            if (TagIds.Any())
            {
                var tags1 = new List<PostTagsMapping>();
                foreach (var tagId in TagIds)
                {
                    tags1.Add(new PostTagsMapping()
                    {
                        TagsId = tagId.ToString(),
                        CreatedBy = post.UserId,
                        CreatedDate = DateTime.Now,
                        PostId = postId,
                    });
                }
                await _postTagsMappingRepo.AddRangeAsync(tags1);
            }
            if (catIds.Any())
            {
                var categories = new List<PostCategoryMapping>();
                foreach (var catId in catIds)
                {
                    categories.Add(new PostCategoryMapping()
                    {
                        CategoryId = catId.ToString(),
                        CreatedBy = post.UserId,
                        CreatedDate = DateTime.Now,
                        PostId = postId,
                    });
                }
                await _postMappingRepo.AddRangeAsync(categories);
            }
            return true;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesByPostIdAsync(string postId)
        {
            var existing = await _postMappingRepo.FindAsync(a => a.PostId == postId);
            if (existing != null)
            {
                var data = new List<CategoryDto>();
                var categoriesId = existing.Select(a => a.CategoryId).Distinct().ToList();
                var categories = await _categoryRepo.FindAsync(a => categoriesId.Contains(a.Id.ToString()));
                foreach (var category in categories)
                {
                    data.Add(new CategoryDto()
                    {
                        Id = category.Id.ToString(),
                        Name = category.Name,
                        Description = category.Description,
                        Slug = category.Slug,
                        ParentId = category.ParentId,
                    });
                }
                return data;
            }
            return null;
        }

        public async Task<List<TagsDto>> GetAllTagsByPostIdAsync(string postId)
        {
            var existing = await _postTagsMappingRepo.FindAsync(a => a.PostId == postId);
            if (existing != null)
            {
                var data = new List<TagsDto>();
                var categoriesId = existing.Select(a => a.TagsId).Distinct().ToList();
                var categories = await _tagsRepo.FindAsync(a => categoriesId.Contains(a.Id.ToString()));
                foreach (var category in categories)
                {
                    data.Add(new TagsDto()
                    {
                        Id = category.Id.ToString(),
                        Name = category.Name,
                        Slug = category.Slug,

                    });
                }
                return data;
            }
            return null;
        }

        public async Task<Media> AddMediaAsync(Media tags)
        {
            await _mediaRepo.AddAsync(tags);
            return tags;
        }

        public async Task<Media> UpdateMediaAsync(Media tags)
        {
            await _mediaRepo.UpdateAsync(tags);
            return tags;
        }

        public async Task<Media> DeleteMediaAsync(Media tags)
        {
            var data = await _mediaRepo.RemoveAsync(tags);
            if (data == 1)
            {
                return tags;
            }
            return null;
        }

        public async Task<Media> GetMediaAsync(string id)
        {
            var data = await _mediaRepo.FindAsync(a => a.Id.ToString() == id);
            if (data == null)
            {
                return null;
            }
            return data.FirstOrDefault();
        }

        public async Task<List<Media>> GetAllMediaAsync()
        {
            var data = await _mediaRepo.GetAllAsync();
            if (data == null)
            {
                return null;
            }
            return data.ToList();
        }
    }
}

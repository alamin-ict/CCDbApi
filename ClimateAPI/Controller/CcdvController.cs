using CCDbApi.Model;
using CCDbApi.Service;
using CCDbApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CCDbApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CcdvController : ControllerBase
    {
        private readonly ICcdvService _ccdvService;
        private readonly EmailService _emailService;


        public CcdvController(ICcdvService ccdvService, EmailService emailService)
        {
            _ccdvService = ccdvService;
            _emailService = emailService;
        }
        [HttpPost("uploadPhoto")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadProfilePhotoAsync(IFormFile file)
        {
            string photoUrl = null;
            string downloadUrl = null;

            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    Message = "No file uploaded or file is empty.",
                    PhotoUrl = photoUrl,
                    DownloadUrl = downloadUrl
                });
            }

            // Allowed file extensions
            var allowedExtensions = new[]
            {
            ".png", ".jpg", ".jpeg", ".ico", ".gif",
            ".pdf", ".doc", ".docx", ".txt"
        };

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new
                {
                    Message = "Invalid file type. Only .png, .jpg, .jpeg, .ico, .gif, .pdf, .doc, .docx and .txt files are allowed.",
                    PhotoUrl = photoUrl,
                    DownloadUrl = downloadUrl
                });
            }

            // Create uploads folder if it doesn't exist
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate unique filename
            string fileName = $"{Guid.NewGuid()}{fileExtension}";
            string filePath = Path.Combine(uploadPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // URLs
            photoUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
            downloadUrl = $"{Request.Scheme}://{Request.Host}/api/Common/download/{fileName}";

            return Ok(new
            {
                Message = "File uploaded successfully.",
                FileName = fileName,
                OriginalFileName = file.FileName,
                FileSize = file.Length,
                PhotoUrl = photoUrl,
                DownloadUrl = downloadUrl
            });
        }

        [HttpGet("download/{fileName}")]
        [AllowAnonymous]
        public IActionResult DownloadFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest(new
                {
                    Message = "File name is required."
                });
            }

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            string filePath = Path.Combine(uploadPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new
                {
                    Message = "File not found."
                });
            }

            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            string contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".ico" => "image/x-icon",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Forces download
            return File(fileBytes, contentType, fileName);
        }
        // GET: api/Tags
        [HttpGet("getAllTags")]
        public async Task<ActionResult<IEnumerable<Tags>>> getAllTags()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Tags data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tags = new List<Tags>();
                tags = await _ccdvService.GetAllTagsAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }

        }
        // GET: api/Tags/{id}
        [HttpGet("getTags/{id}")]
        public async Task<ActionResult<Tags>> getTags(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Tags data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new Tags();
                tag = await _ccdvService.GetTagsAsync(id);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // POST: api/Tags
        [HttpPost("addOrUpdateTags")]
        public async Task<ActionResult<Tags>> addTags(TagsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Tags data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new Tags();
                if (dto.Id == null)
                {
                    tag = new Tags()
                    {
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        Name = dto.Name,
                        Slug = dto.Slug,
                        UserId = userId
                    };

                    tag = await _ccdvService.AddTagsAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to add tag data");
                    }
                }
                else
                {
                    tag = await _ccdvService.GetTagsAsync(dto.Id);
                    if (tag == null)
                    {
                        return BadRequest("No such tag found with this id");
                    }
                    tag.Name = dto.Name;
                    tag.Slug = dto.Slug;
                    tag.UpdatedBy = userId;
                    tag.UpdatedDate = DateTime.Now;
                    tag = await _ccdvService.UpdateTagsAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to update tag data");
                    }
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // PUT: api/Tags/{id}
        [HttpPut("deleteTags/{id}")]
        public async Task<IActionResult> deleteTags(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Tags data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new Tags();
                tag = await _ccdvService.GetTagsAsync(id);
                if (tag != null)
                {
                    tag = await _ccdvService.DeleteTagsAsync(tag);
                    if (tag != null)
                    {
                        return Ok("Successfully deleted");
                    }
                    return BadRequest("Failed to delete record");
                }
                else
                {
                    return BadRequest("No such data found");
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }


        // GET: api/Catgories
        [HttpGet("getAllCategories")]
        public async Task<ActionResult<IEnumerable<Tags>>> getAllCategories()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Category data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tags = new List<Category>();
                tags = await _ccdvService.GetAllCategoriesAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }

        }
        // GET: api/Tags/{id}
        [HttpGet("getCategory/{id}")]
        public async Task<ActionResult<Category>> getCategory(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Category data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new Category();
                tag = await _ccdvService.GetCategoryAsync(id);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // POST: api/Tags
        [HttpPost("addOrUpdateCategory")]
        public async Task<ActionResult<Tags>> addTags(CategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Category data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new Category();
                if (dto.Id == null)
                {
                    tag = new Category()
                    {
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        Name = dto.Name,
                        Slug = dto.Slug,
                        UserId = userId,
                        Description = dto.Description,
                        TagsId = dto.TagsId,

                    };

                    tag = await _ccdvService.AddCategoryAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to add Category data");
                    }
                }
                else
                {
                    tag = await _ccdvService.GetCategoryAsync(dto.Id);
                    if (tag == null)
                    {
                        return BadRequest("No such tag found with this id");
                    }
                    tag.Name = dto.Name;
                    tag.Slug = dto.Slug;
                    tag.UpdatedBy = userId;
                    tag.UpdatedDate = DateTime.Now;
                    tag.TagsId = dto.TagsId;
                    tag.Description = dto.Description;
                    tag = await _ccdvService.UpdateCategoryAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to update Category data");
                    }
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // PUT: api/Tags/{id}
        [HttpPut("deleteCategory/{id}")]
        public async Task<IActionResult> deleteCategory(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Category data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new Category();
                tag = await _ccdvService.GetCategoryAsync(id);
                if (tag != null)
                {
                    tag = await _ccdvService.DeleteCategoryAsync(tag);
                    if (tag != null)
                    {
                        return Ok("Successfully deleted");
                    }
                    return BadRequest("Failed to delete record");
                }
                else
                {
                    return BadRequest("No such data found");
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }




        // GET: api/Publications
        [HttpGet("getAllPublications")]
        public async Task<ActionResult<IEnumerable<Tags>>> getAllPublications()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Publication data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tags = new List<Publication>();
                tags = await _ccdvService.GetAllPublicationsAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }

        }
        // GET: api/Publication/{id}
        [HttpGet("getPublication/{id}")]
        public async Task<ActionResult<PublicationResponseDto>> getPublication(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Publication data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new Publication();
                tag = await _ccdvService.GetPublicationAsync(id);
                if (tag == null)
                {
                    return Ok(tag);
                }
                var categoryDtos = await _ccdvService.GetPublicationCategoryMappingsAsync(id);

                var publication = new PublicationResponseDto()
                {
                    Id = id,
                    Status = tag.Status,
                    Author = tag.Author,
                    Categories = categoryDtos,
                    CoverImage = tag.CoverImage,
                    Date = tag.Date,
                    Description = tag.Description,
                    DownloadUrl = tag.DownloadUrl,
                    FullContent = tag.FullContent,
                    PageSize = tag.PageSize,
                    Price = tag.Price,
                    Publisher = tag.Publisher,
                    Slug = tag.Slug,
                    Title = tag.Title,
                    Year = tag.Year,
                };


                return Ok(publication);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // POST: api/Tags
        [HttpPost("addOrUpdatePublication")]
        public async Task<ActionResult<Publication>> addTags(CreatePublicationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid role data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new Publication();
                if (dto.Id == null)
                {
                    tag = new Publication()
                    {
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        Author = dto.Author,
                        AuthorId = dto.AuthorId,
                        CoverImage = dto.CoverImage,
                        Date = dto.Date.HasValue ? dto.Date.Value : DateTime.Now,
                        DownloadUrl = dto.DownloadUrl,
                        FullContent = dto.FullContent,
                        PageSize = dto.PageSize,
                        Publisher = dto.Publisher,
                        PublisherId = dto.PublisherId,
                        Price = dto.Price,
                        Title = dto.Title,
                        Status = dto.Status,
                        Year = dto.Year,
                        Slug = dto.Slug,
                        UserId = userId,
                        Description = dto.Description,

                    };

                    tag = await _ccdvService.AddPublicationAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to add Publication data");
                    }
                }
                else
                {
                    tag = await _ccdvService.GetPublicationAsync(dto.Id);
                    if (tag == null)
                    {
                        return BadRequest("No such Publication found with this id");
                    }

                    tag.Slug = dto.Slug;
                    tag.UpdatedBy = userId;
                    tag.UpdatedDate = DateTime.Now;
                    tag.Author = dto.Author;
                    tag.AuthorId = dto.AuthorId;
                    tag.CoverImage = dto.CoverImage;
                    tag.Status = dto.Status;
                    tag.DownloadUrl = dto.DownloadUrl;
                    tag.FullContent = dto.FullContent;
                    tag.PageSize = dto.PageSize;
                    tag.Publisher = dto.Publisher;
                    tag.PublisherId = dto.PublisherId;
                    tag.Title = dto.Title;

                    tag.Description = dto.Description;
                    tag = await _ccdvService.UpdatePublicationAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to update Publication data");
                    }
                }

                var categories = new List<PublicationCategoryMapping>();
                foreach (string id in dto.CategoryIds)
                {
                    categories.Add(new PublicationCategoryMapping()
                    {
                        CategoryId = id,
                        PublicationId = tag.Id.ToString(),
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                    });
                }
                if (categories.Any())
                {
                    categories = await _ccdvService.AddPublicationCategoryMappingAsync(categories);
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // PUT: api/Publication/{id}
        [HttpPut("deletePublication/{id}")]
        public async Task<IActionResult> deletePublication(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Publication data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new Publication();
                tag = await _ccdvService.GetPublicationAsync(id);
                if (tag != null)
                {
                    tag = await _ccdvService.DeletePublicationAsync(tag);
                    if (tag != null)
                    {
                        return Ok("Successfully deleted");
                    }
                    return BadRequest("Failed to delete record");
                }
                else
                {
                    return BadRequest("No such data found");
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }


        // GET: api/PagePosts
        [HttpGet("getAllPagePosts")]
        public async Task<ActionResult<IEnumerable<Tags>>> getAllPagePosts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid PagePost data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tags = new List<PagePost>();
                tags = await _ccdvService.GetAllPagePostsAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }

        }
        // GET: api/PagePost/{id}
        [HttpGet("getPagePost/{id}")]
        public async Task<ActionResult<PagePostResponseDto>> getPagePost(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid PagePost data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new PagePost();
                tag = await _ccdvService.GetPagePostAsync(id);
                if (tag == null)
                {
                    return Ok(tag);
                }
                var categories = await _ccdvService.GetAllCategoriesByPostIdAsync(tag.Id.ToString());
                var tags = await _ccdvService.GetAllTagsByPostIdAsync(tag.Id.ToString());
                var data = new PagePostResponseDto()
                {
                    Author = tag.Author,
                    Categories = categories,
                    CoverImage = tag.CoverImage,
                    Date = tag.Date,
                    Description = tag.Description,
                    FullContent = tag.FullContent,
                    Id = tag.Id.ToString(),
                    Permalink = tag.Permalink,
                    Status = tag.Status,
                    Tags = tags,
                    Title = tag.Title,
                };
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // POST: api/PagePost
        [HttpPost("addOrUpdatePagePost")]
        public async Task<ActionResult<PagePost>> addTags(CreatePagePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid role data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new PagePost();
                if (dto.Id == null)
                {
                    tag = new PagePost()
                    {
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        Author = dto.Author,
                        AuthorId = dto.AuthorId,
                        CoverImage = dto.CoverImage,
                        Date = DateTime.Now,

                        FullContent = dto.FullContent,
                        Permalink = dto.Permalink,
                        Publication = dto.Publication,

                        Title = dto.Title,
                        Status = dto.Status,

                        UserId = userId,
                        Description = dto.Description,

                    };

                    tag = await _ccdvService.AddPagePostAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to add PagePost data");
                    }
                }
                else
                {
                    tag = await _ccdvService.GetPagePostAsync(dto.Id);
                    if (tag == null)
                    {
                        return BadRequest("No such PagePost found with this id");
                    }

                    tag.Permalink = dto.Permalink;
                    tag.Publication = dto.Publication;
                    tag.UpdatedBy = userId;
                    tag.UpdatedDate = DateTime.Now;
                    tag.Author = dto.Author;
                    tag.AuthorId = dto.AuthorId;
                    tag.CoverImage = dto.CoverImage;
                    tag.Status = dto.Status;

                    tag.FullContent = dto.FullContent;

                    tag.Title = dto.Title;

                    tag.Description = dto.Description;
                    tag = await _ccdvService.UpdatePagePostAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to update PagePost data");
                    }
                }
                if (dto.CategoryIds.Any() || dto.TagsIds.Any())
                {
                    var data = await _ccdvService.AddTagAndCategoryMappingWithPagePostAsync(tag, dto.CategoryIds, dto.TagsIds);
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // PUT: api/Publication/{id}
        [HttpPut("deletePagePost/{id}")]
        public async Task<IActionResult> deletePagePost(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid PagePost data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new PagePost();
                tag = await _ccdvService.GetPagePostAsync(id);
                if (tag != null)
                {
                    tag = await _ccdvService.DeletePagePostAsync(tag);
                    if (tag != null)
                    {
                        return Ok("Successfully deleted");
                    }
                    return BadRequest("Failed to delete record");
                }
                else
                {
                    return BadRequest("No such data found");
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }


    }
}

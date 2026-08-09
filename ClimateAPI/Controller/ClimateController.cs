using CCDbApi.Model;
using CCDbApi.Service;
using CCDbApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Org.BouncyCastle.Bcpg;
using System.Data;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace CCDbApi.Controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClimateController : ControllerBase
    {
        private readonly IClimateService _climateService;
        private readonly EmailService _emailService;


        public ClimateController(IClimateService climateService, EmailService emailService)
        {
            _climateService = climateService;
            _emailService = emailService;
        }
        [HttpPost("SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> InsertNewUserIntoDB([FromBody] UserViewModel userData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid user data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var roles = await _climateService.GetDbRoleAsync();
            if (!roles.Any(r => r.Type == userData.Role))
            {
                return BadRequest(new
                {
                    message = "Invalid role ID. The specified role does not exist.",
                    errors = new[] { "The provided role ID is not valid." }
                });
            }
            var role = roles.FirstOrDefault(r => r.Type == userData.Role);
            var existUser = await _climateService.GetUserByAsync(userData.UserName, userData.Password, userData.Email, role.Id.ToString());
            if (existUser != null)
            {
                return Ok(new
                {
                    message = "Already existed this user",
                    user = userData

                });
            }
            var id = Guid.NewGuid();
           
            var userdata = new User()
            {
                Id = id,
                CreatedDate = DateTime.Now,
                UserName = userData.UserName,
                Email = userData.Email,
                Status = UserStatus.Pending,
                Password = userData.Password,
                RoleId = role.Id.ToString(),
                CreatedBy = id.ToString(),
                UpdatedBy = id.ToString()

            };
            var addedUser = await _climateService.InsertIntoDbUserAsync(userdata);

            return Ok(new
            {
                message = "Inserted this user into DB successfully",
                user = addedUser


            });


        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginData([FromBody] Login log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid login data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var existUser = await _climateService.GetUserAsync(log.UserName, log.Password);
            if (existUser == null)
            {
                return Ok(new
                {
                    message = "No User existed . Please at first register the user",


                });
            }
            if (existUser.Status != UserStatus.Active)
            {
                return Ok(new
                {
                    message = $"User is not active . Current status is {existUser.Status}",


                });
            }

            var token = await _climateService.GetToken(existUser);
            var role = await _climateService.GetUserByIdAsync(existUser.RoleId);
            return Ok(new
            {
                message = "Login succesfull",
                user = existUser,
                token = token,
                role
            });

        }

        [HttpPost("updateUser")]
     
        public async Task<IActionResult> UpdateData([FromBody] UpdateUserViewModel dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid update user data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            // Retrieve user from context
            var user = HttpContext.User;
            // Optionally retrieve user ID if needed
            var userId = user.FindFirst("Id")?.Value;
            var email = user.FindFirst("Email")?.Value;
            var existUser = await _climateService.GetUserByIdAsync(dto.Id);
            if (existUser == null)
            {
                return Ok(new
                {
                    message = "No User existed . Please at first register the user",


                });
            }
            var roles = await _climateService.GetDbRoleAsync();
            if (!roles.Any(r => r.Type == dto.Role))
            {
                return BadRequest(new
                {
                    message = "Invalid role ID. The specified role does not exist.",
                    errors = new[] { "The provided role ID is not valid." }
                });
            }
            var role = roles.FirstOrDefault(r => r.Type == dto.Role);
            existUser.UpdatedDate = DateTime.Now;
            existUser.UpdatedBy = userId;
            existUser.Email = dto.Email;
            existUser.Status = dto.Status;
            existUser.RoleId = role.Id.ToString();
            existUser.Password = dto.Password;
            existUser.UserName = dto.UserName;
            existUser = await _climateService.UpdateUserAsync(existUser);
            return Ok(new
            {
                message = "Updated user succesfull",
                user = existUser,
                role
            });

        }


        [HttpGet("getAllUser")]

        public async Task<IActionResult> GetAllData()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid update user data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            // Retrieve user from context
            var user = HttpContext.User;
            // Optionally retrieve user ID if needed
            var userId = user.FindFirst("Id")?.Value;
            var email = user.FindFirst("Email")?.Value;
            var users = await _climateService.GetAllUserAsync();
            var roles = await _climateService.GetDbRoleAsync();
            if (users == null)
            {
                return Ok(new
                {
                    message = "No User existed . Please at first register the user",


                });
            }
           
            return Ok(new
            {
                message = "Users are retrived succesfully",
                users,
                roles
               
            });

        }



        [HttpDelete("deleteUser")]

        public async Task<IActionResult> DeeleteUser(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid update user data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            // Retrieve user from context
            var user = HttpContext.User;
            // Optionally retrieve user ID if needed
            var userId = user.FindFirst("Id")?.Value;
            var email = user.FindFirst("Email")?.Value;
            var existUser = await _climateService.GetUserByIdAsync(id);
            if (existUser == null)
            {
                return Ok(new
                {
                    message = "No User existed . Please at first register the user",


                });
            }
          
            
            existUser = await _climateService.DeleteUserAsync(existUser);
            if (existUser == null)
            {
                return StatusCode(500, "Failed to delete user");
            }
            return Ok(new
            {
                message = "Deleted user succesfull",
                user = existUser,
            });

        }

        [HttpPost("addSubscribe")]
        [AllowAnonymous]
        public async Task<IActionResult> addSubscribe([FromBody] SubscribeViewModel sub)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Subscribe data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            var subs = new Subscribe()
            {
                Id = Guid.NewGuid(),
                Email = sub.Email,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Source = "https://portal.ccdbclimatecentre.com/",
                Category = "CCDB_Climate",
                IsActive = true,
                IsDeleted = false

            };
            var subscribe = await _climateService.AddSubscribeAsync(subs);
            return Ok(new
            {
                message = "data retrived succesfully",
                Subscribe = subscribe
            });

        }
        [HttpPost("getAllSubscribe")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSubscribes()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Subscribe data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var subscribes = await _climateService.GetAllSubscribeAsync();
            return Ok(new
            {
                message = "Login succesfull",
                Subscribe = subscribes
            });

        }

        [HttpPost("addRole")]
        public async Task<IActionResult> InsertNewUserRoleIntoDB([FromBody] RoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid role data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            // Retrieve user from context
            var user = HttpContext.User;
            // Optionally retrieve user ID if needed
            var userId = user.FindFirst("Id")?.Value;
            var email = user.FindFirst("Email")?.Value;
            var existRole = await _climateService.GetUserRoleByIdAsync(role.Name);
            if (existRole != null)
            {
                return Ok(new
                {
                    message = "Already existed this user role",
                    role = existRole

                });
            }
            var userRole = new Role()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Name = role.Name,
                Type = role.Type,
                UpdatedBy = userId,
                CreatedBy = userId,
            };
            var addedRole = await _climateService.InsertIntoDbRoleAsync(userRole);
            return Ok(new
            {
                message = "Inserted this user role into DB successfully",
                role = addedRole

            });
        }

        [HttpPost("getAllRoles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoleIntoDB()
        {

            var roles = await _climateService.GetDbRoleAsync();
            return Ok(new
            {
                message = "Get all roles from DB successfully",
                roles = roles

            });
        }

        [HttpPost("addContact")]
        public async Task<IActionResult> InsertAddContactIntoDB([FromBody] ContactViewModel contact)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid contact data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;

            // Check if the user already has a contact entry
            var existingContact = await _climateService.GetContactByIdAsync(userId);
            if (existingContact != null)
            {
                return Ok(new
                {
                    message = "This user already has an existing contact.",
                    contact = existingContact
                });
            }

            // Create a new  contact record (adjusting to match the model structure)
            var newContact = new Contact // Assuming you mean `Role`, adapt this as needed
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow, // Use UTC for consistency
                Name = contact.Name, // Map data from ContactViewModel
                Subject = contact.Subject, // Assuming `Type` is equivalent to Subject
                Message = contact.Message,
                IsResponse = contact.IsResponse,
                Email = contact.Email,
                UserId = userId,
                CreatedBy = userId,
                UpdatedBy = userId
            };

            // Insert the new contact into the database
            var addedContact = await _climateService.InsertIntoDbContactAsync(newContact);

            return Ok(new
            {
                message = "Contact has been successfully inserted into the database.",
                contact = addedContact
            });
        }

        [HttpPost("addPartnerOrClient")]

        public async Task<IActionResult> addPartnerOrClient(IFormFile file, [FromQuery] PartnerViewModel partner)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid  data format.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });

            }
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded or file is empty." });
            }

            // Validate file extension
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".ico", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { message = "Invalid file type. Only .png, .jpg, .jpeg, .ico files are allowed." });
            }
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            try
            {
                // Ensure the upload directory exists
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Save the photo to the file system
                string fileName = Guid.NewGuid() + fileExtension; // Generate unique file name
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // Generate the public URL for the uploaded file
                string publicUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

                // Create a new role or contact record (adjusting to match the model structure)
                var newPartner = new Partner // Assuming you mean `Role`, adapt this as needed
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow, // Use UTC for consistency
                    Title = partner.Title,
                    Description = partner.Description,
                    Image = publicUrl,
                    Type = partner.Type,
                    DetailsLink = partner.DetailsLink,
                    Heading = partner.Heading,
                    SubTitle = partner.SubTitle,
                    UpdatedDate = DateTime.UtcNow,
                    Area = partner.Area,
                    Category = partner.Category,
                    IsDeleted = false,
                    IsActive = true,
                    UserId = userId,
                    CreatedBy = userId,
                    UpdatedBy = userId

                };

                // Insert the new contact into the database
                var addedPartner = await _climateService.InsertIntoDbPartnerAsync(newPartner);

                return Ok(new
                {
                    message = "Data has been successfully inserted into the database.",
                    Data = addedPartner
                });
            }
            catch (Exception ex)
            {
                // Log the exception (assuming a logger is configured)

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An internal server error occurred while uploading the profile photo.",
                    error = ex.Message
                });
            }
        }
        [HttpPost("updatePicture")]
        public async Task<IActionResult> UpdateImageAsync(IFormFile file, string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var partnerId))
            {
                return BadRequest(new { message = "Invalid or missing partner ID." });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded or file is empty." });
            }

            try
            {
                var existingPartner = await _climateService.GetPartnerByIdAsync(partnerId);
                if (existingPartner == null)
                {
                    return NotFound(new { message = "No such data exists with that id." });
                }

                // Validate file extension
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".ico", ".gif" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = "Invalid file type. Allowed: .png, .jpg, .jpeg, .ico, .gif" });
                }
                // Retrieve user details from the HTTP context
                var user = HttpContext.User;
                var userId = user.FindFirst("Id")?.Value;
                var userEmail = user.FindFirst("Email")?.Value;
                // Ensure upload directory exists
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                Directory.CreateDirectory(uploadPath);

                // Save new file
                string fileName = $"{Guid.NewGuid()}{fileExtension}";
                string filePath = Path.Combine(uploadPath, fileName);

                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate public URL for retrieval
                string publicUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

                // Update database with new image URL
                existingPartner.Image = publicUrl;
                existingPartner.UpdatedBy = userId;
                await _climateService.UpdatedDataIntoDbPartnerAsync(existingPartner);

                return Ok(new
                {
                    message = "Image updated successfully.",
                    imageUrl = publicUrl
                });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error updating partner image.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while updating the image.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("updatePartnerOrClient")]
        public async Task<IActionResult> UpdatePartnerOrClientAsync([FromBody] UpdatePartnerViewModel partner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid data format.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                if (!Guid.TryParse(partner.Id, out Guid partnerId))
                {
                    return BadRequest(new { message = "Invalid ID format." });
                }

                var existingPartner = await _climateService.GetPartnerByIdAsync(partnerId);
                if (existingPartner == null)
                {
                    return NotFound(new { message = "No such data exists with that ID." });
                }
                // Retrieve user details from the HTTP context
                var user = HttpContext.User;
                var userId = user.FindFirst("Id")?.Value;
                var userEmail = user.FindFirst("Email")?.Value;

                // Update partner details
                existingPartner.Title = partner.Title;
                existingPartner.SubTitle = partner.SubTitle;
                existingPartner.Heading = partner.Heading;
                existingPartner.Description = partner.Description;
                existingPartner.Type = partner.Type;
                existingPartner.UpdatedDate = DateTime.UtcNow;
                existingPartner.DetailsLink = partner.DetailsLink;
                existingPartner.UpdatedBy = userId;
                existingPartner.Area = partner.Area;
                existingPartner.Category = partner.Category;
                // Save changes to the database
                var updatedPartner = await _climateService.UpdatedDataIntoDbPartnerAsync(existingPartner);

                return Ok(new
                {
                    message = "Data updated successfully.",
                    data = updatedPartner
                });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An internal server error occurred while updating the partner.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("getAllClientsOrPartners")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPartnersResultAsync()
        {
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            var partners = await _climateService.GetAllPartnersOrClients();
            if (partners.Any())
            {
                return Ok(new
                {
                    message = "Data has been found successfully",
                    data = partners

                });
            }
            return Ok(new { message = "No data has been found" });

        }
        [HttpDelete("deletePartnerOrClient")]
        public async Task<IActionResult> DeletedPartnerOrClientAsync(string Id)
        {
            var existedPartner = await _climateService.GetPartnerByIdAsync(Guid.Parse(Id));
            if (existedPartner == null)
            {
                return BadRequest(new
                {
                    message = "No such record is existed with this Id"

                });
            }
            var deletedPartner = await _climateService.DeletedPartnerDataFromDB(existedPartner);
            return Ok(new
            {
                message = "Deleted data successfully",
                Data = deletedPartner
            });
        }
        [HttpPost("addNewsContent")]
        public async Task<IActionResult> addNewsContentAsync([FromBody] NewsContentViewModel news)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid newsContent data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            // Create a new  NewsContent record (adjusting to match the model structure)
            var newNewsContent = new NewsContent // Assuming you mean `NewsContent`, adapt this as needed
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow, // Use UTC for consistency
                Type = news.Type,
                Headline = news.Headline,
                ImageUrl = news.ImageUrl,
                MainText = news.MainText,
                DetailText = news.DetailText,
                Subscribed = news.Subscribed,
                UserId = userId,
                CreatedBy = userId,
                UpdatedBy = userId
            };

            // Insert the news content into the database
            var addedNewsContent = await _climateService.InsertIntoDbNewsContentAsync(newNewsContent);

            return Ok(new
            {
                message = "NewsContent has been successfully inserted into the database.",
                newsContent = addedNewsContent
            });
        }
        [HttpGet("getAllNewsContent")]
        public async Task<IActionResult> GetAllNewsContent()
        {
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            var newsContents = await _climateService.GetAllNewsContentAsync(userId);
            return Ok(newsContents);
        }

        [HttpPost("addImageConfiguration")]
        public async Task<IActionResult> addedImageConfigurationAsync([FromBody] ImageConfigurationViewModel image)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid  data format.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });

            }
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            var newImage = new ImageConfiguration()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                ImagePosition = image.ImagePosition,
                ImageUrl = image.ImageUrl,
                Name = image.Name,
                DetailsText = image.DetailsText,
                UserId = userId,
                CreatedBy = userId,
                UpdatedBy = userId

            };
            var addedImage = await _climateService.AddImageConfigurationAsync(newImage);
            return Ok(new
            {
                message = "Iserted ImageConfiguration into DB sucessfully",
                imageConfiguration = addedImage

            });
        }
        [HttpGet("getAllImageConfigurations")]
        public async Task<IActionResult> GetImageConfiguration()
        {
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            var imageConfigurations = await _climateService.GetImageConfigurationsAsync(userId);
            return Ok(imageConfigurations);
        }
        [HttpPost("addSliderImage")]
        public async Task<IActionResult> AddedSliderImage([FromBody] SliderImageViewModel slider)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid  data format.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });

            }
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            var newImage = new SliderImage()
            {
                Id = Guid.NewGuid(),
                Name = slider.Name,
                SliderDetailText = slider.SliderDetailText,
                CreatedDate = DateTime.UtcNow,
                SliderMainText = slider.SliderMainText,
                SliderType = slider.SliderType,
                ImageUrl = slider.ImageUrl,
                SliderOrder = slider.SliderOrder,
                UserId = userId,
                CreatedBy = userId,
                UpdatedBy = userId

            };
            var addedSliderImage = await _climateService.AddSliderImageAsync(newImage);
            return Ok(new
            {
                message = "Inserted sliderImage into DB successfully",
                sliderImage = addedSliderImage

            });

        }
        [HttpGet("getAllSliderImages")]
        public async Task<IActionResult> GetSliderImages()
        {
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            var sliderImages = await _climateService.GetSliderImagesAsync(userId);
            return Ok(sliderImages);
        }

        // POST: api/ProfilePhoto/addPhoto
        [HttpPost("uploadPhoto")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadProfilePhotoAsync(IFormFile file)
        {


            string PhotoUrl = null;
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    message = "No file uploaded or file is empty.",
                    Photourl = PhotoUrl
                });
            }

            // Validate file extension
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".ico", ".gif", ".pdf", ".docs", ".txt" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new
                {
                    message = "Invalid file type. Only .png, .jpg, .jpeg, .ico,.pdf,.docs,.txt files are allowed.",
                    PhotoUrl = PhotoUrl
                });
            }



            // Ensure the upload directory exists
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Save the photo to the file system
            string fileName = Guid.NewGuid() + fileExtension; // Generate unique file name
            string filePath = Path.Combine(uploadPath, fileName);



            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Generate the public URL for the uploaded file
            PhotoUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";


            // Return success response with the photo details
            return Ok(new
            {
                message = "File uploaded successfully.",
                PhotoUrl = PhotoUrl
            });


        }



        [HttpDelete("deleteTrainingInfo")]
        public async Task<IActionResult> DeleteTrainingInfoAsync(Guid Id)
        {
            var existedPartner = await _climateService.GetTrainingInfoByIdAsync(Id);
            if (existedPartner == null)
            {
                return BadRequest(new
                {
                    message = "No such record is existed with this Id"

                });
            }
            var deletedPartner = await _climateService.DeleteTrainingInfoDataFromDB(existedPartner);
            return Ok(new
            {
                message = "Deleted data successfully",
                Data = deletedPartner
            });
        }
        [HttpPost("addOrUpdateTrainingInfo")]
        public async Task<IActionResult> addOrUpdateTrainingInfoAsync([FromBody] TrainingInfoViewModel training)
        {
            // Validate the input model
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid newsContent data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            TrainingInfo existingTraining = null;
            if (training.Id != null)
            {
                existingTraining = await _climateService.GetTrainingInfoByIdAsync(training.Id.Value);
                if (existingTraining == null)
                {
                    return BadRequest(new
                    {
                        message = "No such record is existed with this Id"

                    });
                }
                existingTraining.CourseDescription = training.CourseDescription;
                existingTraining.CourseFee = training.CourseFee;
                existingTraining.CourseLink = training.CourseLink;
                existingTraining.CourseOverview = training.CourseOverview;
                existingTraining.Date = training.Date;
                existingTraining.EndDate = training.EndDate;

                existingTraining.Register = training.Register;
                existingTraining.RegisterOverview = training.RegisterOverview;
                existingTraining.StartDate = training.StartDate;
                existingTraining.Subject = training.Subject;
                existingTraining.TrainingLocation = training.TrainingLocation;
                existingTraining.UpdatedDate = DateTime.Now;
                existingTraining.UpdatedBy = userId;
                existingTraining.UserId = userId;
                existingTraining.Venue = training.Venue;
                existingTraining = await _climateService.UpdateIntoDbTrainingInfoAsync(existingTraining);

                return Ok(new
                {
                    message = "TrainingInfo has been successfully updated into the database.",
                    newsContent = existingTraining
                });

            }

            // Create a new  NewsContent record (adjusting to match the model structure)
            existingTraining = new TrainingInfo // Assuming you mean `NewsContent`, adapt this as needed
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow, // Use UTC for consistency
                CourseDescription = training.CourseDescription,
                CourseFee = training.CourseFee,
                CourseLink = training.CourseLink,
                CourseOverview = training.CourseOverview,
                Date = training.Date,
                EndDate = training.EndDate,
                IsActive = true,
                IsDeleted = false,
                Register = training.Register,
                RegisterOverview = training.RegisterOverview,
                StartDate = training.StartDate,
                Subject = training.Subject,
                TrainingLocation = training.TrainingLocation,
                UpdatedDate = DateTime.Now,
                CreatedBy = userId,
                UpdatedBy = userId,
                UserId = userId,

                Venue = training.Venue,


            };

            // Insert the news content into the database
            existingTraining = await _climateService.InsertIntoDbTrainingInfoAsync(existingTraining);

            return Ok(new
            {
                message = "TrainingInfo has been successfully inserted into the database.",
                newsContent = existingTraining
            });
        }
        [HttpGet("getAllTrainingInfo")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTrainingInfo()
        {
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            var TrainingInfos = await _climateService.GetAllTrainingInfoAsync();
            return Ok(TrainingInfos);
        }
        [HttpGet("getTrainingInfo")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTrainingInfo(Guid Id)
        {
            // Retrieve user details from the HTTP context
            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;
            var userEmail = user.FindFirst("Email")?.Value;
            var trainingInfo = await _climateService.GetTrainingInfoByIdAsync(Id);
            return Ok(trainingInfo);
        }
        [HttpPost("addOrUpdateTraineeInfo")]
        public async Task<IActionResult> AddOrUpdateTraineeInfoAsync([FromBody] TraineeInfoViewModel trainee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid trainee data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var user = HttpContext.User;
            var userId = user.FindFirst("Id")?.Value;

            TraineeInfo existingTrainee = null;

            if (trainee.Id != null)
            {
                existingTrainee = await _climateService.GetTraineeInfoByIdAsync(trainee.Id.Value);
                if (existingTrainee == null)
                {
                    return BadRequest(new { message = "No such record exists with this Id" });
                }

                // Update fields
                MapTraineeViewModelToEntity(trainee, existingTrainee);
                existingTrainee.UpdatedDate = DateTime.UtcNow;
                existingTrainee.UpdatedBy = userId;
                existingTrainee = await _climateService.UpdateTraineeInfoAsync(existingTrainee);

                return Ok(new
                {
                    message = "Trainee info successfully updated.",
                    data = existingTrainee
                });
            }

            // Create new trainee
            existingTrainee = new TraineeInfo
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedBy = userId,
                IsActive = true,
                IsDeleted = false,

                UpdatedDate = DateTime.UtcNow
            };

            MapTraineeViewModelToEntity(trainee, existingTrainee);

            existingTrainee = await _climateService.InsertTraineeInfoAsync(existingTrainee);

            return Ok(new
            {
                message = "Trainee info successfully created.",
                data = existingTrainee
            });
        }
        [HttpDelete("deleteTraineeInfo")]
        public async Task<IActionResult> DeleteTraineeInfoAsync(Guid id)
        {
            var trainee = await _climateService.GetTraineeInfoByIdAsync(id);
            if (trainee == null)
            {
                return BadRequest(new { message = "No such record exists with this Id" });
            }

            var deleted = await _climateService.DeleteTraineeInfoAsync(trainee);
            return Ok(new
            {
                message = "Trainee info successfully deleted.",
                data = deleted
            });
        }

        [HttpGet("getAllTraineeInfo")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTraineeInfoAsync()
        {
            var trainees = await _climateService.GetAllTraineeInfoAsync();
            return Ok(trainees);
        }
        [HttpGet("getTraineeInfo")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTraineeInfoAsync(Guid id)
        {
            var trainee = await _climateService.GetTraineeInfoByIdAsync(id);
            if (trainee == null)
            {
                return NotFound(new { message = "Trainee not found." });
            }

            return Ok(trainee);
        }

        private void MapTraineeViewModelToEntity(TraineeInfoViewModel vm, TraineeInfo entity)
        {
            entity.FirstName = vm.FirstName;
            entity.LastName = vm.LastName;
            entity.Gender = vm.Gender;
            entity.DateOfBirth = vm.DateOfBirth;
            entity.Nationality = vm.Nationality;
            entity.Email = vm.Email;
            entity.MobilePhone = vm.MobilePhone;
            entity.City = vm.City;
            entity.Country = vm.Country;
            entity.Organisation = vm.Organisation;
            entity.OrganisationType = vm.OrganisationType;
            entity.JobTitle = vm.JobTitle;

            entity.PaymentOrganisationName = vm.PaymentOrganisationName;
            entity.PaymentContactPerson = vm.PaymentContactPerson;
            entity.PaymentGender = vm.PaymentGender;
            entity.PaymentAddress = vm.PaymentAddress;
            entity.PaymentZipCode = vm.PaymentZipCode;
            entity.PaymentCity = vm.PaymentCity;
            entity.PaymentCountry = vm.PaymentCountry;
            entity.PaymentEmail = vm.PaymentEmail;
            entity.PaymentMobilePhone = vm.PaymentMobilePhone;

            entity.IsEligibleForDiscount = vm.IsEligibleForDiscount;
            entity.RequiresVisa = vm.RequiresVisa;
            entity.HasParticipationLimitation = vm.HasParticipationLimitation;
            entity.ParticipationLimitationDetails = vm.ParticipationLimitationDetails;
            entity.AcceptsTermsAndConditions = vm.AcceptsTermsAndConditions;
            entity.SubscribeToNewsletter = vm.SubscribeToNewsletter;
        }


        [HttpPost("sentEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReisterEmployee([FromBody] EmailViewModel email)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }


            var response = await _emailService.SendEmailAsync(email);

            return Ok(new
            {
                message = "Email is sent successfully",
                emailResponse = response

            });
        }



    }
}

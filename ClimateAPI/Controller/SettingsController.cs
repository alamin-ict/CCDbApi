using CCDbApi.Model;
using CCDbApi.Service;
using CCDbApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CCDbApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        public SettingsController(ISettingsService settings)
        {
            _settingsService = settings;
        }

        // GET: api/General
        [HttpGet("getGeneralSetting")]
 
        public async Task<ActionResult<GeneralSettings>> getGeneral()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid GeneralSetting data.",
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
                var tag = new GeneralSettings();
                tag = await _settingsService.GetGeneralSettingsAsync(userId);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // POST: api/GeneralSettings
        [HttpPost("addOrUpdateGeneralSettings")]
        public async Task<ActionResult<GeneralSettings>> addGeneral(CreateGeneralSettingsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid GeneralSettings data.",
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
                var tag = new GeneralSettings();
                tag = await _settingsService.GetGeneralSettingsAsync(userId);
                if (tag == null)
                {
                    tag = new GeneralSettings()
                    {
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        AdminEmail = dto.AdminEmail,
                        PostsPerPage = dto.PostsPerPage,
                        SiteTitle = dto.SiteTitle,
                        SiteUrl = dto.SiteUrl,
                        Tagline = dto.Tagline,
                        UserId = userId
                    };

                    tag = await _settingsService.AddGeneralSettingsAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to add GeneralSettings data");
                    }
                }
                else
                {
                    tag.AdminEmail = dto.AdminEmail;
                    tag.PostsPerPage = dto.PostsPerPage;
                    tag.SiteTitle = dto.SiteTitle;
                    tag.SiteUrl = dto.SiteUrl;
                    tag.Tagline = dto.Tagline;
                    tag.UpdatedBy = userId;
                    tag.UpdatedDate = DateTime.Now;
                    tag = await _settingsService.UpdateGeneralSettingsAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to update GeneralSettings data");
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


        // GET: api/Appearance
        [HttpGet("getAppearance")]
 
        public async Task<ActionResult<Appearance>> getAppearance()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Appearance data.",
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
                var tag = new Appearance();
                tag = await _settingsService.GetAppearanceAsync(userId);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // POST: api/Appearance
        [HttpPost("addOrUpdateAppearance")]
        public async Task<ActionResult<Appearance>> addAppearance(CreateAppearanceDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Appearance data.",
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
                var tag = new Appearance();
                tag = await _settingsService.GetAppearanceAsync(userId);
                if (tag == null)
                {
                    tag = new Appearance()
                    {
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        DonateButtonLabel = dto.DonateButtonLabel,
                        DonateUrl = dto.DonateUrl,
                        FooterTagline = dto.FooterTagline,
                        HeroOverleyOpacity = dto.HeroOverleyOpacity,
                        IsShowGalleryOnHomePage = dto.IsShowGalleryOnHomePage,

                        UserId = userId
                    };

                    tag = await _settingsService.AddAppearanceAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to add Appearance data");
                    }
                }
                else
                {
                    tag.DonateButtonLabel = dto.DonateButtonLabel;
                    tag.DonateUrl = dto.DonateUrl;
                    tag.FooterTagline = dto.FooterTagline;
                    tag.HeroOverleyOpacity = dto.HeroOverleyOpacity;
                    tag.IsShowGalleryOnHomePage = dto.IsShowGalleryOnHomePage;
                    tag.UpdatedBy = userId;
                    tag.UpdatedDate = DateTime.Now;
                    tag = await _settingsService.UpdateAppearanceAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to update Appearance data");
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

        // GET: api/SocialContact
        [HttpGet("getSocialContact")]
      
        public async Task<ActionResult<SocialContact>> getSocialContact()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid SocialContact data.",
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
                var tag = new SocialContact();
                tag = await _settingsService.GetSocialContactAsync(userId);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // POST: api/SocialContact
        [HttpPost("addOrUpdateSocialContact")]
        public async Task<ActionResult<SocialContact>> addSocialContact(CreateSocialContactDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid SocialContact data.",
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
                var tag = new SocialContact();
                tag = await _settingsService.GetSocialContactAsync(userId);
                if (tag == null)
                {
                    tag = new SocialContact()
                    {
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        Address = dto.Address,
                        Email = dto.Email,
                        FacebookUrl = dto.FacebookUrl,
                        GithubUrl = dto.GithubUrl,
                        InstragramUrl = dto.InstragramUrl,
                        LinkedInUrl = dto.LinkedInUrl,
                        Phone = dto.Phone,
                        TwitterUrl = dto.TwitterUrl,
                        YoutubeUrl = dto.YoutubeUrl,
                        UserId = userId
                    };

                    tag = await _settingsService.AddSocialContactAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to add SocialContact data");
                    }
                }
                else
                {
                    tag.Address = dto.Address;
                    tag.Email = dto.Email;
                    tag.FacebookUrl = dto.FacebookUrl;
                    tag.GithubUrl = dto.GithubUrl;
                    tag.InstragramUrl = dto.InstragramUrl;
                    tag.LinkedInUrl = dto.LinkedInUrl;
                    tag.Phone = dto.Phone;
                    tag.TwitterUrl = dto.TwitterUrl;
                    tag.YoutubeUrl = dto.YoutubeUrl;
                    tag.UpdatedBy = userId;
                    tag.UpdatedDate = DateTime.Now;
                    tag = await _settingsService.UpdateSocialContactAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to update SocialContact data");
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

        // GET: api/Sliders
        [HttpGet("getAllSliders")]
        [AllowAnonymous]
        public async Task<ActionResult<Slider>> getAllSlider()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Slider data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                //var user = HttpContext.User;
                //// Optionally retrieve user ID if needed
                //var userId = user.FindFirst("Id")?.Value;
                //var email = user.FindFirst("Email")?.Value;
                var Sliders = new List<Slider>();
                Sliders = await _settingsService.GetAllSlidersAsync();
                return Ok(Sliders);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // GET: api/Slider
        [HttpGet("getSlider/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Slider>> getSlider(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Slider data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                //// Retrieve user from context
                //var user = HttpContext.User;
                //// Optionally retrieve user ID if needed
                //var userId = user.FindFirst("Id")?.Value;
                //var email = user.FindFirst("Email")?.Value;
                var Slider = new Slider();
                Slider = await _settingsService.GetSliderAsync(id);
                return Ok(Slider);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // POST: api/Slider
        [HttpPost("addOrUpdateSlider")]
        public async Task<ActionResult<Slider>> addSlider(CreateSliderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Slider data.",
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
                var tag = new Slider();

                if (dto.Id == null)
                {
                    tag = new Slider()
                    {
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        CTALabel = dto.CTALabel,
                        CTAUrl = dto.CTAUrl,
                        Heading = dto.Heading,
                        ImageUrl = dto.ImageUrl,
                        Subheading = dto.Subheading,
                        UserId = userId
                    };

                    tag = await _settingsService.AddSliderAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to add Slider data");
                    }
                }
                else
                {
                    tag = await _settingsService.GetSliderAsync(dto.Id);
                    tag.CTALabel = dto.CTALabel;
                    tag.CTAUrl = dto.CTAUrl;
                    tag.Heading = dto.Heading;
                    tag.ImageUrl = dto.ImageUrl;
                    tag.Subheading = dto.Subheading;
                    tag.UpdatedBy = userId;
                    tag.UpdatedDate = DateTime.Now;
                    tag = await _settingsService.UpdateSliderAsync(tag);
                    if (tag == null)
                    {
                        return BadRequest("Failed to update Slider data");
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

        // GET: api/deleteSlider
        [HttpDelete("deleteSlider/{id}")]
        public async Task<ActionResult<Slider>> deleteSlider(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Slider data.",
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
                var Slider = new Slider();
                Slider = await _settingsService.GetSliderAsync(id);
                if (Slider == null)
                {
                    return NotFound("No such slider found with this id");
                }
                Slider = await _settingsService.DeleteSliderAsync(Slider);
                if (Slider == null)
                {
                    return BadRequest("Failed to delete slider");
                }
                return Ok("Success fully deleted the slider");
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

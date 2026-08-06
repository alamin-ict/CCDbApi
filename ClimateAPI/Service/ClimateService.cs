using CCDbApi.Model;
using CCDbApi.Repository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CCDbApi.Service
{
    public interface IClimateService
    {
       Task<User> GetUserByIdAsync(string id);  
        //
       Task<User> InsertIntoDbUserAsync(User user);
        Task<TrainingInfo> InsertIntoDbTrainingInfoAsync(TrainingInfo training);
        Task<TrainingInfo> GetTrainingInfoByIdAsync(Guid id);
        Task<TrainingInfo> UpdateIntoDbTrainingInfoAsync(TrainingInfo training);
        Task<TrainingInfo> DeleteTrainingInfoDataFromDB(TrainingInfo partner);
        Task<List<TrainingInfo>> GetAllTrainingInfoAsync();
        Task<Role> GetUserRoleByIdAsync(string id); 
        Task<Role> InsertIntoDbRoleAsync(Role role);
        Task<List<Role>> GetDbRoleAsync();

        Task<Contact> GetContactByIdAsync(string id);
        Task<Contact> InsertIntoDbContactAsync(Contact contact);  
        Task<Partner> InsertIntoDbPartnerAsync(Partner partner);
        Task<NewsContent> InsertIntoDbNewsContentAsync(NewsContent newsContent);    
        Task<ImageConfiguration> AddImageConfigurationAsync(ImageConfiguration imageConfiguration); 
        Task<SliderImage> AddSliderImageAsync(SliderImage sliderImage);
        Task<string> GetToken(User user);
        Task<User> GetUserAsync(string name, string password);
        public string GenerateSecretKey();
        Task<Partner> GetPartnerByIdAsync(Guid id);
        Task<Partner> DeletedPartnerDataFromDB(Partner partner);
        Task<Partner> UpdatedDataIntoDbPartnerAsync(Partner partner);
        Task<List<Partner>> GetAllPartnersOrClientsAsync(string userId);
        Task<List<Partner>> GetAllPartnersOrClients();
        Task<User> GetUserByAsync(string userName, string password, string email, string userRole);
        Task<List<ImageConfiguration>> GetImageConfigurationsAsync(string userId);    
        Task<List<SliderImage>> GetSliderImagesAsync(string userId);  
        Task<List<NewsContent>> GetAllNewsContentAsync(string userId);
        Task<List<Subscribe>> GetAllSubscribeAsync();
        Task<Subscribe> AddSubscribeAsync(Subscribe sub);
        Task<List<TraineeInfo>> GetAllTraineeInfoAsync();
        Task<TraineeInfo> GetTraineeInfoByIdAsync(Guid id);
        Task<TraineeInfo> InsertTraineeInfoAsync(TraineeInfo trainee);
        Task<TraineeInfo> UpdateTraineeInfoAsync(TraineeInfo trainee);
        Task<TraineeInfo> DeleteTraineeInfoAsync(TraineeInfo trainee);
    }

    public class ClimateService : IClimateService {

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IImageConfigurationRepository _imageConfigurationRepository;
        private readonly INewsContentRepository _newsContentPartnerRepository;
        private readonly IPartnerRepository _partnerReository;
        private readonly ISliderImageRepository _sliderImageRepository;
        private readonly IConfiguration _configuration;
        private readonly ISubscribeRepository _subscribeRepository; 
        private readonly ITrainingInfoRepository _trainingInfoRepository;
        private readonly ITraineeInfoRepository _traineeInfoRepository;
        public ClimateService(IConfiguration configuration,IUserRepository userRepository, IRoleRepository roleRepository, IContactRepository contactRepository,
           IImageConfigurationRepository imageConfigurationRepository, INewsContentRepository newsContentPartnerRepository,
           IPartnerRepository partnerReository, ISliderImageRepository sliderImageRepository,ISubscribeRepository sub,
           ITrainingInfoRepository trainingInfoRepository,ITraineeInfoRepository traineeInfoRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _contactRepository = contactRepository;
            _imageConfigurationRepository = imageConfigurationRepository;
            _newsContentPartnerRepository = newsContentPartnerRepository;
            _partnerReository = partnerReository;
            _sliderImageRepository = sliderImageRepository;
            _configuration = configuration;
            _subscribeRepository = sub;
            _trainingInfoRepository = trainingInfoRepository;
            _traineeInfoRepository = traineeInfoRepository;
        }
        public string GenerateSecretKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var keyBytes = new byte[32]; // 32 bytes = 256 bits
                rng.GetBytes(keyBytes);
                return Convert.ToBase64String(keyBytes);
            }
        }
        public async Task<User> GetUserByIdAsync(string id)
        {
            var users= await _userRepository.FindAsync(a=>a.Id==Guid.Parse(id)); 
            return users.FirstOrDefault();
        }
        public async Task<List<Partner>> GetAllPartnersOrClientsAsync(string userId)
        {
            var partners=await _partnerReository.FindAsync(a=>a.UserId==userId);    
            return partners.ToList();   
        }
        public async Task<List<Role>> GetDbRoleAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            if(roles == null)
            {
                return new List<Role>();
            }   
            return roles.ToList();
        }
        public async Task<List<Partner>> GetAllPartnersOrClients()
        {
            var partners = await _partnerReository.GetAllAsync();
            return partners.ToList();
        }
        public async Task<User> InsertIntoDbUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
          
            return user;
        }
        public async Task<User> GetUserAsync(string name, string password)
        {
            var users=await _userRepository.FindAsync(a=>(a.UserName==name||a.Email==name)&&a.Password==password);
            if (users.Any())
            {
                return users.FirstOrDefault();
            }
            return null;
        }
        public async Task<User> GetUserByAsync(string userName, string password, string email, string userRole)
        {
            // Query the database to find the user based on the provided criteria
            var user = await _userRepository.FindAsync(u =>
                    u.UserName == userName &&
                    u.Password == password &&
                    u.Email == email &&
                    u.RoleId == userRole);
            if (user.Any())
            {

                return user.FirstOrDefault(); // Return the found user or null if no match is found

            }
            return null;
        }

        public async  Task<Role> GetUserRoleByIdAsync(string name)
        {
            var role=await _roleRepository.FindAsync(a=>a.Name==name);
            return role.FirstOrDefault();
        }
       public async Task<Role> InsertIntoDbRoleAsync(Role role)
        {
            await _roleRepository.AddAsync(role);
            return role;    
        }
        public async Task<Contact> GetContactByIdAsync(string id)
        {
            var contact=await _contactRepository.FindAsync(a=>a.UserId == id);
            return contact.FirstOrDefault();
        }
        public async Task<Partner> GetPartnerByIdAsync(Guid id)
        {
            var partners = await _partnerReository.FindAsync(a => a.Id == id);
            return partners.FirstOrDefault();
        }
        public async Task<Partner> DeletedPartnerDataFromDB(Partner partner)
        {
            await _partnerReository.RemoveAsync(partner);
            return partner; 
        }
        public  async Task<Contact> InsertIntoDbContactAsync(Contact contact)
        {
            await _contactRepository.AddAsync(contact); 
            return contact;
        }
        public async Task<Partner> InsertIntoDbPartnerAsync(Partner partner)
        {
            await _partnerReository.AddAsync(partner);  
            return partner; 
        }
        public async Task<Partner> UpdatedDataIntoDbPartnerAsync(Partner partner)
        {
            await _partnerReository.UpdateAsync(partner);   
            return partner; 
        }

        public async Task<NewsContent> InsertIntoDbNewsContentAsync(NewsContent newsContent)
        {
            await _newsContentPartnerRepository.AddAsync(newsContent);
            return newsContent; 
        }
        public async Task<ImageConfiguration> AddImageConfigurationAsync(ImageConfiguration imageConfiguration)
        {
            await _imageConfigurationRepository.AddAsync(imageConfiguration);
            return imageConfiguration;  
        }
        public async Task<SliderImage> AddSliderImageAsync(SliderImage sliderImage)
        {
            await _sliderImageRepository.AddAsync(sliderImage);
            return sliderImage; 
        }
        //public async Task<string> GetToken(User user, string AppSecret)
        //{


        //    var key = Encoding.ASCII.GetBytes(AppSecret);
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //            new Claim(ClaimTypes.Name,user.UserName)
        //        }),
        //        Claims = new Dictionary<string, object>(),
        //        Expires = DateTime.UtcNow.AddHours(96),
        //        Audience = "your-audience-here",  // Set your audience here
        //        Issuer = "your-issuer-here",  // Set your issuer here
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    tokenDescriptor.Claims.Add("Id", user.Id.ToString());
        //    tokenDescriptor.Claims.Add("Role", user.UserRole);
        //    tokenDescriptor.Claims.Add("UserName", user.UserName);
        //    //tokenDescriptor.Claims.Add("FirstName", user.FirstName);
        //    //tokenDescriptor.Claims.Add("LastName", user.LastName);
        //    tokenDescriptor.Claims.Add("Email", user.Email);
        //    tokenDescriptor.Claims.Add("Password", user.Password);

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token);
        //    return tokenString;
        //}

        //public async Task<string> GetToken(User user)
        //{
        //    // Convert AppSecret to bytes
        //    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:AppSecret"]);
        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    // Create token descriptor
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //    new Claim(ClaimTypes.Name, user.UserName)
        //}),
        //        Claims = new Dictionary<string, object>
        //{
        //    { "Id", user.Id.ToString() }, // Convert Guid to string
        //    { "Role", user.UserRole },   // Ensure UserRole is a string
        //    { "UserName", user.UserName },
        //    { "Email", user.Email }

        //},
        //        Expires = DateTime.UtcNow.AddHours(96),
        //        Audience = _configuration["Jwt:Audience"], // Set your audience here
        //        Issuer = _configuration["Jwt:Issuer"],     // Set your issuer here
        //        SigningCredentials = new SigningCredentials(
        //            new SymmetricSecurityKey(key),
        //            SecurityAlgorithms.HmacSha256Signature
        //        )
        //    };

        //    // Create and return the token
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}
        public async Task<string> GetToken(User user)
        {
            //var key = Encoding.UTF8.GetBytes(_configuration["Jwt:AppSecret"]);
            //var tokenHandler = new JwtSecurityTokenHandler();

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.Name, user.UserName ?? ""),
            //    new Claim(ClaimTypes.Email, user.Email ?? ""),
            //    new Claim(ClaimTypes.Role, user.UserRole ?? "")
            //};

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddHours(96),
            //    Audience = _configuration["Jwt:Audience"],
            //    Issuer = _configuration["Jwt:Issuer"],
            //    SigningCredentials = new SigningCredentials(
            //        new SymmetricSecurityKey(key),
            //        SecurityAlgorithms.HmacSha256
            //    )
            //};

            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:AppSecret"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString())
                }),
                Claims = new Dictionary<string, object>(),
                Expires = DateTime.UtcNow.AddHours(96),
                Audience = "your-audience-here",  // Set your audience here
                Issuer = "your-issuer-here",  // Set your issuer here
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            tokenDescriptor.Claims.Add("Id", user.Id.ToString());
            tokenDescriptor.Claims.Add("UserName", user.Id.ToString());
            tokenDescriptor.Claims.Add("Email", user.Email);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;

        }

        public async  Task<List<ImageConfiguration>> GetImageConfigurationsAsync(string userId)
        {
            var imageConfigurations = await _imageConfigurationRepository.FindAsync(a=>a.UserId==userId);
            if (imageConfigurations == null)
            {
                return new List<ImageConfiguration>();  
            }
            return imageConfigurations.ToList();
        }
       public  async Task<List<SliderImage>> GetSliderImagesAsync(string userId)
        {
            var sliderImages = await _sliderImageRepository.FindAsync(a => a.UserId == userId);
            if (sliderImages == null)
            {
                return new List<SliderImage>();
            }
            return sliderImages.ToList();
        }
       public async Task<List<NewsContent>> GetAllNewsContentAsync(string userId)
        {
            var newsContents = await _newsContentPartnerRepository.FindAsync(a => a.UserId == userId);
            if (newsContents == null)
            {
                return new List<NewsContent>();
            }
            return newsContents.ToList();
        }

        public async Task<List<Subscribe>> GetAllSubscribeAsync()
        {
            try
            {
                var subscribes = await _subscribeRepository.GetAllAsync();
                return subscribes.ToList();
            }
            catch (Exception ex) { return null; }
           }

        public async Task<Subscribe> AddSubscribeAsync(Subscribe sub)
        {
            try
            {
                await _subscribeRepository.AddAsync(sub);
                return sub;
            }
            catch(Exception ex)
            {
                return null;
            }

        }

        public async  Task<TrainingInfo> InsertIntoDbTrainingInfoAsync(TrainingInfo training)
        {
            try
            {
                await _trainingInfoRepository.AddAsync(training);
                return training;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<TrainingInfo> GetTrainingInfoByIdAsync(Guid id)
        {
            try
            {
                var data=await _trainingInfoRepository.FindAsync(a=>a.Id==id);
                if(data == null)
                {
                    return null;
                }
                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<TrainingInfo> UpdateIntoDbTrainingInfoAsync(TrainingInfo training)
        {
            try
            {
                await _trainingInfoRepository.UpdateAsync(training);
                return training;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<TrainingInfo> DeleteTrainingInfoDataFromDB(TrainingInfo training)
        {
            try
            {
                await _trainingInfoRepository.RemoveAsync(training);
                return training;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<List<TrainingInfo>> GetAllTrainingInfoAsync()
        {
            try
            {
                var training=await _trainingInfoRepository.GetAllAsync();
                if(training == null)
                {
                    return null;
                }
                return training.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async  Task<List<TraineeInfo>> GetAllTraineeInfoAsync()
        {
            try
            {
                var training = await _traineeInfoRepository.GetAllAsync();
                if (training == null)
                {
                    return null;
                }
                return training.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<TraineeInfo> GetTraineeInfoByIdAsync(Guid id)
        {
            try
            {
                var training = await _traineeInfoRepository.FindAsync(a=>a.Id==id);
                if (training == null)
                {
                    return null;
                }
                return training.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<TraineeInfo> InsertTraineeInfoAsync(TraineeInfo trainee)
        {
            try
            {
                var training = await _traineeInfoRepository.AddAsync(trainee);
                
                return trainee;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<TraineeInfo> UpdateTraineeInfoAsync(TraineeInfo trainee)
        {
            try
            {
                var training = await _traineeInfoRepository.UpdateAsync(trainee);

                return trainee;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async  Task<TraineeInfo> DeleteTraineeInfoAsync(TraineeInfo trainee)
        {
            try
            {
                await _traineeInfoRepository.RemoveAsync(trainee);

                return trainee;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


}

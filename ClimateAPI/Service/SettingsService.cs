using CCDbApi.Model;
using CCDbApi.Repository;

namespace CCDbApi.Service
{
    public interface ISettingsService
    {
        // General Settings
        Task<GeneralSettings> AddGeneralSettingsAsync(GeneralSettings settings);
        Task<GeneralSettings> UpdateGeneralSettingsAsync(GeneralSettings settings);
        Task<GeneralSettings> DeleteGeneralSettingsAsync(GeneralSettings settings);
        Task<GeneralSettings> GetGeneralSettingsAsync();
        Task<List<GeneralSettings>> GetAllGeneralSettingsAsync();
        // Appearance
        Task<Appearance> AddAppearanceAsync(Appearance appearance);
        Task<Appearance> UpdateAppearanceAsync(Appearance appearance);
        Task<Appearance> DeleteAppearanceAsync(Appearance appearance);
        Task<Appearance> GetAppearanceAsync();
        Task<List<Appearance>> GetAllAppearancesAsync();


        // Slider
        Task<Slider> AddSliderAsync(Slider slider);
        Task<Slider> UpdateSliderAsync(Slider slider);
        Task<Slider> DeleteSliderAsync(Slider slider);
        Task<Slider> GetSliderAsync(string id);
        Task<List<Slider>> GetAllSlidersAsync();


        // Comment
        Task<Comment> AddCommentAsync(Comment comment);
        Task<Comment> UpdateCommentAsync(Comment comment);
        Task<Comment> DeleteCommentAsync(Comment comment);
        Task<Comment> GetCommentAsync(string id);
        Task<List<Comment>> GetAllCommentsAsync();


        // Social Contact
        Task<SocialContact> AddSocialContactAsync(SocialContact socialContact);
        Task<SocialContact> UpdateSocialContactAsync(SocialContact socialContact);
        Task<SocialContact> DeleteSocialContactAsync(SocialContact socialContact);
        Task<SocialContact> GetSocialContactAsync(string id);
        Task<List<SocialContact>> GetAllSocialContactsAsync();


        // Media
        Task<Media> AddMediaAsync(Media media);
        Task<Media> UpdateMediaAsync(Media media);
        Task<Media> DeleteMediaAsync(Media media);
        Task<Media> GetMediaAsync(string id);
        Task<List<Media>> GetAllMediasAsync();
    }
    public class SettingsService : ISettingsService
    {
        private readonly IGeneralSettingsRepository _generalSettingsRepo;
        private readonly IAppearanceRepository _appearanceRepo;
        private readonly ISliderRepository _sliderRepo;
        private readonly ISocialContactRepository _socialContactRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IMediaRepository _mediaRepo;
        public SettingsService(IGeneralSettingsRepository generalSettings,
            IAppearanceRepository appearanceRepo, ISliderRepository sliderRepository,
            ISocialContactRepository socialContactRepo, ICommentRepository commentRepo,
            IMediaRepository mediaRepo)
        {
            _generalSettingsRepo = generalSettings;
            _appearanceRepo = appearanceRepo;
            _sliderRepo = sliderRepository;
            _socialContactRepo = socialContactRepo;
            _commentRepo = commentRepo;
            _mediaRepo = mediaRepo;
        }
        // Media

        public async Task<Media> AddMediaAsync(Media media)
        {
            var result = await _mediaRepo.AddAsync(media);

            return result == 1 ? media : null;
        }


        public async Task<Media> UpdateMediaAsync(Media media)
        {
            var result = await _mediaRepo.UpdateAsync(media);

            return result == 1 ? media : null;
        }


        public async Task<Media> DeleteMediaAsync(Media media)
        {
            var result = await _mediaRepo.RemoveAsync(media);

            return result == 1 ? media : null;
        }


        public async Task<Media> GetMediaAsync(string id)
        {
            var data = await _mediaRepo.FindAsync(
                x => x.Id.ToString() == id
            );

            return data.FirstOrDefault();
        }


        public async Task<List<Media>> GetAllMediasAsync()
        {
            var data = await _mediaRepo.GetAllAsync();

            return data?.ToList();
        }
        // Social Contact

        public async Task<SocialContact> AddSocialContactAsync(
            SocialContact socialContact)
        {
            var result = await _socialContactRepo.AddAsync(socialContact);

            return result == 1 ? socialContact : null;
        }


        public async Task<SocialContact> UpdateSocialContactAsync(
            SocialContact socialContact)
        {
            var result = await _socialContactRepo.UpdateAsync(socialContact);

            return result == 1 ? socialContact : null;
        }


        public async Task<SocialContact> DeleteSocialContactAsync(
            SocialContact socialContact)
        {
            var result = await _socialContactRepo.RemoveAsync(socialContact);

            return result == 1 ? socialContact : null;
        }


        public async Task<SocialContact> GetSocialContactAsync(string id)
        {
            var data = await _socialContactRepo.FindAsync(
                x => x.UserId == id
            );

            return data.FirstOrDefault();
        }


        public async Task<List<SocialContact>> GetAllSocialContactsAsync()
        {
            var data = await _socialContactRepo.GetAllAsync();

            return data?.ToList();
        }
        // Comment

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            var result = await _commentRepo.AddAsync(comment);

            return result == 1 ? comment : null;
        }


        public async Task<Comment> UpdateCommentAsync(Comment comment)
        {
            var result = await _commentRepo.UpdateAsync(comment);

            return result == 1 ? comment : null;
        }


        public async Task<Comment> DeleteCommentAsync(Comment comment)
        {
            var result = await _commentRepo.RemoveAsync(comment);

            return result == 1 ? comment : null;
        }


        public async Task<Comment> GetCommentAsync(string id)
        {
            var data = await _commentRepo.FindAsync(
                x => x.Id.ToString() == id
            );

            return data.FirstOrDefault();
        }


        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            var data = await _commentRepo.GetAllAsync();

            return data?.ToList();
        }

        // Slider

        public async Task<Slider> AddSliderAsync(Slider slider)
        {
            var result = await _sliderRepo.AddAsync(slider);

            return result == 1 ? slider : null;
        }


        public async Task<Slider> UpdateSliderAsync(Slider slider)
        {
            var result = await _sliderRepo.UpdateAsync(slider);

            return result == 1 ? slider : null;
        }


        public async Task<Slider> DeleteSliderAsync(Slider slider)
        {
            var result = await _sliderRepo.RemoveAsync(slider);

            return result == 1 ? slider : null;
        }


        public async Task<Slider> GetSliderAsync(string id)
        {
            var data = await _sliderRepo.FindAsync(
                x => x.Id.ToString() == id
            );

            return data.FirstOrDefault();
        }


        public async Task<List<Slider>> GetAllSlidersAsync()
        {
            var data = await _sliderRepo.GetAllAsync();

            return data?.ToList();
        }
        // Appearance

        public async Task<Appearance> AddAppearanceAsync(
            Appearance appearance)
        {
            var added = await _appearanceRepo.AddAsync(appearance);

            if (added == 1)
            {
                return appearance;
            }

            return null;
        }



        public async Task<Appearance> UpdateAppearanceAsync(
            Appearance appearance)
        {
            var updated = await _appearanceRepo.UpdateAsync(appearance);

            if (updated == 1)
            {
                return appearance;
            }

            return null;
        }



        public async Task<Appearance> DeleteAppearanceAsync(
            Appearance appearance)
        {
            var deleted = await _appearanceRepo.RemoveAsync(appearance);

            if (deleted == 1)
            {
                return appearance;
            }

            return null;
        }



        public async Task<Appearance> GetAppearanceAsync(
            )
        {
            var data = await _appearanceRepo.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            return data
        .OrderByDescending(x => x.CreatedDate)
        .FirstOrDefault();
        }



        public async Task<List<Appearance>> GetAllAppearancesAsync()
        {
            var data = await _appearanceRepo.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            return data.ToList();
        }
        // General Settings

        public async Task<GeneralSettings> AddGeneralSettingsAsync(
            GeneralSettings settings)
        {
            var added = await _generalSettingsRepo.AddAsync(settings);

            if (added == 1)
            {
                return settings;
            }

            return null;
        }



        public async Task<GeneralSettings> UpdateGeneralSettingsAsync(
            GeneralSettings settings)
        {
            var updated = await _generalSettingsRepo.UpdateAsync(settings);

            if (updated == 1)
            {
                return settings;
            }

            return null;
        }



        public async Task<GeneralSettings> DeleteGeneralSettingsAsync(
            GeneralSettings settings)
        {
            var deleted = await _generalSettingsRepo.RemoveAsync(settings);

            if (deleted == 1)
            {
                return settings;
            }

            return null;
        }



        public async Task<GeneralSettings> GetGeneralSettingsAsync()
        {
            var data = await _generalSettingsRepo.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            return data
         .OrderByDescending(x => x.CreatedDate)
         .FirstOrDefault();
        }



        public async Task<List<GeneralSettings>> GetAllGeneralSettingsAsync()
        {
            var data = await _generalSettingsRepo.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            return data.ToList();
        }

    }
}

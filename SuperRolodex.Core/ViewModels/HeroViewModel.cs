using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Robo.Mvvm.Input;
using Robo.Mvvm.Services;
using SuperRolodex.Core.Models;
using SuperRolodex.Core.Repositories;
using SuperRolodex.Core.Services;

namespace SuperRolodex.Core.ViewModels
{
    public class HeroViewModel : BaseNavigationViewModel
    {
        string HeroId { get; set; }

        string _alias;
        public string Alias
        {
            get => _alias;
            set => SetPropertyChanged(ref _alias, value);
        }

        string _name;
        public  string Name
        {
            get => _name;
            set => SetPropertyChanged(ref _name, value);
        }

        byte[] _imageData;
        public byte[] ImageData
        {
            get => _imageData;
            set => SetPropertyChanged(ref _imageData, value);
        }

        ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new Command(async () => await SaveHeroAsync());
                }
                
                return _saveCommand;
            }
        }

        ICommand _selectImageCommand;
        public ICommand SelectImageCommand
        {
            get
            {
                if (_selectImageCommand == null)
                {
                    _selectImageCommand = new Command(async () => await SelectImage());
                }

                return _selectImageCommand;
            }
        }

        IMediaService MediaService { get; set; }

        public HeroViewModel(INavigationService navigationService, IHeroRepository heroRepository,
                             IMediaService mediaService, Hero hero = null) : base(navigationService, heroRepository)
        {
            MediaService = mediaService;

            if (hero != null)
            {
                HeroId = hero.Id;
                Alias = hero.Alias;
                Name = hero.Name;
                ImageData = hero.ImageData;
            }
        }

        public Task SaveHeroAsync()
        {
            var hero = new Hero
            {
                Id = HeroId ?? Guid.NewGuid().ToString(),
                Alias = Alias,
                Name =  Name,
                ImageData = ImageData
            };

            HeroRepository.SaveHero(hero);

            return Navigation.PopAsync();
        }

        async Task SelectImage()
        {
            var imageData = await MediaService.PickPhotoAsync();

            if (imageData != null)
            {
                ImageData = imageData;
            }
        }
    }
}

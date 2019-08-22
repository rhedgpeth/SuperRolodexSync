using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Robo.Mvvm.Input;
using Robo.Mvvm.Services;
using SuperRolodex.Core.Models;
using SuperRolodex.Core.Repositories;
using SuperRolodex.Core.Services;

namespace SuperRolodex.Core.ViewModels
{
    public class HeroesViewModel : BaseNavigationViewModel
    { 
        ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new Command(async () => await Navigation.PushAsync<HeroViewModel>());
                }

                return _addCommand;
            }
        }

        ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new Command<Hero>(async (hero) =>
                    {
                        var heroViewModel = new HeroViewModel(Navigation, HeroRepository, MediaService, hero);
                        await Navigation.PushAsync(heroViewModel);
                    });
                }

                return _editCommand;
            }
        }

        ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new Command<Hero>((hero) => DeleteHero(hero));
                }

                return _deleteCommand;
            }
        }

        ICommand _toggleSyncStatusCommand;
        public ICommand ToggleSyncStatusCommand
        {
            get
            {
                if (_toggleSyncStatusCommand == null)
                {
                    _toggleSyncStatusCommand = new Command(ToggleSyncStatus);
                }

                return _toggleSyncStatusCommand;
            }
        }

        public string SyncStatusDescription
        {
            get
            {
                return SyncStatus ? "Online" : "Offline";
            }
        }

        bool _syncStatus;
        public bool SyncStatus
        {
            get => _syncStatus;
            set
            {
                SetPropertyChanged(ref _syncStatus, value);
                SetPropertyChanged(nameof(SyncStatusDescription));
            }
        }

        public List<Hero> _heroes;
        public List<Hero> Heroes
        {
            get => _heroes;
            set => SetPropertyChanged(ref _heroes, value);
        }

        IMediaService MediaService { get; set; }

        public HeroesViewModel(INavigationService navigationService, IHeroRepository heroRepository, IMediaService mediaService) : base(navigationService, heroRepository)
        {
            MediaService = mediaService;
        }

        public override async Task InitAsync()
        {
            IsBusy = true;

            HeroRepository.GetHeroes(HeroesUpdated);

            IsBusy = false;
        }

        void HeroesUpdated(List<Hero> heroes) => Heroes = heroes;

        void DeleteHero(Hero hero) => HeroRepository.DeleteHero(hero);

        void ToggleSyncStatus()
        {
            SyncStatus = !SyncStatus;

            if (SyncStatus)
            {
                HeroRepository.StartReplication();
            }
            else
            {
                HeroRepository.StopReplication();
            }
        }
    }
}

using Robo.Mvvm;
using Robo.Mvvm.Services;
using SuperRolodex.Core.Repositories;
using SuperRolodex.Core.Services;
using SuperRolodex.Core.ViewModels;
using SuperRolodex.Repositories;
using SuperRolodex.Services;
using Xamarin.Forms;

namespace SuperRolodex
{
    public partial class App : Application
    {
        INavigationService _navigationService;
        INavigationService NavigationService
        {
            get
            {
                if (_navigationService == null)
                {
                    _navigationService = new NavigationService();
                }

                return _navigationService;
            }
        }

        public App()
        {
            InitializeComponent();

            RegisterThangs();

            SetHomePage();
        }

        void RegisterThangs()
        {
            NavigationService.AutoRegister(GetType().Assembly);
            ServiceContainer.Register(NavigationService);

            ServiceContainer.Register<IHeroRepository>(() => new HeroRepository());
            ServiceContainer.Register<IMediaService>(() => new MediaService());
        }

        void SetHomePage() => NavigationService.SetRoot<HeroesViewModel>();
    }
}

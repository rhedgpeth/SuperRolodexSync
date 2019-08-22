using Robo.Mvvm.Services;
using Robo.Mvvm.ViewModels;
using SuperRolodex.Core.Repositories;

namespace SuperRolodex.Core.ViewModels
{
    public abstract class BaseNavigationViewModel : BaseViewModel
    {
        protected INavigationService Navigation { get; set; }
        protected IHeroRepository HeroRepository { get; set; }

        protected BaseNavigationViewModel(INavigationService navigationService, IHeroRepository heroRepository)
        {
            Navigation = navigationService;
            HeroRepository = heroRepository;
        }
    }
}

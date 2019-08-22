using Robo.Mvvm.Forms.Pages;
using SuperRolodex.Core.Models;
using SuperRolodex.Core.ViewModels;
using Xamarin.Forms;

namespace SuperRolodex.Pages
{
    public partial class HeroesPage : BaseContentPage<HeroesViewModel>
    {
        public HeroesPage()
        {
            InitializeComponent();
        }

        void Handle_Context_Clicked(object sender, System.EventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                var item = menuItem.CommandParameter as Hero;

                if (item != null)
                {
                    ViewModel.DeleteCommand.Execute(item);
                }
            }
        }
    }
}

using Laboratory2.Models;
using Laboratory2.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory2.ViewModels
{

    enum NavigationTypes
    {
        Form,
        Info
    }

    internal class NavigateViewModel : BaseNavigationViewModel<NavigationTypes>
    {
        private Person _person = new();
        public NavigateViewModel()
        {
            Navigate(NavigationTypes.Form);
        }

        protected override INavigatable<NavigationTypes> CreateViewModel(NavigationTypes type)
        {
            switch (type)
            {
                case NavigationTypes.Form:
                    return new FormViewModel(() => Navigate(NavigationTypes.Info), ref _person);
                case NavigationTypes.Info:
                    return new InfoViewModel(() => Navigate(NavigationTypes.Form), ref _person);
                default:
                    return null;
            }
        }
    }
}

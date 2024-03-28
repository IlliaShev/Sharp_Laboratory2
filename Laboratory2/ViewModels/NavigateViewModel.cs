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
        Info,
        List,
        Edit
    }

    internal class NavigateViewModel : BaseNavigationViewModel<NavigationTypes>
    {
        public static Person sharedPerson = new();
        public NavigateViewModel()
        {
            Navigate(NavigationTypes.List);
        }

        protected override INavigatable<NavigationTypes> CreateViewModel(NavigationTypes type)
        {
            switch (type)
            {
                case NavigationTypes.Form:
                case NavigationTypes.Edit:
                    return new FormViewModel(() => Navigate(NavigationTypes.Info), () => Navigate(NavigationTypes.List));
                case NavigationTypes.Info:
                    return new InfoViewModel(() => Navigate(NavigationTypes.List));
                case NavigationTypes.List:
                    return new ListViewModel(() => Navigate(NavigationTypes.Form), () => Navigate(NavigationTypes.Info));
                default:
                    return null;
            }
        }
    }
}

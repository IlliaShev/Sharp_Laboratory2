using Laboratory2.Models;
using Laboratory2.Navigation;
using Laboratory2.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Laboratory2.ViewModels
{
    internal class FormViewModel : INavigatable<NavigationTypes>
    {

        private Person _person;
        private RelayCommand<object> _closeCommand;
        private RelayCommand<object> _proceedCommand;
        private readonly Action _navigateToInfo;
        public FormViewModel(Action navigateToInfo, ref Person person)
        {
            _navigateToInfo = navigateToInfo;
            _person = person;
        }

        public string FirstName
        {
            get {  return _person.FirstName; }
            set {  _person.FirstName = value; }
        }

        public string LastName
        {
            get { return _person.LastName; }
            set { _person.LastName = value; }
        }

        public string? Email
        {
            get { return _person.Email; }
            set { _person.Email = value; }
        }

        public DateTime? Birthday
        {
            get { return _person.Birthday; }
            set { _person.Birthday = value; }
        }

        public RelayCommand<object> ProceedCommand
        {
            get
            {
                return _proceedCommand ??= new RelayCommand<object>(_ => Proceed(), CanExecute);
            }
        }
        public RelayCommand<object> CloseCommand
        {
            get
            {
                return _closeCommand ??= new RelayCommand<object>(_ => Environment.Exit(0));
            }
        }
        private bool CanExecute(object o)
        {
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(Email) &&
                    Birthday != null;
        }

        private async void Proceed()
        {
            try
            {
                await Task.Run(() => _person.Proceed());
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            _navigateToInfo.Invoke();
        }

        public NavigationTypes ViewType => NavigationTypes.Form;
    }
}

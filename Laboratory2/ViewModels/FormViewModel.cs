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
    internal class FormViewModel : INavigatable<NavigationTypes>, INotifyPropertyChanged
    {

        private Person _person;
        private RelayCommand<object> _goToPersonListCommand;
        private RelayCommand<object> _saveCommand;
        private readonly Action _navigateToInfo;
        private readonly Action _navigateToPersonList;
        private PersonRepository _personRepository;
        private bool _isEnabledInterface = true;

        public event PropertyChangedEventHandler? PropertyChanged;

        public FormViewModel(Action navigateToInfo, Action navigateToPersonList)
        {
            _navigateToInfo = navigateToInfo;
            _navigateToPersonList = navigateToPersonList;
            _person = NavigateViewModel.sharedPerson;
            _personRepository = PersonRepository.Instance;
        }

        public bool IsEnabledInterface
        {
            get { return _isEnabledInterface; }
            set 
            {
                _isEnabledInterface = value;
                OnPropertyChanged(nameof(IsEnabledInterface));
            }
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

        public DateTime Birthday
        {
            get { return _person.Birthday; }
            set { _person.Birthday = value; }
        }

        public RelayCommand<object> SaveCommand
        {
            get
            {
                return _saveCommand ??= new RelayCommand<object>(_ => Proceed(), CanExecute);
            }
        }
        public RelayCommand<object> GoToPersonListCommand
        {
            get
            {
                return _goToPersonListCommand ??= new RelayCommand<object>(_ => _navigateToPersonList.Invoke());
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
                IsEnabledInterface = false;
                await Task.Run(() => _person.Proceed());
                IsEnabledInterface = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                IsEnabledInterface = true;
                return;
            }
            _personRepository.UpsertPerson(_person);
            _navigateToInfo.Invoke();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NavigationTypes ViewType => NavigationTypes.Form;
    }
}

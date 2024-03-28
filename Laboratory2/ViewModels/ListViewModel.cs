using Laboratory2.Models;
using Laboratory2.Navigation;
using Laboratory2.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory2.ViewModels
{
    internal class ListViewModel: INavigatable<NavigationTypes>, INotifyPropertyChanged
    {
        private readonly Action _navigateToForm;
        private readonly Action _navigateToInfo;
        private PersonRepository _personRepository;
        private RelayCommand<object> _goToFormCommand;
        private RelayCommand<object> _goToEditCommand;
        private RelayCommand<object> _goToInfoCommand;
        private RelayCommand<object> _closeCommand;
        private RelayCommand<object> _removePersonCommand;
        private ObservableCollection<Person> _people;
        private ObservableCollection<Person> _gridPeople;
        private bool _isAdultFilter;
        private bool _isBirthdayFilter;
        private bool _isEnabledInterface = true;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ListViewModel(Action navigateToForm, Action navigateToInfo)
        {
            _navigateToForm = navigateToForm;
            _navigateToInfo = navigateToInfo;
            _personRepository = PersonRepository.Instance;
            _gridPeople = new ObservableCollection<Person>(_personRepository.PersonList());
            _people = new ObservableCollection<Person>(_gridPeople);
        }

        public ObservableCollection<Person> GridPeople
        {
            get
            {
                return _gridPeople;
            }
            set
            {
                _gridPeople = value;
                OnPropertyChanged(nameof(GridPeople));

            }
        }
        public Person SelectedPerson { get; set; }

        public bool IsAdultFilter
        {
            get
            {
                return _isAdultFilter;
            }
            set
            {
                _isAdultFilter = value;
                filterPeople();
            }
        }

        public bool IsBirthdayFilter
        {
            get
            {
               return _isBirthdayFilter;
            }
            set
            {
                _isBirthdayFilter = value;
                filterPeople();
            }
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

        public RelayCommand<object> GoToFormCommand
        {
            get
            {
                return _goToFormCommand = new RelayCommand<object>(_ => {
                    NavigateViewModel.sharedPerson = new();
                    _navigateToForm.Invoke();
                    });
            }
        }

        public RelayCommand<object> GoToEditCommand
        {
            get
            {
                return new RelayCommand<object>(_ => {
                    if (SelectedPerson != null)
                    {
                        NavigateViewModel.sharedPerson = SelectedPerson;
                        _navigateToForm.Invoke();
                    }
                }, IsPersonSelected);
            }
        }

        public RelayCommand<object> GoToInfoCommand
        {
            get
            {
                return _goToInfoCommand = new RelayCommand<object>(_ => GoToInfoPerson(), IsPersonSelected);
            }
        }

        public RelayCommand<object> RemovePersonCommand
        {
            get
            {
                return _removePersonCommand = new RelayCommand<object>(o => RemovePerson(), IsPersonSelected);
            }
        }

        public RelayCommand<object> CloseCommand
        {
            get
            {
                return _closeCommand ??= new RelayCommand<object>(_ => Environment.Exit(0));
            }
        }


        private void GoToInfoPerson()
        {
            if (SelectedPerson != null)
            {
                NavigateViewModel.sharedPerson = SelectedPerson;
                _navigateToInfo.Invoke();
            }
        }

        private async Task RemovePerson()
        {
            if (SelectedPerson != null)
            {
                IsEnabledInterface = false;
                await Task.Run(() => {
                    _personRepository.RemovePerson(SelectedPerson.Id);
                    _people = new ObservableCollection<Person>(_personRepository.PersonList());
                    _gridPeople = new ObservableCollection<Person>(_people);
                    Thread.Sleep(1000);
                    });
                IsEnabledInterface = true;
                OnPropertyChanged(nameof(GridPeople));
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsPersonSelected(object o)
        {
            return SelectedPerson != null;
        }

        private void filterPeople()
        {
            _gridPeople = new ObservableCollection<Person>(_people);
            if (_isAdultFilter)
            {
                _gridPeople = new ObservableCollection<Person>(_gridPeople.Where(person => person.IsAdult));
            }
            if (_isBirthdayFilter)
            {
                _gridPeople = new ObservableCollection<Person>(_gridPeople.Where(person => person.IsBirthday));
            }
            OnPropertyChanged(nameof(GridPeople));
        }

        public NavigationTypes ViewType => NavigationTypes.List;
    }
}

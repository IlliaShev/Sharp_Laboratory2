﻿using Laboratory2.Models;
using Laboratory2.Navigation;
using Laboratory2.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory2.ViewModels
{
    internal class InfoViewModel: INavigatable<NavigationTypes>
    {
        private readonly Person _person;
        private readonly Action _navigateToForm;
        private RelayCommand<object> _closeCommand;
        private RelayCommand<object> _goToFormCommand;

        public InfoViewModel(Action navigateToForm, ref Person person) 
        {
            _navigateToForm = navigateToForm;
            _person = person;
        }

        public string FirstName
        {
            get { return _person.FirstName; }
        }

        public string LastName
        {
            get { return _person.LastName; }
        }

        public string Email
        {
            get { return _person.Email; }
        }

        public string Birthday
        {
            get { return ((DateTime)_person.Birthday).ToShortDateString(); }
        }

        public string IsAdult
        {
            get { return _person.IsAdult ? "Yes" : "No"; }
        }

        public string WesternZod
        {
            get { return _person.WesternZod.ToString(); }
        }

        public string ChineseZod
        {
            get { return _person.ChineseZod.ToString(); }
        }

        public string IsBirthday
        {
            get {  return _person.IsBirthday ? "Yes" : "No"; }
        }

        public RelayCommand<object> GoToFormCommand
        {
            get
            {
                return _goToFormCommand ??= new RelayCommand<object>(_ => _navigateToForm.Invoke());
            }
        }
        public RelayCommand<object> CloseCommand
        {
            get
            {
                return _closeCommand ??= new RelayCommand<object>(_ => Environment.Exit(0));
            }
        }

        public NavigationTypes ViewType => NavigationTypes.Info;
    }
}

using Laboratory2.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laboratory2.Models
{
    internal class Person
    {
        public enum ChineseZodiac
        {
            Rat, Ox, Tiger, Rabbit, Dragon, Snake, Horse, Goat, Monkey, Rooster, Dog, Pig
        }
        public enum WesternZodiac
        {
            Aries, Taurus, Gemini, Cancer, Leo, Virgo, Libra, Scorpio, Sagittarius, Capricorn, Aquarius, Pisces
        }

        private DateTime? _birthday;
        private ChineseZodiac _chineseZod;
        private WesternZodiac _westernZod;
        private string _fisrtName;
        private string _lastName;
        private string? _email;
        private bool _isAdult;
        private bool _isBirthday;
        private int _age;

        public bool IsAdult 
        { 
            get { return _isAdult; } 
        }
        public bool IsBirthday 
        { 
            get { return _isBirthday; }
        }
        public ChineseZodiac ChineseZod 
        {
            get { return _chineseZod; }
        }
        public WesternZodiac WesternZod
        {
            get { return _westernZod; }
        }
        public int Age 
        { 
            get { return _age; }
        }
        public string FirstName 
        {
            get { return _fisrtName; }
            set { _fisrtName = value; }
        }
        public string LastName 
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string? Email 
        {
            get { return _email; }
            set { _email = value; }
        }
        public DateTime? Birthday 
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        public Person(string firstName, string lastName, string? email, DateTime? birthday)
        {
            _fisrtName = firstName;
            _lastName = lastName;
            _email = email;
            _birthday = birthday;
            if (_birthday != null)
            {
                _age = CalculateAge((DateTime)_birthday);
            }
        }

        public Person(string firstName, string lastName, string email) : this(firstName, lastName, email, null)
        {
            
        }
        public Person(string firstName, string lastName, DateTime birthday) : this(firstName, lastName, null, birthday)
        {

        }
        public Person() : this("", "", null, null) { }


        public async Task Proceed()
        {
            if (!IsValidEmail())
            {
                throw new InvalidEmailException("You entered invalid email!");
            }
            if (_birthday >= DateTime.Today)
            {
                throw new BirthInFutureException("You are not born yet!");
            }
            var calcIsAdult = Task.Run(() => {
                if (_birthday != null)
                {
                    _age = CalculateAge((DateTime)_birthday);
                }
                _isAdult = _age >= 18; 
            });
            var calcIsBirthday = Task.Run(() => {
                if (_birthday == null)
                {
                    _isBirthday = false;
                    return;
                }
                var today = DateTime.Today;
                var birth = (DateTime)_birthday;
                _isBirthday = birth.Day == today.Day && birth.Month == today.Month;
            });
            var calcWesternZod = Task.Run(() => {
                if (_birthday != null)
                {
                    _westernZod = GetWesternZodiac((DateTime)_birthday);
                }
            });
            var calcChineseZod = Task.Run(() => {
                if (_birthday != null)
                {
                    _chineseZod = GetChineseZodiac((DateTime)_birthday);
                }
            });
            await calcIsAdult;
            await calcIsBirthday; 
            await calcWesternZod; 
            await calcChineseZod;
            if (_age >= 135)
            {
                throw new BirthFarInPastException($"You age is {Age}. You are too old for this!");
            }
        }

        private bool IsValidEmail()
        {
            Regex regex = new Regex(@"(\w+)@(\w+)\.(\w+)");
            return regex.IsMatch(Email);
        }

        public WesternZodiac GetWesternZodiac(DateTime birthday)
        {
            int month = birthday.Month;
            int day = birthday.Day;

            switch (month)
            {
                case 1: return day <= 20 ? WesternZodiac.Capricorn : WesternZodiac.Aquarius;
                case 2: return day <= 19 ? WesternZodiac.Aquarius : WesternZodiac.Pisces;
                case 3: return day <= 20 ? WesternZodiac.Pisces : WesternZodiac.Aries;
                case 4: return day <= 20 ? WesternZodiac.Aries : WesternZodiac.Taurus;
                case 5: return day <= 21 ? WesternZodiac.Taurus : WesternZodiac.Gemini;
                case 6: return day <= 21 ? WesternZodiac.Gemini : WesternZodiac.Cancer;
                case 7: return day <= 23 ? WesternZodiac.Cancer : WesternZodiac.Leo;
                case 8: return day <= 23 ? WesternZodiac.Leo : WesternZodiac.Virgo;
                case 9: return day <= 23 ? WesternZodiac.Virgo : WesternZodiac.Libra;
                case 10: return day <= 23 ? WesternZodiac.Libra : WesternZodiac.Scorpio;
                case 11: return day <= 22 ? WesternZodiac.Scorpio : WesternZodiac.Sagittarius;
                case 12: return day <= 21 ? WesternZodiac.Sagittarius : WesternZodiac.Capricorn;
                default: throw new ArgumentOutOfRangeException("Invalid month");
            }
        }

        public ChineseZodiac GetChineseZodiac(DateTime birthday)
        {
            int year = birthday.Year;
            return (ChineseZodiac)((year - 1900) % 12);
        }

        public int CalculateAge(DateTime birthday)
        {
            var today = DateTime.Today;
            var age = today.Year - birthday.Year;
            if (birthday > today.AddYears(-age)) age--;
            return age;
        }
    }
}

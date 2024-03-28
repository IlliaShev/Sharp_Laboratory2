using Laboratory2.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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

        private DateTime _birthday;
        private Guid _id;
        private ChineseZodiac _chineseZod;
        private WesternZodiac _westernZod;
        private string _firstName;
        private string _lastName;
        private string? _email;
        private bool _isAdult;
        private bool _isBirthday;
        private int _age;

        public Guid Id { get { return _id; } }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
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

        public DateTime Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        public int Age
        {
            get { return _age; }
            //set { _age = value; }
        }

        public bool IsAdult 
        { 
            get { return _isAdult; } 
            //set { _isAdult = value; }
        }
        public bool IsBirthday 
        { 
            get { return _isBirthday; }
            //set { _isBirthday = value; }
        }
        public ChineseZodiac ChineseZod 
        {
            get { return _chineseZod; }
            //set { _chineseZod = value; }
        }
        public WesternZodiac WesternZod
        {
            get { return _westernZod; }
            //set { _westernZod = value; }
        }

        public string BirthdayShorten
        {
            get { return _birthday.ToShortDateString(); }
        }

        [JsonConstructor]
        public Person(Guid id, string firstName, string lastName, string email,
            int age, bool isAdult, bool isBirthday, ChineseZodiac chineseZod, WesternZodiac westernZod)
        {
            _id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            //Birthday = birthday;
            _age = age;
            _isAdult = isAdult;
            this._westernZod = (WesternZodiac)westernZod;
            this._chineseZod = (ChineseZodiac)chineseZod;
            this._isBirthday = isBirthday;
        }

        public Person(string firstName, string lastName, string? email, DateTime birthday)
        {
            _id = Guid.NewGuid();
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _birthday = birthday;
            if (_birthday != null)
            {
                _chineseZod = GetChineseZodiac((DateTime)_birthday);
                _age = CalculateAge((DateTime)_birthday);
                _westernZod = GetWesternZodiac((DateTime)_birthday);
                _isAdult = _age >= 18;
                var today = DateTime.Today;
                var birth = (DateTime)_birthday;
                _isBirthday = birth.Day == today.Day && birth.Month == today.Month;
            }

        }

        public Person(string firstName, string lastName, string email) : this(firstName, lastName, email, DateTime.Today)
        {
            
        }
        public Person(string firstName, string lastName, DateTime birthday) : this(firstName, lastName, null, birthday)
        {

        }
        public Person() : this("", "", null, DateTime.Today) { }

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
                Thread.Sleep(400);
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

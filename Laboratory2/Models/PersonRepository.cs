using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Laboratory2.Models
{
    internal class PersonRepository
    {
        private readonly static string DB = Path.Combine(Environment.CurrentDirectory, "storage");

        private static PersonRepository _instance;
        private static readonly object _lock = new object();
        public static PersonRepository Instance 
        { 
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new PersonRepository();
                    }
                    return _instance;
                }
            }
        }

        private PersonRepository() { 
            if (!Directory.Exists(DB))
            {
                Directory.CreateDirectory(DB);
            }

            if (PersonList().Count == 0)
            {
                var rnd = new Random();
                for (int i = 0; i < 50; i++)
                {
                    DateTime birthday = new DateTime(rnd.Next(2000, 2021), rnd.Next(1, 13), rnd.Next(1, 29));
                    string firstName = "User_" + i;
                    string lastName = "Surname_" + i;
                    UpsertPerson(new Person(firstName, lastName, i + "@gmail.com", birthday));
                }
            }
        }

        public List<Person> PersonList()
        {
            var people = new List<Person>();
            foreach (var file in Directory.EnumerateFiles(DB))
            {
                string? personInString = null;
                using (var reader = new StreamReader(file))
                {
                    personInString = reader.ReadToEnd();
                }
                if (personInString != null)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    people.Add(JsonSerializer.Deserialize<Person>(personInString, options));
                }
            }
            return people;
        }


        public void RemovePerson(Guid id)
        {
            File.Delete(Path.Combine(DB, id.ToString()));
        }

        public void UpsertPerson(Person person)
        {
            var personInString = JsonSerializer.Serialize(person);
            using (var writer = new StreamWriter(Path.Combine(DB, person.Id.ToString()), false))
            {
                writer.Write(personInString);
            }
        }

    }
}

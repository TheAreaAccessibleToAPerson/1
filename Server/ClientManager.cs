using System.Collections.Concurrent;

namespace Gudron
{
    public class PersonManager : Controller.Output<string>
    {
        private readonly ConcurrentDictionary<ulong, RequestDB<Person>> _values 
            = new ConcurrentDictionary<ulong, RequestDB<Person>>();

        private ulong _uniqueID = 0;

        public void Add(RequestDB<Person> request)
        {
        }

        public void Add(string message)
        {
        }
    }
}
using System.Collections.Concurrent;

namespace Gudron
{
    public class DBManager : Controller.Output<string>
    {
        private readonly ConcurrentDictionary<ulong, RequestDB> _values 
            = new ConcurrentDictionary<ulong, RequestDB>();

        private ulong _uniqueID = 0;

        public void Add(RequestDB request)
        {
            output("HELLO1111");
        }

        public void Add(string message)
        {
        }
    }
}
using System.Threading;
namespace Gudron
{
    public sealed class CollectorCollection<T>
    {
        private readonly List<T> _values = new List<T>();

        private readonly Action<T[]> _action;

        private object _1 = new object();
        private object _2 = new object();
        private object _3 = new object();

        private bool _is1 = false;

        public CollectorCollection(Action<T[]> callAction)
            => _action = callAction;

        public void Subscribe(T value)
        {
        }

        private void Process(T[] values)
        {
            //...
        }
    }
}
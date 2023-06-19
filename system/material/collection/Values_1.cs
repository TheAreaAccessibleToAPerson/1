namespace Gudron.system.collection
{
    public sealed class Value<ValueType> 
    {
        private List<ValueType> _values = new List<ValueType>();

        private object _locker = new object();

        public void Add(ValueType value)
        {
            lock(_locker)
            {
                _values.Add(value);
            }
        }

        public bool TryExtractAll(out ValueType[] values)
        {
            values = null;

            if (_values.Count == 0)
            {
                return false;
            }
            else 
            {
                lock(_locker)
                {
                    if (_values.Count > 0) 
                    {
                        values = _values.ToArray();

                        _values.Clear();

                        return true;
                    }
                    else 
                        return false;
                }
            }

            return false;
        }
    }
}
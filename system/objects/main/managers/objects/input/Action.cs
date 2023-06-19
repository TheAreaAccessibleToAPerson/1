namespace Gudron.system.objects.main
{
    public sealed class UnsafeParallelActionObject<T> : IInput<T>, IInputConnected
    {
        private readonly Action<T> _action;

        public UnsafeParallelActionObject(ref IInput<T> input, Action<T> action) 
            : base ()
        {
            _action = action;
            input = this;
        }

        private IInput<Action> _connect;

        public void SetConnected(object inputConnect)
            => Hellper.Connected<IInput<Action>>(inputConnect, ref _connect, GetType());

        public void To(T value) => _connect.To(() => _action(value));
    }

    public class ActionObject<T> : IInput<T>
    {
        private readonly Action<T> _action;

        public ActionObject(ref IInput<T> input, Action<T> action) 
        {
            _action = action;
            input = this;
        }

        void IInput<T>.To(T value) => _action.Invoke(value);
    }

    public sealed class ActionObject<T1, T2> : IInput<T1, T2>
    {
        private readonly Action<T1, T2> _action;

        public ActionObject(ref IInput<T1, T2> input, Action<T1, T2> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2>.To(T1 value1, T2 value2) => _action.Invoke(value1, value2);
    }

    public sealed class ActionObject<T1, T2, T3> : IInput<T1, T2, T3>
    {
        private readonly Action<T1, T2, T3> _action;

        public ActionObject(ref IInput<T1, T2, T3> input, Action<T1, T2, T3> action)
        {
            _action = action;
            input = this;
        }

        void IInput<T1, T2, T3>.To
            (T1 value1, T2 value2, T3 value3)
                => _action.Invoke(value1, value2, value3);
    }
}
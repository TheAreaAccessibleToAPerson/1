namespace Gudron
{
    public interface IReturn<R> 
    { 
        void To(R value); 
        ulong GetID();
        ulong GetUnieueID();
    }

    public interface IReturn<R1, R2> 
    { 
        void To(R1 value1, R2 value2); 
        ulong GetID();
        ulong GetUnieueID();
    }
}

namespace Gudron.system.objects.main
{
    /// <summary>
    /// Прослушивает входящие сообщения с возможностью возрата отправителю ответа.
    /// </summary>
    /// <typeparam name="T">Тип входящий данных.</typeparam>
    /// <typeparam name="R">Тип возращаемых данныx.</typeparam>
    public sealed class ListenEcho<T, R> : Redirect<T, IReturn<R>>, IInput<T, IReturn<R>>, IInputConnect
    {
        public ListenEcho(IInformation information) : base (information) {}

        void IInput<T, IReturn<R>>.To
            (T value1, IReturn<R> value2) => _returnAction(value1, value2);

        object IInputConnect.GetConnect() => this;
    }

    public sealed class SendEcho<T, R> : Redirect<R>, IInput<T>, IReturn<R>, IInputConnected
    {
        private IInput<T, IReturn<R>> _connect;

        /// <summary>
        /// Уникальный индетификационый номер для данного обьекта. 
        /// </summary>
        private readonly ulong _uniqueID;

        void IInputConnected.SetConnected(object inputConnect)
            => Hellper.Connected<IInput<T, IReturn<R>>>(inputConnect, ref _connect, GetType());

        public SendEcho(IInformation information) : base(information)
            => _uniqueID = s_uniqueID++;

        void IInput<T>.To(T value) => _connect.To(value, this);

        void IReturn<R>.To(R value) => input.To(value);
        public ulong GetUnieueID() => _uniqueID;

        private static ulong s_uniqueID = 0;
    }
}
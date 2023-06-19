namespace Gudron.system.objects.main
{
    public abstract class Information : IInformation
    {
        private readonly IInformation _information;

        public Information(IInformation information) 
            => _information = information;

        public string GetExplorer() => _information.GetExplorer();
        public ulong GetID() => _information.GetID();
        public ulong GetNodeID() => _information.GetNodeID();

        public manager.IGlobalObjects GetGlobalObjectsManager() 
            => _information.GetGlobalObjectsManager();

    }

    public abstract class Redirect<T> : Information, IRedirect<T>
    {
        public Redirect(IInformation information) : base(information){}

        protected IInput<T> input;

        public void output_to(Action<T> action)
            => new ActionObject<T>(ref input, action);

        public IRedirect<OutputValueType> output_to<OutputValueType>(Func<T, OutputValueType> func)
            => new FuncObject<T, OutputValueType> (ref input, func, this);

        public void send_message_to(string name)
            => GetGlobalObjectsManager().Get<ListenMessage<T>, IInput<T>> (name, ref input);

        public IRedirect<T> send_echo_to(string name) 
            => GetGlobalObjectsManager().Get<ListenEcho<T, T>, SendEcho<T, T>, IInput<T>, IRedirect<T>> 
                (name, ref input, new SendEcho<T, T>(this));
    }

    public abstract class Redirect<T1, T2> : Information, IRedirect<T1, T2>
    {
        private readonly manager.IGlobalObjects _globalObjectsManager;

        protected Action<T1, T2> _returnAction;

        public Redirect(IInformation information) : base(information){}

        public void output_to(Action<T1, T2> action) => _returnAction = action;

        public IRedirect<OutputValueType> output_to<OutputValueType>(Func<T1, T2, OutputValueType> action)
        {
            return default;
        }
    }
}
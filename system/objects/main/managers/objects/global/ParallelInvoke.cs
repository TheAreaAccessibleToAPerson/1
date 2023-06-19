namespace Gudron.system.objects.main
{
    public sealed class UnsafeParallelInvoke : Information, IInput<Action>, IInputConnect
    {
        private readonly CollectorCollection<Action> collection;

        public UnsafeParallelInvoke(IInformation information)
            : base(information)
                => collection = new CollectorCollection<Action>
                    (CreatingParallelInvoke);

        void IInput<Action>.To(Action value)
            => collection.Subscribe(value);

        void CreatingParallelInvoke(params Action[] actions)
            => Parallel.Invoke(actions);
        

        public object GetConnect() => this;
    }
}
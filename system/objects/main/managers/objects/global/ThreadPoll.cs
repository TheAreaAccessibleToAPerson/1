namespace Gudron.system.objects.main
{
    public class ThreadPoll : Information, IInput<Action>, IInputConnect
    {
        public ThreadPoll(IInformation information)
            : base(information)
        {
        }

        void IInput<Action>.To(Action value){}

        public void Update()
        {
        }

        public object GetConnect() => this;
    }
}
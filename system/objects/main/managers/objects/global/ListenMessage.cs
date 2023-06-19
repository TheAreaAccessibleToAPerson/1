namespace Gudron.system.objects.main
{
    public sealed class ListenMessage<T> : Redirect<T>, IInput<T>
    {
        public ListenMessage(IInformation information) : base (information){}

        public void To(T value) => input.To(value);
    }
}
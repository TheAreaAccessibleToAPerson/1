namespace Gudron.system.objects.root.manager
{
    public sealed class ActionInvoke : Controller 
    {
        private readonly collection.Value<global::System.Action> _values = new collection.Value<Action>();

        public global::System.Action Add 
        {
            set { _values.Add(value); }
        }

        void Start() => add_thread("Process", Process, 5, Thread.Priority.Lowest);

        void Process()
        {
            if (_values.TryExtractAll(out global::System.Action[] actions))
            {
                foreach(global::System.Action action in actions)
                    action.Invoke();
            }
        }
    }
}
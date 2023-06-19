namespace Gudron.system.objects.root
{
    public class Object<ObjectType> : Controller.Board, IManager, description.ILife
        where ObjectType : main.Object, new()
    {
        private manager.ActionInvoke ActionInvokeManager;
        private manager.PollThreads PollThreads;

        void Construction()
        {
            PollThreads = obj<manager.PollThreads>("PollThreads");
            ActionInvokeManager = obj<manager.ActionInvoke>("ActionInvoke");
        }

        void Start() => obj<ObjectType>("");

        void IManager.ActionInvoke(Action action) 
            => ActionInvokeManager.Add = action;

        void IManager.AddSubscribeTickets(main.SubscribePollTicket[] tickets)
            => PollThreads.Add(tickets);

        void description.ILife.Run()
        {
            ((main.description.IDOM)this).NodeDefine
                ("", 0, new ulong[0], this, this, this, new Dictionary<string, object>());

            ((main.description.IDOM)this).CreatingNode();

            global::System.Threading.Thread.CurrentThread.Priority = global::System.Threading.ThreadPriority.Lowest;

            while (true)
            {
                global::System.GC.Collect();

                if (StateInformation.IsDestroying)
                {
                    return;
                }

                sleep (5000);
            }
        }
    }
}
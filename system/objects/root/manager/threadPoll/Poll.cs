namespace Gudron.system.objects.root
{
    public class Poll : manager.Clients
    {
        /// <summary>
        /// Локер блокирующийся на время выполнения action.
        /// </summary>
        /// <returns></returns>
        private readonly object _actionRunLocker = new object();

        /// <summary>
        /// Oбьект начал процесс подписания полученых билетов?
        /// </summary>
        private bool _isProcessSubscribeAndUnsubscribe = false;
        private readonly object _processSubscribeAndUnsubscribeLocker = new object();

        /// <summary>
        /// Блокирует доступ к регистрации. 
        /// </summary>
        /// <returns></returns>
        private readonly object _subsribeAndUnsubscribeLocker = new object();


        /// <summary>
        /// Сдесь будут хранится билеты. 
        /// </summary>
        /// <typeparam name="object"></typeparam>
        /// <returns></returns>
        private readonly collection.Value<object> _tickets = new collection.Value<object>();

        /// <summary>
        /// Получаем билет. 
        /// </summary>
        /// <param name="ticket"></param>
        public void AddTicket(main.SubscribePollTicket ticket)
        {
            if (global::System.Threading.Monitor.TryEnter(_actionRunLocker))
            {
                if (global::System.Threading.Monitor.TryEnter(_subsribeAndUnsubscribeLocker))
                {
                    Add(ticket);

                    global::System.Threading.Monitor.Exit(_subsribeAndUnsubscribeLocker);
                }
                else
                    lock (_subsribeAndUnsubscribeLocker) Add(ticket);

                global::System.Threading.Monitor.Exit(_actionRunLocker);
            }
            else if (global::System.Threading.Monitor.TryEnter(_subsribeAndUnsubscribeLocker))
            {
                lock (_processSubscribeAndUnsubscribeLocker)
                {
                    if (_isProcessSubscribeAndUnsubscribe == false)
                    {
                        _tickets.Add(ticket);

                        global::System.Threading.Monitor.Exit(_subsribeAndUnsubscribeLocker);

                        return;
                    }
                    else
                        lock (_subsribeAndUnsubscribeLocker) Add(ticket);
                }

                global::System.Threading.Monitor.Exit(_subsribeAndUnsubscribeLocker);
            }
            else
                lock (_subsribeAndUnsubscribeLocker) Add(ticket);
        }

        void Start()
            => add_thread(Field.Name, () =>
            {
                lock (_processSubscribeAndUnsubscribeLocker) _isProcessSubscribeAndUnsubscribe = false;

                lock (_actionRunLocker) Run();

                lock (_subsribeAndUnsubscribeLocker)
                {
                    lock (_processSubscribeAndUnsubscribeLocker) _isProcessSubscribeAndUnsubscribe = true;

                    if (_tickets.TryExtractAll
                        (out object[] tickets)) Add(tickets);

                    if (Count == 0) Field.Destroy(this);
                }
            },
            Field.TimeDelay, Field.ThreadPriority);
    }
}
namespace Gudron.system.objects.main
{
    /// <summary>
    /// Главный обьект.
    /// </summary>
    public class Object : informing.IMain, description.IDOM, description.IPoll,
        manager.IDispatcher, IInformation
    {
        public readonly information.State StateInformation;
        private readonly information.State.Manager _stateManagerInformation;
        public readonly information.Header HeaderInformation;
        private readonly information.Tegs _tegsInformation;

        public Object(string type)
        {
            StateInformation = new information.State();
            _stateManagerInformation = new information.State.Manager(this, StateInformation);

            HeaderInformation = new information.Header(this, type);
            _tegsInformation = new information.Tegs();
        }

        #region LifeCyrcleManager

        private manager.LifeCyrcle _lifeCyrcleManager;

        public void destroy()
        {
        }

        #endregion

        #region Dispatcher

        private manager.Dispatcher _dispatcherManager;

        void manager.IDispatcher.Process(string command) 
            => _dispatcherManager.Process(command);

        #endregion

        #region DOM

        private information.DOM _DOMInformation;

        void description.IDOM.CreatingNode() 
            => _dispatcherManager.Start();

        void description.IDOM.BranchDefine(string keyObject, ulong nodeID, ulong nestingNodeNamberInTheSystem,
            ulong nestingNodeNamberInTheNode, ulong[] parentObjectsID, main.Object parentObject, main.Object nodeObject,
                main.Object nearIndependentNodeObject, root.IManager rootManager, Dictionary<string, object> globalObjects)
        {
            HeaderInformation.BranchDefine(parentObject.HeaderInformation.Directory, GetType(), parentObject._DOMInformation, keyObject);

            _DOMInformation = new information.DOM(keyObject, nodeID, nestingNodeNamberInTheSystem, nestingNodeNamberInTheNode, parentObjectsID,
                this, parentObject, nodeObject, nearIndependentNodeObject, rootManager);

            Define(globalObjects);
        }

        void description.IDOM.NodeDefine(string keyObject, ulong nestingNodeNamberInTheSystem, ulong[] parentObjectsID,
            main.Object parentObject, main.Object nearIndependentNodeObject, root.IManager rootManager,
                Dictionary<string, object> globalObjects)
        {
            HeaderInformation.NodeDefine(parentObject.HeaderInformation.Directory, GetType(), parentObject._DOMInformation, keyObject);

            _DOMInformation = new information.DOM(keyObject, nestingNodeNamberInTheSystem, parentObjectsID, this, parentObject,
                nearIndependentNodeObject, rootManager);

            Define(globalObjects);
        }

        private void Define(Dictionary<string, object> globalObjects)
        {
            _dispatcherManager = new manager.Dispatcher(this, HeaderInformation);
            {
                _globalObjectsManager = new manager.GlobalObjects(this, globalObjects, HeaderInformation, StateInformation, _DOMInformation);

                _branchObjectsManager = new manager.BranchObjects(this, HeaderInformation, StateInformation, _DOMInformation, globalObjects);
                _nodeObjectsManager = new manager.NodeObjects(this, HeaderInformation, StateInformation, _DOMInformation, globalObjects);

                _threadsManager = new manager.Threads(this, StateInformation);
                _subscribeManager = new manager.Subscribe(this, StateInformation, HeaderInformation, _DOMInformation, _dispatcherManager);
                _lifeCyrcleManager = new manager.LifeCyrcle(this, HeaderInformation, StateInformation, _stateManagerInformation, _DOMInformation,
                    _branchObjectsManager, _nodeObjectsManager, _dispatcherManager);
            }
            _dispatcherManager.Initialize(_lifeCyrcleManager, _subscribeManager, _threadsManager, _nodeObjectsManager);
        }

        #endregion

        #region GlobalObjectsManager

        private manager.GlobalObjects _globalObjectsManager;

        protected IRedirect<T, IReturn<R>> listen_echo<T, R>(string name) 
            => _globalObjectsManager.Add<ListenEcho<T, R>, IRedirect<T, IReturn<R>>>
                (name, new ListenEcho<T, R>(this));

        protected IRedirect<T, IReturn<T>> listen_echo<T>(string name) 
            => _globalObjectsManager.Add<ListenEcho<T, T>, IRedirect<T, IReturn<T>>>
                (name, new ListenEcho<T, T>(this));

        protected IRedirect<T> send_echo<T>(ref IInput<T> input, string name) 
            => _globalObjectsManager.Get<ListenEcho<T, T>, SendEcho<T, T>, IInput<T>, IRedirect<T>> 
                (name, ref input, new SendEcho<T, T>(this));

        protected IRedirect<R> send_echo_1_1<T, R>(ref IInput<T> input, string name) 
            => _globalObjectsManager.Get<ListenEcho<T, R>, SendEcho<T, R>, IInput<T>, IRedirect<R>> 
                (name, ref input, new SendEcho<T, R>(this));

        protected void send_message<T>(ref IInput<T> input, string name) 
            => _globalObjectsManager.Get<ListenMessage<T>, IInput<T>> 
                (name, ref input);

        protected IRedirect<T> listen_message<T>(string name) 
            => _globalObjectsManager.Add<ListenMessage<T>, IRedirect<T>> 
                (name, new ListenMessage<T>(this));

        protected void thread_poll(string name)
            => _subscribeManager.Add 
                (name, _globalObjectsManager.Add(name, new ThreadPoll(this)).Update);

        #endregion

        #region Input

        protected void input_to<T>(ref IInput<T> input, Action<T> action)
            => new ActionObject<T>(ref input, action);
        protected void input_to<T1, T2>(ref IInput<T1, T2> input, Action<T1, T2> action)
            => new ActionObject<T1, T2>(ref input, action);
        protected void input_to<T1, T2, T3>(ref IInput<T1, T2, T3> input, Action<T1, T2, T3> action)
            => new ActionObject<T1, T2, T3>(ref input, action);

        protected IRedirect<R> input_to<T, R>(ref IInput<T> input, Func<T, R> func) 
            => new FuncObject<T, R>(ref input, func, this);
        protected IRedirect<R> input_to<T1, T2, R>(ref IInput<T1, T2> input, Func<T1, T2, R> func) 
            => new FuncObject<T1, T2, R>(ref input, func, this);
        protected IRedirect<R> input_to<T1, T2, T3, R>(ref IInput<T1, T2, T3> input, Func<T1, T2, T3, R> func) 
            => new FuncObject<T1, T2, T3, R>(ref input, func, this);

        #endregion

        #region ObjectsManager

        private manager.BranchObjects _branchObjectsManager;
        private manager.NodeObjects _nodeObjectsManager;

        protected IRedirect<R> obj<ObjectType, R>(string key, params object[] localFields)
            where ObjectType : main.Object, IRedirect<R>, new()
                => obj<ObjectType>(key, localFields);

        protected ObjectType obj<ObjectType>(string key, params object[] localFields)
            where ObjectType : main.Object, new()
        {
            if (StateInformation.IsContruction)
            {
                return _branchObjectsManager.Add<ObjectType> 
                    (key, localFields);
            }
            else
                return _nodeObjectsManager.Add<ObjectType> 
                    (key, localFields);
        }

        #endregion

        #region Events

        protected void add_event(string name, Action action)
            => _subscribeManager.Add(name, action);

        #endregion

        #region Thread

        private manager.Threads _threadsManager;

        protected void add_thread(string name, Action action, uint timeDelay, Thread.Priority priority) =>
            _threadsManager.Add(name, action, timeDelay, priority);

        #endregion

        #region Informing

        public void Console<T>(T message) where T : IConvertible
            => global::System.Console.WriteLine($"{HeaderInformation.Explorer}:{message.ToString()}");

        public Exception Exception(string message, params string[] arg)
        {
            global::System.Console.ForegroundColor = ConsoleColor.Red;

            System.Console.WriteLine(message, arg);

            sleep(100000);

            throw new Exception(message);
        }

        public void SystemInformation(string message)
        {
        }

        #endregion

        #region SuscribeManager

        private manager.Subscribe _subscribeManager;

        void description.IPoll.Add(string name, Action action)
            => _subscribeManager.Add(name, action);

        #endregion

        #region Information

        public ulong GetID() => _DOMInformation.ID;
        public ulong GetNodeID() => _DOMInformation.NodeID;
        public string GetExplorer() => HeaderInformation.Explorer;

        manager.IGlobalObjects IInformation.GetGlobalObjectsManager() => _globalObjectsManager;

        #endregion

        #region Hellpers

        private System.DateTime d_localDateTime = System.DateTime.Now;
        private static System.DateTime d_globalStartTime = System.DateTime.Now;

        protected void start_timer() 
            => d_localDateTime = System.DateTime.Now;

        protected int step_timer() 
            => (System.DateTime.Now.Subtract(d_localDateTime).Seconds * 1000) 
                + System.DateTime.Now.Subtract(d_localDateTime).Milliseconds;

        protected static void global_start_timer() 
            => d_globalStartTime = System.DateTime.Now;

        protected static int global_end_timer() 
            => System.DateTime.Now.Subtract(d_globalStartTime).Milliseconds;

        protected void sleep(int pTimeSpeep) 
            => global::System.Threading.Thread.Sleep(pTimeSpeep);

        #endregion
    }
}
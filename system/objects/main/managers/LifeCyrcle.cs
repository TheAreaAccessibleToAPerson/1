namespace Gudron.system.objects.main.manager
{
    public class LifeCyrcle : Informing, dispatcher.ILifeCyrcle
    {
        public struct Data 
        {
            /// <summary>
            /// Запускаем contruction в обьектах ветках.
            /// </summary>
            public const string BEGIN_BRANCH_OBJECT_CONTRUCTION = "ContructionObject";

            /// <summary>
            /// Приступаем к запуску обьектов.
            /// </summary>
            public const string BEGIN_STARTING = "StartingObject";

            /// <summary>
            /// Продолжаем запуск обьекта. 
            /// </summary>
            public const string CONTINUE_STARTING = "ContinueStartingObject";
        }

        private readonly information.State.Manager _stateManagerInformation;
        private readonly information.State _stateInformation;
        private readonly information.Header _headerInformation;
        private readonly information.DOM _DOMInformation;

        private readonly manager.BranchObjects _branchObjectsManager;
        private readonly manager.NodeObjects _nodeObjectsManager;

        private readonly IDispatcher _dispatcher;

        public LifeCyrcle(informing.IMain mainInforming, information.Header headerInformation, information.State stateInformation, 
            information.State.Manager stateManagerInformation, information.DOM DOMInformation, manager.BranchObjects branchObjectsManager,
                manager.NodeObjects nodeObjectsManager, IDispatcher dispatcher)
            : base ("LifeCyrcleManager", mainInforming)
        {
            _stateManagerInformation = stateManagerInformation;
            _stateInformation = stateInformation;
            _headerInformation = headerInformation;
            _DOMInformation = DOMInformation; 

            _branchObjectsManager = branchObjectsManager;
            _nodeObjectsManager = nodeObjectsManager;

            _dispatcher = dispatcher;
        }

        void dispatcher.ILifeCyrcle.Contruction()
        {
            lock(_stateInformation.Locker)
            {
                if (_stateManagerInformation.Replace(information.State.Data.CONSTRUCTION, information.State.Data.OCCUPERENCE))
                {
                    if (Hellper.GetSystemMethod("Construction", _DOMInformation.CurrentObject.GetType(), out System.Reflection.MethodInfo oSystemMethodConstruction)) 
                        oSystemMethodConstruction.Invoke(_DOMInformation.CurrentObject, null);

                    _branchObjectsManager.LifeCyrcle(Data.BEGIN_BRANCH_OBJECT_CONTRUCTION);

                    _dispatcher.Process(manager.Dispatcher.Command.THE_OBJECT_IS_CONSTRUCTION);
                }   
                else 
                    Exception(data.LifeCyrcleManager.x100001, information.State.Data.OCCUPERENCE, _stateInformation.CurrentState);
            }
        }

        void dispatcher.ILifeCyrcle.Starting()
        {
            lock(_stateInformation.Locker)
            {
                if (_stateManagerInformation.Replace(information.State.Data.STARTING, information.State.Data.CONSTRUCTION))
                {
                    if (Hellper.GetSystemMethod("Configurate", _DOMInformation.CurrentObject.GetType(), out System.Reflection.MethodInfo oSystemMethodConfigurate)) 
                        oSystemMethodConfigurate.Invoke(_DOMInformation.CurrentObject, null);

                    _branchObjectsManager.LifeCyrcle(Data.BEGIN_STARTING);

                    if (Hellper.GetSystemMethod("Start", _DOMInformation.CurrentObject.GetType(), out System.Reflection.MethodInfo oSystemMethodStart)) 
                    {
                        oSystemMethodStart.Invoke(_DOMInformation.CurrentObject, null);
                    }

                    _dispatcher.Process(manager.Dispatcher.Command.THE_FIRST_STAGE_STARTING_IS_CLOSED);
                }
                else 
                    Exception(data.LifeCyrcleManager.x100001, information.State.Data.CONSTRUCTION, _stateInformation.CurrentState);
            }
        }

        void dispatcher.ILifeCyrcle.ContinueStarting()
        {
            _branchObjectsManager.LifeCyrcle(Data.CONTINUE_STARTING);

            _dispatcher.Process(
                manager.Dispatcher.Command.STARTING_THREAD + __.AND + manager.Dispatcher.Command.CREATING_DEFERRED_NODE_OBJECT);
        }

        public void Configurate()
        {

        }

        public void Start()
        {

        }

        public void ContinueStart()
        {

        }
    }
}
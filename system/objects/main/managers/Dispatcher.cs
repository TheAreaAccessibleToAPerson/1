namespace Gudron.system.objects.main.manager 
{
    public interface IDispatcher
    {
        public void Process(string command);
    }

    namespace dispatcher
    {
        /// <summary>
        /// Описывает сборос работы диспетчера с LifeCyrcleManager'ом.
        /// </summary>
        public interface ILifeCyrcle
        {
            public void Contruction();
            public void Starting();
            public void ContinueStarting();
        }

        /// <summary>
        /// Описывает способ работы диспетчера с SubscribeManager'ом.
        /// </summary>
        public interface ISubscribe
        {
            void StartSubscribe();
        }

        public interface IThreads
        {
            void Start();
        }

        public interface INode
        {
            /// <summary>
            /// Создает отложеные обьекты которые были добавлены в методе Start(). 
            /// </summary>
            void CreatingDeferredObject();
        }
    }

    public sealed class Dispatcher : main.Informing, IDispatcher
    {
        public struct Command
        {
            /// <summary>
            /// Создание Node обьекта.
            /// </summary>
            public const string NODE_OBJECT_CREATING = "NodeObjectCreating";
            /// <summary>
            /// Запустить метод Contruction в LifeCyrcleManager, утановить связи между обьектами,
            /// создать билеты для регистрации в пуллах, и в рассылках сообщений.
            /// </summary>
            public const string CALL_CONSTRUCTION_IN_LIFE_CYRCLE_MANAGER = "CallContructionInLifeCyrcleManager";

            /// <summary>
            /// Обьект сконструирован, данное сообщение ожидается из LifeCyrcleManager, в конце выполнения
            /// метода Contruction. 
            /// </summary>
            public const string THE_OBJECT_IS_CONSTRUCTION = "TheObjectIsContruction";

            /// <summary>
            /// После того как обьект сконструирован, его узел или если обьект сам является узлом
            /// приступает к подписки.
            /// </summary>
            public const string START_SUBSCRIBE = "StartSubscribe";

            /// <summary>
            /// Обьект узел подписался в необходимые места и теперь приступает с запуску.
            /// </summary>
            public const string END_SUBSCRIBE = "EndSubscribe";

            /// <summary>
            /// Вызывает метод Starting в LifeCyrlceManager'е. 
            /// </summary>
            public const string CALL_STARTING_IN_LIFE_CYRCLE_MANAGER = "CallStartingInLifeCyrcleManager";

            /// <summary>
            /// Первая стадия запуска подошла к концу.
            /// В ней мы вызвали метод Configurate, и если в данном методе обьект не был 
            /// выставлен на уничтожение, то запустился метод Start(). 
            /// 
            /// </summary>
            public const string THE_FIRST_STAGE_STARTING_IS_CLOSED = "TheFirstStageStartingIsClosed.";

            /// <summary>
            /// Проложить запуска обьекта.
            /// /// </summary>
            public const string THE_CONTINUE_STARTING = "TheContinueStarting";

            /// <summary>
            /// Запускаем потоки.
            /// </summary>
            public const string STARTING_THREAD = "StartingThread";

            /// <summary>
            /// Создать отложеные узлы, которые были добавлены в методе Start().
            /// </summary>
            public const string CREATING_DEFERRED_NODE_OBJECT = "CreatingDeferredNodeObject";
        }

        private readonly information.Header _headerInformation;

        /// <summary>
        /// Текущая команда которую выполняет диспетчер. 
        /// </summary>
        private string CurrentProcess = Command.NODE_OBJECT_CREATING;

        private dispatcher.ILifeCyrcle _lifeCyrcleDispatcher;
        private dispatcher.ISubscribe _subscribeDispatcher;
        private dispatcher.IThreads _threadsDispatcher;
        private dispatcher.INode _nodeDispatcher;

        public Dispatcher(informing.IMain mainInforming, information.Header headerInformation)
                : base("DispatcherManager", mainInforming)
        {
            _headerInformation = headerInformation;
        }

        public void Initialize(dispatcher.ILifeCyrcle lifeCyrcleDispathcer, dispatcher.ISubscribe subscribeDispatcher, 
            dispatcher.IThreads threadsDispatcher, dispatcher.INode nodeDispatcher)
        {
            _lifeCyrcleDispatcher = lifeCyrcleDispathcer;
            _subscribeDispatcher = subscribeDispatcher;
            _threadsDispatcher = threadsDispatcher;
            _nodeDispatcher = nodeDispatcher;
        }

        public void Process(string command)
        {
            switch(command)
            {
                case Command.CALL_CONSTRUCTION_IN_LIFE_CYRCLE_MANAGER:

                    CurrentProcess = Command.CALL_CONSTRUCTION_IN_LIFE_CYRCLE_MANAGER;

                    _lifeCyrcleDispatcher.Contruction();

                break;

                case Command.THE_OBJECT_IS_CONSTRUCTION:

                    CurrentProcess = Command.START_SUBSCRIBE;

                    if (_headerInformation.IsNodeObject()) 
                    {
                        _subscribeDispatcher.StartSubscribe();
                    }

                break;

                case Command.END_SUBSCRIBE:

                    CurrentProcess = Command.CALL_STARTING_IN_LIFE_CYRCLE_MANAGER;

                    _lifeCyrcleDispatcher.Starting();

                break;

                case Command.THE_FIRST_STAGE_STARTING_IS_CLOSED:

                    CurrentProcess = Command.THE_CONTINUE_STARTING;

                    _lifeCyrcleDispatcher.ContinueStarting();

                break;

                case Command.STARTING_THREAD + __.AND + Command.CREATING_DEFERRED_NODE_OBJECT:

                    CurrentProcess = Command.STARTING_THREAD + __.AND + Command.CREATING_DEFERRED_NODE_OBJECT;

                    _threadsDispatcher.Start();

                    _nodeDispatcher.CreatingDeferredObject();

                break;
            }
        }

        /// <summary>
        /// Запустить диспетчер, начать сборку обьекта.
        /// </summary>
        public void Start()
        {
            if (CurrentProcess == Command.NODE_OBJECT_CREATING)
            {
                CurrentProcess = Command.CALL_CONSTRUCTION_IN_LIFE_CYRCLE_MANAGER;

                _lifeCyrcleDispatcher.Contruction();
            }
            else 
                throw new Exception($"При вызове метода Start() текущая поджен должна быть {Command.NODE_OBJECT_CREATING}, но она {CurrentProcess}.");
        }
    }
}
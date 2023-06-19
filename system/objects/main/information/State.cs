namespace Gudron.system.objects.main.information
{
    namespace state 
    {
        public interface IManager
        {
            /// <summary>
            /// Установить новое состояние обьекта. 
            /// </summary>
            /// <param name="state">Состояние обьекта.</param>
            public void Set(string state);

            /// <summary>
            /// Получить текущее состояние обьекта. 
            /// </summary>
            /// <returns></returns>
            public string Get();

            /// <summary>
            /// Выставить обьект в состояние уничтожения. 
            /// </summary>
            public void Destroy();

            /// <summary>
            /// Выставить обьект в состояние отложеного уничтожения. 
            /// </summary>
            public void DeferredDestroy();
        }
    }

    public class State : state.IManager
    {
        public struct Data
        {
            public const string OCCUPERENCE = "Occuperence";
            public const string CONSTRUCTION = "Construction";

            public const string STARTING = "Starting";
            public const string START = "Start";

            public const string STOPPING = "Stopping";
            public const string STOP = "Stop";
        }

        public string CurrentState { private set; get;} = Data.OCCUPERENCE;

        public readonly object Locker = new object();

        /// <summary>
        /// Обьект уничтожается.
        /// </summary>
        private bool _isDestroying = false;

        /// <summary>
        /// Объект находится в процессе уничтожения?
        /// </summary>
        public bool IsDestroying
        { 
            get { lock(Locker) { return _isDestroying;} }
        }

        /// <summary>
        /// Отложеное уничтожение. Перед тем как начать уничтожение обьекта
        /// он дожен запустится так как от его работы возможно зависят другие объекты.
        /// </summary>
        private bool _isDeferredDestroying = false;

        /// <summary>
        /// Объект выставлен на отложеное уничтожение?
        /// </summary>
        public bool IsDeferredDestroying 
        {
            get { lock(Locker) { return _isDeferredDestroying; } }
        }

        /// <summary>
        /// Обьект находится в состоянии зародыша?
        /// </summary>
        public bool IsOccuperence 
        { 
            get { return CurrentState == Data.OCCUPERENCE ? true : false;  } 
        }

        /// <summary>
        /// Обьект находится в состоянии сборки?
        /// </summary>
        public bool IsContruction
        { 
            get { return CurrentState == Data.CONSTRUCTION ? true : false;  } 
        }

        /// <summary>
        /// Обьект находится в состоянии запуска.
        /// </summary>
        public bool IsStarting
        { 
            get { return CurrentState == Data.STARTING ? true : false;  } 
        }
        /// <summary>
        /// Обьект запущен?
        /// </summary>
        public bool IsStart
        { 
            get { return CurrentState == Data.START ? true : false;  } 
        }
        /// <summary>
        /// Обьект останавливается?
        /// </summary>
        public bool IsStopping
        { 
            get { return CurrentState == Data.STOPPING ? true : false;  } 
        }
        /// <summary>
        /// Обьект остановлен?
        /// </summary>
        public bool IsStop
        { 
            get { return CurrentState == Data.STOP ? true : false;  } 
        }

        void state.IManager.Set(string state) 
        {
            CurrentState = state;
        }

        string state.IManager.Get()
        {
            return CurrentState;
        }

        void state.IManager.Destroy()
        {
            lock(Locker)
            {
                _isDestroying = true;
            }
        }

        void state.IManager.DeferredDestroy()
        {
            lock(Locker)
            {
                _isDeferredDestroying = true;
            }
        }

        /// <summary>
        /// Менеджер для управления состояние обьекта. 
        /// </summary>
        public class Manager : Informing
        {
            private readonly state.IManager _stateManager;

            public Manager(informing.IMain mainInforming, state.IManager stateManager)
                : base("StateManager", mainInforming)
            {
                _stateManager = stateManager;
            }

            /// <summary>
            /// Сменить состояние обьекта с проверкой на текущее состояние. 
            /// </summary>
            /// <param name="state">Состояние на которое нужно сменить.</param>
            /// <param name="currentState">Текущее состояние в котором должен пребывать обьект.</param>
            public bool Replace(string state, params string[] currentState)
            {
                string s = _stateManager.Get();

                if (currentState.Contains(s))
                {
                    SystemInformation($"State replace:{s}->{state}.");

                    _stateManager.Set(state);

                    return true;
                }
                else 
                {
                    Exception(data.StateManager.x100001, _stateManager.Get(), state, String.Join(", ", currentState));

                    return false;
                }
            }

            /// <summary>
            /// Выставить обьекту задачу получения состояния уничтожен.
            /// </summary>
            public void Destroy() => _stateManager.Destroy();

            /// <summary>
            /// Выставить обьекту задачу при первой возможности начать получение состояния уничтожен. 
            /// </summary>
            public void DeferredDestroy() => _stateManager.DeferredDestroy();
        }
    }
}
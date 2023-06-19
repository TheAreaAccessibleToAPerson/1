namespace Thread
{
    public enum Priority
    {
        Lowest = 0,
        BelowNormal = 1,
        Normal = 2,
        AboveNormal = 3,
        Highest = 4
    }
}

namespace Gudron
{
    public class ThreadController
    {
    }
}

namespace Gudron.system.objects.main.manager
{

    public sealed class Threads : Informing, dispatcher.IThreads
    {
        private readonly information.State _stateInformation;

        public Threads(informing.IMain mainInforming, information.State stateInformation)
            : base("ThreadsManager", mainInforming)
        {
            _stateInformation = stateInformation;
        }

        private global::System.Threading.Thread[] _threads = new global::System.Threading.Thread[0];
        private int[] _timesDelay = new int[0];
        private string[] _prioritys = new string[0];

        private Bool[] IsRuns = new Bool[0];

        public void Add(string name, global::System.Action action, uint timeDelay, Thread.Priority priority)
        {
            if (_stateInformation.IsStarting)
            {
                IsRuns = Hellper.ExpendArray(IsRuns, new Bool(true));
                _prioritys = Hellper.ExpendArray(_prioritys, priority.ToString());
                _timesDelay = Hellper.ExpendArray(_timesDelay, (int)timeDelay);

                if (timeDelay > 0)
                {
                    _threads = Hellper.ExpendArray(_threads, new global::System.Threading.Thread(() =>
                    {
                        int timeDelay = _timesDelay[_timesDelay.Length - 1];
                        Bool isRun = IsRuns[IsRuns.Length - 1];

                        while (true)
                        {
                            if (isRun.Value)
                            {
                                action.Invoke();

                                global::System.Threading.Thread.Sleep(timeDelay);
                            }
                            else
                            {
                                return;
                            }

                        }
                    }));
                }
                else
                {
                    _threads = Hellper.ExpendArray(_threads, new global::System.Threading.Thread(() =>
                    {
                        Bool isRun = IsRuns[IsRuns.Length - 1];

                        while (true)
                        {
                            if (isRun.Value)
                            {
                                action.Invoke();
                            }
                            else
                            {
                                return;
                            }
                        }
                    }));
                }

                _threads[_threads.Length - 1].Name = name;
                _threads[_threads.Length - 1].Priority = (global::System.Threading.ThreadPriority)priority;
            }
            else
                Exception(data.ThreadManager.x100001, name);
        }

        void dispatcher.IThreads.Start()
        {
            foreach (var thread in _threads)
                thread.Start();
        }

        public void Stop()
        {
            if (_threads.Length == 0) return;

            string nameThreads = "";
            foreach(var nameThread in _threads)
                nameThreads += nameThread.Name + " ";


            SystemInformation($"StoppingThreads:{nameThreads}");

            for (int i = 0; i < IsRuns.Length; i++)
                IsRuns[i].False();

            bool[] isStopThreads = new bool[_threads.Length];

            int stopThreadsCount = 0;
            while (true)
            {
                for (int i = 0; i < _threads.Length; i++)
                {
                    if (isStopThreads[i] == false)
                    {
                        if (_threads[i].IsAlive)
                        {
                            //...
                        }
                        else
                        {
                            isStopThreads[i] = true;
                            stopThreadsCount++;

                            SystemInformation($"StopThread:" + _threads[i].Name);

                            if (stopThreadsCount == _threads.Length) break;
                        }
                    }
                }
                
                if (stopThreadsCount == _threads.Length) break;
            }

            SystemInformation($"StopThread.");
        }
    }

    public class ThreadBuffer
    {
        public readonly string Name;
        public global::System.Threading.Tasks.Task Thread;
        private global::System.Action BreakTheBlockAction;

        public ThreadBuffer(string name, global::System.Threading.Tasks.Task thread, global::System.Action breakTheBlockAction)
        {
            Name = name;
            Thread = thread;
            BreakTheBlockAction = breakTheBlockAction;
        }

        public void BreakTheBlock()
        {
            if (BreakTheBlockAction != null)
            {
                BreakTheBlockAction.Invoke();
            }
        }
    }
}
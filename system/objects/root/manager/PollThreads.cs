using System.Threading;
namespace Gudron
{
    public class Events
    {
        public class PollInformation
        {
            public string Name { get; init; }
            public uint TimeDelay { get; init; }
            public Thread.Priority ThreadPriority { get; init; }
            public bool IsDestroy{ get; init; } = true;
        }

        private static Dictionary<string, PollInformation> _values = new Dictionary<string, PollInformation>();

        public static void AddSetting(string name, uint timeDelay, Thread.Priority threadPriority)
            => _values.Add
                (name, new PollInformation()
                {
                    Name = name,
                    TimeDelay = timeDelay,
                    ThreadPriority = threadPriority
                });

        public static bool GetSetting(string name, out PollInformation information)
            => _values.TryGetValue(name, out information);
    }
}

namespace Gudron.system.objects.root.manager
{
    public sealed class PollThreads : Controller
    {
        private readonly Dictionary<string, Poll> _values = new Dictionary<string, Poll>();
        private readonly object _locker = new object();

        /// <summary>
        /// Передаем в нужные потоки регистрационые билеты.
        /// </summary>
        public void Add(main.SubscribePollTicket[] tickets)
        {
            lock (_locker)
            {
                for (int i = 0; i < tickets.Length; i++)
                {
                    if (_values.TryGetValue(tickets[i].Name, out Poll poll))
                    {
                        poll.Add(tickets[i]);
                    }
                    else 
                        Creating(tickets[i]);
                }
            }
        }

        private Poll Creating(main.SubscribePollTicket ticket)
        {
            if (Events.GetSetting(ticket.Name, out Events.PollInformation eventInformation))
            {
                Poll poll = obj<Poll>($"Poll[Name:{ticket.Name}, TimeDelay:{eventInformation.TimeDelay}," +
                    $" ThreadPriority:{eventInformation.ThreadPriority}, IsDestroy:{eventInformation.IsDestroy}].",
                        new poll.Fields()
                        {
                            Name = eventInformation.Name,
                            TimeDelay = eventInformation.TimeDelay,
                            ThreadPriority = eventInformation.ThreadPriority,
                            IsDestroy = eventInformation.IsDestroy,
                            Destroy = Destroy
                        });

                poll.Add(ticket);
            }
            else 
                Exception ($"Вы пытаетесь добавить событие {ticket.Name}, но вы не добавили для него настройки с помощью" +
                    $" статического метода Events.SetSetting(...)");

            return default;
        }

        private bool Destroy(Poll poll)
        {
            return false;
        }

        private long UniqueID = 0;
    }
}
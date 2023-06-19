namespace Gudron.system.objects.main
{
    public struct SubscribePollTicket
    {
        /// <summary>
        /// Настройка для пулла. Имя.
        /// </summary>
        public string Name;

        /// <summary>
        /// Ссылка на Action которую будет обрабатывать пулл.
        /// </summary>
        public Action Action;

        /// <summary>
        /// Ссылка на метод в PollManager с помощью которого пулл будет общатся со своим подписоным обьектом. 
        /// 1) Тип информирования.
        /// 2) Номер индекса в массиве SubsribePollManager.
        /// 3) ID пулла.
        /// 4) Номер индекса в массиве RootPollClients.
        /// </summary>
        public Action<root.poll.InformingType, int, ulong, int> Informing;

        /// <summary>
        /// ID обьекта(узла) который регистрирует свои подписки и своих веток в пулл.
        /// </summary>
        public ulong IDObject;

        /// <summary>
        /// Индекс в массиве где хранится информация о регистрации в PollManager.  
        /// </summary>
        public int IndexInSubscribePollManager;
    }

    public struct UnsubscribePollTicket
    {
        /// <summary>
        /// ID пулла на который мы подписаны. 
        /// </summary>
        public ulong PollID;

        /// <summary>
        /// ID обьекта(узла) который регистрирует свои отписик и своих веток в пулл.
        /// </summary>
        public ulong IDObject;

        /// <summary>
        /// Индекс в пулле на который мы подписались. 
        /// </summary>
        public int IndexInRootPoll;

        /// <summary>
        /// Индекс в массиве subsribePollMangaer где хранится 
        /// вся информация о пуле в котором мы работаем.
        /// </summary>
        public int IndexInSubscribePollManager;
    }

    public struct StateType
    {
        /// <summary>
        /// Создание билетов.
        /// </summary>
        public const string CREATING_TICKET = "CreatingTicket";

        /// <summary>
        /// Регистрируем подписки.
        /// </summary>
        public const string REGISTER_SUBSCRIBE = "RegisterSubscribe";

        /// <summary>
        /// Все подписки зарегистрировались.
        /// </summary>
        public const string END_SUBSCRIBE = "EndSubscribe";

        /// <summary>
        /// Регистрируем все отписки.
        /// </summary>
        public const string REGISTER_UNSUBSCRIBE = "RegisterUnsubscribe";

        /// <summary>
        /// Мы отписались ото всех отписок.
        /// </summary>
        public const string END_UNSUBSCRIBE = "EndUnsubscribe";
    }
}
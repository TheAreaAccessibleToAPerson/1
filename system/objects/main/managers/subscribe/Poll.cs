namespace Gudron.system.objects.main.controller
{
    public class Poll : Informing
    {
        private readonly manager.description.ISubscribe _subscribeManager;

        private readonly information.State _stateInformation;
        private readonly information.Header _headerInformation;
        private readonly information.DOM _DOMInformation;

        /// <summary>
        /// Текущее состояние.
        /// </summary>
        private string State = StateType.CREATING_TICKET;

        private SubscribePollTicket[] _subscribeTickets = new SubscribePollTicket[0];

        /// <summary>
        /// Сдесь хранятся индексы пуллов в которых работают наши Action.
        /// </summary>
        private ulong[] _pointerToThePollID;
        /// <summary>
        /// Указан индекс в массиве в менеджере пуллов где хранится наш обрабатываемый Action.
        /// </summary>
        private int[] _pointerIndexInArrayToThePoll;

        /// <summary>
        /// Количесво подписаных билетов. Это значение нужно для того что бы при уничтожении
        /// обьекта дождатся пока мы отключимся от всех пулов на которые подписались.
        /// </summary>
        private int _subscribeTicketCount = 0;

        private readonly object _locker = new object();

        public Poll(informing.IMain mainInforming, information.State stateInformation,
            information.Header headerInformation, information.DOM DOMInformation,
                manager.description.ISubscribe subscribeManager)
            : base("SubscribeManager", mainInforming)
        {
            _stateInformation = stateInformation;
            _headerInformation = headerInformation;
            _DOMInformation = DOMInformation;

            _subscribeManager = subscribeManager;
        }

        /// <summary>
        /// Ссылка на метод в PollManager с помощью которого пулл будет общатся со своим подписоным обьектом. 
        /// 1) Тип информирования.
        /// 2) Номер индекса в массиве SubsribePollManager.
        /// 3) ID пулла.
        /// 4) Номер индекса в массиве RootPollClients.
        /// </summary>
        public void ToInforming(root.poll.InformingType informingType, int indexSubsribePollManager,
            ulong idPoll, int indexRootPollClients)
        {
            lock (_locker)
            {
                // Сообщает что мы отовсюду отписались;
                if (informingType.HasFlag(root.poll.InformingType.EndUnsubscribe))
                {
                    if (State == StateType.REGISTER_UNSUBSCRIBE)
                    {
                        if (_pointerToThePollID[indexSubsribePollManager] != 0)
                        {
                            if (_pointerToThePollID[indexSubsribePollManager] == idPoll &&
                                _pointerIndexInArrayToThePoll[indexSubsribePollManager] == indexRootPollClients)
                            {
                                if (--_subscribeTicketCount == 0)
                                {
                                    State = StateType.END_UNSUBSCRIBE;

                                    _subscribeManager.EndUnsubscribe();
                                }
                            }
                        }
                    }
                }
                else if (informingType.HasFlag(root.poll.InformingType.ChangeOfIndex))
                {
                    _pointerToThePollID[indexSubsribePollManager] = idPoll;
                    _pointerIndexInArrayToThePoll[indexSubsribePollManager] = indexRootPollClients;

                    // В момент отписки наше место положение изменилось, повторное информирование
                    // с новым местом лежит на нас.
                    if (State == StateType.REGISTER_UNSUBSCRIBE)
                    {
                        _DOMInformation.RootManager.ActionInvoke(()
                            => RepeetUnsubscribe(indexSubsribePollManager));
                    }
                }
                else if (informingType.HasFlag(root.poll.InformingType.EndSubscribe))
                {
                    if (State == StateType.REGISTER_SUBSCRIBE)
                    {
                        // Запишем куда зарегестрировался наш тикет.
                        _pointerToThePollID[indexSubsribePollManager] = idPoll;
                        _pointerIndexInArrayToThePoll[indexSubsribePollManager] = indexRootPollClients;

                        // Дожидаемся пока все билеты откликрутся о завершении регистрации.
                        if (++_subscribeTicketCount == _pointerToThePollID.Length)
                        {
                            State = StateType.END_SUBSCRIBE;

                            _subscribeManager.EndSubscribe();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Если вовремя отписки, место положения нашего тикета изменилось, то нужно продублировать регистрацию, но уже в новое место.
        /// </summary>
        private void RepeetUnsubscribe(int indexSubsribePollManager)
        {
            /*
            UnsubscribePollTicket ticket = new UnsubscribePollTicket(root.poll.InformingType.);

            poll.data.ticket.Struct structUnsubscribeTicket = new poll.data.ticket.Struct
                                        (poll.data.ticket.Struct.Type.UNSUBSCRIBE, pIndexTicketInSubscribeManager, PointerToThePollID[pIndexTicketInSubscribeManager],
                                            PointerIndexInArrayToThePoll[pIndexTicketInSubscribeManager], NodeInformation.ID);

            NodeInformation.SystemAccess.AddTicketsToThePoll(new poll.data.ticket.Struct[] { structUnsubscribeTicket });
            */
        }

        /// <summary>
        /// Добавляет регистриационый билет для пулла потоков. 
        /// </summary>
        /// <param name="name">Имя пулла потоков куда нам будет неоходимо произвести подписку.</param>
        /// <param name="action">Action который нам предстоит обрабатывать.</param>
        public void Add(string name, Action action)
        {
            if (_headerInformation.IsNodeObject())
            {
                SubscribePollTicket ticket = new SubscribePollTicket()
                {
                    Name = name,
                    Action = action,
                    Informing = ToInforming,
                    IDObject = _DOMInformation.ID,
                    IndexInSubscribePollManager = _subscribeTicketCount
                };

                Hellper.ExpendArray(ref _subscribeTickets, ticket);
            }
            else
                ((main.description.IPoll)_DOMInformation.NodeObject).Add(name, action);
        }

        public void Subscribe()
        {
            if (State == StateType.CREATING_TICKET)
            {
                State = StateType.REGISTER_SUBSCRIBE;

                // Сюда запишутся данные о том в каком пуле зарегестрирован наш билет.
                // Если в последсвии билет будет передан в другой пулл, то
                // он обязан будет сообщить о том где он в данный момент работает.
                _pointerToThePollID = new ulong[_subscribeTickets.Length];

                _pointerIndexInArrayToThePoll = new int[_subscribeTickets.Length];
                // Поставим в очередь на регистрацию.
                _DOMInformation.RootManager.AddSubscribeTickets(_subscribeTickets);
            }
        }

        /// <summary>
        /// Регистрируемся на отписку из пуллов.
        /// </summary>
        private void Unsubscribe()
        {
            /*
            if (SubscribeTicketCount > 0)
            {
                State = StateType.REGISTER_UNSUBSCRIBE;

                poll.data.ticket.Struct[] structUnsubscribeTickets
                = new poll.data.ticket.Struct[StructSubscribeTickets.Length];

                for (int i = 0; i < StructSubscribeTickets.Length; i++)
                {
                    structUnsubscribeTickets[i] = new poll.data.ticket.Struct
                        (poll.data.ticket.Struct.Type.UNSUBSCRIBE, i, PointerToThePollID[i], PointerIndexInArrayToThePoll[i], NodeInformation.ID);
                }

                NodeInformation.SystemAccess.AddTicketsToThePoll(structUnsubscribeTickets);
            }
            */
        }
    }
}
namespace Gudron.system.objects.root.manager
{
    public abstract class Clients : Controller.Board.LocalField<poll.Fields>
    {
        /// <summary>
        /// Текущее количесво клиентов. 
        /// </summary>
        /// <value></value>
        public int Count { private set; get; } = 0;

        /// <summary>
        /// ID текущего пулла. 
        /// </summary>
        /// <value></value>
        public ulong ID { get { return Field.ID; } }

        /// <summary>
        /// Имя текущего пулла. 
        /// </summary>
        /// <value></value>
        public string Name { get { return Field.Name; } }

        /// <summary>
        /// Xранятся исполняемые Actions клинтов.
        /// </summary>
        private readonly Action[] _runs = new Action[128];

        /// <summary>
        /// Хранит уникальные id клинтов. 
        /// </summary>
        private readonly ulong[] _idClients = new ulong[128];

        /// <summary>
        /// Хранятся методы для информирования клинта.
        /// </summary>
        private readonly Action<root.poll.InformingType, int, ulong, int>[] _toInformingClients
            = new Action<root.poll.InformingType, int, ulong, int>[128];

        /// <summary>
        /// Индекс в массиве в PollSubscribeManager. 
        /// </summary>
        private readonly int[] _indexInSubscribePollManager = new int[128];

        /// <summary>
        /// Интекс пустого слота под клинта. 
        /// </summary>
        private int _indexEmptySlot = 0;

        public void Run()
        {
            for (int i = 0; i < _indexEmptySlot; i++)
                _runs[i].Invoke();
        }

        public void Add(object[] tickets)
        {
            foreach (object ticket in tickets)
                Add(ticket);
        }

        public void Add(object ticket)
        {
            if (ticket is main.SubscribePollTicket subscribeTicket)
            {
                _runs[_indexEmptySlot] = subscribeTicket.Action;
                _toInformingClients[_indexEmptySlot] = subscribeTicket.Informing;
                _idClients[_indexEmptySlot] = subscribeTicket.IDObject;
                _indexInSubscribePollManager[_indexEmptySlot] = subscribeTicket.IndexInSubscribePollManager;

                subscribeTicket.Informing.Invoke(root.poll.InformingType.EndSubscribe,
                    subscribeTicket.IndexInSubscribePollManager, ID, _indexEmptySlot);

                _indexEmptySlot++;
            }
            else if (ticket is main.UnsubscribePollTicket unsubscribeTicket)
            {
                // Индекс по которому преположительно хранится наш клиент.
                int index = unsubscribeTicket.IndexInRootPoll;

                // Проверяем не переместили ли клинта в другой слот.
                if (_idClients[index] == unsubscribeTicket.IDObject)
                {
                    // Один и тот же клиент может несколько раз подписаться в один и тот же пулл.
                    // проверяем номер билета на совподение.
                    if (_indexInSubscribePollManager[index]
                        == unsubscribeTicket.IndexInSubscribePollManager)
                    {
                        // Проинформирем клинта что он отключон.
                        _toInformingClients[index].Invoke(root.poll.InformingType.EndUnsubscribe,
                            _indexInSubscribePollManager[index], ID, index);

                        _runs[index] = null;
                        _toInformingClients[index] = null; 
                        _idClients[index] = ulong.MaxValue;
                        _indexInSubscribePollManager[index] = -1;

                        // Если это был единсвеный клиент.
                        if ((_indexEmptySlot - 1) < 0) return;

                        // Если освобожденый слот и был крайним. То инкрементируем 
                        // край массива с данными клиетов, и перейдем к обработки следующего билета.
                        if ((_indexEmptySlot - 1) == index)
                        {
                            _indexEmptySlot--;
                            return;
                        }

                        // Если индекс отписаного клинта выше чем крайний клиент,
                        // то перейдем к следующему билету.
                        if ((_indexEmptySlot - 1) < index)
                        {
                            return;
                        }

                        // Декриментируем крайнее значение. Так как крайний клиент переедит 
                        // в освободившийся слот.
                        _indexEmptySlot--;

                        _runs[index] = _runs[_indexEmptySlot];
                        _toInformingClients[index] = _toInformingClients[_indexEmptySlot];
                        _idClients[index] = _idClients[_indexEmptySlot];
                        _indexInSubscribePollManager[index] = _indexInSubscribePollManager[index];

                        _toInformingClients[index].Invoke(root.poll.InformingType.ChangeOfIndex,
                                _indexInSubscribePollManager[index], ID, index);
                    }
                }
            }
            else
                throw new Exception();
        }
    }
}
namespace Gudron.system.objects.root
{
    /// <summary>
    /// Описывает методы для работы обьектов с Root.
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// Добавить в очередь на вызов Action в системный поток. 
        /// </summary>
        public void ActionInvoke(global::System.Action action);

        /// <summary>
        /// Добавить регистрационые билеты для Threads пулла. 
        /// </summary>
        /// <param name="tickets"></param>
        public void AddSubscribeTickets(main.SubscribePollTicket[] tickets);
    }
}
namespace Gudron.system.objects.main
{
    /// <summary>
    /// Описывает способ подключения к обьекту 
    /// реализующего данный интерфейс.
    /// </summary>
    public interface IInputConnect
    {
        /// <summary>
        /// Возращает ссылку на обьект. 
        /// </summary>
        /// <returns></returns>
        public object GetConnect();
    }

    /// <summary>
    /// Описывает способ подлючения к обьекту.
    /// </summary>
    public interface IInputConnected
    {
        /// <summary>
        /// Получает обьект с которым нужно установить соединение.
        /// </summary>
        /// <param name="inputConnect"></param>
        public void SetConnected(object inputConnect);
    }
}
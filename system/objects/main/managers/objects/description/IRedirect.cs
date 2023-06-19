namespace Gudron.system.objects.main
{
    /// <summary>
    /// Описывает методы для перенаправления данных в другие участки программы. 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    public interface IRedirect<T1>
    {
        public void output_to(Action<T1> action);
        public IRedirect<OutputValueType> output_to<OutputValueType>(Func<T1, OutputValueType> func);
        public void send_message_to(string name);
        public IRedirect<T1> send_echo_to(string name);
    }

    /// <summary>
    /// Описывает методы для перенаправления данных в другие участки программы. 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public interface IRedirect<T1, T2>
    {
        public void output_to(Action<T1, T2> action);
        public IRedirect<OutputValueType> output_to<OutputValueType>(Func<T1, T2, OutputValueType> action);
    }

    /// <summary>
    /// Описывает методы для перенаправления данных в другие участки программы. 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public interface IRedirect<T1, T2, T3>
    {
        public void output_to(Action<T1, T2, T3> action);
        public IRedirect<OutputValueType> output_to<OutputValueType>(Func<T1, T2, T3, OutputValueType> action);
    }

    /// <summary>
    /// Описывает методы для перенаправления данных в другие участки программы. 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    public interface IRedirect<T1, T2, T3, T4>
    {
        public void output_to(Action<T1, T2, T3, T4> action);
        public IRedirect<OutputValueType> output_to<OutputValueType>(Func<T1, T2, T3, T4, OutputValueType> action);
    }
}
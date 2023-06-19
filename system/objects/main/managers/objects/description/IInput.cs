namespace Gudron
{
    /// <summary>
    /// Описывает способ доставки данных. 
    /// </summary>
    public interface IInput<T>
    {
        public void To(T value);
    }

    /// <summary>
    /// Описывает способ доставки данных. 
    /// </summary>
    public interface IInput<T1, T2>
    {
        public void To(T1 value1, T2 value2);
    }

    /// <summary>
    /// Описывает способ доставки данных. 
    /// </summary>
    public interface IInput<T1, T2, T3>
    {
        public void To(T1 value1, T2 value2, T3 value3);
    }

    /// <summary>
    /// Описывает способ доставки данных. 
    /// </summary>
    public interface IInput<T1, T2, T3, T4>
    {
        public void To(T1 value1, T2 value2, T3 value3, T4 value4);
    }

    /// <summary>
    /// Описывает способ доставки данных. 
    /// </summary>
    public interface IInput<T1, T2, T3, T4, T5>
    {
        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5);
    }

    /// <summary>
    /// Описывает способ доставки данных. 
    /// </summary>
    public interface IInput<T1, T2, T3, T4, T5, T6>
    {
        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6);
    }

    /// <summary>
    /// Описывает способ доставки данных. 
    /// </summary>
    public interface IInput<T1, T2, T3, T4, T5, T6, T7>
    {
        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7);
    }

    /// <summary>
    /// Описывает способ доставки данных. 
    /// </summary>
    public interface IInput<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8);
    }

    /// <summary>
    /// Описывает способ доставки данных. 
    /// </summary>
    public interface IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9);
    }

    /// <summary>
    /// Описывает способ доставки данных. 
    /// </summary>
    public interface IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10);
    }
}
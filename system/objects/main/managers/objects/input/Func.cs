namespace Gudron.system.objects.main
{
    public sealed class FuncObject<T1, R> : Redirect<R>, IInput<T1>
    {
        private readonly Func<T1, R> _func;

        public FuncObject(ref IInput<T1> input, Func<T1, R> func, IInformation information) 
            : base (information)
            {
                _func = func;
                input = this;
            }

        public void To(T1 value) 
            => input.To(_func(value));
    }

    public sealed class FuncObject<T1, T2, R> : Redirect<R>, IInput<T1, T2>
    {
        private readonly Func<T1, T2, R> _func;

        public FuncObject(ref IInput<T1, T2> input, Func<T1, T2, R> func, IInformation information)
            : base (information)
            {
                _func = func;
                input = this;
            }

        public void To(T1 value1, T2 value2) 
            => input.To(_func(value1, value2));
    }

    public sealed class FuncObject<T1, T2, T3, R> : Redirect<R>, IInput<T1, T2, T3>
    {
        private readonly Func<T1, T2, T3, R> _func;

        public FuncObject(ref IInput<T1, T2, T3> input, Func<T1, T2, T3, R> func, IInformation information)
            : base (information)
            {
                _func = func;
                input = this;
            }

        public void To(T1 value1, T2 value2, T3 value3) 
            => input.To(_func(value1, value2, value3));
    }

    public sealed class FuncObject<T1, T2, T3, T4, R> 
        : Redirect<R>, IInput<T1, T2, T3, T4>
    {
        private readonly Func<T1, T2, T3, T4, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4> input, Func<T1, T2, T3, T4, R> func, IInformation information)
            : base (information)
            {
                _func = func;
                input = this;
            }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4) 
            => input.To(_func(value1, value2, value3, value4));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, R> 
        : Redirect<R>, IInput<T1, T2, T3, T4, T5>
    {
        private readonly Func<T1, T2, T3, T4, T5, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5> input, Func<T1, T2, T3, T4, T5, R> func, IInformation information)
            : base (information)
            {
                _func = func;
                input = this;
            }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) 
            => input.To(_func(value1, value2, value3, value4, value5));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, T6, R> 
        : Redirect<R>, IInput<T1, T2, T3, T4, T5, T6>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5, T6> input, 
            Func<T1, T2, T3, T4, T5, T6, R> func, IInformation information)
            : base (information)
            {
                _func = func;
                input = this;
            }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) 
            => input.To(_func(value1, value2, value3, value4, value5, value6));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, T6, T7, R> 
        : Redirect<R>, IInput<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5, T6, T7> input, 
            Func<T1, T2, T3, T4, T5, T6, T7, R> func, IInformation information)
            : base (information)
            {
                _func = func;
                input = this;
            }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) 
            => input.To(_func(value1, value2, value3, value4, value5, value6, value7));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, T6, T7, T8, R> 
        : Redirect<R>, IInput<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8> input, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, IInformation information)
            : base (information)
            {
                _func = func;
                input = this;
            }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) 
            => input.To(_func(value1, value2, value3, value4, value5, value6, value7, value8));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> 
        : Redirect<R>, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9> input, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> func, IInformation information)
            : base (information)
            {
                _func = func;
                input = this;
            }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9) 
            => input.To(_func(value1, value2, value3, value4, value5, value6, value7, value8, value9));
    }

    public sealed class FuncObject<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> 
        : Redirect<R>, IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> _func;

        public FuncObject(ref IInput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> input, 
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> func, IInformation information)
            : base (information)
            {
                _func = func;
                input = this;
            }

        public void To(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10) 
            => input.To(_func(value1, value2, value3, value4, value5, value6, value7, value8, value9, value10));
    }
}
namespace Gudron
{
    /// <summary>
    /// Каждый обьект учавствующий в системе должен наследоваться от данного абстрактного класса. 
    /// Обьекты создаются с помощью метода obj<ObjectType>(string, obj[]) и имеют 2 типа и 2 вида:
    /// 1) Первый тип это обычный Controller. Данный обьект является неотъемлемой частю своего родителя.
    /// При его уничтожении через публичный метод destroy(), уничтожится и его родитель.
    /// 2) Второй тип это Controller.Board. Данный объект представляет новое и отдельное от своего родителя
    /// отвитвление. Жизнь родителя не как не связана с данным обьектом.
    ///
    /// В зависимости от того в каком системном методе будет создан обьект, автоматически определится его вид.
    /// В системном методе void Contruction() можно создать явно или не явно через системные методы input_to, output_to
    /// отьекты типа Controller, которые реализуют идею ветки, а обьект родитель будет для них узлом. В методе void Start()
    /// так же можно создать обьект типа Controller только через метод obj<ObjectType>(string, obj[]), данный обьект
    /// будет являеться новым узлом, но всеравно он реализует идею текущего обьект, так как он связывает родительскую
    /// жизнь со своей.
    /// 
    /// Создавая обьекты и устанавливая связи в методе void Contruction() нельзя обращаться к ним.
    /// Когда запустится метод void Start() обьекты, подключния, подписки, созданые реанее в методе void Contruction() 
    /// будут готовы
    /// 
    /// ....
    /// </summary>
    public abstract class Controller : system.objects.main.Object
    {
        public Controller()
            : base(system.objects.main.information.Header.Data.CONTROLLER) { }

        public Controller(string type) : base(type) { }

        public abstract class LocalField<F> : Controller, ILocalField
        {
            protected F Field;

            void ILocalField.SetField(object value)
            {
                if (value is F valueReduse)
                {
                    Field = valueReduse;
                }
                else
                    Exception($"При создании обьекта вы передали локальное значение типа {value.GetType()}" +
                              $", но при создании обьекта указали ожидаемый тип {typeof(F).FullName}");
            }
        }

        public abstract class Output<T> : Controller, system.objects.main.IRedirect<T>
        {
            public Output() { }

            protected IInput<T> input;

            protected void output(T value) => input.To(value);

            public void output_to(Action<T> action)
                => new system.objects.main.ActionObject<T>(ref input, action);

            public system.objects.main.IRedirect<OutputValueType> output_to<OutputValueType>(Func<T, OutputValueType> func)
                => new system.objects.main.FuncObject<T, OutputValueType>(ref input, func, this);

            public void send_message_to(string name)
                => ((system.objects.main.IInformation)this).GetGlobalObjectsManager().Get
                    <system.objects.main.ListenMessage<T>, IInput<T>>(name, ref input);

            public system.objects.main.IRedirect<T> send_echo_to(string name)
                => ((system.objects.main.IInformation)this).GetGlobalObjectsManager().Get
                        <system.objects.main.ListenEcho<T, T>,
                        system.objects.main.SendEcho<T, T>,
                        IInput<T>,
                        system.objects.main.IRedirect<T>>
                    (name, ref input, new system.objects.main.SendEcho<T, T>(this));

            public abstract class LocalField<F> : Output<T>, ILocalField
            {
                protected F Field;

                void ILocalField.SetField(object value)
                {
                    if (value is F valueReduse)
                    {
                        Field = valueReduse;
                    }
                    else
                        Exception($"При создании обьекта вы передали локальное значение типа {value.GetType()}" +
                                  $", но при создании обьекта указали ожидаемый тип {typeof(F).FullName}");
                }
            }
        }

        public abstract class Board : Controller
        {
            public Board() : base(system.objects.main.information.Header.Data.BOARD) { }

            public abstract class Output<T> : Board, system.objects.main.IRedirect<T>
            {
                public Output() { }

                protected IInput<T> input;

                protected void output(T value) => input.To(value);

                public void output_to(Action<T> action)
                    => new system.objects.main.ActionObject<T>(ref input, action);

                public system.objects.main.IRedirect<OutputValueType> output_to<OutputValueType>(Func<T, OutputValueType> func)
                    => new system.objects.main.FuncObject<T, OutputValueType>(ref input, func, this);

                public void send_message_to(string name)
                    => ((system.objects.main.IInformation)this).GetGlobalObjectsManager().Get
                        <system.objects.main.ListenMessage<T>, IInput<T>>(name, ref input);

                public system.objects.main.IRedirect<T> send_echo_to(string name)
                    => ((system.objects.main.IInformation)this).GetGlobalObjectsManager().Get
                            <system.objects.main.ListenEcho<T, T>,
                            system.objects.main.SendEcho<T, T>,
                            IInput<T>,
                            system.objects.main.IRedirect<T>>
                        (name, ref input, new system.objects.main.SendEcho<T, T>(this));
            }
        }
    }

    public interface ILocalField
    {
        public void SetField(object value);
    }
}
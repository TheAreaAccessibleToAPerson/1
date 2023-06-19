namespace Gudron.system.objects.main.manager
{
    public sealed class GlobalObjects : Informing, IGlobalObjects
    {
        private readonly Dictionary<string, object> _values;

        /// <summary>
        /// Ключи обьектов созданых в текущем обьекте. 
        /// </summary>
        private string[] _creatingObjectKeys = new string[0];

        private readonly information.Header _headerInformation;
        private readonly information.State _stateInformation;
        private readonly information.DOM _DOMInformation;

        public GlobalObjects(informing.IMain mainInforming, Dictionary<string, object> globalObjects,
            information.Header headerInformation, information.State stateInformation, information.DOM DOMInformation)
            : base("GlobalObjectsManager", mainInforming)
        {
            _values = globalObjects;

            _headerInformation = headerInformation;
            _stateInformation = stateInformation;
            _DOMInformation = DOMInformation;
        }

        public RedirectType Add<GlobalObjectType, RedirectType, InputType>
            (string key, ref InputType input, GlobalObjectType value)
                where GlobalObjectType : InputType, RedirectType
                    => Hellper.GetInput<GlobalObjectType, InputType>
                        (ref input, Add(key, value));

        public RedirectType Add<GlobalObjectType, RedirectType>
            (string key, GlobalObjectType value)
                where GlobalObjectType : RedirectType
                    => Add(key, value);

        public RedirectType Get<GlobalObjectType, LocalObjectType, InputType, RedirectType>
            (string key, ref InputType input, LocalObjectType localObject)
                where LocalObjectType : InputType, RedirectType, IInputConnected
                    where GlobalObjectType : IInformation, IInputConnect
                        => Hellper.SetConnected<LocalObjectType, InputType, GlobalObjectType>
                            (ref input, localObject, Get<GlobalObjectType>(key));

        public void Get<GlobalObjectType, LocalObjectType>
            (string key, LocalObjectType localObject)
                where LocalObjectType : IInputConnected
                    where GlobalObjectType : IInformation, IInputConnect
                        => localObject.SetConnected(Get<GlobalObjectType>(key).GetConnect());

        public void Get<GlobalObjectType, InputType>
            (string key, ref InputType input)
                where GlobalObjectType : InputType, IInformation
                    => input = Get<GlobalObjectType>(key);

        public GlobalObjectType Get<GlobalObjectType>(string key)
            where GlobalObjectType : IInformation
        {
            if (_stateInformation.IsContruction)
            {
                if (_values.TryGetValue(key, out object globalObject))
                {
                    if (globalObject is GlobalObjectType globalObjectReduse)
                    {
                        if (_DOMInformation.IsParentID(globalObjectReduse.GetNodeID()))
                        {
                            return globalObjectReduse;
                        }
                        else
                            Exception($"Глобальный обьект с именем {key} не определен не у одного из ваших родителей. " +
                                $"Обьект с таким именем находится в {globalObjectReduse.GetExplorer()}");
                    }
                    else
                        throw new Exception($"Вы пытаетесь получить глобальный обьект типа {typeof(GlobalObjectType).FullName} по ключу {key}" +
                            $", но под данным ключом находится обьект типа {globalObject.GetType().FullName}.");
                }
                else
                    Exception($"Вы пытаетесь получить несущесвующий глобальный обьект по ключу {key}");
            }
            else
                Exception($"Вы можете установить ссылку на глобальный слушатель сообщений {key} только в методе Contruction().");

            return default;
        }

        public GlobalObjectType Add<GlobalObjectType>(string key, GlobalObjectType value)
        {
            if (_stateInformation.IsContruction)
            {
                if (_values.TryGetValue(key, out object globalObject))
                {
                    if (globalObject is IInformation globalObjectExplorer)
                    {
                        Exception($"Вы уже создали глобальный обьект с именем {key} c типом " +
                            $"{globalObject.GetType().FullName} в {globalObjectExplorer.GetExplorer()}.");
                    }
                    else
                        throw new Exception($"Обьект типа {globalObject.GetType().FullName} не реализует интерфейс {typeof(IInformation).FullName}");
                }
                else
                {
                    _values.Add(key, value);

                    Hellper.ExpendArray(ref _creatingObjectKeys, key);

                    return value;
                }
            }
            else
                Exception($"Вы можете установить ссылку на глобальный слушатель сообщений {key} только в методе Contruction().");

            return default;
        }
    }
}
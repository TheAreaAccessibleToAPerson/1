namespace Gudron.system.objects.main.manager
{
    public sealed class BranchObjects : Informing
    {
        private readonly informing.IMain _mainInforming;

        private readonly Dictionary<string, object> _globalObjects;

        private readonly information.Header _headerInformation;
        private readonly information.State _stateInformation;
        private readonly information.DOM _DOMInformation;

        public BranchObjects(informing.IMain mainInforming, information.Header headerInformation, information.State stateInformation,
            information.DOM DOMInformation, Dictionary<string, object> globalObjects)
            : base("BranchObjectsManager", mainInforming)
        {
            _mainInforming = mainInforming;

            _headerInformation = headerInformation;
            _stateInformation = stateInformation;
            _DOMInformation = DOMInformation;

            _globalObjects = globalObjects;
        }

        private Dictionary<string, main.Object> _values;

        /// <summary>
        /// Текущее количесво собраных обьектов.
        /// </summary>
        private int _collectedCount = 0;

        /// <summary>
        /// Инкрементируем количесво собраных объектов.
        /// </summary>
        public void IncrementCollectedCount()
        {
            if (_values == null)
            {

            }
            else
            {
                if (++_collectedCount == _values.Count)
                {
                }
            }
        }

        public void LifeCyrcle(string state)
        {
            if (_values == null) return;

            switch (state)
            {
                case manager.LifeCyrcle.Data.BEGIN_BRANCH_OBJECT_CONTRUCTION:
                    foreach (main.Object value in _values.Values)
                        ((manager.IDispatcher)value).Process(manager.Dispatcher.Command.CALL_CONSTRUCTION_IN_LIFE_CYRCLE_MANAGER);
                    break;

                case manager.LifeCyrcle.Data.BEGIN_STARTING:
                    foreach (main.Object value in _values.Values)
                        ((manager.IDispatcher)value).Process(manager.Dispatcher.Command.END_SUBSCRIBE);
                    break;

                case manager.LifeCyrcle.Data.CONTINUE_STARTING:
                    foreach (main.Object value in _values.Values)
                        ((manager.IDispatcher)value).Process(manager.Dispatcher.Command.THE_CONTINUE_STARTING);
                    break;
            }
        }

        public BranchObjectType Add<BranchObjectType>(string key, params object[] localFields)
            where BranchObjectType : main.Object, new()
        {
            if (_values == null) _values = new Dictionary<string, Object>();

            if (_values.TryGetValue(key, out main.Object value))
            {
                if (value is BranchObjectType valueReduse)
                {
                    return valueReduse;
                }
                else
                {
                    Exception(data.BranchManager.x100002, typeof(BranchObjectType).FullName, key, value.GetType().FullName);
                }
            }
            else
            {
                BranchObjectType branchObject = new BranchObjectType();

                if (localFields.Length == 1)
                {
                    if (branchObject is ILocalField objectLocalFieldReduse)
                    {
                        objectLocalFieldReduse.SetField(localFields[0]);
                    }
                    else 
                        Exception("Вы передали значение для локального поля обьекта, не не подключили его.");
                }

                ((main.description.IDOM)branchObject).BranchDefine(key, _DOMInformation.NodeID, _DOMInformation.NestingNodeNamberInTheSystem + 1,
                    _DOMInformation.NestingObjectNamberInTheNode + 1, _DOMInformation.ParentObjectsID, _DOMInformation.CurrentObject,
                    _DOMInformation.NodeObject, _DOMInformation.NearIndependentNodeObject, _DOMInformation.RootManager, _globalObjects);

                if (branchObject.HeaderInformation.IsBoard())
                {
                    Exception(data.BranchManager.x100003, typeof(BranchObjectType).FullName, key);
                }
                else
                {
                    _values.Add(key, branchObject);
                }

                return branchObject;
            }

            return default;
        }
    }
}
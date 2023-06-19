namespace Gudron.system.objects.main.manager
{
    public sealed class NodeObjects : Informing, dispatcher.INode
    {
        private readonly informing.IMain _mainInforming;

        private readonly Dictionary<string, object> _globalObjects;

        private readonly information.Header _headerInformation;
        private readonly information.State _stateInformation;
        private readonly information.DOM _DOMInformation;

        public NodeObjects(informing.IMain mainInforming, information.Header headerInformation, information.State stateInformation,
            information.DOM DOMInformation, Dictionary<string, object> globalObject)
            : base("BranchObjectsManager", mainInforming)
        {
            _mainInforming = mainInforming;

            _headerInformation = headerInformation;
            _stateInformation = stateInformation;
            _DOMInformation = DOMInformation;

            _globalObjects = globalObject;
        }

        private Dictionary<string, main.Object> _values;

        /// <summary>
        /// Текущее количесво собираемых обьектов.
        /// </summary>
        private int _collectedCount = 0;

        /// <summary>
        /// Инкрементируем количесво собраных объектов.
        /// </summary>
        public void IncrementCollectedCount()
        {
            lock (_stateInformation.Locker)
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
        }

        public NodeObjectType Add<NodeObjectType>(string key, params object[] localFields)
            where NodeObjectType : main.Object, main.description.IDOM, new()
        {
            lock (_stateInformation.Locker)
            {
                if (_stateInformation.IsStarting || _stateInformation.IsStart)
                {
                    if (_values == null) _values = new Dictionary<string, Object>();

                    if (_values.TryGetValue(key, out main.Object value))
                    {
                        if (value is NodeObjectType valueReduse)
                        {
                            return valueReduse;
                        }
                        else
                            Exception(data.BranchManager.x100002, typeof(NodeObjectType).FullName, key, value.GetType().FullName);
                    }
                    else
                    {
                        NodeObjectType nodeObject = new NodeObjectType();

                        if (localFields.Length == 1)
                        {
                            if (nodeObject is ILocalField objectLocalFieldReduse)
                            {
                                objectLocalFieldReduse.SetField(localFields[0]);
                            }
                            else
                                Exception("Вы передали значение для локального поля обьекта, не не подключили его.");
                        }

                        nodeObject.NodeDefine(key, _DOMInformation.NestingNodeNamberInTheSystem + 1,
                                _DOMInformation.ParentObjectsID, _DOMInformation.CurrentObject,
                                    _DOMInformation.NearIndependentNodeObject, _DOMInformation.RootManager, _globalObjects);

                        _values.Add(key, nodeObject);

                        if (_stateInformation.IsStart)
                        {
                            _collectedCount++;
                            _DOMInformation.RootManager.ActionInvoke(nodeObject.CreatingNode);
                        }

                        return nodeObject;
                    }

                }

                return default;
            }
        }

        void dispatcher.INode.CreatingDeferredObject()
        {
            lock (_stateInformation.Locker)
            {
                if (_values == null) return;

                foreach (main.Object value in _values.Values)
                {
                    _collectedCount++;
                    _DOMInformation.RootManager.ActionInvoke(((main.description.IDOM)value).CreatingNode);
                }
            }
        }
    }
}
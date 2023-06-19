namespace Gudron.system.objects.main.information
{
    public class Header : Informing
    {
        public Header(informing.IMain informing, string type)
            : base("HeaderInformation", informing) 
        {
            if (type == Data.BOARD) Board = true;
        }

        public struct Data
        {
            public const string ROOT = "Root";

            public const string BOARD = "BoardController";
            public const string CONTROLLER = "Controller";

            public const string Node = "Node";
            public const string Branch = "Branch";
        }

        private bool Board;

        public global::System.Type Type { private set; get; }

        public string ObjectType {private set;get;}

        /// <summary>
        /// Имя обьекта.
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// Хранит место положение обьекта относительно все системы.
        /// </summary>
        public string Explorer { private set; get; } = "";

        public string Directory {private set;get;} = "";

        public bool IsSystemController() => Data.ROOT == ObjectType;

        public bool IsNodeObject() => (ObjectType == Data.Node) || (ObjectType == Data.ROOT);

        public bool IsBranchObject() => ObjectType == Data.Branch;

        public bool IsBoard() => Board;

        public void NodeDefine(string directory, global::System.Type type, information.DOM parentDomInformation, string keyObject)
        {
            ObjectType = Data.Node;

            Define(directory, type, parentDomInformation, keyObject);
        }

        public void BranchDefine(string directory, global::System.Type type, information.DOM parentDomInformation, string keyObject)
        {
            ObjectType = Data.Branch;

            Define(directory, type, parentDomInformation, keyObject);
        }

        private void Define(string directory, global::System.Type type, information.DOM parentDomInformation, string keyObject)
        {
            if (type.FullName.Contains("root.Object"))
            {
                ObjectType = Data.ROOT;
                Name = "system";
            }
            else 
                Name = type.Name;
            
            string info = "";
            if (IsBoard() && IsSystemController())
                info = "[R]";
            else if (IsBoard())
                info = "[B]";
            else 
                info = "[I]";

            Directory = $"{directory}/{Name}";
            Explorer = $"{info}{Directory}";
        }
    }
}
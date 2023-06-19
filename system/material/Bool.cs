namespace Gudron
{
    public class Bool
    {
        private bool Is;

        public Bool() { Is = false; }
        public Bool(bool pIsValue) { Is = pIsValue; }

        public bool Value { get { return Is; } }

        public void True() => Is = true;
        public void False() => Is = false;
    }
}

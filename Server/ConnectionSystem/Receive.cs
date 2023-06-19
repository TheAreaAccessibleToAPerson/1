namespace Gudron
{
    public class ReceiveConnectionToSystem : Controller.LocalField<Receive.Settings>
    {
        protected IInput<RequestDB> InputToDB;

        protected void ConnectionToSystem()
        {
            send_message<RequestDB>(ref InputToDB, "RequestDB");
        }
    }
}
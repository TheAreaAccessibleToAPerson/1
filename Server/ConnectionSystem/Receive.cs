namespace Gudron
{
    public class ReceiveConnectionToSystem : Controller.LocalField<Receive.Settings>
    {
        protected IInput<RequestDB<List<Person>>> InputToDBGetUsers;

        void Construction()
        {
            send_message<RequestDB<List<Person>>>(ref InputToDBGetUsers, "DB");
        }
    }
}
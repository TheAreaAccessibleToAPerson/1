namespace Gudron
{
    public sealed class Receive : ReceiveConnectionToSystem
    {
        WebApplication _webApp;

        void Start() => _webApp.Run();

        void Configurate()
        {
            var builder = WebApplication.CreateBuilder();
            _webApp = builder.Build();

            _webApp.Map("/", new MainPage("Server/html/index.html").Process);

            _webApp.MapGet("/api/users", new API_Get_Users(InputToDBGetUsers, 50, "Person").Process);
        }

        void Stop()
        {
        }

        public class Settings
        {
        }
    }

    public class Person
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    public class API_Get_Users : RequestDB<List<Person>>
    {
        private readonly IInput<RequestDB<List<Person>>> _inputToDB;
        private readonly int _timeDelay;

        public API_Get_Users(IInput<RequestDB<List<Person>>> inputToDB, int timeDelay, string tableName)
            : base(RequestType.Select)
        {
            _inputToDB = inputToDB;
            _timeDelay = timeDelay;
        }

        public async Task Process(HttpContext context)
        {
            _inputToDB.To(this);

            await Task.Delay(_timeDelay);

            if (IsResponse)
            {
                //...
            }
            else
            {
                await context.Response.WriteAsync("");
            }
        }
    }

    public class API_Get_User
    {
    }

    public class API_Delete_User
    {
    }

    public class API_Post_Users
    {
    }

    public class RequestDB<T>
    {
        public enum RequestType
        {
            None = 0,
            Select = 1,
        }

        public readonly RequestType Type;

        public string Response { set; get; } 

        public bool IsResponse { get { return !(Response is null);  } }

        public readonly string Request;

        public RequestDB(RequestType type)
        {
            Type = type;
        }
    }


    public class MainPage
    {
        private static string s_pageHTML;

        public MainPage(string path)
        {
            if (s_pageHTML is null)
                s_pageHTML = Hellper.FileRead(path);
        }

        public async Task Process(HttpContext context)
        {
            await context.Response.WriteAsync(s_pageHTML);
        }
    }
}
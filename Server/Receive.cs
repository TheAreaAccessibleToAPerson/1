namespace Gudron
{
    public sealed class Receive : ReceiveConnectionToSystem
    {
        void Construction() => ConnectionToSystem();

        void Start() => _webApp.Run();

        WebApplication _webApp;

        void Configurate()
        {
            var builder = WebApplication.CreateBuilder();
            _webApp = builder.Build();

            _webApp.Map("/", new MainPage("Server/html/index.html").Process);

            _webApp.MapGet("/api/users", async (context) => 
            {
                await new API_Get_Users(InputToDB, 2050).Process(context);
            });
        }

        void Stop()
        {
        }

        public class Settings
        {
        }
    }

    public class Users
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    public class API_Get_Users : RequestDB
    {
        private readonly IInput<RequestDB> _inputToDB;
        private readonly int _timeDelay;

        public API_Get_Users(IInput<RequestDB> inputToDB, int timeDelay)
            : base("SELECT * FROM Users")
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

    public class RequestDB
    {
        public string Response { set; get; } 

        public bool IsResponse { get { return !(Response is null);  } }

        public readonly string Request;

        public RequestDB(string request)
        {
            Request = request;
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
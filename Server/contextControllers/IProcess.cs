namespace Context.Controllers
{
    public interface IProcess
    {
        public Task Use(HttpContext context);
    }
}
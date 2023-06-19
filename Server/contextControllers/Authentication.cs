using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Context.Controllers
{
    public sealed class Authentication
    {
        public Authentication()
        {
        }

        record class Person(string Email, string Password);

        List<Person> peoples = new List<Person>()
        {
            new Person("tom@gmail.com", "12345"),
            new Person("kdjf@gmail.com", "124444")
        };

        public async Task<IResult> Use(string returnUrl, HttpContext context)
        {
            // Получаем из формы email и password.
            var form = context.Request.Form;

            // если email и/или пароль не установлены, посылаем статусный код ошибки 400
            if (!form.ContainsKey("email") || !form.ContainsKey("password"))
                return Results.BadRequest("Email и/или пароль не установлены.");

            string email = form["email"];
            string password = form["password"];

            // находим пользователя.
            Person person = peoples.FirstOrDefault(p => p.Email == email && p.Password == password);

            // если пользователь не найден отправим статусный код 401.
            if (person is null) return Results.Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };

            // Создадим обьект ClaimsIdentity.
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            // Установка аутентификационые куки.
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return Results.Redirect(returnUrl ?? "/");
        }
    }
}
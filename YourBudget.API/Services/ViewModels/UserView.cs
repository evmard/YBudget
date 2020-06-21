using YourBudget.API.Models;

namespace YourBudget.API.Services.ViewModels
{
    public class UserView
    {
        public UserView()
        {
        }

        public UserView(User user)
        {
            Login = user.Login;
            Name = user.Name;
            Mail = user.Mail;
        }

        public string Login { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
    }
}

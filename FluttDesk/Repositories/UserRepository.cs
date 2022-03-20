using FluttDesk.Models;

namespace FluttDesk.Repositories
{
    public static class UserRepository
    {
        public static Users Get(string username, string password)
        {
            var users = new List<Users>
            {
                new Users { UserId = 1, UserName = "admin", UserPwd="admin", NickName="Admin", UserStatus="A", UserEmail="admin@admin.com.br", UserRole="manager", UserDtCreation= DateTime.Now },
                new Users { UserId = 1, UserName = "employee", UserPwd="employee", NickName="Employee", UserStatus="A", UserEmail="employee@employee.com.br", UserRole="employee", UserDtCreation= DateTime.Now }
            };

            return users.Where(x => x.UserName.ToLower() == username.ToLower() && x.UserPwd == password).FirstOrDefault();
        }
    }
}

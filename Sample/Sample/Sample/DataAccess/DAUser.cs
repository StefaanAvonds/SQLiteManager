using Sample.DataModel;
using SQLiteManager.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.DataAccess
{
    public class DAUser : BaseDataAccess<User>
    {
        public DAUser()
            : base()
        {

        }

        public User SelectByUsernameAndPassword(string username, string password)
        {
            return PerformQuery(() =>
            {
                return AsyncConnection.Table<User>()
                    .Where(x => x.Username.ToLower().Equals(username.ToLower()) && x.Password.Equals(password))
                    .FirstOrDefaultAsync();
            });
        }
    }
}

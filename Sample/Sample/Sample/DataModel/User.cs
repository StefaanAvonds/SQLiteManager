using SQLiteManager.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.DataModel
{
    public class User : BaseDataModel
    {
        private string _username;
        private string _password;

        public String Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public String Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public User()
            : base()
        {
            Username = String.Empty;
            Password = String.Empty;
        }

        public User(string username, string password)
            : this()
        {
            Username = username;
            Password = password;
        }
    }
}

using Sample.DataAccess;
using Sample.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Sample.Views
{
    public partial class LoginPage : ContentPage
    {
        private string _username;
        private string _password;

        private ObservableCollection<User> _listUsers;

        private DAUser _daUser;

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

        public ObservableCollection<User> ListUsers
        {
            get { return _listUsers; }
            set
            {
                _listUsers = value;
                OnPropertyChanged();
            }
        }

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = this;

            _daUser = new DAUser();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ReloadUsers();
        }
        
        /// <summary>
        /// Reload the ListView with all the users from SQLite.
        /// </summary>
        private void ReloadUsers()
        {
            ListUsers = new ObservableCollection<User>(_daUser.SelectAll());
        }

        public void btnLogin_Click(object sender, EventArgs e)
        {
            var user = _daUser.SelectByUsernameAndPassword(Username, Password);
            
            if (user == null)
            {
                DisplayAlert("Error", "User has not been found", "OK");
            }
            else
            {
                DisplayAlert("Success", "User found", "OK");
            }
        }

        public void btnAdd_Click(object sender, EventArgs e)
        {
            var user1 = new User("Admin", "Admin");
            var user2 = new User("User", "User");
            var user3 = new User("Guest", "Guest");

            var listUsers = new List<User>
            {
                user1, user2, user3
            };

            _daUser.InsertBatch(listUsers);

            ReloadUsers();
        }
    }
}

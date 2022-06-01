using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Launcher0._2.Models
{
    public class User : INotifyPropertyChanged
    {
        public int ID { get; set; }
        private string userName { get; set; }
        public string UserName
        {
            get { return userName; }
            set
            {

                userName = value;
                OnPropertyChanged();
            }
        }

        public string Password { get; set; }

        private string email { get; set; }
        public string Email
        {
            get { return email; }
            set
            {

                email = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateOfCreated { get; set; }

        private string userStatus_id { get; set; }
        public string UserStatus_id
        {
            get { return userStatus_id; }
            set
            {

                userStatus_id = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage photo;
        public BitmapImage Photo
        {
            get { return photo; }
            set { 
                    photo = value;
                    OnPropertyChanged();
                }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

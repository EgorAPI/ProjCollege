using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Launcher0._2.Models
{
    public class Apps : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string NameApp { get; set; }
        public string Description { get; set; }

        private BitmapImage photo;
        public BitmapImage Photo
        {
            get { return photo; }
            set
            {
                photo = value;
                OnPropertyChanged();
            }
        }
        public DateTime DateOfCreated { get; set; }
        public string Path { get; set; }
        public int Downloads { get; set; }
        public int Author_id { get; set; }
        public int AppCategory_id { get; set; }
        public string Author { get; set; }
        public string AppCategory { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

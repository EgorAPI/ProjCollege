using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher0._2.Models
{
    public class Apps
    {
        public int ID { get; set; }
        public string NameApp { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public DateTime DateOfCreated { get; set; }
        public string Path { get; set; }
        public User Author_id { get; set; }
        public int AppCategory_id { get; set; }
    }
}

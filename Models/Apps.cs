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
        public string Photo { get; set; } = "https://n1s1.hsmedia.ru/ec/71/3e/ec713e8db6eb948cc0a3aa406f9f53a8/728x510_1_a6964aa55fffee8bc6c3631aee11f20a@2000x1400_0xac120003_8794949311568810898.jpg";
        public DateTime DateOfCreated { get; set; }
        public string Path { get; set; }
        public int Author_id { get; set; }
        public int AppCategory_id { get; set; }
        public string Author { get; set; }
        public string AppCategory { get; set; }
    }
}

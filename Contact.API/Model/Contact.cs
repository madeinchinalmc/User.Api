using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Model
{
    public class Contact
    {
        public Contact()
        {
            Tags = new List<string>();
        }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        /// <summary>
        /// 工作职业
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public List<string> Tags { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Model
{
    public class ContactApplyRequest
    {

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
        /// <summary>
        /// 申请人
        /// </summary>
        public int ApplierId { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public string Approvaled { get; set; }

        public DateTime HandledTime { get; set; }
        public string Phone { get; set; }
        public DateTime ApplyTime { get; set; }
    }
}

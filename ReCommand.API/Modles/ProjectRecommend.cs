using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReCommend.API.Modles
{
    public class ProjectRecommend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserAvatar { get; set; }
        public int ProjectId { get; set; }
        public EnumRecommendType EnumRecommendType { get; set; }
        public string projectAvatar { get; set; }
        public string Company { get; set; }
        //public string 
    }
    public enum EnumRecommendType
    {
        Platfrom =1,
        Friend = 2,
        Foaf =3
    }
}

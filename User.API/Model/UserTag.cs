﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Model
{
    public class UserTag
    {
        public int UserId { get; set; }
        public string Tag { get; set; }

        public DateTime CreateTime { get; set; }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.API.Dtos;

namespace Project.API.Controllers
{
    public class BaseController : Controller
    {
        //protected UserIdentity UserIdentity => new UserIdentity { UserId = 1, Name = "jony" };
        protected UserIdentity UserIdentity
        {
            get
            {
                var identity = new UserIdentity();
                identity.UserId = int.Parse(User.Claims.First(x => x.Type == "sub").Value);
                identity.Avatar = User.Claims.First(x => x.Type == "avatar").Value;
                identity.Company = User.Claims.First(x => x.Type == "company").Value;
                identity.Name = User.Claims.First(x => x.Type == "name").Value;
                identity.Title = User.Claims.First(x => x.Type == "title").Value;
                return identity;
            }
        }
    }
}

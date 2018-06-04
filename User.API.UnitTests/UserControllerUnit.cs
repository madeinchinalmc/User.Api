using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using User.API.Controllers;
using Microsoft.AspNetCore.JsonPatch;
using User.API.Data;
using System.Collections.Generic;
using System.Linq;

namespace User.API.UnitTests
{
    public class UserControllerUnit
    {
        private User.API.Data.UserContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<Data.UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new User.API.Data.UserContext(options);
            context.Users.Add(new Model.AppUser { Id = 1, Name = "lmc" });
            context.SaveChanges();
            return context;
        }
        private (UserController, UserContext) GetUserController()
        {
            var context = GetUserContext();
            var loggerMoq = new Mock<ILogger<API.Controllers.UserController>>();

            //loggerMoq.Setup(t =>t.LogError("自定义错误",new object[] { })); //自定义配置
            var logger = loggerMoq.Object;
            return  (UserController: new UserController(context, logger), UserContext: context) ;
        }
        //方法 返回结果 期待
        [Fact]
        public async Task Get_ReturnRightUser_WithExpectedParameters()
        {
            var controller = GetUserController();
            var response = await controller.Item1 .Get();

            //Assert.IsType<JsonResult>(response.GetType());
            var result = response.Should().BeOfType<JsonResult>().Subject;
            var appUser = result.Value.Should().BeAssignableTo<Model.AppUser>().Subject;
            appUser.Id.Should().Be(1);
            appUser.Name.Should().Be("lmc");
        }
        [Fact]
        public async Task Path_ReturnNewName_WithExperctedNewNameParameter()
        {
            var controller = GetUserController();
            var document = new JsonPatchDocument<Model.AppUser>();
            document.Add(user => user.Name, "liu");
            document.Replace(u => u.Name, "liu");
            var response =await controller.Item1.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;

            //assert reponse
            var appUser = result.Value.Should().BeAssignableTo<Model.AppUser>().Subject;
            appUser.Name.Should().Be("liu");

            //assert name value in ef context
            var userModel =await controller.Item2.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Should().NotBeNull();
            userModel.Name.Should().Be("liu");
        }

        [Fact]
        public async Task Path_ReturnNewProperties_WithAddNewProperty()
        {
            var controller = GetUserController();
            var document = new JsonPatchDocument<Model.AppUser>();
            document.Replace(u => u.userProperties,new List<Model.UserProperty> {
                new Model.UserProperty{Key="fin_industry",Value="互联网",Text="互联网"}
            });
            var response = await controller.Item1.Patch(document);
            var result = response.Should().BeOfType<JsonResult>().Subject;

            //assert reponse
            var appUser = result.Value.Should().BeAssignableTo<Model.AppUser>().Subject;
            appUser.userProperties.Count.Should().Be(1);
            appUser.userProperties.First().Value.Should().Be("互联网");
            appUser.userProperties.First().Key.Should().Be("fin_industry");

            //assert name value in ef context
            var userModel = await controller.Item2.Users.SingleOrDefaultAsync(u => u.Id == 1);
            userModel.Should().NotBeNull();

            userModel.userProperties.First().Value.Should().Be("互联网");
            userModel.userProperties.First().Key.Should().Be("fin_industry");
        }
        [Fact]
        public async Task Path_ReturnRemoveProperties_WithAddRemoveProperty()
        {
            var controller = GetUserController();
            var document = new JsonPatchDocument<Model.AppUser>();
            document.Replace(u => u.userProperties, new List<Model.UserProperty> {
            });
            var response = await controller.Item1.Patch(document); 
            var result = response.Should().BeOfType<JsonResult>().Subject;

            //assert reponse
            var appUser = result.Value.Should().BeAssignableTo<Model.AppUser>().Subject;
            appUser.userProperties.Count.Should().Be(0);
            appUser.userProperties.Should().BeEmpty();

            //assert name value in ef context
            var userModel = await controller.Item2.Users.SingleOrDefaultAsync(u => u.Id == 1); 
            userModel.userProperties.Should().BeEmpty();
        }
    }
}

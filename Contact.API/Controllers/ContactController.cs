using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Contact.API.Model;
using Contact.API.Data;
using Contact.API.Service;
using System.Threading;
using Contact.API.ViewModel;

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : BaseController
    {
        private IContactApplyRequestRepository _contactApplyRequestRepository;
        private IContactRepository _contactRepository;
        private IUserService _userService;
        public ContactController(IContactApplyRequestRepository contactApplyRequestRepository, IUserService userService, IContactRepository contactRepository)
        {
            _contactApplyRequestRepository = contactApplyRequestRepository;
            _userService = userService;
            _contactRepository = contactRepository;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return Ok(await _contactRepository.GetContactsAsync(UserIdentity.UserId, cancellationToken));
        }
        [HttpPut]
        [Route("tag")]
        public async Task<IActionResult> TagContact([FromBody]TagContactInputViewModel tagContactInputViewModel, CancellationToken cancellationToken)
        {
            var result = await _contactRepository.TagContactAsync(UserIdentity.UserId, tagContactInputViewModel.ContactId, tagContactInputViewModel.Tags, cancellationToken);
            if (result)
                return Ok();
            else
                return BadRequest();

        }
        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("apply-requests")]
        public async Task<IActionResult> GetApplyRequests(CancellationToken cancellationToken)
        {
            var result = await _contactApplyRequestRepository.GetRequestListAsync(UserIdentity.UserId, cancellationToken);
            return Ok(result);
        }
        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("apply-requests")]
        public async Task<IActionResult> ApplyRequest(int userId, CancellationToken cancellationToken)
        {
            var baseuserInfo = await _userService.GetBaseUserInfoAsync(userId);
            if (baseuserInfo == null)
                throw new Exception("用户参数错误");
            var result = await _contactApplyRequestRepository.AddReqeustAsync(new ContactApplyRequest
            {
                UserId = userId,
                ApplierId = UserIdentity.UserId,
                Name = baseuserInfo.Name,
                Company = baseuserInfo.Company,
                Title = baseuserInfo.Title,
                HandledTime = DateTime.Now,
                Avatar = baseuserInfo.Avatar
            }, cancellationToken);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        /// <summary>
        /// 通过好友请求
        /// </summary>
        /// <param name="appli"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("apply-requests")]
        public async Task<IActionResult> ApplyUpdate(int applierId, CancellationToken cancellationToken)
        {
            var result = await _contactApplyRequestRepository.ApprovalAsync(UserIdentity.UserId, applierId, cancellationToken);

            if (!result)
            {
                return BadRequest();
            }
            var applier = await _userService.GetBaseUserInfoAsync(applierId);
            var userinfo = await _userService.GetBaseUserInfoAsync(UserIdentity.UserId);
            await _contactRepository.AddContactAsync(UserIdentity.UserId, applier, cancellationToken);
            await _contactRepository.AddContactAsync(applierId, applier, cancellationToken);
            return Ok();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Model;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class MongoContactApplyRepository : IContactApplyRequestRepository
    {
        private readonly ContactContext _contactContext;
        public MongoContactApplyRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }
        /// <summary>
        /// 添加好友申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> AddReqeustAsync(ContactApplyRequest request, CancellationToken cancellationToken)
        {
            var filter = Builders<ContactApplyRequest>.Filter.Where(c => c.UserId == request.UserId && c.ApplierId == request.ApplierId);
            if ((await _contactContext.ContactApplyRequests.CountAsync(filter, null, cancellationToken)) > 0)
            {
                var update = Builders<ContactApplyRequest>.Update.Set(r => r.ApplyTime, DateTime.Now);
                //var options = new UpdateOptions { IsUpsert = true }  //没有就插入
                var result = await _contactContext.ContactApplyRequests.UpdateOneAsync(filter, update, null, cancellationToken);
                return result.MatchedCount == result.ModifiedCount && result.MatchedCount > 0;
            }
            await _contactContext.ContactApplyRequests.InsertOneAsync(request, null, cancellationToken);
            return true;
        }

        public async Task<bool> ApprovalAsync(int userId,int applierId,CancellationToken cancellationToken)
        {
            var filter = Builders<ContactApplyRequest>.Filter.Where(c => c.UserId == userId && c.ApplierId == applierId);
            var update = Builders<ContactApplyRequest>.Update.Set(r => r.ApplyTime, DateTime.Now)
                .Set(r=>r.ApplierId,1)
                .Set(r=>r.HandledTime,DateTime.Now);

            var options = new UpdateOptions { IsUpsert = true };
            var result = await _contactContext.ContactApplyRequests.UpdateOneAsync(filter, update, null, cancellationToken);
            return result.MatchedCount == result.ModifiedCount && result.MatchedCount ==1;
        }

        public async Task<List<ContactApplyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken)
        {
            return (await _contactContext.ContactApplyRequests.FindAsync(r => r.UserId == userId)).ToList(cancellationToken);
        }
    }
}

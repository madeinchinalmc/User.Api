﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Contact.API.Model;
using MongoDB.Driver;

namespace Contact.API.Data
{
    public class MongoContactRepository : IContactRepository
    {
        private readonly ContactContext _contactContext;
        public MongoContactRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }
        public async Task<bool> UpdateContactInfoAsync(BaseUserInfo userInfo,CancellationToken cancellationToken)
        {
            var contactBook =(await _contactContext.ContactBooks.FindAsync(c => c.UserId == userInfo.UserId, null, cancellationToken)).FirstOrDefault();
            if (contactBook == null)
                return true;
            var contactIds = contactBook.Contacts.Select(c => c.UserId);
            var filterDefinition = Builders<ContactBook>.Filter.And(
                Builders<ContactBook>.Filter.In(c => c.UserId, contactIds),
                Builders<ContactBook>.Filter.ElemMatch(c => c.Contacts, contact => contact.UserId == userInfo.UserId)
                );
            var update = Builders<ContactBook>.Update
                .Set("Contacts.$.Name", userInfo.Name)
                .Set("Contacts.$.Avatar", userInfo.Avatar)
                .Set("Contacts.$.Company", userInfo.Company)
                .Set("Contacts.$.Title", userInfo.Title);
            var updateResult =await _contactContext.ContactBooks.UpdateManyAsync(filterDefinition, update);
            return (updateResult.MatchedCount == updateResult.ModifiedCount);
        }
        public async Task<bool> AddContactAsync(int userId, BaseUserInfo contact, CancellationToken cancellationToken)
        {
            if(_contactContext.ContactBooks.Count(c=>c.UserId == userId)==0)
            {
                await _contactContext.ContactBooks.InsertOneAsync(new ContactBook { UserId = userId });
            }
            var filter = Builders<ContactBook>.Filter.Eq(c => c.UserId, userId);
            var update = Builders<ContactBook>.Update.AddToSet(c => c.Contacts, new Model.Contact {
                UserId = contact.UserId,
                Avatar = contact.Avatar,
                Company = contact.Company,
                Name = contact.Name,
                Title = contact.Title,
            });
            var updateResult = await _contactContext.ContactBooks.UpdateOneAsync(filter, update,null, cancellationToken);
            return (updateResult.MatchedCount == updateResult.ModifiedCount);
        }
        /// <summary>
        /// 拿到用户的Contact
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Model.Contact>> GetContactsAsync(int userId, CancellationToken cancellationToken)
        {
            var contactBook = (await _contactContext.ContactBooks.FindAsync(c => c.UserId == userId)).FirstOrDefault();
            if (contactBook != null)
                return contactBook.Contacts;
            return new List<Model.Contact>();
        }
        /// <summary>
        /// 用户打标签
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contactId"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task<bool> TagContactAsync(int userId,int contactId,List<string> tags,CancellationToken cancellationToken)
        {
            var filter = Builders<ContactBook>.Filter.And(
                Builders<ContactBook>.Filter.Eq(c=>c.UserId ,userId),
                Builders<ContactBook>.Filter.Eq("Contacts.UserId",contactId)
                );
            var update = Builders<ContactBook>.Update
                .Set("Contacts.$.Tags", tags);
            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update, null, cancellationToken);
            return result.MatchedCount == result.ModifiedCount && result.MatchedCount == 1;
        }
    }
}

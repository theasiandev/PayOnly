﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayOnlyWebApp.Models;
using PayOnlyWebApp.DAL;

namespace PayOnlyWebApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private ApiDAL ApiContext = new ApiDAL();

        [Route("GetMerchant/{MerchantId}")]
        [HttpGet]
        public Merchant GetMerchant(int MerchantId)
        {
            Merchant merchant = ApiContext.GetMerchant(MerchantId);
            return merchant;
        }

        [Route("GetUserById/{UserId}")]
        [HttpGet]
        public List<User> GetUserById(int UserId)
        {
            User user = ApiContext.GetUserById(UserId);
            List<User> userlist = new List<User>();
            userlist.Add(user);
            return userlist;
        }

        [Route("GetUserByEmail/{email}")]
        [HttpGet]
        public User GetUserByEmail(string email)
        {
            User user = ApiContext.GetUserByEmail(email);
            return user;
        }

        [Route("PostTransaction")]
        [HttpPost]
        public object PostTransaction([FromBody] TransactionPost transaction)
        {
            bool otherstatus = ApiContext.DeductUser(transaction.UserID, transaction.TransactionAmount);
            if (otherstatus == true)
            {
                bool status = ApiContext.PostTxn(transaction);
                bool otherotherstatus = ApiContext.CreditMerchant(transaction.MerchantID, transaction.TransactionAmount);
                return Ok();
            }

            else
            {
                return Unauthorized();
            }
        }

        [Route("TopUp")]
        [HttpPost]
        public object TopUpUser([FromBody] TopUpModel topup)
        {
            bool status = ApiContext.TopUpUser(topup);
            return Ok();
        }
        // PUT: api/API/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
﻿using LicenseLibrary;
using LicenseLibrary.ModelLibrary.EntityFrameworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace License.Web.Controllers
{
    public class UserAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveUser([FromBody]User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //if (Mail.networkIsAvailable())
                    //{
                    string errMsg = string.Empty;
                    user.CreatedOn = System.DateTime.Now;
                    user.FirstTime = true;
                    string password = System.Web.Security.Membership.GeneratePassword(8, 0);
                    user.HashedPassword = PasswordHash.MD5Hash(password);

                    bool result = UserPL.Save(user, out errMsg);
                    if (string.IsNullOrEmpty(errMsg))
                    {
                        if (result)
                        {
                            user.HashedPassword = password;
                            Mail.SendNewUserMail(user);
                            return Request.CreateResponse(HttpStatusCode.OK, "User added successfully.");
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
                        }
                    }
                    else
                    {
                        var response = Request.CreateResponse(HttpStatusCode.BadRequest, errMsg);
                        return response;
                    }
                    //}
                    //else
                    //{
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, "Kindly ensure that internet connection is available before creating a user");
                    //}
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateUser([FromBody]User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                bool result = UserPL.Update(user);
                return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "User Updated Successfully.") : Request.CreateResponse(HttpStatusCode.BadRequest, "Failed");
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpPut]
        public HttpResponseMessage ChangePassword([FromBody]PasswordModel changePassword)
        {
            try
            {
                string password = PasswordHash.MD5Hash(changePassword.Password);
                string username = changePassword.Username;
                bool result = UserPL.ChangePassword(username, password);
                return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Successful") : Request.CreateResponse(HttpStatusCode.BadRequest, "Failed");
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpPut]
        public HttpResponseMessage ForgotPassword([FromBody]PasswordModel changePassword)
        {
            try
            {
                //if (Mail.networkIsAvailable())
                //{
                    string username = changePassword.Username;
                    User user = UserPL.RetrieveUserByUsername(username);
                    if (user != null)
                    {

                        Mail.SendForgotPasswordMail(user);
                        return Request.CreateResponse(HttpStatusCode.OK, user.Email);
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid username");
                //}
                //else
                //{
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, "Kindly ensure that internet connection is available in order to reset your password.");
                //}
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpPost]
        public HttpResponseMessage ConfirmUsername([FromBody]PasswordModel changePassword)
        {
            try
            {
                string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");
                string username = Crypter.Decrypt(key, changePassword.Username);
                User user = UserPL.RetrieveUserByUsername(username);
                if (user != null)
                {
                    //Add a mail for password reset
                    object response = new
                    {
                        Status = "Successful",
                        Username = user.Username
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid username");
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveUsers()
        {
            try
            {
                IEnumerable<Object> users = UserPL.RetrieveUsers();
                object returnedUsers = new { data = users };
                return Request.CreateResponse(HttpStatusCode.OK, returnedUsers);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveUsersWithoutSmartCard()
        {
            try
            {
                IEnumerable<Object> users = UserPL.RetrieveUsersWithoutSmartCard();
                object returnedUsers = new { data = users };
                return Request.CreateResponse(HttpStatusCode.OK, returnedUsers);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.ReasonPhrase = ex.Message;
                return response;
            }
        }

        [HttpPost]
        public HttpResponseMessage AuthenticateUser([FromBody]PasswordModel passwordModel)
        {
            try
            {
                string password = PasswordHash.MD5Hash(passwordModel.Password);
                var userObj = UserPL.AuthenticateUser(passwordModel.Username, password);
                if (userObj == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Username/Password");
                }
                else if (userObj.ID == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Username/Password");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, userObj);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }
    }
}

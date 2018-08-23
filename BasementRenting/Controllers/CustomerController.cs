using DataAccess.Interface;
using EntityModel.DomainModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace BasementRenting.Controllers
{
    public partial class CustomerController : Controller
    {
        public CommonController _objCommonController;
        private IRegisterRepository _registerRepository;
        private ICommonRepository _commonRepository;
        private ISettingRepository _settingRepository;

        public CustomerController(IRegisterRepository registerRepository,
            CommonController objCommonController, ICommonRepository CommonRepository, ISettingRepository settingRepository)
        {
            this._registerRepository = registerRepository;
            this._objCommonController = objCommonController;
            this._commonRepository = CommonRepository;
            this._settingRepository = settingRepository;
        }

        #region login & register
        [ActionName("Login")]
        public ActionResult Login()
        {
            List<UserRole> lstUserRoles = _registerRepository.GetUserRole();
            var admin = lstUserRoles.Find(x => x.RoleName == "Admin");
            lstUserRoles.Remove(admin);

            if (TempData["LoginStatusMessage"] != null)
            {
                ViewBag.LoginStatusMessage = TempData.Peek("LoginStatusMessage");
            }

            if (TempData["RegisterStatusMessage"] != null)
            {
                ViewBag.RegisterStatusMessage = TempData.Peek("RegisterStatusMessage");
            }

            return View("~/views/common/login.cshtml", lstUserRoles);
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult Login(FormCollection fcol)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(fcol["txtEmail"])) && !string.IsNullOrEmpty(Convert.ToString(fcol["txtPassword"])))
            {
                RegisterCustomer objRegisterCustomer = new RegisterCustomer();

                objRegisterCustomer.EmailId = _objCommonController.EncryptString(fcol["txtEmail"]);
                objRegisterCustomer.Password = fcol["txtPassword"];

                var record = _registerRepository.GetLoginDetails(objRegisterCustomer);

                if (record != null)
                {
                    if(record.Published == true)
                    {
                        Session["userid"] = record.Id;
                        var userid = _commonRepository.getUserRolebyId(record.Id);
                        _registerRepository.updatelastLoginDetail(record.Id);

                        var userrole = _commonRepository.getUserRolebyId(record.Id);
                        if (userrole != null)
                        {
                            Session["userrole"] = userrole.RoleName;
                            Session["username"] = record.UserName;
                        }

                        if(userrole.RoleName == "Admin")
                        {
                            return RedirectToAction("profile", "customer");
                        }
                        else if(userrole.RoleName == "Landlord")
                        {
                            return RedirectToAction("my-properties", "landlord");
                        }
                        else
                        {
                            return RedirectToAction("index", "home");
                        }
                    }
                    else
                    {
                        TempData["LoginStatusMessage"] = "Please verify your email first to login!";

                        return RedirectToAction("login", "customer");
                    }
                }
                else
                {
                    TempData["LoginStatusMessage"] = "Invalid Credentials..!";

                    return RedirectToAction("login", "customer");
                }
            }
            else
            {
                return RedirectToAction("login", "customer");
            }
        }

        [ActionName("login-again")]
        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("login", "customer");
        }

        [HttpGet]
        public ActionResult PasswordRecovery()
        {
            return View("~/Views/Common/PasswordRecovery.cshtml");
        }

        [HttpPost]
        public ActionResult PasswordRecovery(string emailId)
        {
            try
            {
                //var EnctMail = _objCommonController.EncryptString(emailId);
                var newpassword = _objCommonController.RandomStringGenerator();
                var Subject = "Password Recovery";
                var Body = "<html><body style=\"background:#F6F6F6; font-family:Verdana, Arial, Helvetica, sans-serif; font-size:12px; margin:0; padding:0;\"><div style=\"background:#F6F6F6; font-family:Verdana, Arial, Helvetica, sans-serif; font-size:12px; margin:0; padding:0;\"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"100%\" width=\"100%\"> <tbody> <tr> <td style=\"padding:20px 0 20px 0\" align=\"center\" valign=\"top\"> <table style=\"border:1px solid #E0E0E0;\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" width=\"650\"> <tbody> <tr> <td align=\"center\" valign=\"top\"><a href=\"http://basementrenting.nyusoft.in/\"><img src=\"http://basementrenting.nyusoft.in/content/images/logo-transparent.png\" alt=\"logo\" style=\"margin-bottom:5px;\" border=\"0\"/></a></td></tr><tr> <td style=\"text-align: center;\"><span style=\"font-size: 26px; color:#088ACA;\">Password Recovery</span></td></tr><tr> <td style=\"font-size: 14px; color: rgb(90, 90, 90);\"><p>Welcome to the Basement Renting website!</p><p>Your Password has been reset. Please use below password as you new one:<br/><strong>" + newpassword + "</strong></p></td></tr><tr> <td style=\"background:#EAEAEA; text-align:center;\" bgcolor=\"#EAEAEA\" align=\"center\"><center> <p style=\"font-size:12px; margin:0;\">Thank you, <strong>Basement Renting</strong></p></center></td></tr></tbody> </table></td></tr></tbody> </table></div></body></html>";

                RegisterCustomer record = _registerRepository.GetUserDetails(_objCommonController.EncryptString(emailId));

                record.Password = newpassword;

                var UserId = _registerRepository.AddUpdateRegisterCustomer(record);

                bool status = _objCommonController.SendMail(emailId, Subject, Body);

                return Json(status, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("Register")]
        public ActionResult Register()
        {
            //return View("Login");
            return RedirectToAction("Login", "Customer");
        }

        [HttpPost]
        [ActionName("Register")]
        public ActionResult Register(FormCollection fcol)
        {
            if (fcol["txtEmailReg"] != null)
            {
                var UserDetails = _registerRepository.GetUserDetails(_objCommonController.EncryptString(fcol["txtEmailReg"] != null ? fcol["txtEmailReg"].ToLower().Trim() : fcol["txtEmailReg"]));
                if (UserDetails == null)
                {
                    RegisterCustomer objRegisterCustomer = new RegisterCustomer();
                    objRegisterCustomer.EmailId = _objCommonController.EncryptString(fcol["txtEmailReg"] != null ? fcol["txtEmailReg"].ToLower().Trim() : fcol["txtEmailReg"]);
                    objRegisterCustomer.PhoneNo = _objCommonController.EncryptString(fcol["txtPhone"]);

                    if (fcol["rdoRegisterAs"] != "")
                    {
                        objRegisterCustomer.UserRoleId = Convert.ToInt32(fcol["rdoRegisterAs"]);
                    }

                    objRegisterCustomer.UserName = fcol["txtName"] != null ? fcol["txtName"].ToLower() : null;
                    objRegisterCustomer.Password = fcol["txtPasswordReg"];

                    if (fcol["chkNewsLetter"] == "on")
                    {
                        objRegisterCustomer.AgreeToReceiverEmailCommunication = true;
                    }
                    else
                    {
                        objRegisterCustomer.AgreeToReceiverEmailCommunication = false;
                    }

                    objRegisterCustomer.IpAddress = _objCommonController.GetIPAddress();
                    objRegisterCustomer.Browser = _objCommonController.GetBrowserDetails();

                    var userData = _registerRepository.AddUpdateRegisterCustomer(objRegisterCustomer);
                    //Session["userid"] = userData;

                    //var userrole = _commonRepository.getUserRolebyId(userData);
                    //if (userrole != null)
                    //{
                    //    Session["userrole"] = userrole.RoleName;
                    //    Session["username"] = objRegisterCustomer.UserName.ToString();
                    //}

                    CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                    TextInfo textInfo = cultureInfo.TextInfo;

                    #region mail to the user
                    string UserEmailId = Convert.ToString(fcol["txtEmailReg"]).Trim().ToLower();
                    string ActivationLink = "<a href=\"http://" + Request.Url.Authority + "/customer/activate-account?useremail=" + UserEmailId + "\">";
                    //string strEmailBody = "<div style=\"text-align:center;font-family:Verdana,Arial,Helvetica,sans-serif;\"><table style=\"width:100%;max-width:650px;margin:0 auto;border:none;border-collapse:collapse;border:1px solid #e0e0e0;\" cellpadding=\"0\"> <tr> <td style=\"padding:22px;text-align:center;border-bottom:1px solid #e0e0e0;\"> <img src=\"http://basementrenting.nyusoft.in/Content/Images/logo.png\"/></td></tr><tr><td style=\"padding:9px;text-align:center;\"><div style=\"font-size:26px;color:#088aca\">Registration Successful</div></td></tr><tr><td style=\"padding:22px;text-align:left;font-size:14px;color:rgb(90,90,90);\"><p style=\"margin-bottom:5px;\"> Hello <strong> " + textInfo.ToTitleCase(objRegisterCustomer.UserName) + "</strong>,</p><p style=\"margin-bottom: 5px;\">Thank you for Registration on Basement Renting. Happy Renting...</p></td></tr><tr><td style=\"background:#eaeaea;text-align:center;padding: 8px;\"bgcolor=\"#EAEAEA\" align=\"center\"><p style=\"margin:0\">Thank you, <strong>Basement Renting</strong></p></td></tr></table></div>";
                    string strEmailBody = "<div style='text-align:center;font-family:Verdana,Arial,Helvetica,sans-serif;'> <table style='width:100%;max-width:650px;margin:0 auto;border:none;border-collapse:collapse;border:1px solid #e0e0e0;' cellpadding='0'> <tr> <td style='padding:22px;text-align:center;border-bottom:1px solid #e0e0e0;'> <img src='http://basementrenting.nyusoft.in/Content/Images/logo.png'/></td></tr><tr> <td style='padding:9px;text-align:center;'> <div style='font-size:26px;color:#088aca'>Registration Successful</div></td></tr><tr> <td style='padding:22px;text-align:left;font-size:14px;color:rgb(90,90,90);'> <p style='margin-bottom:5px;'> Hello <strong> " + textInfo.ToTitleCase(objRegisterCustomer.UserName) + "</strong>,</p><p style='margin-bottom: 5px;'>Thank you for Registration on Basement Renting. " + ActivationLink + "Click here to Confirm and Activate your account.</a></p><p>Happy Renting...</p></td></tr><tr> <td style='background:#eaeaea;text-align:center;padding: 8px;'bgcolor='#EAEAEA' align='center'> <p style='margin:0'>Thank you, <strong>Basement Renting</strong></p></td></tr></table></div>";

                    bool mailstatus = _objCommonController.SendMail(UserEmailId, "Registration Successful", strEmailBody);
                    #endregion

                    #region mail to the admin
                    strEmailBody = "<div style=\"text-align:center;font-family:Verdana,Arial,Helvetica,sans-serif;\"><table style=\"width: 100%; max-width:650px; margin: 0 auto; border: none; border-collapse:collapse; border: 1px solid #e0e0e0;\"cellpadding=\"0\"><tr><td style=\"padding: 22px;text-align:center;border-bottom:1px solid #e0e0e0;\"><img src=\"http://basementrenting.nyusoft.in/Content/Images/logo.png\"/></td></tr><tr><td style=\"padding:9px;text-align:center;\"><div style=\"font-size:26px;color:#088aca\">Registration Successful</div></td></tr><tr><td style=\"padding:22px;text-align:left;font-size:14px;color:rgb(90,90,90);\"><p style=\"margin-bottom: 5px;\"> Hello <strong> Admin</strong>,</p><p style=\"margin-bottom: 5px;\"> New User is Registered, Check details below: <br/><strong>Name: </strong> " + textInfo.ToTitleCase(objRegisterCustomer.UserName) + "<br/><strong>Email: </strong>" + Convert.ToString(fcol["txtEmailReg"]).ToLower().Trim() + "<br/><strong>Phone: </strong>" + Convert.ToString(fcol["txtPhone"]).Trim() + " </p></td></tr><tr> <td style=\"background:#eaeaea;text-align:center;padding: 8px;\" bgcolor=\"#EAEAEA\" align=\"center\"> <p style=\"margin:0\">Thank you, <strong>Basement Renting</strong></p></td></tr></table></div>";

                    mailstatus = _objCommonController.SendMail(_settingRepository.GetValueBySettingName("AdminEmailId"), "Registration Successful - " + objRegisterCustomer.UserName, strEmailBody);
                    #endregion
                    //bool mailstatus = true;

                    //TempData["RegisterStatusMessage"] = "SuccessfullyRegistered";

                    //return RedirectToAction("index", "home");
                    return Json(new { RegisterStatus = true, RegisterStatusMessage = "", MailId = UserEmailId, MailStatus = mailstatus });
                }
                else
                {
                    TempData["RegisterStatusMessage"] = "The email address you have entered is already registered..!";

                    //return RedirectToAction("register", "customer");
                    return Json(new { RegisterStatus = false, RegisterStatusMessage = "The email address you have entered is already registered..!" });
                }
            }
            else
            {
                TempData["RegisterStatusMessage"] = "The email address you have entered is already registered..!";

                //return RedirectToAction("register", "customer");
                return Json(new { RegisterStatus = false, RegisterStatusMessage = "The email address you have entered is already registered..!" });
            }
        }

        //[HttpPost]
        [ActionName("activate-account")]
        public ActionResult ActivateUserAccount(string UserEmail)
        {
            _registerRepository.ActivateAccountByUserEmail(_objCommonController.EncryptString(UserEmail));

            //return true;
            return RedirectToAction("login", "customer");
        }
        #endregion

        #region Profile
        [ActionName("profile")]
        public ActionResult MyProfile()
        {
            var CurrentUserId = Convert.ToInt32(Session["Userid"]);
            if (CurrentUserId != 0)
            {
                var UserProfileData = _registerRepository.GetUserProfile(CurrentUserId);
                UserProfileData.EmailId = _objCommonController.DecryptString(UserProfileData.EmailId);
                UserProfileData.PhoneNo = _objCommonController.DecryptString(UserProfileData.PhoneNo);

                if (TempData["UpdateProfileStatus"] != null)
                {
                    ViewBag.UpdateProfileStatus = TempData["UpdateProfileStatus"].ToString();
                }

                if (Session["userrole"].ToString() == "Landlord")
                {
                    return View("~/Views/Landlord/MyProfile.cshtml", UserProfileData);
                }
                else
                {
                    return View("~/Views/Common/MyProfile.cshtml", UserProfileData);
                }
            }
            else
            {
                return RedirectToAction("Login", "Customer");
            }
        }

        [HttpPost]
        [ActionName("update-profile")]
        public ActionResult UpdateProfile(UserProfile objUserProfile)
        {
            if (Session["userid"] != null)
            {
                _registerRepository.UpdateUserProfile(objUserProfile.UserName, _objCommonController.EncryptString(objUserProfile.EmailId), _objCommonController.EncryptString(objUserProfile.PhoneNo), Convert.ToInt32(Session["userid"]));

                TempData["UpdateProfileStatus"] = "Profile has been updated successfully";

                return RedirectToAction("profile", "customer");
            }
            else
            {
                return RedirectToAction("login", "customer");
            }
        }

        [HttpPost]
        [ActionName("update-password")]
        public bool UpdateUserPassword(string NewPassword)
        {
            if (Session["userid"] != null)
            {
                _registerRepository.UpdateUserPassword(NewPassword, Convert.ToInt32(Session["userid"]));

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Contact us
        [ActionName("contact-us")]
        public ActionResult ContactUs()
        {
            if (TempData["SentMailStatus"] != null)
            {
                ViewBag.SentMailStatus = TempData["SentMailStatus"].ToString();
            }

            if(TempData["ContactUsModel"] != null)
            {
                var objContactUsModel = (ContactUs)TempData["ContactUsModel"];

                return View("~/views/customer/contactus.cshtml", objContactUsModel);
            }
            else
            {
                return View("~/views/customer/contactus.cshtml");
            }

        }

        #region submit-contact-us: Do not delete it (with Google verification)
        //[ActionName("submit-contact-us")]
        //public ActionResult ContactUs(FormCollection fcol, ContactUs objContactUs)
        //{
        //    if (!string.IsNullOrWhiteSpace(fcol["g-recaptcha-response"]))
        //    {
        //        //Local & nyusoft: 6LeZc04UAAAAAIXtQWMvCoqWgw1HG4-Ljenb1I4L
        //        //Live: 6LddqWcUAAAAAA_F61bYV3diA_fSEakbSkzmFNBf
        //        ReCaptchaResponse reCaptchaResponse = VerifyCaptcha("6LeZc04UAAAAAIXtQWMvCoqWgw1HG4-Ljenb1I4L", fcol["g-recaptcha-response"]);
        //        if (reCaptchaResponse.success)
        //        {
        //            string NameOfUser = fcol["Name"].ToString(), EmailId = fcol["EmailId"].ToString(), PhoneNumber = fcol["PhoneNumber"].ToString(), MessageContent = fcol["MessageContent"].ToString();
        //            string EmailBody = "";
        //            bool AdminMailStatus, UserMailStatus;

        //            #region Send mail to admin
        //            EmailBody = "<div style=\"text-align:center;font-family: Verdana,Arial,Helvetica,sans-serif;\"> <table style=\"width:100%;max-width:650px;margin:0 auto;border:none;border-collapse:collapse;border:1px solid #e0e0e0;\" cellpadding=\"0\"> <tr> <td style=\"padding:22px;text-align:center;border-bottom:1px solid #e0e0e0;\"> <img src=\"http://basementrenting.nyusoft.in/Content/Images/logo.png\"/> </td></tr><tr> <td style=\"padding:22px;text-align:left;font-size:14px;color:rgb(90,90,90);\"> <p style=\"margin-bottom:5px;\">Hello <strong>Admin</strong>,</p><p style=\"margin-bottom:5px;\">Below person has contacted us:</p><br/> <p style=\"margin-bottom:5px;\"><b>User Name: </b>" + NameOfUser + "</p><p style=\"margin-bottom:5px;\"><b>Email: </b>" + EmailId + "</p><p style=\"margin-bottom:5px;\"><b>Phone: </b>" + PhoneNumber + "</p><p style=\"margin-bottom:5px;\"><b>Message: </b>" + MessageContent + "</p></td></tr><tr> <td style=\"background:#eaeaea;text-align:center;padding:8px;\" bgcolor=\"#EAEAEA\" align=\"center\"><p style=\"margin:0\">Thank you, <strong>Basement Renting</strong></p></td></tr></table></div>";
        //            AdminMailStatus = _objCommonController.SendMail(_settingRepository.GetValueBySettingName("AdminEmailId"), "Contact Us", EmailBody);
        //            #endregion Send mail to admin

        //            #region Send mail to user
        //            EmailBody = "<div style=\"text-align:center;font-family: Verdana,Arial,Helvetica,sans-serif;\"> <table style=\"width:100%;max-width:650px;margin:0 auto;border:none;border-collapse:collapse;border:1px solid #e0e0e0;\" cellpadding=\"0\"> <tr> <td style=\"padding:22px;text-align:center;border-bottom:1px solid #e0e0e0;\"> <img src=\"http://basementrenting.nyusoft.in/Content/Images/logo.png\"/> </td></tr><tr> <td style=\"padding:22px;text-align:left;font-size:14px;color:rgb(90,90,90);\"> <p style=\"margin-bottom:5px;\">Hello <strong>" + NameOfUser + "</strong>,</p><p style=\"margin-bottom:5px;\"><b>Greetings from the Basement Renting!</b></p><br/> <p style=\"margin-bottom:5px;\">Thank you for contacting us, we'll get back to you soon for your query.</p></td></tr><tr> <td style=\"background:#eaeaea;text-align:center;padding:8px;\" bgcolor=\"#EAEAEA\" align=\"center\"><p style=\"margin:0\">Thank you, <strong>Basement Renting</strong></p></td></tr></table></div>";
        //            UserMailStatus = _objCommonController.SendMail(EmailId, "Thank you for Contacting us", EmailBody);
        //            #endregion Send mail to user

        //            if (AdminMailStatus == true && UserMailStatus == true)
        //            {
        //                TempData["SentMailStatus"] = "<div class='alert alert-success text-center'>Email has been sent successfully..!</div>";

        //                return RedirectToAction("contact-us", "customer");
        //            }
        //            else
        //            {
        //                TempData["SentMailStatus"] = "<div class='alert alert-danger text-center'>Something went wrong, please try again after sometime!</div>";

        //                return RedirectToAction("contact-us", "customer");
        //            }
        //        }
        //        else
        //        {
        //            TempData["SentMailStatus"] = "<div class='alert alert-danger text-center'>Google verification failed..!</div>";

        //            TempData["ContactUsModel"] = objContactUs;
        //            return RedirectToAction("contact-us", "customer");
        //        }
        //    }
        //    else
        //    {
        //        TempData["SentMailStatus"] = "<div class='alert alert-danger text-center'>Google verification failed..!</div>";
        //        TempData["ContactUsModel"] = objContactUs;

        //        return RedirectToAction("contact-us", "customer");
        //    }
        //}
        #endregion

        [ActionName("submit-contact-us")]
        public ActionResult ContactUs(FormCollection fcol, ContactUs objContactUs)
        {
            string NameOfUser = fcol["Name"].ToString(), EmailId = fcol["EmailId"].ToString(), PhoneNumber = fcol["PhoneNumber"].ToString(), MessageContent = fcol["MessageContent"].ToString();
            string EmailBody = "";
            bool AdminMailStatus, UserMailStatus;

            #region Send mail to admin
            EmailBody = "<div style=\"text-align:center;font-family: Verdana,Arial,Helvetica,sans-serif;\"> <table style=\"width:100%;max-width:650px;margin:0 auto;border:none;border-collapse:collapse;border:1px solid #e0e0e0;\" cellpadding=\"0\"> <tr> <td style=\"padding:22px;text-align:center;border-bottom:1px solid #e0e0e0;\"> <img src=\"http://basementrenting.nyusoft.in/Content/Images/logo.png\"/> </td></tr><tr> <td style=\"padding:22px;text-align:left;font-size:14px;color:rgb(90,90,90);\"> <p style=\"margin-bottom:5px;\">Hello <strong>Admin</strong>,</p><p style=\"margin-bottom:5px;\">Below person has contacted us:</p><br/> <p style=\"margin-bottom:5px;\"><b>User Name: </b>" + NameOfUser + "</p><p style=\"margin-bottom:5px;\"><b>Email: </b>" + EmailId + "</p><p style=\"margin-bottom:5px;\"><b>Phone: </b>" + PhoneNumber + "</p><p style=\"margin-bottom:5px;\"><b>Message: </b>" + MessageContent + "</p></td></tr><tr> <td style=\"background:#eaeaea;text-align:center;padding:8px;\" bgcolor=\"#EAEAEA\" align=\"center\"><p style=\"margin:0\">Thank you, <strong>Basement Renting</strong></p></td></tr></table></div>";
            AdminMailStatus = _objCommonController.SendMail(_settingRepository.GetValueBySettingName("AdminEmailId"), "Contact Us", EmailBody);
            #endregion Send mail to admin

            #region Send mail to user
            EmailBody = "<div style=\"text-align:center;font-family: Verdana,Arial,Helvetica,sans-serif;\"> <table style=\"width:100%;max-width:650px;margin:0 auto;border:none;border-collapse:collapse;border:1px solid #e0e0e0;\" cellpadding=\"0\"> <tr> <td style=\"padding:22px;text-align:center;border-bottom:1px solid #e0e0e0;\"> <img src=\"http://basementrenting.nyusoft.in/Content/Images/logo.png\"/> </td></tr><tr> <td style=\"padding:22px;text-align:left;font-size:14px;color:rgb(90,90,90);\"> <p style=\"margin-bottom:5px;\">Hello <strong>" + NameOfUser + "</strong>,</p><p style=\"margin-bottom:5px;\"><b>Greetings from the Basement Renting!</b></p><br/> <p style=\"margin-bottom:5px;\">Thank you for contacting us, we'll get back to you soon for your query.</p></td></tr><tr> <td style=\"background:#eaeaea;text-align:center;padding:8px;\" bgcolor=\"#EAEAEA\" align=\"center\"><p style=\"margin:0\">Thank you, <strong>Basement Renting</strong></p></td></tr></table></div>";
            UserMailStatus = _objCommonController.SendMail(EmailId, "Thank you for Contacting us", EmailBody);
            #endregion Send mail to user

            if (AdminMailStatus == true && UserMailStatus == true)
            {
                TempData["SentMailStatus"] = "<div class='alert alert-success text-center'>Email has been sent successfully..!</div>";

                return RedirectToAction("contact-us", "customer");
            }
            else
            {
                TempData["SentMailStatus"] = "<div class='alert alert-danger text-center'>Something went wrong, please try again after sometime!</div>";

                return RedirectToAction("contact-us", "customer");
            }
        }
        #endregion

        //public bool RegistrationMailToUser()
        //{
        //    try
        //    {
        //        #region mail to the user
        //        _objCommonController.SendMail("anil.nyusoft@gmail.com", "Registration Successful", "User mail content");
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //        return false;
        //    }
        //}

        //public bool RegistrationMailToAdmin()
        //{
        //    try
        //    {
        //        #region mail to the user
        //        _objCommonController.SendMail("fenil.nyusoft@gmail.com", "Registration Successful - Admin", "Admin mail content");
        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //        return false;
        //    }
        //}

        #region Google reCAPTCHA
        public static ReCaptchaResponse VerifyCaptcha(string secret, string response)
        {
            if (response != null)
            {
                using (System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient())
                {
                    var values = new Dictionary<string, string> {
                        {
                            "secret",
                            secret
                        },
                        {
                            "response",
                            response
                        }
                    };
                    var content = new System.Net.Http.FormUrlEncodedContent(values);
                    var Response = hc.PostAsync("https://www.google.com/recaptcha/api/siteverify", content).Result;
                    var responseString = Response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrWhiteSpace(responseString))
                    {
                        ReCaptchaResponse objReCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(responseString);

                        return objReCaptchaResponse;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }
        public class ReCaptchaResponse
        {
            public bool success
            {
                get;
                set;
            }
            public string challenge_ts
            {
                get;
                set;
            }
            public string hostname
            {
                get;
                set;
            }
            [JsonProperty(PropertyName = "error-codes")]
            public List<string> error_codes
            {
                get;
                set;
            }
        }
        #endregion

    } //end of class
} //end of namespace
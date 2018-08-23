using DataAccess.Interface;
using EntityModel.DomainModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BasementRenting.Controllers
{
    public partial class AdminController : Controller
    {
        private ICommonRepository _commonRepository;
        private CommonController _commonController;
        private IRegisterRepository _registerRepository;
        private IPropertyRepository _propertyRepository;
        private IRegionRepository _regionRepository;

        public AdminController(ICommonRepository CommonRepository, CommonController commonController, IRegisterRepository registerRepository, IPropertyRepository propertyRepository, IRegionRepository regionRepository)
        {
            this._commonRepository = CommonRepository;
            this._commonController = commonController;
            this._registerRepository = registerRepository;
            this._propertyRepository = propertyRepository;
            _regionRepository = regionRepository;
        }

        #region Homepage Content
        [ActionName("homepage-cms")]
        public ActionResult ManageHomePageContent()
        {
            var Userid = Convert.ToInt32(Session["Userid"]);
            if (Userid != 0)
            {
                var UserRole = _commonRepository.getUserRolebyId(Userid).RoleName;
                if (UserRole == "Admin")
                {
                    return View("~/views/admin/contentmanagement/contentmanagement.cshtml", _commonRepository.getHomePageCms());
                }
                else
                {
                    return RedirectToAction("page-not-found", "home");
                }
            }
            else
            {
                return RedirectToAction("login", "customer");
            }
        }

        //To update homepage cms content
        [HttpPost]
        [ActionName("update-homepage-content")]
        [ValidateInput(false)]
        public ActionResult ManageHomePageContent(ContentManagement objContentManagement)
        {
            _commonRepository.UpdateHomePageCms(objContentManagement);

            return RedirectToAction("homepage-cms", "admin");
        }
        #endregion

        #region Terms & Conditions
        [ActionName("terms-conditions")]
        public ActionResult ManageTermsAndConditions()
        {
            var Userid = Convert.ToInt32(Session["Userid"]);
            if (Userid != 0)
            {
                var UserRole = _commonRepository.getUserRolebyId(Userid).RoleName;
                if (UserRole == "Admin")
                {
                    return View("~/views/admin/contentmanagement/contentmanagement.cshtml", _commonRepository.getTermsConditionsPageCms());
                }
                else
                {
                    return RedirectToAction("page-not-found", "home");
                }
            }
            else
            {
                return RedirectToAction("login", "customer");
            }
        }

        //To update terms & conditions content
        [HttpPost]
        [ValidateInput(false)]
        [ActionName("update-termsandconditions")]
        public ActionResult ManageTermsAndConditionsContent(ContentManagement objContentManagement)
        {
            _commonRepository.UpdateTermsConditionsPageCms(objContentManagement);

            return RedirectToAction("terms-conditions", "admin");
        }
        #endregion

        #region User management
        [ActionName("filter-users")]
        [HttpPost]
        public ActionResult FilterUsersByRole(FormCollection fcol)
        {
            if (fcol["UserRoles"] != null)
            {
                return RedirectToAction("manage-users", "admin", new { userroleid = Convert.ToInt32(fcol["UserRoles"]), pageindex = 1 });
            }
            else
            {
                return RedirectToAction("manage-users", "admin");
            }
        }

        [ActionName("manage-users")]
        //[ValidateInput(false)]
        public ActionResult getAllUsersByRole(int UserRoleId, int PageIndex, int PageSize = 10)
        {
            if (Session["userid"] != null && Session["userrole"].ToString() == "Admin")
            {
                List<RegisterCustomer> lstRegisterCustomers = new List<RegisterCustomer>();

                lstRegisterCustomers = _registerRepository.GetAllUsersByRoleIdWithPaging(UserRoleId, PageSize, PageIndex);
                foreach (var userdetails in lstRegisterCustomers)
                {
                    userdetails.EmailId = _commonController.DecryptString(userdetails.EmailId);
                    userdetails.PhoneNo = _commonController.DecryptString(userdetails.PhoneNo);
                }

                #region bind user roles
                List<SelectListItem> lstUserRoles = new List<SelectListItem>();
                //lstUserRoles.Add(new SelectListItem { Text = "Select User role", Value = "0" });
                foreach (var item in _registerRepository.GetUserRole())
                {
                    lstUserRoles.Add(new SelectListItem { Text = item.RoleName, Value = item.RoleId.ToString(), Selected = item.RoleId.ToString() == UserRoleId.ToString() ? true : false });
                }
                ViewBag.UserRoles = lstUserRoles;
                #endregion

                ViewBag.UserRoleId = UserRoleId;
                ViewBag.CurrentPageIndex = PageIndex;

                return View("~/views/admin/usermanagement/userlisting.cshtml", lstRegisterCustomers);
            }
            else
            {
                return RedirectToAction("login-again", "customer");
            }

        }

        [ActionName("update-user")]
        [HttpPost]
        public ActionResult updateUserRecord(int UserId, string Username, string Email, string Phonenumber, bool IsPublished, bool IsDeleted)
        {
            try
            {
                _registerRepository.UpdateUserRecord(UserId, Username, _commonController.EncryptString(Email), _commonController.EncryptString(Phonenumber), IsPublished, IsDeleted);

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        #endregion

        #region Property management
        [ActionName("filter-properties")]
        [HttpPost]
        public ActionResult FilterPropertiesByLandlord(FormCollection fcol)
        {
            if (fcol["lstLandlord"] != null)
            {
                return RedirectToAction("manage-properties", "admin", new { seletecedlandlordid = Convert.ToInt32(fcol["lstLandlord"]), isapproved = Convert.ToBoolean(fcol["lstIsApprvoved"]), pageindex = Convert.ToInt32(fcol["hiddenPageIndex"]) });
            }
            else
            {
                return RedirectToAction("login-again", "customer");
            }
        }

        [ActionName("manage-properties")]
        public ActionResult getAllProperties(int UserRoleId = 2, int SeletecedLandlordId = 0, bool IsApproved = true, int pageindex = 1, int pageSize = 12)
        {
            if (Session["userid"] != null && Session["userrole"].ToString() == "Admin")
            {
                #region Bind Landlord dropdown
                List<SelectListItem> lstLandlord = new List<SelectListItem>();
                lstLandlord.Add(new SelectListItem() { Text = "All Landlord", Value = "0" });
                foreach (var users in _registerRepository.GetAllUsersByRoleId(UserRoleId).OrderBy(x => x.UserName))
                {
                    lstLandlord.Add(new SelectListItem()
                    {
                        Text = users.UserName,
                        Value = users.Id.ToString(),
                        Selected = users.Id == SeletecedLandlordId ? true : false
                    });
                }

                ViewBag.lstLandlord = lstLandlord;
                #endregion

                #region Bind IsApproved
                List<SelectListItem> lstIsApprvoved = new List<SelectListItem>();
                lstIsApprvoved.Add(new SelectListItem() { Text = "Approved", Value = "true", Selected = IsApproved == true ? true : false });
                lstIsApprvoved.Add(new SelectListItem() { Text = "Unapproved", Value = "false", Selected = IsApproved == false ? true : false });

                ViewBag.lstIsApprvoved = lstIsApprvoved;
                #endregion

                //if (SeletecedLandlordId == 0)
                //{
                //    SeletecedLandlordId = Convert.ToInt32(lstLandlord.FirstOrDefault().Value);
                //}
                List<Property> lstProperty = _propertyRepository.GetAllPropertiesByLandlordWithPaging(SeletecedLandlordId, IsApproved, pageindex, pageSize);

                ViewBag.CurrentPageIndex = pageindex;

                return View("~/views/admin/propertymanagement/manageproperty.cshtml", lstProperty);
            }
            else
            {
                return RedirectToAction("login-again", "customer");
            }

        }

        [ActionName("edit-property")]
        public ActionResult EditProperty(int propertyid)
        {
            if (Session["userid"] != null && Session["userrole"].ToString() == "Admin")
            {
                var Property = _propertyRepository.GetPropertyById(propertyid);
                Property.lstPropertyImages = _propertyRepository.getImagesByPropertyId(propertyid);

                Property.lstProvince.Add(new SelectListItem { Text = "Select Province", Value = "0" });
                Property.lstCity.Add(new SelectListItem { Text = "Select City", Value = "0" });

                foreach (var item in _regionRepository.GetAllProvince())
                {
                    Property.lstProvince.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }

                #region binding dropdownlist for no. of bedrooms & bathrooms
                List<SelectListItem> lstNoOfBedrooms = new List<SelectListItem>
                {
                    new SelectListItem { Text = "No. Of Bedrooms", Value = "all"},
                    new SelectListItem { Text = "01", Value = "01"},
                    new SelectListItem { Text = "02", Value = "02"},
                    new SelectListItem { Text = "03", Value = "03"}
                };
                List<SelectListItem> lstNoOfBathrooms = new List<SelectListItem>
                {
                    new SelectListItem { Text = "No. Of Bathrooms", Value = "all"},
                    new SelectListItem { Text = "01", Value = "01"},
                    new SelectListItem { Text = "02", Value = "02"}
                };
                #endregion

                Property.lstNoOfBedrooms.AddRange(lstNoOfBedrooms);
                Property.lstNoOfBathrooms.AddRange(lstNoOfBathrooms);
                Property.ProvinceId = _regionRepository.GetCityById(Property.CityId).ProvinceId;
                Property.AvailableFromDate = Convert.ToDateTime(Property.AvailableFromDate).ToString("MM/dd/yyyy");

                if (TempData["PropertyUpdateStatus"] != null)
                {
                    ViewBag.PropertyUpdateStatus = TempData["PropertyUpdateStatus"].ToString();
                }

                return View("~/Views/admin/propertymanagement/submitproperty.cshtml", Property);
            }
            else
            {
                return RedirectToAction("login-again", "customer");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("save-property")]
        public ActionResult SubmitProperty(FormCollection fcol, List<HttpPostedFileBase> lstPostedFiles)
        {
            if (Session["userid"] != null && Session["userrole"].ToString() == "Admin")
            {
                Property objProperty = new Property();
                var InsertUpdateStatus = false;

                #region Update Property
                objProperty.UserId = Convert.ToInt32(fcol["UserId"]);
                objProperty.Id = Convert.ToInt32(fcol["Id"]);
                objProperty.PropertyTitle = fcol["PropertyTitle"];
                objProperty.StreetAddress = fcol["StreetAddress"];
                objProperty.CityId = Convert.ToInt32(fcol["lstCity"]);
                objProperty.PostalCode = fcol["PostalCode"];
                objProperty.NoOfBedroom = Convert.ToInt32(fcol["lstNoOfBedrooms"]);
                objProperty.NoOfBathroom = Convert.ToInt32(fcol["lstNoOfBathrooms"]);
                objProperty.IsFurnished = Convert.ToBoolean(fcol["IsFurnished"]);
                objProperty.IsFeatured = Convert.ToBoolean(fcol["IsFeatured"]);
                objProperty.PropertyInformation = fcol["PropertyInformation"];
                objProperty.MonthlyRent = Convert.ToInt32(fcol["MonthlyRent"]);
                objProperty.Area = fcol["Area"];
                //objProperty.Availability = true;
                objProperty.AvailableFromDate = Convert.ToDateTime(fcol["AvailableFromDate"]).ToShortDateString();
                //objProperty.Published = true;
                //objProperty.IsApproved = true;
                objProperty.ApproxDistanceFromSchool = fcol["ApproxDistanceFromSchool"];
                objProperty.MajorIntersection = fcol["MajorIntersection"];
                objProperty.IsSeparateEntrance = Convert.ToBoolean(fcol["IsSeparateEntrance"]);
                objProperty.IsSeparateLaundry = Convert.ToBoolean(fcol["IsSeparateLaundry"]);
                objProperty.IsInternet = Convert.ToBoolean(fcol["IsInternet"]);
                objProperty.IsTV = Convert.ToBoolean(fcol["IsTV"]);
                objProperty.IsParkingAvailable = Convert.ToBoolean(fcol["IsParkingAvailable"]);
                objProperty.IsSmokeFriendly = Convert.ToBoolean(fcol["IsSmokeFriendly"]);
                objProperty.IsPetFriendly = Convert.ToBoolean(fcol["IsPetFriendly"]);
                objProperty.IsWalkOutOrNot = Convert.ToBoolean(fcol["IsWalkOutOrNot"]);
                objProperty.IsVegetarian = Convert.ToBoolean(fcol["IsVegetarian"]);
                objProperty.IsFirstAndLastMonthRentRequired = Convert.ToBoolean(fcol["IsFirstAndLastMonthRentRequired"]);

                try
                {
                    //objProperty.Id = _propertyRepository.InsertUpdateProperty(objProperty);
                    _propertyRepository.InsertUpdateProperty(objProperty);
                    InsertUpdateStatus = true;

                    if (InsertUpdateStatus == true)
                    {
                        TempData["PropertyUpdateStatus"] = "Property has been updated successfully!";
                    }
                    else
                    {
                        TempData["PropertyUpdateStatus"] = "Something went wrong, Please try again after sometime..!";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion

                #region Image upload
                if (objProperty.Id != 0)
                {
                    List<Image> lstImages = new List<Image>();
                    string NewDir = Server.MapPath("~/Content/PropertyImages/Landlord_" + fcol["UserId"] + "/");

                    foreach (var file in Directory.GetFiles(Server.MapPath("~/Content/PropertyImages/Landlord_" + fcol["UserId"] + "/temp"), "*.*"))
                    {
                        string FileName = Path.GetFileName(file);
                        var NewDestination = Path.Combine(NewDir, FileName);
                        System.IO.File.Move(file, NewDestination);

                        Image objImage = new Image();
                        objImage.ImagePath = "/Content/PropertyImages/Landlord_" + fcol["UserId"] + "/" + FileName;
                        objImage.PropertyId = objProperty.Id;
                        objImage.IsDeleted = false;

                        lstImages.Add(objImage);
                    }

                    InsertUpdateStatus = _propertyRepository.InsertPropertyImages(lstImages, objProperty.Id);
                }
                #endregion

                return RedirectToAction("edit-property", "admin", new { propertyid = objProperty.Id });
            }
            else
            {
                return RedirectToAction("login-again", "customer");
            }
        }

        [ActionName("delete-property")]
        public bool DeleteProperty(int PropertyId)
        {
            try
            {
                if (Session["userid"] != null && Session["userrole"].ToString() == "Admin")
                {
                    return _propertyRepository.deleteProperty(PropertyId);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("approve-property")]
        public bool ApproveProperty(int PropertyId)
        {
            try
            {
                if (Session["userid"] != null && Session["userrole"].ToString() == "Admin")
                {
                    return _propertyRepository.approveProperty(PropertyId);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ActionName("removeImage")]
        public bool RemoveImageOnEdit(int Id)
        {
            try
            {
                if (Session["userid"] != null && Session["userrole"].ToString() == "Admin")
                {
                    if (Id != 0)
                    {
                        _propertyRepository.deletePropertyImageOnEdit(Id);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UploadPropertyImages()
        {
            if (Session["userid"] != null && Session["userrole"].ToString() == "Admin")
            {
                // Checking no of files injected in Request object
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        string DirectoryPath = Server.MapPath("~/Content/PropertyImages/Landlord_" + Session["userid"] + "/");
                        string TempDirectoryPath = Server.MapPath("~/Content/PropertyImages/Landlord_" + Session["userid"] + "/temp/");

                        if (!Directory.Exists(DirectoryPath))
                        {
                            Directory.CreateDirectory(DirectoryPath);
                        }
                        if (!Directory.Exists(TempDirectoryPath))
                        {
                            Directory.CreateDirectory(TempDirectoryPath);
                        }

                        // Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        List<string> lstImagePath = new List<string>();
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            string fname = Guid.NewGuid().ToString();

                            var path = Server.MapPath("~/Content/PropertyImages/Landlord_" + Session["userid"] + "/temp/" + fname + Path.GetExtension(file.FileName));
                            file.SaveAs(path);

                            lstImagePath.Add("/Content/PropertyImages/Landlord_" + Session["userid"] + "/temp/" + fname + Path.GetExtension(file.FileName));
                        }

                        return Json(lstImagePath);
                    }
                    catch (Exception ex)
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json("No files selected");
                }
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
        #endregion
    }
}
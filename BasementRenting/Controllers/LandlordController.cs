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
    public partial class LandlordController : Controller
    {
        private CommonController _objCommonController;
        private IRegionRepository _regionRepository;
        private IPropertyRepository _propertyRepository;

        public LandlordController(CommonController objCommonController,
            IRegionRepository RegionRepository,
            IPropertyRepository PropertyRepository)
        {
            _objCommonController = objCommonController;
            _regionRepository = RegionRepository;
            _propertyRepository = PropertyRepository;
        }

        [HttpPost]
        public ActionResult getCityListByProvinceId(int ProvinceId)
        {
            try
            {
                List<SelectListItem> lstCitylist = new List<SelectListItem>();
                lstCitylist.Add(new SelectListItem { Text = "Select City", Value = "0" });

                foreach (var item in _regionRepository.GetCityByProvinceId(ProvinceId))
                {
                    lstCitylist.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                }

                return Json(lstCitylist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [ActionName("property")]
        public ActionResult SubmitProperty()
        {
            if (Session["userid"] != null && Session["userrole"].ToString() == "Landlord")
            {
                if (_propertyRepository.getPropertyListByLandlord(Convert.ToInt32(Session["userid"])).Count >= 2)
                {
                    ViewBag.EnableSubmitProperty = false;

                    return View("~/Views/landlord/submitproperty.cshtml");
                }
                else
                {
                    Property objProperty = new Property();
                    objProperty.lstProvince.Add(new SelectListItem { Text = "Select Province", Value = "0" });
                    objProperty.lstCity.Add(new SelectListItem { Text = "Select City", Value = "0" });

                    foreach (var item in _regionRepository.GetAllProvince())
                    {
                        objProperty.lstProvince.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
                    }

                    #region binding dropdownlist for no. of bedrooms & bathrooms
                    List<SelectListItem> lstNoOfBedrooms = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "No. Of Bedrooms", Value = "all"},
                        new SelectListItem { Text = "01", Value = "01"},
                        new SelectListItem { Text = "02", Value = "02"},
                        new SelectListItem { Text = "03", Value = "03"}//,
                        //new SelectListItem { Text = "04", Value = "04"},
                        //new SelectListItem { Text = "05", Value = "05"},
                        //new SelectListItem { Text = "06", Value = "06"}
                    };
                    List<SelectListItem> lstNoOfBathrooms = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "No. Of Bathrooms", Value = "all"},
                        new SelectListItem { Text = "01", Value = "01"},
                        new SelectListItem { Text = "02", Value = "02"}//,
                        //new SelectListItem { Text = "03", Value = "03"},
                        //new SelectListItem { Text = "04", Value = "04"},
                        //new SelectListItem { Text = "05", Value = "05"},
                        //new SelectListItem { Text = "06", Value = "06"}
                    };
                    #endregion

                    objProperty.lstNoOfBedrooms.AddRange(lstNoOfBedrooms);
                    objProperty.lstNoOfBathrooms.AddRange(lstNoOfBathrooms);
                    objProperty.AvailableFromDate = null; //Convert.ToDateTime(objProperty.AvailableFromDate).ToShortDateString();

                    if (TempData["InsertStatus"] != null)
                    {
                        ViewBag.InsertStatus = TempData["InsertStatus"];
                    }

                    ViewBag.EnableSubmitProperty = true;
                    return View("~/Views/landlord/submitproperty.cshtml", objProperty);
                }

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
            if (Session["userid"] != null && Session["userrole"].ToString() == "Landlord")
            {
                Property objProperty = new Property();
                var InsertUpdateStatus = false;

                if (Convert.ToInt32(fcol["Id"]) == 0)
                {
                    #region Insert Property
                    objProperty.UserId = Convert.ToInt32(Session["userid"]);
                    objProperty.PropertyTitle = fcol["PropertyTitle"];
                    objProperty.StreetAddress = fcol["StreetAddress"];
                    objProperty.CityId = Convert.ToInt32(fcol["lstCity"]);
                    objProperty.PostalCode = fcol["PostalCode"];
                    objProperty.NoOfBedroom = Convert.ToInt32(fcol["lstNoOfBedrooms"]);
                    objProperty.NoOfBathroom = Convert.ToInt32(fcol["lstNoOfBathrooms"]);
                    objProperty.IsFurnished = Convert.ToBoolean(fcol["IsFurnished"]);
                    //objProperty.IsFeatured = Convert.ToBoolean(fcol["IsFeatured"]);
                    objProperty.PropertyInformation = fcol["PropertyInformation"];
                    objProperty.MonthlyRent = Convert.ToInt32(fcol["MonthlyRent"]);
                    objProperty.Area = fcol["Area"];
                    objProperty.Availability = true;
                    objProperty.AvailableFromDate = fcol["AvailableFromDate"]; //Convert.ToDateTime(fcol["AvailableFromDate"]);
                    objProperty.Published = true;
                    objProperty.IsApproved = false;
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

                    //var PropertyId = _propertyRepository.InsertUpdateProperty(objProperty);

                    try
                    {
                        objProperty.Id = _propertyRepository.InsertUpdateProperty(objProperty);
                        InsertUpdateStatus = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    #region Image upload
                    //if (PropertyId != 0)
                    //{
                    //    //int uploadedCount = 0;
                    //    //List<Image> lstImages = new List<Image>();
                    //    //foreach (var item in Request.Files)
                    //    //{
                    //    //    var uploadedFile = Request.Files[uploadedCount];
                    //    //    if (uploadedFile.ContentLength > 0)
                    //    //    {
                    //    //        var fileName = Path.GetFileName(uploadedFile.FileName);
                    //    //        var fileSavePath = Server.MapPath("~/Content/PropertyImages/Landlord_" + Session["userid"] + "/property" + PropertyId + Path.GetExtension(fileName));
                    //    //        uploadedFile.SaveAs(fileSavePath);
                    //    //        uploadedCount++;

                    //    //        Image objImage = new Image();
                    //    //        objImage.ImagePath = "/Content/PropertyImages/Landlord_" + Session["userid"] + "/property" + PropertyId + Path.GetExtension(fileName);
                    //    //        objImage.PropertyId = PropertyId;

                    //    //        lstImages.Add(objImage);
                    //    //    }
                    //    //}

                    //    //var lstHiddenPreviewImage = fcol.AllKeys.Where(k => k.StartsWith("HiddenPreviewImage")).ToDictionary(k => k, k => fcol[k]).ToList();
                    //    //foreach (var item in lstHiddenPreviewImage)
                    //    //{
                    //    //    //var fileName = Path.GetFileName(item.Value);
                    //    //}

                    //    List<Image> lstImages = new List<Image>();
                    //    string NewDir = Server.MapPath("~/Content/PropertyImages/Landlord_" + Session["userid"] + "/");

                    //    foreach (var file in Directory.GetFiles(Server.MapPath("~/Content/PropertyImages/Landlord_" + Session["userid"] + "/temp"), "*.*"))
                    //    {
                    //        string FileName = Path.GetFileName(file);
                    //        var NewDestination = Path.Combine(NewDir, FileName);
                    //        System.IO.File.Move(file, NewDestination);

                    //        Image objImage = new Image();
                    //        objImage.ImagePath = "/Content/PropertyImages/Landlord_" + Session["userid"] + "/" + FileName;
                    //        objImage.PropertyId = PropertyId;

                    //        lstImages.Add(objImage);
                    //    }

                    //    InsertUpdateStatus = _propertyRepository.InsertPropertyImages(lstImages, PropertyId);
                    //    if (InsertUpdateStatus == true)
                    //    {
                    //        TempData["InsertStatus"] = "Property has been submitted successfully and will appear on the website shortly.";
                    //    }
                    //    else
                    //    {
                    //        TempData["InsertStatus"] = "Something went wrong, Please try again after sometime..!";
                    //    }
                    //}
                    #endregion

                    if (InsertUpdateStatus == true)
                    {
                        TempData["InsertUpdateStatus"] = "Property has been submitted successfully and will appear on the website shortly.";
                    }
                    else
                    {
                        TempData["InsertUpdateStatus"] = "Something went wrong, Please try again after sometime..!";
                    }
                    #endregion
                }
                else
                {
                    #region Update Property
                    objProperty.UserId = Convert.ToInt32(Session["userid"]);
                    objProperty.Id = Convert.ToInt32(fcol["Id"]);
                    objProperty.PropertyTitle = fcol["PropertyTitle"];
                    objProperty.StreetAddress = fcol["StreetAddress"];
                    objProperty.CityId = Convert.ToInt32(fcol["lstCity"]);
                    objProperty.PostalCode = fcol["PostalCode"];
                    objProperty.NoOfBedroom = Convert.ToInt32(fcol["lstNoOfBedrooms"]);
                    objProperty.NoOfBathroom = Convert.ToInt32(fcol["lstNoOfBathrooms"]);
                    objProperty.IsFurnished = Convert.ToBoolean(fcol["IsFurnished"]);
                    //objProperty.IsFeatured = Convert.ToBoolean(fcol["IsFeatured"]);
                    objProperty.PropertyInformation = fcol["PropertyInformation"];
                    objProperty.MonthlyRent = Convert.ToInt32(fcol["MonthlyRent"]);
                    objProperty.Area = fcol["Area"];
                    //objProperty.Availability = true;
                    objProperty.AvailableFromDate = fcol["AvailableFromDate"]; //Convert.ToDateTime(fcol["AvailableFromDate"]);
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
                        //WAS WORKING PROPERLY IN LOCAL, BUT NOT ON LIVE - SO CHANGED
                        //objProperty.Id = _propertyRepository.InsertUpdateProperty(objProperty);

                        _propertyRepository.InsertUpdateProperty(objProperty);
                        InsertUpdateStatus = true;

                        if (InsertUpdateStatus == true)
                        {
                            TempData["InsertUpdateStatus"] = "Property has been updated successfully!";
                        }
                        else
                        {
                            TempData["InsertUpdateStatus"] = "Something went wrong, Please try again after sometime..!";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    #endregion
                }

                #region Image upload
                if (objProperty.Id != 0)
                {
                    List<Image> lstImages = new List<Image>();
                    string NewDir = Server.MapPath("~/Content/PropertyImages/Landlord_" + Session["userid"] + "/");

                    foreach (var file in Directory.GetFiles(Server.MapPath("~/Content/PropertyImages/Landlord_" + Session["userid"] + "/temp"), "*.*"))
                    {
                        string FileName = Path.GetFileName(file);
                        var NewDestination = Path.Combine(NewDir, FileName);
                        System.IO.File.Move(file, NewDestination);

                        Image objImage = new Image();
                        objImage.ImagePath = "/Content/PropertyImages/Landlord_" + Session["userid"] + "/" + FileName;
                        objImage.PropertyId = objProperty.Id;
                        objImage.IsDeleted = false;

                        lstImages.Add(objImage);
                    }

                    InsertUpdateStatus = _propertyRepository.InsertPropertyImages(lstImages, objProperty.Id);
                }
                #endregion

                return RedirectToAction("edit-property", "landlord", new { propertyid = objProperty.Id });
            }
            else
            {
                return RedirectToAction("login-again", "customer");
            }
        }

        [HttpPost]
        public ActionResult UploadPropertyImages()
        {
            if (Session["userid"] != null && Session["userrole"].ToString() == "Landlord")
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

                            //// Checking for Internet Explorer  
                            //if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            //{
                            //    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            //    fname = testfiles[testfiles.Length - 1];
                            //}
                            //else
                            //{
                            //    fname = file.FileName;
                            //}

                            // Get the complete folder path and store the file inside it.
                            //var path = Path.Combine(Server.MapPath("~/Uploads/temp/"), fname + Path.GetExtension(file.FileName));
                            var path = Server.MapPath("~/Content/PropertyImages/Landlord_" + Session["userid"] + "/temp/" + fname + Path.GetExtension(file.FileName));
                            file.SaveAs(path);

                            lstImagePath.Add("/Content/PropertyImages/Landlord_" + Session["userid"] + "/temp/" + fname + Path.GetExtension(file.FileName));
                        }
                        //// Returns message that successfully uploaded
                        //return Json("File Uploaded Successfully!");

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

        [HttpPost]
        public ActionResult RemovePreviewImage(string ImagePath)
        {
            var TempImagesPath = Server.MapPath("~" + ImagePath.Substring(ImagePath.IndexOf("/Content/")));

            if (System.IO.File.Exists(TempImagesPath))
            {
                System.IO.File.Delete(TempImagesPath);

                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        public ActionResult ClearTempDataOfParticularUser()
        {
            try
            {
                if (Session["userid"] != null && Session["userrole"].ToString() == "Landlord")
                {
                    string TempImagesPath = Server.MapPath("/Content/PropertyImages/Landlord_" + Session["userid"] + "/temp");
                    if (Directory.Exists(TempImagesPath))
                    {
                        DirectoryInfo directory = new DirectoryInfo(TempImagesPath);

                        //delete files:
                        directory.GetFiles().ToList().ForEach(f => f.Delete());

                        //delete folders inside choosen folder
                        directory.GetDirectories().ToList().ForEach(d => d.Delete(true));
                    }

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return RedirectToAction("login-again", "customer");
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

        }

        [ActionName("my-properties")]
        public ActionResult getMyProperties()
        {
            if (Session["userid"] != null && Session["userrole"].ToString() == "Landlord")
            {
                var PropertyList = _propertyRepository.getPropertyListByLandlord(Convert.ToInt32(Session["userid"]));

                return View("~/views/landlord/myproperties.cshtml", PropertyList);
            }
            else
            {
                return RedirectToAction("login-again", "customer");
            }
        }

        [ActionName("delete-property")]
        public bool DeleteMyProperty(int PropertyId)
        {
            try
            {
                if (Session["userid"] != null && Session["userrole"].ToString() == "Landlord")
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

        //[ActionName("edit-property")]
        //public ActionResult EditProperty(int propertyid)
        //{
        //    if (Session["userid"] != null && Session["userrole"].ToString() == "Landlord")
        //    {
        //        if (_propertyRepository.getPropertyListByLandlord(Convert.ToInt32(Session["userid"])).Count >= 2)
        //        {
        //            ViewBag.EnableSubmitProperty = false;

        //            return View("~/Views/landlord/submitproperty.cshtml");
        //        }
        //        else
        //        {
        //            var Property = _propertyRepository.GetPropertyById(propertyid);
        //            Property.lstPropertyImages = _propertyRepository.getImagesByPropertyId(propertyid);
        //            //List<Image> imageList = _propertyRepository.getImagesByPropertyId(propertyid);

        //            Property.lstProvince.Add(new SelectListItem { Text = "Select Province", Value = "0" });
        //            Property.lstCity.Add(new SelectListItem { Text = "Select City", Value = "0" });

        //            foreach (var item in _regionRepository.GetAllProvince())
        //            {
        //                Property.lstProvince.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
        //            }

        //            #region binding dropdownlist for no. of bedrooms & bathrooms
        //            List<SelectListItem> lstNoOfBedrooms = new List<SelectListItem>
        //            {
        //                new SelectListItem { Text = "No. Of Bedrooms", Value = "all"},
        //                new SelectListItem { Text = "01", Value = "01"},
        //                new SelectListItem { Text = "02", Value = "02"},
        //                new SelectListItem { Text = "03", Value = "03"}//,
        //                //new SelectListItem { Text = "04", Value = "04"},
        //                //new SelectListItem { Text = "05", Value = "05"},
        //                //new SelectListItem { Text = "06", Value = "06"}
        //            };
        //            List<SelectListItem> lstNoOfBathrooms = new List<SelectListItem>
        //            {
        //                new SelectListItem { Text = "No. Of Bathrooms", Value = "all"},
        //                new SelectListItem { Text = "01", Value = "01"},
        //                new SelectListItem { Text = "02", Value = "02"}//,
        //                //new SelectListItem { Text = "03", Value = "03"},
        //                //new SelectListItem { Text = "04", Value = "04"},
        //                //new SelectListItem { Text = "05", Value = "05"},
        //                //new SelectListItem { Text = "06", Value = "06"}
        //            };
        //            #endregion

        //            Property.lstNoOfBedrooms.AddRange(lstNoOfBedrooms);
        //            Property.lstNoOfBathrooms.AddRange(lstNoOfBathrooms);
        //            Property.ProvinceId = _regionRepository.GetCityById(Property.CityId).ProvinceId;
        //            Property.AvailableFromDate = Convert.ToDateTime(Property.AvailableFromDate).ToString("MM/dd/yyyy");

        //            if (TempData["InsertUpdateStatus"] != null)
        //            {
        //                ViewBag.InsertUpdateStatus = TempData["InsertUpdateStatus"].ToString();
        //            }

        //            ViewBag.EnableSubmitProperty = true;

        //            return View("~/Views/landlord/submitproperty.cshtml", Property);
        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("login", "customer");
        //    }
        //}

        [ActionName("edit-property")]
        public ActionResult EditProperty(int propertyid)
        {
            if (Session["userid"] != null && Session["userrole"].ToString() == "Landlord")
            {
                var Property = _propertyRepository.GetPropertyById(propertyid);
                Property.lstPropertyImages = _propertyRepository.getImagesByPropertyId(propertyid);
                //List<Image> imageList = _propertyRepository.getImagesByPropertyId(propertyid);

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

                if (TempData["InsertUpdateStatus"] != null)
                {
                    ViewBag.InsertUpdateStatus = TempData["InsertUpdateStatus"].ToString();
                }

                ViewBag.EnableSubmitProperty = true;

                return View("~/Views/landlord/submitproperty.cshtml", Property);
            }
            else
            {
                return RedirectToAction("login", "customer");
            }
        }

        [ActionName("removeImage")]
        public bool RemoveImageOnEdit(int Id)
        {
            try
            {
                if (Session["userid"] != null && Session["userrole"].ToString() == "Landlord")
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
    } //end of class
} //end of namespace
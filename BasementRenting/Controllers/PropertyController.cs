using DataAccess.Interface;
using EntityModel.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BasementRenting.Controllers
{
    public partial class PropertyController : Controller
    {
        private CommonController _objCommonController;
        private IPropertyRepository _propertyRepository;
        private IRegionRepository _regionRepository;

        public PropertyController(IPropertyRepository propertyRepository,
            CommonController objCommonController,
            IRegionRepository regionRepository)
        {
            this._propertyRepository = propertyRepository;
            this._objCommonController = objCommonController;
            this._regionRepository = regionRepository;
        }

        [ActionName("get-properties-by-city")]
        public ActionResult getPropertiesByCityId(int cityId, string city, string sortby, int pageindex)
        {
            #region sorting options
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Unsorted",
                Value = "unsorted",
                Selected = true
            });
            listItems.Add(new SelectListItem
            {
                Text = "Price - Low To High",
                Value = "lowtohigh"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Price - High To Low",
                Value = "hightolow"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Bedrooms - Most to Least",
                Value = "mosttoleast"
            });
            listItems.Add(new SelectListItem
            {
                Text = "Bedrooms - Least to Most",
                Value = "leasttomost"
            });
            #endregion

            #region set viewbag value, so can be access from listing page
            ViewBag.SortingOptions = listItems;

            if (city != null)
            {

                Session["CurrentLocation"] = city.ToString();
            }
            //else
            //{
            //    Session.Remove("CurrentLocation");
            //}

            //if (city == null)
            //{
            //    //Request.Cookies["CityName"].Value;
            //    var CitybyCookie = Request.Cookies["CurrentLocation"].Value;
            //    HttpCookie CurrentLocation = new HttpCookie("CurrentLocation");
            //    CurrentLocation.Value = CitybyCookie;
            //    HttpContext.Response.Cookies.Add(CurrentLocation);
            //}
            //else
            //{
            //    city = city.Replace(Environment.NewLine, string.Empty);
            //    HttpCookie CurrentLocation = new HttpCookie("CurrentLocation");

            //    // Set the cookie value
            //    DateTime now = DateTime.Now;
            //    CurrentLocation.Domain = Request.Url.Host.ToString();
            //    CurrentLocation.Value = Convert.ToString(city);
            //    CurrentLocation.Expires = now.AddYears(1);
            //    Response.Cookies.Add(CurrentLocation);
            //}

            ViewBag.CityId = cityId;
            ViewBag.sortby = sortby;
            ViewBag.pageindex = pageindex;
            #endregion

            return View("PropertyListing", _propertyRepository.getPropertiesByCityId(cityId, sortby, pageindex));
        }


        //[ActionName("change-location")]
        //[HttpPost]
        //public ActionResult changeByCityId(int CityId, string city, string sortby, int pageindex=1)
        //{
        //    #region sorting options
        //    List<SelectListItem> listItems = new List<SelectListItem>();
        //    listItems.Add(new SelectListItem
        //    {
        //        Text = "Unsorted",
        //        Value = "unsorted",
        //        Selected = true
        //    });
        //    listItems.Add(new SelectListItem
        //    {
        //        Text = "Price - Low To High",
        //        Value = "lowtohigh"
        //    });
        //    listItems.Add(new SelectListItem
        //    {
        //        Text = "Price - High To Low",
        //        Value = "hightolow"
        //    });
        //    listItems.Add(new SelectListItem
        //    {
        //        Text = "Bedrooms - Most to Least",
        //        Value = "mosttoleast"
        //    });
        //    listItems.Add(new SelectListItem
        //    {
        //        Text = "Bedrooms - Least to Most",
        //        Value = "leasttomost"
        //    });
        //    #endregion

        //    #region set viewbag value, so can be access from listing page
        //    ViewBag.SortingOptions = listItems;

        //    if (city == null)
        //    {
        //        //Request.Cookies["CityName"].Value;
        //        var CitybyCookie = Request.Cookies["CurrentLocation"].Value;
        //        HttpCookie CurrentLocation = new HttpCookie("CurrentLocation");
        //        CurrentLocation.Value = CitybyCookie;
        //        HttpContext.Response.Cookies.Add(CurrentLocation);
        //    }
        //    else
        //    {
        //        city = city.Replace(Environment.NewLine, string.Empty);
        //        HttpCookie CurrentLocation = new HttpCookie("CurrentLocation");

        //        // Set the cookie value
        //        DateTime now = DateTime.Now;
        //        CurrentLocation.Domain = Request.Url.Host.ToString();
        //        CurrentLocation.Value = Convert.ToString(city);
        //        CurrentLocation.Expires = now.AddYears(1);
        //        Response.Cookies.Add(CurrentLocation);
        //    }

        //    ViewBag.CityId = CityId;
        //    ViewBag.sortby = sortby;
        //    ViewBag.pageindex = pageindex;
        //    #endregion

        //    return View("PropertyListing", _propertyRepository.getPropertiesByCityId(CityId, sortby, pageindex));
        //}


        [HttpPost]
        public ActionResult SortProperties(FormCollection fcol)
        {
            return RedirectToAction("get-properties-by-city", "property", new { @cityId = fcol["hiddenCityId"], @sortby = fcol["hiddenSortBy"], @PageSize = fcol["hiddenPageSize"], @pageindex = fcol["hiddenPageIndex"] });
        }

        [ActionName("get-featured-properties")]
        public ActionResult getFeaturedProperties()
        {
            return PartialView("FeaturedProperties", _propertyRepository.getFeaturedProperties());
        }

        [ActionName("get-latest-properties")]
        public ActionResult getLatestProperties()
        {
            return PartialView("LatestProperties", _propertyRepository.getLatestProperties());
        }

        public ActionResult SearchPropertyForm(SearchProperty objSearchProperty)
        {
            #region bind province & city dropdown
            objSearchProperty.lstProvince.Add(new SelectListItem { Text = "Province", Value = "0" });
            foreach (var item in _regionRepository.GetAllProvince())
            {
                objSearchProperty.lstProvince.Add(new SelectListItem { Text = item.Name, Value = Convert.ToString(item.Id) });
            }
            objSearchProperty.lstCity.Add(new SelectListItem { Text = "City", Value = "0" });
            #endregion

            return PartialView("_locationform", objSearchProperty);
        }

        [ActionName("get-searched-properties")]
        public ActionResult getSearchedProperties()
        {
            SearchProperty objSearchProperty = new SearchProperty();
            objSearchProperty.Keyword = "";
            objSearchProperty.CurrentPageIndex = 1;

            ViewBag.objSearchProperty = objSearchProperty;
            ViewBag.HiddenPageIndex = objSearchProperty.CurrentPageIndex;

            return View("SearchPropertyListing", _propertyRepository.getSearchedProperties(objSearchProperty));
        }

        [HttpPost]
        [ActionName("get-searched-properties")]
        public ActionResult getSearchedProperties(SearchProperty objSearchProperty, FormCollection fcol)
        {
            if (fcol["hiddenPageIndex"] != null)
            {
                objSearchProperty.CurrentPageIndex = Convert.ToInt32(fcol["hiddenPageIndex"]);
            }
            else
            {
                objSearchProperty.CurrentPageIndex = 1;
            }

            if (string.IsNullOrEmpty(objSearchProperty.Keyword) && fcol["Keyword"] != null)
            {
                objSearchProperty.Keyword = fcol["Keyword"];
            }

            if (fcol["lstProvince"] != null)
            {
                objSearchProperty.ProvinceId = Convert.ToInt32(fcol["lstProvince"]);
            }
            if (fcol["lstCity"] != null)
            {
                objSearchProperty.CityId = Convert.ToInt32(fcol["lstCity"]);
            }

            if (objSearchProperty.AvailableFromDate == DateTime.MinValue.ToShortDateString())
            {
                objSearchProperty.AvailableFromDate = "";
            }

            ViewBag.objSearchProperty = objSearchProperty;
            ViewBag.HiddenPageIndex = objSearchProperty.CurrentPageIndex;

            return View("SearchPropertyListing", _propertyRepository.getSearchedProperties(objSearchProperty));
        }

        public ActionResult PropertyDetails(int PropertyId)
        {
            Property objProperty = new Property();
            objProperty = _propertyRepository.GetPropertyById(PropertyId);
            objProperty.OwnerPhoneNumber = _objCommonController.DecryptString(objProperty.OwnerPhoneNumber);
            objProperty.lstPropertyImages = _propertyRepository.getImagesByPropertyId(PropertyId);
            objProperty.CityName = _regionRepository.GetCityById(objProperty.CityId).Name;
            objProperty.AvailableFromDate = Convert.ToDateTime(objProperty.AvailableFromDate).ToShortDateString();

            if (Session["userid"] != null)
            {
                objProperty.IsFavourite = IsFavouriteProperty(objProperty.Id);
            }

            return View("PropertyDetails", objProperty);
        }

        [HttpPost]
        [ActionName("user-to-landlord-email")]
        public bool SendEmailFromUserToLandlord(string PropertyTitle, string LandlordEmailId, string FullName, string CustomerEmail, string CustomerPhone, string Message)
        {
            try
            {
                #region send mail from user to landlord
                //var EmailBodyContent = "Hello, <br /><strong>Greetings from the Basement Renting!</strong><br /><br />Below User is interested in your <strong>" + PropertyTitle + "</strong> property. <br />User name: " + FullName + "<br />email: " + CustomerEmail + "<br />Phone: " + CustomerPhone + "<br />Message: " + Message;
                var EmailBodyContent = "<div style=\"text-align:center;font-family: Verdana,Arial,Helvetica,sans-serif;\"> <table style=\"width:100%;max-width:650px;margin:0 auto;border:none;border-collapse:collapse;border:1px solid #e0e0e0;\" cellpadding=\"0\"> <tr> <td style=\"padding:22px;text-align:center;border-bottom:1px solid #e0e0e0;\"> <img src=\"http://basementrenting.nyusoft.in/Content/Images/logo.png\"/> </td></tr><tr> <td style=\"padding:22px;text-align:left;font-size:14px;color:rgb(90,90,90);\"> <p style=\"margin-bottom:5px;\">Hello,</p><p style=\"margin-bottom:5px;\"><strong>Greetings from the Basement Renting!</strong></p><p style=\"margin-bottom:5px;\">Below User is interested in your <strong>" + PropertyTitle + "</strong> property.</p><br/> <p style=\"margin-bottom:5px;\"><b>User Name: </b>" + FullName + "</p><p style=\"margin-bottom:5px;\"><b>Email: </b>" + CustomerEmail + "</p><p style=\"margin-bottom:5px;\"><b>Phone: </b>" + CustomerPhone + "</p><p style=\"margin-bottom:5px;\"><b>Message: </b>" + Message + "</p></td></tr><tr> <td style=\"background:#eaeaea;text-align:center;padding:8px;\" bgcolor=\"#EAEAEA\" align=\"center\"><p style=\"margin:0\">Thank you, <strong>Basement Renting</strong></p></td></tr></table></div>";
                _objCommonController.SendMail(_objCommonController.DecryptString(LandlordEmailId), "Property Inquiry", EmailBodyContent);
                #endregion

                #region send mail to user
                EmailBodyContent = "<div style=\"text-align:center;font-family: Verdana,Arial,Helvetica,sans-serif;\"> <table style=\"width:100%;max-width:650px;margin:0 auto;border:none;border-collapse:collapse;border:1px solid #e0e0e0;\" cellpadding=\"0\"> <tr> <td style=\"padding:22px;text-align:center;border-bottom:1px solid #e0e0e0;\"> <img src=\"http://basementrenting.nyusoft.in/Content/Images/logo.png\"/> </td></tr><tr> <td style=\"padding:22px;text-align:left;font-size:14px;color:rgb(90,90,90);\"> <p style=\"margin-bottom:5px;\">Hello " + FullName + ",</p><p style=\"margin-bottom:5px;\"><strong>Greetings from the Basement Renting!</strong></p><p style=\"margin-bottom:5px;\">Thank you for your interest in our  <strong>" + PropertyTitle + "</strong> property. We'll contact you back very soon regarding your inquiry.</p><br/> <p style=\"margin-bottom:5px;\"></p></td></tr><tr> <td style=\"background:#eaeaea;text-align:center;padding:8px;\" bgcolor=\"#EAEAEA\" align=\"center\"><p style=\"margin:0\">Thank you, <strong>Basement Renting</strong></p></td></tr></table></div>";
                _objCommonController.SendMail(CustomerEmail, "Thank you for Property Inquiry", EmailBodyContent);
                #endregion

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ActionResult GetSimilarProperties(int NoOfBedrooms, int NoOfBathrooms)
        {
            return PartialView("SimilarProperties", _propertyRepository.getSimilarProperties(NoOfBedrooms, NoOfBathrooms));
        }

        public bool IsFavouriteProperty(int PropertyId)
        {
            var PropertyIdsFromDb = _propertyRepository.getFavouritePropertyIdsByUserId(Convert.ToInt32(Session["userid"]));
            List<int> lstPropertyIds = new List<int>();

            if (PropertyIdsFromDb != null)
            {
                if (PropertyIdsFromDb.PropertyIds.Contains(","))
                {
                    lstPropertyIds = PropertyIdsFromDb.PropertyIds.ToString().Split(',').Select(int.Parse).ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(PropertyIdsFromDb.PropertyIds))
                    {
                        lstPropertyIds.Add(Convert.ToInt32(PropertyIdsFromDb.PropertyIds));
                    }
                }

                if (lstPropertyIds.Contains(PropertyId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult ToggleFavouritePropertyByUser(int PropertyId, bool ToggleFavourite)
        {
            bool IsFavourite;
            List<int> lstPropertyIds = new List<int>();
            try
            {
                if (Session["userid"] != null)
                {
                    var PropertyAndFavMapping = _propertyRepository.getFavouritePropertyIdsByUserId(Convert.ToInt32(Session["userid"]));
                    if (PropertyAndFavMapping != null)
                    {
                        if (PropertyAndFavMapping.PropertyIds.Contains(","))
                        {
                            lstPropertyIds = PropertyAndFavMapping.PropertyIds.Split(',').Select(int.Parse).ToList();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(PropertyAndFavMapping.PropertyIds))
                            {
                                lstPropertyIds.Add(Convert.ToInt32(PropertyAndFavMapping.PropertyIds));
                            }
                        }

                        if (lstPropertyIds.Contains(PropertyId))
                        {
                            lstPropertyIds.Remove(PropertyId);
                            IsFavourite = false;
                        }
                        else
                        {
                            lstPropertyIds.Add(PropertyId);
                            IsFavourite = true;
                        }

                        var UpdateStatus = _propertyRepository.UpdateFavouritePropertiesByUserId(string.Join(",", lstPropertyIds), Convert.ToInt32(Session["userid"]));
                        if (UpdateStatus == true)
                        {
                            return Json(IsFavourite);
                        }
                        else
                        {
                            return Json(IsFavourite);
                        }
                    }
                    else
                    {
                        lstPropertyIds.Add(PropertyId);
                        IsFavourite = true;

                        var UpdateStatus = _propertyRepository.UpdateFavouritePropertiesByUserId(string.Join(",", lstPropertyIds), Convert.ToInt32(Session["userid"]));
                        if (UpdateStatus == true)
                        {
                            return Json(IsFavourite);
                        }
                        else
                        {
                            return Json(IsFavourite);
                        }
                    }
                }
                else
                {
                    return Json("User needs to login first!");
                }
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex);
            }

        }

        //[HttpPost]
        [ActionName("favorite-properties")]
        public ActionResult FavouritePropertiesByUser()
        {
            List<Property> lstFavouriteProperties = new List<Property>();
            if (Session["userid"] != null)
            {
                //var FavPropertyIds = _propertyRepository.getFavouritePropertyIdsByUserId(Convert.ToInt32(Session["userid"])).PropertyIds;
                //if(string.IsNullOrEmpty(FavPropertyIds))
                //{
                //    FavPropertyIds = "0";
                //}

                lstFavouriteProperties = _propertyRepository.getFavouritePropertiesByUserId(Convert.ToInt32(Session["userid"]));
            }

            return View("FavouritePropertyListing", lstFavouriteProperties);
        }

        [ActionName("properties-by-city")]
        public ActionResult GetPropertiesByCityName(FormCollection fcol)
        {
            ViewBag.SearchByCityName = fcol["txtCityName"].ToString();
            ViewBag.pageindex = 1;

            return View("PropertyListing", _propertyRepository.getPropertiesByCityName(fcol["txtCityName"].ToString()));
        }
    }
}
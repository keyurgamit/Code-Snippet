using DataAccess.Interface;
using EntityModel.DomainModel;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace BasementRenting.Controllers
{
    public class SavedSearchController : Controller
    {
        private ISavedSearchRepository _savedSearchRepository;
        private IRegionRepository _regionRepository;
        public SavedSearchController(ISavedSearchRepository savedSearchRepository, IRegionRepository regionRepository)
        {
            this._savedSearchRepository = savedSearchRepository;
            this._regionRepository = regionRepository;
        }

        [HttpPost]
        [ActionName("save-search")]
        public ActionResult SaveSearch(string objSaveSearchModelData)
        {
            if (Session["userid"] != null)
            {
                try
                {
                    SearchProperty objSearchProperty = new SearchProperty();
                    objSearchProperty = JsonConvert.DeserializeObject<SearchProperty>(objSaveSearchModelData);
                    objSearchProperty.UserId = Convert.ToInt32(Session["userid"]);

                    var ExistSavedSearch = _savedSearchRepository.SearchResultsByUserId(objSearchProperty.UserId);
                    if (ExistSavedSearch != null)
                    {
                        objSearchProperty.Id = ExistSavedSearch.Id;
                    }

                    var RecordId = _savedSearchRepository.SaveSearch(objSearchProperty);

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }


        [ActionName("search-by-user")]
        public ActionResult getSavedSearchesByUserId()
        {
            if (Session["userid"] != null)
            {
                SearchProperty objSearchProperty = new SearchProperty();

                objSearchProperty = _savedSearchRepository.SearchResultsByUserId(Convert.ToInt32(Session["userid"]));

                if (objSearchProperty.AvailableFromDate != DateTime.MinValue.ToShortDateString() && objSearchProperty.AvailableFromDate != null)
                {
                    objSearchProperty.AvailableFromDate = Convert.ToDateTime(objSearchProperty.AvailableFromDate).ToShortDateString();
                }
                else
                {
                    objSearchProperty.AvailableFromDate = string.Empty;
                }

                var ProvinceName = _regionRepository.GetProvinceById(objSearchProperty.ProvinceId) != null ? _regionRepository.GetProvinceById(objSearchProperty.ProvinceId).Name : null;
                var CityName = _regionRepository.GetCityById(objSearchProperty.CityId) != null ? _regionRepository.GetCityById(objSearchProperty.CityId).Name : null;

                ViewBag.ProvinceName = ProvinceName;
                ViewBag.CityName = CityName;
                return View("~/views/customer/savedsearch.cshtml", objSearchProperty);
            }
            else
            {
                return RedirectToAction("login-again", "customer");
            }


        }
    }
}
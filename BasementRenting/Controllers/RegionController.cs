using DataAccess.Interface;
using System;
using System.Web.Mvc;

namespace BasementRenting.Controllers
{
    public class RegionController : Controller
    {
        private IRegionRepository _RegionRepository;
        public RegionController(IRegionRepository RegionRepository)
        {
            this._RegionRepository = RegionRepository;
        }

        [HttpGet]
        [ActionName("browse-rental-location")]
        public ActionResult GetAllProvince()
        {
            try
            {
                return View("BrowseProvince", _RegionRepository.GetAllProvince());
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetProvinceById(int Id)
        {
            try
            {
                return View("BrowseProvince", _RegionRepository.GetProvinceById(Id));
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("CityList")]
        public ActionResult GetAllCity()
        {
            try
            {
                return View("BrowseCity", _RegionRepository.GetAllCity());
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetCityById(int Id)
        {
            try
            {
                return View("BrowseProvince", _RegionRepository.GetCityById(Id));
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAllCityByProvinceId(int ProvinceId)
        {
            try
            {
                return PartialView("BrowseAllCity", _RegionRepository.GetCityByProvinceId(ProvinceId));
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ActionName("cities-by-name")]
        public ActionResult GetCitylistByName(string CityName)
        {
            try
            {
                return Json(new { CitylistStatus = true, CityList = _RegionRepository.GetCitylistByName(CityName) });
            }
            catch (Exception ex)
            {
                return Json(new { CitylistStatus = false, CityList = "" });
            }
        }
    }
}
using DataAccess.Interface;
using System;
using System.Web.Mvc;

namespace BasementRenting.Controllers
{
    public class HomeController : Controller
    {
        private ICommonRepository _commonRepository;
        public HomeController(ICommonRepository commonRepository)
        {
            this._commonRepository = commonRepository;
        }

        // GET: Home
        public ActionResult Index()
        {
            //if (TempData["RegisterStatusMessage"] != null && TempData.Peek("RegisterStatusMessage").ToString() == "SuccessfullyRegistered")
            //{
            //    ViewBag.RegisterStatusMessage = "You are successfully registered..!";
            //}

            return View();
        }

        [ActionName("homepage-welcome-content")]
        public string ManageHomePageContent()
        {
            return _commonRepository.getHomePageCms().Value;
        }

        [ActionName("terms-and-conditions")]
        public ActionResult TermsAndConditionsPageContent()
        {
            ViewBag.TermsAndConditionsContent = _commonRepository.getTermsConditionsPageCms().Value;

            return View("~/views/common/termsandconditions.cshtml");
        }

        [ActionName("page-not-found")]
        public ActionResult PageNotFound()
        {
            return View("~/views/home/pagenotfound.cshtml");
        }
    }
}
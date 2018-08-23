using DataAccess.Interface;
using System;
using System.Web.Mvc;

namespace BasementRenting.Controllers
{
    public class SettingController : Controller
    {
        private ISettingRepository _SettingRepository;
        public SettingController(ISettingRepository SettingRepository)
        {
            this._SettingRepository = SettingRepository;
        }

        [HttpPost]
        [ActionName("get-settingvalue-by-key")]
        public string GetValueBySettingName(string SettingName)
        {
            return _SettingRepository.GetValueBySettingName(SettingName);
        }

    }
}
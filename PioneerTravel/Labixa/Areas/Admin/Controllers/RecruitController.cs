using AutoMapper;
using Labixa.Areas.Admin.ViewModel;
using Outsourcing.Core.Common;
using Outsourcing.Core.Framework.Controllers;
using Outsourcing.Data.Models;
using Outsourcing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    public class RecruitController : Controller
    {
        #region Field
        readonly IRecruitService _RecruitService;
        readonly IDepartmentService _DepartmentService;
        readonly ILocationService _LocationService;
        #endregion

        #region Ctor
        public RecruitController(IRecruitService Recruitervice, IDepartmentService departmentService, ILocationService locationService)
        {
            this._RecruitService = Recruitervice;
            this._DepartmentService = departmentService;
            this._LocationService = locationService;
        }
        #endregion
        //
        // GET: /Admin/Recruit/
        public ActionResult Index()
        {
            var list = _RecruitService.GetRecruits();
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Recruit = _RecruitService.GetRecruits();
            ViewBag.Department = _DepartmentService.GetDepartments();
            ViewBag.Location = _LocationService.GetLocations();
            var model = new RecruitFormModel();
         
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(RecruitFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                obj.Deleted = false;
                Recruit item = Mapper.Map<RecruitFormModel, Recruit>(obj);
                if (String.IsNullOrEmpty(item.Temp_1))
                {
                    item.Temp_1 = StringConvert.ConvertShortName(item.Title);
                }
                _RecruitService.CreateRecruit(item);
                return continueEditing ? RedirectToAction("Edit", "Recruit", new { RecruitId = item.Id })
                                 : RedirectToAction("Index", "Recruit");
            }
            else return View("Create", obj);
        }
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit(int RecruitId)
        {
            var item = _RecruitService.GetRecruitById(RecruitId);
            ViewBag.Department = _DepartmentService.GetDepartments();
            ViewBag.Location = _LocationService.GetLocations();
            RecruitFormModel obj = Mapper.Map<Recruit, RecruitFormModel>(item);
            if (obj != null)
            {
                //int year = obj.DayStart.Year;
                //int month = obj.DayStart.Month;
                //int day = obj.DayStart.Day;
                //string strDay = day.ToString();
                //string strMonth = month.ToString();
                //if (month < 10)
                //{
                //    strMonth = "0" + month;
                //}
                //if (day < 10)
                //{
                //    strDay = "0" + day;
                //}
                //string kq = year + "-" + strMonth + "-" + strDay;
                //obj.DayStart = DateTime.Parse(kq);
                return View(obj);
            }
            return RedirectToAction("Index", "Recruit");
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(RecruitFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Department = _DepartmentService.GetDepartments();
                ViewBag.Location = _LocationService.GetLocations();
                Recruit item = Mapper.Map<RecruitFormModel, Recruit>(obj);
                if (String.IsNullOrEmpty(item.Temp_1))
                {
                    item.Temp_1 = StringConvert.ConvertShortName(item.Title);
                }
                _RecruitService.EditRecruit(item);
                return continueEditing ? RedirectToAction("Edit", "Recruit", new { RecruitId = item.Id })
                    : RedirectToAction("Index", "Recruit");
            }
            else
                return View("Edit", obj);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int RecruitId)
        {
            _RecruitService.DeleteRecruit(RecruitId);
            return RedirectToAction("Index", "Recruit");
        }


    }
}
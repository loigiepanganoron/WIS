using IMS.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Data.Entity;
using System.Web.Services;
using System.Data;
using IMS.@class;
using System.Data.SqlClient;
using IMS.Classess;
using System.Web.Script.Serialization;
namespace IMS.Controllers
{
    public class PersonelController : BaseController
    {
        private pmisEntities pmis = new pmisEntities();
        private IMSEntities db = new IMSEntities();

        // GET: Personel
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Summary()
        {
            return View();
        }
        public ActionResult Wis()
        {
            return View();
        }
        public ActionResult _in()
        {
            return PartialView();
        }
        public ActionResult _out()
        {
            return PartialView();
        }
        public ActionResult monitor()
        {
            return PartialView();
        }
        public ActionResult printer()
        {
            return PartialView("openprinter");
        }
        public ActionResult printernapud()
        {
            return PartialView("printernapud");
        }
        public ActionResult Totalavailable([DataSourceRequest] DataSourceRequest request, int officeid , string all)
        {
            ViewBag.id = officeid;
            ViewBag.all = all;
            read rd = new read();
            // int x = officeid == null ? 0 : officeid;, 
            var data = rd.TotalAll(officeid, all);
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult TotalOut([DataSourceRequest] DataSourceRequest request, int officeid, string all)
        {
            ViewBag.id = officeid;
            ViewBag.all = all;
            read rd = new read();
            // int x = officeid == null ? 0 : officeid;
            var data = rd.TotalOut(officeid, all);
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult wischrt([DataSourceRequest] DataSourceRequest request, int officeid, int itemid)
        {
            ViewBag.officeid = officeid;
            ViewBag.itemid = itemid;
            read rd = new read();
            // int x = officeid == null ? 0 : officeid;
            var data = rd.wischart(officeid, itemid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult imgwis([DataSourceRequest] DataSourceRequest request, int eid )
        {
            ViewBag.eid = eid;
            wis data = new wis();
            var d = data.Employee(eid);
            return Json(d.ToDataSourceResult(request));
        }
        public ActionResult imgwis2([DataSourceRequest] DataSourceRequest request, int req_eid)
        {
            DataTable dt =  (@"select * from [pmis].[dbo].[m_vwGetAllEmployee] where isactive=1 and eid ='" + req_eid + "'").DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public ActionResult emp() { 
            return PartialView();
        }
        public ActionResult employeeLista([DataSourceRequest] DataSourceRequest request, int id)
        {
            var dt = pmis.vwMergeAllEmployees.Where(b => b.Department == id).Select(a => new { eid = a.eid, empName = a.EmpName }).OrderBy(b => b.empName).ToList();
            return Json(dt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult manualin()
        {
            return PartialView();
        }
    }
}
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

namespace IMS.Controllers
{
    public class PlcController : BaseController
    {
        // GET: Plc
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult plcrep()
        {
            return PartialView("plcprinter");
        }
        public ActionResult PLC()
        {

            return View("PLC");
        }
        public ActionResult plcitems([DataSourceRequest] DataSourceRequest request)
        {
            read rd = new read();
            var d = rd.plcitems();
            return Json(d, JsonRequestBehavior.AllowGet);
        }
        public ActionResult plcunits([DataSourceRequest] DataSourceRequest request) 
        {
            read rd = new read();
            var d = rd.plcunits();
            return Json(d, JsonRequestBehavior.AllowGet);
        }
        public ActionResult plcunits2([DataSourceRequest] DataSourceRequest request)
        {
            read rd = new read();
            var d = rd.plcunits();
            return Json(d.ToDataSourceResult(request));
        }
        public string sv(item it) 
        {
            savein r = new savein();
            return r.ins(it);
        }
        public string Savein(item it)
        {
            savein r = new savein();
            return r.b(it);
        }
        public ActionResult readplc([DataSourceRequest] DataSourceRequest request)
        {
            read r = new read();
            var d = r.readplcgrid();
            return Json(d.ToDataSourceResult(request));
        }
        public ActionResult chart([DataSourceRequest] DataSourceRequest request, int itemid)
        {
            ViewBag.id = itemid;
            read r = new read();
            var d = r.chart(itemid);
            return Json(d,JsonRequestBehavior.AllowGet);
        }

        public ActionResult plctrns([DataSourceRequest] DataSourceRequest request)
        {
            read r = new read();
            var d = r.plctrans();
            return Json(d.ToDataSourceResult(request));
        }
        public string plcsave(item it)
        {
            savein r = new savein();
            return r.plcsaveout(it);
        }
        public PartialViewResult plcemp()
        { 
             return PartialView();
        }
    }
}
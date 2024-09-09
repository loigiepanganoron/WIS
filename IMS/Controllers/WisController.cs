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
    public class WisController : BaseController
    {
        // GET: Wis
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult wis()
        {
            return View();
        }
        public PartialViewResult instock()
        {
            return PartialView();
        }
        public PartialViewResult balance()
        {
            return PartialView();
        }
        //read instock
        public ActionResult wis_instock([DataSourceRequest] DataSourceRequest request)
        {
            wis_read rd = new wis_read();
            var data = rd.read_instock();
            return Json(data.ToDataSourceResult(request));
        }

        public PartialViewResult popupOwner()
        {
            return PartialView(); 
        }
        public ActionResult wis_item_owner([DataSourceRequest] DataSourceRequest request, int itemid)
        {
            ViewBag.itemid = itemid;
            wis_read rd = new wis_read();
            var data = rd.read_Owneroffice(itemid);
            return Json(data.ToDataSourceResult(request));
        }

        public PartialViewResult history_in()
        {
            return PartialView();
        }
        public PartialViewResult history_out()
        {
            return PartialView();
        }
        public ActionResult history_in_read([DataSourceRequest] DataSourceRequest request)
        {
            wis_read rd = new wis_read();
            // int x = officeid == null ? 0 : officeid;
            var data = rd.history_in();
            return Json(data.ToDataSourceResult(request));
        }
        public ActionResult history_out_read([DataSourceRequest] DataSourceRequest request)
        {
            wis_read rd = new wis_read();
            // int x = officeid == null ? 0 : officeid;
            var data = rd.history_out();
            return Json(data.ToDataSourceResult(request));
        }
        public PartialViewResult dbm_bb()
        {
            return PartialView();
        }
        public PartialViewResult ris()
        {
            return PartialView();
        }
        public ActionResult create_ris([DataSourceRequest] DataSourceRequest request)
        {
            wis_read rd = new wis_read();
            // int x = officeid == null ? 0 : officeid;
            var data = rd.new_ris();
            return Json(data.ToDataSourceResult(request));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Data.Entity;
using Kendo.Mvc.UI;
using System.Web.Services;
using IMS.Models;
using System.Data;
using IMS.@class;
using System.Data.SqlClient;

namespace IMS.Controllers
{
    public class MaintenanceController : BaseController
    {
        private pmisEntities pmis = new pmisEntities();
        private IMSEntities db = new IMSEntities();
        // GET: Maintenance
       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult adc()
        {
            return PartialView("Addcategory");
        }
        public string UpdateI(item it)
        {
                savein r = new savein();
                return r.update(it);      
        }
        public string DestroyI(item it)
        {
                savein r = new savein();
                return r.destroy(it);
        }

        //view for maintenance page
        public ActionResult plcCategory([DataSourceRequest]DataSourceRequest request)
        {
            Maintenance mn = new Maintenance();
            var d = mn.addCategory();
            return Json(d.ToDataSourceResult(request));
        }
        public String deleteunit(int id)
        {
            Maintenance mn = new Maintenance();
            var d = mn.deleteunit(id);
            return(d);
        }
        public String saveunit(string unit)
        {
            Maintenance mn = new Maintenance();
            var d = mn.addunit(unit);
            return (d);
        }
        public ActionResult Addunit()
        { 
             return PartialView("PlcAddUnit");           
        } 
    }
}
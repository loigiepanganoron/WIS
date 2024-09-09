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
using System.Collections;
using System.Data;
using IMS.@class;
using System.Data.SqlClient;
using System.Configuration;
using IMS.Classess;
using System.Web.Script.Serialization;
using System.IO;
using System.Web.Security;
using System.Threading;
using System.Data.Common;
using System.Data.EntityModel;
using System.Data.Mapping;
using System.Data.OleDb;
using IMS.SMS;

namespace IMS.Controllers
{
    public class SampleController : Controller
    {
        // GET: Sample
        public ActionResult Index()
        {
            return View();
        }
        public string GetFeed()
        {
            var x = new
            {
                feed = new[]
        {
            new
            {
                    id = 1,
                name = "Loigie Panganoron",
                image = "http://192.168.2.104/hris/Content/images/photos/7075.png",
                status = "\"Science is a beautiful and emotional human endeavor,\" says Brannon Braga, executive producer and director. \"And Cosmos is all about making science an experience.\"",
                profilePic =  "http://192.168.2.104/hris/Content/images/photos/7075.png",
               timeStamp = "1403375851930",
               url = ""
            },
            new
            {
                    id = 1,
                name = "Loigie Panganoron",
                image = "http://192.168.2.104/hris/Content/images/photos/7075.png",
                status = "\"Science is a beautiful and emotional human endeavor,\" says Brannon Braga, executive producer and director. \"And Cosmos is all about making science an experience.\"",
                profilePic =  "http://192.168.2.104/hris/Content/images/photos/7078.png",
               timeStamp = "1403375851930",
               url = ""
            }
        }
            };
            // return x;
            return new JavaScriptSerializer().Serialize(x);
        }

        public ActionResult jsonfeed([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = (@"select top 50 a.id,b.EmpFullName as name ,c.subtask_description as status, a.timestamp as timeStamp ,'http://192.168.2.104/hris/Content/images/photos/'+cast(a.eid as nvarchar(20) ) +'.png' as profilePic ,
   'http://10.0.0.5/proof/'+CAST(a.eid as nvarchar(20))+'/'+cast(a.id as nvarchar(20))+'.'+attachment_extension as image FROM [spms].[dbo].[spms_tblSubTaskProof]  as a
  inner join  [pmis].[dbo].[m_vwGetAllEmployee] as b on a.eid = b.eid
  inner join [spms].[dbo].[spms_tblSubTask] as c on a.subtask_id = c.id order by id desc").DataSet();

            var serializer = new JavaScriptSerializer();
            var result = new ContentResult();
            serializer.MaxJsonLength = Int32.MaxValue;
            result.Content = serializer.Serialize(dt.ToDataSourceResult(request));
            result.ContentType = "application/json";
            return result;
        }
        public JsonResult GetFeeds()
        {
            var jsonResult = new
            {
                id = 1,
                name = "Loigie Panganoron",
                image = "http://192.168.2.104/hris/Content/images/photos/7075.png",
                status = "\"Science is a beautiful and emotional human endeavor,\" says Brannon Braga, executive producer and director. \"And Cosmos is all about making science an experience.\"",
                profilePic = "http://192.168.2.104/hris/Content/images/photos/7075.png",
                timeStamp = "1403375851930",
                url = ""
            };
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
    }
}
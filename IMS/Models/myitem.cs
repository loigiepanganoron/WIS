﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IMS.Models
{

    public class item
    {

        public double reorder { get; set; }
        public string obr { get; set; }
        public string account_str { get; set; }
        public string dbm_bb_str { get; set; }
        public string mooe_no_str { get; set; }
        public string quarter_str { get; set; }
        public int year { get; set; }
        public int quarter { get; set; }
        public int mooe_no { get; set; }
        public int dbm_bb { get; set; }
        public int allocated { get; set; }
        public int running_balance { get; set; }
        public bool receive { get; set; }
        public bool deleted { get; set; }
        public bool to_be_deleted { get; set; }
        public bool edited { get; set; }
        public bool agree { get; set; }
        public int request_eid { get; set; }
        public int req_eid { get; set; }
        public string controlno { get; set; }
        public int preparation_id { get; set; }
        public double request_quantity { get; set; }
        public bool stat_id { get; set; }
        public double accepted { get; set; }
        public double dremaining { get; set; }
        public int total_quantity { get; set; }
        public double recieve_quantity { get; set; }
        public double remaining { get; set; }
        public string str_borrowed_qty { get; set; }
        public string str_bid { get; set; }
        public int deducted { get; set; }
        public string borrowed_source { get; set; }
        public int borrowed_quantity { get; set; }
        public int bid { get; set; }
        public int toprint { get; set; }
        public decimal totalamount { get; set; }
        public int ris { get; set; }
        public decimal rprice { get; set; }
        public string transcode { get; set; }
        public string itemcode { get; set; }
        public string item_description { get; set; }
        public string price { get; set; }
        public string stock_in { get; set; }
        public string date_approve { get; set;}
        public bool stat { get; set; }
        public string ris_quantity { get; set; }
        public string risid { get; set; }
        public DateTime date_approved { get; set; }
        public DateTime date_submitted { get; set; }
        public int status { get; set; }
        public double iquantity { get; set; }
        public string officename { get; set; }
        public string unitname { get; set; }
        public int id { get; set; }
        public string plceid { get; set; }
        public string reid { get; set; }
        public int eid { get; set; }
        public double totalout { get; set; }
        public int qt { get; set; }
        public int itemid { get; set; }
        public string itemname { get; set; }
        public double total { get; set; }
        public long tt { get; set; }
        public DateTime date { get; set; }
        public string empname {get;set;}
        public double quantity {get;set;}
        public string in_out {get;set;}
        public string descript { get; set; }
        public string unit { get; set; }
        public int officeid { get; set; }
        public string ntotal { get; set; }
        public string eName { get; set; }
        public string  qty { get; set; }
        public double available { get; set; }
        public int released { get; set; }
        public string navailable { get; set; }
        public string nreleased { get; set; }
        public string unitcost { get; set; }
        public int srcid { get; set; }
        public int accountid { get; set; }
        public int tid { get; set; }
        public int obj { get; set; }
        public string quit { get; set; }
        public string average { get; set; }

       
        /// model for reportplc
        /// 

        public string totalin {get;set;}
        public string totalot {get;set;}
        //public  string unit {get;set;}
       // public string unitcost{get;set;}
        public string totalcostout {get;set;}
        public string totalcostin {get;set;}

        public int istire { get; set; }
        public string prnumber { get; set; }

        public string comment { get; set; }
        
      }
}
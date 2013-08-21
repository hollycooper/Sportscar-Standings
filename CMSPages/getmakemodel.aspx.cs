﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net.Mail;
using CMS.EmailEngine;
using CMS.CMSHelper;
using CMS.SettingsProvider;
using System.IO;
using CMS.GlobalHelper;
using CMS.SiteProvider;
using CMS.DataEngine;

public partial class getmakemodel : System.Web.UI.Page
{
   
    string year = "";
    string make = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["Year"] != null)
        {
            year = Request.Params["Year"];
        }
        if (Request.Params["Make"] != null)
        {
            make = Request.Params["Make"];
        }

        get_list();
    }


    private void get_list()
    {
        string sql = "";
        if (year == "" &&  make == "")
        {
            sql = "";
        }
        else
        {
            if (year != "")
            {
                if (year=="All") sql = "";
                else
                sql = " Year ='" + year + "'";
               
            }
            if (sql != "")
            {
                if (make != "")
                {


                    if (make == "All")
                    {
                        if (year != "All" && year != "")
                            sql = sql;
                        else
                        sql += "";
                    }
                    else
                        sql += " and (Make = '" + make + "' or URLslug = '" + make + "')";
                }

            }
            else
                if (make != "")
                {
                    if (make == "All") sql = "";
                    else
                        sql = " Make ='" + make + "' or URLslug = '" + make + "'";
                  
                }





        }


        DataSet ds = new DataSet();
       
       
        string column_name = "";
        if (make == "")
            column_name = "Make,URLslug";
        else column_name = "Model,ItemID";
        if (make == "All") column_name = "Model,ItemID";
        GeneralConnection cn = null;
        if (Session["GeneralConnection"] != null)
        {
            cn = (GeneralConnection)Session["GeneralConnection"];
           
        }
        else
        {
            cn = ConnectionHelper.GetConnection();
          
        }

       
        ds = cn.ExecuteQuery("select " + column_name + " from  dbo.customtable_carz  " + (sql != "" ? " where " + sql : "") + "group by " + column_name, null, QueryTypeEnum.SQLQuery, false);

        if (!DataHelper.DataSourceIsEmpty(ds))
        {
            DataTable myTableApp = ds.Tables[0];
            string[] JaggedArray = new string[myTableApp.Rows.Count];
            int i = 0;
            string value = "";
            string temp = "";
            string temp2 = "";
            foreach (DataRow ItemRow in ds.Tables[0].Rows)
            {

                if (column_name == "Make,URLslug")
                {
                    temp = ItemRow["Make"].ToString().Trim() + (string.IsNullOrEmpty(ItemRow["URLslug"].ToString().Trim()) ? "" : "|" + ItemRow["URLslug"].ToString().Trim());
                }
                if (column_name == "Model,ItemID")
                {
                    temp = ItemRow["ItemID"].ToString().Trim() + "|" + ItemRow["Model"].ToString().Trim();
                }

                if (temp != value)
                {
                    
                        if (column_name == "Make,URLslug" || column_name == "Model,ItemID")
                        {
                            JaggedArray[i] = temp;
                        }
                        else
                            JaggedArray[i] = ItemRow[column_name].ToString().Trim();

                        temp2 = JaggedArray[i];

                    i++;
                }
                value = temp2;
               
            }
           
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(JaggedArray);
            Response.Expires = -1;		//required to keep the page from being cached on the client"s browser
            Response.ContentType = "text/plain";
            Response.Write(sJSON);
            Response.End();
        }
        else
        {
            Response.ContentType = "text/plain";
            Response.Write(" ");
            Response.End();
        }




        cn.Dispose();

    }
  
}

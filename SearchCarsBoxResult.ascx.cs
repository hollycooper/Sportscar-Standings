using System;
using System.Data;
using System.Web;
using System.Web.UI;

using CMS.PortalControls;
using CMS.GlobalHelper;
using CMS.Controls;
using CMS.PortalEngine;
using CMS.CMSHelper;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.SettingsProvider;
using System.Net.Mail;
using System.Configuration;
using CMS.SiteProvider;
using System.Web.UI.WebControls;
using CMS.DocumentEngine;
using CMS.MediaLibrary;




public partial class CMSWebParts_Carz_SearchCarsBoxResult : CMSAbstractWebPart
{


    HttpCookie CookieTempMsg = null;
    HttpCookie CookieFilter = null;
    CustomTableItemProvider tp = new CustomTableItemProvider(CMSContext.CurrentUser);
    DataSet ds_ = null;
    DataTable dtb = null;
    DataSet ds = null;
    string year = "";
    string make = "";
    string model = "";
    string h1 = "";
    string other = "";
    string classname = "";
    string class_ = "";
    string previewimage = "";
    string qryname = "";
    int it = 0;
    string next = "";
    string preview = "";
    string cidn_ = "";
    string cidp_ = "";
    string cid = "";
    string link_detail = "";
    string View = "";
    int item_l_k = 0;
    string filter = "";
    string title = "";
    string sql = "";
    string y2 = "";
    string m2 = "";
    string md2 = "";
    string bd2 = "";
    string r2 = "";
    string y3 = "";
    string m3 = "";
    string md3 = "";
    string bd3 = "";
    string r3 = "";
    float time60 = 0;
    float time601 = 0;
    string default_image = "/App_Themes/carz/images/Photo_Coming_Soon.jpg";
    Boolean bSecond = false;
    GeneralConnection cn = null;
    int n_row = 0;
    string searchtext = "";
    int nparam = 0;
    clsSearch clsC = new clsSearch();
	string debug="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["GeneralConnection"] != null)
        {
            cn = (GeneralConnection)Session["GeneralConnection"];

        }
        else
        {
            cn = ConnectionHelper.GetConnection();

        }

      
       
      
        if (!IsPostBack)
        {
            Session["SortExpression"] = null;
            lodding();
            if (Session["list_result"] != null)
            {

                GridViewResult.DataSource = (DataTable)Session["list_result"];
                GridViewResult.DataBind();

                get_detail();
            }

        }


    }
    
    protected void lodding()
    {


        clsC.init_value(Request.Params["year"], Request.Params["make"], Request.Params["model"], Request.Params["other"], Request.Params["class"], Request.Params["searchtext"]);
        year = Session["year"].ToString();
        make = Session["make"].ToString();
        model = Session["model"].ToString();
        class_ = Session["classname"].ToString();
        searchtext = Session["searchtext"].ToString();
        nparam = Session["nparam"] != null ? int.Parse(Session["nparam"].ToString()) : 0;

        clsC.get_sql(year, make, model, other, searchtext);

        sql = Session["sql"].ToString();
        title = Session["title"].ToString();

        get_query();
        string[] aqn = qryname.Split('.');

        //LiteralTitle.Text = qryname;

        string sql2 = "select QueryName,QueryText from  dbo.CMS_Query where QueryName='{0}'";
        ds_ = cn.ExecuteQuery(string.Format(sql2, aqn[2]), null, QueryTypeEnum.SQLQuery, false);
        if (ds_ != null)
        {
            foreach (DataRow ItemRow in ds_.Tables[0].Rows)
            {


                ds_ = cn.ExecuteQuery(ItemRow["QueryText"].ToString().Replace("##WHERE##", sql), null, QueryTypeEnum.SQLQuery, false);
                Session["123456"] = sql;

                if (ds_ != null)
                {
                    if (ds_.Tables[0].Rows.Count <= 0)
                    {
                        LiteralResultDetail.Text = "<div class='ResultTitle'><p><h1>Sorry, no car found.</h1></p></div>";
                        Session["list_result"] = null;
						 debug+="9";
                        //Uri url = Request.Url;
                        //LiteralResultDetail.Text = url.AbsoluteUri;

                        //LiteralResultDetail.Text = make;
                    }
                    else
                    {
					   debug="1";
                        if (ds_.Tables[0].Rows.Count > 0)
                        {
                            if (ds_.Tables[0].Rows.Count > 0 && nparam < 3)
                            {
 debug+="2";
                                Session["list_result"] = ds_.Tables[0];

                            }
                            else
                                if (Session["list_result"] == null && ds_.Tables[0].Rows.Count == 1)
                                {
                                    Session["list_result"] = ds_.Tables[0]; debug+="3";
                                }
                            n_row = ds_.Tables[0].Rows.Count;
                            Session["list_result_detail"] = ds_.Tables[0].Rows[0];
                            class_ = ds_.Tables[0].Rows[0]["TimeClass"].ToString().Trim();
 debug+="4";

                        }

                        try
                        {
                            dtb = (DataTable)Session["list_result"];
							debug+="-10";
                        }
                        catch
                        {
debug+="-11";
                        }

                        //for (int i = 0; i < ds_.Tables[0].Rows.Count; i++)
                        //{
                            //ds_.Tables[0].Rows[i]["STT"] = ds_.Tables[0].Rows[i]["TimeClassRank"];
                        //}


                        if (dtb.Rows.Count > 0)
                        {
                            if (dtb.Rows.Count > 1)
                            {
 debug+="5";
                                Session["list_result"] = dtb.Select("", "STT ASC").CopyToDataTable();

                            }
							 debug+="6";
                            n_row = ds_.Tables[0].Rows.Count;
                            Session["list_result_detail"] = ds_.Tables[0].Rows[0];
                        }
						 debug+="7";
						
                    }
                }

            }
        }

    }

    private void get_detail()
    {

        dtb = get_data();

        if (dtb != null)
        {
            DataRow dr = (DataRow)Session["list_result_detail"];
            Boolean b = false;
            for (int i = 0; i < dtb.Rows.Count; i++)
            {


                if (dtb.Rows[i]["ItemID"].ToString() == dr["ItemID"].ToString())
                {
                    bSecond = true;
                    dr["STT"] = dtb.Rows[i]["STT"];
                    try
                    {
                        if (i == 0)
                        {
                            //begin
                            if (dtb.Rows.Count > 1)
                            {
                                next = dtb.Rows[1]["Year"].ToString() + "-" + CarzHelpers.URLEncode(dtb.Rows[1]["Make"].ToString()) + "-" + CarzHelpers.URLEncode(dtb.Rows[1]["Model"].ToString(), true)  ;
                                cidn_ = dtb.Rows[1]["ItemID"].ToString();
                            }
                            preview = "";
                        }
                        else
                            if (i == (dtb.Rows.Count - 1))
                            {
                                //the end
                                if (dtb.Rows.Count > 1)
                                {
                                    preview = dtb.Rows[dtb.Rows.Count - 2]["Year"].ToString() + "-" + CarzHelpers.URLEncode(dtb.Rows[dtb.Rows.Count - 2]["Make"].ToString()) + "-" + CarzHelpers.URLEncode(dtb.Rows[dtb.Rows.Count - 2]["Model"].ToString(), true)  ;
                                    cidp_ = dtb.Rows[dtb.Rows.Count - 2]["ItemID"].ToString();
                                }
                            }
                            else
                            {
                                preview = dtb.Rows[i - 1]["Year"].ToString() + "-" + CarzHelpers.URLEncode(dtb.Rows[i - 1]["Make"].ToString()) + "-" + CarzHelpers.URLEncode(dtb.Rows[i - 1]["Model"].ToString(), true)  ;
                                cidp_ = dtb.Rows[i - 1]["ItemID"].ToString();
                                next = dtb.Rows[i + 1]["Year"].ToString() + "-" + CarzHelpers.URLEncode(dtb.Rows[i + 1]["Make"].ToString()) + "-" + CarzHelpers.URLEncode(dtb.Rows[i + 1]["Model"].ToString(), true)  ;
                                cidn_ = dtb.Rows[i + 1]["ItemID"].ToString();
                                //center
                            }
                    }
                    catch
                    {
                    }
                }



                try
                {
                    if (bSecond == true)
                    {
                        bSecond = false;
                        if (dtb.Rows.Count > 1)
                        {
                            if (i == (dtb.Rows.Count - 1))
                            {
                                if (dtb.Rows.Count == 2)
                                {
                                    y2 = dtb.Rows[i - 1]["Year"].ToString();
                                    m2 = dtb.Rows[i - 1]["Make"].ToString();
                                    md2 = dtb.Rows[i - 1]["Model"].ToString();
                                    r2 = dtb.Rows[i - 1]["STT"].ToString();
                                    time60 = Parse_double(dtb.Rows[i]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i - 1]["Best060Time"].ToString());
                                }
                                else
                                {
                                    y2 = dtb.Rows[i - 2]["Year"].ToString();
                                    m2 = dtb.Rows[i - 2]["Make"].ToString();
                                    md2 = dtb.Rows[i - 2]["Model"].ToString();
                                    r2 = dtb.Rows[i - 2]["STT"].ToString();
                                    time60 = Parse_double(dtb.Rows[i]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i - 2]["Best060Time"].ToString());
                                    y3 = dtb.Rows[i - 1]["Year"].ToString();
                                    m3 = dtb.Rows[i - 1]["Make"].ToString();
                                    md3 = dtb.Rows[i - 1]["Model"].ToString();
                                    r3 = dtb.Rows[i - 1]["STT"].ToString();
                                    time601 = Parse_double(dtb.Rows[i]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i - 1]["Best060Time"].ToString());
                                }
                            }
                            else
                            {
                                if (dr["STT"].ToString() == "1")
                                {
                                    if (dtb.Rows.Count == 2)
                                    {
                                        y2 = dtb.Rows[i + 1]["Year"].ToString();
                                        m2 = dtb.Rows[i + 1]["Make"].ToString();
                                        md2 = dtb.Rows[i + 1]["Model"].ToString();
                                        r2 = dtb.Rows[i + 1]["STT"].ToString();
                                        time60 = Parse_double(dtb.Rows[i + 1]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i]["Best060Time"].ToString());
                                    }
                                    else
                                    {
                                        y2 = dtb.Rows[i + 1]["Year"].ToString();
                                        m2 = dtb.Rows[i + 1]["Make"].ToString();
                                        md2 = dtb.Rows[i + 1]["Model"].ToString();
                                        bd2 = dtb.Rows[i + 1]["BodyText"].ToString();
                                        r2 = dtb.Rows[i + 1]["STT"].ToString();
                                        time60 = Parse_double(dtb.Rows[i + 1]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i]["Best060Time"].ToString());
                                        y3 = dtb.Rows[i + 2]["Year"].ToString();
                                        m3 = dtb.Rows[i + 2]["Make"].ToString();
                                        md3 = dtb.Rows[i + 2]["Model"].ToString();
                                        bd3 = dtb.Rows[i + 2]["BodyText"].ToString();
                                        r3 = dtb.Rows[i + 2]["STT"].ToString();
                                        time601 = Parse_double(dtb.Rows[i + 2]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i]["Best060Time"].ToString());
                                    }
                                }
                                else
                                {
                                    y2 = dtb.Rows[i - 1]["Year"].ToString();
                                    m2 = dtb.Rows[i - 1]["Make"].ToString();
                                    md2 = dtb.Rows[i - 1]["Model"].ToString();
                                    bd2 = dtb.Rows[i - 1]["BodyText"].ToString();
                                    r2 = dtb.Rows[i - 1]["STT"].ToString();
                                    time60 = Parse_double(dtb.Rows[i]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i - 1]["Best060Time"].ToString());
                                    try
                                    {
                                        y3 = dtb.Rows[i + 1]["Year"].ToString();
                                        m3 = dtb.Rows[i + 1]["Make"].ToString();
                                        md3 = dtb.Rows[i + 1]["Model"].ToString();
                                        bd3 = dtb.Rows[i + 1]["BodyText"].ToString();
                                        r3 = dtb.Rows[i + 1]["STT"].ToString();
                                        time601 = Parse_double(dtb.Rows[i + 1]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i]["Best060Time"].ToString());
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            Session["classname_search"] = (Request.Params["class"] != null ? Request.Params["class"].ToString() : "");
            if (n_row > 0 && nparam < 3)
            {
                string path = "";
                if (Request.Params["searchtext"] == null)
                {                                
                    path = (year != "" ? year : "") + (make != "" ? (year != "" ? "-" + CarzHelpers.URLEncode(make) : CarzHelpers.URLEncode(make)) : "") + (model != "" ? (year != "" ? "-" + CarzHelpers.URLEncode(model, true) : (make != "" ? "-" + CarzHelpers.URLEncode(model, true) : CarzHelpers.URLEncode(model, true))) : "");
                    if (Session["classname_search"].ToString() == "all") path = "all";
                    Session["path_result"] = "/Top-10-Fastest-Cars/" + path;

                    Session["classname_search"] = path;
                }
                else
                {
                    Session["classname_search"] = (Request.Params["class"] != null ? Request.Params["class"].ToString() : "");
                    Session["path_result"] = "/Search/Custom-Search/searchtext=" + Request.Params["searchtext"].ToString();
                    //Session["classname_search"] = CarzHelpers.URLDecode(Request.Params["searchtext"].ToString());
                }


            }


            getimage_class(Session["classname_search"].ToString());

//--------*** This sets up the winner's circle text on the detail page--------

            class_ = Session["classname_search"].ToString();
            string custompath = "/Top-10-Fastest-Cars/" + class_;
            if (CarzHelpers.URLDecode(class_, true) == "Custom-Search") custompath = (Session["path_result"] != null ? Session["path_result"].ToString() : "");
            cid = dr["ItemID"].ToString();
            View = (dr["Views"].ToString() == "" ? "0" : dr["Views"].ToString());
            link_detail = "/Top-10-Fastest-Cars" + (class_ != null ? "/" + class_ : "") + "/" + dr["Year"].ToString() + "-" + CarzHelpers.URLEncode(dr["Make"].ToString()) + "-" + CarzHelpers.URLEncode(dr["Model"].ToString(), true) ;
            string default_text = "The <a href='{7}' title='{0} {1} {2}{3}'>{0} {1} {2}{3}</a> ranks #{4} in the <a href='{9}' title='{5}'>{5}</a> class and races 0-60 mph in {8} seconds.";

            string path1 = "/Top-10-Fastest-Cars/" + class_ + "/" + dr["Year"].ToString() + "-" + CarzHelpers.URLEncode(dr["Make"].ToString()) + "-" + CarzHelpers.URLEncode(dr["Model"].ToString(), true);
            int rank = int.Parse(dr["STT"].ToString());
            default_text = string.Format(default_text, dr["Year"].ToString(), dr["Make"].ToString(), dr["Model"].ToString(), dr["BodyText"].ToString() == "" ? "" : " " + dr["BodyText"].ToString(), rank.ToString(), CarzHelpers.URLDecode(class_, true).Replace("-", " "), class_, path1, dr["Best060Time"].ToString(), custompath);
            string path2 = "/Top-10-Fastest-Cars/" + class_ + "/" + y2 + "-" + CarzHelpers.URLEncode(m2) + "-" + CarzHelpers.URLEncode(md2, true);
            string path3 = "/Top-10-Fastest-Cars/" + class_ + "/" + y3 + "-" + CarzHelpers.URLEncode(m3) + "-" + CarzHelpers.URLEncode(md3, true);
            string winner1 = "It pulls ahead of the #2 ranked <a href='{5}' title='{1} {2} {3}{4}'>{1} {2} {3}{4}</a> by {6}";
            if (y3 != "") winner1 += " and the #3 ranked <a href='{11}' title='{7} {8} {9}{10}'>{7} {8} {9}{10}</a> by {12}.";
            else winner1 += ".";
            if (y2 == "") winner1 = "";
            winner1 = string.Format(winner1, "", y2, m2, md2, bd2 == "" ? "" : " " + bd2, path2, (time60 <= 0.099999 ? "just hundredths of a second" : time60.ToString("0.0") + " seconds"), y3, m3, md3, bd3, path3, (compare(time601) ? " just hundredths of a second" : time601.ToString("0.0") + " seconds"));


            string winner2 = "It falls behind the #{6} ranked <a href='{5}' title='{1} {2} {3}{4}'>{1} {2} {3}{4}</a> by {7}";
            if (y3 != "") winner2 += " and pulls ahead of the #{8} ranked <a href='{13}' title='{9} {10} {11}{12}'>{9} {10} {11}{12}</a> by {14}.";
            else winner2 += ".";
            winner2 = string.Format(winner2, "", y2, m2, md2, bd2 == "" ? "" : " " + bd2, path2, r2, (time60 <= 0.099999 ? "just hundredths of a second" : time60.ToString("0.0") + " seconds"), r3, y3, m3, md3, bd3, path3, (compare(time601) ? " just hundredths of a second" : time601.ToString("0.0") + " seconds"));


            string winner3 = "It falls behind the #{6} ranked <a href='{5}' title='{1} {2} {3}{4}'>{1} {2} {3}{4}</a> by {7}";
            if (y3 != "") winner3 += " and the #{8} ranked <a href='{13}' title='{9} {10} {11}{12}'>{9} {10} {11}{12}</a> by {14}.";
            else winner3 += ".";

            winner3 = string.Format(winner3, "", y2, m2, md2, bd2 == "" ? "" : " " + bd2, path2, r2, (time60 <= 0.099999 ? "just hundredths of a second" : time60.ToString("0.0") + " seconds"), r3, y3, m3, md3, bd3, path3, (compare(time601) ? " just hundredths of a second" : time601.ToString("0.0") + " seconds"));
            string link_media = CarzHelpers.getMediaURL(dr["Image"].ToString());

            previewimage = (link_media != "" ? link_media : default_image);
            if (previewimage != "") previewimage = string.Format("<div class='ResultImage'><img   border='0'  alt='' src='{0}'/></div>", previewimage);
            string view_de = " {0}<div class='ResultTitle'><p>0482</p><p>Rank: #{1}</p><p>Class:  <a href='{2}' title='{3}'>{3}</a></p><p>0-60 Time: {4} seconds</p></div><div class='clear'></div>";

            view_de = string.Format(view_de, previewimage, rank, custompath, CarzHelpers.URLDecode(class_, true).Replace("-", " "), Parse_double(dr["Best060Time"].ToString()).ToString("0.0"));

            string html_content = "";

            //html_content = view_de + "<div class='ResultContent'><h2>Winner�s Circle</h2><p>" + default_text + "</p></div>";

            if (rank == 1)
                html_content = view_de + "<div class='ResultContent'><h2 style='0122'>Winner�s Circle</h2><p>" + default_text + "</p><p>" + winner1 + "</p></div>";
            else
                if (rank > 1 && rank < (dtb.Rows.Count))
                    html_content = view_de + "<div class='ResultContent'><h2 style='0122'>Winner�s Circle</h2><p>" + default_text + "</p><p>" + winner2 + "</p></div>";
                else
                    html_content = view_de + "<div class='ResultContent'><h2 style='0122'>Winner�s Circle</h2><p>" + default_text + "</p><p>" + winner3 + "</p></div>";


            LiteralResultDetail.Text = html_content;



            h1 = string.Format("{0} {1} {2}", dr["Year"].ToString(), dr["Make"].ToString(), dr["Model"].ToString());

            if (n_row == 1 && nparam > 2)
            {

                string html = "<div class='Car_Details list clearfix'><div class='Vehicle_information' > <h2>Vehicle information</h2><div class='conten'>{0}</div></div><div class='performance_statistics'><h2>Performance statistics</h2><div class='conten'>{1}</div></div></div><p style='clear:both;'>" + View + " people have viewed this car.</p> ";
                LiteralResultDetail2.Text = string.Format(html, "<p>Year: " + dr["Year"].ToString() +
                    "</p>" + "<p>Make: " + dr["Make"].ToString() + "</p>" +
                    "<p>Model: " + dr["Model"].ToString() + "</p>" +
                    "<p>Engine: " + dr["Engine"].ToString() + "</p>" +
                    "<p>Horsepower: " + Parse_double(dr["Horsepower"].ToString()).ToString("0") + "</p>" +
                    "<p>Torque: " + dr["Torque"].ToString() + "</p>" +
                    "<p>Transmission: " + dr["Transmission"].ToString() + "</p>" +                   
                    "<p>Curb Weight: " + dr["CurbWeight"].ToString() + "</p>" +
                    "<p>City MPG: " + dr["MPGCity"].ToString() + "</p>" +
                    "<p>Highway MPG: " + dr["MPG"].ToString() + "</p>" +
                    "<p>MSRP: $" + Parse_double(dr["MSRP"].ToString()).ToString("0,0") + "</p>",
                    "<p>Tags: " + dr["Tags"].ToString() + "</p>" +
      "<p>Class: <a href='/Popular-Lineups/" + dr["TimeClass"].ToString() + "' title='" + dr["TimeClass"].ToString() + "' >" + dr["TimeClass"].ToString().Replace("-", " ") + "</a></p>" + "<p>Rank: #" + dr["TimeClassRank"].ToString() + "</p>" +
      "<p>Best 0-60 Time: " + Parse_double(dr["Best060Time"].ToString()).ToString("0.0") + " seconds </p>" +
     "<p>Best &#188;  Mile: " + Parse_double(dr["Best1of4MileTime"].ToString()).ToString("0.0") + " seconds </p>" +
     "<p>Best  &#188;  Mile speed: " + Parse_double(dr["Best1of4MileSpeed"].ToString()).ToString("0.0") + " mph </p>" +
     "<p>Best Top Speed: " + Parse_double(dr["BestTopSpeed"].ToString()).ToString("0.0") + " mph </p>"
                    );



                try
                {
                    /*
                    CustomTableItemProvider tbProvide = new CustomTableItemProvider();
                
                    CustomTableItem tbRecord = tbProvide.GetItem(int.Parse(cid), "customtable.carz");
                    if (tbRecord != null)
                    {
                        // Set new field values
                        tbRecord.SetValue("Views", int.Parse(View) + 1);

                        // Save updates in the database
                        tbRecord.Update();
                       

                    }
                    
                    */

                    int k = cn.ExecuteNonQuery(string.Format("UPDATE [dbo].[customtable_carz] SET Views='{0}' WHERE ItemID='{1}' ", int.Parse(View) + 1, cid), null, QueryTypeEnum.SQLQuery, false);
                    
                }
                catch
                {
                    
                }

                GridViewResult.Visible = false;
                LiteralNone.Text = " style='display:none;' ";
                LiteralResultDetail2.Visible = true;
                LiteralTitle.Text = "<h1>" + h1 + "</h1>";

//--------*** This sets up the individual car profile text on the detail page--------


                string back = "<a title='Back to Results' href='" + (Session["path_result"] != null ? Session["path_result"].ToString() : "javascript:history.back();") + "'>Back to Results</a> | ";
                LiteralResultDetail.Text = LiteralResultDetail.Text.Replace("0122", "display:none;");
                LiteralResultDetail.Text = LiteralResultDetail.Text.Replace("0482", (preview == "" && next == "" ? "" : back) + (preview != "" ? "<  <a title='Prev' href='/Top-10-Fastest-Cars/" + class_ + "/" + preview + "'>Prev</a>&nbsp;&nbsp;&nbsp;" : "") + (next != "" ? " <a title='Next' href='/Top-10-Fastest-Cars/" + class_ + "/" + next + "'>Next > </a> " : ""));

               

            }
            else
            {


                LiteralResultDetail.Text = LiteralResultDetail.Text.Replace("0482", string.Format("<a href='{0}' title='{1}'>{1}</a>", path1, h1));

            }
            //Uri url = Request.Url;
            //LiteralResultDetail.Text = Session["123456"].ToString();
			//LiteralResultDetail.Text+="<br>"+Session["123456"].ToString()+"<br>"+debug + "<br>n_row =" + n_row + "<br>nparam=="+nparam;
        }

    }
    protected float min_speed(object t1, object t2, object t3, object t4)
    {
        float v1 = Parse_double(t1.ToString());
        float v2 = Parse_double(t2.ToString());
        float v3 = Parse_double(t3.ToString());
        float v4 = Parse_double(t4.ToString());
        float min = v1;
        if (v2 != 0 && v2 < min) min = v2;
        if (v3 != 0 && v3 < min) min = v3;
        if (v4 != 0 && v4 < min) min = v4;
        return min;

    }
    protected DataTable get_data()
    {
        try
        {

            return (DataTable)Session["list_result"];
        }
        catch
        {

        }
        return null;
    }
    protected void get_query()
    {
        DataClassInfo dci = DataClassInfoProvider.GetDataClass("customtable.carz");
        if (dci != null)
        {

            qryname = dci.ClassName + ".search_result";

        }
    }
    private void getimage_class(string cls)
    {
        TreeProvider tree = new TreeProvider();
        ds = tree.SelectNodes(CMSContext.CurrentSiteName, "/Classes/%", CMSContext.CurrentDocumentCulture.CultureCode, true, "CMS.MenuItem", "DocumentMenuItemHideInNavigation='False' and ExpressClasses='false'", "NodeOrder ASC", -1, true);
        if (!DataHelper.DataSourceIsEmpty(ds))
        {


            foreach (DataRow ItemRow in ds.Tables[0].Rows)
            {

                if (ItemRow["MenuItemName"].ToString() == class_.Replace("-Cars", "").Replace("-", " "))
                {

                    //html result detail

                    if (searchtext != "")
                        LiteralTitle.Text = string.Format("<h1>{0}</h1>", "Search results for " + (classname != "" ? CarzHelpers.URLDecode(classname.Replace("-", " ")) + " " + title : CarzHelpers.URLDecode(cls.Replace("-", " "))));
                    else
                        LiteralTitle.Text = string.Format("<h1>{0}</h1>", "Top 10 Fastest " + (classname != "" ? CarzHelpers.URLDecode(classname.Replace("-", " ")) + " " + title : CarzHelpers.URLDecode(cls.Replace("-", " "))));
                    //classname = ItemRow["MenuItemName"].ToString();


                    previewimage = ItemRow["MenuItemTeaserImage"].ToString();
                    classname = ItemRow["MenuItemName"].ToString();


                }
                // LiteralResultDetail.Text =dr["TimeClass"].ToString().Replace("-"," ");
            }
        }


        if (searchtext != "") LiteralTitle.Text = string.Format("<h1>{0}</h1>", "Search results for " + searchtext + " " + title);
        if (searchtext == "") LiteralTitle.Text = string.Format("<h1>{0}</h1>", "Top 10 Fastest Cars from " + searchtext + " " + title);
        
    }
    protected float Parse_double(string value)
    {
        try
        {
            return float.Parse(value);
        }
        catch
        {
            return 0;
        }
    }
    protected bool compare(float v)
    {
        return (int.Parse(((v - 0.1) * 100).ToString("0")) < 0);
    }
    protected void GridViewResult_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {

    }
    protected void GridViewResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        string filter = "";
        filter = string.Format(" Make like '%{0}%'  or Model like '%{0}%'  or Year like '%{0}%' ", (TextBoxFilter.Text.Trim() == "Keyword filter") ? "" : TextBoxFilter.Text.Trim());

        //Retrieve the table from the session object.
        dtb = get_data();

        if (dtb != null)
        {

            GridViewResult.PageIndex = e.NewPageIndex;
            DataTable dt = dtb.Select(filter).CopyToDataTable();
            if (Session["SortExpression"] != null)
            {
                string sort=Session["SortExpression"].ToString();
                //if (sort.LastIndexOf("ASC") > -1) sort = sort.Replace("ASC", "DESC");
                //else if (sort.LastIndexOf("DESC") > -1) sort = sort.Replace("DESC", "ASC");
                dt.DefaultView.Sort = sort;
                
                
            }
            
            GridViewResult.DataSource = dt;

            GridViewResult.DataBind();
        }

    }
    private string GetSortDirection(string column)
    {

        // By default, set the sort direction to ascending.
        string sortDirection = "ASC";

        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                
                string lastDirection = ViewState["SortDirection"] as string;
               
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }else
                    if ((lastDirection != null) && (lastDirection == "DESC"))
                    {
                        sortDirection = "ASC";
                    }

            }
        }

        // Save new values in ViewState.
        ViewState["SortDirection"] = sortDirection;
        ViewState["SortExpression"] = column;

        return sortDirection;
    }

    protected void GridViewResult_Sorting(object sender, GridViewSortEventArgs e)
    {
        string filter = "";
        filter = string.Format(" Make like '%{0}%'  or Model like '%{0}%'  or Year like '%{0}%' ", (TextBoxFilter.Text.Trim() == "Keyword filter") ? "" : TextBoxFilter.Text.Trim());

        //Retrieve the table from the session object.
        dtb = get_data();

        if (dtb != null)
        {
            DataTable dt = dtb.Select(filter).CopyToDataTable();
            Session["SortExpression"] = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            dt.DefaultView.Sort = Session["SortExpression"].ToString();
          
            GridViewResult.DataSource = dt;
            GridViewResult.DataBind();
        }



    }
    protected void GridViewResult_RowCreated(object sender, GridViewRowEventArgs e)
    {

        // Use the RowType property to determine whether the 
        // row being created is the header row. 

        if (e.Row.RowType == DataControlRowType.Header)
        {
            Control hlink = e.Row.Cells[0].Controls[0];
            // Create the sorting image based on the sort direction.
            string sort = "<img alt='{1}' src='{0}'/>";

            sort = string.Format(sort, "~/App_Themes/carz/images/bg.png", "Order");


            // Add the image to the appropriate header cell.
            e.Row.Cells[0].Controls[0].Controls.Add(new LiteralControl("Rank<span style='margin-left: 5px;' >" + sort + "</span>"));
            e.Row.Cells[1].Controls[0].Controls.Add(new LiteralControl("Year<span style='margin-left: 5px;' >" + sort + "</span>"));
            e.Row.Cells[2].Controls[0].Controls.Add(new LiteralControl("Make<span style='margin-left: 5px;' >" + sort + "</span>"));
            e.Row.Cells[3].Controls[0].Controls.Add(new LiteralControl("Model<span style='margin-left: 5px;' >" + sort + "</span>"));
            e.Row.Cells[4].Controls[0].Controls.Add(new LiteralControl("Horsepower<span style='margin-left: 5px;' >" + sort + "</span>"));
            e.Row.Cells[5].Controls[0].Controls.Add(new LiteralControl("MSRP<span style='margin-left: 5px;' >" + sort + "</span>"));
            e.Row.Cells[6].Controls[0].Controls.Add(new LiteralControl("0-60 Time<span style='margin-left: 5px;' >" + sort + "</span>"));
        }

        if (e.Row.RowType == DataControlRowType.Pager)
        {


            Table pagerTable = e.Row.Cells[0].Controls[0] as Table;
            pagerTable.Style["float"] = "right";
            pagerTable.Style["display"] = "inline";
            pagerTable.Style["margin-right"] = "15px";

            dtb = (DataTable)GridViewResult.DataSource;
            int sumcount = dtb != null ? dtb.Rows.Count : 0;


            LiteralControl lt = new LiteralControl("<span style='float:left;font-weight: bold;width: auto;margin-left: 10px;'>Displaying results " + (GridViewResult.PageIndex * 20 + 1) + "-" + ((GridViewResult.PageIndex * 20 + 20) > sumcount ? sumcount.ToString() : (GridViewResult.PageIndex * 20 + 20).ToString()) + " (of " + sumcount.ToString() + ")</span>");

            e.Row.Cells[0].Controls.Add(lt);

        }
    }
    // This is a helper method used to determine the index of the
    // column being sorted. If no column is being sorted, -1 is returned.
    int GetSortColumnIndex()
    {

        // Iterate through the Columns collection to determine the index
        // of the column being sorted.
        foreach (DataControlField field in GridViewResult.Columns)
        {
            if (field.SortExpression == GridViewResult.SortExpression)
            {
                return GridViewResult.Columns.IndexOf(field);
            }
        }

        return -1;
    }
    // This is a helper method used to add a sort direction
    // image to the header of the column being sorted.
    void AddSortImage(int columnIndex, GridViewRow headerRow)
    {

        // Create the sorting image based on the sort direction.
        Image sortImage = new Image();
        if (GridViewResult.SortDirection == SortDirection.Ascending)
        {
            sortImage.ImageUrl = "~/App_Themes/carz/images/bg.png";
            sortImage.AlternateText = "Ascending Order";
        }
        else
        {
            sortImage.ImageUrl = "~/App_Themes/carz/images/bg.png";
            sortImage.AlternateText = "Descending Order";
        }

        // Add the image to the appropriate header cell.
        headerRow.Cells[columnIndex].Controls.Add(sortImage);

    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        string filter = "";
        filter = string.Format(" Make like '%{0}%'  or Model like '%{0}%'  or Year like '%{0}%' ", (TextBoxFilter.Text.Trim() == "Keyword filter") ? "" : TextBoxFilter.Text.Trim());

        //Retrieve the table from the session object.
        dtb = get_data();

        if (dtb != null)
        {
            try
            {
                GridViewResult.PageIndex = 0;
                GridViewResult.DataSource = dtb.Select(filter).CopyToDataTable();
                
            }
            catch
            {
                GridViewResult.DataSource = null;
            }
           
        }
        else
        {
            GridViewResult.DataSource = null;
           
        }
        GridViewResult.DataBind();
        

    }
    protected void GridViewResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        string y = "";
        string m = "";
        string md = "";
        string rank = "";
        string path_STT = "";
        string classname = (Request.Params["class"] != null ? Request.Params["class"].ToString().Replace(".aspx", "") + "/" : "");
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

//--------*** This sets up the table rows on the results page--------

            rank = e.Row.Cells[0].Text.Trim();
            y = e.Row.Cells[1].Text.Trim();
            m = e.Row.Cells[2].Text.Trim();
            md = e.Row.Cells[3].Text.Trim();
            //class_ = e.Row.Cells[7].Text.Trim();
            path_STT = "<a href='/Top-10-Fastest-Cars/" + classname + CarzHelpers.URLEncode(y) + "-" + CarzHelpers.URLEncode(m) + "-" + CarzHelpers.URLEncode(md, true) + "' title='" + rank + "'>" + rank + "</a>";
            e.Row.Cells[0].Text = path_STT;

            path_STT = "<a href='/Top-10-Fastest-Cars/" + CarzHelpers.URLEncode(y) + "' title='" + e.Row.Cells[1].Text + "'>" + e.Row.Cells[1].Text + "</a>";
            e.Row.Cells[1].Text = path_STT;

            path_STT = "<a href='/Top-10-Fastest-Cars/" + CarzHelpers.URLEncode(m) + "' title='" + e.Row.Cells[2].Text + "'>" + e.Row.Cells[2].Text + "</a>";
            e.Row.Cells[2].Text = path_STT;

            path_STT = "<a href='/Top-10-Fastest-Cars/" + classname + CarzHelpers.URLEncode(y) + "-" + CarzHelpers.URLEncode(m) + "-" + CarzHelpers.URLEncode(md, true) + "' title='" + e.Row.Cells[3].Text + "'>" + e.Row.Cells[3].Text + "</a>";
            e.Row.Cells[3].Text = path_STT;


            //LiteralTitle.Text+= e.Row.Cells[0].Text;



        }
    }
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        clsC.init_value(Request.Params["year"], Request.Params["make"], Request.Params["model"], Request.Params["other"], Request.Params["class"], Request.Params["searchtext"]);
        year = Session["year"].ToString();
        make = Session["make"].ToString();
        model = Session["model"].ToString();
        string name = (year != "" ? year : "") + (make != "" ? (year != "" ? " " + CarzHelpers.URLDecode(make) : CarzHelpers.URLDecode(make)) : "") + (model != "" ? (year != "" ? " " + CarzHelpers.URLDecode(model, true).Replace("_", "-") : (make != "" ? " " + CarzHelpers.URLDecode(model, true).Replace("_", "-") : CarzHelpers.URLDecode(model, true).Replace("_", "-"))) : "");
        if (Request.Params["searchtext"] == null)
        {


            CMSContext.CurrentTitle = "Top 10 Fastest Cars from " + (name.Replace(".aspx", "") == "" ? "All" : name.Replace(".aspx", "").Replace(" All", ""));
        }
        else
        {
            CMSContext.CurrentTitle = "Search Results - " + CarzHelpers.URLDecode(Request.Params["searchtext"].ToString().Replace(".aspx", ""));

        }

    }

}


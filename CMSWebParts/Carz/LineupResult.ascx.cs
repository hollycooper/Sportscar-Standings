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
using System.Collections;
using CMS.EventLog;
using CMS.Scheduler;
using CMS.MediaLibrary;



public partial class CMSWebParts_Carz_LineupResult : CMSAbstractWebPart
{
    HttpCookie CookieTempMsg = null;
    HttpCookie CookieFilter = null;
    CustomTableItemProvider tp = null;
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
    string id2 = "";
    string id3 = "";
    string ModelURL2 = "";
    string ModelURL3 = "";
    string searchtext = "";

    int page = 0;
    float time60 = 0;
    float time601 = 0;
    string default_image = "/App_Themes/carz/images/Photo_Coming_Soon.jpg";
    Boolean bSecond = false;
    bool ExpressClasses = false;
    GeneralConnection cn = null;
    int n_row = 0;
    clsSearch clsC = new clsSearch();
    protected void Page_Load(object sender, EventArgs e)
    {
        string urlslash = HttpContext.Current.Request.RawUrl.ToString();
        if (urlslash.Substring(urlslash.Length - 1, 1) == "/")
            Response.Redirect(urlslash.Substring(0, urlslash.Length - 1));


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
            Session["filter_keyword"] = null;
            Session["path"] = null;
            if (Request.Params["page"] != null)
            {
                int.TryParse(Request.Params["page"].ToString(), out page);
                if (page == 1)
                {

                    if (Request.Params["searchtext"] != null)
                    {
                        Response.Redirect(Request.RawUrl.Substring(0, (Request.RawUrl.IndexOf("?"))) + "/searchtext=" + Request.Params["searchtext"].ToString().Replace(".aspx", "").Replace("?", ""));
                    }
                    else
                    {
                        if (HttpContext.Current.Request.RawUrl.ToString().IndexOf("?") != -1)
                        {
                            Session["Next_Url"] = Request.RawUrl.Substring(0, (Request.RawUrl.IndexOf("?")));
                            Response.Redirect(Request.RawUrl.Substring(0, (Request.RawUrl.IndexOf("?"))));
                        }
                    }
                }
                else
                {
                    GridViewResult.PageIndex = page - 1;
                }
            }
            else
            {
                if (Session["Next_Url"] != null)
                {
                    string curent_url = "";
                    if (HttpContext.Current.Request.RawUrl.ToString().IndexOf("?") != -1)
                    {
                        curent_url = Request.RawUrl.Substring(0, (Request.RawUrl.IndexOf("?")));
                    }
                    else
                    {
                        curent_url = HttpContext.Current.Request.RawUrl.ToString();
                    }

                    if (Session["Next_Url"].ToString() != curent_url.Replace(".aspx", ""))
                    {
                        Session["SortExpression"] = null;
                    }

                    Session["Next_Url"] = null;
                }
                else
                {
                    Session["SortExpression"] = null;
                }
            }


            if (HttpContext.Current.Request.RawUrl.ToString() == "/Popular-Lineups")
            {
                Session["filter_key"] = null;
                Session["filter_path"] = null;
            }

            if (Session["filter_path"] != null)
            {
                string new_url = "";
                if (HttpContext.Current.Request.RawUrl.ToString().IndexOf("?") != -1)
                {
                    new_url = Request.RawUrl.Substring(0, (Request.RawUrl.IndexOf("?")));
                }
                else
                {
                    new_url = HttpContext.Current.Request.RawUrl.ToString();
                }

                if (Session["filter_path"].ToString().Replace(".aspx", "") != new_url.Replace(".aspx", ""))
                {
                    Session["filter_key"] = null;
                    Session["filter_path"] = null;
                    TextBoxFilter.Text = "Keyword filter";
                    Session["filter_path"] = new_url;
                    Session["SortExpression"] = null;
                }
            }

            lodding();

            if (Session["list_result"] != null)
            {
                GridViewResult.DataSource = (DataTable)Session["list_result"];
                GridViewResult.DataBind();
                get_detail();

                if (Session["filter_key"] != null)
                {
                    TextBoxFilter.Text = Session["filter_key"].ToString();
                    filter = string.Format(" Make like '%{0}%'  or Model like '%{0}%'  or Year like '%{0}%' ", (TextBoxFilter.Text.Trim() == "Keyword filter") ? "" : TextBoxFilter.Text.Trim());
                    dtb = get_data();

                    if (dtb != null)
                    {
                        GridViewResult.DataSource = dtb.Select(filter).CopyToDataTable();
                        GridViewResult.DataBind();
                    }
                }


                if (Session["SortExpression"] != null)
                {
                    filter = string.Format(" Make like '%{0}%'  or Model like '%{0}%'  or Year like '%{0}%' ", (TextBoxFilter.Text.Trim() == "Keyword filter") ? "" : TextBoxFilter.Text.Trim());
                    //Retrieve the table from the session object.
                    dtb = get_data();
                    DataTable dtsort = dtb.Select(filter).CopyToDataTable();
                    dtsort.DefaultView.Sort = Session["SortExpression"].ToString();
                    GridViewResult.DataSource = dtsort;
                    GridViewResult.DataBind();
                }
            }
        }


    }
    protected void RedirectNotFound()
    {

        Response.Clear();
        Response.Status = "301 Moved Permanently";
        Response.AddHeader("Location", "/Not-Found");
        Response.End();

        //Response.Redirect("/Not-Found");
        //Response.StatusCode = 301;
        //Response.End();
        //HttpContext.Current.Server.Transfer("/404.aspx");

    }
    protected void lodding()
    {


        /*Literaldebug.Text += "<br>"+(Request.Params["year"] != null ? "year=" + Request.Params["year"].ToString() + "-" : "") +
                    (Request.Params["other"] != null ? "other=" + Request.Params["other"].ToString() + "-" : "") +
                    (Request.Params["class"] != null ? "class=" + Request.Params["class"].ToString() + "-" : "") +
                    (Request.Params["searchtext"] != null ? "searchtext=" + Request.Params["searchtext"].ToString() + "-" : "");
            */
        //clsC.init_value(Request.Params["year"], Request.Params["make"], Request.Params["model"], Request.Params["other"], Request.Params["class"], Request.Params["searchtext"]);

        clsC.init_value2(Request.Params["year"], Request.Params["other"], Request.Params["class"], Request.Params["searchtext"], "");

        year = Session["year"].ToString();
        make = Session["make"].ToString();
        model = Session["model"].ToString();
        class_ = Session["classname"].ToString().Replace("/", "");
        int nparam = Session["nparam"] != null ? int.Parse(Session["nparam"].ToString()) : 0;
        clsC.get_sql(year, make, model, other, "");

        sql = Session["sql"].ToString();
        //Response.Write(sql);
        title = Session["title"].ToString();

        get_query();
        string[] aqn = qryname.Split('.');
        //Literaldebug.Text += "<br>" + sql;
        //LiteralTitle.Text = qryname;

        if (Request.Params["class"] != null && Session["atclass"] == null)
        {
            Session["atclass"] = Request.Params["class"].ToString();
        }



        string replace_go_detail = "";
        string sql2 = "select QueryName,QueryText from  dbo.CMS_Query where QueryName='{0}'";
        ds_ = cn.ExecuteQuery(string.Format(sql2, aqn[2]), null, QueryTypeEnum.SQLQuery, false);
        //Literaldebug.Text += "<br>" + string.Format(sql2, aqn[2]);
        if (ds_ != null)
        {
            foreach (DataRow ItemRow in ds_.Tables[0].Rows)
            {
                if (sql.Contains("Year"))
                {
                    //replace_go_detail = "Year ='2013'";
                    searchtext = Request.Params["year"].ToString();
                }
                if (sql.Contains("MakeURL"))
                {
                    //replace_go_detail = "Make = 'Chevrolet' or MakeURL = 'Chevrolet'";
                    searchtext = (Request.Params["other"] != null ? Request.Params["other"].ToString() : "");
                }

                if (Request.Params["other"] != null && sql.Contains("ItemID") == false && sql.Contains("Year") == false && sql.Contains("Make") == false) RedirectNotFound();
                //if (sql.Contains("Make") == false)
                ds_ = cn.ExecuteQuery(ItemRow["QueryText"].ToString().Replace("##WHERE##", "(" + sql + ")"), null, QueryTypeEnum.SQLQuery, false);
                //Literaldebug.Text += "<br>" + ItemRow["QueryText"].ToString().Replace("##WHERE##", sql);

                if (ds_ != null)
                {
                    if (ds_.Tables[0].Rows.Count <= 0)
                    {
                        LiteralResultDetail.Text = "<div class='ResultTitle'><p><h1>No car found</h1></p></div>";
                        Session["list_result"] = null;
                        //LiteralResultDetail.Text = Session["123456"].ToString();
                        RedirectNotFound();
                    }
                    else
                    {
                        if (ds_.Tables[0].Rows.Count > 0)
                        {
                            if (ds_.Tables[0].Rows.Count > 1)
                            {

                                Session["list_result"] = ds_.Tables[0];

                            }
                            else
                                if (Session["list_result"] == null && ds_.Tables[0].Rows.Count == 1)
                                {
                                    DataSet ds_temp = null;
                                    if (Request.Params["searchtext"] != null)
                                    {
                                        ds_temp = cn.ExecuteQuery(ItemRow["QueryText"].ToString().Replace("##WHERE##", string.Format("(Year ='{0}' or Make = '{0}' or MakeURL = '{0}')", Request.Params["searchtext"].ToString())), null, QueryTypeEnum.SQLQuery, false);

                                    }
                                    else
                                        ds_temp = cn.ExecuteQuery(ItemRow["QueryText"].ToString().Replace("##WHERE##", "1=1"), null, QueryTypeEnum.SQLQuery, false);

                                    if (ds_temp != null) Session["list_result"] = ds_temp.Tables[0];
                                    else Session["list_result"] = ds_.Tables[0];
                                    //endfix


                                }
                                else
                                {
                                    if (Session["list_result"] != null && ds_.Tables[0].Rows.Count == 1)
                                    {
                                        //debug += "wswswswswsw";
                                        string tempquery = "";
                                        if (Request.Params["searchtext"] != null)
                                        {
                                            tempquery = string.Format("(Year ='{0}' or Make = '{0}' or MakeURL = '{0}')", Request.Params["searchtext"].ToString());
                                        }
                                        else
                                            if (Request.Params["class"] != null)
                                            {

                                                if (Session["atclass"] != null)
                                                {
                                                    if (Session["atclass"].ToString() != Request.Params["class"].ToString()) tempquery = "1=1";

                                                }
                                            }
                                        if (tempquery != "")
                                        {
                                            DataSet ds_temp = cn.ExecuteQuery(ItemRow["QueryText"].ToString().Replace("##WHERE##", tempquery), null, QueryTypeEnum.SQLQuery, false);
                                            Session["list_result"] = ds_temp.Tables[0];
                                            //debug += "vu2";
                                            //endfix
                                        }

                                    }
                                }
                            n_row = ds_.Tables[0].Rows.Count;
                            Session["list_result_detail"] = ds_.Tables[0].Rows[0];

                        }

                        try
                        {
                            dtb = (DataTable)Session["list_result"];
                        }
                        catch
                        {

                        }
                        if (!ExpressClasses)
                        {
                            if (nparam != 1)
                                for (int i = 0; i < ds_.Tables[0].Rows.Count; i++)
                                {
                                    if (dtb != null)
                                    {
                                        if (dtb.Rows.Count > 1)
                                            ds_.Tables[0].Rows[i]["STT"] = ds_.Tables[0].Rows[i]["TimeClassRank"];
                                    }
                                }
                        }

                        if (ds_.Tables[0].Rows.Count > 0)
                        {
                            if (ds_.Tables[0].Rows.Count > 1)
                            {

                                Session["list_result"] = ds_.Tables[0].Select("", "STT ASC").CopyToDataTable();

                            }
                            n_row = ds_.Tables[0].Rows.Count;
                            Session["list_result_detail"] = ds_.Tables[0].Rows[0];
                        }
                    }
                    if (Request.Params["class"] != null)
                    {
                        Session["atclass"] = Request.Params["class"].ToString();
                    }
                }
                else
                {
                    LiteralResultDetail.Text = "<div class='ResultTitle'><p><h1>No car found</h1></p></div>";
                    Session["list_result"] = null;
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
                                cidn_ = dtb.Rows[1]["ItemID"].ToString();
                                next = dtb.Rows[1]["Year"].ToString() + "-" + CarzHelpers.URLEncode(dtb.Rows[1]["Make"].ToString()) + "-" +
                            (dtb.Rows[1]["ModelURL"].ToString() != "" ? dtb.Rows[1]["ModelURL"].ToString() : CarzHelpers.URLEncode(dtb.Rows[1]["Model"].ToString(), true)) + "-" + cidn_ + (Request.Params["searchtext"] != null ? "?searchtext=" + Request.Params["searchtext"].ToString() : "");

                            }
                            preview = "";
                        }
                        else
                            if (i == (dtb.Rows.Count - 1))
                            {
                                //the end
                                if (dtb.Rows.Count > 1)
                                {
                                    cidp_ = dtb.Rows[dtb.Rows.Count - 2]["ItemID"].ToString();
                                    preview = dtb.Rows[dtb.Rows.Count - 2]["Year"].ToString() + "-" + CarzHelpers.URLEncode(dtb.Rows[dtb.Rows.Count - 2]["Make"].ToString()) + "-" +
                                   (dtb.Rows[dtb.Rows.Count - 2]["ModelURL"].ToString() != "" ? dtb.Rows[dtb.Rows.Count - 2]["ModelURL"].ToString() : CarzHelpers.URLEncode(dtb.Rows[dtb.Rows.Count - 2]["Model"].ToString(), true)) + "-" + cidp_ + (Request.Params["searchtext"] != null ? "?searchtext=" + Request.Params["searchtext"].ToString() : "");

                                }
                            }
                            else
                            {
                                cidp_ = dtb.Rows[i - 1]["ItemID"].ToString();
                                preview = dtb.Rows[i - 1]["Year"].ToString() + "-" + CarzHelpers.URLEncode(dtb.Rows[i - 1]["Make"].ToString()) + "-" +
                                                                                (dtb.Rows[i - 1]["ModelURL"].ToString() != "" ? dtb.Rows[i - 1]["ModelURL"].ToString() : CarzHelpers.URLEncode(dtb.Rows[i - 1]["Model"].ToString(), true)) + "-" + cidp_ + (Request.Params["searchtext"] != null ? "?searchtext=" + Request.Params["searchtext"].ToString() : "");
                                cidn_ = dtb.Rows[i + 1]["ItemID"].ToString();
                                next = dtb.Rows[i + 1]["Year"].ToString() + "-" + CarzHelpers.URLEncode(dtb.Rows[i + 1]["Make"].ToString()) + "-" +
                               (dtb.Rows[i + 1]["ModelURL"].ToString() != "" ? dtb.Rows[i + 1]["ModelURL"].ToString() : CarzHelpers.URLEncode(dtb.Rows[i + 1]["Model"].ToString(), true)) + "-" + cidn_ + (Request.Params["searchtext"] != null ? "?searchtext=" + Request.Params["searchtext"].ToString() : "");

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
                                    id2 = dtb.Rows[i - 1]["ItemID"].ToString();
                                    y2 = dtb.Rows[i - 1]["Year"].ToString();
                                    m2 = (string.IsNullOrEmpty(dtb.Rows[i - 1]["URLslug"].ToString()) == true ? dtb.Rows[i - 1]["Make"].ToString() : dtb.Rows[i - 1]["URLslug"].ToString());
                                    md2 = dtb.Rows[i - 1]["Model"].ToString();
                                    ModelURL2 = dtb.Rows[i - 1]["ModelURL"].ToString();
                                    r2 = dtb.Rows[i - 1]["STT"].ToString();
                                    time60 = Parse_double(dtb.Rows[i]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i - 1]["Best060Time"].ToString());
                                }
                                else
                                {
                                    id2 = dtb.Rows[i - 2]["ItemID"].ToString();
                                    y2 = dtb.Rows[i - 2]["Year"].ToString();
                                    m2 = (string.IsNullOrEmpty(dtb.Rows[i - 2]["URLslug"].ToString()) == true ? dtb.Rows[i - 2]["Make"].ToString() : dtb.Rows[i - 2]["URLslug"].ToString());
                                    md2 = dtb.Rows[i - 2]["Model"].ToString();
                                    ModelURL2 = dtb.Rows[i - 2]["ModelURL"].ToString();
                                    r2 = dtb.Rows[i - 2]["STT"].ToString();
                                    time60 = Parse_double(dtb.Rows[i]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i - 2]["Best060Time"].ToString());
                                    id3 = dtb.Rows[i - 1]["ItemID"].ToString();
                                    y3 = dtb.Rows[i - 1]["Year"].ToString();
                                    m3 = (string.IsNullOrEmpty(dtb.Rows[i - 1]["URLslug"].ToString()) == true ? dtb.Rows[i - 1]["Make"].ToString() : dtb.Rows[i - 1]["URLslug"].ToString());
                                    md3 = dtb.Rows[i - 1]["Model"].ToString();
                                    ModelURL3 = dtb.Rows[i - 1]["ModelURL"].ToString();
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
                                        id2 = dtb.Rows[i + 1]["ItemID"].ToString();
                                        y2 = dtb.Rows[i + 1]["Year"].ToString();
                                        m2 = (string.IsNullOrEmpty(dtb.Rows[i + 1]["URLslug"].ToString()) == true ? dtb.Rows[i + 1]["Make"].ToString() : dtb.Rows[i + 1]["URLslug"].ToString());
                                        md2 = dtb.Rows[i + 1]["Model"].ToString();
                                        ModelURL2 = dtb.Rows[i + 1]["ModelURL"].ToString();
                                        r2 = dtb.Rows[i + 1]["STT"].ToString();
                                        time60 = Parse_double(dtb.Rows[i + 1]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i]["Best060Time"].ToString());
                                    }
                                    else
                                    {
                                        id2 = dtb.Rows[i + 1]["ItemID"].ToString();
                                        y2 = dtb.Rows[i + 1]["Year"].ToString();
                                        m2 = (string.IsNullOrEmpty(dtb.Rows[i + 1]["URLslug"].ToString()) == true ? dtb.Rows[i + 1]["Make"].ToString() : dtb.Rows[i + 1]["URLslug"].ToString());
                                        md2 = dtb.Rows[i + 1]["Model"].ToString();
                                        ModelURL2 = dtb.Rows[i + 1]["ModelURL"].ToString();
                                        bd2 = dtb.Rows[i + 1]["BodyText"].ToString();
                                        r2 = dtb.Rows[i + 1]["STT"].ToString();
                                        time60 = Parse_double(dtb.Rows[i + 1]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i]["Best060Time"].ToString());
                                        id3 = dtb.Rows[i + 2]["ItemID"].ToString();
                                        y3 = dtb.Rows[i + 2]["Year"].ToString();
                                        m3 = (string.IsNullOrEmpty(dtb.Rows[i + 2]["URLslug"].ToString()) == true ? dtb.Rows[i + 2]["Make"].ToString() : dtb.Rows[i + 2]["URLslug"].ToString());
                                        md3 = dtb.Rows[i + 2]["Model"].ToString();
                                        ModelURL3 = dtb.Rows[i + 2]["ModelURL"].ToString();
                                        bd3 = dtb.Rows[i + 2]["BodyText"].ToString();
                                        r3 = dtb.Rows[i + 2]["STT"].ToString();
                                        time601 = Parse_double(dtb.Rows[i + 2]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i]["Best060Time"].ToString());
                                    }
                                }
                                else
                                {
                                    id2 = dtb.Rows[i - 1]["ItemID"].ToString();
                                    y2 = dtb.Rows[i - 1]["Year"].ToString();
                                    m2 = (string.IsNullOrEmpty(dtb.Rows[i - 1]["URLslug"].ToString()) == true ? dtb.Rows[i - 1]["Make"].ToString() : dtb.Rows[i - 1]["URLslug"].ToString());
                                    md2 = dtb.Rows[i - 1]["Model"].ToString();
                                    ModelURL2 = dtb.Rows[i - 1]["ModelURL"].ToString();
                                    bd2 = dtb.Rows[i - 1]["BodyText"].ToString();
                                    r2 = dtb.Rows[i - 1]["STT"].ToString();
                                    time60 = Parse_double(dtb.Rows[i]["Best060Time"].ToString()) - Parse_double(dtb.Rows[i - 1]["Best060Time"].ToString());
                                    try
                                    {
                                        id3 = dtb.Rows[i + 1]["ItemID"].ToString();
                                        y3 = dtb.Rows[i + 1]["Year"].ToString();
                                        m3 = (string.IsNullOrEmpty(dtb.Rows[i + 1]["URLslug"].ToString()) == true ? dtb.Rows[i + 1]["Make"].ToString() : dtb.Rows[i + 1]["URLslug"].ToString());
                                        md3 = dtb.Rows[i + 1]["Model"].ToString();
                                        ModelURL3 = dtb.Rows[i + 1]["ModelURL"].ToString();
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






            cid = dr["ItemID"].ToString();
            View = (dr["Views"].ToString() == "" ? "0" : dr["Views"].ToString());
            string make_encode = CarzHelpers.URLEncode(string.IsNullOrEmpty(dr["URLslug"].ToString()) == true ? dr["Make"].ToString() : dr["URLslug"].ToString());

            link_detail = "/Popular-Lineups" + (Request.Params["class"] != null ? "/" + Request.Params["class"].ToString() : "") + "/" + dr["Year"].ToString() + "-" + make_encode + "-" + CarzHelpers.URLEncode(dr["Model"].ToString(), true);




            //string default_text = "The <a href='{7}' title='{0} {1} {2}{3}'>{0} {1} {2}{3}</a> ranks #{4} in the <a href='/Popular-Lineups/{6}' title='{5}'>{5}</a> class and races 0-60 mph in {8} seconds.";
			string default_text = "The top 10 fastest cars are lined up from fastest to slowest by their 0 to 60 times. We bring you this list of <a href='/Popular-Lineups/{6}' title='{5}'>{5}</a> based on the best estimates from trusted sources, including Car & Driver, Motor Trend, Road & Track and more.</p><p>Ranked at <strong>#{4}</strong>, the <a href='{7}' title='{0} {1} {2}{3}'>{0} {1} {2}{3}</a> races 0-60 mph in {8} seconds. Sports cars that fall within this range of acceleration are categorized as <a href='/Popular-Lineups/" + dr["TimeClass"].ToString() + "' title='" + dr["TimeClass"].ToString() + "' >" + dr["TimeClass"].ToString().Replace("-", " ") + "</a>.</p><p> ";
			if(searchtext=="")
             searchtext=(Request.Params["searchtext"] != null ? Request.Params["searchtext"].ToString() : "");
            string path1 = "/Popular-Lineups/" + class_ + "/" + dr["Year"].ToString() + "-" + make_encode + "-" + (string.IsNullOrEmpty(dr["ModelURL"].ToString()) == false ? dr["ModelURL"].ToString() : CarzHelpers.URLEncode(dr["Model"].ToString(), true)) + "-" + dr["ItemID"].ToString() + (searchtext != "" ? "?searchtext=" + searchtext : "");
            int rank = int.Parse(dr["STT"].ToString());
            default_text = string.Format(default_text,
            dr["Year"].ToString(),
            dr["Make"].ToString(),
            dr["Model"].ToString(),
            dr["BodyText"].ToString() == "" ? "" : " " + dr["BodyText"].ToString(),
            rank.ToString(),
            class_.Replace("-", " "),
            class_,
            path1,
            dr["Best060Time"].ToString());

            //string path2 = "/Popular-Lineups/" + class_ + "/" + y2 + "-" + CarzHelpers.URLEncode(m2) + "-" + CarzHelpers.URLEncode(md2, true) + "-" + id2;
            //string path3 = "/Popular-Lineups/" + class_ + "/" + y3 + "-" + CarzHelpers.URLEncode(m3) + "-" + CarzHelpers.URLEncode(md3, true) + "-" + id3;
            string path2 = "/Popular-Lineups/" + class_ + "/" + y2 + "-" + CarzHelpers.URLEncode(m2) + "-" + (ModelURL2 != "" ? ModelURL2 : CarzHelpers.URLEncode(md2, true)) + "-" + id2 + (searchtext != "" ? "?searchtext=" + searchtext : "");
            string path3 = "/Popular-Lineups/" + class_ + "/" + y3 + "-" + CarzHelpers.URLEncode(m3) + "-" + (ModelURL3 != "" ? ModelURL3 : CarzHelpers.URLEncode(md3, true)) + "-" + id3 + (searchtext != "" ? "?searchtext=" + searchtext : "");

            string winner1 = "It pulls ahead of the #2 ranked <a href='{5}' title='{1} {2} {3}{4}'>{1} {2} {3}{4}</a> by {6}";
            if (y3 != "") winner1 += " and the #3 ranked <a href='{11}' title='{7} {8} {9}{10}'>{7} {8} {9}{10}</a> by {12}.";
            else winner1 += ".";
            if (y2 == "") winner1 = "";

            winner1 = string.Format(winner1, "", y2, m2, md2, bd2 == "" ? "" : " " + bd2, path2, (compare(time60) ? "just hundredths of a second" : time60.ToString("0.0") + " seconds"), y3, m3, md3, bd3, path3, (compare(time601) ? " just hundredths of a second" : time601.ToString("0.0") + " seconds"));
            //--------*** add new text winner1 --------
            string winner1B = "<a href='{5}' title='{1} {2} {3}{4}'>{1} {2} {3}{4}</a>, you would <strong>WIN</strong> by {6}!";
            winner1B = string.Format(winner1B, "", y2, m2, md2, bd2 == "" ? "" : " " + bd2, path2, (time60 <= 0.099999 ? "just hundredths of a second" : time60.ToString("0.0") + " seconds"), y3, m3, md3, bd3, path3, (compare(time601) ? " just hundredths of a second" : time601.ToString("0.0") + " seconds"));



            string winner2 = "It falls behind the #{6} ranked <a href='{5}' title='{1} {2} {3}{4}'>{1} {2} {3}{4}</a> by {7}";
            if (y3 != "") winner2 += " and pulls ahead of the #{8} ranked <a href='{13}' title='{9} {10} {11}{12}'>{9} {10} {11}{12}</a> by {14}.";
            else winner2 += ".";
            winner2 = string.Format(winner2, "", y2, m2, md2, bd2 == "" ? "" : " " + bd2, path2, r2, (compare(time60) ? "just hundredths of a second" : time60.ToString("0.0") + " seconds"), r3, y3, m3, md3, bd3, path3, (compare(time601) ? " just hundredths of a second" : time601.ToString("0.0") + " seconds"));
            //--------*** add new text winner2 --------
            string winner2B = "<a href='{13}' title='{9} {10} {11}{12}'>{9} {10} {11}{12}</a>, you would <strong>WIN</strong> by {14}!";
            winner2B = string.Format(winner2B, "", y2, m2, md2, bd2 == "" ? "" : " " + bd2, path2, r2, (time60 <= 0.099999 ? "just hundredths of a second" : time60.ToString("0.0") + " seconds"), r3, y3, m3, md3, bd3, path3, (compare(time601) ? " just hundredths of a second" : time601.ToString("0.0") + " seconds"));


            string winner3 = "It falls behind the #{6} ranked <a href='{5}' title='{1} {2} {3}{4}'>{1} {2} {3}{4}</a> by {7}";
            if (y3 != "") winner3 += " and the #{8} ranked <a href='{13}' title='{9} {10} {11}{12}'>{9} {10} {11}{12}</a> by {14}.";
            else winner3 += ".";
            winner3 = string.Format(winner3, "", y2, m2, md2, bd2 == "" ? "" : " " + bd2, path2, r2, (compare(time60) ? "just hundredths of a second" : time60.ToString("0.0") + " seconds"), r3, y3, m3, md3, bd3, path3, (compare(time601) ? " just hundredths of a second" : time601.ToString("0.0") + " seconds"));
            //--------*** add new text winner3 --------
            string winner3B = "<a href='{13}' title='{9} {10} {11}{12}'>{9} {10} {11}{12}</a>, you would <strong>LOSE</strong> by {14}!";
            winner3B = string.Format(winner3B, "", y2, m2, md2, bd2 == "" ? "" : " " + bd2, path2, r2, (time60 <= 0.099999 ? "just hundredths of a second" : time60.ToString("0.0") + " seconds"), r3, y3, m3, md3, bd3, path3, (compare(time601) ? " just hundredths of a second" : time601.ToString("0.0") + " seconds"));

            string link_media = CarzHelpers.getMediaURL(dr["Image"].ToString());

            previewimage = (link_media != "" ? link_media : default_image);

            if (previewimage != "") previewimage = string.Format("<div class='ResultImage'><img   border='0'  alt='' src='{0}'/></div>", previewimage);
            string view_de = " {0}<div class='ResultTitle'><p>vav0482</p><p>Rank: #{1}</p><p>Class:  <a href='{2}' title='{3}'>{3}</a></p><p>0-60 Time: {4} seconds</p></div><div class='clear'></div>";
            view_de = string.Format(view_de, previewimage, rank, "/Popular-Lineups/" + class_, class_.Replace("-", " "), Parse_double(dr["Best060Time"].ToString()).ToString("0.0"));

            //--------*** Section 3 --------
            string default_textB = "Before getting in the driver's seat of one of these cars and punching the gas, you should know where you stand. In a race from 0-60 mph between <a href='{7}' title='{0} {1} {2}{3}'>{0} {1} {2}{3}</a> vs. ";
			
            string pathB = "/Popular-Lineups/" + class_ + "/" + dr["Year"].ToString() + "-" + (string.IsNullOrEmpty(dr["URLslug"].ToString()) == false ? dr["URLslug"].ToString() : CarzHelpers.URLEncode(dr["Make"].ToString(), true)) + "-" + (string.IsNullOrEmpty(dr["ModelURL"].ToString()) == false ? dr["ModelURL"].ToString() : CarzHelpers.URLEncode(dr["Model"].ToString(), true)) + "-" + dr["ItemID"].ToString() + (searchtext != "" ? "?searchtext=" + searchtext : "");
            int rankB = int.Parse(dr["STT"].ToString());
            default_textB = string.Format(default_textB, dr["Year"].ToString(), dr["Make"].ToString(), dr["Model"].ToString(), dr["BodyText"].ToString() == "" ? "" : " " + dr["BodyText"].ToString(), rank.ToString(), class_.Replace("-", " "), class_, pathB, dr["Best060Time"].ToString());
            //--------*** Section 4 --------
            string default_textC = "If you are thinking about going head to head with another <a href='/Popular-Lineups/" + class_ + "/{1}' title='{1}'>{1}</a>, make sure you know your competition. Find out where your car ranks against the <a href='/Popular-Lineups/" + class_ + "/{0}' title='{0}'>top 10 fastest cars from {0}</a>.";
            string pathC = pathB;//"/Popular-Lineups/" + class_ + "/" + dr["Year"].ToString() + "-" + CarzHelpers.URLEncode(dr["Make"].ToString()) + "-" + CarzHelpers.URLEncode(dr["Model"].ToString(), true);
            int rankC = int.Parse(dr["STT"].ToString());
            default_textC = string.Format(default_textC, dr["Year"].ToString(), dr["Make"].ToString(), dr["Model"].ToString(), dr["BodyText"].ToString() == "" ? "" : " " + dr["BodyText"].ToString(), rank.ToString(), class_.Replace("-", " "), class_, pathC, dr["Best060Time"].ToString());


            //--------*** Output HTML --------
            string html_content = "";
            if (rank == 1)
                html_content = view_de + "<div class='ResultContent'><h2 style='0122'>Winner's Circle</h2><p>" + default_text + winner1 + "</p><p>" + default_textB + winner1B + "<p>" + default_textC + "</p></div>";
            else
                if (rank > 1 && rank < (dtb.Rows.Count))
                    html_content = view_de + "<div class='ResultContent'><h2 style='0122'>Winner?s Circle</h2><p>" + default_text + winner2 + "</p><p>" + default_textB + winner2B + "<p>" + default_textC + "</p></div>";
                else
                    html_content = view_de + "<div class='ResultContent'><h2 style='0122'>Winner?s Circle</h2><p>" + default_text + winner3 + "</p><p>" + default_textB + winner3B + "<p>" + default_textC + "</p></div>";







            h1 = string.Format("{0} {1} {2}", dr["Year"].ToString(), dr["Make"].ToString(), dr["Model"].ToString());
            LiteralResultDetail.Text = html_content;
            if (n_row == 1)
            {
                string html = "<div style='margin-bottom:10px;' class='addthis_toolbox addthis_default_style'><a class='addthis_button_preferred_1'></a><a class='addthis_button_preferred_2'></a><a class='addthis_button_preferred_3'></a><a class='addthis_button_preferred_4'></a><a class='addthis_button_compact'></a><a class='addthis_counter addthis_bubble_style'></a></div><div class='Car_Details list clearfix'><div class='Vehicle_information' > <h2>Vehicle information</h2><div class='conten'>{0}</div></div><div class='performance_statistics'><h2>Performance statistics</h2><div class='conten'>{1}</div></div></div><p style='clear:both;'>" + View + " people have viewed this car.</p> ";
                LiteralResultDetail2.Text = string.Format(html, "<p><strong>Year:</strong> " + dr["Year"].ToString() + "</p>" +
				"<p><strong>Make:</strong> " + dr["Make"].ToString() + "</p>" +
				"<p><strong>Model:</strong> " + dr["Model"].ToString() + "</p>" +
				"<p><strong>Engine:</strong> " + dr["Engine"].ToString() + "</p>" +
				"<p><strong>Horsepower:</strong> " + Parse_double(dr["Horsepower"].ToString()).ToString("0") + "</p>" +
				"<p><strong>Torque:</strong> " + dr["Torque"].ToString() + "</p>" +
				"<p><strong>Transmission:</strong> " + dr["Transmission"].ToString() + "</p>" +
				"<p><strong>Curb Weight:</strong> " + dr["CurbWeight"].ToString() + "</p>" +
				"<p><strong>City MPG:</strong> " + dr["MPGCity"].ToString() + "</p>" +
				"<p><strong>Highway MPG:</strong> " + dr["MPG"].ToString() + "</p>" +
				"<p><strong>MSRP:</strong> $" + Parse_double(dr["MSRP"].ToString()).ToString("0,0") + "</p>",
				"<p><strong>Tags:</strong> " + dr["Tags"].ToString() + "</p>" +
				"<p><strong>Class:</strong> <a href='/Popular-Lineups/" + dr["TimeClass"].ToString() + "' title='" + dr["TimeClass"].ToString() + "' >" + dr["TimeClass"].ToString().Replace("-", " ") + "</a></p>" +
				"<p><strong>Rank:</strong> #" + dr["TimeClassRank"].ToString() + "</p>" +
				"<p><strong>Best 0-60 Time:</strong> " + Parse_double(dr["Best060Time"].ToString()).ToString("0.0") + " seconds </p>" +
				"<p><strong>Best &#188; Mile:</strong> " + Parse_double(dr["Best1of4MileTime"].ToString()).ToString("0.0") + " seconds </p>" +
				"<p><strong>@ speed:</strong> " + Parse_double(dr["Best1of4MileSpeed"].ToString()).ToString("0.0") + " mph </p>" +
				"<p><strong>Best Top Speed:</strong> " + Parse_double(dr["BestTopSpeed"].ToString()).ToString("0.0") + " mph </p>" +
				"<p><strong>Best Skid Pad:</strong> " + Parse_double(dr["BestSkidPad"].ToString()).ToString("0.0") + " G </p>"
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


                    }*/
                    int k = cn.ExecuteNonQuery(string.Format("UPDATE [dbo].[customtable_carz] SET Views='{0}' WHERE ItemID='{1}' ", int.Parse(View) + 1, cid), null, QueryTypeEnum.SQLQuery, false);
                }
                catch
                {

                }

                GridViewResult.Visible = false;
                LiteralNone.Text = " style='display:none;' ";
                LiteralResultDetail2.Visible = true;
                LiteralTitle.Text = "<h1>" + h1 + "</h1>";

                LiteralResultDetail.Text = LiteralResultDetail.Text.Replace("0122", "display:none;");
                string back = "<a title='Back to Results' href='" + "/Popular-Lineups" + (Request.Params["class"] != null ? "/" + Request.Params["class"].ToString() + (Request.Params["searchtext"] != null ? "/" + Request.Params["searchtext"].ToString() : "") : "") + "'>Back to Results</a> | ";
                LiteralResultDetail.Text = LiteralResultDetail.Text.Replace("vav0482", (preview != "" || next != "" ? back : "") + (preview != "" ? "<  <a title='Prev' href='/Popular-Lineups" + (Request.Params["class"] != null ? "/" + Request.Params["class"].ToString() : "") + "/" + preview + "'>Prev</a>&nbsp;&nbsp;&nbsp;" : "") + (next != "" ? " <a title='Next' href='/Popular-Lineups" + (Request.Params["class"] != null ? "/" + Request.Params["class"].ToString() : "") + "/" + next + "'>Next > </a> " : ""));
            }
            else
            {
                LiteralResultDetail.Text = LiteralResultDetail.Text.Replace("vav0482", string.Format("<a href='{0}' title='{1}'>{1}</a>", path1, h1));

            }


        }


    }
    public static string getMediaURL(string Imagepath)
    {
        string link_media = "";
        string[] arr = null;
        if (Imagepath.Trim() != "")
        {


            if (Imagepath.LastIndexOf("media") > 2 || Imagepath.LastIndexOf("images") > -1)
            {
                link_media = Imagepath.Trim().Split('?')[0];
                arr = link_media.Split('/');
                link_media = arr[arr.Length - 1];
                MediaFileInfo mfi = MediaFileInfoProvider.GetMediaFileInfo(CMSContext.CurrentSiteName, link_media);
                if (mfi != null)
                {

                    return link_media = "/getmedia/" + mfi.FileGUID.ToString() + "/" + mfi.FileName + ".aspx";
                }
            }


        }
        return Imagepath;
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

        TreeProvider tree = new TreeProvider();
        ds = tree.SelectNodes(CMSContext.CurrentSiteName, CMSContext.CurrentAliasPath + "%", CMSContext.CurrentDocumentCulture.CultureCode, true, "CMS.MenuItem", "DocumentMenuItemHideInNavigation='False' ", "NodeOrder ASC", -1, true);
        if (!DataHelper.DataSourceIsEmpty(ds))
        {

            foreach (DataRow ItemRow in ds.Tables[0].Rows)
            {
                if (ItemRow["QueryName"].ToString() != "")
                {

                    qryname = ItemRow["QueryName"].ToString();

                    classname = ItemRow["MenuItemName"].ToString();
                    previewimage = ItemRow["MenuItemTeaserImage"].ToString();

                    string html = "<h1>{0}</h1>";


                    LiteralTitle.Text = string.Format(html, "Standings for " + classname + " " + title);

                    if (ItemRow["ExpressClasses"].ToString() == "1" || ItemRow["ExpressClasses"].ToString() == "True")
                    {
                        ExpressClasses = true;
                        LiteralActiveMenu.Text = "jQuery('#menuElem').find('li').each(function(){ if(jQuery(this).find('a').attr('href')=='/popular-lineups'){jQuery(this).addClass('HighLighted'); } })";
                        try
                        {

                            int k = cn.ExecuteNonQuery(string.Format("UPDATE [dbo].[CONTENT_MenuItem] SET VisitorView='{0}' WHERE MenuItemID='{1}' ", (int.Parse(ItemRow["VisitorView"].ToString()) + 1).ToString(), ItemRow["MenuItemID"].ToString()), null, QueryTypeEnum.SQLQuery, false);


                        }
                        catch (Exception e)
                        {
                            LiteralTesst.Text = e.ToString();
                        }
                    }

                }
            }
        }
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
        int index = e.NewPageIndex + 1;
        string url = "";
        Response.Clear();

        if (!string.IsNullOrEmpty(Request.QueryString["class"]))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["other"]))
            {
                url = "http://" + HttpContext.Current.Request.Url.Host.ToLower() + "/Popular-Lineups/" + Request.QueryString["class"].ToString().Replace(".aspx", "").Replace("?", "") + "/" + Request.QueryString["other"].ToString().Replace(".aspx", "").Replace("?", "");
                Response.Redirect(string.Format("{0}?page={1}", url, index));
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["year"]))
            {
                url = "http://" + HttpContext.Current.Request.Url.Host.ToLower() + "/Popular-Lineups/" + Request.QueryString["class"].ToString().Replace(".aspx", "").Replace("?", "") + "/" + Request.QueryString["year"].ToString().Replace(".aspx", "").Replace("?", "");
                Response.Redirect(string.Format("{0}?page={1}", url, index));
            }
            else
            {
                url = "http://" + HttpContext.Current.Request.Url.Host.ToLower() + "/Popular-Lineups/" + Request.QueryString["class"].ToString().Replace(".aspx", "").Replace("?", "");
                Response.Redirect(string.Format("{0}?page={1}", url, index));
            }
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
            //TableCell cell = new TableCell();
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
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        Session["SortExpression"] = null;
        string filter = "";
        filter = string.Format(" Make like '%{0}%'  or Model like '%{0}%'  or Year like '%{0}%' ", (TextBoxFilter.Text.Trim() == "Keyword filter") ? "" : TextBoxFilter.Text.Trim());

        if (!string.IsNullOrEmpty(TextBoxFilter.Text))
        {
            Session["filter_key"] = null;
            Session["filter_key"] = TextBoxFilter.Text.Trim();
            Session["filter_path"] = null;

            if (HttpContext.Current.Request.RawUrl.ToString().IndexOf("?") != -1)
            {
                Session["filter_path"] = Request.RawUrl.Substring(0, (Request.RawUrl.IndexOf("?")));
            }
            else
            {
                Session["filter_path"] = HttpContext.Current.Request.RawUrl.ToString();
            }
        }
        else
        {
            Session["filter_key"] = null;
            Session["filter_key"] = TextBoxFilter.Text;
        }

        //Retrieve the table from the session object.
        dtb = get_data();


        if (dtb != null)
        {
            try
            {
                GridViewResult.PageIndex = 0;
                GridViewResult.DataSource = dtb.Select(filter).CopyToDataTable();
                GridViewResult.DataBind();

                if (!string.IsNullOrEmpty(Request.QueryString["class"]))
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["other"]))
                    {
                        string url = "http://" + HttpContext.Current.Request.Url.Host.ToLower() + "/Popular-Lineups/" + Request.QueryString["class"].ToString().Replace(".aspx", "").Replace("?", "") + "/" + Request.QueryString["other"].ToString().Replace(".aspx", "").Replace("?", "");
                        Response.Redirect(string.Format("{0}", url));
                    }
                    else if (!string.IsNullOrEmpty(Request.QueryString["year"]))
                    {
                        string url = "http://" + HttpContext.Current.Request.Url.Host.ToLower() + "/Popular-Lineups/" + Request.QueryString["class"].ToString().Replace(".aspx", "").Replace("?", "") + "/" + Request.QueryString["year"].ToString().Replace(".aspx", "").Replace("?", "");
                        Response.Redirect(string.Format("{0}", url));
                    }
                    else
                    {
                        string url = "http://" + HttpContext.Current.Request.Url.Host.ToLower() + "/Popular-Lineups/" + Request.QueryString["class"].ToString().Replace(".aspx", "").Replace("?", "");
                        Response.Redirect(string.Format("{0}", url));
                    }
                }
            }
            catch
            {
                GridViewResult.DataSource = null;
                GridViewResult.DataBind();
            }

        }
        else
        {
            GridViewResult.DataSource = null;
            GridViewResult.DataBind();
        }
    }
    protected void GridViewResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        string y = "";
        string m = "";
        string md = "";
        string rank = "";
        string path_STT = "";

        class_ = (Session["classname"] != null ? Session["classname"].ToString().Replace(".aspx", "") : "");


        //DataRow row = ((DataRowView)e.Row.DataItem);
        object URLslug = (object)DataBinder.Eval(e.Row.DataItem, "URLslug");
        object ItemID = (object)DataBinder.Eval(e.Row.DataItem, "ItemID");
        object ModelURL = (object)DataBinder.Eval(e.Row.DataItem, "ModelURL");

        if (e.Row.RowType == DataControlRowType.DataRow)
        {



            rank = e.Row.Cells[0].Text.Trim();
            y = e.Row.Cells[1].Text.Trim();
            m = e.Row.Cells[2].Text.Trim();
            md = e.Row.Cells[3].Text.Trim();
            if (string.IsNullOrEmpty(ItemID.ToString())) ItemID = "";
            if (string.IsNullOrEmpty(URLslug.ToString())) URLslug = CarzHelpers.URLEncode(m);
            if (string.IsNullOrEmpty(ModelURL.ToString())) ModelURL = CarzHelpers.URLEncode(md, true);



            path_STT = "<a href='/Popular-Lineups/" + class_ + "/" + CarzHelpers.URLEncode(y) + "-" + URLslug + "-" + ModelURL + "-" + ItemID + (searchtext != "" ? "?searchtext=" + searchtext : "") + "' title='" + rank + "'>" + rank + "</a>";
            e.Row.Cells[0].Text = path_STT;


            path_STT = "<a href='/Popular-Lineups/" + class_ + "/" + CarzHelpers.URLEncode(y) + "' title='" + e.Row.Cells[1].Text + "'>" + e.Row.Cells[1].Text + "</a>";
            e.Row.Cells[1].Text = path_STT;

            //string URLslug="";
            //DataRow[] dr = dtb.Select("Make ='"+m+"'");  

            path_STT = "<a href='/Popular-Lineups/" + class_ + "/" + URLslug + "' title='" + e.Row.Cells[2].Text + "'>" + e.Row.Cells[2].Text + "</a>";
            e.Row.Cells[2].Text = path_STT;

            path_STT = "<a href='/Popular-Lineups/" + class_ + "/" + CarzHelpers.URLEncode(y) + "-" + URLslug + "-" + ModelURL + "-" + ItemID + (searchtext != "" ? "?searchtext=" + searchtext : "") + "' title='" + e.Row.Cells[3].Text + "'>" + e.Row.Cells[3].Text + "</a>";
            e.Row.Cells[3].Text = path_STT;


            //LiteralTitle.Text+= e.Row.Cells[0].Text;



        }

    }
    void GridViewMainddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        dtb = get_data();

        if (dtb != null)
        {
            //changes page size
            if ((((DropDownList)sender).SelectedValue).ToString() == "All")
            {
                GridViewResult.PageSize = dtb.Rows.Count;
            }
            else
            {
                GridViewResult.PageSize = int.Parse(((DropDownList)sender).SelectedValue);
            }

            //binds data source
            GridViewResult.DataSource = dtb;
            //GridViewMain.PageIndex = 0;
            GridViewResult.DataBind();
            GridViewResult.AllowPaging = true;
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
        name = name.Trim();
        if (Request.Params["class"] != null)
        {
            CMSContext.CurrentTitle = CarzHelpers.URLDecode(Request.Params["class"].ToString()).Replace("-", " ") + (name == "" ? "" : ": " + name.Replace(".aspx", "").Replace("-", " ").Trim());
        }
    }

}


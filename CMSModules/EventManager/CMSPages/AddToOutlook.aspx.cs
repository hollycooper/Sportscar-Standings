using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.GlobalHelper;
using CMS.SettingsProvider;
using CMS.SiteProvider;
using CMS.DocumentEngine;
using CMS.UIControls;

using TimeZoneInfo = CMS.SiteProvider.TimeZoneInfo;
using TreeNode = CMS.DocumentEngine.TreeNode;

public partial class CMSModules_EventManager_CMSPages_AddToOutlook : LivePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Get event NodeID from querystring
        int eventNodeId = QueryHelper.GetInteger("eventid", 0);

        if (eventNodeId != 0)
        {
            TreeProvider mTree = new TreeProvider();
            TreeNode eventInfo = mTree.SelectSingleNode(eventNodeId);

            if (eventInfo != null && eventInfo.NodeClassName.EqualsCSafe("cms.bookingevent", true))
            {
                // Get file content.
                byte[] fileContent = GetContent(eventInfo);

                if (fileContent != null)
                {
                    // Clear response.
                    CookieHelper.ClearResponseCookies();
                    Response.Clear();

                    // Prepare response.
                    Response.ContentType = "text/calendar";
                    // Disposition type - For files "attachment" is default
                    Response.AddHeader("Content-Disposition", "attachment;filename=Reminder.ics");

                    Response.OutputStream.Write(fileContent, 0, fileContent.Length);

                    //RequestHelper.CompleteRequest();
                    RequestHelper.EndResponse();
                }
            }
        }
    }


    /// <summary>
    /// Gets iCalendar file content.
    /// </summary>
    /// <param name="row">Datarow</param>
    protected byte[] GetContent(IDataContainer data)
    {
        if (data != null)
        {
            // Get server time zone
            TimeZoneInfo serverTimeZone = TimeZoneHelper.ServerTimeZone;
            // Shift current time (i.e. server time) to GMT
            DateTime currentDateGMT = DateTime.Now;
            currentDateGMT = TimeZoneHelper.ConvertTimeZoneDateTimeToGMT(currentDateGMT, serverTimeZone);

            // Get event start time (i.e. server time)
            DateTime eventStart = ValidationHelper.GetDateTime(data.GetValue("EventDate"), DataHelper.DATETIME_NOT_SELECTED);

            // Get if it is all day event
            bool isAllDay = ValidationHelper.GetBoolean(data.GetValue("EventAllDay"), false);

            // Create content
            StringBuilder content = new StringBuilder();
            content.AppendLine("BEGIN:vCalendar");
            content.AppendLine("METHOD:PUBLISH");
            content.AppendLine("BEGIN:vEvent");
            content.Append("DTSTAMP:").Append(currentDateGMT.ToString("yyyyMMdd'T'HHmmss")).AppendLine("Z");
            content.Append("DTSTART");
            if (isAllDay)
            {
                content.Append(";VALUE=DATE:").AppendLine(eventStart.ToString("yyyyMMdd"));
            }
            else
            {
                // Shift event start time to GMT
                eventStart = TimeZoneHelper.ConvertTimeZoneDateTimeToGMT(eventStart, serverTimeZone);
                content.Append(":").Append(eventStart.ToString("yyyyMMdd'T'HHmmss")).AppendLine("Z");
            }

            // Get event end time
            DateTime eventEnd = ValidationHelper.GetDateTime(data.GetValue("EventEndDate"), DataHelper.DATETIME_NOT_SELECTED);
            if (eventEnd != DataHelper.DATETIME_NOT_SELECTED)
            {
                content.Append("DTEND");
                if (isAllDay)
                {
                    content.Append(";VALUE=DATE:").AppendLine(eventEnd.AddDays(1).ToString("yyyyMMdd"));
                }
                else
                {
                    // Shift event end time to GMT
                    eventEnd = TimeZoneHelper.ConvertTimeZoneDateTimeToGMT(eventEnd, serverTimeZone);
                    content.Append(":").Append(eventEnd.ToString("yyyyMMdd'T'HHmmss")).AppendLine("Z");
                }
            }

            // Get location
            string location = ValidationHelper.GetString(data.GetValue("EventLocation"), string.Empty);

            // Include location if specified
            if (!String.IsNullOrEmpty(location))
            {
                content.Append("LOCATION:").AppendLine(HTMLHelper.StripTags(HttpUtility.HtmlDecode(location)));
            }

            content.Append("DESCRIPTION:").AppendLine(HTMLHelper.StripTags(HttpUtility.HtmlDecode(ValidationHelper.GetString(data.GetValue("EventDetails"), "")).Replace("\r\n", "").Replace("<br />", "\\n")) + "\\n\\n" + HTMLHelper.StripTags(HttpUtility.HtmlDecode(ValidationHelper.GetString(data.GetValue("EventLocation"), "")).Replace("\r\n", "").Replace("<br />", "\\n")));
            content.Append("SUMMARY:").AppendLine(HttpUtility.HtmlDecode(ValidationHelper.GetString(data.GetValue("EventName"), "")));
            content.AppendLine("PRIORITY:3");
            content.AppendLine("BEGIN:vAlarm");
            content.AppendLine("TRIGGER:P0DT0H15M");
            content.AppendLine("ACTION:DISPLAY");
            content.AppendLine("DESCRIPTION:Reminder");
            content.AppendLine("END:vAlarm");
            content.AppendLine("END:vEvent");
            content.AppendLine("END:vCalendar");

            // Return byte array
            return Encoding.UTF8.GetBytes(content.ToString());
        }

        return null;
    }
}
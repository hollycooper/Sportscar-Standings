using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CMS.OutputFilter;
using CMS.SettingsProvider;
using CMS.UIControls;

public partial class CMSModules_System_Debug_System_DebugOutput : CMSDebugPage
{
    private ArrayList logs = new ArrayList();


    protected void Page_Load(object sender, EventArgs e)
    {
        btnClear.Text = GetString("DebugOutput.ClearLog");

        ReloadData();
    }


    protected override void Render(HtmlTextWriter writer)
    {
        if (logs.Count > 0)
        {
            double totalDuration = 0;

            // Summarize all logs
            foreach (LogControl log in logs)
            {
                totalDuration += log.TotalDuration;
            }

            lblInfo.Text = String.Format(GetString("RequestLog.Total"), totalDuration, logs.Count);
        }
        else
        {
            if (OutputHelper.DebugOutput)
            {
                lblInfo.Text = GetString("RequestLog.NotFound");
            }
        }

        base.Render(writer);
    }


    protected void ReloadData()
    {
        if (!OutputHelper.DebugOutput)
        {
            ShowWarning(GetString("DebugOutput.NotConfigured"), null, null);
        }
        else
        {
            plcLogs.Controls.Clear();

            for (int i = OutputHelper.LastLogs.Count - 1; i >= 0; i--)
            {
                try
                {
                    // Get the log
                    RequestLog log = (RequestLog)OutputHelper.LastLogs[i];
                    if (log != null)
                    {
                        // Load the control
                        OutputLog logCtrl = (OutputLog)LoadUserControl("~/CMSAdminControls/Debug/OutputLog.ascx");
                        logCtrl.ID = "outputLog";
                        logCtrl.EnableViewState = false;
                        logCtrl.Log = log;
                        logCtrl.DisplayHeader = false;
                            
                        // Add to the output
                        plcLogs.Controls.Add(new LiteralControl("<div>&lrm;<strong>&nbsp;" + GetRequestLink(log.RequestURL, log.RequestGUID) + "</strong> (" + log.RequestTime.ToString("hh:mm:ss") + ")&lrm;<br /><br />"));
                        plcLogs.Controls.Add(logCtrl);
                        plcLogs.Controls.Add(new LiteralControl("</div><br /><br />"));

                        logs.Add(logCtrl);
                    }
                }
                catch //(Exception ex)
                {
                }
            }
        }
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        OutputHelper.LastLogs.Clear();
        logs.Clear();
        ReloadData();
    }
}
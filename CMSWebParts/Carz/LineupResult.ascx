<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSWebParts_Carz_LineupResult" CodeFile="~/CMSWebParts/Carz/LineupResult.ascx.cs" %>

<div class="introBlock container-field clearfix ResultDetail">
     <asp:Literal ID="LiteralTesst" runat="server"></asp:Literal>
    <asp:Literal ID="LiteralTitle" runat="server"></asp:Literal>
    <div class="featuredCar container-field" style="width: 400px;">
					
		 <asp:Literal ID="LiteralResultDetail" runat="server"></asp:Literal>
        
    </div>
    <div class="ad3Block clearfix"><!--Ad3 here-->
        <script type="text/javascript"><!--
            google_ad_client = "ca-pub-6361847574253241";
            /* RR AD3 - Right Block */
            google_ad_slot = "8374760863";
            google_ad_width = 250;
            google_ad_height = 250;
            //-->
        </script>
        <script type="text/javascript"
        src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
        </script>
        </div>
</div>

<asp:Literal ID="LiteralResultDetail2" runat="server"></asp:Literal>
	<div  class="list clearfix" <asp:Literal ID="LiteralNone" runat="server"></asp:Literal>>
                      <br />
<asp:UpdateProgress ID="UpdateProgress1" runat="server" >
<ProgressTemplate>
    <div style="left: 47%; top: 35%; position: fixed;" >  
                   <img  width="32px" height="32px" src="~/App_Themes/carz/images/loading.gif" alt="Loading ... "/>
     </div>
    <div  style="opacity: 10;"></div>  
</ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
 <ContentTemplate>

<div style="float:left;clear:both;width:100%;">

<p  class="searchfilter" style="float:left;" ><strong>0-60 mph Standings</strong>

      <!-- AddThis Button BEGIN -->
<div class="addthis_toolbox addthis_default_style " style="float:right;width:150px">
<a class="addthis_button_preferred_1"></a>
<a class="addthis_button_preferred_2"></a>
<a class="addthis_button_preferred_3"></a>
<a class="addthis_button_preferred_4"></a>
<a class="addthis_button_compact"></a>
<a class="addthis_counter addthis_bubble_style"></a>
</div>
<script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-527c86a23018d570"></script>
<!-- AddThis Button END -->
   
    <div style="float:right;margin-right:45px;">
    
       <asp:ImageButton ID="ImageButton1" runat="server"  ImageUrl="/App_Themes/carz/images/btn-search.gif" CssClass="button ImageButton" OnClick="ImageButton1_Click" AutoPostBack="True"/>
          
      <asp:TextBox ID="TextBoxFilter" runat="server" CssClass="inputbox" Text="Keyword filter"  onblur="if (this.value=='') this.value='Keyword filter';" onfocus="if (this.value=='Keyword filter') this.value='';"></asp:TextBox>
       
     
    </div>  
   
  

</div>

 <div style="float:left;">
        

                 <asp:GridView ID="GridViewResult" EmptyDataText="No data available." runat="server" DataKeyNames="ItemID" AutoGenerateColumns="False" GridLines="None" AllowPaging="True" CssClass="activityResults fixedWidthSmall" PageSize="20" Width="677px" OnPageIndexChanging="GridViewResult_PageIndexChanging" AllowSorting="True" OnRowCreated="GridViewResult_RowCreated" OnSorting="GridViewResult_Sorting" OnRowDataBound="GridViewResult_RowDataBound" PagerSettings-Mode="NumericFirstLast" AlternatingRowStyle-BackColor="WhiteSmoke" CellPadding="4" ForeColor="#333333">
         <AlternatingRowStyle BackColor="White" />
         <Columns>
             <asp:BoundField DataField="STT" HeaderText="Rank"
                 HeaderStyle-Wrap="false"
                 SortExpression="STT" >

             <HeaderStyle Wrap="False" />
             </asp:BoundField>

             <asp:BoundField DataField="Year" HeaderText="Year"
                 HeaderStyle-Wrap="false"
                 SortExpression="Year" >
             <HeaderStyle Wrap="False" />
             </asp:BoundField>
             <asp:BoundField DataField="Make" HeaderText="Make"
                 HeaderStyle-Wrap="false"
                 SortExpression="Make" >
             <HeaderStyle Wrap="False" />
             </asp:BoundField>
             <asp:BoundField DataField="Model" HeaderText="Model"
                 HeaderStyle-Wrap="false"
                 SortExpression="Model" >
             <HeaderStyle Wrap="False" />
             </asp:BoundField>
             <asp:BoundField DataField="Horsepower" HeaderText="Horsepower"
                 HeaderStyle-Wrap="false"
                 SortExpression="Horsepower" DataFormatString="{0:0}" >
             <HeaderStyle Wrap="False" />
             </asp:BoundField>
             <asp:BoundField DataField="MSRP" HeaderText="MSRP"
                 HeaderStyle-Wrap="false"
                 SortExpression="MSRP" DataFormatString="${0:0,0}" >
             <HeaderStyle Wrap="False" />
             </asp:BoundField>
             <asp:BoundField DataField="Best060Time" HeaderText="0-60 Time"
                 HeaderStyle-Wrap="false"
                 SortExpression="Best060Time" DataFormatString="{0:0.0}" >

             <HeaderStyle Wrap="False" />
             </asp:BoundField>
             
             <asp:BoundField DataField="ItemID" HeaderText="ItemID" Visible="false" />

             <asp:BoundField DataField="URLslug" HeaderText="URLslug" Visible="False"></asp:BoundField>
         </Columns>
       
        
         <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="white" />
         <PagerStyle CssClass="PagerStyle" VerticalAlign="Middle" HorizontalAlign="Center"  ForeColor="#175E97" Font-Bold="True" />
         <RowStyle BackColor="WhiteSmoke" />
        
     </asp:GridView>
        </div>
      
 </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
   
jQuery(document).ready(function() 
{
    <asp:Literal ID="LiteralActiveMenu" runat="server"></asp:Literal>
   
   
}


   
);

  </script>

     

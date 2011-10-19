<%@ page language="C#" masterpagefile="~/MasterPage/MasterPage.Master" autoeventwireup="true" inherits="Administrator_Report, App_Web_adafxnuj" %>

<%@ Register Assembly="StatefullScrollPanel" Namespace="CustomControls" TagPrefix="asp" %>
<%@ Register Src="Reports_UserControls/NavigationReportControl.ascx" TagName="NavigationReportControl"
    TagPrefix="uc1" %>
<%@ Register src="../UserControlsForAll/BlueButton.ascx" tagname="BlueButton" tagprefix="uc2" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

    <script language="Javascript" src="../FusionCharts/FusionCharts.js"></script>
    <script type="text/javascript" language="javascript" src="../anychartstock_files/js/AnyChartStock.js"></script>
   
</asp:Content>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="AccordionContent" ContentPlaceHolderID="VerticalOutlookMenu_PlaceHolder"
    runat="server">

    <script type="text/javascript">
        function resizeReports() {
            var reportPanelHeight = document.getElementById('outputId').style.height;
            reportPanelHeight = reportPanelHeight.substring(0, reportPanelHeight.length - 2);
            document.getElementById('tabs').style.height = reportPanelHeight - 7 + "px";
            document.getElementById('ctl00_Reports_PlaceHolder_NavigationReportControl1_ScrollPanel').style.height = reportPanelHeight - 70 + "px";
            
            var oneAccPanelHeight = document.getElementById('firstAccordionPanel').style.height;
            oneAccPanelHeight = oneAccPanelHeight.substring(0, oneAccPanelHeight.length - 2);
            document.getElementById('DriverOverFlowPanel').style.height = oneAccPanelHeight - 45 + "px";
            document.getElementById('VehicleOverFlowPanel').style.height = oneAccPanelHeight - 2 + "px";
            document.getElementById('MultiDriverOverFlowPanel').style.height = oneAccPanelHeight - 21 + "px";
            document.getElementById('MultiVehicleOverFlowPanel').style.height = oneAccPanelHeight - 21 + "px";
            document.getElementById('PLFOverFlowPanel').style.height = oneAccPanelHeight - 135 + "px";
        }

        function pageLoad() {
            resizeReports();
        }

        $(document).ready(function() {
            resizeReports();
            $("#dialog").dialog();
            $("#dialog2").dialog({ autoOpen: false, draggable: true, resizable: true });
        });

        $(window).resize(function() {
            resizeAllMaster();
            $("#accordion").accordion("resize");
            resizeReports();
        });
        
      function showModal() {
          $find('ShowModal').show();
      }
    </script>

    <asp:Panel ID="modalPopupPanel" CssClass="modalPopup" runat="server" Style="display: none">
        <asp:Image ImageUrl="~/images/icons/long1.gif" runat="server" Width="100%" />
        <asp:Table ID="ModalPopupTable" runat="server" Width="100%">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Center" VerticalAlign="Top">
                    <asp:Label ID="Label5" ForeColor="Black" Font-Bold="false" Text="Загрузка, подождите...." runat="server" />
                </asp:TableCell></asp:TableRow></asp:Table></asp:Panel><asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="modalPopupHdnField"
        PopupControlID="modalPopupPanel" BackgroundCssClass="modalBackgroung" BehaviorID="ShowModal" />
    <asp:HiddenField ID="modalPopupHdnField" runat="server" />
    
    
     <asp:HiddenField ID="AccordionSelectedPane" Visible="true" runat="server" Value="0" /> 
    
     <script type="text/javascript" language="javascript">
         function acordionIndexSwitch(accIndex) {
             document.getElementById("<% =AccordionSelectedPane.ClientID %>").value = accIndex;
         }
    </script>   
    
    <div id="accordion" style="width: 5;">
        <h3>
            <asp:LinkButton ID="AccordionHeader1_Driver" runat="server" OnClientClick="acordionIndexSwitch(0);" PostBackUrl="#" Text="Водитель" /></h3>
        <div id="firstAccordionPanel">
            <asp:UpdatePanel ID="DriverSelectUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Table runat="server" Width="100%" Height="1px"><asp:TableRow><asp:TableCell></asp:TableCell></asp:TableRow></asp:Table><asp:DropDownList ID="drSearch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DriverSearchMade"
                        Width="100%"/>     
                    <asp:ListSearchExtender ID="ListSearchExtender2" runat="server" PromptPosition="Top" PromptText="Начните вводить для поиска"
                         TargetControlID="drSearch"  QueryPattern="Contains" PromptCssClass="ListSearchExtenderPrompt"/>
                    <hr/>
                    <div id="DriverOverFlowPanel" style="overflow:auto; border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px; border: 1px solid #AFCBDE;">
                        <asp:TreeView ID="DriversTreeView" runat="server" ForeColor="Black" HoverNodeStyle-ForeColor="Firebrick"
                            SelectedNodeStyle-ForeColor="Firebrick" RootNodeStyle-Font-Bold="true" SelectedNodeStyle-Font-Underline="true"
                            NodeStyle-HorizontalPadding="5" NodeIndent="20" OnSelectedNodeChanged="DriversTreeView_SelectedNodeChanged" ShowLines="true"  />
                    </div>
                 
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DriversTreeView" EventName="SelectedNodeChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <h3>
            <asp:LinkButton ID="AccordionHeader2_Vehicle" runat="server" OnClientClick="acordionIndexSwitch(1);" PostBackUrl="#" Text="Транспортное средство" /></h3>
        <div>
            <asp:UpdatePanel ID="VehicleSelectUpdatePanel" runat="server">
                <ContentTemplate>
                    <div id="VehicleOverFlowPanel" style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px; border: 1px solid #AFCBDE;">
                        <asp:TreeView ID="VehiclesTreeView" runat="server" ForeColor="Black" HoverNodeStyle-ForeColor="Firebrick"
                            SelectedNodeStyle-ForeColor="Firebrick" RootNodeStyle-Font-Bold="true" SelectedNodeStyle-Font-Underline="true"
                            NodeStyle-HorizontalPadding="5" NodeIndent="20" OnSelectedNodeChanged="VehTree_SelectedNodeChanged" ShowLines="true" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="VehiclesTreeView" EventName="SelectedNodeChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <h3>
            <asp:LinkButton ID="AccordionHeader3_DriversGroup" runat="server" OnClientClick="acordionIndexSwitch(2);" PostBackUrl="#" Text="Группа водителей" /></h3>
        <div>
            <asp:UpdatePanel ID="MultiDriversSelectUpdatePanel" runat="server">
                <ContentTemplate>
                    <div style="padding:0px 0px 0px 8px;">
                        <asp:CheckBox  ID="MultiDriversSelectAllDrivers" runat="server" AutoPostBack="true"
                            Text="Все водители" BackColor="White" OnCheckedChanged="SelectAllMultiDrivers" />
                    </div>
                    <div id="MultiDriverOverFlowPanel" style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px; border: 1px solid #AFCBDE;">
                        <asp:CheckBoxList ID="MultiDrivers" runat="server" BackColor="White"
                            CellSpacing="1" AutoPostBack="true" Font-Bold="true"
                            OnSelectedIndexChanged="MultiDrivers_SelectedIndexChanged" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="MultiDrivers" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="MultiDriversSelectAllDrivers" EventName="CheckedChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <h3>
            <asp:LinkButton ID="AccordionHeader4_VehiclesGroup" runat="server" OnClientClick="acordionIndexSwitch(3);" PostBackUrl="#" Text="Группа ТС" /></h3>
        <div>
            <asp:UpdatePanel ID="MultiVehiclesSelectUpdatePanel" runat="server">
                <ContentTemplate>
                    <div style="padding:0px 0px 0px 8px;">
                        <asp:CheckBox  ID="MultiVehiclesSelectAllVehicles" runat="server" AutoPostBack="true"
                            Text="Все ТС" BackColor="White" />
                    </div>
                    <div id="MultiVehicleOverFlowPanel" style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px; border: 1px solid #AFCBDE;">
                        <asp:CheckBoxList ID="MultiVehicles" runat="server" BackColor="White"
                            CellSpacing="1" AutoPostBack="true" Font-Bold="true"/>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <h3>
            <asp:LinkButton ID="AccordionHeader5_PLF" runat="server" OnClientClick="acordionIndexSwitch(4);" PostBackUrl="#" Text="PLF Файлы(Датчики)" /></h3>
        <div>
            <asp:UpdatePanel ID="PlfFileChoiseUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Table ID="Table1" runat="server" Width="100%" Height="1px"><asp:TableRow><asp:TableCell></asp:TableCell></asp:TableRow></asp:Table><asp:DropDownList ID="plfDrSearch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PlfDriverSearchMade"
                        Width="225px"/>
                    <asp:ListSearchExtender ID="plfDrSearchExtender" runat="server" PromptPosition="Top" PromptText="Начните вводить для поиска"
                         TargetControlID="plfDrSearch"  QueryPattern="Contains" PromptCssClass="ListSearchExtenderPrompt"/>
                    <hr/>
                    <div id="PLFOverFlowPanel" style="overflow:auto; border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px; border: 1px solid #AFCBDE;">
                        <asp:TreeView ID="PLFTreeView" runat="server" ForeColor="Black" HoverNodeStyle-ForeColor="Firebrick"
                            SelectedNodeStyle-ForeColor="Firebrick" SelectedNodeStyle-Font-Underline="true"
                            RootNodeStyle-Font-Bold="true" NodeStyle-HorizontalPadding="5" NodeIndent="20"
                            OnSelectedNodeChanged="PLFTree_SelectedNodeChanged" ShowLines="true" />
                    </div>        
                    <hr/>
                    <asp:Panel runat="server" ID="PlfFilesListPanel" Height="70px" ScrollBars="Auto">
                        <asp:Label ID="PlfFilesList" runat="server" ForeColor="Red" Font-Bold="false"/>
                    </asp:Panel>     
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="PLFTreeView" EventName="SelectedNodeChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ChoisesContent" ContentPlaceHolderID="MainConditions_PlaceHolder"
    runat="server">
                <asp:UpdatePanel ID="ChoisesUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Table ID="choisesTable" runat="server" Width="100%" BackColor="ControlLight">
                            <asp:TableRow Height="25px">
                                <asp:TableCell Width="330px">
                                    <asp:Label ID="ReportNameLabel" runat="server" Font-Size="Small" ForeColor="DarkBlue"
                                        Font-Bold="true" Text="Выберите отчет" />
                                </asp:TableCell><asp:TableCell Width="330px">
                                    <asp:Label ID="DriversNameSourceLabel" runat="server" Font-Size="Small" ForeColor="DarkBlue"
                                        Font-Bold="true" />
                                    <asp:Label ID="DriversNameLabel" runat="server" Font-Size="Small" ForeColor="DarkBlue"
                                        Font-Bold="false" />
                                </asp:TableCell><asp:TableCell></asp:TableCell><asp:TableCell Width="150px">
                                    <asp:UpdatePanel ID="PeriodsReportButtonUpdatePanel" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc2:BlueButton ID="GenerateReportPeriodically" runat="server" Text="Периодически" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:TableCell></asp:TableRow><asp:TableRow  Height="25px">
                                <asp:TableCell>
                                    <asp:Label ID="fromCalendarDateLabel" Text="Начало " runat="server" />
                                    <asp:TextBox ID="fromCalendarDate" Width="174px" runat="server" />
                                </asp:TableCell><asp:TableCell>
                                    <asp:Label ID="toCalendarDateLabel" Text="   Окончание " runat="server" />
                                    <asp:TextBox ID="toCalendarDate" Width="174px" runat="server" />
                                </asp:TableCell><asp:TableCell></asp:TableCell><asp:TableCell Width="150px">
                                    <asp:UpdatePanel ID="GenerateReportButtonUpdatePanel" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <uc2:BlueButton ID="CalendarApplyButton2" runat="server" OnClientClick="showModal();" CausesValidation="false"  Text="Сгенерировать отчет" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:TableCell></asp:TableRow></asp:Table><asp:Panel ID="periodCalendarPanel" runat="server" Visible="true" Width="100%">
                            <asp:CalendarExtender ID="CalendarFromExtender" runat="server" TargetControlID="fromCalendarDate"
                                Format="d MMMM yyyy" Animated="true" />
                            <asp:CalendarExtender ID="CalendarToExtender" runat="server" TargetControlID="toCalendarDate"
                                Format="d MMMM yyyy" Animated="true" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="DataContent" ContentPlaceHolderID="Reports_PlaceHolder" runat="server">
<asp:UpdatePanel ID="OutputUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>    
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(asss);
        function asss() {
            $('#tabs').tabs();           
        }
    </script>       


    <div id="tabs" style="background: transparent;">                
        <ul style="height:30px;">
		    <li><asp:LinkButton ID="ReportsTab" runat="server" Text="Отчеты" href="#tabs-1"/></li>
		    <li><asp:LinkButton ID="ChartsTab" runat="server" Text="Графики" href="#tabs-2"/></li>
	    </ul>
        <div id="tabs-1">
            <asp:UpdatePanel ID="ReportUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="NavigationReportControl1_ScrollPanel" Height="100%" ScrollBars="Auto">
                            <asp:UpdatePanel ID="PlfReportsPickUpdatePanel1" runat="server"  UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="Panel7" runat="server" BorderWidth="1px" BorderColor="LightGray" Width="300px" Height="38px"
                                    style="padding: 9px; border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;">
                                    Выберите тип отчета: <asp:DropDownList ID="PlfReportsRadioButtonList" Font-Size="Small" Visible="false" 
                                        runat="server" AutoPostBack="true" Width="100%"
                                        OnSelectedIndexChanged="PlfReportsDataGrid_RadioButton_Checked"/>
                                    </asp:Panel>
                                    <asp:HiddenField ID="Selected_PlfReportsDataGrid_Index" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        <uc1:NavigationReportControl ID="NavigationReportControl1" runat="server" />
                    </asp:Panel>    
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="tabs-2">
            <asp:Panel runat="server" ID="AnyChart_ScrollPanel" Height="350px" ScrollBars="Auto">
            
                <asp:Panel Width="100%" ID="AddCondWidthPanel" runat="server">
                        <asp:UpdatePanel ID="ChartsCheckBoxListUpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="Panel6" runat="server" BorderWidth="1px" BorderColor="LightGray"
                                    style="padding: 9px; vertical-align:middle; border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;">
                                    <asp:CheckBoxList runat="server" ID="ChartsCheckBoxList" Font-Size="X-Small" RepeatDirection="Horizontal" CellSpacing="1"
                                        Width="100%" RepeatColumns="8" OnSelectedIndexChanged="ChartsCheckBoxList_SelectedIndexChanged" />
                                </asp:Panel>            
                            </ContentTemplate>
                        </asp:UpdatePanel>
                </asp:Panel>    
            
                <asp:Literal ID="ChartLiteral" runat="server" Mode="Transform" />
                <asp:Panel ID="AnycahrtPanel" Visible="false" runat="server">
                    <div id="AnyChartContainer" style="width: 100%; height: 400px">
                    </div>
                </asp:Panel>
                <asp:HiddenField runat="server" ID="MapPath_HiddenFieldForDeleteingXml" />
                <asp:HiddenField runat="server" ID="AnyChartHiddenField" />
           </asp:Panel>     
        </div>
    </div>
   
    <script type="text/javascript" language="javascript">
        function updateAnyChart() {
            //<![CDATA[
            var chart = new AnyChartStock("../anychartstock_files/swf/AnyChartStock.swf", "../anychartstock_files/swf/Preloader.swf");
            var fileName = document.getElementById("<% =AnyChartHiddenField.ClientID %>").value;
            chart.onChartDataLoad = DeleteFileName;
            chart.setXMLFile(fileName);
            chart.write("AnyChartContainer");
            //]]>
        }
        function DeleteFileName() {
            var mapPath = document.getElementById("<% =MapPath_HiddenFieldForDeleteingXml.ClientID %>").value;
            var fileName = document.getElementById("<% =AnyChartHiddenField.ClientID %>").value;
            PageMethods.DeleteMethod(mapPath + "\\" + fileName, null, null);
        }
    </script>
    </ContentTemplate>
</asp:UpdatePanel>
 <asp:UpdatePanelAnimationExtender ID="ReportUpdatePanelnExtender" runat="server"
        TargetControlID="OutputUpdatePanel" Enabled="True">
        <Animations>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="updateAnyChart();" />                                                       
                </Parallel>   
            </OnUpdated>
        </Animations></asp:UpdatePanelAnimationExtender></asp:Content><asp:Content ID="DecisionContent1" ContentPlaceHolderID="Decision_PlaceHolder" runat="server">
<asp:UpdatePanel ID="DecisionUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Table runat="server"  ID="resultActionButtonsTable" CellPadding="3" GridLines="None" Width="100%">
            <asp:TableRow>
                <asp:TableCell Width="15%">
                    <uc2:BlueButton ID="Export_Button" Text="Экспорт" Enabled="false" runat="server" OnClientClick="$('#dialog').dialog({autoOpen: true, draggable: false, resizable: false, modal:true });"/>                    
                </asp:TableCell><asp:TableCell></asp:TableCell><asp:TableCell Width="15%">
                    <uc2:BlueButton ID="Choises_CANCEL_Button" Text="Отменить" runat="server" Visible="false" CausesValidation="false" />
                </asp:TableCell><asp:TableCell Width="15%">        
                    <uc2:BlueButton ID="Choises_ADD_Button" Text="Добавить" runat="server" Visible="false" CausesValidation="true"/>
                </asp:TableCell><asp:TableCell Width="15%">        
                    <uc2:BlueButton ID="Email_Button" Text="E-mail" runat="server" CausesValidation="true" OnClientClick="$('#dialog2').dialog({autoOpen: true, draggable: true, resizable: true, modal:true });"/>
                </asp:TableCell><asp:TableCell Width="15%">        
                    <uc2:BlueButton ID="Print_Button" Text="Печать" runat="server" CausesValidation="true"/>
                </asp:TableCell></asp:TableRow></asp:Table></ContentTemplate><Triggers>
    </Triggers>
</asp:UpdatePanel>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="Bottom_PlaceHolder" runat="server">

    <script type="text/javascript" language="javascript">
        function ExportRbl_SelIndexChanged() {
            var selectedValue = $('#<%= ReportsExport_RadioButtonList.ClientID %>').find('input:checked').val();
            document.getElementById("<% =ReportsExport_RadioButtonList_SelectedValHiddenField.ClientID %>").value = selectedValue;
        }
        function AllPagesSelected() {
            document.getElementById("<% =CurrentPage_RadioButton.ClientID %>").checked = false;
            document.getElementById("<% =CustomPages_RadioButton.ClientID %>").checked = false;
            document.getElementById("<% =IntervalPages_TextBox.ClientID %>").disabled = true;
            document.getElementById("<% =PagesExportsOption_hiddenField.ClientID %>").value = "AllPages";
        }
        function CurrentPageSelected() {
            document.getElementById("<% =AllPages_RadioButton.ClientID %>").checked = false;
            document.getElementById("<% =CustomPages_RadioButton.ClientID %>").checked = false;
            document.getElementById("<% =IntervalPages_TextBox.ClientID %>").disabled = true;
            document.getElementById("<% =PagesExportsOption_hiddenField.ClientID %>").value = "CurrentPage";
        }
        function CustomPageSelected() {
            document.getElementById("<% =CurrentPage_RadioButton.ClientID %>").checked = false;
            document.getElementById("<% =AllPages_RadioButton.ClientID %>").checked = false;
            document.getElementById("<% =CustomPages_RadioButton.ClientID %>").checked = true;
            document.getElementById("<% =IntervalPages_TextBox.ClientID %>").disabled = false;
            document.getElementById("<% =PagesExportsOption_hiddenField.ClientID %>").value = "CustomPages";
        }
        function CustomPageTextBoxUpdate() {
            var currentTextBoxVal = document.getElementById("<% =IntervalPages_TextBox.ClientID %>").value;
            document.getElementById("<% =CustomPagesValue_hiddenField.ClientID %>").value = currentTextBoxVal;
        }
        function EmailAddressTextBoxUpdate() {
            var currentTextBoxVal = document.getElementById("<% =EmailSend_AdressTextBox.ClientID %>").value;
            document.getElementById("<% =EmailAddress_HiddenField.ClientID %>").value = currentTextBoxVal;
        }
    </script>
    <asp:HiddenField id="ReportsExport_RadioButtonList_SelectedValHiddenField" Value="PDF" runat="server" />
    <asp:HiddenField ID="CustomPagesValue_hiddenField" Value="" runat="server"/>
    <asp:HiddenField ID="EmailAddress_HiddenField" runat="server"/>
    <asp:HiddenField ID="PagesExportsOption_hiddenField" Value="AllPages" runat="server"/>
    <div id="dialog" title="Экспорт отчета">
                  
                Выберите формат для экспорта: <asp:RadioButtonList ID="ReportsExport_RadioButtonList" RepeatColumns="2" runat="server" Width="100%" onchange="ExportRbl_SelIndexChanged();" />
                <hr />
                <asp:Panel ID="PrintPagesSelectorControl" runat="server">
                    <asp:Label ID="PagesInterval_Label" runat="server" Text="Диапазон страниц:" />
                    <asp:Table ID="PrintPageSelectorTable" runat="server" Width="100%" Height="55px" GridLines="None">
                        <asp:TableRow>
                            <asp:TableCell Width="29%">
                                <asp:RadioButton ID="AllPages_RadioButton" runat="server" Text="Все" Checked="true" onchange="AllPagesSelected();" />
                            </asp:TableCell><asp:TableCell>
                                <asp:RadioButton ID="CurrentPage_RadioButton" runat="server" Text="Текущая страница" onchange="CurrentPageSelected();"/>
                            </asp:TableCell></asp:TableRow><asp:TableRow>
                             <asp:TableCell>
                                <asp:RadioButton ID="CustomPages_RadioButton" runat="server" Text="Страницы" onchange="CustomPageSelected();"/>
                            </asp:TableCell><asp:TableCell>
                                <div onmousedown="CustomPageSelected();">
                                    <asp:TextBox ID="IntervalPages_TextBox" runat="server" onchange="CustomPageTextBoxUpdate();"  Height="14px" Width="80%" Font-Size="9" Enabled="false"/>
                                </div>
                            </asp:TableCell></asp:TableRow></asp:Table><p><asp:Label ID="IntervalPages_Label" runat="server" Text="Введите номера или интервал страниц через запятую(например: 1,3,5-10)" /></p>
                </asp:Panel>
        <asp:UpdatePanel ID="ExportUpdatePanel" runat="server" UpdateMode="Always">
            <ContentTemplate>  
                <asp:Table ID="Table2" runat="server" Width="100%" GridLines="None">
                    <asp:TableRow>
                        <asp:TableCell Width="25%">
                            <uc2:BlueButton ID="SendByEmail_DialogOpen" Text="E-mail" runat="server" CausesValidation="false"  OnClientClick="$('#dialog2').dialog({autoOpen: true, draggable: true, resizable: true, modal:true });"/>
                        </asp:TableCell><asp:TableCell ></asp:TableCell><asp:TableCell Width="25%">
                            <uc2:BlueButton ID="Export_OK_Button" Text="Сохранить" runat="server" CausesValidation="false" OnClientClick="$('#dialog').dialog('close');" />
                        </asp:TableCell><asp:TableCell Width="25%">
                            <uc2:BlueButton ID="Export_Cancel_Button" Text="Отмена" runat="server" CausesValidation="false" OnClientClick="$('#dialog').dialog('close');" />
                        </asp:TableCell></asp:TableRow></asp:Table>
            </ContentTemplate>
            <Triggers>  
                <asp:PostBackTrigger ControlID="Export_OK_Button" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div id="dialog2" title="Отправить по электронной почте">
        <asp:UpdatePanel ID="EmailSendDialog_UpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="EmailSend_AdressLabel" runat="server" Text="Укажите E_mail адрес:" /> 
                <br />           
                <asp:TextBox ID="EmailSend_AdressTextBox" runat="server" Width="70%" onchange="EmailAddressTextBoxUpdate();"/>
                <asp:Table ID="Table3" runat="server" Width="100%">
                    <asp:TableRow Height="60px"></asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="25%">
                            <uc2:BlueButton ID="SendByEmail" Text="Отправить" runat="server" CausesValidation="true" OnClientClick="$('#dialog2').dialog('close');" />
                        </asp:TableCell>
                        <asp:TableCell></asp:TableCell>
                        <asp:TableCell Width="25%">
                            <uc2:BlueButton ID="SendByEmail_Cancel" Text="Отмена" runat="server" CausesValidation="false" OnClientClick="$('#dialog2').dialog('close');" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>           
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        
    <asp:UpdatePanel ID="resultActionUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <h3><asp:Label ID="resultActionLabel" Text="resultAction" runat="server" /></h3>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="HiddenFieldUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenField" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>   
</asp:Content>

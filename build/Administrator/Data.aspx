<%@ page language="C#" masterpagefile="~/MasterPage/MasterPage.Master" autoeventwireup="true" inherits="Administrator_Data, App_Web_adafxnuj" enableeventvalidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControlsForAll/BlueButton.ascx" TagName="BlueButton" TagPrefix="uc2" %>
<%@ Register Src="Data_UserControls/LoadDataStatistics.ascx" TagName="LoadDataStatistics"
    TagPrefix="uc1" %>
<asp:Content ID="AccordionContent" ContentPlaceHolderID="VerticalOutlookMenu_PlaceHolder"
    runat="server">
    <script src="../js/custom/Arhive.js" type="text/javascript"></script>
    <script type="text/javascript">
        //run on page load
        $(function () {

            $("#accordion").accordion({
                change: function (event, ui) {
                    //закладка "Восстановить у пользователя"
                    if ($("a", ui.newHeader).text() == "Восстановить у пользователя") {
                        loadRecoverUserData();
                    };
                    if ($("a", ui.newHeader).text() == "Просмотреть(Водитель)") {
                        loadOverlookDriver();
                    }
                    if ($("a", ui.newHeader).text() == "Просмотреть(ТС)") {
                        loadOverlookVehicle();
                    }
                }
            });


            resizeReports();
            $('#dialog2').dialog({ autoOpen: false, draggable: true, resizable: true, modal: true, width: 640 });
        });

        function showModal() {
            $find('ShowModal').show();
        }

        function onAccordionSelectedIndexChanged(accIndex) {
            /*document.getElementById("<% =AccordionCurrentTabIndex_HiddenField.ClientID %>").value = accIndex;
            var actionBtn = "<%= InvisibleAccordionButton.ClientID %>";
            document.getElementById(actionBtn).click();*/
        }

        function resizeReports() {
            var vertHeightSTR = document.getElementById('vertical-menu').style.height;
            vertHeightSTR = vertHeightSTR.substr(0, vertHeightSTR.length - 2);
            document.getElementById('outputId').style.height = (vertHeightSTR - 20) + "px";
        }

        function LoadDays() {
            alert("23423423");
        }

        $(window).resize(function () {
            resizeAllMaster();
            resizeReports();
            $("#accordion").accordion("resize");
        });       
    </script>
    <script id="tmplGroupTree" type="text/x-jquery-tmpl">
        <li class="folder"><a><span>${OrgName}</span></a>
        <ul>
            {{each groups}}
            <li class="file"><a><span>${GroupName}</span></a>
                <ul>
                    {{each values}}
                    <li class="file"><a><span>${Value}</span></a></li>
                    {{/each}}
                </ul>
                </li>
            {{/each}}
        </ul>
        </li>
    </script>
    <script id="tmplTreeItem" type="text/x-jquery-tmpl">
        <li class="file"><a><span key=${Key}>${Value}</span></a></li>
    </script>
    <script id="tmplDriversTable" type="text/x-jquery-tmpl">
        
        <tr class="wijmo-wijgrid-row ui-widget-content wijmo-wijgrid-datarow">
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell" style="margin-left:5px;">
                    {{html Number}}
                </div>
            </td>

            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    {{html Name}}
                </div>
            </td>

            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    {{html CardTypeName}}
                </div>
            </td>

            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                        {{html FromDate}}
                    </center>
                </div>
            </td>

            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                        {{html ToDate}}
                    </center>
                </div>
            </td>

            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                    {{html RecordsCount}}
                    </center>
                </div>
            </td>

            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                    {{html CreationTime}}
                    </center>
                </div>
            </td>
            <td class="wijgridtd wijdata-type-string">
                <div class="wijmo-wijgrid-innercell">
                    <center>
                    <form name="_ctl0" method="post" action="Download.aspx" id="_ctl0">
                        <input type="hidden" name="type" value="RecoverUserDownload"/> 
                        <input type="hidden" name="dataBlockId" value="{{html DataBlockId}}"/> 
                        <input type="submit" value="" class="document-icon"/> 
                    </form>
                    </center>
                </div>
            </td>                      
        </tr>

    </script>
    <script id="tmplHeadColumn" type="text/x-jquery-tmpl">
        <th class="ui-widget wijmo-c1basefield ui-state-default wijmo-c1field" style="{{html style}}">
            <div class="wijmo-wijgrid-innercell">
                <span class="wijmo-wijgrid-headertext">{{html text}}</span>
            </div>
        </th>
    </script>
    <asp:UpdatePanel ID="InvisibleUpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div style="display: none;">
                <asp:Button ID="InvisibleAccordionButton" runat="server" CausesValidation="false"
                    OnClick="AccordionHeader1_Click" Visible="true" />
                <asp:HiddenField ID="AccordionCurrentTabIndex_HiddenField" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="modalPopupPanel" CssClass="modalPopup" runat="server" Style="display: none">
        <asp:Image ID="Image1" ImageUrl="~/images/icons/long1.gif" runat="server" Width="100%" />
        <asp:Table ID="ModalPopupTable" runat="server" Width="100%">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Center" VerticalAlign="Top">
                    <asp:Label ID="Label7" Text="Разбор , подождите...." runat="server" ForeColor="Black" />
                </asp:TableCell></asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="modalPopupHdnField"
        PopupControlID="modalPopupPanel" BackgroundCssClass="modalBackgroung" BehaviorID="ShowModal" />
    <asp:HiddenField ID="modalPopupHdnField" runat="server" />
    <div id="accordion" style="width: 5">
        <h3>
            <asp:LinkButton ID="AccordionHeader1_Upload" runat="server" PostBackUrl="#" OnClientClick="onAccordionSelectedIndexChanged(0);"
                Text="Загрузить на сервер" />
        </h3>
        <div>
            <asp:UpdatePanel ID="UploadTestUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <p>
                        <b>
                            <asp:Label ID="FileIploadLabel" runat="server" Text="Укажите файл для загрузки на сервер:" />
                        </b>
                    </p>
                    <asp:FileUpload ID="MyFileUpload" Width="90%" runat="server" />
                    <br />
                    <br />
                    <uc2:BlueButton ID="Upload_Button" Text="Отправить" runat="server" BtnWidth="150" />
                    <br />
                    <asp:DropDownList runat="server" ID="SelectPLFDriver" Width="100%" Visible="false"
                        OnSelectedIndexChanged="Upload_PLFFile" AutoPostBack="true" />
                    <asp:Panel runat="server" ID="createDriverPanel" Visible="false">
                        <asp:Label ID="Label8" runat="server" Text="Имя водителя: " Width="30%" />
                        <asp:TextBox runat="server" ID="CreateDriversName" Width="50%" />
                        <asp:Label ID="Label9" runat="server" Text="Фамилия водителя: " Width="40%" />
                        <asp:TextBox runat="server" ID="CreateDriversSurname" Width="50%" />
                        <asp:Label ID="Label10" runat="server" Text="Номер карты: " Width="30%" />
                        <asp:TextBox runat="server" ID="CreateDriversNumber" Width="50%" />
                        <asp:Button ID="CreateDriver" runat="server" Text="Создать" OnClick="CreateDriverClick" />
                        <asp:Button ID="CancelCreateDriver" runat="server" Text="Отмена" OnClick="CancelCreateDriverClick" />
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="SelectPLFDriver" />
                    <asp:PostBackTrigger ControlID="CreateDriver" />
                    <asp:AsyncPostBackTrigger ControlID="CancelCreateDriver" EventName="Click" />
                    <asp:PostBackTrigger ControlID="Upload_Button" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <h3>
            <asp:LinkButton ID="AccordionHeader2_UserBackUp" runat="server" PostBackUrl="#" OnClientClick="onAccordionSelectedIndexChanged(1);"
                Text="Восстановить у пользователя" />
        </h3>
        <div>
            <!--<asp:UpdatePanel ID="UserFileRecover_UpdatePanel" runat="server" UpdateMode="Always"
                Visible="false">
                <ContentTemplate>
                    <asp:Panel ID="UserFileRecover_Panel" Style="border-radius: 10px; -moz-border-radius: 10px;
                        -webkit-border-radius: 10px;" runat="server" BorderWidth="1px" BorderColor="LightGray">
                        <asp:TreeView ID="UserFileRecover_TreeView" runat="server" ShowCheckBoxes="None"
                            Width="100%" SelectedNodeStyle-ForeColor="Firebrick" ForeColor="Black" HoverNodeStyle-ForeColor="Firebrick"
                            RootNodeStyle-Font-Bold="true" ShowLines="true" OnSelectedNodeChanged="UserFileRecoverTree_SelectedNodeChanged" />
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ManagmentLoadedInfo" EventName="SelectedNodeChanged" />
                </Triggers>
            </asp:UpdatePanel>-->
         <!--Дерево-->
            <div>
                <ul id="tree">
                    <li class="folder"><a><span key="Drivers">Водители</span></a>
                        <ul id="DriversTree">
                        </ul>
                    </li>
                    <li class="folder"><a><span key="Transport">Транспортные средства</span></a>
                        <ul id="TransportTree">
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <h3>
            <asp:LinkButton ID="AccordionHeader3_Driver" runat="server" PostBackUrl="#" OnClientClick="onAccordionSelectedIndexChanged(2);"
                Text="Просмотреть(Водитель)" /></h3>
        <div>
            <!--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Table runat="server" GridLines="None" Width="100%">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Panel ID="DriversSelectTreeBorderPanel" Style="border-radius: 10px; -moz-border-radius: 10px;
                                    -webkit-border-radius: 10px;" runat="server" BorderWidth="1px" BorderColor="LightGray">
                                    <asp:TreeView ID="DriversSelectTree" ShowCheckBoxes="None" runat="server" Width="100%"
                                        SelectedNodeStyle-ForeColor="Firebrick" ForeColor="Black" Font-Bold="true" OnSelectedNodeChanged="DriversSelectTree_NodeChanged" />
                                </asp:Panel>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <uc2:BlueButton ID="ShowGroupContent" Visible="false" runat="server" Text="Статистика загруженных файлов"
                                    OnClientClick="$('#dialog2').dialog({autoOpen: true, draggable: true, resizable: true, modal:true, width:640 });" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:CheckBox ID="ViewPLF" runat="server" Text="Показать статистику PLF файлов" onchange="checkBoxUdpdated();" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DriversSelectTree" EventName="SelectedNodeChanged" />
                </Triggers>
            </asp:UpdatePanel> -->
            <!--Дерево-->
            <div>
                <ul id="OverlookDriverTree">
                </ul>
            </div>
        </div>

        <h3>
        <asp:LinkButton ID="AccordionHeader4_Vehicle" runat="server" PostBackUrl="#" OnClientClick="onAccordionSelectedIndexChanged(3);"
            Text="Просмотреть(ТС)" /></h3>
        <div>
        <!--<asp:UpdatePanel ID="UpdateP2" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Table runat="server" GridLines="None" Width="100%">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Panel ID="VehiclesSelectTreeBorderPanel" Style="border-radius: 10px; -moz-border-radius: 10px;
                                -webkit-border-radius: 10px;" runat="server" BorderWidth="1px" BorderColor="LightGray">
                                <asp:TreeView ID="VehiclesSelectTree" ShowCheckBoxes="None" runat="server" Width="100%"
                                    SelectedNodeStyle-ForeColor="Firebrick" ForeColor="Black" Font-Bold="true" OnSelectedNodeChanged="VehiclesSelectTree_NodeChanged" />
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <uc2:BlueButton ID="ShowVehiclesGroupContent" Visible="false" runat="server" Text="Статистика загруженных файлов"
                                OnClientClick="$('#dialog2').dialog({autoOpen: true, draggable: true, resizable: true, modal:true, width:640 });" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="VehiclesSelectTree" EventName="SelectedNodeChanged" />
            </Triggers>
        </asp:UpdatePanel>-->
        <!--Дерево-->
            <div>
                <ul id="OverlookVehicleTree">
                </ul>
            </div>
        </div>
        <!--<h3>
        <asp:LinkButton ID="AccordionHeader5_LOadedInfo" runat="server" PostBackUrl="#" OnClientClick="onAccordionSelectedIndexChanged(4);"
                Text="Управление загруженной информацией" /></h3>
        <div>
            <asp:UpdatePanel ID="ManagmentTreePanel" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Panel ID="Panel1" Style="border-radius: 10px; -moz-border-radius: 10px; -webkit-border-radius: 10px;"
                        runat="server" BorderWidth="1px" BorderColor="LightGray">
                        <asp:TreeView ID="ManagmentLoadedInfo" runat="server" ShowCheckBoxes="None" Width="100%"
                            SelectedNodeStyle-ForeColor="Firebrick" ForeColor="Black" HoverNodeStyle-ForeColor="Firebrick"
                            RootNodeStyle-Font-Bold="true" ShowLines="true" OnSelectedNodeChanged="ManagmentLoadedInfo_SelectedNodeChanged" />
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ManagmentLoadedInfo" EventName="SelectedNodeChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <br />
            <br />
            <br />
            <div class="button">
                <asp:LinkButton ID="Button1" runat="server" OnClick="LoadDeleteTable" Text="Удаление файлов"
                    Width="150px" />
            </div>
        </div>-->
    </div>
</asp:Content>
<asp:Content ID="ChoisesContent" ContentPlaceHolderID="MainConditions_PlaceHolder"
    runat="server">
    <asp:Label ID="ChoisesLabel" Text="" runat="server" />
    <asp:UpdatePanel ID="DriversCardEditButtonsPanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Table runat="server" Width="100%" Height="50px">
                <asp:TableRow>
                    <asp:TableCell Width="70%" HorizontalAlign="Left" VerticalAlign="Middle">
                        <asp:Button ID="DriversNameChangeButton" Text="Изменить имя водителя" OnClick="ChangeDriversNameClick"
                            runat="server" Width="30%" Height="30px" />
                        <asp:Button ID="DriversNumberChangeButton" Text="Изменить номер водителя" OnClick="ChangeDriversNumberClick"
                            runat="server" Width="30%" Height="30px" />
                        <asp:Button ID="DeleteDriversCardButton" Text="Удалить водителя" OnClick="DeleteDriversCardWithAllInfo"
                            runat="server" Width="30%" Height="30px" />
                        <asp:ConfirmButtonExtender ConfirmText="Это удалит всю информацию, связанную с этим водителем. Вы уверены?"
                            runat="server" TargetControlID="DeleteDriversCardButton" ID="ConfirmButtonExtender1" />
                    </asp:TableCell><asp:TableCell>
                        <h4>
                            <asp:Label ID="SelectedManagmentDriver" runat="server" /></h4>
                    </asp:TableCell></asp:TableRow>
            </asp:Table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="AdditionalChoisesContent" ContentPlaceHolderID="AdditionalConditions_PlaceHolder"
    runat="server">
    <asp:UpdatePanel ID="DriverCardEditFormsPanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Panel ID="DriverCardEditUpdatePanel_NameEdit" runat="server" Width="100%">
                <h3>
                    <asp:Label runat="server" Text="Изменение имени водителя" /></h3>
                Имя водителя:
                <asp:TextBox runat="server" ID="new_DriversName" Width="20%" />
                Фамилия водителя:
                <asp:TextBox runat="server" ID="new_DriversSurName" Width="20%" />
                <asp:ImageButton ID="ChangeDrName_OK" runat="server" OnClick="makeChangeDriversNameAndSurName"
                    ToolTip="Применить" ImageUrl="~/images/icons/button_ok.png" Width="3%" />
                <asp:ImageButton ID="ChangeDrName_Cancel" runat="server" OnClick="makeChangeDriversInfo_cancel"
                    ToolTip="Отмена" ImageUrl="~/images/icons/button_cancel.png" Width="3%" />
            </asp:Panel>
            <asp:Panel ID="DriverCardEditUpdatePanel_NumberEdit" runat="server" Width="100%">
                <h3>
                    <asp:Label runat="server" Text="Изменение номера карты водителя" /></h3>
                Номер карты водителя:
                <asp:TextBox runat="server" ID="new_DriversCardNumber" Width="30%" />
                <asp:ImageButton ID="ChangeDrNumber_OK" runat="server" OnClick="makeChangeDriversNumber"
                    ToolTip="Применить" ImageUrl="~/images/icons/button_ok.png" Width="3%" />
                <asp:ImageButton ID="ChangeDrNumber_Cancel" runat="server" OnClick="makeChangeDriversInfo_cancel"
                    ToolTip="Отмена" ImageUrl="~/images/icons/button_cancel.png" Width="3%" />
            </asp:Panel>
            <br />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ChangeDrName_OK" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ChangeDrNumber_OK" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="DataContent" ContentPlaceHolderID="Reports_PlaceHolder" runat="server">
    <asp:UpdatePanel ID="DataStatisticsUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:LoadDataStatistics ID="LoadDataStatistics1" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <!--<table id="table-remote">
    </table>-->
    <div id="contentTableWrapper">
        <table id="contentTable" style="border-collapse: separate;" class="wijmo-wijgrid-root wijmo-wijgrid-table"
            border="0" cellpadding="0" cellspacing="0">
            <thead id="contentTableHeader">
            </thead>
            <tbody id="contentTableBody" class="ui-widget-content wijmo-wijgrid-data">
            </tbody>
        </table>
    </div>
    <asp:UpdatePanel ID="GridPanel" UpdateMode="Always" runat="server" Visible="false">
        <ContentTemplate>
            <asp:Panel runat="server" ID="AddGridPanel" CssClass="ui-jqgrid">
                <asp:DataGrid ID="AddGrid" Width="100%" runat="server" OnItemDataBound="AddGrid_ItemDataBound"
                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                    CellSpacing="0" CellPadding="3" rules="all" BorderColor="#CCC" border="0" OnItemCommand="AddGridCommand">
                    <HeaderStyle BackColor="#778899"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:UpdatePanel ID="RemoveDataUpdatePanel" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="RemoveDataId" CommandName="RemoveData" ImageUrl="../images/icons/X.png"
                                            Width="20" runat="server" CausesValidation="false" />
                                        <asp:ConfirmButtonExtender ID="cbe" runat="server" TargetControlID="RemoveDataId"
                                            ConfirmText="Вы уверены ?" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="RemoveDataId" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="ShowDriverButton" CommandName="ViewData" ImageUrl="../images/icons/preview.png"
                                    Width="20" runat="server" CausesValidation="false" OnClientClick="showModal();" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </asp:Panel>
            <br />
            <br />
            <h3>
                <asp:Label ID="Label5" runat="server" /></h3>
            <uc2:BlueButton ID="Parse_Button" Text="Разобрать файлы" runat="server" BtnWidth="150"
                OnClientClick="showModal();" />
            <asp:HiddenField ID="onlyForInternal" runat="server" />
            <asp:Panel runat="server" ID="DriverPreviewButtonsPanel" Visible="false" Width="100%">
                <asp:Table ID="DriverPreviewTable" runat="server" Width="100%">
                    <asp:TableRow>
                        <asp:TableCell>
                            <uc2:BlueButton ID="ShowStatistics_btn" Visible="false" runat="server" Text="Статистика выбранного файла"
                                OnClientClick="$('#dialog2').dialog({autoOpen: true, draggable: true, resizable: true, modal:true, width:640 });" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="16%">
                            <uc2:BlueButton ID="DriverFileContent_btn" runat="server" Text="File Contents" OnClientClick="showModal();"
                                Enabled="false" />
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="16%">
                            <uc2:BlueButton ID="Aplication_Identification_btn" runat="server" Text="Aplication Identification"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell Width="16%">
                            <uc2:BlueButton ID="ICC_btn" runat="server" Text="ICC" OnClientClick="showModal();"
                                Enabled="true" />
                        </asp:TableCell><asp:TableCell Width="16%">
                            <uc2:BlueButton ID="IC_btn" runat="server" Text="IC" OnClientClick="showModal();"
                                Enabled="true" />
                        </asp:TableCell><asp:TableCell Width="16%">
                            <uc2:BlueButton ID="CardIdentification_btn" runat="server" Text="CardIdentification"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell Width="16%">
                            <uc2:BlueButton ID="CardDownload_btn" runat="server" Text="CardDownload" OnClientClick="showModal();"
                                Enabled="true" />
                        </asp:TableCell><asp:TableCell Width="16%">
                            <uc2:BlueButton ID="DrivingLicenceInfo_btn" runat="server" Text="DrivingLicenceInfo"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <uc2:BlueButton ID="Events_btn" runat="server" Text="Events" OnClientClick="showModal();"
                                Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="Faults_btn" runat="server" Text="Faults" OnClientClick="showModal();"
                                Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="Places_btn" runat="server" Text="Places" OnClientClick="showModal();"
                                Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="Current_Usage_btn" runat="server" Text="Current Usage" OClientClick="showModal();"
                                Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="ControlActivityData_btn" runat="server" Text="ControlActivityData"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="Specific_Conditions_btn" runat="server" Text="Specific Conditions"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </asp:Panel>
            <asp:Panel runat="server" ID="VehiclePreviewButtonsPanel" Visible="false" Width="100%">
                <asp:Table ID="VehiclePreviewButtonsTable" runat="server" Width="100%">
                    <asp:TableRow>
                        <asp:TableCell Width="16.5%">
                            <uc2:BlueButton ID="VehicleFileContent_btn" runat="server" Text="File Contents" OnClientClick="showModal();"
                                Enabled="false" />
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="16.5%">
                            <uc2:BlueButton ID="VehicleIdentification_btn" runat="server" Text="Vehicle Identification"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell Width="16.5%">
                            <uc2:BlueButton ID="VehicleCurrentDateTime_btn" runat="server" Text="CurrentDateTime"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell Width="16.5%">
                            <uc2:BlueButton ID="VehicleDownloadablePeriod_btn" runat="server" Text="Downloadable period"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell Width="16.5%">
                            <uc2:BlueButton ID="VehicleInsertedCardType" runat="server" Text="Inserted card type"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell Width="16.5%">
                            <uc2:BlueButton ID="VehicleDownloadActivityData_btn" runat="server" Text="Download activity data"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell Width="16.5%">
                            <uc2:BlueButton ID="VehicleCompanyLocksData_btn" runat="server" Text="Company locks data"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <uc2:BlueButton ID="VehicleControlActivityData_btn" runat="server" Text="Control activity data"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="VehicleEventData_btn" runat="server" Text="Event data" OnClientClick="showModal();"
                                Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="VehicleFaultData_btn" runat="server" Text="Fault data" OnClientClick="showModal();"
                                Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="VehicleOverSpeedingControlData_btn" runat="server" Text="Overspeeding control data"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="VehicleOverSpeedingEventData_btn" runat="server" Text="Overspeeding event data"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="VehicleTimeAdjustmentData_btn" runat="server" Text="Time adjustment data"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <uc2:BlueButton ID="VehicleDetailedSpeedData_btn" runat="server" Text="Detailed speed"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="VehicleFullIdentification_btn" runat="server" Text="Identification"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="VehicleSensorPaired_btn" runat="server" Text="Sensor paired"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell><asp:TableCell>
                            <uc2:BlueButton ID="VehicleCalibrationData_btn" runat="server" Text="Calibration data"
                                OnClientClick="showModal();" Enabled="true" />
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </asp:Panel>
            <div style="width: 90%; padding: 25px;">
                <asp:Label ID="TextBoxTest" runat="server" Font-Size="12" Width="100%" />
            </div>
            <asp:Panel ID="FilesPreviewPanel" CssClass="ui-jqgrid" runat="server" Visible="false">
                <asp:GridView ID="FilesPreviewDataGrid" Width="100%" runat="server" AutoGenerateColumns="true"
                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
                    SelectedRowStyle-BackColor="LightBlue" CellSpacing="0" CellPadding="3" BorderColor="#CCC"
                    border="0">
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>
<asp:Content ID="BottomContent1" ContentPlaceHolderID="Bottom_PlaceHolder" runat="server">
    <asp:UpdatePanel ID="StatusUpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h3>
                <asp:Label ID="Status" runat="server" /></h3>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="dialog2" title="Статистика загруженной информации">
    </div>
</asp:Content>

<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Adminisration_UserControls_LogTab_UserControl, App_Web_tjh32c0f" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Panel if="LogFilterPanel" BackColor="White" runat="server" Width="100%" GroupingText="Фильтр" DefaultButton="ApplyLogFilterButton" >
    <asp:Table ID="LogFilterTable"   GridLines="None" runat="server" Width="100%">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="LogFilterTable_StartDateLabel" runat="server" Font-Size="Small" Text="Начальная дата " />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="LogFilterTable_StartDateTextBox" runat="server" Width="80%"/>
                <asp:CalendarExtender ID="StartDateCalendarExtender" runat="server" TargetControlID="LogFilterTable_StartDateTextBox"
                                Format="d, MMMM, yyyy" Animated="true" />
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="LogFilterTable_EndDateLabel" runat="server" Font-Size="Small" Text="Конечная дата " />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="LogFilterTable_EndDateTextBox" runat="server" Width="80%"/>
                <asp:CalendarExtender ID="EndDateCalendarExtender1" runat="server" TargetControlID="LogFilterTable_EndDateTextBox"
                                Format="d, MMMM, yyyy" Animated="true" />
            </asp:TableCell>
             <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="LogFilterTable_EventLabel" runat="server" Font-Size="Small" Text="Событие" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="LogFilterTable_EventDropDown" runat="server"  Width="200px" />
            </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow>
             <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="LogFilterTable_StartTimeLabel" runat="server" Font-Size="Small" Text="время" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="LogFilterTable_StartTimeTextBox" runat="server" Width="40%"/>
                <asp:MaskedEditExtender ID="StartTime_MaskedEditExtender" 
                    runat="server" 
                    TargetControlID="LogFilterTable_StartTimeTextBox" 
                    Mask="99:99" 
                    MaskType="Time"
                    MessageValidatorTip="true" 
                    OnInvalidCssClass="validatorCalloutHighlight" 
                    AutoComplete="true"
                    AutoCompleteValue="00:00"
                    />
                <asp:MaskedEditValidator ID="StartTime_MaskedEditValidator" 
                    runat="server" 
                    ControlToValidate="LogFilterTable_StartTimeTextBox" 
                    ControlExtender="StartTime_MaskedEditExtender" 
                    Display="None"                     
                    TooltipMessage="Введите время" 
                    IsValidEmpty="true" 
                    EmptyValueMessage="Введите время(ЧЧ:ММ)" 
                    InvalidValueMessage="Время введено неправильно" />
                <asp:ValidatorCalloutExtender ID="StartTime_MaskedEditValidator_validator" runat="server" TargetControlID="StartTime_MaskedEditValidator"
                    HighlightCssClass="validatorCalloutHighlight" />
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="LogFilterTable_EndTimeLabel" runat="server" Font-Size="Small" Text="время" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="LogFilterTable_EndTimeTextBox" runat="server"  Width="40%"/>
                <asp:MaskedEditExtender ID="EndTime_MaskedEditExtender1" 
                    runat="server" 
                    TargetControlID="LogFilterTable_EndTimeTextBox" 
                    Mask="99:99" 
                    MaskType="Time"
                    MessageValidatorTip="true" 
                    OnInvalidCssClass="validatorCalloutHighlight" 
                    AutoComplete="true"
                    AutoCompleteValue="00:00"
                    />
                <asp:MaskedEditValidator ID="EndTime_MaskedEditValidator" 
                    runat="server" 
                    ControlToValidate="LogFilterTable_EndTimeTextBox" 
                    ControlExtender="EndTime_MaskedEditExtender1" 
                    Display="None"                     
                    TooltipMessage="Введите время" 
                    IsValidEmpty="true" 
                    InvalidValueMessage="Время введено неправильно" />
                <asp:ValidatorCalloutExtender ID="EndTime_MaskedEditValidator_validator" runat="server" TargetControlID="EndTime_MaskedEditValidator"
                    HighlightCssClass="validatorCalloutHighlight" />
            </asp:TableCell>
             <asp:TableCell HorizontalAlign="Right">
                <asp:Label ID="LogFilterTable_NoteTextLabel" runat="server" Font-Size="Small" Text="Текст в описании" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="LogFilterTable_NoteTextTextBox" runat="server" Width="200px"  />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="ApplyLogFilterButton" runat="server" Text="Применить" OnClick="ApplyLogFilterButton_Click" />
            </asp:TableCell>
        </asp:TableRow>            
    </asp:Table>
</asp:Panel>
<asp:Panel ID="LogDataPanel" CssClass="ui-jqgrid" runat="server" Width="100%" ScrollBars="None" Height="100%">
    <asp:DataGrid ID="LogDataGrid" runat="server" Width="100%" AutoGenerateColumns="false"
        HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"
        CellSpacing="0" CellPadding="3" rules="all" BorderColor="#CCC" border="0"
        ItemStyle-Font-Size="9">
        <Columns>
            <asp:BoundColumn DataField="USER_ID"  Visible="false" />
            <asp:BoundColumn DataField="ACTION_ID" Visible="false"/>
            <asp:BoundColumn DataField="TABLE_ID" Visible="false"/>
            <asp:BoundColumn DataField="Дата и время" HeaderText="Дата и время"/>
            <asp:BoundColumn DataField="Пользователь" HeaderText="Пользователь"/>
            <asp:BoundColumn DataField="Описание" HeaderText="Описание"/>
        </Columns>
    </asp:DataGrid>
</asp:Panel>

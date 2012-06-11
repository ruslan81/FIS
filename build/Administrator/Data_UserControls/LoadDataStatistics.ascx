<%@ control language="C#" autoeventwireup="true" inherits="Administrator_Data_UserControls_LoadDataStatistics, App_Web_m1b3uc5v" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:UpdatePanel runat="server" ID="DateUpdate" UpdateMode="Conditional">
<ContentTemplate>
 <asp:Panel ID="DatePanel" runat="server" Width="100%" BackColor="#eeeeee">
    <asp:Table ID="MainTable" runat="server" Width="100%" GridLines="None">
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <asp:Table ID="DateTable" runat ="server" Width="100%" Height="100%" GridLines="None">
                    <asp:TableRow>
                        <asp:TableCell VerticalAlign="Top" Width="50%">    
                            <div class="ui-jqgrid">
                                <asp:GridView runat="server" ID="YearsGrid" Width="100%" SelectedIndex="0"
                                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other" SelectedRowStyle-BackColor="LightBlue"
                                    CellSpacing="0" CellPadding="3" BorderColor="#CCC" border="0" OnRowDataBound="RowDataBound" OnSelectedIndexChanged="ReloadStats_Click">
                                </asp:GridView>
                            </div>
                            <br />
                       </asp:TableCell>
                       <asp:TableCell VerticalAlign="Top" Width="50%">
                            <div class="ui-jqgrid">
                                <asp:GridView runat="server" ID="YearsGrid_Values" Width="100%"
                                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"   
                                    CellSpacing="0" CellPadding="3" BorderColor="#CCC" border="0">
                                </asp:GridView>
                            </div>
                       </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <div class="ui-jqgrid">
                                <asp:GridView runat="server" ID="MonthGrid"  Width="100%"  SelectedIndex="0"
                                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other" SelectedRowStyle-BackColor="LightBlue"    
                                    CellSpacing="0" CellPadding="3" BorderColor="#CCC" border="0" OnRowDataBound="RowDataBound" OnSelectedIndexChanged="ReloadStats_Click">
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                        <asp:TableCell>
                            <div class="ui-jqgrid">
                                <asp:GridView runat="server" ID="MonthGrid_Values"  Width="100%" 
                                    HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"    
                                    CellSpacing="0" CellPadding="3" BorderColor="#CCC" border="0">
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>                 
                </asp:Table>
            </asp:TableCell>
            <asp:TableCell Width="40%">
                <div class="ui-jqgrid">
                    <asp:GridView runat="server" ID="DaysGrid" Width="100%"
                        HeaderStyle-CssClass="ui-jqgrid-titlebar" AlternatingItemStyle-CssClass="other"    
                        CellSpacing="0" CellPadding="3" BorderColor="#CCC" border="0">
                    </asp:GridView>
                </div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Label ID="Status" runat="server" ForeColor="Blue" Font-Size="12" />
</asp:Panel> 
</ContentTemplate>
</asp:UpdatePanel>


        <asp:UpdatePanelAnimationExtender ID="upae" BehaviorID="animation" runat="server" TargetControlID="DateUpdate">
            <Animations>
                <OnUpdating>
                    <Sequence>     
                        <Parallel duration="0">
                            <EnableAction AnimationTarget="YearsGrid" Enabled="false" />
                            <EnableAction AnimationTarget="MonthGrid" Enabled="false" />
                            <EnableAction AnimationTarget="YearsGrid_Values" Enabled="false" />
                            <EnableAction AnimationTarget="MonthGrid_Values" Enabled="false" />
                            <EnableAction AnimationTarget="DateTable" Enabled="false" />
                        </Parallel>
                    
                                      
                        <StyleAction Attribute="overflow" Value="hidden" />
                        
                        <%-- Do each of the selected effects --%>
                        <Parallel duration=".50" Fps="30">
                                <FadeOut AnimationTarget="up_container" minimumOpacity=".2" />
                                <Color AnimationTarget="up_container" PropertyKey="backgroundColor"
                                    EndValue="#40669A" StartValue="#FFFFFF" />
                        </Parallel>
                    </Sequence>
                </OnUpdating>
                <OnUpdated>
                    <Sequence>
                        <%-- Do each of the selected effects --%>
                        <Parallel duration=".25" Fps="30">
                                <FadeIn AnimationTarget="up_container" minimumOpacity=".2" />
                                <Color AnimationTarget="up_container" PropertyKey="backgroundColor"
                                    StartValue="#40669A" EndValue="#FFFFFF" />
                        </Parallel>
                        <Parallel duration="0">
                            <EnableAction AnimationTarget="YearsGrid" Enabled="true" />
                            <EnableAction AnimationTarget="MonthGrid" Enabled="true" />
                            <EnableAction AnimationTarget="YearsGrid_Values" Enabled="true" />
                            <EnableAction AnimationTarget="MonthGrid_Values" Enabled="true" />
                            <EnableAction AnimationTarget="DateTable" Enabled="true" />
                        </Parallel>          
                    </Sequence>
                </OnUpdated>
            </Animations>
        </asp:UpdatePanelAnimationExtender>
    

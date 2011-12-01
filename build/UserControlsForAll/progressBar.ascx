<%@ control language="C#" autoeventwireup="true" inherits="UserControlsForAll_progressBar, App_Web_20unmis3" %>

<asp:UpdatePanel ID="ProgressUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>    
        <asp:Table Width="330px" Height="29px" runat="server" ID="ProgressTable" GridLines="Both" BorderWidth="1px">
            <asp:TableRow>
                <asp:TableCell ID="ProgressTableCell_1" Width="30px">            
                </asp:TableCell>
                <asp:TableCell ID="ProgressTableCell_2" Width="30px">            
                </asp:TableCell>
                <asp:TableCell ID="ProgressTableCell_3" Width="30px">            
                </asp:TableCell>
                <asp:TableCell ID="ProgressTableCell_4" Width="30px">            
                </asp:TableCell>
                <asp:TableCell ID="ProgressTableCell_5" Width="30px">            
                </asp:TableCell>
                <asp:TableCell ID="ProgressTableCell_6" Width="30px" HorizontalAlign="Center" VerticalAlign="Middle">    
                    <asp:Label ID="ProgressLabel" runat="server"/>        
                </asp:TableCell>
                <asp:TableCell ID="ProgressTableCell_7" Width="30px">            
                </asp:TableCell>
                <asp:TableCell ID="ProgressTableCell_8" Width="30px">            
                </asp:TableCell>
                <asp:TableCell ID="ProgressTableCell_9" Width="30px">            
                </asp:TableCell>
                <asp:TableCell ID="ProgressTableCell_10" Width="30px">            
                </asp:TableCell>
                <asp:TableCell ID="ProgressTableCell_11" Width="30px">            
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Table Width="330px" Height="9px" runat="server" ID="Table1" GridLines="Both" BorderWidth="1px">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle">
                    <asp:Label ID="TextResultLabel" runat="server" Font-Size="XX-Small"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <asp:Timer ID="Timer1" runat="server" OnTick="TimerTick" Interval="300" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
    </Triggers>
</asp:UpdatePanel>
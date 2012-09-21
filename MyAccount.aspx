<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="MyAccount.aspx.cs" Inherits="MyAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 142px;
            height: 110px;
        }
        .style4
        {
            width: 766px;
        }
        .style5
        {
            width: 425px;
        }
        .style8
        {
            clear: both;
            width: 143px;
        }
        .style9
        {
            width: 762px;
        }
        .style10
        {
            width: 312px;
        }
        .style7
        {
            width: 81px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="vertical-align:middle">
        <tr>
                 <td>
        <img alt="myaccount" class="style2" longdesc="my account image" 
            src="images/acount.jpg" /></td>
                  <td class="style4">
                      <asp:Label ID="accountlbl" runat="server"  font-size="16px"
            Text="Label" CssClass="bold"></asp:Label>
                      <br />
                      <br />
                      <br />
                      <br />
                      <br />
                 </td>
        </tr>
        </table>
    <p>
       
    </p>
      <table  runat="server"  id="table1" style="vertical-align:middle; width: 915px;"   >
        <tr>
                 <td style="font-size: medium" class="style5"   colspan = "2" align="center" >
                     Account Details</td>
                
                 
        </tr>
          <tr>
                 <td class="style8" style="font-size: medium"  >
                    &nbsp;Contacts:</td>
                      <td class="style9" id="Cell1">
                   </td>
                 
        </tr>
                <tr>
                 <td class="style8" style="font-size: medium" >
                    Account Created:</td>
                      <td class="style9" id="Cell2">
                   </td>
                 
        </tr>
                        <tr>
                 <td class="style8" style="font-size: medium" >
                    Email:</td>
                      <td class="style9" id="Cell3">
                   </td>
                 
        </tr>
        </table>

       
    
      <table style="width: 912px">
    
        <tr>
        
                 <td class="style7"  align="center">  <a  href="Default.aspx"> 
           Home</a></td>

           
        </tr>
        
        </table>
       
           <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ApplicationServices %>" 
                SelectCommand="SELECT [CreateDate], [Email], [UserName] FROM [vw_aspnet_MembershipUsers] WHERE ([UserName] = @UserName)" 
        >
               <SelectParameters>
                   <asp:SessionParameter Name="UserName" SessionField="userid" Type="String" />
               </SelectParameters>

            </asp:SqlDataSource>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Visible = "false"
        DataSourceID="SqlDataSource1"  OnDataBinding="gv3_databound"
          >
        <Columns>
            <asp:BoundField DataField="CreateDate" HeaderText="CreateDate" 
                SortExpression="CreateDate" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="UserName" HeaderText="UserName" 
                SortExpression="UserName" />
        </Columns>
    </asp:GridView>
</asp:Content>


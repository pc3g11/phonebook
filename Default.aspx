<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 724px;
            height: 233px;
        }
        .style2
        {
            width: 914px;
            height: 252px;
        }
        .style7
        {
            width: 81px;
        }
        .style9
        {
            width: 309px;
        }
        .style10
        {
            width: 312px;
        }
        .style11
        {
            width: 190px;
            height: 138px;
        }
        .style12
        {
            width: 190px;
            height: 138px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <p>
        &nbsp;</p>
    <table >
        <tr>
            <td align="center">
                <img alt="civic center" class="style2" longdesc="civic center image"  src="images/civic3.gif" /></td>
        </tr>
        </table>
   
    
    <table>
    <tr>
    <td> </td> 
    <td> <h3> Home </h3> </td>
    <td> </td> 
    </tr>
    <tr>
     <td ></td>
     <td >Welcome to sotonphonebook, the Southampton&#39;s phone book website that can be the 
        private phone catalogue for any citizen in Southampton or for anyone who wants 
        an easy search application for people that are related to Southampton</td>
     <td ></td>
    </tr>
    <tr>
    <td></td>
    <td><p></p></td>
    <td ></td>

    </tr>
      </table>
      
       
    
      <table>
        <tr>
        <td class="style10"></td>
        <td>
        <a  href="Search.aspx"> 
            <img alt="search" class="style11" 
                src="images/global-search-icon1.jpg"  border="1" /> </a></td>
        <td class="style7">&nbsp;&nbsp;&nbsp;</td>
        <td>
         <a  id="myacountimg" href="Search.aspx" runat="server"> 
            <img alt="account" class="style12" src="images/acount.jpg" 
                border="1" /> </a></td>
        <td class="style9"></td>
        </tr>
        <tr>
          <td class="style10"></td>
        <td class="style7"  align="center">  <a  href="Search.aspx"> 
           Search Now </a></td>
        <td class="style7"></td>
        <td align="center">  <a  id="myacount" href="Search.aspx" runat="server">  My Account </a> </td>
        <td class="style9"></td>
        </tr>
        
        </table>
       
                 <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ApplicationServices %>" 
                SelectCommand="SELECT [UserName] FROM [vw_aspnet_Users]" 
        >

            </asp:SqlDataSource>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  Visible = "false"
        DataSourceID="SqlDataSource1"  
          >
        <Columns>
            <asp:BoundField DataField="UserName" HeaderText="UserName" 
                SortExpression="UserName" />
        </Columns>
    </asp:GridView>

</asp:Content>

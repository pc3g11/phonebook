<%@ Page Title="tests" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
CodeFile="Search.aspx.cs" Inherits="Default3" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .Panel
        {}
        .style5
        {
            width: 55px;
        }
        .style8
        {
            width: 89px;
        }
        .style9
        {
            width: 98px;
        }
        .style14
        {
            height: 21px;
        }
          
       .hide
    {
     display:none;
    }

    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[

       
// ]]>
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    

    <h2><asp:Label ID="Label2" runat="server" Text="Public Phonebook Search"></asp:Label>    </h2>
   
    <asp:Panel ID="PhoneBook" runat="server" CssClass="Panel" Width="922px"  
        GroupingText="Phone Book" HorizontalAlign="Center">

          <asp:Panel ID="PrivatePanel" runat="server"  GroupingText="Private Data"  
              Width="869px" >

            <asp:GridView ID="GridView2" runat="server"   AllowPaging="True"     OnRowUpdating = "GV2_RowUpdating"
                PageSize = "10" HtmlEncodeFormatString="false" AutoGenerateColumns ="False" OnRowCancelingEdit="GV2_RowCancelingEdit"
            AutoPostBack="FALSE"  OnDataBound = "gv2DataBound"  Width="873px"   OnRowEditing="GV2_RowEditing"  OnRowDataBound = "GV2_RowDataBound"
                OnRowCommand = "GV_RowCommand"   OnPageIndexChanging="gv2Paging_PageIndexChanging"
                >

            <Columns>
             <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" ReadOnly="True"/>
              <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" ReadOnly="True"/>
              <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Surname" ReadOnly="True" />
             <asp:BoundField DataField="E-mail" DataFormatString="<a href=mailto:{0}>{0}</a>" HtmlEncodeFormatString="false" HeaderText="E-mail" SortExpression="E-mail" ReadOnly="True" />            
             <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" ReadOnly="True" />

             <asp:BoundField DataField="Faculty" HeaderText="Faculty" SortExpression="Faculty"   Visible="false"  />
                       
            <asp:TemplateField HeaderText="Faculty">
            <ItemTemplate>
            <asp:Label ID="lblText" runat="server" CssClass="Label" 
            Text='<%# FacultyShort(DataBinder.Eval(Container.DataItem, "Faculty")) %>'  
            Width="100px"  ></asp:Label>
            <asp:HiddenField ID="HFText" runat="server" value='<%# Bind("Faculty") %>'/>
            </ItemTemplate>
            <HeaderStyle CssClass="LabelTitleGrid" Width="100px"  />
            <ItemStyle Width="100px" />
            </asp:TemplateField>

              <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments"  />
             <asp:TemplateField HeaderText="Comments">
                <ItemTemplate>
                    <%# Eval("Comments") %>
                </ItemTemplate>
                <EditItemTemplate>
                   
                </EditItemTemplate>
            </asp:TemplateField>


            <asp:TemplateField HeaderText="Edit">
                <ItemTemplate>
                    
                    <asp:ImageButton runat="server" ID="btnEdit" height="25px" Width ="25px"  ImageUrl="images\update4_25x25.gif"  CommandName="Edit"  ToolTip="Edit Comments"/>
                    <asp:ImageButton runat="server" ID="btnDeleteFavor" height="25px" Width ="25px"  ImageUrl="images\Delete_25x25.gif"   onclick="RemoveFavorClick" CommandName="RemoveRow"  ToolTip="Delete Contact"/>
                    <asp:ImageButton runat="server" ID="btnExport" height="25px" Width ="25px"  ImageUrl="images\export-icon.gif"   onclick="ExportFavorClick" CommandName="ExportRow"  ToolTip="Export Contact as VCard"/>

                   
                </ItemTemplate>
                <EditItemTemplate>
                          <asp:Button  ID="btnUpdate" runat="server" CommandName="Update" tabIndex="2" Text="Update"   onclick="UpdateRDFClick"   Width="50px"/>
                           <asp:Button  ID="btnCancel" runat="server" CommandName="Cancel" tabIndex="2" Text="Cancel"     Width="50px"/>
                </EditItemTemplate> 
                
            </asp:TemplateField>

                <asp:BoundField DataField="dataid" HeaderText="dataid" SortExpression="dataid" > <HeaderStyle CssClass="hide"  /><ItemStyle CssClass="hide"/></asp:BoundField>
            </Columns>

        </asp:GridView>

          </asp:Panel >
          <asp:Panel ID="PSearch" runat="server" CssClass="Panel" GroupingText="Search" 
              Width="869px" >
                <table style="vertical-align:middle">

                 <tr>
                 <td align="right" style="width:100px">
                            <asp:Label ID="Label3" runat="server" CssClass="Label" Text="Title:"></asp:Label>
                        </td>
                        <td align="left">
                            <input id="titlepicker" type="text" title="Select Title" runat="server"  style="width:83px; text-align:center;"  />
                        </td>

                        <td align="right" class="style5">
                            <asp:Label ID="lblName" runat="server" CssClass="Label" Text="Name:"></asp:Label>
                        </td>
                        <td align="left" style="width:100px">
                            <input id="namepicker" type="text" title="Select Name" runat="server"  style="width:180px; text-align:center;"  />
                        </td>
                        
                         <td align="right" class="style9">
                            <asp:Label ID="Label1" runat="server" CssClass="Label" Text="Surname:"></asp:Label>
                        </td>
                        <td align="left" style="width:100px">
                            <input id="surnamepicker" type="text" title="Select Surname" runat="server"  style="width:180px; text-align:center;" />
                        </td>
                     </tr>
                     <tr align="center">
                     <td>
                         <asp:Label ID="Label6" runat="server" CssClass="Label" Text="PhoneNum:"></asp:Label>
                     </td>
                      <td>
                          <input id="numberpicker" type="text" title="Select PhoneNumber" 
                              runat="server"  style="width:83px; text-align:center;"  />
                     </td>
                       <td align="right" class="style5">
                            <asp:Label ID="Label4" runat="server" CssClass="Label" Text="Email:"></asp:Label>
                        </td>
                        <td align="left" style="width:100px">
                            <input id="emailpicker" type="text" title="Select Email" runat="server"  style="width:180px; text-align:center;"  />
                        </td>
                     
                   
                   
                      <td align="right" class="style5">
                            Faculty:</td>
                        <td align="left" style="width:100px">
                            <input id="facultypicker" type="text" title="Select Faculty" 
                                runat="server"  style="width:180px; text-align:center;" /></td>
                         <td align="center" class="style8" >
                            <asp:Button ID="btnFind" runat="server" CommandName="Find" onclick="BtnClick" 
                                tabIndex="2" Text="Find" ValidationGroup="Find" Width="75px" />
                            </td>
                            <td>
                            <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Clear" 
                                 Width="75px" />
                        </td>
                    </tr>
                </table>

        </asp:Panel>

        <asp:GridView ID="GridView1" runat="server"   AllowPaging="True"    PageSize = "10" HtmlEncodeFormatString="false" AutoGenerateColumns ="false"
            AutoPostBack="false"  OnDataBound = "gvDataBound"  Width="873px"  OnRowCommand = "GV_RowCommand" OnRowDataBound="GV1_RowDataBound"   
            OnPageIndexChanging="gv1Paging_PageIndexChanging">

            <Columns>
             <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
              <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
              <asp:BoundField DataField="Surname" HeaderText="Surname" SortExpression="Surname" />
             <asp:BoundField DataField="E-mail" DataFormatString="<a href=mailto:{0}>{0}</a>" HtmlEncodeFormatString="false" HeaderText="E-mail" SortExpression="E-mail" />
              <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
                 <asp:BoundField DataField="Faculty" HeaderText="Faculty" SortExpression="Faculty" />
                <asp:TemplateField  HeaderText="Favourites"   >
                          <ItemTemplate>
                          <asp:Button  ID="btnAddFavor" runat="server" CommandName="AddFavor" onclick="AddFavorClick"   
                                tabIndex="2" Text="Add"   Width="50px"/>
                          </ItemTemplate>
                          <HeaderStyle CssClass="LabelTitleGrid" Width="75px" />
                         <ItemStyle Width="75px" />
   
                         </asp:TemplateField>
                       
            <asp:BoundField DataField="dataid" HeaderText="dataid" SortExpression="dataid" > <HeaderStyle CssClass="hide"  /><ItemStyle CssClass="hide"/></asp:BoundField>
             
        
            </Columns>

        </asp:GridView>
          
        
        
     </asp:Panel>

    <table style="width: 912px">
    
        <tr>
         
                 <td class="style14"  align="center">  <a  href="Default.aspx"> 
           Home</a></td>

           
        </tr>
        
        </table>
</asp:Content>

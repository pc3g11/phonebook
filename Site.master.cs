using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using VDS.RDF.Writing.Formatting;
using System.Text;
using VDS.RDF.Writing;


public partial class SiteMaster : System.Web.UI.MasterPage
{
    public string path_user = "C:/Users/panayiotis/master/MSC PROJECT/db/users/";
    public string path_public = "C:/Users/panayiotis/master/MSC PROJECT/db/public.rdf";

    protected void Page_Load(object sender, EventArgs e)
    {

        var userName = Page.User.Identity.Name;
        Session["UserId"] = userName;


        if (userName != "")
        {

            CheckIfFileExists(userName);
          
        }
        else
        {
            return;
          
            /*
            Graph g = new Graph();
            FileLoader.Load(g, "C:/Users/panayiotis/master/MSC PROJECT/db/HelloWorld.rdf"+userName+".rdf";
            Session["ggraph"] = g;*/
           
        }
      
    }
    protected void NavigationMenu_MenuItemClick(object sender, MenuEventArgs e)
    {
 
    
        // Display the text of the menu item selected by the user.
        if (e.Item.Text.Equals("MyAccount"))
        {
            if (Session["UserId"] != "")
                
            Response.Redirect("~/MyAccount.aspx");
            else
            Response.Redirect("Account/Login.aspx");

        }else if(e.Item.Text.Equals("Home"))
             Response.Redirect("~/Default.aspx");
        else if(e.Item.Text.Equals("About"))
         Response.Redirect("~/About.aspx");
        else if(e.Item.Text.Equals("Search"))
            Response.Redirect("~/Search.aspx");
      /*    NavigateUrl="~/Default.aspx"
              NavigateUrl="~/MyAccount.aspx"
                  NavigateUrl="~/About.aspx"
                       NavigateUrl="~/Search.aspx"*/
    }

    protected void CheckIfFileExists(string filename)
    {
        string curFile = @path_user + filename+".rdf";
        if (File.Exists(curFile) == false)
            createrdf(filename);
        else
        {
            Graph g = new Graph();
            FileLoader.Load(g, path_user + filename + ".rdf");
            Session["ggraph"] = g;
        }
    }

    protected void createrdf(string filename)
    {
        Graph g = new Graph();
        Session["ggraph"] = g;
        RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
        rdfxmlwriter.Save(g, path_user + filename + ".rdf");
    }
    

}

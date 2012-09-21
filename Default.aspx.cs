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
public partial class _Default : System.Web.UI.Page
{
    public  string path_user = "C:/Users/panayiotis/master/MSC PROJECT/db/users/";
    public  string path_public = "C:/Users/panayiotis/master/MSC PROJECT/db/public.rdf";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session["userflag"] = 1;
        }

        var userName = Page.User.Identity.Name;
        Session["UserId"] = userName;
        Session["check"] = 0;
       // checkuser();
        publicrdf();
        if (userName != "")
        {
            myacount.HRef = "MyAccount.aspx";
            myacountimg.HRef = "MyAccount.aspx";
        }
        else
        {
            myacount.HRef = "Account/Login.aspx";
            myacountimg.HRef = "Account/Login.aspx";
        }
       
    }
    
    protected void Button1_Click(object sender, EventArgs e)
    {
       
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("About.aspx");
    }
    protected void publicrdf()
    {
        int flag = Convert.ToInt32(Session["userflag"].ToString());
        string userid = Session["UserId"].ToString();
        if (userid != "")
        {
         //ele3e an iparxi to public arxio,
            checkifpublicexists();
            //an den iparxi kameto
            //an iparxi kameto load
          // System.Diagnostics.Debug.WriteLine("1. " );


            LoadPersonalData();
       //an flag ==1
            //kame load ta data
        //ele3e an to username iparxi
            if (checkuser() == 0)
            {
                addUserInRDF();
                Session["userflag"] = 0;
            }
            else
            {
                Session["userflag"]  = 0;
            }
            //an den iparxi varto
                //flag =0
            //an iparxi tpt
                //flag =0
          //  System.Diagnostics.Debug.WriteLine("user=" + userid);
        }
        else
        {
            return;
        }
    }
    protected void LoadPersonalData()
    {
        Graph g = new Graph();
        FileLoader.Load(g, path_public);
        List<string[]> PersonalList = new List<string[]>();

        string[] temptaple = new string[3];
        //Iterate over and print Triples

        foreach (Triple t in g.Triples)
        {
            string[] personaldata = new string[3];

            temptaple = t.ToString().Split(',');

            for (int cv01 = 0; cv01 <= 2; cv01++)
            {
                //    personaldata[cv01] = temptaple[cv01];//.Replace(" ", string.Empty); 
                if (cv01 == 0)
                    personaldata[cv01] = temptaple[cv01].Remove(temptaple[cv01].Length - 1, 1); //.Replace(" ", string.Empty); 
                else if (cv01 == 1)
                {
                    string temp = temptaple[cv01].Remove(temptaple[cv01].Length - 1, 1); //.Replace(" ", string.Empty); 
                    personaldata[cv01] = temp.Remove(0, 1); //.Replace(" ", string.Empty); 
                }

                else
                    personaldata[cv01] = temptaple[cv01].Remove(0, 1);
                //personaldata[cv01] = temptaple[cv01];//.Replace(" ", string.Empty); 
            }
            PersonalList.Add(personaldata);

            // System.Diagnostics.Debug.WriteLine(""+ cv011 + " " + t.ToString());
        }
        Session["UserList"] = PersonalList;
         printinconsoletest(PersonalList);
    }
    protected void checkifpublicexists()
    {

        string curFile = @path_public;
        if (File.Exists(curFile) == false)
        {
            createPublic();
        }
        else
        {
            Graph g = new Graph();
            FileLoader.Load(g, path_public);
            Session["ugraph"] = g;
            return;
        }
    }
    private void printinconsoletest(List<string[]> list)
    {
        System.Diagnostics.Debug.WriteLine("printing..");
        for (int cv03 = 0; cv03 < list.Count; cv03++)
        {

            //RDFDATA =6
            for (int cv04 = 0; cv04 < 3; cv04++)
            {
                System.Diagnostics.Debug.WriteLine(" --|" + list[cv03][cv04] + "|--");

            }
        }
    }
    protected void createPublic()
    {
        Graph g = new Graph();
        Session["ugraph"] = g;
        RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
        rdfxmlwriter.Save(g, path_public);
    }

    protected void addUserInRDF()
    {
        var list = (List<string[]>)Session["UserList"];

        int numberOfRows = 0;

        if (list.Count() > 0)
            numberOfRows = int.Parse(list[list.Count() - 1][0].ToString().Split('/')[4].ToString());

        for (int cv01 = 0; cv01 < list.Count; cv01++)
        {
           if (numberOfRows < int.Parse(list[cv01][0].ToString().Split('/')[4].ToString()))
           {
            numberOfRows =  int.Parse(list[cv01][0].ToString().Split('/')[4].ToString());

            }
        }

        //System.Diagnostics.Debug.WriteLine(list.Count() + " " + numberOfRows);

        //int numberOfRows = GridView2.Rows.Count;
        numberOfRows++;
        String dataid = numberOfRows.ToString();
        Session["Sdataid"] = dataid;
        Graph g = (Graph)Session["ugraph"];
        

        g.NamespaceMap.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));

      
     
        IUriNode Username = g.CreateUriNode("foaf:accountName");
        IUriNode rdftype = g.CreateUriNode("rdf:type");//FOAFPERSON
        IUriNode foafPerson = g.CreateUriNode("foaf:OnlineAccount");//FOAFPERSON

        IUriNode personID = g.CreateUriNode(UriFactory.Create("http://www.phonebook.co.uk/Person/" + numberOfRows));
      
        

        ILiteralNode NUsername = g.CreateLiteralNode(Session["UserId"].ToString());
        

        g.Assert(personID, Username, NUsername);
        g.Assert(personID, rdftype, foafPerson);//FOAFPERSON

        RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
        rdfxmlwriter.Save(g, path_public);

        LoadPersonalData();

    }
    protected int checkuser()
    {
        Graph g = new Graph();
        FileLoader.Load(g, path_public);
        var list = (List<string[]>)Session["UserList"];
        string username = Session["userid"].ToString();
        //System.Diagnostics.Debug.WriteLine("printing..");
        for (int cv03 = 0; cv03 < list.Count; cv03++)
        {
            if (username == list[cv03][2])
            {
               
                return 1;
            }
           
        }
        return 0;
    }
}

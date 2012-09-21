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


public partial class Default3 : System.Web.UI.Page
{
    public string path_user = "C:/Users/panayiotis/master/MSC PROJECT/db/users/";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            datastructure();
            List<string[]> list = new List<string[]>();
            Session["datalist"] = list;

            List<string[]> list2 = new List<string[]>();
            Session["datalist2"] = list2;

            var userName = Page.User.Identity.Name;
            Session["UserId"] = userName;

          

            if (userName != "")
            {
                LoadPersonalData();
                LoadPersonalGrid();
                PrivatePanel.Visible = true;

            }
            else
            {
                PrivatePanel.Visible = false;
            }
            //UNIT TEST FUNCTIONS
            //UnitTestaddRow()
            //UnitTestaddRDFTriple()
            //UnitTestExportFavorClick()
            //END UNIT TEST
        }
    }
    //loaddin person after refresh
    protected void LoadPersonalData()
    {
        Graph g = new Graph();
        FileLoader.Load(g, path_user + Session["UserId"].ToString() + ".rdf");
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
        Session["PersonalList"] = PersonalList;
             printinconsoletest(PersonalList);
    }
    protected void LoadPersonalGrid()
    {

        GridView2.DataSource = null;
        GridView2.DataBind();
        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];
        var list = (List<string[]>)Session["PersonalList"];

       printinconsoletest(list);
        for (int cv03 = 0; cv03 < list.Count / 8; cv03++)
        {
            //take out the source from the viewstate object
            //initialise a new Datarow object
            DataRow drNewRow = dt2Datas.NewRow();

            drNewRow["Comments"] = list[1 + 8 * cv03][2];
            drNewRow["Faculty"] = list[2 + 8 * cv03][2];
            drNewRow["Surname"] = list[3 + 8 * cv03][2];
            drNewRow["E-mail"] = list[4 + 8 * cv03][2].Split(':')[1];
            drNewRow["Name"] = list[5 + 8 * cv03][2];
            drNewRow["Phone"] = list[6 + 8 * cv03][2].Split(':')[1];
            drNewRow["Title"] = list[7 + 8 * cv03][2];

            // int.Parse( list[list.Count() - 1][0].ToString().Split('/')[6].ToString() 

            drNewRow["dataid"] = list[0 + 8 * cv03][0].ToString();
            //drNewRow["Fullname"] = list[cv03][5];
            //add this new row to the Datatable and commit changes
            dt2Datas.Rows.Add(drNewRow);
            dt2Datas.AcceptChanges();

            //now since you have your datasource,bind it to the grid
        }
        GridView2.DataSource = dt2Datas;
        GridView2.DataBind();

    }
    protected void FindData()
    {
        Resetdatalist();
        var list = (List<string[]>)Session["datalist"];
        //Define a remote endpoint
        //Use the DBPedia SPARQL endpoint with the default Graph set to DBPedia
        SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(
            new Uri("http://sparql.data.southampton.ac.uk/"), "http://data.southampton.ac.uk/");
        var name = namepicker.Value;
        var surname = surnamepicker.Value;
        var title = titlepicker.Value;
        var email = emailpicker.Value;
        var number = numberpicker.Value;
        var faculty = facultypicker.Value;

        //UNIT SEARCH TEST
     /*   name ="Nigel Richard";
        surname = "Shadbolt";
        title = "Prof";
        email = "nrs@ecs.soton.ac.uk";
        number = "+442380597682";
        faculty = "Electronics & computer Science";*/
        //END UNIT SEARCH TEST
        number = number.Replace("+", "");
        SparqlResultSet results = endpoint.QueryWithResultSet("PREFIX foaf: <http://xmlns.com/foaf/0.1/> PREFIX soton: <http://id.southampton.ac.uk/ns/> PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#> SELECT ?title ?name ?surname ?mbox ?phone ?person ?education   WHERE {    GRAPH <http://id.southampton.ac.uk/dataset/phonebook/latest>    {       ?person a foaf:Person ;             foaf:familyName ?surname ;             foaf:givenName ?name ;             foaf:mbox ?mbox ;                foaf:phone ?phone ;             foaf:title ?title .       }       OPTIONAL{         ?division foaf:member ?person ; rdfs:label ?education . FILTER regex(?education, '" + faculty + "' , 'i') }        FILTER regex(?name, '" + name + "' , 'i')         FILTER regex(?surname, '" + surname + "', 'i')         FILTER regex(?title, '" + title + "' , 'i')         FILTER(regex(str(?mbox), '" + email + "'))       FILTER(regex(str(?phone), '" + number + "')) .    }     LIMIT 10");

        string[] table1;
        foreach (SparqlResult result in results)
        {
            string[] phonebookdata = new string[7];
         
            table1 = result.ToString().Split(',');
            for (int cv01 = 0; cv01 < 7; cv01++)
            {
                phonebookdata[cv01] = table1[cv01].Split('=')[1];
                if (cv01 == 4 || cv01 == 3)
                {
                    phonebookdata[cv01] = phonebookdata[cv01].Split(':')[1].Remove(phonebookdata[cv01].Split(':')[1].Length - 1, 1);
                }
                else if (cv01 == 6) { phonebookdata[cv01] = phonebookdata[cv01].Remove(0, 1); }
                else if (cv01 == 5) { phonebookdata[cv01] = phonebookdata[cv01].Replace(" ", ""); }
                else
                {
                    string temp = phonebookdata[cv01].Remove(phonebookdata[cv01].Length - 1, 1);
                    phonebookdata[cv01] = temp.Remove(0, 1);
                }

            }
            list.Add(phonebookdata);
        }
        var list2 = (List<string[]>)Session["datalist2"];
    }
    private void printinconsoletest(List<string[]> list)
    {
        for (int cv03 = 0; cv03 < list.Count; cv03++)
        {

            //RDFDATA =6
            for (int cv04 = 0; cv04 < 3; cv04++)
            {
                 // System.Diagnostics.Debug.WriteLine(" --|" + list[cv03][cv04]+ "|--" );

            }
        }
    }
    private void datastructure()
    {

        GridView1.AutoGenerateColumns = false;
        DataTable dtDatas = new DataTable();

        //add columns to the datatable

        dtDatas.Columns.Add("Title");
        dtDatas.Columns.Add("Name");
        dtDatas.Columns.Add("Surname");
        dtDatas.Columns.Add("E-mail");
        dtDatas.Columns.Add("Phone");
        dtDatas.Columns.Add("dataid");
        dtDatas.Columns.Add("Faculty");

        //store the state of the datatable into a ViewState object

        ViewState["dtDatas"] = dtDatas;

        GridView2.AutoGenerateColumns = false;
        DataTable dt2Datas = new DataTable();

        //add columns to the datatable

        dt2Datas.Columns.Add("Title");
        dt2Datas.Columns.Add("Name");
        dt2Datas.Columns.Add("Surname");
        dt2Datas.Columns.Add("E-mail");
        dt2Datas.Columns.Add("Phone");
        dt2Datas.Columns.Add("comments");
        dt2Datas.Columns.Add("dataid");
        dt2Datas.Columns.Add("Faculty");
        //store the state of the datatable into a ViewState object

        ViewState["dt2Datas"] = dt2Datas;

    }
    protected void GV2_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //Set the edit index.
        GridView2.EditIndex = e.NewEditIndex;

        //Bind data to the GridView control.

        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];

        GridView2.DataSource = dt2Datas;
        GridView2.DataBind();
    }
    protected void GV2_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = GridView2.Rows[e.RowIndex];
        string UpdatedComments = ((TextBox)(row.Cells[7].Controls[0])).Text;

        if (UpdatedComments.Length > 30)
        {
            string script = "alert(\"Please use fewer than 30 letters for your comments\");";
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                          "ServerControlScript", script, true);
            return;
        }


        Graph g = new Graph();
        FileLoader.Load(g, path_user + Session["UserId"].ToString() + ".rdf");
        //Retrieve the table from the session object.
        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];

        //Update the values.

        dt2Datas.Rows[row.DataItemIndex]["Comments"] = ((TextBox)(row.Cells[7].Controls[0])).Text;

        //RDF
        string Title = Session["STitle"].ToString();
        string Name = Session["SName"].ToString();
        string Surname = Session["SSurname"].ToString();
        string Email = Session["SE-mail"].ToString();
        Email = "mailto:" + Email;
        string Phone = Session["SPhone"].ToString();
        Phone = "tel:" + Phone;
        string dataid = Session["Sdataid"].ToString();
        string Comments = Session["SComments"].ToString();
        string Faculty = Session["SFaculty"].ToString();

        //System.Diagnostics.Debug.WriteLine(" Comments1=" + Comments);

        g.NamespaceMap.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
        g.NamespaceMap.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
        g.NamespaceMap.AddNamespace("v", new Uri("http://www.w3.org/2006/vcard/ns#"));

        IUriNode person = g.CreateUriNode(UriFactory.Create(dataid));
        IUriNode ITitle = g.CreateUriNode("foaf:title");
        IUriNode IName = g.CreateUriNode("foaf:name");
        IUriNode ISurname = g.CreateUriNode("foaf:familyName");
        IUriNode IEmail = g.CreateUriNode("foaf:mbox");
        IUriNode IPhone = g.CreateUriNode("foaf:phone");
        IUriNode IFaculty = g.CreateUriNode("rdfs:label");
        IUriNode IComments = g.CreateUriNode("rdfs:comment");
        IUriNode rdftype = g.CreateUriNode("rdf:type");//FOAFPERSON
        IUriNode foafPerson = g.CreateUriNode("foaf:Person");//FOAFPERSON

        g.Retract(person, ITitle, g.CreateLiteralNode(Title));
        g.Retract(person, IName, g.CreateLiteralNode(Name));
        g.Retract(person, ISurname, g.CreateLiteralNode(Surname));
        g.Retract(person, IEmail, g.CreateUriNode(UriFactory.Create((Email))));
        g.Retract(person, IPhone, g.CreateUriNode(UriFactory.Create((Phone))));
        g.Retract(person, IComments, g.CreateLiteralNode(Comments));
        g.Retract(person, IFaculty, g.CreateLiteralNode(Faculty));
        g.Retract(person, rdftype, foafPerson); //FOAFPERSON

        RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
        rdfxmlwriter.Save(g, path_user + Session["UserId"].ToString() + ".rdf");

        g.Assert(person, ITitle, g.CreateLiteralNode(Title));
        g.Assert(person, IName, g.CreateLiteralNode(Name));
        g.Assert(person, ISurname, g.CreateLiteralNode(Surname));
        g.Assert(person, IEmail, g.CreateUriNode(UriFactory.Create((Email))));
        g.Assert(person, IPhone, g.CreateUriNode(UriFactory.Create((Phone))));
        g.Assert(person, IFaculty, g.CreateLiteralNode(Faculty));
        g.Assert(person, rdftype, foafPerson); //FOAFPERSON


        g.Assert(person, IComments, g.CreateLiteralNode(UpdatedComments));
       // System.Diagnostics.Debug.WriteLine(" Comments2=" + UpdatedComments);
        rdfxmlwriter.Save(g, path_user + Session["UserId"].ToString() + ".rdf");
        LoadPersonalData();
        //END RDF

        //Reset the edit index.
        GridView2.EditIndex = -1;
        GridView2.DataSource = dt2Datas;
        //Bind data to the GridView control.
        GridView2.DataBind();
        //ViewState["dt2Datas"] = dt2Datas;

    }
    protected void GV2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //Reset the edit index.

        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];
        GridView2.DataSource = dt2Datas;
        GridView2.EditIndex = -1;
        //Bind data to the GridView control.
        GridView2.DataBind();
    }
    protected void Resetdatalist()
    {
        var list = (List<string[]>)Session["datalist"];
        DataTable dtDatas = (DataTable)ViewState["dtDatas"];
        dtDatas.Clear();
        list.Clear();

        Session["datalist"] = list;
    }
    protected void fillGridwithData()
    {
        FindData();

        GridView1.DataSource = null;
      
        GridView1.DataBind();

        DataTable dtDatas = (DataTable)ViewState["dtDatas"];


        var list = (List<string[]>)Session["datalist"];
        //  printinconsoletest(list);
        for (int cv03 = 0; cv03 < list.Count; cv03++)
        {
            //take out the source from the viewstate object
            //initialise a new Datarow object

            DataRow drNewRow = dtDatas.NewRow();

            drNewRow["Title"] = list[cv03][0];
            drNewRow["Name"] = list[cv03][1];
            drNewRow["Surname"] = list[cv03][2];
            drNewRow["E-mail"] = list[cv03][3];
            drNewRow["Phone"] = list[cv03][4];
            drNewRow["Faculty"] = list[cv03][6];
            drNewRow["dataid"] = list[cv03][5];

            //add this new row to the Datatable and commit changes
            dtDatas.Rows.Add(drNewRow);
            dtDatas.AcceptChanges();

            //now since you have your datasource,bind it to the grid



        }
        GridView1.DataSource = dtDatas;

        GridView1.DataBind();
        // dtDatas.Clear();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        fillGridwithData();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

        namepicker.Value = "";
        emailpicker.Value = "";
        numberpicker.Value = "";
        surnamepicker.Value = "";
        titlepicker.Value = "";
        facultypicker.Value = "";
        
        GridView1.DataSource = null;
        GridView1.DataBind();
    }
    protected void gv1Paging_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dtDatas = (DataTable)ViewState["dtDatas"];
        GridView1.DataSource = dtDatas;
        GridView1.PageIndex = e.NewPageIndex;

        GridView1.DataBind();
    }
    protected void gv2Paging_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];
        GridView2.DataSource = dt2Datas;
        GridView2.PageIndex = e.NewPageIndex;

        GridView2.DataBind();
    }
    protected void gvDataBound(object sender, EventArgs e)
    { int same =  0;
        var userid = Session["UserId"];
        GridView1.Columns[7].Visible = true;
        if (userid == null || userid.Equals(""))
            GridView1.Columns[6].Visible = false;
        else
            GridView1.Columns[6].Visible = true;

        
      //  if (GridView1.Rows.Count > GridView2.Rows.Count)
        //{
           // for (int rows2 = 0; rows2 < GridView2.Rows.Count; rows2++)
            for (int rows = 0; rows < GridView1.Rows.Count; rows++)
            {
                if ((GridView1.Rows[rows].FindControl("btnAddFavor") as Button).Enabled == false)
                {
                    for (int rows2 = 0; rows2 < GridView2.Rows.Count; rows2++)
                    {
                       // System.Diagnostics.Debug.WriteLine("AHA! AHAAAAAAAAA======" + GridView2.Rows[rows2].Cells[3].Text+ "!!!!!" + GridView1.Rows[rows].Cells[3].Text);
                        if ((GridView2.Rows[rows2].Cells[0].Text == GridView1.Rows[rows].Cells[0].Text) &&//title
                             (GridView2.Rows[rows2].Cells[1].Text == GridView1.Rows[rows].Cells[1].Text) && //name
                              (GridView2.Rows[rows2].Cells[2].Text == GridView1.Rows[rows].Cells[2].Text) && //surname 
                               (GridView2.Rows[rows2].Cells[4].Text == GridView1.Rows[rows].Cells[4].Text)  &&       //phone
                            (GridView2.Rows[rows2].Cells[3].Text == GridView1.Rows[rows].Cells[3].Text) //email 
                           // ((GridView2.Rows[rows2].Cells[5].Text == GridView1.Rows[rows].Cells[5].Text.Replace("&amp;", "&")) || (GridView2.Rows[rows2].Cells[5].Text == GridView1.Rows[rows].Cells[5].Text.Replace("&nbsp;", ""))) //faculty 
                     )
                        {
                            same = 1;
                        }
                    }
                    if(same == 0)
                            (GridView1.Rows[rows].FindControl("btnAddFavor") as Button).Enabled = true;
                    same = 0;
                }
                /*
                if (GridView2.Rows.Count == 0)
                {
                      for (rows = 0; rows < GridView1.Rows.Count; rows++)
            {
                if ((GridView1.Rows[rows].FindControl("btnAddFavor") as Button).Enabled == false)
                { (GridView1.Rows[rows].FindControl("btnAddFavor") as Button).Enabled = true;
                }

            }*/
        }


    }
    
    protected void GV2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[6].ToolTip = ((HiddenField)e.Row.Cells[7].FindControl("HFText")).Value;
           
               
        }
    }
    protected void GV1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int same =  0;
        var list = (List<string[]>)Session["PersonalList"];

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
           // for (int cv01 = 0; cv01 < e.Row.Cells.Count-2 /*6 fields*/; cv01++)
               // System.Diagnostics.Debug.WriteLine(e.Row.Cells[cv01].Text);
            if (list != null)
            {
                for (int cv03 = 0; cv03 < list.Count / 8; cv03++)
                {

                  //  System.Diagnostics.Debug.WriteLine("AHA! AHAAAAAAAAA" + e.Row.Cells[5].Text + "=" + list[1 + 8 * cv03][2] + "=" );
                    if ((e.Row.Cells[0].Text == list[7 + 8 * cv03][2]) && //title
                        (e.Row.Cells[1].Text == list[5 + 8 * cv03][2]) && //name
                        (e.Row.Cells[2].Text == list[3 + 8 * cv03][2]) && //surname 
                        (e.Row.Cells[4].Text == list[6 + 8 * cv03][2].Split(':')[1]) &&        //phone
                        (e.Row.Cells[3].Text.Split(':')[1].Split('>')[0] == list[4 + 8 * cv03][2].Split(':')[1]) && //email 
                        ((list[2 + 8 * cv03][2] == e.Row.Cells[5].Text.Replace("&amp;", "&")) || (list[2 + 8 * cv03][2] == e.Row.Cells[5].Text.Replace("&nbsp;", ""))) //faculty 
    
                        )
                    {
                        same = 1;
                       // System.Diagnostics.Debug.WriteLine("mpika");
                        break;
                    }
                }
                if (same == 1) { (e.Row.FindControl("btnAddFavor") as Button).Enabled = false; same = 0; }
                else
                {
                    (e.Row.FindControl("btnAddFavor") as Button).Enabled = true;
                    same = 0;

                }
            }
        }
    }
    protected void gv2DataBound(object sender, EventArgs e)
    {
        var userid = Session["UserId"];
        GridView2.Columns[8].Visible = false;
        GridView2.Columns[10].Visible = true;


    }
    protected void BtnClick(object sender, EventArgs e)
    {

        var name = namepicker.Value;
        var surname = surnamepicker.Value;
        fillGridwithData();
        // var email = emailpicker.Value;
        // var number = numberpicker.Value;
    }
    protected void AddFavorClick(object sender, EventArgs e)
    {
        // Label6.Text = "i will add";

        var list = (List<string[]>)Session["datalist"];
        
        

    }
    protected void RemoveFavorClick(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];

        //delete from rdf
        Graph g = new Graph();
        FileLoader.Load(g, path_user + Session["UserId"].ToString() + ".rdf");

        string Title = dt2Datas.Rows[index]["Title"].ToString();
        string Name = dt2Datas.Rows[index]["Name"].ToString();
        string Surname = dt2Datas.Rows[index]["Surname"].ToString();
        string Email = dt2Datas.Rows[index]["E-mail"].ToString();
        Email = "mailto:" + Email;
        string Phone = dt2Datas.Rows[index]["Phone"].ToString();
        Phone = "tel:" + Phone;
        string dataid = dt2Datas.Rows[index]["dataid"].ToString();
        string Comments = dt2Datas.Rows[index]["Comments"].ToString();
        string Faculty = dt2Datas.Rows[index]["Faculty"].ToString();
        

        g.NamespaceMap.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
        g.NamespaceMap.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
        g.NamespaceMap.AddNamespace("v", new Uri("http://www.w3.org/2006/vcard/ns#"));


        IUriNode person = g.CreateUriNode(UriFactory.Create(dataid));
        IUriNode ITitle = g.CreateUriNode("foaf:title");
        IUriNode IName = g.CreateUriNode("foaf:name");
        IUriNode ISurname = g.CreateUriNode("foaf:familyName");
        IUriNode IEmail = g.CreateUriNode("foaf:mbox");
        IUriNode IPhone = g.CreateUriNode("foaf:phone");
        IUriNode IFaculty = g.CreateUriNode("rdfs:label");
        IUriNode IComments = g.CreateUriNode("rdfs:comment");
        IUriNode rdftype = g.CreateUriNode("rdf:type");//FOAFPERSON
        IUriNode foafPerson = g.CreateUriNode("foaf:Person");//FOAFPERSON
      
       // IUriNode NPhone = g.CreateUriNode(UriFactory.Create("tel:" + Session["Snumber"].ToString()));
      //  IUriNode NEmail = g.CreateUriNode(UriFactory.Create("mailto:" + Session["Semail"].ToString().Split('>')[1].Split('<')[0].ToString()));

        g.Retract(person, ITitle, g.CreateLiteralNode(Title));
        g.Retract(person, IName, g.CreateLiteralNode(Name));
        g.Retract(person, ISurname, g.CreateLiteralNode(Surname));
        g.Retract(person, IEmail, g.CreateUriNode(UriFactory.Create((Email))));
        g.Retract(person, IPhone, g.CreateUriNode(UriFactory.Create((Phone))));
        g.Retract(person, IComments, g.CreateLiteralNode(Comments));
        g.Retract(person, IFaculty, g.CreateLiteralNode(Faculty));
        g.Retract(new Triple(person, rdftype, foafPerson));//FOAFPERSON

        RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
        rdfxmlwriter.Save(g, path_user + Session["UserId"].ToString() + ".rdf");

        //remove from datatable
        dt2Datas.Rows[index].Delete();
        dt2Datas.AcceptChanges();
        GridView2.DataSource = dt2Datas;
        GridView2.DataBind();

    /*    if (faculty.Equals("&nbsp;"))
            faculty = "";
        else if (faculty.Contains("&amp;"))
            faculty = faculty.Replace("&amp;", "&");
        */
        if (GridView1.Enabled)
        {
          
           
            for (int rows = 0; rows < GridView1.Rows.Count; rows++)
            {
                //System.Diagnostics.Debug.WriteLine("AHA! AHAAAAAAAAA" + Phone + "=" + GridView1.Rows[rows].Cells[4].Text + "=" + GridView1.Rows[rows].Cells[5].Text.Replace("&nbsp;", ""));
                if ((Title == GridView1.Rows[rows].Cells[0].Text) &&//title
                  (Name == GridView1.Rows[rows].Cells[1].Text) && //name
                 (Surname == GridView1.Rows[rows].Cells[2].Text) &&//surname 
                 (Phone.Split(':')[1] == GridView1.Rows[rows].Cells[4].Text) &&    //phone
                    (Email == GridView1.Rows[rows].Cells[3].Text.Split('=')[1].Split('>')[0]) &&//email 
                    ((Faculty == GridView1.Rows[rows].Cells[5].Text.Replace("&amp;", "&")) ||
                    (Faculty == GridView1.Rows[rows].Cells[5].Text.Replace("&nbsp;", "")) ) //faculty */
                  )
                {
                   // System.Diagnostics.Debug.WriteLine("AHA! AHAAAAAAAAA" + rows);
                    (GridView1.Rows[rows].FindControl("btnAddFavor")  as Button).Enabled = true;
                }
            }
        }
        
    }
    protected void ExportFavorClick(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "text/x-vCard";
        string vname = dt2Datas.Rows[index]["Name"].ToString() + " " + dt2Datas.Rows[index]["Surname"].ToString();
        string vfilename = vname + ".VCF";
        HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=" + vfilename);
        StringBuilder strHtmlContent = new StringBuilder();
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        stringWrite.WriteLine("BEGIN:VCARD");
        stringWrite.WriteLine("VERSION:2.1");

        string vcomp = "";
        string vadd = "";
        string vjobtitle = dt2Datas.Rows[index]["Title"].ToString();
        string vmail = dt2Datas.Rows[index]["E-mail"].ToString();
        string vcno = dt2Datas.Rows[index]["Phone"].ToString();
        string vcomments = dt2Datas.Rows[index]["Comments"].ToString();
        string vorg = dt2Datas.Rows[index]["Faculty"].ToString();

        stringWrite.WriteLine("FN:" + vname);
        stringWrite.WriteLine("ORG:" + vorg);
        stringWrite.WriteLine("NOTE:" + vcomments);
        stringWrite.WriteLine("Email:" + vmail);
        stringWrite.WriteLine("ORG:" + vcomp);
        stringWrite.WriteLine("TITLE:" + vjobtitle);
        stringWrite.WriteLine("TEL;WORK;VOICE:" + vcno);
        stringWrite.WriteLine("ADR;WORK;ENCODING=QUOTED-PRINTABLE:" + vadd);

        stringWrite.WriteLine("END:VCARD");
        HttpContext.Current.Response.Write(stringWrite.ToString());
        HttpContext.Current.Response.End();
    }
    protected void UpdateRDFClick(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];

        //store values and send to update
        Graph g = new Graph();
        FileLoader.Load(g, path_user + Session["UserId"].ToString() + ".rdf");

        Session["STitle"] = dt2Datas.Rows[index]["Title"].ToString();
        Session["SName"] = dt2Datas.Rows[index]["Name"].ToString();
        Session["SSurname"] = dt2Datas.Rows[index]["Surname"].ToString();
        Session["SE-mail"] = dt2Datas.Rows[index]["E-mail"].ToString();
        Session["SPhone"] = dt2Datas.Rows[index]["Phone"].ToString();
        Session["Sdataid"] = dt2Datas.Rows[index]["dataid"].ToString();
        Session["SComments"] = dt2Datas.Rows[index]["Comments"].ToString();
        Session["SFaculty"] = dt2Datas.Rows[index]["Faculty"].ToString();
        Session["Sdataid"] = dt2Datas.Rows[index]["dataid"].ToString();


    }
    //adding to grid and rdf start from here
    protected void GV_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        
        if (e.CommandName == "AddFavor")
        {

          
            
            GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

            Button addbutton = (Button)row.FindControl("btnAddFavor");
            addbutton.Enabled = false;

            // row contains current Clicked Gridview Row    
            String title = row.Cells[0].Text;
            Session["Stitle"] = title;
            String name = row.Cells[1].Text;
            Session["Sname"] = name;
            String surname = row.Cells[2].Text;
            Session["Ssurname"] = surname;
            String email = row.Cells[3].Text;
            Session["Semail"] = email;
            String number = row.Cells[4].Text;
            Session["Snumber"] = number;
            String faculty = row.Cells[5].Text;
            if (faculty.Equals("&nbsp;"))
                faculty = "";
            else if (faculty.Contains("&amp;"))
                faculty = faculty.Replace("&amp;", "&");
            Session["Sfaculty"] = faculty;

            String dataid = row.Cells[7].Text;
            Session["Sdataid"] = dataid;

          //  System.Diagnostics.Debug.WriteLine(" dataid=" + dataid + "|--");
            addRDFTriple();

            addRow();

        }
        if (e.CommandName == "RemoveRow")
        {
            
        }

    }
    protected void addRow()
    {

        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];
        DataRow drNewRow = dt2Datas.NewRow();

        drNewRow["Title"] = Session["Stitle"];
        drNewRow["Name"] = Session["Sname"];
        drNewRow["Surname"] = Session["Ssurname"];
        drNewRow["E-mail"] = Session["Semail"].ToString().Split('>')[1].Split('<')[0].ToString();
        // Label6.Text = Session["Semail"].ToString().Split(',')[2];
        drNewRow["Phone"] = Session["Snumber"];
        drNewRow["Faculty"] = Session["Sfaculty"];
        drNewRow["dataid"] = Session["Sdataid"];

        dt2Datas.Rows.Add(drNewRow);
        dt2Datas.AcceptChanges();

        GridView2.DataSource = dt2Datas;
        GridView2.DataBind();

        string userid = Session["UserId"].ToString();
        if (userid == null || userid.Equals(""))
        {

            // checkfileextists(userid+".rdf");
            //createrdf(userid);
        }
        else
        {


            //  checkfileextists(userid + ".rdf");

        }

    }
    protected void addRDFTriple()
    {
        var list = (List<string[]>)Session["PersonalList"];      
        Graph g = (Graph)Session["ggraph"];

        g.NamespaceMap.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
        g.NamespaceMap.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));
        g.NamespaceMap.AddNamespace("v", new Uri("http://www.w3.org/2006/vcard/ns#"));

        IUriNode personID = g.CreateUriNode(UriFactory.Create(Session["Sdataid"].ToString()));
        IUriNode Title = g.CreateUriNode("foaf:title");
        IUriNode Name = g.CreateUriNode("foaf:name");
        IUriNode Surname = g.CreateUriNode("foaf:familyName");
        IUriNode Email = g.CreateUriNode("foaf:mbox");
        IUriNode Phone = g.CreateUriNode("foaf:phone");
        IUriNode Faculty = g.CreateUriNode("rdfs:label");
        IUriNode Comments = g.CreateUriNode("rdfs:comment");

        IUriNode rdftype = g.CreateUriNode("rdf:type");//FOAFPERSON
        IUriNode foafPerson = g.CreateUriNode("foaf:Person");//FOAFPERSON

        ILiteralNode NTitle = g.CreateLiteralNode(Session["Stitle"].ToString());
        ILiteralNode NName = g.CreateLiteralNode(Session["Sname"].ToString());
        ILiteralNode NSurname = g.CreateLiteralNode(Session["Ssurname"].ToString());
        IUriNode NPhone = g.CreateUriNode(UriFactory.Create("tel:" + Session["Snumber"].ToString()));
        IUriNode NEmail = g.CreateUriNode(UriFactory.Create("mailto:" + Session["Semail"].ToString().Split('>')[1].Split('<')[0].ToString()));
      
        ILiteralNode NFaculty = g.CreateLiteralNode(Session["Sfaculty"].ToString());
        ILiteralNode NComments = g.CreateLiteralNode(" ");

        g.Assert(personID, Title, NTitle);
        g.Assert(personID, Name, NName);
        g.Assert(personID, Surname, NSurname);
        g.Assert(personID, Email, NEmail);
        g.Assert(personID, Phone, NPhone);
        g.Assert(personID, Faculty, NFaculty);
        g.Assert(personID, Comments, NComments);
        g.Assert(personID, rdftype, foafPerson); //FOAFPERSON

        RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
        rdfxmlwriter.Save(g, path_user + Session["UserId"].ToString() + ".rdf");

        LoadPersonalData();

    }

    protected string FacultyShort(object faculty)
    {
        #region Variable Declarations
        string text = faculty as string;
        char[] values = text.ToCharArray();
        #endregion
        if (text.Length > 10)
            return text.Substring(0, 10) + "...";
        else return text;
    }
    protected void UnitTestaddRow()
    {

        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];
        DataRow drNewRow = dt2Datas.NewRow();

        drNewRow["Title"] = "Title";
        drNewRow["Name"] = "Sname";
        drNewRow["Surname"] ="Surname";
        drNewRow["E-mail"] = "email@unittest.com";
        drNewRow["Phone"] = "+442380123456";
        drNewRow["Faculty"] = "faculty";
        drNewRow["dataid"] = "1234567";

        dt2Datas.Rows.Add(drNewRow);
        dt2Datas.AcceptChanges();

        GridView2.DataSource = dt2Datas;
        GridView2.DataBind();
    }
    protected void UnitTestaddRDFTriple()
    {
        var list = (List<string[]>)Session["PersonalList"];      
        Graph g = (Graph)Session["ggraph"];

        g.NamespaceMap.AddNamespace("foaf", new Uri("http://xmlns.com/foaf/0.1/"));
        g.NamespaceMap.AddNamespace("rdfs", new Uri("http://www.w3.org/2000/01/rdf-schema#"));

        IUriNode personID = g.CreateUriNode(UriFactory.Create("http://unittest.com/"));
        IUriNode Title = g.CreateUriNode("foaf:title");
        IUriNode Name = g.CreateUriNode("foaf:name");
        IUriNode Surname = g.CreateUriNode("foaf:familyName");
        IUriNode Email = g.CreateUriNode("foaf:mbox");
        IUriNode Phone = g.CreateUriNode("foaf:phone");
        IUriNode Faculty = g.CreateUriNode("rdfs:label");
        IUriNode Comments = g.CreateUriNode("rdfs:comment");

        IUriNode rdftype = g.CreateUriNode("rdf:type");//FOAFPERSON
        IUriNode foafPerson = g.CreateUriNode("foaf:Person");//FOAFPERSON

        ILiteralNode NTitle = g.CreateLiteralNode("title");
        ILiteralNode NName = g.CreateLiteralNode("name");
        ILiteralNode NSurname = g.CreateLiteralNode("surname");
        IUriNode NPhone = g.CreateUriNode(UriFactory.Create("tel:" + "+442380123456"));
        IUriNode NEmail = g.CreateUriNode(UriFactory.Create("mailto:" + "email@abc.com"));
      
        ILiteralNode NFaculty = g.CreateLiteralNode("Sfaculty");
        ILiteralNode NComments = g.CreateLiteralNode("Comment");

        g.Assert(personID, Title, NTitle);
        g.Assert(personID, Name, NName);
        g.Assert(personID, Surname, NSurname);
        g.Assert(personID, Email, NEmail);
        g.Assert(personID, Phone, NPhone);
        g.Assert(personID, Faculty, NFaculty);
        g.Assert(personID, Comments, NComments);
        g.Assert(personID, rdftype, foafPerson); //FOAFPERSON

        RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
        rdfxmlwriter.Save(g, path_user + Session["UserId"].ToString() + ".rdf");

        LoadPersonalData();

    }
    protected void UnitTestExportFavorClick(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        DataTable dt2Datas = (DataTable)ViewState["dt2Datas"];

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "text/x-vCard";
        string vname = dt2Datas.Rows[index]["Name"].ToString() + " " + dt2Datas.Rows[index]["Surname"].ToString();
        string vfilename = vname + ".VCF";
        HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=" + vfilename);
        StringBuilder strHtmlContent = new StringBuilder();
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        stringWrite.WriteLine("BEGIN:VCARD");
        stringWrite.WriteLine("VERSION:2.1");

        string vcomp = "";
        string vadd = "";
        string vjobtitle = dt2Datas.Rows[index]["Title"].ToString();
        string vmail = dt2Datas.Rows[index]["E-mail"].ToString();
        string vcno = dt2Datas.Rows[index]["Phone"].ToString();
        string vcomments = dt2Datas.Rows[index]["Comments"].ToString();
        string vorg = dt2Datas.Rows[index]["Faculty"].ToString();

        //EXPORT VCARD UNIT TEST
        vcomp = "unit test";
        vadd = "unit test";
        vjobtitle = "unit test";
        vmail = "unit test";
        vcno = "unit test";
        vcomments = "unit test";
        vorg = "unit test";
        //END EXPORT VCARD UNIT TEST

        stringWrite.WriteLine("FN:" + vname);
        stringWrite.WriteLine("ORG:" + vorg);
        stringWrite.WriteLine("NOTE:" + vcomments);
        stringWrite.WriteLine("Email:" + vmail);
        stringWrite.WriteLine("ORG:" + vcomp);
        stringWrite.WriteLine("TITLE:" + vjobtitle);
        stringWrite.WriteLine("TEL;WORK;VOICE:" + vcno);
        stringWrite.WriteLine("ADR;WORK;ENCODING=QUOTED-PRINTABLE:" + vadd);

        stringWrite.WriteLine("END:VCARD");
        HttpContext.Current.Response.Write(stringWrite.ToString());
        HttpContext.Current.Response.End();
    }
}
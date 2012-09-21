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

public partial class MyAccount : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var userName = Page.User.Identity.Name;
        Session["UserId"] = userName;
        if (userName != "")
        {

            accountlbl.Text = "Welcome " + userName;
            LoadPersonalData();
        }
        else
        {
            accountlbl.Text = "Welcome " + userName;
        }
    }
    protected void LoadPersonalData()
    {
        Graph g = new Graph();
        FileLoader.Load(g, "C:/Users/panayiotis/master/MSC PROJECT/db/users/" + Session["UserId"].ToString() + ".rdf");
        List<string[]> NumPersonalList = new List<string[]>();

        string[] temptaple = new string[3];
        //Iterate over and print Triples

        foreach (Triple t in g.Triples)
        {
            string[] personaldata = new string[3];

            temptaple = t.ToString().Split(',');

            for (int cv01 = 0; cv01 <= 2; cv01++)
            {
                if (cv01 == 0)
                    personaldata[cv01] = temptaple[cv01].Remove(temptaple[cv01].Length - 1, 1); 
                else if (cv01 == 1)
                {
                    string temp = temptaple[cv01].Remove(temptaple[cv01].Length - 1, 1); 
                    personaldata[cv01] = temp.Remove(0, 1); 
                }

                else
                    personaldata[cv01] = temptaple[cv01].Remove(0, 1);
            }

            NumPersonalList.Add(personaldata);
        }
        int number = NumPersonalList.Count / 6;
 
        Cell1.InnerText = number.ToString();
        Cell2.InnerText = GridView1.Rows[0].Cells[0].Text.ToString();
        Cell3.InnerText = GridView1.Rows[0].Cells[1].Text.ToString();
    }



    protected void gv3_databound(object sender, EventArgs e)
    {
       
    }
}
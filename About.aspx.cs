using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDS.RDF;
using VDS.RDF.Writing;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Writing.Formatting;
using System.Text;

public partial class About : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {



    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        var list = (List<string[]>)Session["PersonalList"];
        printinconsoletest(list);

        for (int cv03 = 0; cv03 < list.Count / 6; cv03++)
        {
            System.Diagnostics.Debug.WriteLine("etre3e:" + cv03 + " " + list.Count );

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "text/x-vCard";
        string vfilename = "MyContact"+cv03+".VCF";
        HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=" + vfilename);
        StringBuilder strHtmlContent = new StringBuilder();
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        stringWrite.WriteLine("BEGIN:VCARD");
        stringWrite.WriteLine("VERSION:2.1");

        
        string vmail = "";
        string vname = "";
        string vcno = "";
        string vcomp = "";
        string vjobtitle = "";
        string vadd = "";
        string vcomments="";

       


            vcomments = list[0 + 6 * cv03][2];
            vmail = list[1+6 * cv03][2];
            vname = list[2+6 * cv03][2] + " " + list[4+6 * cv03][2];
            vcno = list[3+6* cv03][2];
            vcomp = "ADDCompanyName";
            vjobtitle = list[5+6 * cv03][2];
             vadd = "ADDAddress1";

             stringWrite.WriteLine("FN:" + vcomments);
            stringWrite.WriteLine("NOTE:" + vname); 
            stringWrite.WriteLine("Email:" + vmail);
            stringWrite.WriteLine("ORG:" + vcomp);
            stringWrite.WriteLine("TITLE:" + vjobtitle);
            stringWrite.WriteLine("ADR;WORK;ENCODING=QUOTED-PRINTABLE:" + vadd);


            stringWrite.WriteLine("END:VCARD");
            HttpContext.Current.Response.Write(stringWrite.ToString());
            HttpContext.Current.Response.End();
            System.Diagnostics.Debug.WriteLine("eteliose");
        }
       
        /////////////////////

    }

    protected void addRDFTriple()
    {
        var list = (List<string[]>)Session["PersonalList"];

        int numberOfRows = 0;

        if (list.Count() > 0)
            numberOfRows = int.Parse(list[list.Count() - 1][0].ToString().Split('/')[6].ToString());

        for (int cv01 = 0; cv01 < list.Count; cv01++)
        {
            if (numberOfRows < int.Parse(list[cv01][0].ToString().Split('/')[6].ToString()))
            {
                numberOfRows = int.Parse(list[cv01][0].ToString().Split('/')[6].ToString());

            }
        }

        //System.Diagnostics.Debug.WriteLine(list.Count() + " " + numberOfRows);

        //int numberOfRows = GridView2.Rows.Count;
        numberOfRows++;
        String dataid = numberOfRows.ToString();
        Session["Sdataid"] = dataid;
        Graph g = (Graph)Session["ggraph"];

        IUriNode personID = g.CreateUriNode(UriFactory.Create("http://www.phonebook.co.uk/Person/" + numberOfRows));
        IUriNode Title = g.CreateUriNode(UriFactory.Create("http://xmlns.com/foaf/0.1/Title"));
        IUriNode Name = g.CreateUriNode(UriFactory.Create("http://xmlns.com/foaf/0.1/Name"));
        IUriNode Surname = g.CreateUriNode(UriFactory.Create("http://xmlns.com/foaf/0.1/Surname"));
        IUriNode Email = g.CreateUriNode(UriFactory.Create("http://xmlns.com/foaf/0.1/Email"));
        IUriNode Phone = g.CreateUriNode(UriFactory.Create("http://xmlns.com/foaf/0.1/Phone"));
        IUriNode Comments = g.CreateUriNode(UriFactory.Create("http://xmlns.com/foaf/0.1/Comments"));

        ILiteralNode NTitle = g.CreateLiteralNode(Session["Stitle"].ToString());
        ILiteralNode NName = g.CreateLiteralNode(Session["Sname"].ToString());
        ILiteralNode NSurname = g.CreateLiteralNode(Session["Ssurname"].ToString());
        ILiteralNode NEmail = g.CreateLiteralNode(Session["Semail"].ToString().Split('>')[1].Split('<')[0].ToString());
        ILiteralNode NPhone = g.CreateLiteralNode(Session["Snumber"].ToString());
        ILiteralNode NComments = g.CreateLiteralNode(" ");

        g.Assert(personID, Title, NTitle);
        g.Assert(personID, Name, NName);
        g.Assert(personID, Surname, NSurname);
        g.Assert(personID, Email, NEmail);
        g.Assert(personID, Phone, NPhone);
        g.Assert(personID, Comments, NComments);

        RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
        rdfxmlwriter.Save(g, "C:/Users/panayiotis/master/MSC PROJECT/db/" + Session["UserId"].ToString() + ".rdf");

      
    }


    private void printinconsoletest(List<string[]> list)
    {
        for (int cv03 = 0; cv03 < list.Count; cv03++)
        {

            //RDFDATA =6
            for (int cv04 = 0; cv04 < 3; cv04++)
            { //0=comments, 
              //1=email
              //2=name
              //3=phone
              //4=surname
              //5=title
  
                System.Diagnostics.Debug.WriteLine(" --|" + list[cv03][cv04] + "|--");

            }
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        var list = (List<string[]>)Session["PersonalList"];

        printinconsoletest(list);
    }
}
    




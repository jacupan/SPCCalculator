using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Reflection;

namespace SPCLogParser
{
    
    public partial class frmGTSMold : Form
    {        
        public class GTSMoldLogFile
        {            
            public string LotNumber { get; set; }
            public string FramePosition { get; set; }
            public string Element { get; set; }
            public decimal Actual { get; set; }
            public decimal Nominal { get; set; }
            public decimal Deviation { get; set; }
            public decimal UpTol { get; set; }
            public decimal LowTol { get; set; }
            public string Status { get; set; }
            public string Status2 { get; set; }
            public DateTime DateCreated { get; set; }
        }

        public frmGTSMold()
        {
            InitializeComponent();
        }        

        List<GTSMoldLogFile> GTSMolds = new List<GTSMoldLogFile>();

        DataTable dt_ = new DataTable();

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string[] textFiles = System.IO.Directory.GetFiles(@"C:\Users\jacupan\Documents\", "*.txt");

            foreach (string fileName in textFiles)            
            {                
                dt_.Clear();

                string fileCont = System.IO.File.ReadAllText(fileName);
                if (fileCont.Contains("FINISHED...") == true)
                {
                    //it found something
                    int MAX = (int)Math.Floor((double)(Int32.MaxValue / 5000));     // 5000 lines per read.
                    //string fileName = "C:\\Users\\jacupan\\Documents\\1540331DDAA_CH.txt";
                    string[] AllLines = null;
                    AllLines = new string[MAX];                 // Only allocate memory here
                    AllLines = File.ReadAllLines(fileName);     // Read ALL lines 


                    string regexPattern = @"(\w+)(?:\.\w+)*$";  //get filename from fullpath
                    Match baseName = Regex.Match(fileName.Replace(".TXT", ""), regexPattern);
                    string[] lotInfo = baseName.Value.ToString().Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);


                    // Process per line.
                    Parallel.For(0, AllLines.Length, x =>
                    {

                        //Debug.Print(AllLines[x]);

                        // Parse and process the lines from the textfile
                        ParseNow(AllLines[x], lotInfo[0], lotInfo[1]);
                    });

                    //Debug.Print(sbuilder.ToString()); // <==== For debuging purposes only

                    Debug.Print("Done.");

                   
                }

            }

            dt_ = ToDataTable(GTSMolds);

            dataGridView1.DataSource = dt_;

            InsertTextFileData();
            InsertParseData();
                        
        }


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        private void InsertTextFileData()
        { 
            string strQuery;
            

            SqlConnection conn = new SqlConnection("Data Source=AMPIAPPDEV02;Initial Catalog=SPCCalculator;MultipleActiveResultSets=true;user id=sa; password=sa123456");
            SqlTransaction trans;
            conn.Open();
            trans = conn.BeginTransaction();

           
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                strQuery = @"INSERT INTO logs_MoldGts VALUES ('"
                    + dataGridView1.Rows[i].Cells["LotNumber"].Value + "', '"
                    + dataGridView1.Rows[i].Cells["FramePosition"].Value + "', '"
                    + dataGridView1.Rows[i].Cells["Element"].Value + "', "
                    + dataGridView1.Rows[i].Cells["Actual"].Value + ", "
                    + dataGridView1.Rows[i].Cells["Nominal"].Value + ", "
                    + dataGridView1.Rows[i].Cells["Deviation"].Value + ", "
                    + dataGridView1.Rows[i].Cells["UpTol"].Value + ", "
                    + dataGridView1.Rows[i].Cells["LowTol"].Value + ", '"
                    + dataGridView1.Rows[i].Cells["Status"].Value + "', '"
                    + dataGridView1.Rows[i].Cells["Status2"].Value + "', '"
                    + dataGridView1.Rows[i].Cells["DateCreated"].Value + "');";

                try
                {                    
                    using (SqlCommand comm = new SqlCommand(strQuery, conn, trans))
                    {
                        comm.ExecuteNonQuery();
                        trans.Commit();
                    }
                    
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    try
                    {
                        trans.Rollback();
                    }

                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }                
            }
              

            conn.Close();
        }

        private void InsertParseData()
        {
            string strQuery;

            SqlConnection conn = new SqlConnection("Data Source=AMPIAPPDEV02;Initial Catalog=SPCCalculator;MultipleActiveResultSets=true;user id=sa; password=sa123456");
            SqlTransaction trans;
            conn.Open();
            trans = conn.BeginTransaction();

            strQuery = @"
                        insert into [SPCCalculator].[dbo].[spc_MoldGts]([GUID],[LotNumber],[Device],[RunNumber],[FramePosition],[Machine],[A2Operator],[Remarks],[PkgDiameter],                            [PkgThickness],[PkgHeight],[AValue],[DXValue],[DYValue],[EPIN],[Status],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
                      SELECT NEWID() AS [GUID],Lot,Device,RunNumber,[FramePosition],'' [Machine],'' [A2Operator],'' [Remarks],diameter [PkgDiameter],0 [PkgThickness],lc                               [PkgHeight],[AValue],dx [DXValue],dy [DYValue],0 [EPIN],[Status2] [Status],'' [CreatedBy],GETDATE() [DateCreated],'' [UpdatedBy],'' [DateUpdated] FROM                           [SPCCalculator].[dbo].[VN_LotWaferInfo_MoldGts_Temp] with(nolock);";

            try
            {
                using (SqlCommand comm = new SqlCommand(strQuery, conn, trans))
                {
                    comm.ExecuteNonQuery();
                    trans.Commit();
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                try
                {
                    trans.Rollback();
                }

                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }                
            
        }
        

        private string ParseElement(string sourcestring)
        {
            string elementValue = "";

            if (sourcestring.ToLower().Contains("diameter"))
            {
                elementValue = "DIAMETER";
            }

            if (sourcestring.ToLower().Contains("dx"))
            {
                elementValue = "DX";
            }

            if (sourcestring.ToLower().Contains("dy"))
            {
                elementValue = "DY";
            }

            if (sourcestring.ToLower().Contains("lc"))
            {
                elementValue = "LC";
            }

            // Return the result
            return elementValue;
        }

        private void ParseNow(string sourcestring, string lotNumber, string framePosition)
        {

            string value = sourcestring.Replace(" = ", " ").Trim();

            if (sourcestring.ToLower().Contains("diameter =")
                || sourcestring.ToLower().Contains("dx =")
                || sourcestring.ToLower().Contains("dy =")
                || sourcestring.ToLower().Contains("lc ="))
            {

                //sbuilder.AppendLine(value);   // <==== For debuging purposes only

                // Get integer & decimal numbers (positive and negative)
                // Regular Expression = (?:-?\d*\.)?\d+
                // Element              Actual         Nominal         Deviat.         Up Tol.        Low Tol.       Pass/Fail
                // Diameter =           7.9775          8.0000         -0.0225          0.0500         -0.0500            PASS

                string regexPattern = @"(?:-?\d*\.)?\d+";   // Get integer & decimal numbers (positive and negative)
                var results = Regex.Matches(value, regexPattern)
                    .OfType<Match>()
                    .Select(m => m.Groups[0].Value)
                    .ToArray();


                // Check the results have records.
                if (!results.Length.Equals(0))
                {

                    // Create new class
                    GTSMoldLogFile gtsMoldItem = new GTSMoldLogFile();

                    gtsMoldItem.Element = ParseElement(value.Trim());
                    gtsMoldItem.LotNumber = lotNumber;
                    gtsMoldItem.FramePosition = framePosition;
                    gtsMoldItem.Actual = decimal.Parse(results.GetValue(0).ToString());
                    gtsMoldItem.Nominal = decimal.Parse(results.GetValue(1).ToString());
                    gtsMoldItem.Deviation = decimal.Parse(results.GetValue(2).ToString());
                    gtsMoldItem.UpTol = decimal.Parse(results.GetValue(3).ToString());
                    gtsMoldItem.LowTol = decimal.Parse(results.GetValue(4).ToString());
                    gtsMoldItem.Status = (value.Contains("PASS") ? "PASS" : "FAIL");
                    gtsMoldItem.Status2 = "New";
                    gtsMoldItem.DateCreated = DateTime.Now;

                    // Add item to the list
                    GTSMolds.Add(gtsMoldItem);                                                                                   
    
                }

            }
        }
    }

}

using System.Configuration;
using System.Net;
using System.Text;

namespace AttendanceNotification
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void lineNotify(string msg)
        {
            string token = ConfigurationManager.AppSettings.Get("Token");
            try
            {
                //Initial LINE API
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);
                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //
            }
            catch (Exception e)
            {
                //Create Log File
                string LogFilePath = ConfigurationManager.AppSettings.Get("Logfile");
                using (FileStream FileLog = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
                using (StreamWriter WriteFile = new StreamWriter(FileLog))
                {
                    WriteFile.WriteLine(DateTime.Now);
                    WriteFile.WriteLine(e.Message);
                    WriteFile.WriteLine("{0} Exception caught.", e);
                    WriteFile.WriteLine("-----------------------------------------------------------------");
                }
            }
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //Download File from Google Sheet
                string Url = "https://docs.google.com/spreadsheets/d/1ERuiSwPufeUyQ4OTpyX-JrQiODwtPVFhAEt5BZgtN24/export?format=csv&gid=1296848798";
                System.Net.WebClient GoogleSheet = new System.Net.WebClient();
                byte[] buffer = GoogleSheet.DownloadData(Url);
                string Path = ConfigurationManager.AppSettings.Get("ReadFilePath");
                Stream streamFile = new FileStream(Path, FileMode.Create);
                BinaryWriter writeFile = new BinaryWriter(streamFile);
                writeFile.Write(buffer);
                streamFile.Close();
                string[] GetData = System.IO.File.ReadAllLines(ConfigurationManager.AppSettings.Get("ReadFilePath"));
                List<string> listofCheckID = System.IO.File.ReadAllLines(ConfigurationManager.AppSettings.Get("ReadFileID")).ToList();
                //
                // Max People of System D&D
                int MaxSystemDD = 32; //DSI=18 / D&D=14
                int CountSystemDD = 0;
                var listofID = new List<string>();
                var listofgetcheckID = new List<string>();
                // Get date for month format 1-9
                string DateNowM = DateTime.Now.ToString("d/M/yyyy");
                // Get date for month format 10-12
                string DateNowMM = DateTime.Now.ToString("dd/MM/yyyy");
                foreach (string EachlinesData in GetData)
                {
                    string[] GetSplitData = EachlinesData.Split(',');
                    string GetDate = GetSplitData[2];
                    if (GetDate == DateNowM || GetDate == DateNowMM)
                    {
                        if (EachlinesData.Contains("D&D") || EachlinesData.Contains("DSI") && EachlinesData.Contains(GetDate))
                        {
                            string[] CheckDupicateID = EachlinesData.Split(',');
                            // Add data to list
                            listofID.Add(CheckDupicateID[3]);
                        }
                    }
                }
                // Check Duplicate ID
                if (listofID.Count != listofID.Distinct().Count())
                {
                    CountSystemDD = listofID.Distinct().Count();
                    int ResultCountDD = MaxSystemDD - CountSystemDD;
                    if (listofID.Distinct().Count() == MaxSystemDD)
                    {
                        lineNotify("ประจำวันที่: " + DateNowMM + "\n" + "ทำแบบสำรวจครบแล้ว");
                        Application.Exit();
                    }
                    else if (ResultCountDD < MaxSystemDD)
                    {
                        var IDCheckList = listofCheckID.Where(CheckID1 => listofID.All(CheckID2 => CheckID1 != CheckID2));
                        foreach (string LoopGetID in IDCheckList)
                        {
                            listofgetcheckID.Add(LoopGetID);
                        }
                        lineNotify("ประจำวันที่: " + DateNowMM + "\n" + "ยังไม่ได้ทำแบบสำรวจอีก: " + listofgetcheckID.Count() + " ท่าน" + "\n" + "พนักงานหมายเลข: " + "\n" +
                            (String.Join(", ", listofgetcheckID)) + "\n" + "กรุณาทำแบบสำรวจที่: https://forms.gle/wyH5mvAJRnbLA3x47");
                    }
                }
                else
                {
                    int ResultCountDD = MaxSystemDD - listofID.Count;
                    if (listofID.Count == MaxSystemDD)
                    {
                        lineNotify("ประจำวันที่: " + DateNowMM + "\n" + "ทำแบบสำรวจครบแล้ว");
                        Application.Exit();
                    }
                    else if (ResultCountDD < MaxSystemDD)
                    {
                        var IDCheckList = listofCheckID.Where(CheckID1 => listofID.All(CheckID2 => CheckID1 != CheckID2));
                        foreach (string LoopGetID in IDCheckList)
                        {
                            listofgetcheckID.Add(LoopGetID);
                        }
                        lineNotify("ประจำวันที่: " + DateNowMM + "\n" + "ยังไม่ได้ทำแบบสำรวจอีก: " + listofgetcheckID.Count() + " ท่าน" + "\n" + "พนักงานหมายเลข: " + "\n" +
                            (String.Join(", ", listofgetcheckID)) + "\n" + "กรุณาทำแบบสำรวจที่: https://forms.gle/wyH5mvAJRnbLA3x47");
                    }
                }
            }
            catch (Exception ex)
            {
                //Create Log File
                string LogFilePath = ConfigurationManager.AppSettings.Get("Logfile");
                using (FileStream FileLog = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write))
                using (StreamWriter WriteFile = new StreamWriter(FileLog))
                {
                    WriteFile.WriteLine(DateTime.Now);
                    WriteFile.WriteLine(ex.Message);
                    WriteFile.WriteLine("{0} Exception caught.", ex);
                    WriteFile.WriteLine("-----------------------------------------------------------------");
                }
            }
        }
    }
}
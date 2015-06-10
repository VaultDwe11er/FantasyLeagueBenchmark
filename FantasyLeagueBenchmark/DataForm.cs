using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;

namespace FantasyLeagueBenchmark
{
    public partial class DataForm : Form
    {
        public List<int> arrOB, arrCT, arrFH, arrSH, arrLF, arrLK, arrFR;
        public List<int> subOB, subCT, subFH, subSH, subLF, subLK, subFR;

        int cnt;
        int x;
        int cntOB;
        int cntCT;
        int cntFH;
        int cntSH;
        int cntLF;
        int cntLK;
        int cntFR;

        List<Player> players;
        bool isFound = false;
        List<int> resultList;

        MainForm parent;
        int topToSelect;
        XDocument xdoc;

        Thread t1;

        public delegate void ModelComplete();
        public event ModelComplete RaiseModelCompleted;

        public DataForm()
        {
            InitializeComponent();

            xdoc = XDocument.Load("data.xml");

            ProcessPlayerJson();
        }

        public void GetPlayerJson()
        {
            WebRequest request = WebRequest.Create("http://fantasy.udtgames.com/superrugby/json/getAllPlayers1?tournamentId=2");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String responseFromServer = reader.ReadToEnd();

            System.IO.File.WriteAllText("players.json", responseFromServer);

            ProcessPlayerJson();
        }

        public void ProcessPlayerJson()
        {
            String responseFromServer = System.IO.File.ReadAllText("players.json");
            String[][] data = JsonConvert.DeserializeObject<String[][]>(responseFromServer);


            players = new List<Player>();
            cnt = 1;

            foreach (var entry in data)
            {
                Player p = new Player();
                p.Name = entry[13];
                p.Position = entry[6];
                p.Team = entry[7];
                p.Price = double.Parse(entry[3]);
                p.Points = int.Parse(entry[18]);
                p.PercPicked = int.Parse(entry[16]);
                p.Pickable = 1;
                p.IsPicked = 0;

                if (xdoc.XPathSelectElement("Data/Teams/Team[@Name=\"" + p.Team + "\"]").Attribute("Target").Value == "0") p.Pickable = 0;
                if (xdoc.XPathSelectElements("Data/NotPickable/Player[@Name=\"" + p.Name + "\"]").Count() > 0) p.Pickable = 0;

                players.Add(p);
            }

            players.Sort(new PlayerComp());

            int tmp = 1;
            while (dgv.Rows.Count > 0) dgv.Rows.RemoveAt(0);
            foreach (var elem in players)
            {
                dgv.Rows.Add(elem.Name, elem.Position, elem.Team, elem.Price, elem.Points, elem.PercPicked, tmp, elem.IsPicked, elem.Pickable);
                tmp++;
            }

            foreach (var elem in xdoc.XPathSelectElements("Data/ResultList/Result"))
            {
                int pos = int.Parse(elem.Attribute("Value").Value);
                dgv.Rows[pos - 1].Cells[7].Value = 1;
            }
        }

        public void Start(int tts, MainForm sender)
        {
            parent = sender;
            topToSelect = tts;

            t1 = new Thread(RunModel);
            t1.Start();
            //RunModel();
        }

        public void Stop()
        {
            if (!(t1.Equals(null))) t1.Abort();
        }

        public void RunModel()
        {
            isFound = false;
            xdoc = XDocument.Load("data.xml");

            ProcessPlayerJson();

            cnt = 1;
            for (x = topToSelect; x < 500; x++)
            {
                cntOB = 4;
                cntCT = 3;
                cntFH = 2;
                cntSH = 2;
                cntLF = 4;
                cntLK = 3;
                cntFR = 4;

                parent.UpdateStatus("Processing top " + x + " players...");

                arrOB = new List<int>();
                arrCT = new List<int>();
                arrFH = new List<int>();
                arrSH = new List<int>();
                arrLF = new List<int>();
                arrLK = new List<int>();
                arrFR = new List<int>();

                for (int i = 0; i < x - 1; i++)
                {
                    switch (players.ElementAt(i).Position)
                    {
                        case "Outside Back":
                            arrOB.Add(i);
                            break;
                        case "Centre":
                            arrCT.Add(i);
                            break;
                        case "Fly Half":
                            arrFH.Add(i);
                            break;
                        case "Scrum Half":
                            arrSH.Add(i);
                            break;
                        case "Loose Forward":
                            arrLF.Add(i);
                            break;
                        case "Lock":
                            arrLK.Add(i);
                            break;
                        case "Front Row":
                            arrFR.Add(i);
                            break;
                        default:
                            throw new KeyNotFoundException("Position not found");
                    }

                }

                switch (players.ElementAt(x-1).Position)
                {
                    case "Outside Back":
                        cntOB--;
                        break;
                    case "Centre":
                        cntCT--;
                        break;
                    case "Fly Half":
                        cntFH--;
                        break;
                    case "Scrum Half":
                        cntSH--;
                        break;
                    case "Loose Forward":
                        cntLF--;
                        break;
                    case "Lock":
                        cntLK--;
                        break;
                    case "Front Row":
                        cntFR--;
                        break;
                    default:
                        throw new KeyNotFoundException("Position not found");
                }

                ProcessSubsetsOB();

                if (isFound) break;
            }
        }

        //OB
        void ProcessSubsetsOB()
        {
            int[] subset = new int[cntOB];

            ProcessLargerSubsetsOB(subset, 0, 0);
        }

        void ProcessLargerSubsetsOB(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessOB(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrOB.Count; j++)
                {
                    subset[subsetSize] = arrOB[j];
                    ProcessLargerSubsetsOB(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessOB(int[] subset)
        {
            subOB = subset.ToList();
            ProcessSubsetsCT();
        }

        //CT
        void ProcessSubsetsCT()
        {
            int[] subset = new int[cntCT];

            ProcessLargerSubsetsCT(subset, 0, 0);
        }

        void ProcessLargerSubsetsCT(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessCT(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrCT.Count; j++)
                {
                    subset[subsetSize] = arrCT[j];
                    ProcessLargerSubsetsCT(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessCT(int[] subset)
        {
            subCT = subset.ToList();
            ProcessSubsetsFH();
        }

        //FH
        void ProcessSubsetsFH()
        {
            int[] subset = new int[cntFH];

            ProcessLargerSubsetsFH(subset, 0, 0);
        }

        void ProcessLargerSubsetsFH(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessFH(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrFH.Count; j++)
                {
                    subset[subsetSize] = arrFH[j];
                    ProcessLargerSubsetsFH(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessFH(int[] subset)
        {
            subFH = subset.ToList();
            ProcessSubsetsSH();
        }

        //SH
        void ProcessSubsetsSH()
        {
            int[] subset = new int[cntSH];

            ProcessLargerSubsetsSH(subset, 0, 0);
        }

        void ProcessLargerSubsetsSH(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessSH(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrSH.Count; j++)
                {
                    subset[subsetSize] = arrSH[j];
                    ProcessLargerSubsetsSH(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessSH(int[] subset)
        {
            subSH = subset.ToList();
            ProcessSubsetsLF();
        }

        //LF
        void ProcessSubsetsLF()
        {
            int[] subset = new int[cntLF];

            ProcessLargerSubsetsLF(subset, 0, 0);
        }

        void ProcessLargerSubsetsLF(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessLF(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrLF.Count; j++)
                {
                    subset[subsetSize] = arrLF[j];
                    ProcessLargerSubsetsLF(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessLF(int[] subset)
        {
            subLF = subset.ToList();
            ProcessSubsetsLK();
        }

        //LK
        void ProcessSubsetsLK()
        {
            int[] subset = new int[cntLK];

            ProcessLargerSubsetsLK(subset, 0, 0);
        }

        void ProcessLargerSubsetsLK(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessLK(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrLK.Count; j++)
                {
                    subset[subsetSize] = arrLK[j];
                    ProcessLargerSubsetsLK(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessLK(int[] subset)
        {
            subLK = subset.ToList();
            ProcessSubsetsFR();
        }

        //FR
        void ProcessSubsetsFR()
        {
            int[] subset = new int[cntFR];

            ProcessLargerSubsetsFR(subset, 0, 0);
        }

        void ProcessLargerSubsetsFR(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                if (cnt % 1000 == 0) parent.UpdateStatus("Processing " + cnt + " (top " + x + " players)...");

                ProcessFR(subset);
                cnt++;
            }
            else
            {
                for (int j = nextIndex; j < arrFR.Count; j++)
                {
                    subset[subsetSize] = arrFR[j];
                    ProcessLargerSubsetsFR(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessFR(int[] subset)
        {
            subFR = subset.ToList();

            resultList = new List<int>();
            resultList.AddRange(subOB);
            resultList.AddRange(subCT);
            resultList.AddRange(subFH);
            resultList.AddRange(subSH);
            resultList.AddRange(subLF);
            resultList.AddRange(subLK);
            resultList.AddRange(subFR);
            resultList.Add(x - 1);

            CheckFound();
        }

        void CheckFound()
        {
            int tgtPercPicked = parent.TargetPercPicked;
            double tgtCost = parent.TargetCost;

            players.ForEach(x => x.IsPicked = 0);

            foreach (int i in resultList)
            {
                players.ElementAt(i).IsPicked = 1;
            }

            isFound = (players.Where(x => x.IsPicked == 1).Sum(x => x.Price) <= tgtCost
                && players.Where(x => x.IsPicked == 1).Sum(x => x.PercPicked) >= tgtPercPicked);

            if (isFound)
            {
                foreach (DataGridViewRow row in parent.DgvTeams.Rows)
                {
                    isFound = (players.Where(x => x.IsPicked == 1 && x.Team == row.Cells[0].Value.ToString()).Count() <=
                        int.Parse(row.Cells[2].Value.ToString()));

                    if (!isFound) break;
                }
            }

            if (isFound)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    dgv.Rows[i].Cells[7].Value = players.ElementAt(i).IsPicked;
                }

                foreach (var elem in xdoc.XPathSelectElements("Data/Teams/Team"))
                {
                    elem.Attribute("Current").Value =
                        players.Where(x => x.IsPicked == 1 && x.Team == elem.Attribute("Name").Value).Count().ToString();
                }

                xdoc.XPathSelectElement("Data/Cost").Attribute("Actual").Value = players.Where(x => x.IsPicked == 1).Sum(x => x.Price).ToString();
                xdoc.XPathSelectElement("Data/PercPicked").Attribute("Actual").Value = players.Where(x => x.IsPicked == 1).Sum(x => x.PercPicked).ToString();

                xdoc.XPathSelectElement("Data/ResultList").RemoveNodes();

                foreach (int result in resultList)
                {
                    XElement elem = new XElement("Result");
                    elem.Add(new XAttribute("Value", result + 1));
                    xdoc.XPathSelectElement("Data/ResultList").Add(elem);
                }

                xdoc.Save("data.xml");

                RaiseModelCompleted();

                MessageBox.Show("Done");
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}

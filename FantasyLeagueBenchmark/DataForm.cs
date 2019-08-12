using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO.Compression;

namespace FantasyLeagueBenchmark
{
    public enum SupportedSite
    {
        UDT,
        PRO14,
        FOX,
        SportsDeck,
        PremierLeague
    }
    public partial class DataForm : Form
    {
        public List<int> arrOB, arrCT, arrFH, arrSH, arrLF, arrLK, arrHK, arrPR, arrFR, arrFWD, arrMID, arrDEF, arrGKP;
        public List<int> subOB, subCT, subFH, subSH, subLF, subLK, subHK, subPR, subFR, subFWD, subMID, subDEF, subGKP;
        public int cntOB, cntCT, cntFH, cntSH, cntLF, cntLK, cntHK, cntPR, cntFR, cntFWD, cntMID, cntDEF, cntGKP;

        int cnt;
        int x;

        List<Player> players;
        bool isFound = false;
        List<int> resultList;

        MainForm parent;
        int topToSelect;
        XDocument xdoc;

        BackgroundWorker bgw;

        public delegate void ModelComplete();
        public event ModelComplete RaiseModelCompleted;

        SupportedSite site;

        public DataForm(string s)
        {
            InitializeComponent();

            ChangeSupportedSite(s);
            
            bgw = new BackgroundWorker();
            bgw.DoWork += bgw_DoWork;
            bgw.ProgressChanged += bgw_ProgressChanged;
            bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
            bgw.WorkerSupportsCancellation = true;

            xdoc = XDocument.Load("data_" + s + ".xml");

            ProcessPlayerJson();
        }

        public void GetPlayerJson(String url, MainForm mf)
        {
            String responseFromServer = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                WebRequest request = WebRequest.Create(url);

                WebProxy webProxy = new WebProxy("http://172.17.6.119:8080/", true)
                {
                    UseDefaultCredentials = true
                };

                request.Proxy = webProxy;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader;

                if (response.ContentEncoding == "gzip")
                {
                    using (GZipStream gzipstream = new GZipStream(dataStream, CompressionMode.Decompress))
                    {
                        reader = new StreamReader(gzipstream);
                        responseFromServer = reader.ReadToEnd();
                    }
                }
                else
                {
                    reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }
            }
            catch (UriFormatException)
            {
                mf.UpdateStatus("Invalid URL");
                return;
            }

            File.WriteAllText("players_" + site.ToString() + ".json", responseFromServer);

            ProcessPlayerJson();
        }

        public void ProcessPlayerJson()
        {
            String responseFromServer = File.ReadAllText("players_" + site.ToString() + ".json");

            players = new List<Player>();
            cnt = 1;

            switch (site)
            {
                case SupportedSite.UDT:

                    String[][] udtdata = JsonConvert.DeserializeObject<String[][]>(responseFromServer);

                    foreach (var entry in udtdata)
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

                        var team = xdoc.XPathSelectElement("Data/Teams/Team[@Name=\"" + p.Team + "\"]");
                        if (team == null) p.Pickable = 0;
                        else if (team.Attribute("Target").Value == "0") p.Pickable = 0;
                        if (xdoc.XPathSelectElements("Data/NotPickable/Player[@Name=\"" + p.Name + "\"]").Count() > 0) p.Pickable = 0;

                        players.Add(p);
                    }
                    break;

                case SupportedSite.PRO14:
                    Pro14Json pro14data = JsonConvert.DeserializeObject<Pro14Json>(responseFromServer);

                    foreach (var entry in pro14data.players)
                    {
                        Player p = new Player();

                        p.Name = entry.info1;

                        switch (entry.categoryId)
                        {
                            case 8:
                                p.Position = "Front Row";
                                break;
                            case 9:
                                p.Position = "Lock";
                                break;
                            case 10:
                                p.Position = "Loose Forward";
                                break;
                            case 11:
                                p.Position = "Scrum Half";
                                break;
                            case 12:
                                p.Position = "Fly Half";
                                break;
                            case 13:
                                p.Position = "Centre";
                                break;
                            case 14:
                                p.Position = "Outside Back";
                                break;
                            default:
                                throw new KeyNotFoundException("Position not found");
                        }

                        switch (entry.sideId)
                        {
                            case 1:
                                p.Team = "Benetton Rugby Treviso";
                                break;
                            case 2:
                                p.Team = "Cardiff Blues";
                                break;
                            case 3:
                                p.Team = "Connacht Rugby";
                                break;
                            case 4:
                                p.Team = "Edinburgh Rugby";
                                break;
                            case 5:
                                p.Team = "Glasgow Warriors";
                                break;
                            case 6:
                                p.Team = "Leinster Rugby";
                                break;
                            case 7:
                                p.Team = "Munster Rugby";
                                break;
                            case 8:
                                p.Team = "Dragons Rugby";
                                break;
                            case 9:
                                p.Team = "Ospreys";
                                break;
                            case 10:
                                p.Team = "Scarlets";
                                break;
                            case 11:
                                p.Team = "Ulster Rugby";
                                break;
                            case 12:
                                p.Team = "Zebre Rugby";
                                break;
                            case 13:
                                p.Team = "Southern Kings";
                                break;
                            case 14:
                                p.Team = "Toyota Cheetahs";
                                break;
                            default:
                                throw new KeyNotFoundException("Team not found");
                        }
                        p.Price = entry.value;
                        p.Points = entry.totalPoints;
                        p.PercPicked = entry.teams_selected;
                        p.Pickable = 1;
                        p.IsPicked = 0;


                        var team = xdoc.XPathSelectElement("Data/Teams/Team[@Name=\"" + p.Team + "\"]");
                        if (team == null) p.Pickable = 0;
                        else if (team.Attribute("Target").Value == "0") p.Pickable = 0;
                        if (xdoc.XPathSelectElements("Data/NotPickable/Player[@Name=\"" + p.Name + "\"]").Count() > 0) p.Pickable = 0;

                        players.Add(p);
                    }
                    break;

                case SupportedSite.FOX:

                    FoxJson[] foxdata = JsonConvert.DeserializeObject<FoxJson[]>(responseFromServer);

                    foreach (var entry in foxdata)
                    {
                        Player p = new Player();

                        p.Name = entry.first_name + " " + entry.last_name;

                        switch (entry.positions[0])
                        {
                            case 1:
                                p.Position = "Prop";
                                break;
                            case 2:
                                p.Position = "Hooker";
                                break;
                            case 3:
                                p.Position = "Lock";
                                break;
                            case 4:
                                p.Position = "Loose Forward";
                                break;
                            case 5:
                                p.Position = "Scrum Half";
                                break;
                            case 6:
                                p.Position = "Fly Half";
                                break;
                            case 7:
                                p.Position = "Centre";
                                break;
                            case 8:
                                p.Position = "Outside Back";
                                break;
                            default:
                                throw new KeyNotFoundException("Position not found");
                        }

                        switch (entry.squad_id)
                        {
                            case 50001:
                                p.Team = "Crusaders";
                                break;
                            case 50002:
                                p.Team = "Brumbies";
                                break;
                            case 50003:
                                p.Team = "Highlanders";
                                break;
                            case 50004:
                                p.Team = "Waratahs";
                                break;
                            case 50005:
                                p.Team = "Reds";
                                break;
                            case 50006:
                                p.Team = "Stormers";
                                break;
                            case 50007:
                                p.Team = "Bulls";
                                break;
                            case 50008:
                                p.Team = "Sharks";
                                break;
                            case 50009:
                                p.Team = "Lions";
                                break;
                            case 50010:
                                p.Team = "Chiefs";
                                break;
                            case 50011:
                                p.Team = "Hurricanes";
                                break;
                            case 50012:
                                p.Team = "Blues";
                                break;
                            case 50051:
                                p.Team = "Rebels";
                                break;
                            case 50115:
                                p.Team = "Sunwolves";
                                break;
                            case 50116:
                                p.Team = "Jaguares";
                                break;
                            default:
                                throw new KeyNotFoundException("Team not found");
                        }
                        p.Price = entry.cost;
                        p.Points = entry.stats.total_points;
                        p.PercPicked = entry.stats.owned_by;
                        p.Pickable = 1;
                        p.IsPicked = 0;


                        var team = xdoc.XPathSelectElement("Data/Teams/Team[@Name=\"" + p.Team + "\"]");
                        if (team == null) p.Pickable = 0;
                        else if (team.Attribute("Target").Value == "0") p.Pickable = 0;
                        if (xdoc.XPathSelectElements("Data/NotPickable/Player[@Name=\"" + p.Name + "\"]").Count() > 0) p.Pickable = 0;

                        players.Add(p);
                    }
                    break;

                case SupportedSite.SportsDeck:
                    SportsDeckJson[] sportsdeckdata = JsonConvert.DeserializeObject<SportsDeckJson[]>(responseFromServer);

                    foreach (var entry in sportsdeckdata)
                    {
                        Player p = new Player();

                        p.Name = entry.first_name + " " + entry.last_name;
                        p.Position = entry.positions[0].position_long;
                        p.Team = entry.team.name;
                        p.Price = entry.player_stats[0].price;
                        p.Points = entry.player_stats[0].total_points;
                        p.PercPicked = entry.player_stats[0].owned;
                        p.Pickable = 1;
                        p.IsPicked = 0;

                        var team = xdoc.XPathSelectElement("Data/Teams/Team[@Name=\"" + p.Team + "\"]");
                        if (team == null) p.Pickable = 0;
                        else if (team.Attribute("Target").Value == "0") p.Pickable = 0;
                        if (xdoc.XPathSelectElements("Data/NotPickable/Player[@Name=\"" + p.Name + "\"]").Count() > 0) p.Pickable = 0;

                        players.Add(p);
                    }
                    break;

                case SupportedSite.PremierLeague:
                    PremierLeagueJSON premierleaguedata = JsonConvert.DeserializeObject<PremierLeagueJSON>(responseFromServer);

                    foreach (var entry in premierleaguedata.elements)
                    {
                        Player p = new Player();

                        p.Name = entry.first_name + " " + entry.second_name;

                        switch(entry.element_type)
                        {
                            case 1:
                                p.Position = "Goalkeeper";
                                break;
                            case 2:
                                p.Position = "Defender";
                                break;
                            case 3:
                                p.Position = "Midfielder";
                                break;
                            case 4:
                                p.Position = "Forward";
                                break;
                        }
                        switch (entry.team_code)
                        {
                            case 3:
                                p.Team = "Arsenal";
                                break;
                            case 7:
                                p.Team = "Aston Villa";
                                break;
                            case 91:
                                p.Team = "Bournemouth";
                                break;
                            case 36:
                                p.Team = "Brighton";
                                break;
                            case 90:
                                p.Team = "Burnley";
                                break;
                            case 8:
                                p.Team = "Chelsea";
                                break;
                            case 31:
                                p.Team = "Crystal Palace";
                                break;
                            case 11:
                                p.Team = "Everton";
                                break;
                            case 13:
                                p.Team = "Leicester";
                                break;
                            case 14:
                                p.Team = "Liverpool";
                                break;
                            case 43:
                                p.Team = "Man City";
                                break;
                            case 1:
                                p.Team = "Man Utd";
                                break;
                            case 4:
                                p.Team = "Newcastle";
                                break;
                            case 45:
                                p.Team = "Norwich";
                                break;
                            case 49:
                                p.Team = "Sheffield Utd";
                                break;
                            case 20:
                                p.Team = "Southampton";
                                break;
                            case 6:
                                p.Team = "Spurs";
                                break;
                            case 57:
                                p.Team = "Watford";
                                break;
                            case 21:
                                p.Team = "Westham";
                                break;
                            case 39:
                                p.Team = "Wolves";
                                break;
                            default:
                                throw new KeyNotFoundException("Team not found");

                        }
                        p.Price = entry.now_cost / 10;
                        p.Points = entry.total_points;
                        p.PercPicked = entry.selected_by_percent;
                        p.Pickable = 1;
                        p.IsPicked = 0;

                        var team = xdoc.XPathSelectElement("Data/Teams/Team[@Name=\"" + p.Team + "\"]");
                        if (team == null) p.Pickable = 0;
                        else if (team.Attribute("Target").Value == "0") p.Pickable = 0;
                        if (xdoc.XPathSelectElements("Data/NotPickable/Player[@Name=\"" + p.Name + "\"]").Count() > 0) p.Pickable = 0;

                        players.Add(p);
                    }
                    break;

                default:
                    throw new NotSupportedException("Site is not supported");
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

            bgw.RunWorkerAsync();
        }

        void bgw_DoWork(object sender, EventArgs e)
        {
            RunModel();
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RaiseModelCompleted();
        }

        public void Stop()
        {
            bgw.CancelAsync();
        }

        void RunModel()
        {
            isFound = false;
            xdoc = XDocument.Load("data_" + site.ToString() + ".xml");

            ProcessPlayerJson();

            cntOB = 0;
            cntCT = 0;
            cntFH = 0;
            cntSH = 0;
            cntLF = 0;
            cntLK = 0;
            cntHK = 0;
            cntPR = 0;
            cntFR = 0;
            cntFWD = 0;
            cntMID = 0;
            cntDEF = 0;
            cntGKP = 0;

            cnt = 1;
            for (x = topToSelect; x < 500; x++)
            {
                switch (site)
                {
                    case SupportedSite.UDT:
                        cntOB = 4;
                        cntCT = 3;
                        cntFH = 2;
                        cntSH = 2;
                        cntLF = 3;
                        cntLK = 3;
                        cntFR = 4;

                        break;
                    case SupportedSite.PRO14:
                        cntOB = 3;
                        cntCT = 2;
                        cntFH = 1;
                        cntSH = 1;
                        cntLF = 3;
                        cntLK = 2;
                        cntFR = 3;
                        break;
                    case SupportedSite.FOX:
                        cntOB = 4;
                        cntCT = 3;
                        cntFH = 2;
                        cntSH = 2;
                        cntLF = 4;
                        cntLK = 3;
                        cntHK = 2;
                        cntPR = 3;
                        break;
                    case SupportedSite.SportsDeck:
                        cntOB = 4;
                        cntCT = 3;
                        cntFH = 2;
                        cntSH = 2;
                        cntLF = 4;
                        cntLK = 3;
                        cntHK = 2;
                        cntPR = 3;
                        break;
                    case SupportedSite.PremierLeague:
                        cntFWD = 3;
                        cntMID = 5;
                        cntDEF = 5;
                        cntGKP = 2;
                        break;
                    default:
                        throw new NotSupportedException("Site is not supported");
                }

                parent.UpdateStatus("Processing top " + x + " players...");

                arrOB = new List<int>();
                arrCT = new List<int>();
                arrFH = new List<int>();
                arrSH = new List<int>();
                arrLF = new List<int>();
                arrLK = new List<int>();
                arrHK = new List<int>();
                arrPR = new List<int>();
                arrFR = new List<int>();
                arrFWD = new List<int>();
                arrMID = new List<int>();
                arrDEF = new List<int>();
                arrGKP = new List<int>();

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
                        case "Backrower":
                            arrLF.Add(i);
                            break;
                        case "Lock":
                            arrLK.Add(i);
                            break;
                        case "Hooker":
                            arrHK.Add(i);
                            break;
                        case "Prop":
                            arrPR.Add(i);
                            break;
                        case "Front Row":
                            arrFR.Add(i);
                            break;
                        case "Forward":
                            arrFWD.Add(i);
                            break;
                        case "Midfielder":
                            arrMID.Add(i);
                            break;
                        case "Defender":
                            arrDEF.Add(i);
                            break;
                        case "Goalkeeper":
                            arrGKP.Add(i);
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
                    case "Backrower":
                        cntLF--;
                        break;
                    case "Lock":
                        cntLK--;
                        break;
                    case "Hooker":
                        cntHK--;
                        break;
                    case "Prop":
                        cntPR--;
                        break;
                    case "Front Row":
                        cntFR--;
                        break;
                    case "Forward":
                        cntFWD--;
                        break;
                    case "Midfielder":
                        cntMID--;
                        break;
                    case "Defender":
                        cntDEF--;
                        break;
                    case "Goalkeeper":
                        cntGKP--;
                        break;
                    default:
                        throw new KeyNotFoundException("Position not found");
                }

                ProcessSubsetsOB();

                if (isFound || bgw.CancellationPending) break;
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
            if (isFound || bgw.CancellationPending)
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
            if (isFound || bgw.CancellationPending)
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
            if (isFound || bgw.CancellationPending)
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
            if (isFound || bgw.CancellationPending)
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
            if (isFound || bgw.CancellationPending)
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
            if (isFound || bgw.CancellationPending)
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
            ProcessSubsetsHK();
        }

        //HK
        void ProcessSubsetsHK()
        {
            int[] subset = new int[cntHK];

            ProcessLargerSubsetsHK(subset, 0, 0);
        }

        void ProcessLargerSubsetsHK(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound || bgw.CancellationPending)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessHK(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrHK.Count; j++)
                {
                    subset[subsetSize] = arrHK[j];
                    ProcessLargerSubsetsHK(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessHK(int[] subset)
        {
            subHK = subset.ToList();
            ProcessSubsetsPR();
        }

        //PR
        void ProcessSubsetsPR()
        {
            int[] subset = new int[cntPR];

            ProcessLargerSubsetsPR(subset, 0, 0);
        }

        void ProcessLargerSubsetsPR(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound || bgw.CancellationPending)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessPR(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrPR.Count; j++)
                {
                    subset[subsetSize] = arrPR[j];
                    ProcessLargerSubsetsPR(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessPR(int[] subset)
        {
            subPR = subset.ToList();
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
            if (isFound || bgw.CancellationPending)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessFR(subset);
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
            ProcessSubsetsFWD();
        }

        //FWD
        void ProcessSubsetsFWD()
        {
            int[] subset = new int[cntFWD];

            ProcessLargerSubsetsFWD(subset, 0, 0);
        }

        void ProcessLargerSubsetsFWD(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound || bgw.CancellationPending)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessFWD(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrFWD.Count; j++)
                {
                    subset[subsetSize] = arrFWD[j];
                    ProcessLargerSubsetsFWD(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessFWD(int[] subset)
        {
            subFWD = subset.ToList();
            ProcessSubsetsMID();
        }

        //MID
        void ProcessSubsetsMID()
        {
            int[] subset = new int[cntMID];

            ProcessLargerSubsetsMID(subset, 0, 0);
        }

        void ProcessLargerSubsetsMID(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound || bgw.CancellationPending)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessMID(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrMID.Count; j++)
                {
                    subset[subsetSize] = arrMID[j];
                    ProcessLargerSubsetsMID(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessMID(int[] subset)
        {
            subMID = subset.ToList();
            ProcessSubsetsDEF();
        }

        //DEF
        void ProcessSubsetsDEF()
        {
            int[] subset = new int[cntDEF];

            ProcessLargerSubsetsDEF(subset, 0, 0);
        }

        void ProcessLargerSubsetsDEF(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound || bgw.CancellationPending)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                ProcessDEF(subset);
            }
            else
            {
                for (int j = nextIndex; j < arrDEF.Count; j++)
                {
                    subset[subsetSize] = arrDEF[j];
                    ProcessLargerSubsetsDEF(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessDEF(int[] subset)
        {
            subDEF = subset.ToList();
            ProcessSubsetsGKP();
        }

        //GKP
        void ProcessSubsetsGKP()
        {
            int[] subset = new int[cntGKP];

            ProcessLargerSubsetsGKP(subset, 0, 0);
        }

        void ProcessLargerSubsetsGKP(int[] subset, int subsetSize, int nextIndex)
        {
            if (isFound || bgw.CancellationPending)
            {
                return;
            }
            else if (subsetSize == subset.Count())
            {
                if (cnt % 1000 == 0) parent.UpdateStatus("Processing " + cnt + " (top " + x + " players)...");

                ProcessGKP(subset);
                cnt++;
            }
            else
            {
                for (int j = nextIndex; j < arrGKP.Count; j++)
                {
                    subset[subsetSize] = arrGKP[j];
                    ProcessLargerSubsetsGKP(subset, subsetSize + 1, j + 1);
                }
            }
        }
        void ProcessGKP(int[] subset)
        {
            subGKP = subset.ToList();

            resultList = new List<int>();
            resultList.AddRange(subOB);
            resultList.AddRange(subCT);
            resultList.AddRange(subFH);
            resultList.AddRange(subSH);
            resultList.AddRange(subLF);
            resultList.AddRange(subLK);
            resultList.AddRange(subHK);
            resultList.AddRange(subPR);
            resultList.AddRange(subFR);
            resultList.AddRange(subFWD);
            resultList.AddRange(subMID);
            resultList.AddRange(subDEF);
            resultList.AddRange(subGKP);
            resultList.Add(x - 1);

            CheckFound();
        }

        void CheckFound()
        {
            double tgtPercPicked = parent.TargetPercPicked;
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
                    if (!row.IsNewRow)
                    {
                        isFound = (players.Where(x => x.IsPicked == 1 && x.Team == row.Cells[0].Value.ToString()).Count() <=
                            int.Parse(row.Cells[2].Value.ToString()));

                        if (!isFound) break;
                    }
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

                xdoc.Save("data_" + site.ToString() + ".xml");

                MessageBox.Show("Done");
            }

        }

        public void ChangeSupportedSite(string s)
        {
            bool result = Enum.TryParse<SupportedSite>(s, out site);
            if (!result) throw new NotSupportedException("Site is not supported");
        }
    }
}

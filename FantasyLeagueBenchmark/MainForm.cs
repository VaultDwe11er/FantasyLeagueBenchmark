using System;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;

namespace FantasyLeagueBenchmark
{
    public partial class MainForm : Form
    {
        DataForm df;
        XDocument xdoc;

        public MainForm()
        {
            InitializeComponent();

            cbSite.DataSource = Enum.GetValues(typeof(SupportedSite));

            UpdateValues();

            df = new DataForm(cbSite.Text);
            df.RaiseModelCompleted += df_RaiseModelCompleted;
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (df.IsDisposed)
            {
                df = new DataForm(cbSite.Text);
                df.RaiseModelCompleted += df_RaiseModelCompleted;
            }

            SaveData();

            string url = xdoc.XPathSelectElement("Data/Url").Value;
            df.GetPlayerJson(url, this, cbProxy.Checked);
        }

        private void btnRunModel_Click(object sender, EventArgs e)
        {
            if (df.IsDisposed)
            {
                df = new DataForm(cbSite.Text);
                df.RaiseModelCompleted += df_RaiseModelCompleted;
            }

            SaveData();

            btnRunModel.Text = "Stop Model";
            btnRunModel.Click -= btnRunModel_Click;
            btnRunModel.Click += btnStop_Click;

            df.Start(int.Parse(tbSelect.Text), this);
        }

        public void UpdateStatus(String text)
        {
            toolStripStatusLabel1.Text = text;
        }

        private void btnShowData_Click(object sender, EventArgs e)
        {
            if (df.IsDisposed)
            {
                df = new DataForm(cbSite.Text);
                df.RaiseModelCompleted += df_RaiseModelCompleted;
            }
            df.Show();
        }

        public decimal TargetPercPicked
        {
            get { return decimal.Parse(dgvPercPicked.Rows[0].Cells[1].Value.ToString()); }
        }

        public decimal TargetCost
        {
            get { return decimal.Parse(dgvCost.Rows[0].Cells[1].Value.ToString()); }
        }

        public DataGridView DgvTeams
        {
            get { return dgvTeams; }
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            df.Stop();
        }

        public void UpdateValues()
        {
            string strSite = File.ReadAllText("site.txt");
            cbSite.Text = strSite;

            xdoc = XDocument.Load("data_" + strSite + ".xml");

            //TODO: handle case where file does not exist

            while (dgvTeams.Rows.Count > 1) dgvTeams.Rows.RemoveAt(0);
            foreach (var node in xdoc.XPathSelectElements("Data/Teams/Team"))
            {
                dgvTeams.Rows.Add(node.Attribute("Name").Value, node.Attribute("Current").Value, node.Attribute("Target").Value, 0);
            }

            while (dgvNotPickable.Rows.Count > 1) dgvNotPickable.Rows.RemoveAt(0);
            foreach (var node in xdoc.XPathSelectElements("Data/NotPickable/Player"))
            {
                dgvNotPickable.Rows.Add(node.Attribute("Name").Value);
            }

            decimal targetCost = decimal.Parse(xdoc.XPathSelectElement("Data/Cost").Attribute("Target").Value);
            decimal actualCost = decimal.Parse(xdoc.XPathSelectElement("Data/Cost").Attribute("Actual").Value);
            decimal costInvalid = actualCost <= targetCost ? 0 : 1;

            if (dgvCost.Rows.Count == 1) dgvCost.Rows.RemoveAt(0);
            dgvCost.Rows.Add(actualCost, targetCost, costInvalid);

            decimal targetPercPicked = decimal.Parse(xdoc.XPathSelectElement("Data/PercPicked").Attribute("Target").Value);
            decimal actualPercPicked = decimal.Parse(xdoc.XPathSelectElement("Data/PercPicked").Attribute("Actual").Value);
            int percPickedInvalid = actualPercPicked >= targetPercPicked ? 0 : 1;

            if (dgvPercPicked.Rows.Count == 1) dgvPercPicked.Rows.RemoveAt(0);
            dgvPercPicked.Rows.Add(actualPercPicked, targetPercPicked, percPickedInvalid);

            tbSelect.Text = xdoc.XPathSelectElement("Data/TopSelect").Attribute("Value").Value;
        }

        public void SaveData()
        {
            xdoc.XPathSelectElement("Data/Cost").Attribute("Target").Value = dgvCost.Rows[0].Cells[1].Value.ToString();
            xdoc.XPathSelectElement("Data/PercPicked").Attribute("Target").Value = dgvPercPicked.Rows[0].Cells[1].Value.ToString();
            xdoc.XPathSelectElement("Data/TopSelect").Attribute("Value").Value = tbSelect.Text;

            xdoc.XPathSelectElement("Data/Teams").RemoveNodes();

            foreach (DataGridViewRow row in dgvTeams.Rows)
            {
                if (!row.IsNewRow)
                {
                    XElement elem = new XElement("Team");
                    elem.Add(new XAttribute("Name", row.Cells[0].Value));
                    elem.Add(new XAttribute("Current", row.Cells[1].Value ?? 0));
                    elem.Add(new XAttribute("Target", row.Cells[2].Value));

                    xdoc.XPathSelectElement("Data/Teams").Add(elem);
                }
            }

            xdoc.XPathSelectElement("Data/NotPickable").RemoveNodes();

            foreach (DataGridViewRow row in dgvNotPickable.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    XElement elem = new XElement("Player");
                    elem.Add(new XAttribute("Name", row.Cells[0].Value.ToString()));
                    xdoc.XPathSelectElement("Data/NotPickable").Add(elem);
                }
            }

            xdoc.Save("data_" + cbSite.Text + ".xml");
        }


        void df_RaiseModelCompleted()
        {
            BeginInvoke(new DefaultDelegate(UpdateValues));

            btnRunModel.Text = "Run Model";
            btnRunModel.Click -= btnStop_Click;
            btnRunModel.Click += btnRunModel_Click;

        }

        public delegate void DefaultDelegate();

        private void cbSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (df != null)
            {
                var cb = (ComboBox)sender;
                df.ChangeSupportedSite(cb.Text);
                File.WriteAllText("site.txt", cb.Text);
                UpdateValues();
                if (File.Exists("players_" + cb.Text + ".json"))
                {
                    df.ProcessPlayerJson();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace FantasyLeagueBenchmark
{
    public partial class MainForm : Form
    {
        DataForm df;
        XDocument xdoc;
        
        public MainForm()
        {
            InitializeComponent();

            df = new DataForm();
            df.RaiseModelCompleted += df_RaiseModelCompleted;

            UpdateValues();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (df.IsDisposed)
            {
                df = new DataForm();
                df.RaiseModelCompleted += df_RaiseModelCompleted;
            }

            SaveData();

            df.GetPlayerJson(tbURL.Text, this);
        }

        private void btnRunModel_Click(object sender, EventArgs e)
        {
            if (df.IsDisposed)
            {
                df = new DataForm();
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
                df = new DataForm();
                df.RaiseModelCompleted += df_RaiseModelCompleted;
            }
            df.Show();
        }

        public int TargetPercPicked
        {
            get { return int.Parse(dgvPercPicked.Rows[0].Cells[1].Value.ToString()); }
        }

        public double TargetCost
        {
            get { return double.Parse(dgvCost.Rows[0].Cells[1].Value.ToString()); }
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
            xdoc = XDocument.Load("data.xml");

            tbURL.Text = xdoc.XPathSelectElement("Data/Url").Value;

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

            double targetCost = double.Parse(xdoc.XPathSelectElement("Data/Cost").Attribute("Target").Value);
            double actualCost = double.Parse(xdoc.XPathSelectElement("Data/Cost").Attribute("Actual").Value);
            double costInvalid = actualCost <= targetCost ? 0 : 1;

            if(dgvCost.Rows.Count == 1) dgvCost.Rows.RemoveAt(0);
            dgvCost.Rows.Add(actualCost, targetCost, costInvalid);

            int targetPercPicked = int.Parse(xdoc.XPathSelectElement("Data/PercPicked").Attribute("Target").Value);
            int actualPercPicked = int.Parse(xdoc.XPathSelectElement("Data/PercPicked").Attribute("Actual").Value);
            int percPickedInvalid = actualPercPicked >= targetPercPicked ? 0 : 1;

            if (dgvPercPicked.Rows.Count == 1) dgvPercPicked.Rows.RemoveAt(0);
            dgvPercPicked.Rows.Add(actualPercPicked, targetPercPicked, percPickedInvalid);

            tbSelect.Text = xdoc.XPathSelectElement("Data/TopSelect").Attribute("Value").Value;
        }

        public void SaveData()
        {
            xdoc.XPathSelectElement("Data/Url").Value = tbURL.Text;
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

            xdoc.Save("data.xml");
        }


        void df_RaiseModelCompleted()
        {
            BeginInvoke(new DefaultDelegate(UpdateValues));

            btnRunModel.Text = "Run Model";
            btnRunModel.Click -= btnStop_Click;
            btnRunModel.Click += btnRunModel_Click;

        }

        public delegate void DefaultDelegate();
    }
}

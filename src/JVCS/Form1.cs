using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JVCS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_store_Click(object sender, EventArgs e)
        {
            try
            {
                List<string>? readAllLines = null;
                if (this.tb_filter.Text is var fitler and not null and not "")
                {
                    readAllLines = File.ReadAllLines(fitler).ToList();
                }

                VCS v = new VCS(this.tb_repoPath.Text);
                v.ImportPatch(this.tb_lolpath.Text, readAllLines);
                MessageBox.Show("Import Finish!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btn_restore_Click(object sender, EventArgs e)
        {
            try
            {
                VCS v = new VCS(this.tb_repoPath.Text);
                v.ExportPatch(this.tb_version.Text, this.tb_exportPath.Text, true);
                MessageBox.Show("Export Finish!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btn_loadVersions_Click(object sender, EventArgs e)
        {
            VCS v = new VCS(this.tb_repoPath.Text);
            var listVersions = v.GetPatchList();
            this.listBox1.Items.Clear();
            foreach (var version in listVersions)
            {
                this.listBox1.Items.Add(version);
            }
        }

        private void btn_version_Select_Click(object sender, EventArgs e)
        {
            this.tb_version.Text = this.listBox1.SelectedItem.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Tuple<string, DirectoryInfo[], FileInfo[]>> tuples = Utils.Walk(this.tb_repoPath.Text);
            Console.WriteLine(tuples.Count);
        }
    }
}
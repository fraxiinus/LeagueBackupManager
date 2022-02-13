using System.Windows.Forms;

namespace JVCS
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_store = new System.Windows.Forms.Button();
            this.tb_lolpath = new System.Windows.Forms.TextBox();
            this.tb_repoPath = new System.Windows.Forms.TextBox();
            this.tb_version = new System.Windows.Forms.TextBox();
            this.btn_export = new System.Windows.Forms.Button();
            this.tb_exportPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_console = new System.Windows.Forms.TextBox();
            this.tb_filter = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_loadPatches = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn_version_Select = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_store
            // 
            this.btn_store.Location = new System.Drawing.Point(160, 108);
            this.btn_store.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_store.Name = "btn_store";
            this.btn_store.Size = new System.Drawing.Size(109, 35);
            this.btn_store.TabIndex = 0;
            this.btn_store.Text = "scan and Import";
            this.btn_store.UseVisualStyleBackColor = true;
            this.btn_store.Click += new System.EventHandler(this.btn_store_Click);
            // 
            // tb_lolpath
            // 
            this.tb_lolpath.Location = new System.Drawing.Point(67, 44);
            this.tb_lolpath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_lolpath.Name = "tb_lolpath";
            this.tb_lolpath.Size = new System.Drawing.Size(211, 21);
            this.tb_lolpath.TabIndex = 1;
            this.tb_lolpath.Text = "D:\\test\\Game";
            // 
            // tb_repoPath
            // 
            this.tb_repoPath.Location = new System.Drawing.Point(67, 7);
            this.tb_repoPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_repoPath.Name = "tb_repoPath";
            this.tb_repoPath.Size = new System.Drawing.Size(211, 21);
            this.tb_repoPath.TabIndex = 2;
            this.tb_repoPath.Text = "D:\\vcs";
            // 
            // tb_version
            // 
            this.tb_version.Location = new System.Drawing.Point(412, 44);
            this.tb_version.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_version.Name = "tb_version";
            this.tb_version.Size = new System.Drawing.Size(232, 21);
            this.tb_version.TabIndex = 3;
            this.tb_version.Text = "11.24.412.2185";
            // 
            // btn_export
            // 
            this.btn_export.Location = new System.Drawing.Point(523, 296);
            this.btn_export.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(88, 51);
            this.btn_export.TabIndex = 4;
            this.btn_export.Text = "Export";
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_restore_Click);
            // 
            // tb_exportPath
            // 
            this.tb_exportPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_exportPath.Location = new System.Drawing.Point(412, 8);
            this.tb_exportPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_exportPath.Name = "tb_exportPath";
            this.tb_exportPath.Size = new System.Drawing.Size(232, 21);
            this.tb_exportPath.TabIndex = 5;
            this.tb_exportPath.Text = "D:/RESTORE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "RepoPath:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "LolPath:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(329, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Export Path:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(299, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Selected Patch";
            // 
            // tb_console
            // 
            this.tb_console.Location = new System.Drawing.Point(10, 164);
            this.tb_console.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_console.Multiline = true;
            this.tb_console.Name = "tb_console";
            this.tb_console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_console.Size = new System.Drawing.Size(311, 140);
            this.tb_console.TabIndex = 10;
            // 
            // tb_filter
            // 
            this.tb_filter.Location = new System.Drawing.Point(67, 74);
            this.tb_filter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_filter.Name = "tb_filter";
            this.tb_filter.Size = new System.Drawing.Size(211, 21);
            this.tb_filter.TabIndex = 11;
            this.tb_filter.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "Filter:";
            this.label5.Visible = false;
            // 
            // btn_loadPatches
            // 
            this.btn_loadPatches.Location = new System.Drawing.Point(391, 240);
            this.btn_loadPatches.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_loadPatches.Name = "btn_loadPatches";
            this.btn_loadPatches.Size = new System.Drawing.Size(137, 21);
            this.btn_loadPatches.TabIndex = 14;
            this.btn_loadPatches.Text = "Read Patch List";
            this.btn_loadPatches.UseVisualStyleBackColor = true;
            this.btn_loadPatches.Click += new System.EventHandler(this.btn_loadVersions_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(391, 74);
            this.listBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(220, 148);
            this.listBox1.TabIndex = 15;
            // 
            // btn_version_Select
            // 
            this.btn_version_Select.Location = new System.Drawing.Point(546, 236);
            this.btn_version_Select.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_version_Select.Name = "btn_version_Select";
            this.btn_version_Select.Size = new System.Drawing.Size(65, 28);
            this.btn_version_Select.TabIndex = 16;
            this.btn_version_Select.Text = "select";
            this.btn_version_Select.UseVisualStyleBackColor = true;
            this.btn_version_Select.Click += new System.EventHandler(this.btn_version_Select_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(244, 361);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 34);
            this.button1.TabIndex = 17;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 394);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_version_Select);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btn_loadPatches);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_filter);
            this.Controls.Add(this.tb_console);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_exportPath);
            this.Controls.Add(this.btn_export);
            this.Controls.Add(this.tb_version);
            this.Controls.Add(this.tb_repoPath);
            this.Controls.Add(this.tb_lolpath);
            this.Controls.Add(this.btn_store);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btn_store;
        private TextBox tb_lolpath;
        private TextBox tb_repoPath;
        private TextBox tb_version;
        private Button btn_export;
        private TextBox tb_exportPath;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox tb_console;
        private TextBox tb_filter;
        private Label label5;
        private Button btn_loadPatches;
        private ListBox listBox1;
        private Button btn_version_Select;
        private Button button1;
    }
}
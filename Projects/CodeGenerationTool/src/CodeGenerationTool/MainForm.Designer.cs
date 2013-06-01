namespace CodeGenerationTool
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.cbServerName = new System.Windows.Forms.ComboBox();
      this.cbDatabaseName = new System.Windows.Forms.ComboBox();
      this.btnGenerate = new System.Windows.Forms.Button();
      this.cbSpType = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtOutput = new System.Windows.Forms.TextBox();
      this.cbStoredProcedure = new System.Windows.Forms.ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnRefreshDropdowns = new System.Windows.Forms.Button();
      this.label6 = new System.Windows.Forms.Label();
      this.txtTabIndent = new System.Windows.Forms.NumericUpDown();
      this.btnGenerateCRUD = new System.Windows.Forms.Button();
      this.cbTables = new System.Windows.Forms.ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.panel1 = new System.Windows.Forms.Panel();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.tabPage3 = new System.Windows.Forms.TabPage();
      this.cbTablesClasses = new System.Windows.Forms.ComboBox();
      this.label7 = new System.Windows.Forms.Label();
      this.btnGenerateClassCode = new System.Windows.Forms.Button();
      this.panel2 = new System.Windows.Forms.Panel();
      this.fbdGenerateScripts = new System.Windows.Forms.FolderBrowserDialog();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.txtTabIndent)).BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(37, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(72, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Server Name:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(22, 47);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(87, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Database Name:";
      // 
      // cbServerName
      // 
      this.cbServerName.FormattingEnabled = true;
      this.cbServerName.Location = new System.Drawing.Point(115, 15);
      this.cbServerName.Name = "cbServerName";
      this.cbServerName.Size = new System.Drawing.Size(417, 21);
      this.cbServerName.TabIndex = 3;
      // 
      // cbDatabaseName
      // 
      this.cbDatabaseName.FormattingEnabled = true;
      this.cbDatabaseName.Location = new System.Drawing.Point(115, 44);
      this.cbDatabaseName.Name = "cbDatabaseName";
      this.cbDatabaseName.Size = new System.Drawing.Size(417, 21);
      this.cbDatabaseName.TabIndex = 4;
      // 
      // btnGenerate
      // 
      this.btnGenerate.Location = new System.Drawing.Point(434, 3);
      this.btnGenerate.Name = "btnGenerate";
      this.btnGenerate.Size = new System.Drawing.Size(91, 48);
      this.btnGenerate.TabIndex = 5;
      this.btnGenerate.Text = "Generate Code";
      this.btnGenerate.UseVisualStyleBackColor = true;
      this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
      // 
      // cbSpType
      // 
      this.cbSpType.FormattingEnabled = true;
      this.cbSpType.Location = new System.Drawing.Point(108, 30);
      this.cbSpType.Name = "cbSpType";
      this.cbSpType.Size = new System.Drawing.Size(320, 21);
      this.cbSpType.TabIndex = 7;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(68, 35);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(34, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Type:";
      // 
      // txtOutput
      // 
      this.txtOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.txtOutput.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.txtOutput.Location = new System.Drawing.Point(0, 87);
      this.txtOutput.Multiline = true;
      this.txtOutput.Name = "txtOutput";
      this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtOutput.Size = new System.Drawing.Size(1135, 594);
      this.txtOutput.TabIndex = 8;
      this.txtOutput.WordWrap = false;
      // 
      // cbStoredProcedure
      // 
      this.cbStoredProcedure.FormattingEnabled = true;
      this.cbStoredProcedure.Location = new System.Drawing.Point(108, 3);
      this.cbStoredProcedure.Name = "cbStoredProcedure";
      this.cbStoredProcedure.Size = new System.Drawing.Size(320, 21);
      this.cbStoredProcedure.TabIndex = 10;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(9, 6);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(93, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Stored Procedure:";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnRefreshDropdowns);
      this.groupBox1.Controls.Add(this.cbDatabaseName);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.cbServerName);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(1135, 72);
      this.groupBox1.TabIndex = 11;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Details";
      // 
      // btnRefreshDropdowns
      // 
      this.btnRefreshDropdowns.Location = new System.Drawing.Point(538, 12);
      this.btnRefreshDropdowns.Name = "btnRefreshDropdowns";
      this.btnRefreshDropdowns.Size = new System.Drawing.Size(128, 24);
      this.btnRefreshDropdowns.TabIndex = 17;
      this.btnRefreshDropdowns.Text = "Refresh Dropdowns";
      this.btnRefreshDropdowns.UseVisualStyleBackColor = true;
      this.btnRefreshDropdowns.Click += new System.EventHandler(this.btnRefreshDropdowns_Click);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(43, 33);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(62, 13);
      this.label6.TabIndex = 15;
      this.label6.Text = "Tab Indent:";
      // 
      // txtTabIndent
      // 
      this.txtTabIndent.Location = new System.Drawing.Point(111, 31);
      this.txtTabIndent.Name = "txtTabIndent";
      this.txtTabIndent.Size = new System.Drawing.Size(46, 20);
      this.txtTabIndent.TabIndex = 14;
      this.txtTabIndent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.txtTabIndent.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
      // 
      // btnGenerateCRUD
      // 
      this.btnGenerateCRUD.Location = new System.Drawing.Point(437, 6);
      this.btnGenerateCRUD.Name = "btnGenerateCRUD";
      this.btnGenerateCRUD.Size = new System.Drawing.Size(91, 48);
      this.btnGenerateCRUD.TabIndex = 13;
      this.btnGenerateCRUD.Text = "Generate CRUD";
      this.btnGenerateCRUD.UseVisualStyleBackColor = true;
      this.btnGenerateCRUD.Click += new System.EventHandler(this.btnGenerateCRUD_Click);
      // 
      // cbTables
      // 
      this.cbTables.FormattingEnabled = true;
      this.cbTables.Location = new System.Drawing.Point(111, 6);
      this.cbTables.Name = "cbTables";
      this.cbTables.Size = new System.Drawing.Size(320, 21);
      this.cbTables.TabIndex = 12;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(68, 9);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(37, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = "Table:";
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.tabPage2);
      this.tabControl1.Controls.Add(this.tabPage3);
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
      this.tabControl1.Location = new System.Drawing.Point(0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(1135, 87);
      this.tabControl1.TabIndex = 12;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.panel1);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(1127, 61);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Stored Procedures";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.btnGenerate);
      this.panel1.Controls.Add(this.label3);
      this.panel1.Controls.Add(this.cbSpType);
      this.panel1.Controls.Add(this.label4);
      this.panel1.Controls.Add(this.cbStoredProcedure);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(1121, 55);
      this.panel1.TabIndex = 13;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.label6);
      this.tabPage2.Controls.Add(this.cbTables);
      this.tabPage2.Controls.Add(this.txtTabIndent);
      this.tabPage2.Controls.Add(this.label5);
      this.tabPage2.Controls.Add(this.btnGenerateCRUD);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(1127, 61);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "CRUD";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // tabPage3
      // 
      this.tabPage3.Controls.Add(this.cbTablesClasses);
      this.tabPage3.Controls.Add(this.label7);
      this.tabPage3.Controls.Add(this.btnGenerateClassCode);
      this.tabPage3.Location = new System.Drawing.Point(4, 22);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Size = new System.Drawing.Size(1127, 61);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Classes";
      this.tabPage3.UseVisualStyleBackColor = true;
      // 
      // cbTablesClasses
      // 
      this.cbTablesClasses.FormattingEnabled = true;
      this.cbTablesClasses.Location = new System.Drawing.Point(111, 6);
      this.cbTablesClasses.Name = "cbTablesClasses";
      this.cbTablesClasses.Size = new System.Drawing.Size(320, 21);
      this.cbTablesClasses.TabIndex = 15;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(68, 9);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(37, 13);
      this.label7.TabIndex = 14;
      this.label7.Text = "Table:";
      // 
      // btnGenerateClassCode
      // 
      this.btnGenerateClassCode.Location = new System.Drawing.Point(437, 6);
      this.btnGenerateClassCode.Name = "btnGenerateClassCode";
      this.btnGenerateClassCode.Size = new System.Drawing.Size(91, 48);
      this.btnGenerateClassCode.TabIndex = 16;
      this.btnGenerateClassCode.Text = "Generate Class Code";
      this.btnGenerateClassCode.UseVisualStyleBackColor = true;
      this.btnGenerateClassCode.Click += new System.EventHandler(this.btnGenerateClassCode_Click);
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.txtOutput);
      this.panel2.Controls.Add(this.tabControl1);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(0, 72);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(1135, 681);
      this.panel2.TabIndex = 13;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1135, 753);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.groupBox1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MainForm";
      this.Text = "MainForm";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.txtTabIndent)).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      this.tabPage3.ResumeLayout(false);
      this.tabPage3.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cbServerName;
    private System.Windows.Forms.ComboBox cbDatabaseName;
    private System.Windows.Forms.Button btnGenerate;
    private System.Windows.Forms.ComboBox cbSpType;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtOutput;
    private System.Windows.Forms.ComboBox cbStoredProcedure;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btnGenerateCRUD;
    private System.Windows.Forms.ComboBox cbTables;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.NumericUpDown txtTabIndent;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.ComboBox cbTablesClasses;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Button btnGenerateClassCode;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button btnRefreshDropdowns;
    private System.Windows.Forms.FolderBrowserDialog fbdGenerateScripts;
  }
}
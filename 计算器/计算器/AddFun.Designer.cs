namespace 计算器
{
    partial class AddFun
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddFun));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFunName = new System.Windows.Forms.TextBox();
            this.txtFunVars = new System.Windows.Forms.TextBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.funExpr = new System.Windows.Forms.RichTextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtSearchKey = new System.Windows.Forms.TextBox();
            this.listBox_FunList = new System.Windows.Forms.ListBox();
            this.menu_ListBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制函数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.删除选中的函数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除所有函数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.btnExample = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.cmbFunSort = new System.Windows.Forms.ComboBox();
            this.cmbFunSearchSort = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.menu_ListBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "函数名称：";
            this.toolTip1.SetToolTip(this.label1, "请输入函数名称");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "函数描述：";
            this.toolTip1.SetToolTip(this.label2, "请输入函数描述，描述该函数的功能");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "函数表达式：";
            this.toolTip1.SetToolTip(this.label3, "请输入函数表达式，例：\r\nln(X+sqrt(X*X+1)+Y\r\n其中X和Y是函数的参数");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "参数列表：";
            this.toolTip1.SetToolTip(this.label4, "请输入函数的参数列表，例：\r\nX\"这里是X的注释\",Y\"这里是Y的注释\"\r\n参数之间请用英文逗号分隔");
            // 
            // txtFunName
            // 
            this.txtFunName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFunName.Location = new System.Drawing.Point(95, 19);
            this.txtFunName.Name = "txtFunName";
            this.txtFunName.Size = new System.Drawing.Size(163, 21);
            this.txtFunName.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtFunName, "请输入函数名称");
            this.txtFunName.Validated += new System.EventHandler(this.txtFunName_Validated);
            // 
            // txtFunVars
            // 
            this.txtFunVars.Location = new System.Drawing.Point(95, 130);
            this.txtFunVars.Name = "txtFunVars";
            this.txtFunVars.Size = new System.Drawing.Size(352, 21);
            this.txtFunVars.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txtFunVars, "请输入函数的参数列表\r\n请用逗号（英文逗号）分隔");
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(95, 56);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(352, 21);
            this.txtDesc.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtDesc, "请输入函数描述");
            // 
            // funExpr
            // 
            this.funExpr.Location = new System.Drawing.Point(95, 93);
            this.funExpr.Multiline = false;
            this.funExpr.Name = "funExpr";
            this.funExpr.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedHorizontal;
            this.funExpr.Size = new System.Drawing.Size(352, 21);
            this.funExpr.TabIndex = 6;
            this.funExpr.Text = "";
            this.toolTip1.SetToolTip(this.funExpr, "请输入函数表达式");
            this.funExpr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.funExpr_KeyDown);
            this.funExpr.TextChanged += new System.EventHandler(this.funExpr_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(291, 168);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "存储";
            this.toolTip1.SetToolTip(this.btnOK, "存储函数");
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(372, 168);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "关闭";
            this.toolTip1.SetToolTip(this.btnCancel, "关闭窗口");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtSearchKey
            // 
            this.txtSearchKey.Location = new System.Drawing.Point(95, 222);
            this.txtSearchKey.Name = "txtSearchKey";
            this.txtSearchKey.Size = new System.Drawing.Size(163, 21);
            this.txtSearchKey.TabIndex = 10;
            this.txtSearchKey.TextChanged += new System.EventHandler(this.txtSearchKey_TextChanged);
            // 
            // listBox_FunList
            // 
            this.listBox_FunList.ContextMenuStrip = this.menu_ListBox;
            this.listBox_FunList.FormattingEnabled = true;
            this.listBox_FunList.HorizontalScrollbar = true;
            this.listBox_FunList.ItemHeight = 12;
            this.listBox_FunList.Location = new System.Drawing.Point(14, 259);
            this.listBox_FunList.Name = "listBox_FunList";
            this.listBox_FunList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_FunList.Size = new System.Drawing.Size(433, 184);
            this.listBox_FunList.TabIndex = 12;
            this.listBox_FunList.SelectedIndexChanged += new System.EventHandler(this.listBox_FunList_SelectedIndexChanged);
            this.listBox_FunList.DoubleClick += new System.EventHandler(this.listBox_FunList_DoubleClick);
            // 
            // menu_ListBox
            // 
            this.menu_ListBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制函数ToolStripMenuItem,
            this.toolStripSeparator1,
            this.删除选中的函数ToolStripMenuItem,
            this.删除所有函数ToolStripMenuItem,
            this.toolStripSeparator2,
            this.导出ToolStripMenuItem,
            this.导入ToolStripMenuItem});
            this.menu_ListBox.Name = "menu_ListBox";
            this.menu_ListBox.Size = new System.Drawing.Size(155, 126);
            // 
            // 复制函数ToolStripMenuItem
            // 
            this.复制函数ToolStripMenuItem.Name = "复制函数ToolStripMenuItem";
            this.复制函数ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.复制函数ToolStripMenuItem.Text = "复制函数";
            this.复制函数ToolStripMenuItem.ToolTipText = "复制函数到剪切板";
            this.复制函数ToolStripMenuItem.Click += new System.EventHandler(this.复制函数ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // 删除选中的函数ToolStripMenuItem
            // 
            this.删除选中的函数ToolStripMenuItem.Name = "删除选中的函数ToolStripMenuItem";
            this.删除选中的函数ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.删除选中的函数ToolStripMenuItem.Text = "删除选中的函数";
            this.删除选中的函数ToolStripMenuItem.Click += new System.EventHandler(this.删除选中的函数ToolStripMenuItem_Click_1);
            // 
            // 删除所有函数ToolStripMenuItem
            // 
            this.删除所有函数ToolStripMenuItem.Name = "删除所有函数ToolStripMenuItem";
            this.删除所有函数ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.删除所有函数ToolStripMenuItem.Text = "删除所有函数";
            this.删除所有函数ToolStripMenuItem.ToolTipText = "删除当前分类下的所有函数";
            this.删除所有函数ToolStripMenuItem.Click += new System.EventHandler(this.删除所有函数ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(151, 6);
            // 
            // 导出ToolStripMenuItem
            // 
            this.导出ToolStripMenuItem.Name = "导出ToolStripMenuItem";
            this.导出ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.导出ToolStripMenuItem.Text = "导出";
            this.导出ToolStripMenuItem.ToolTipText = "导出所有自定义函数";
            this.导出ToolStripMenuItem.Click += new System.EventHandler(this.导出ToolStripMenuItem_Click);
            // 
            // 导入ToolStripMenuItem
            // 
            this.导入ToolStripMenuItem.Name = "导入ToolStripMenuItem";
            this.导入ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.导入ToolStripMenuItem.Text = "导入";
            this.导入ToolStripMenuItem.ToolTipText = "从文件中导入自定义函数";
            this.导入ToolStripMenuItem.Click += new System.EventHandler(this.导入ToolStripMenuItem_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "关键字：";
            this.toolTip1.SetToolTip(this.label5, "请输入搜索关键字\r\n用于匹配函数名称、函数描述或参数列表");
            // 
            // btnExample
            // 
            this.btnExample.Location = new System.Drawing.Point(48, 168);
            this.btnExample.Name = "btnExample";
            this.btnExample.Size = new System.Drawing.Size(75, 30);
            this.btnExample.TabIndex = 15;
            this.btnExample.Text = "例子";
            this.toolTip1.SetToolTip(this.btnExample, "例子\r\n注意参数要与函数表达式对应");
            this.btnExample.UseVisualStyleBackColor = true;
            this.btnExample.Click += new System.EventHandler(this.btnExample_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(129, 168);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 30);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "清空";
            this.toolTip1.SetToolTip(this.btnClear, "清空");
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRename
            // 
            this.btnRename.Location = new System.Drawing.Point(210, 168);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(75, 30);
            this.btnRename.TabIndex = 17;
            this.btnRename.Text = "重命名";
            this.toolTip1.SetToolTip(this.btnRename, "重命名选中的函数");
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // cmbFunSort
            // 
            this.cmbFunSort.FormattingEnabled = true;
            this.cmbFunSort.ItemHeight = 12;
            this.cmbFunSort.Location = new System.Drawing.Point(335, 19);
            this.cmbFunSort.Name = "cmbFunSort";
            this.cmbFunSort.Size = new System.Drawing.Size(112, 20);
            this.cmbFunSort.Sorted = true;
            this.cmbFunSort.TabIndex = 20;
            this.toolTip1.SetToolTip(this.cmbFunSort, "请输入函数类别或者选择已有的函数类别");
            // 
            // cmbFunSearchSort
            // 
            this.cmbFunSearchSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFunSearchSort.FormattingEnabled = true;
            this.cmbFunSearchSort.Location = new System.Drawing.Point(335, 222);
            this.cmbFunSearchSort.Name = "cmbFunSearchSort";
            this.cmbFunSearchSort.Size = new System.Drawing.Size(112, 20);
            this.cmbFunSearchSort.Sorted = true;
            this.cmbFunSearchSort.TabIndex = 21;
            this.toolTip1.SetToolTip(this.cmbFunSearchSort, "请选择函数类别");
            this.cmbFunSearchSort.SelectedIndexChanged += new System.EventHandler(this.cmbFunSearchSort_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(264, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "函数类别：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(264, 225);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "函数类别：";
            // 
            // AddFun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 460);
            this.Controls.Add(this.cmbFunSearchSort);
            this.Controls.Add(this.cmbFunSort);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listBox_FunList);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnExample);
            this.Controls.Add(this.txtSearchKey);
            this.Controls.Add(this.funExpr);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtFunVars);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtFunName);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AddFun";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "函数存储与管理";
            this.menu_ListBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFunName;
        private System.Windows.Forms.TextBox txtFunVars;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.RichTextBox funExpr;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtSearchKey;
        private System.Windows.Forms.ListBox listBox_FunList;
        private System.Windows.Forms.ContextMenuStrip menu_ListBox;
        private System.Windows.Forms.ToolStripMenuItem 删除选中的函数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除所有函数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnExample;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.ToolStripMenuItem 复制函数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbFunSort;
        private System.Windows.Forms.ComboBox cmbFunSearchSort;
        private System.Windows.Forms.Label label7;
    }
}
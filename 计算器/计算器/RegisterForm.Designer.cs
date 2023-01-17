namespace 计算器
{
    partial class RegisterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtMachineCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRegisterCode = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.lblRegisterInfo = new System.Windows.Forms.Label();
            this.btnCopyMachineCode = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lkNoRegisterCode = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "机器码：";
            this.toolTip1.SetToolTip(this.label1, "您电脑的机器码");
            // 
            // txtMachineCode
            // 
            this.txtMachineCode.BackColor = System.Drawing.SystemColors.Window;
            this.txtMachineCode.Location = new System.Drawing.Point(83, 51);
            this.txtMachineCode.Name = "txtMachineCode";
            this.txtMachineCode.ReadOnly = true;
            this.txtMachineCode.Size = new System.Drawing.Size(270, 21);
            this.txtMachineCode.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "注册码：";
            this.toolTip1.SetToolTip(this.label2, "请输入注册码");
            // 
            // txtRegisterCode
            // 
            this.txtRegisterCode.Location = new System.Drawing.Point(83, 86);
            this.txtRegisterCode.Name = "txtRegisterCode";
            this.txtRegisterCode.Size = new System.Drawing.Size(270, 21);
            this.txtRegisterCode.TabIndex = 3;
            // 
            // btnRegister
            // 
            this.btnRegister.Location = new System.Drawing.Point(83, 127);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(90, 30);
            this.btnRegister.TabIndex = 4;
            this.btnRegister.Text = "注 册";
            this.toolTip1.SetToolTip(this.btnRegister, "输入注册码，注册该软件");
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // lblRegisterInfo
            // 
            this.lblRegisterInfo.AutoSize = true;
            this.lblRegisterInfo.Location = new System.Drawing.Point(12, 20);
            this.lblRegisterInfo.Name = "lblRegisterInfo";
            this.lblRegisterInfo.Size = new System.Drawing.Size(65, 12);
            this.lblRegisterInfo.TabIndex = 5;
            this.lblRegisterInfo.Text = "注册信息：";
            // 
            // btnCopyMachineCode
            // 
            this.btnCopyMachineCode.Location = new System.Drawing.Point(197, 127);
            this.btnCopyMachineCode.Name = "btnCopyMachineCode";
            this.btnCopyMachineCode.Size = new System.Drawing.Size(90, 30);
            this.btnCopyMachineCode.TabIndex = 6;
            this.btnCopyMachineCode.Text = "复制机器码";
            this.toolTip1.SetToolTip(this.btnCopyMachineCode, "复制机器码到剪切板");
            this.btnCopyMachineCode.UseVisualStyleBackColor = true;
            this.btnCopyMachineCode.Click += new System.EventHandler(this.btnCopyMachineCode_Click);
            // 
            // lkNoRegisterCode
            // 
            this.lkNoRegisterCode.AutoSize = true;
            this.lkNoRegisterCode.Location = new System.Drawing.Point(276, 20);
            this.lkNoRegisterCode.Name = "lkNoRegisterCode";
            this.lkNoRegisterCode.Size = new System.Drawing.Size(77, 12);
            this.lkNoRegisterCode.TabIndex = 7;
            this.lkNoRegisterCode.TabStop = true;
            this.lkNoRegisterCode.Text = "没有注册码？";
            this.toolTip1.SetToolTip(this.lkNoRegisterCode, "没有注册码？");
            this.lkNoRegisterCode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lkNoRegisterCode_LinkClicked);
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 169);
            this.Controls.Add(this.lkNoRegisterCode);
            this.Controls.Add(this.btnCopyMachineCode);
            this.Controls.Add(this.lblRegisterInfo);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.txtRegisterCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMachineCode);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计算器注册";
            this.Shown += new System.EventHandler(this.RegisterForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMachineCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRegisterCode;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label lblRegisterInfo;
        private System.Windows.Forms.Button btnCopyMachineCode;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel lkNoRegisterCode;
    }
}
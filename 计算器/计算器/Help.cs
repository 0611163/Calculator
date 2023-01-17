using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 计算器
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();

            this.treeView1.Nodes[0].Expand();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    //重置颜色和字体
                    richTextBox1.SelectAll();
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, richTextBox1.Font.Size);

                    string tag = e.Node.Tag == null ? "" : e.Node.Tag.ToString();
                    int index = richTextBox1.Text.IndexOf(tag);

                    richTextBox1.SelectionStart = index;
                    richTextBox1.SelectionLength = tag.Length;
                    richTextBox1.SelectionColor = Color.FromArgb(0, 128, 0);
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, richTextBox1.Font.Size, FontStyle.Bold);
                    richTextBox1.ScrollToCaret();
                }
                catch
                {
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                //重置颜色和字体
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, richTextBox1.Font.Size);

                string tag = e.Node.Tag == null ? "" : e.Node.Tag.ToString();
                int index = richTextBox1.Text.IndexOf(tag);

                richTextBox1.SelectionStart = index;
                richTextBox1.SelectionLength = tag.Length;
                richTextBox1.SelectionColor = Color.FromArgb(0, 128, 0);
                richTextBox1.SelectionFont = new Font(richTextBox1.Font.FontFamily, richTextBox1.Font.Size, FontStyle.Bold);
                richTextBox1.ScrollToCaret();
            }
            catch
            {
            }
        }

    }
}

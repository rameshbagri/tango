﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using Microsoft.Office.Tools;
using System.Windows.Forms;
using word = Microsoft.Office.Interop.Word;

namespace Tango
{
    public partial class TangoUserControl : UserControl
    {
        private bool UpDnKeyPress = false;
        public TangoUserControl()
        {
            InitializeComponent();
        }

        private void btnProcess_Click_1(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Home");
            Control CLB = GetCtrl("tabControl1");

            TabPage tp = new TabPage();
            tp.Name = "Summary";
            tp.Text = "Summary";
            ((TabControl)CLB).Controls.Add(tp);
            comboBox1.Items.Add("Summary");

            TextBox txtBox = new TextBox();
            txtBox.Font = new Font("Arial", 9, FontStyle.Bold);
            
            txtBox.AppendText("String Count Summary");
            txtBox.AppendText(Environment.NewLine);

            Control TabSummary = GetCtrl("Summary");

            ((TabPage)TabSummary).Controls.Add(txtBox);

            txtBox.Width = ((TabPage)TabSummary).Width;
            txtBox.Height = ((TabPage)TabSummary).Height;
            txtBox.Multiline = true;
            txtBox.ScrollBars = ScrollBars.Both;
            txtBox.ReadOnly = true;

            object[] SrchItem = { "Sri Lanka", "Wicket", "only", "merely", "actually", "fully", "generally", "completely", "rarely", "continuously", "immediately" };

            //MessageBox.Show(txtBox.Text);

            if (checkBox1.Checked)
            {
                tp = new TabPage();
                tp.Name = "Page1";
                tp.Text = checkBox1.Text;
                ((TabControl)CLB).Controls.Add(tp);
                txtBox.AppendText(AddSummary1A("Page1", "Page1_1", SrchItem));
                txtBox.AppendText(Environment.NewLine);
                //AddResult1("Page1", "Page1_1", SrchItem);
                comboBox1.Items.Add(checkBox1.Text);
            }
            if (checkBox2.Checked)
            {
                tp = new TabPage();
                tp.Name = "Page2";
                tp.Text = checkBox2.Text;
                ((TabControl)CLB).Controls.Add(tp);
                AddResult("Page2", "Page2_1", "Wicket");
                txtBox.AppendText(Environment.NewLine);
                comboBox1.Items.Add(checkBox2.Text);
            }

            if (checkBox3.Checked)
            {
                tp = new TabPage();
                tp.Name = "Page3";
                tp.Text = "Function 3";
                ((TabControl)CLB).Controls.Add(tp);
            }

            if (checkBox4.Checked)
            {
                tp = new TabPage();
                tp.Name = "Page4";
                tp.Text = "Function 4";
                ((TabControl)CLB).Controls.Add(tp);
            }

            if (checkBox5.Checked)
            {
                tp = new TabPage();
                tp.Name = "Page5";
                tp.Text = "Function 5";
                ((TabControl)CLB).Controls.Add(tp);
            }

            if (checkBox6.Checked)
            {
                tp = new TabPage();
                tp.Name = "Page6";
                tp.Text = "Function 6";
                ((TabControl)CLB).Controls.Add(tp);
            }
            TabPage t = new TabPage();
            tabControl1.SelectedIndex = 1;
        }

        private string AddSummary1A(string basePage, string addPage, object[] srchItem)
        {
            DateTime dt1 = DateTime.Now;
            Globals.ThisAddIn.Application.ScreenUpdating = false;
            string RetVal = "";
            Control Ctr = GetCtrl(basePage);
            Control P = AddPanel(addPage, 0, 0, Ctr.Width - 2, Ctr.Height - 2);
            Ctr.Controls.Add(P);
            
            CheckedListBox CLB = new CheckedListBox();
            CLB.Name = "CheckedListBox_" + basePage;
            CLB.Top = 0;
            CLB.Left = 0;
            CLB.Height = (int)((double)(P.Height) * 0.4);
            CLB.Width = (int)((double)(P.Width) * 0.98);

            TabControl TbCtrl = AddTabCtrl("TabCtrl" + basePage, (int)((double)(P.Height) * 0.42), 0, (int)((double)(P.Width) * 0.98), (int)((double)(P.Height) - CLB.Height));
    
            Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;
            Microsoft.Office.Interop.Word.Range rng = docs.Content;
            rng.Find.ClearFormatting();

            int scount = docs.Sentences.Count;
            object[] findtext = srchItem;
            int rng1count = 0;
            TabPage tp = new TabPage();
            for (int i = 0; i < findtext.Length; i++)
            {
                tp = new TabPage();
                tp.Name = basePage + i.ToString();
                tp.Text = findtext[i].ToString();
                ((TabControl)TbCtrl).Controls.Add(tp);
                
                CheckedListBox CLB1 = new CheckedListBox();
                CLB1.Name = "TabCtrl" + basePage + "CLB_" + i.ToString();
                CLB1.Width = CLB.Width;
                tp.Controls.Add(CLB1);
                
                rng.Start = 0;
                rng1count = 0;
                rng.Find.Execute(ref findtext[i]);
                while (rng.Find.Found)
                {
                    rng1count += 1;
                    rng.Find.Execute(ref findtext[i]);
                    rng.Select();
                    string setence = (Globals.ThisAddIn.Application.Selection.Range.Sentences[1].Text.Trim());
                    if (setence.Length > findtext[i].ToString().Length)
                    {
                        CLB1.Items.Add(Globals.ThisAddIn.Application.Selection.Range.Sentences[1].Text.Trim());
                    }
                }
                if (rng1count == 0)
                {
                    TbCtrl.Controls.Remove(tp);
                }
                else
                {
                    RetVal += findtext[i] + Environment.NewLine;
                    RetVal += "Word Count : " + rng1count.ToString() + Environment.NewLine + Environment.NewLine;
                    CLB.Items.Add(findtext[i] + "( " + rng1count.ToString() + " )");
                    CLB1.Click += CheckedListBox_Click;
                    CLB1.HorizontalScrollbar = true;
                }
                rng = docs.Content;
                rng.Start = 0;
                rng.End = 0;
                rng.Select();
            }
            CLB.Click += CheckedListBox_Click;
            
            P.Controls.Add(CLB);
            P.Controls.Add(TbCtrl);
            P.Visible = true;
            
            Globals.ThisAddIn.Application.ScreenUpdating = true;
            DateTime dt2 = DateTime.Now;

            TimeSpan dt = dt2 - dt1;

            MessageBox.Show(dt.Milliseconds.ToString());

            return RetVal;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            string TName = "TabCtrlPage" + (tabIndex - 1).ToString();
            TabControl TC = GetCtrl(TName) as TabControl;
            int Tindex = TC.SelectedIndex;
            string ClbNm = "CheckedListBox_Page" + (tabIndex - 1).ToString();
            CheckedListBox CLB = GetCtrl(ClbNm) as CheckedListBox;

            string CHkLBNm = TName + "CLB_" + Tindex;
            CheckedListBox TabCL = GetCtrl(CHkLBNm) as CheckedListBox;
            MessageBox.Show(CHkLBNm);


            bool SAll = (TabCL.Items.Count == TabCL.CheckedItems.Count);

            Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;
            Microsoft.Office.Interop.Word.Range rng = docs.Content;
            rng.Find.ClearFormatting();

            object fText = "Sri Lanka";
            object rText = "Canada";

            if (SAll)
            {
                rng.Find.Text = fText.ToString();
                rng.Find.Replacement.Text = rText.ToString();
                rng.Find.Forward = true;
                rng.Find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
                rng.Find.Format = false;
                rng.Find.MatchCase = false;
                rng.Find.MatchWholeWord = false;
                rng.Find.MatchWildcards = false;
                rng.Find.MatchSoundsLike = false;
                rng.Find.MatchAllWordForms = false;
                
                rng.Find.Execute(Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll);

                TC.Controls.RemoveAt(Tindex);
                CLB.Items.RemoveAt(Tindex);
            }
            else
            {
                int i = 0;
                List<int> remotem = new List<int>();

                foreach (object itemChecked in TabCL.CheckedItems)
                {
                    Microsoft.Office.Interop.Word.Range rng1 = docs.Content;
                    rng1.Find.Text = itemChecked.ToString();
                    rng1.Find.Execute();
                    rng1.Select();
                    rng = (Globals.ThisAddIn.Application.Selection.Range.Sentences[1]);
                    rng.Find.Text = fText.ToString();
                    rng.Find.Replacement.Text = rText.ToString();
                    rng.Find.Forward = true;
                    rng.Find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
                    rng.Find.Format = false;
                    rng.Find.MatchCase = false;
                    rng.Find.MatchWholeWord = false;
                    rng.Find.MatchWildcards = false;
                    rng.Find.MatchSoundsLike = false;
                    rng.Find.MatchAllWordForms = false;
            
                    rng.Find.Execute(Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    TabCL.FindString(itemChecked.ToString());
                    //int indextodel = TabCL.SelectedIndex;
                    int indextodel = TabCL.FindString(itemChecked.ToString());
                    remotem.Add(indextodel);
                    int countnum1 = CLB.Items[Tindex].ToString().IndexOf("(", 0);
                    int countnum2 = CLB.Items[Tindex].ToString().IndexOf(")", 0);

                    string nm = CLB.Items[Tindex].ToString().Substring(0, countnum1);
                    string NumPart = CLB.Items[Tindex].ToString().Substring(countnum1 + 1, countnum2-countnum1 - 1).Trim();

                    int countnum = int.Parse(NumPart) - 1;

                    CLB.Items[Tindex] = nm + "( " + countnum.ToString() + " )";
                    rng = docs.Content;
                    rng.Start = 0;
                    rng.End=0;
                    rng.Select();
                }
                i++;
                remotem.Sort();
                remotem.Reverse();
                foreach (int x in remotem.ToList())
                {
                    MessageBox.Show(x.ToString());
                    TabCL.Items.RemoveAt(x);
                }
            }
        }

        private void CheckedListBox_Click(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;

            string sendName = ((CheckedListBox)sender).Name;
            
            Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;
            CheckedListBox C1 = GetCtrl(sendName) as CheckedListBox;

            CheckedListBox CLB = (CheckedListBox)C1;
            Microsoft.Office.Interop.Word.Range rng1 = docs.Content;

            string findText = CLB.SelectedItem.ToString();
            string fText = findText.Trim();
            CLB.SetItemChecked(CLB.SelectedIndex, true);
            int scnt = findText.IndexOf("(", 0);
            if(scnt>0)
            {
                fText = (findText.Substring(0, scnt)).Trim();
            }

            rng1.Start = 0;
            rng1.Find.Forward = true;
            rng1.Find.ClearHitHighlight();
            rng1.Find.HitHighlight(FindText: fText, MatchCase: false, HighlightColor: Microsoft.Office.Interop.Word.WdColor.wdColorBlue, TextColor: Microsoft.Office.Interop.Word.WdColor.wdColorWhite);
            rng1.Find.Execute();
            rng1.Select();
            if(MastListBox(sendName))
            {
                int TIndex = CLB.SelectedIndex;
                string Cname = "TabCtrlPage" + (tabIndex - 1).ToString();
                TabControl TC = GetCtrl(Cname) as TabControl;
                TC.SelectTab(TIndex);
            }
        }

        private bool MastListBox(string sendName)
        {
            bool mList = false;
            if (sendName == "CheckedListBox_Page1") { mList = true; }
            if (sendName == "CheckedListBox_Page2") { mList = true; }
            if (sendName == "CheckedListBox_Page3") { mList = true; }
            if (sendName == "CheckedListBox_Page4") { mList = true; }
            if (sendName == "CheckedListBox_Page5") { mList = true; }
            if (sendName == "CheckedListBox_Page6") { mList = true; }
            return mList;
        }

        private void CheckedListBox1_Click(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;

            Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;
            CheckedListBox C = GetCLBName(tabIndex);
            CheckedListBox CLB = (CheckedListBox)C;
            Microsoft.Office.Interop.Word.Range rng1 = docs.Content;

            string findText = CLB.SelectedItem.ToString();

            int scnt = findText.IndexOf("(", 0);
            string fText = (findText.Substring(0, scnt)).Trim();

            rng1.Start = 0;
            rng1.Find.Forward = true;
            rng1.Find.ClearHitHighlight();
            rng1.Find.HitHighlight(FindText: fText, MatchCase: false, HighlightColor: Microsoft.Office.Interop.Word.WdColor.wdColorBlue, TextColor: Microsoft.Office.Interop.Word.WdColor.wdColorWhite);
            rng1.Find.Execute();
        }

        private void FindSentence(int tabIndex, string fText, word.Document docs)
        {
            DateTime dt = DateTime.Now;
            int cnt = 0;
            for(int sentcount = 1; sentcount <= docs.Sentences.Count; sentcount++)
            {
                if (docs.Sentences[sentcount].Text.Contains(fText))
                {
                    cnt++;
                }
            }
            MessageBox.Show(cnt.ToString());
            DateTime dt1 = DateTime.Now;
            MessageBox.Show((dt1 - dt).ToString());
        }
        
        private CheckedListBox GetCLBName1(int tabIndex)
        {
            CheckedListBox C = null;
            {
                if (tabIndex == 2) { C = GetCtrl("CheckedListBox_Page1_2") as CheckedListBox; }
                if (tabIndex == 3) { C = GetCtrl("CheckedListBox_Page2_2") as CheckedListBox; }
                if (tabIndex == 4) { C = GetCtrl("CheckedListBox_Page3_2") as CheckedListBox; }
                if (tabIndex == 5) { C = GetCtrl("CheckedListBox_Page4_2") as CheckedListBox; }
                if (tabIndex == 6) { C = GetCtrl("CheckedListBox_Page5_2") as CheckedListBox; }
                if (tabIndex == 7) { C = GetCtrl("CheckedListBox_Page6_2") as CheckedListBox; }
            }
            return C;
        }

        private CheckedListBox GetCLBName(int tabIndex)
        {
            CheckedListBox C = null;
            {
                if (tabIndex == 2) { C = GetCtrl("CheckedListBox_Page1") as CheckedListBox; }
                if (tabIndex == 3) { C = GetCtrl("CheckedListBox_Page2") as CheckedListBox; }
                if (tabIndex == 4) { C = GetCtrl("CheckedListBox_Page3") as CheckedListBox; }
                if (tabIndex == 5) { C = GetCtrl("CheckedListBox_Page4") as CheckedListBox; }
                if (tabIndex == 6) { C = GetCtrl("CheckedListBox_Page5") as CheckedListBox; }
                if (tabIndex == 7) { C = GetCtrl("CheckedListBox_Page6") as CheckedListBox; }
            }
            return C;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            //MessageBox.Show(tabIndex.ToString());
            if(tabIndex == 0)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox4.Checked = true;
                checkBox5.Checked = true;
                checkBox6.Checked = true;
            }
            else if(tabIndex > 1)
            {
                string TName = "TabCtrlPage" + (tabIndex - 1).ToString();
                TabControl TC = GetCtrl(TName) as TabControl;
                int Tindex = TC.SelectedIndex;
                string CHkLBNm = TName + "CLB_" + Tindex;
                CheckedListBox TabCL = GetCtrl(CHkLBNm) as CheckedListBox;

                for (int i = 0; i < ((CheckedListBox)TabCL).Items.Count; i++)
                {
                    ((CheckedListBox)TabCL).SetItemChecked(i, true);
                }
            }
        }

        private void btnReser_Click(object sender, EventArgs e)
        {
            TabControl TbCtrl = (TabControl)GetCtrl("tabControl1");
            foreach(TabPage Page in TbCtrl.TabPages)
            {
                if(Page.Name != "HomePage")
                {
                    TbCtrl.Controls.Remove(Page);
                    Page.Dispose();
                }
                comboBox1.Items.Clear();
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;

            }
        }

        private Panel AddPanel(string name, int top, int left, int wid, int H)
        {
            Panel pnl = new Panel();
            pnl.Name = name;
            pnl.Top = top;
            pnl.Left = left;
            pnl.Width = wid;
            pnl.Height = H;
            return pnl;
        }

        private TabControl AddTabCtrl(string name, int top, int left, int wid, int H)
        {
            TabControl tCtrl = new TabControl();
            tCtrl.Name = name;
            tCtrl.Top = top;
            tCtrl.Left = left;
            tCtrl.Width = wid;
            tCtrl.Height = H;
            return tCtrl;
        }

        private Control AddLabel(int Top, int Left, int founCount)
        {
            Label lb = new Label();
            lb.Text = "Count : " + founCount.ToString();
            lb.Top = Top;
            lb.Left = Left;
            return lb;
        }

        private Control AddTextBox(int Top, int Left, string name, string val)
        {
            TextBox T = new TextBox();
            T.Name = name;
            T.Top = Top;
            T.Left = Left;
            if (!val.Equals("")) { T.Text = val; }
            return T;
        }

        private Control AddBtn(int top, int left, string name, string caption, EventHandler clickevent)
        {
            Button B = new Button();
            B.Name = name;
            B.Top = top;
            B.Left = left;
            B.Text = caption;
            B.Click += clickevent;
            return B;
        }

        private Control AddListBox(int top, int left, string name)
        {
            ListBox L = new ListBox();
            L.Name = name;
            L.Top = top;
            L.Left = left;
            
            return L;
        }

        public string GetTextVal(string cName)
        {
            string findtext = "";
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    var tnm = c.Name;
                    if (tnm == cName)
                    {
                        findtext = c.Text;
                        //MessageBox.Show(findtext);
                    }
                }
                else
                {
                    foreach (Control c1 in c.Controls)
                    {
                        if (c1 is TextBox)
                        {
                            var tnm = c1.Name;
                            if (tnm == cName)
                            {
                                findtext = c1.Text;
                                //MessageBox.Show(findtext);
                            }
                        }
                        else
                        {
                            foreach (Control c2 in c1.Controls)
                            {
                                if (c2 is TextBox)
                                {
                                    var tnm = c2.Name;
                                    if (tnm == cName)
                                    {
                                        findtext = c2.Text;
                                        //MessageBox.Show(findtext);
                                    }
                                }
                                else
                                {
                                    foreach (Control c3 in c2.Controls)
                                    {
                                        if (c3 is TextBox)
                                        {
                                            var tnm = c3.Name;
                                            if (tnm == cName)
                                            {
                                                findtext = c3.Text;
                                                //MessageBox.Show(findtext);
                                            }
                                        }
                                        else
                                        {
                                            foreach (Control c4 in c3.Controls)
                                            {
                                                if (c4 is TextBox)
                                                {
                                                    var tnm = c4.Name;
                                                    if (tnm == cName)
                                                    {
                                                        findtext = c4.Text;
                                                        //MessageBox.Show(findtext);
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (Control c5 in c4.Controls)
                                                    {
                                                        if (c5 is TextBox)
                                                        {
                                                            var tnm = c5.Name;
                                                            if (tnm == cName)
                                                            {
                                                                findtext = c5.Text;
                                                                //MessageBox.Show(findtext);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            foreach (Control c6 in c5.Controls)
                                                            {
                                                                if (c6 is TextBox)
                                                                {
                                                                    var tnm = c6.Name;
                                                                    if (tnm == cName)
                                                                    {
                                                                        findtext = c6.Text;
                                                                        //MessageBox.Show(findtext);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    foreach (Control c7 in c6.Controls)
                                                                    {
                                                                        if (c7 is TextBox)
                                                                        {
                                                                            var tnm = c7.Name;
                                                                            if (tnm == cName)
                                                                            {
                                                                                findtext = c7.Text;
                                                                                //MessageBox.Show(findtext);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            foreach (Control c8 in c7.Controls)
                                                                            {
                                                                                if (c8 is TextBox)
                                                                                {
                                                                                    var tnm = c8.Name;
                                                                                    if (tnm == cName)
                                                                                    {
                                                                                        findtext = c8.Text;
                                                                                        //MessageBox.Show(findtext);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    foreach (Control c9 in c8.Controls)
                                                                                    {
                                                                                        if (c9 is TextBox)
                                                                                        {
                                                                                            var tnm = c9.Name;
                                                                                            if (tnm == cName)
                                                                                            {
                                                                                                findtext = c9.Text;
                                                                                                //MessageBox.Show(findtext);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            foreach (Control c0 in c9.Controls)
                                                                                            {
                                                                                                if (c0 is TextBox)
                                                                                                {
                                                                                                    var tnm = c0.Name;
                                                                                                    if (tnm == cName)
                                                                                                    {
                                                                                                        findtext = c0.Text;
                                                                                                        //MessageBox.Show(findtext);
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return findtext;
        }

        public Control GetCtrl(string cName)
        {
            Control findtext = new Control();
            foreach (Control c in this.Controls)
            {
                var tnm = c.Name;
                if (tnm == cName)
                {
                    findtext = c;
                }
                else
                {
                    foreach (Control c1 in c.Controls)
                    {
                        tnm = c1.Name;
                        if (tnm == cName)
                        {
                            findtext = c1;
                        }
                        else
                        {
                            foreach (Control c2 in c1.Controls)
                            {
                                tnm = c2.Name;
                                if (tnm == cName)
                                {
                                    findtext = c2;
                                }
                                else
                                {
                                    foreach (Control c3 in c2.Controls)
                                    {
                                        tnm = c3.Name;
                                        if (tnm == cName)
                                        {
                                            findtext = c3;
                                        }
                                        else
                                        {
                                            foreach (Control c4 in c3.Controls)
                                            {
                                                tnm = c4.Name;
                                                if (tnm == cName)
                                                {
                                                    findtext = c4;
                                                }
                                                else
                                                {
                                                    foreach (Control c5 in c4.Controls)
                                                    {
                                                        tnm = c5.Name;
                                                        if (tnm == cName)
                                                        {
                                                            findtext = c5;
                                                        }
                                                        else
                                                        {
                                                            foreach (Control c6 in c5.Controls)
                                                            {
                                                                tnm = c6.Name;
                                                                if (tnm == cName)
                                                                {
                                                                    findtext = c6;
                                                                }
                                                                else
                                                                {
                                                                    foreach (Control c7 in c6.Controls)
                                                                    {
                                                                        tnm = c7.Name;
                                                                        if (tnm == cName)
                                                                        {
                                                                            findtext = c7;
                                                                        }
                                                                        else
                                                                        {
                                                                            foreach (Control c8 in c7.Controls)
                                                                            {
                                                                                tnm = c8.Name;
                                                                                if (tnm == cName)
                                                                                {
                                                                                    findtext = c8;
                                                                                }
                                                                                else
                                                                                {
                                                                                    foreach (Control c9 in c8.Controls)
                                                                                    {
                                                                                        tnm = c9.Name;
                                                                                        if (tnm == cName)
                                                                                        {
                                                                                            findtext = c9;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            foreach (Control c0 in c9.Controls)
                                                                                            {
                                                                                                tnm = c0.Name;
                                                                                                if (tnm == cName)
                                                                                                {
                                                                                                    findtext = c0;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return findtext;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = tabControl1.SelectedIndex;
            if (index < 2) { btnExecute.Enabled = false; } else { btnExecute.Enabled = true; }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            tabControl1.SelectedIndex = index;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            //MessageBox.Show(tabIndex.ToString());
            if (tabIndex == 0)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
            }
            else if (tabIndex > 1)
            {
                string TName = "TabCtrlPage" + (tabIndex - 1).ToString();
                TabControl TC = GetCtrl(TName) as TabControl;
                int Tindex = TC.SelectedIndex;
                string CHkLBNm = TName + "CLB_" + Tindex;
                CheckedListBox TabCL = GetCtrl(CHkLBNm) as CheckedListBox;

                for (int i = 0; i < ((CheckedListBox)TabCL).Items.Count; i++)
                {
                    ((CheckedListBox)TabCL).SetItemChecked(i, false);
                }
            }
        }

        private string AddSummary1(object[] srchItem)
        {
            string RetVal = "";

            Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;
            Microsoft.Office.Interop.Word.Range rng = docs.Content;
            rng.Find.ClearFormatting();

            int scount = docs.Sentences.Count;
            object[] findtext = srchItem;
            int rng1count = 0;

            for (int i = 0; i < findtext.Length; i++)
            {
                rng.Start = 0;
                rng1count = 0;
                rng.Find.Execute(ref findtext[i]);
                while (rng.Find.Found)
                {
                    rng1count += 1;
                    rng.Find.Execute(ref findtext[i]);
                }
                RetVal += findtext[i] + Environment.NewLine;
                RetVal += "Word Count : " + rng1count.ToString() + Environment.NewLine + Environment.NewLine;
            }
            return RetVal;
        }

        private void AddResult1(string basePage, string addPage, object[] srchItem)
        {
            Control Ctr = GetCtrl(basePage);
            Control P = AddPanel(addPage, 0, 0, Ctr.Width - 2, Ctr.Height - 2);
            Ctr.Controls.Add(P);

            CheckedListBox CLB = new CheckedListBox();
            CLB.Name = "CheckedListBox_" + basePage;
            CLB.Top = 0;
            CLB.Left = 0;
            CLB.Height = (int)((double)(P.Height) * 0.4);
            CLB.Width = (int)((double)(P.Width) * 0.98);

            Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;
            Microsoft.Office.Interop.Word.Range rng = docs.Content;
            rng.Find.ClearFormatting();

            object[] findtext = srchItem;
            int rng1count = 0;

            for (int i = 0; i < findtext.Length; i++)
            {
                rng.Start = 0;
                rng1count = 0;
                rng.Find.Execute(ref findtext[i]);
                while (rng.Find.Found)
                {
                    rng1count += 1;
                    rng.Find.Execute(ref findtext[i]);
                }
                CLB.Items.Add(findtext[i] + "( " + rng1count.ToString() + " )");
            }
            CLB.Click += CheckedListBox_Click;

            P.Controls.Add(CLB);
            P.Visible = true;
        }

        private void AddResult(string basePage, string addPage, string SearText)
        {
            Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;
            Microsoft.Office.Interop.Word.Range rng = docs.Content;
            rng.Find.ClearFormatting();

            Control Ctr = GetCtrl(basePage);
            Control P = AddPanel(addPage, 0, 0, Ctr.Width - 2, Ctr.Height - 2);
            Ctr.Controls.Add(P);

            CheckedListBox CLB = new CheckedListBox();
            CLB.Name = "CheckedListBox_" + basePage;
            CLB.Top = 0;
            CLB.Left = 0;
            CLB.Height = (int)((double)(P.Height) * 0.25);
            CLB.Width = (int)((double)(P.Width) * 0.98);

            CheckedListBox CL2 = new CheckedListBox();
            CL2.Name = "CheckedListBox_" + basePage + "_2";
            CL2.Visible = false;

            int scount = docs.Sentences.Count;
            object findtext = SearText;
            object findtext1 = "Sri Lanka";
            object findtext2 = "Wicket";
            int rng1count = 0;
            int rng2count = 0;


            for (int i = 1; i <= scount; i++)
            {
                Microsoft.Office.Interop.Word.Range rng1 = docs.Sentences[i];
                Microsoft.Office.Interop.Word.Range rng2 = docs.Sentences[i];
                rng1.Find.ClearFormatting();
                rng1.Find.Forward = true;
                rng1.Find.Execute(ref findtext1);
                rng2.Find.ClearFormatting();
                rng2.Find.Forward = true;
                rng2.Find.Execute(ref findtext2);


                if (rng1.Find.Found)
                {
                    rng1count++;
                }
                if (rng2.Find.Found)
                {
                    rng2count++;
                }
            }
            CLB.Items.Add("Sri Lanka ( " + rng1count.ToString() + " )");
            CLB.Items.Add("Wicket ( " + rng2count.ToString() + " )");

            CLB.ScrollAlwaysVisible = true;
            CLB.HorizontalScrollbar = true;
            CLB.Click += CheckedListBox_Click;

            P.Controls.Add(CLB);
            P.Visible = true;
        }

        private int CountString(string v, Microsoft.Office.Interop.Word.Document docs)
        {
            int i = 0;
            string MyDoc = docs.Content.ToString();
            word.Range rng = docs.Content;
            rng.Find.Text = v;
            rng.Find.Replacement.Text = "[]";
            rng.Find.Forward = true;
            rng.Find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
            rng.Find.Format = false;
            rng.Find.MatchCase = false;
            rng.Find.MatchWholeWord = false;
            rng.Find.MatchWildcards = false;
            rng.Find.MatchSoundsLike = false;
            rng.Find.MatchAllWordForms = false;

            rng.Find.Execute(Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll);
            string MyDocT = rng.Text;

            MessageBox.Show(docs.Content.Text);
            MessageBox.Show(MyDocT);

            MessageBox.Show("MyDoc Length is " + docs.Content.Text.Length.ToString());
            MessageBox.Show("MyDocT Length is " + MyDocT.Length.ToString());
            MessageBox.Show("Search Sring Length is " + v.Length.ToString());

            i = (((docs.Content.Text.Length - MyDocT.Length)) / v.Length);

            MessageBox.Show("Occurances is " + i.ToString());

            rng.Find.Replacement.Text = v;
            rng.Find.Text = "[]";
            rng.Find.Execute(Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll);

            return i;
        }

        private void btnExecute1_Click(object sender, EventArgs e)
        {
            CheckedListBox C = null;
            CheckedListBox C1 = null;
            int tabIndex = tabControl1.SelectedIndex;

            string TName = "TabCtrlPage" + (tabIndex - 1).ToString();
            TabControl TC = GetCtrl(TName) as TabControl;
            int Tindex = TC.SelectedIndex;
            string CHkLBNm = TName + "CLB_" + Tindex;
            CheckedListBox TabCL = GetCtrl(CHkLBNm) as CheckedListBox;

            if (tabIndex > 1)
            {
                if (tabIndex == 2) { C = GetCtrl("CheckedListBox_Page1") as CheckedListBox; C1 = GetCtrl("CheckedListBox_Page1_2") as CheckedListBox; }
                if (tabIndex == 3) { C = GetCtrl("CheckedListBox_Page2") as CheckedListBox; C1 = GetCtrl("CheckedListBox_Page2_2") as CheckedListBox; }
                if (tabIndex == 4) { C = GetCtrl("CheckedListBox_Page3") as CheckedListBox; C1 = GetCtrl("CheckedListBox_Page3_2") as CheckedListBox; }
                if (tabIndex == 5) { C = GetCtrl("CheckedListBox_Page4") as CheckedListBox; C1 = GetCtrl("CheckedListBox_Page4_2") as CheckedListBox; }
                if (tabIndex == 6) { C = GetCtrl("CheckedListBox_Page5") as CheckedListBox; C1 = GetCtrl("CheckedListBox_Page5_2") as CheckedListBox; }
                if (tabIndex == 7) { C = GetCtrl("CheckedListBox_Page6") as CheckedListBox; C1 = GetCtrl("CheckedListBox_Page6_2") as CheckedListBox; }
            }

            CheckedListBox CLB = (CheckedListBox)C;
            CheckedListBox CL2 = (CheckedListBox)C1;

            bool SAll = (TabCL.Items.Count == TabCL.CheckedItems.Count);

            Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;
            Microsoft.Office.Interop.Word.Range rng = docs.Content;
            rng.Find.ClearFormatting();


            object fText = "Sri Lanka";
            object rText = "Canada";

            if (SAll)
            {
                rng.Find.Text = fText.ToString();
                rng.Find.Replacement.Text = rText.ToString();
                rng.Find.Forward = true;
                rng.Find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
                rng.Find.Format = false;
                rng.Find.MatchCase = false;
                rng.Find.MatchWholeWord = false;
                rng.Find.MatchWildcards = false;
                rng.Find.MatchSoundsLike = false;
                rng.Find.MatchAllWordForms = false;

                rng.Find.Execute(Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll);
            }
            else
            {
                int i = 0;
                foreach (CheckBox item in TabCL.Controls)
                {
                    if (item.Checked == true)
                    {
                        int nindex = int.Parse(CL2.Items[i].ToString());
                        rng = docs.Sentences[nindex];
                        rng.Find.Text = fText.ToString();
                        rng.Find.Replacement.Text = rText.ToString();
                        rng.Find.Forward = true;
                        rng.Find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
                        rng.Find.Format = false;
                        rng.Find.MatchCase = false;
                        rng.Find.MatchWholeWord = false;
                        rng.Find.MatchWildcards = false;
                        rng.Find.MatchSoundsLike = false;
                        rng.Find.MatchAllWordForms = false;

                        rng.Find.Execute(Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                    }
                    i++;
                }
            }
        }


    }
}
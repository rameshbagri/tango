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

using Microsoft.Office.Tools.Ribbon;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace Tango
{
    public partial class TangoUserControl : UserControl
    {
        public TangoUserControl()
        {
            InitializeComponent();
            pnlFunctionList.Width = (int)(double)(pnlFunctionList.Parent.Width * .998);
            tabControl1.Width = (int)(double)(tabControl1.Parent.Width * .95);
            panel1.Width = (int)(double)(panel1.Parent.Width * .95);
            panel2.Width = (int)(double)(panel2.Parent.Width * .95);
            comboBox1.Width = (int)(double)(comboBox1.Parent.Width * .5);
            comboBox1.Left = 0;
            button2.Width = (int)(double)(button2.Parent.Width * .22);
            button2.Left = (int)(double)(button2.Parent.Width * .52); ;
            CmdRep.Width = (int)(double)(button2.Parent.Width * .22);
            CmdRep.Left = (int)(double)(button2.Parent.Width * .75); ;
            btnSelectAll.Width = (int)(double)(btnSelectAll.Parent.Width * .23);
            button1.Width = (int)(double)(button1.Parent.Width * .23);
            btnExecute.Width = (int)(double)(btnExecute.Parent.Width * .23);
            btnReser.Width = (int)(double)(btnReser.Parent.Width * .23);
            btnSelectAll.Left = 0;
            button1.Left = (int)(double)(button1.Parent.Width * .25);
            btnExecute.Left = (int)(double)(btnExecute.Parent.Width * .51);
            btnReser.Left = (int)(double)(btnReser.Parent.Width * .76);
        }

        private void btnProcess_Click_1(object sender, EventArgs e)
        {
            TabControl TbCtrl = (TabControl)GetCtrl("tabControl1");
            foreach (TabPage Page in TbCtrl.TabPages)
            {
                if (Page.Name != "HomePage")
                {
                    TbCtrl.Controls.Remove(Page);
                    Page.Dispose();
                }
                comboBox1.Items.Clear();
            }

            comboBox1.Items.Add("Home");
            Control CLB = GetCtrl("tabControl1");

            TabPage tp = new TabPage();
            tp.Name = "Summary";
            tp.Text = "Summary";
            ((TabControl)CLB).Controls.Add(tp);
            comboBox1.Items.Add("Summary");

            TextBox txtBox = new TextBox();
            txtBox.Name = "txtSummary";
            txtBox.Font = new Font("Arial", 9, FontStyle.Bold);

            txtBox.AppendText("String Count Summary");
            txtBox.AppendText(Environment.NewLine);
            txtBox.AppendText(Environment.NewLine);

            Control TabSummary = GetCtrl("Summary");

            ((TabPage)TabSummary).Controls.Add(txtBox);

            txtBox.Width = ((TabPage)TabSummary).Width;
            txtBox.Height = ((TabPage)TabSummary).Height;
            txtBox.Multiline = true;
            txtBox.ScrollBars = ScrollBars.Both;
            txtBox.ReadOnly = true;

            object[] SrchItem = { "Sri Lanka", "Wicket", "only", "merely", "actually", "fully", "generally", "completely", "rarely", "continuously", "immediately" };
            object[] ReplItem = { "Canada", "Bucket", "one and only", "hardly", "absolutely", "totally", "broadly", "thoroughly", "uncommon", "progressively", "instantly" };

            //MessageBox.Show(txtBox.Text);

            if (checkBox1.Checked)
            {
                tp = new TabPage();
                tp.Name = "Page1";
                tp.Text = checkBox1.Text;
                ((TabControl)CLB).Controls.Add(tp);
                txtBox.AppendText(AddSummary1A("Page1", "Page1_1", SrchItem, ReplItem));
                txtBox.AppendText(Environment.NewLine);
                //AddResult1("Page1", "Page1_1", SrchItem);
                comboBox1.Items.Add(checkBox1.Text);
            }
            TabPage t = new TabPage();
            tabControl1.SelectedIndex = 1;
            btnExecute.Enabled = false;
            LstEvents.Items.Add(((Button)sender).Name.ToString() + " clicked");
            LstEvents.Items.Add(" ====> " + ((Button)sender).Text.ToString() + " clicked");
        }

        private string AddSummary1A(string basePage, string addPage, object[] srchItem, object[] ReplItem)
        {
            //bool Trev = false;
            DateTime dt1 = DateTime.Now;
            Globals.ThisAddIn.Application.ScreenUpdating = false;
            string RetVal = "";
            Control Ctr = GetCtrl(basePage);
            Control P = AddPanel(addPage, 0, 0, Ctr.Width - 2, Ctr.Height - 2);
            Ctr.Controls.Add(P);

            /*TextBox TBOX = new TextBox();
            TBOX.Name = "FilterBox" + basePage;
            TBOX.Top = 10;
            TBOX.Width = (int)((double)(P.Width) * 0.98);
            TBOX.TextChanged += TextBox_TextChanged;*/

            CheckedListBox CLR = new CheckedListBox();
            CLR.Name = "CheckedListBox_R" + basePage;
            CheckedListBox CLB = new CheckedListBox();
            CLB.Name = "CheckedListBox_" + basePage;
            CLB.Top = 0; // TBOX.Height + 20;
            CLB.Left = 0;
            CLB.Height = (int)((double)(P.Height) * 0.4);
            CLB.Width = P.Width;

            TabControl TbCtrl = AddTabCtrl("TabCtrl" + basePage, (int)((double)(P.Height) * 0.42), 0, P.Width, (int)((double)(P.Height) - CLB.Height));

            Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;
            Microsoft.Office.Interop.Word.View v = Globals.ThisAddIn.Application.ActiveWindow.View;

            v.RevisionsView = Microsoft.Office.Interop.Word.WdRevisionsView.wdRevisionsViewFinal;
            v.ShowRevisionsAndComments = true;
            v.ShowComments = true;
            v.ShowFormatChanges = true;
            v.ShowInkAnnotations = true;
            v.ShowInsertionsAndDeletions = true;

            //Trev = docs.TrackRevisions;
            //docs.TrackRevisions = false;
            Microsoft.Office.Interop.Word.Range rng = docs.Content;
            rng.Find.ClearFormatting();

            int scount = docs.Sentences.Count;
            object[] findtext = srchItem;
            object[] repltext = ReplItem;
            int rng1count = 0;
            TabPage tp = new TabPage();
            int PageNo = -1;
            for (int i = 0; i < findtext.Length; i++)
            {
                PageNo++;
                tp = new TabPage();
                tp.Name = basePage + i.ToString();
                tp.Text = findtext[i].ToString();
                ((TabControl)TbCtrl).Controls.Add(tp);

                CheckedListBox CLB1N = new CheckedListBox();
                CLB1N.Name = "TabCtrl" + basePage + "CLBN_" + PageNo.ToString();
                CheckedListBox CLB1 = new CheckedListBox();
                CLB1.Name = "TabCtrl" + basePage + "CLB_" + PageNo.ToString();
                CLB1.Width = CLB.Width;
                CLB1.Height = (int)(double)(TbCtrl.Height * 0.99);
                tp.Controls.Add(CLB1);
                tp.Controls.Add(CLB1N);
                CLB1N.Visible = false;
                //MessageBox.Show(CLB1.Name);

                rng.Start = 0;
                rng1count = 0;
                rng.Find.Execute(ref findtext[i]);
                string Ctxt = "";
                string Ptxt = "";
                //MessageBox.Show(findtext[i].ToString());
                int sec = 1;
                while (rng.Find.Found)
                {
                    rng1count += 1;
                    rng.Select();
                    string setence = (Globals.ThisAddIn.Application.Selection.Range.Sentences[1].Text.Trim());
                    Ctxt = setence;

                    if (setence.Length > findtext[i].ToString().Length)
                    {
                        if (Ctxt.Equals(Ptxt)) { sec++; } else { sec = 1; }
                        //MessageBox.Show("ptxt is : " + Ptxt + Environment.NewLine + "Ctxt is : " + Ctxt + Environment.NewLine + "Count is : " + sec.ToString());
                        CLB1N.Items.Add(sec);
                        CLB1.Items.Add(Ctxt);
                        Ptxt = Ctxt;
                    }
                    rng.Find.Execute(ref findtext[i]);
                }
                if (rng1count == 0)
                {
                    TbCtrl.Controls.Remove(tp);
                    PageNo--;
                }
                else
                {
                    RetVal += findtext[i] + " => Word Count : " + rng1count.ToString() + Environment.NewLine + Environment.NewLine;
                    CLB.Items.Add(findtext[i] + " ( " + rng1count.ToString() + " )");
                    CLR.Items.Add(repltext[i]);
                    CLB1.Click += CheckedListBox_Click;
                    CLB1.ItemCheck += CLB1_ItemCheck;
                    CLB1.MouseHover += CheckedListBox_MouseHover;
                    CLB1.HorizontalScrollbar = true;
                }
                rng = docs.Content;
                rng.Start = 0;
                rng.End = 0;
                rng.Select();
            }
            CLB.Click += CheckedListBox_Click;
            CLB.ItemCheck += CheckedListBox_ItemCheck;
            CLB.MouseHover += CheckedListBox_MouseHover;
            CLB.MouseEnter += CheckedListBox_MouseEnter;
            CLB.MouseMove += CheckedListBox_MouseMove;

            //P.Controls.Add(TBOX);
            P.Controls.Add(CLR);
            CLR.Visible = false;
            P.Controls.Add(CLB);
            P.Controls.Add(TbCtrl);
            P.Visible = true;

            Globals.ThisAddIn.Application.ScreenUpdating = true;
            DateTime dt2 = DateTime.Now;

            TimeSpan dt = dt2 - dt1;
            //docs.TrackRevisions = Trev;
            btnExecute.Enabled = false;
            button2.Enabled = true;

            return RetVal;
        }

        private void CLB1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            LstEvents.Items.Add(((CheckedListBox)sender).Name.ToString() + " bottom part clicked");
            btnExecute.Enabled = false;
            int tabIndex = tabControl1.SelectedIndex;
            string TName = "TabCtrlPage" + (tabIndex - 1).ToString();
            TabControl TC = GetCtrl(TName) as TabControl;
            int si = TC.SelectedIndex;
            CheckState chkstat = e.NewValue;
            bool cstat = false;
            int UC_click = 0;
            if (chkstat.ToString().ToUpper().Equals("CHECKED"))
            {
                cstat = true;
            }
            else
            {
                UC_click = 1;
                for (int i = 0; i < TC.TabPages.Count; i++)
                {
                    TabPage TP = TC.TabPages[i];
                    string CHkLBNm = TName + "CLB_";
                    foreach (Control control1 in TP.Controls) { CHkLBNm = control1.Name.ToString(); }
                    CheckedListBox TabCL = GetCtrl(CHkLBNm) as CheckedListBox;
                    int ChkCount = TabCL.CheckedItems.Count - UC_click;
                    if (ChkCount >= 1) { cstat = true; }
                    UC_click = 0;
                }
            }
            if (cstat == true) { btnExecute.Enabled = true; } else { btnExecute.Enabled = false; }
            TC.SelectedTab = TC.TabPages[si];
        }

        private void CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            LstEvents.Items.Add(((CheckedListBox)sender).Name.ToString() + " Top part clicked");
            CheckState chkstat = e.NewValue;
            bool cstat = true;
            if (chkstat.ToString().ToUpper().Equals("UNCHECKED"))
            {
                cstat = false;
            }
            int i = e.Index;
            int tabIndex = tabControl1.SelectedIndex;
            string TName = "TabCtrlPage" + (tabIndex - 1).ToString();
            TabControl TC = GetCtrl(TName) as TabControl;
            TC.SelectedTab = TC.TabPages[i];

            TabPage TP = TC.TabPages[i];
            //MessageBox.Show(TP.Name);
            string CHkLBNm = TName + "CLB_";
            foreach (Control control1 in TP.Controls)
            {
                CHkLBNm = control1.Name.ToString();
            }
            CheckedListBox TabCL = GetCtrl(CHkLBNm) as CheckedListBox;

            for (i = 0; i < ((CheckedListBox)TabCL).Items.Count; i++)
            {
                ((CheckedListBox)TabCL).SetItemChecked(i, cstat);
            }
        }

        private void CheckedListBox_MouseMove(object sender, MouseEventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            string sendName = ((CheckedListBox)sender).Name;
            CheckedListBox C1 = GetCtrl(sendName) as CheckedListBox;
            CheckedListBox CLB = (CheckedListBox)C1;
            CLB.Focus();
        }

        private void CheckedListBox_MouseEnter(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            string sendName = ((CheckedListBox)sender).Name;
            CheckedListBox C1 = GetCtrl(sendName) as CheckedListBox;
            CheckedListBox CLB = (CheckedListBox)C1;
            CLB.Focus();
        }

        private void CheckedListBox_MouseHover(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            string sendName = ((CheckedListBox)sender).Name;
            CheckedListBox C1 = GetCtrl(sendName) as CheckedListBox;
            CheckedListBox CLB = (CheckedListBox)C1;
            CLB.Focus();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            string TName = "TabCtrlPage" + (tabIndex - 1).ToString();
            TabControl TC = GetCtrl(TName) as TabControl;
            int Tindex = TC.SelectedIndex;
            string ClbNm = "CheckedListBox_Page" + (tabIndex - 1).ToString();
            CheckedListBox CLB = GetCtrl(ClbNm) as CheckedListBox;
            string TboxName = "FilterBoxPage" + (tabIndex - 1).ToString();
            TextBox TB = GetCtrl(TboxName) as TextBox;
            int sl = TB.Text.Length;
            MessageBox.Show(TB.Text);
            MessageBox.Show(CLB.Items.Count.ToString());
            foreach (object CB in CLB.Controls)
            {
                CheckBox chkBOX = (CheckBox)CB as CheckBox;
                MessageBox.Show(chkBOX.Text.Substring(0, sl));
                if (chkBOX.Text.ToString().Substring(0, sl) == TB.Text)
                {
                    MessageBox.Show(chkBOX.Text.ToString());
                    chkBOX.Visible = true;
                }
                else
                {
                    chkBOX.Visible = false;
                }
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            string TName = "TabCtrlPage" + (tabIndex - 1).ToString();
            TabControl TC = GetCtrl(TName) as TabControl;
            string ClbNm = "CheckedListBox_Page" + (tabIndex - 1).ToString();
            CheckedListBox CLB = GetCtrl(ClbNm) as CheckedListBox;
            string ClbNm1 = "CheckedListBox_RPage" + (tabIndex - 1).ToString();
            CheckedListBox CLR = GetCtrl(ClbNm1) as CheckedListBox;
            List<Control> remotemall = new List<Control>();
            List<int> remotemall1 = new List<int>();
            TabPage pagename = new TabPage();

            for (int Tindex = TC.TabCount - 1; Tindex >= 0; --Tindex)
            {
                TabPage TP = TC.TabPages[Tindex];

                string CHkLBNmn = TName + "CLBN_" + Tindex;
                string CHkLBNm = TName + "CLB_" + Tindex;
                foreach (Control tpc in TP.Controls)
                {
                    if (tpc.Name.ToString().Contains("CLBN"))
                    {
                        CHkLBNmn = tpc.Name.ToString();
                    }
                    else
                    {
                        CHkLBNm = tpc.Name.ToString();
                    }
                }

                CheckedListBox TabCL = GetCtrl(CHkLBNm) as CheckedListBox;
                CheckedListBox TabCLn = GetCtrl(CHkLBNmn) as CheckedListBox;

                bool SAll = (TabCL.Items.Count == TabCL.CheckedItems.Count);
                Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;

                Microsoft.Office.Interop.Word.View v = Globals.ThisAddIn.Application.ActiveWindow.View;

                v.RevisionsView = Microsoft.Office.Interop.Word.WdRevisionsView.wdRevisionsViewFinal;
                v.ShowRevisionsAndComments = true;
                v.ShowComments = true;
                v.ShowFormatChanges = true;
                v.ShowInkAnnotations = true;
                v.ShowInsertionsAndDeletions = true;

                //bool Trev = docs.TrackRevisions;
                //docs.TrackRevisions = false;
                Microsoft.Office.Interop.Word.Range rng = docs.Content;
                rng.Find.ClearFormatting();
                string str1 = CLB.Items[Tindex].ToString();
                int cx1 = str1.IndexOf("(", 0);
                string str2 = str1.Substring(0, cx1).Trim();
                object fText = str2; //"Sri Lanka";
                object rText = CLR.Items[Tindex].ToString(); //"Canada";
                pagename = TC.TabPages[Tindex];
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
                    //docs.TrackRevisions = true;
                    rng.Find.Execute(Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll);
                    //docs.TrackRevisions = false;
                    remotemall.Add(pagename);
                    remotemall1.Add(Tindex);
                }
                else
                {
                    int i = 0;
                    List<int> remotem = new List<int>();
                    foreach (object itemChecked in TabCL.CheckedItems)
                    {
                        Microsoft.Office.Interop.Word.Range rng1 = docs.Content;
                        int len = itemChecked.ToString().Length;
                        if (len < 254)
                        {
                            rng1.Find.Text = itemChecked.ToString();
                        }
                        else
                        {
                            rng1.Find.Text = itemChecked.ToString().Substring(0, 254);
                        }
                        string txt111 = (rng1.Find.Text + "    ===>>    " + rng1.Find.Text.Length.ToString());
                        rng1.Find.Execute();
                        rng1.Select();
                        rng = (Globals.ThisAddIn.Application.Selection.Range.Sentences[1]);
                        rng.Find.Text = fText.ToString();
                        //MessageBox.Show(rText.ToString());
                        rng.Find.Replacement.Text = rText.ToString();
                        rng.Find.Forward = true;
                        rng.Find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
                        rng.Find.Format = false;
                        rng.Find.MatchCase = false;
                        rng.Find.MatchWholeWord = false;
                        rng.Find.MatchWildcards = false;
                        rng.Find.MatchSoundsLike = false;
                        rng.Find.MatchAllWordForms = false;
                        int indextodel = TabCL.FindString(itemChecked.ToString());
                        bool cnt = true;
                        while (cnt)
                        { if (!TabCL.GetItemChecked(indextodel)) { indextodel++; rng.Find.Execute(); } else { cnt = false; } }
                        remotem.Add(indextodel);
                        string ReplIndex = "No Value";
                        ReplIndex = TabCLn.Items[indextodel].ToString();

                        //docs.TrackRevisions = true;
                        rng.Find.Execute(Replace: Microsoft.Office.Interop.Word.WdReplace.wdReplaceOne);
                        //docs.TrackRevisions = false;
                        int countnum1 = CLB.Items[Tindex].ToString().IndexOf("(", 0);
                        int countnum2 = CLB.Items[Tindex].ToString().IndexOf(")", 0);

                        string nm = CLB.Items[Tindex].ToString().Substring(0, countnum1);
                        string NumPart = CLB.Items[Tindex].ToString().Substring(countnum1 + 1, countnum2 - countnum1 - 1).Trim();

                        int countnum = int.Parse(NumPart) - 1;

                        CLB.Items[Tindex] = nm + "( " + countnum.ToString() + " )";
                        //CLR.Items[Tindex] = countnum;
                        rng = docs.Content;
                        rng.Start = 0;
                        rng.End = 0;
                        rng.Select();
                    }
                    i++;
                    if (remotem.ToList().Count > 0)
                    {
                        Globals.ThisAddIn.Application.ScreenUpdating = false;
                        for (int it = TabCL.Items.Count - 1; it >= 0; it--)
                        {
                            TabCL.Items.RemoveAt(it);
                        }

                        rng = docs.Content;
                        rng.Find.ClearFormatting();
                        rng.Start = 0;
                        rng.Find.Execute(ref fText);
                        while (rng.Find.Found)
                        {
                            rng.Select();
                            string setence = (Globals.ThisAddIn.Application.Selection.Range.Sentences[1].Text.Trim());
                            if (setence.Length > fText.ToString().Length)
                            {
                                string txt = Globals.ThisAddIn.Application.Selection.Range.Sentences[1].Text.Trim();
                                TabCL.Items.Add(txt);
                            }
                            rng.Find.Execute(ref fText);
                        }
                        TabCL.Refresh();
                        rng = docs.Content;
                        rng.Start = 0;
                        rng.End = 0;
                        rng.Select();
                        Globals.ThisAddIn.Application.ScreenUpdating = true;
                    }
                }
                //docs.TrackRevisions = Trev;
            }
            remotemall.Sort();
            remotemall.Reverse();
            foreach (Control x in remotemall.ToList())
            {
                //MessageBox.Show(x.ToString());
                TC.Controls.Remove(x);
            }

            remotemall1.Sort();
            remotemall1.Reverse();
            foreach (int x in remotemall1.ToList())
            {
                MessageBox.Show(x.ToString());
                CLB.Items.RemoveAt(x);
            }

            string SumVal = "String Count Summary" + Environment.NewLine + Environment.NewLine;

            for (int i = 0; i < CLB.Items.Count; i++)
            {
                string str1 = CLB.Items[i].ToString();
                int cx1 = str1.IndexOf("(", 0);
                string str2 = str1.Substring(0, cx1).Trim();
                SumVal += str2 + " => Word Count : " + CLR.Items[i].ToString() + Environment.NewLine + Environment.NewLine;
            }
            TextBox txtBOX = GetCtrl("txtSummary") as TextBox;
            txtBOX.Text = "";
            txtBOX.AppendText(SumVal);
            btnExecute.Enabled = false;
            LstEvents.Items.Add(((Button)sender).Name.ToString() + " clicked");
            LstEvents.Items.Add(" ====> " + ((Button)sender).Text.ToString() + " clicked");
        }

        private void CheckedListBox_Click(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            string sendName = ((CheckedListBox)sender).Name;
            Microsoft.Office.Interop.Word.Document docs = Globals.ThisAddIn.Application.ActiveDocument;
            CheckedListBox C1 = GetCtrl(sendName) as CheckedListBox;
            CheckedListBox CLB = (CheckedListBox)C1;
            Microsoft.Office.Interop.Word.Range rng1 = docs.Content;
            string findText1 = CLB.SelectedItem.ToString();
            string findText = CLB.SelectedItem.ToString();
            if (findText1.Length > 254) { findText = findText1.Substring(0, 254); }
            string fText = findText.Trim();
            int scnt = findText.IndexOf("(", 0);
            if (scnt > 0) { fText = (findText.Substring(0, scnt)).Trim(); }

            rng1.Start = 0;
            rng1.Find.Forward = true;
            rng1.Find.ClearHitHighlight();
            rng1.Find.HitHighlight(FindText: fText, MatchCase: false, HighlightColor: Microsoft.Office.Interop.Word.WdColor.wdColorBlue, TextColor: Microsoft.Office.Interop.Word.WdColor.wdColorWhite);
            rng1.Find.Execute();

            rng1.Select();
            bool chkd = !(CLB.GetItemChecked(CLB.SelectedIndex));
            if (MastListBox(sendName))
            {
                int TIndex = CLB.SelectedIndex;
                string Cname = "TabCtrlPage" + (tabIndex - 1).ToString();
                TabControl TC = GetCtrl(Cname) as TabControl;
                TC.SelectTab(TIndex);
            }
            LstEvents.Items.Add(((CheckedListBox)sender).Name.ToString() + " clicked");
            LstEvents.Items.Add(" ====> " + ((CheckedListBox)sender).Text.ToString() + " clicked");
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
        private void button2_Click(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            bool TF = true;
            if (button2.Text == "Select All")
            {
                TF = true;
                button2.Text = "UnSelect All";
            }
            else
            {
                TF = false;
                button2.Text = "Select All";
            }

            if (tabIndex > 1)
            {
                string CName = "CheckedListBox_Page" + (tabIndex - 1).ToString();
                CheckedListBox CBL = GetCtrl(CName) as CheckedListBox;

                for (int i = 0; i < ((CheckedListBox)CBL).Items.Count; i++)
                {
                    ((CheckedListBox)CBL).SetItemChecked(i, TF);
                }
                LstEvents.Items.Add(((Button)sender).Name.ToString() + " clicked");
                LstEvents.Items.Add(" ====> " + ((Button)sender).Text.ToString() + " clicked");
            }
        }
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            int tabIndex = tabControl1.SelectedIndex;
            //MessageBox.Show(tabIndex.ToString());
            if (tabIndex == 0)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox4.Checked = true;
                checkBox5.Checked = true;
                checkBox6.Checked = true;
            }
            else if (tabIndex > 1)
            {
                string TName = "TabCtrlPage" + (tabIndex - 1).ToString();
                TabControl TC = GetCtrl(TName) as TabControl;
                TabPage TP = TC.SelectedTab;
                string CHkLBNm = TName + "CLB_";
                foreach (Control control1 in TP.Controls)
                {
                    CHkLBNm = control1.Name.ToString();
                }
                CheckedListBox TabCL = GetCtrl(CHkLBNm) as CheckedListBox;

                for (int i = 0; i < ((CheckedListBox)TabCL).Items.Count; i++)
                {
                    ((CheckedListBox)TabCL).SetItemChecked(i, true);
                }
            }
            LstEvents.Items.Add(((Button)sender).Name.ToString() + " clicked");
            LstEvents.Items.Add(" ====> " + ((Button)sender).Text.ToString() + " clicked");
        }
        private void btnReser_Click(object sender, EventArgs e)
        {
            TabControl TbCtrl = (TabControl)GetCtrl("tabControl1");
            foreach (TabPage Page in TbCtrl.TabPages)
            {
                if (Page.Name != "HomePage")
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
            LstEvents.Items.Add(((Button)sender).Name.ToString() + " clicked");
            LstEvents.Items.Add(" ====> " + ((Button)sender).Text.ToString() + " clicked");
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
            button2.Enabled = true;
            if (index < 2) { btnExecute.Enabled = false; button2.Enabled = false; }
            LstEvents.Items.Add(((TabControl)sender).Name.ToString() + " clicked");
            LstEvents.Items.Add(" ====> " + ((TabControl)sender).Text.ToString() + " clicked");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            tabControl1.SelectedIndex = index;
            LstEvents.Items.Add(((ComboBox)sender).Name.ToString() + " clicked");
            LstEvents.Items.Add(" ====> " + ((ComboBox)sender).Text.ToString() + " clicked");
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
                TabPage TP = TC.SelectedTab;
                string CHkLBNm = TName + "CLB_";
                foreach (Control control1 in TP.Controls)
                {
                    CHkLBNm = control1.Name.ToString();
                }
                CheckedListBox TabCL = GetCtrl(CHkLBNm) as CheckedListBox;

                for (int i = 0; i < ((CheckedListBox)TabCL).Items.Count; i++)
                {
                    ((CheckedListBox)TabCL).SetItemChecked(i, false);
                }
            }
            LstEvents.Items.Add(((Button)sender).Name.ToString() + " clicked");
            LstEvents.Items.Add(" ====> " + ((Button)sender).Text.ToString() + " clicked");
        }

        private void TangoUserControl_Resize(object sender, EventArgs e)
        {
            pnlFunctionList.Width = (int)(double)(pnlFunctionList.Parent.Width * .998);
            tabControl1.Width = (int)(double)(tabControl1.Parent.Width * .95);
            panel1.Width = (int)(double)(panel1.Parent.Width * .95);
            panel2.Width = (int)(double)(panel2.Parent.Width * .95);
            comboBox1.Width = (int)(double)(comboBox1.Parent.Width * .5);
            comboBox1.Left = 0;
            button2.Width = (int)(double)(button2.Parent.Width * .22);
            button2.Left = (int)(double)(button2.Parent.Width * .52); ;
            CmdRep.Width = (int)(double)(button2.Parent.Width * .22);
            CmdRep.Left = (int)(double)(button2.Parent.Width * .75); ;
            btnSelectAll.Width = (int)(double)(btnSelectAll.Parent.Width * .23);
            button1.Width = (int)(double)(button1.Parent.Width * .23);
            btnExecute.Width = (int)(double)(btnExecute.Parent.Width * .23);
            btnReser.Width = (int)(double)(btnReser.Parent.Width * .23);
            btnSelectAll.Left = 0;
            button1.Left = (int)(double)(button1.Parent.Width * .25);
            btnExecute.Left = (int)(double)(btnExecute.Parent.Width * .51);
            btnReser.Left = (int)(double)(btnReser.Parent.Width * .76);
        }

        private void checkBoxCheckedChanged(object sender, EventArgs e)
        {
            LstEvents.Items.Add(((CheckBox)sender).Name.ToString() + " clicked");
            BtnProcess.Enabled = false;
            if (checkBox1.Checked) { BtnProcess.Enabled = true; }
            if (checkBox2.Checked) { BtnProcess.Enabled = true; }
            if (checkBox3.Checked) { BtnProcess.Enabled = true; }
            if (checkBox4.Checked) { BtnProcess.Enabled = true; }
            if (checkBox5.Checked) { BtnProcess.Enabled = true; }
            if (checkBox6.Checked) { BtnProcess.Enabled = true; }
        }

        private void CmdRep_Click(object sender, EventArgs e)
        {
            if (CmdRep.Text.Equals("Report"))
            {
                CmdRep.Text = "Hide Report";
                tabControl1.Visible = false;
                LstEvents.Width = (int)(double)(tabControl1.Parent.Width * .95);
                LstEvents.Height = (int)(double)(tabControl1.Parent.Height * .8);
                LstEvents.Top = 100;
                LstEvents.Visible = true;
            }
            else
            {
                CmdRep.Text = "Report";
                tabControl1.Visible = true;
                LstEvents.Visible = false;
            }
            SendMail();
        }
        private void SendMail()
        {
            string path = @"C:\Tango\ReportLog" + DateTime.Now.ToString("_yyyy_mm_dd_hh_MM_ss") + ".txt";
            MessageBox.Show(path);
            MessageBox.Show("success");
            MessageBox.Show(LstEvents.Items.Count.ToString());

            using (FileStream fs = File.Create(path))
            {
                for (int i = 0; i < LstEvents.Items.Count; i++)
                {
                    Byte[] txt = new UTF8Encoding(true).GetBytes(LstEvents.Items[i].ToString() + Environment.NewLine);
                    fs.Write(txt, 0, txt.Length);
                    //MessageBox.Show(LstEvents.Items[i].ToString());
                }
            }

            //bool x = Is_OutLook_Running();
        }

        private bool Is_OutLook_Running()
        {
            bool RetVal = true;
            Boolean Result = true;
            Boolean CanQuit = true;

            Process[] Processes = Process.GetProcessesByName("OUTLOOK");
            Outlook.Application m_olApp = new Outlook.Application();
            if (Processes.Length != 0) { m_olApp = (Outlook.Application)(Marshal.GetActiveObject("Outlook.Application")); }
            Outlook.NameSpace m_olNamespace = m_olApp.GetNamespace("MAPI");
            if (Processes.Length != 0) { m_olNamespace = m_olApp.Session; }
            
            MessageBox.Show(Processes.Length.ToString());

            return RetVal;
        }
    }
}
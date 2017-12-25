using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using Microsoft.Office.Tools;
using System.Windows.Forms;

namespace Tango
{
    public partial class ThisAddIn
    {
        private Microsoft.Office.Tools.CustomTaskPane taskPane;
        private TangoUserControl TangoUC;
        public Word.Document doc;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            TangoUC = new TangoUserControl();
            taskPane = this.CustomTaskPanes.Add(TangoUC, "Task Pane");
            taskPane.Width = 320;
            doc = this.Application.ActiveDocument;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void TaskPane_VisibleChanged(object sender, System.EventArgs e)
        {
            
        }

        public Microsoft.Office.Tools.CustomTaskPane TaskPane
        {
            get
            {
                return taskPane;
            }
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        public int FindCount(object txt)
        {
            Word.Document doc = this.Application.ActiveDocument;
            Word.Range rng = doc.Content;

            rng.Find.ClearFormatting();
            rng.Find.Execute(ref txt);

            int FindCount = 0;
            while(rng.Find.Found)
            {
                FindCount++;
            }
            return FindCount;


        }
        #endregion
    }
}

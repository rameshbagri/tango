using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

namespace Tango
{
    public partial class TangoRibbon
    {
        
        private void TangoRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            
        }

        private void toggleButton1_Click(object sender, RibbonControlEventArgs e)
        {
            if (!(Globals.ThisAddIn.TaskPane.Visible = false)) { Globals.ThisAddIn.TaskPane.Visible = ((RibbonToggleButton)sender).Checked; }
        }
    }
}

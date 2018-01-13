namespace Tango
{
    partial class TangoRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public TangoRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Tango = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.toggleButton1 = this.Factory.CreateRibbonToggleButton();
            this.Tango.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tango
            // 
            this.Tango.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.Tango.Groups.Add(this.group1);
            this.Tango.Label = "TabAddIns";
            this.Tango.Name = "Tango";
            this.Tango.Tag = "Tango";
            // 
            // group1
            // 
            this.group1.Items.Add(this.toggleButton1);
            this.group1.Label = "Options";
            this.group1.Name = "group1";
            // 
            // toggleButton1
            // 
            this.toggleButton1.Label = "Execute";
            this.toggleButton1.Name = "toggleButton1";
            this.toggleButton1.ShowImage = true;
            this.toggleButton1.ShowLabel = false;
            this.toggleButton1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.toggleButton1_Click);
            // 
            // TangoRibbon
            // 
            this.Name = "TangoRibbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.Tango);
            this.Tag = "Tango";
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.TangoRibbon_Load);
            this.Tango.ResumeLayout(false);
            this.Tango.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab Tango;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton toggleButton1;
    }

    partial class ThisRibbonCollection
    {
        internal TangoRibbon TangoRibbon
        {
            get { return this.GetRibbon<TangoRibbon>(); }
        }
    }
}

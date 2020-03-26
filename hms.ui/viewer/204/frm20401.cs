using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Hms.Ui
{
    public partial class frm20401 : frmBaseMdi
    {
        public frm20401()
        {
            InitializeComponent();
        }

        public override void New()
        {
            frmPopup2040101 frm = new frmPopup2040101();
            frm.ShowDialog();
        }

        private void frm20401_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                this.imageListBoxControl.Items.Add((i + 1).ToString().PadLeft(4, '0'), 0);
            }

            DevExpress.XtraEditors.LabelControl lblNav = null;
            for (int i = 0; i < 100; i++)
            {
                lblNav = new DevExpress.XtraEditors.LabelControl();
                lblNav.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(172)))), ((int)(((byte)(226)))));
                lblNav.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblNav.Appearance.ForeColor = System.Drawing.Color.White;
                lblNav.Appearance.Options.UseBackColor = true;
                lblNav.Appearance.Options.UseFont = true;
                lblNav.Appearance.Options.UseForeColor = true;
                lblNav.Appearance.Options.UseTextOptions = true;
                lblNav.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                lblNav.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
                lblNav.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
                lblNav.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
                lblNav.Cursor = System.Windows.Forms.Cursors.Hand;
                lblNav.Dock = System.Windows.Forms.DockStyle.Top;
                lblNav.Name = "lbl" + i.ToString();
                lblNav.Size = new System.Drawing.Size(187, 40);
                lblNav.Text = "家族病史" + (i + 1).ToString();
                lblNav.Click += new System.EventHandler(this.lblNav_Click);
                navBarGroupControlContainer.Controls.Add(lblNav);
            }

            foreach (DevExpress.XtraNavBar.ViewInfo.BaseViewInfoRegistrator obj in this.navBarControl.AvailableNavBarViews)
            {
                this.navBarControl.View = obj;
                this.navBarControl.ResetStyles();
            }
        }

        private void lblNav_Click(object sender, EventArgs e)
        {
            MessageBox.Show((sender as DevExpress.XtraEditors.LabelControl).Text);
        }


    }
}

using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hms.Ui
{
    public partial class frm20402 : frmBaseMdi
    {
        public frm20402()
        {
            InitializeComponent();
        }

        public override void Edit()
        {
            frmPopup2040101 frm = new frmPopup2040101();
            frm.ShowDialog();
        }

        private void frm20402_Load(object sender, EventArgs e)
        {

        }
    }
}

using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hms.Ui
{
    public partial class frm20403 : frmBaseMdi
    {
        public frm20403()
        {
            InitializeComponent();
        }

        public override void Copy()
        {
            frmPopup2040301 frm = new frmPopup2040301();
            frm.ShowDialog();
        }

        public override void Remind()
        {
            frmPopup2040302 frm = new frmPopup2040302();
            frm.ShowDialog();
        }

    }
}

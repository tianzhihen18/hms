using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using weCare.Core.Entity;

namespace Hms.Ui
{
    public partial class Quest01 : UserControl, IQuest
    {
        public Quest01()
        {
            InitializeComponent();
        }

        public List<EntityDicQnDetail> GetQnCtrls()
        {
            List<EntityDicQnDetail> data = new List<EntityDicQnDetail>();
            foreach (Control ctr in plBack.Controls)
            {
                if (ctr is DevExpress.XtraEditors.CheckEdit)
                {
                    if (!string.IsNullOrEmpty(((DevExpress.XtraEditors.CheckEdit)ctr).Properties.AccessibleName) && ((DevExpress.XtraEditors.CheckEdit)ctr).Checked)
                    {
                        data.Add(new EntityDicQnDetail() { fieldId = ((DevExpress.XtraEditors.CheckEdit)ctr).Properties.AccessibleName });
                    }
                }
            }
            return data;
        }

        public void SetQnCtrls(List<EntityDicQnDetail> lstCtrls)
        {
            if (lstCtrls == null) return;
            string accessibleName = string.Empty;
            foreach (Control ctr in plBack.Controls)
            {
                if (ctr is DevExpress.XtraEditors.CheckEdit)
                {
                    if (!string.IsNullOrEmpty(((DevExpress.XtraEditors.CheckEdit)ctr).Properties.AccessibleName))
                    {
                        accessibleName = ((DevExpress.XtraEditors.CheckEdit)ctr).Properties.AccessibleName;
                        if (lstCtrls.Any(t => t.fieldId == accessibleName))
                        {
                            ((DevExpress.XtraEditors.CheckEdit)ctr).Checked = true;
                        }
                    }
                }
            }
        }

    }
}

using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hms.Entity;

namespace Hms.Ui
{
    public partial class frm20402 : frmBaseMdi
    {
        public frm20402()
        {
            InitializeComponent();
        }

        #region var/property
        List<EntityDisplayPromotionPlan> lstPromotionPlan { get; set; }
        #endregion

        #region overrid
        public override void Edit()
        {
            frmPopup2040101 frm = new frmPopup2040101();
            frm.ShowDialog();
        }
        #endregion


        #region methods
        void Init()
        {
            using (ProxyHms proxy = new ProxyHms())
            {
                lstPromotionPlan = proxy.Service.GetPromotionPlans();
                gridControl.DataSource = lstPromotionPlan;
                gridControl.RefreshDataSource();
            }
        }
        #endregion

        #region events

        private void frm20402_Load(object sender, EventArgs e)
        {
            Init();
        }
        
        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        #endregion
    }
}

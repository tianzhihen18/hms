using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hms.Entity;

namespace Hms.Ui
{
    public partial class frm20202 : frmBaseMdi
    {
        public frm20202()
        {
            InitializeComponent();
        }

        #region var
        List<EntityQnRecord> lstQnRecords { get; set; }
        List<EntityClientInfo> lstClientInfo { get; set; }
        #endregion

        #region override
        public override void New()
        {
            frmPopup2020101 frm = new frmPopup2020101(lstClientInfo);
            frm.ShowDialog();
            if (frm.IsRequireRefresh)
            {
                this.RefreshData();
            }
        }

        public override void Edit()
        {
            if (this.gvNormalQnRecord.SelectedRowsCount > 0)
            {
                EntityQnRecord vo = this.gvNormalQnRecord.GetRow(this.gvNormalQnRecord.GetSelectedRows()[0]) as EntityQnRecord;
                frmPopup2020101 frm = new frmPopup2020101(vo,lstClientInfo);
                frm.ShowDialog();
                if (frm.IsRequireRefresh)
                {
                    this.RefreshData();
                }
            }
            else
            {
                DialogBox.Msg("请选择要编辑的记录.");
            }
        }


        #region RefreshData
        /// <summary>
        /// RefreshData
        /// </summary>
        public override void RefreshData()
        {
            uiHelper.BeginLoading(this);

            using (ProxyHms proxy = new ProxyHms())
            {
                lstQnRecords = proxy.Service.GetQnRecords();
            }
            this.gcNormalQnRecord.DataSource = lstQnRecords;
            this.gcNormalQnRecord.RefreshDataSource();
            uiHelper.CloseLoading(this);
        }
        #endregion

        #endregion

        #region methods

        #region Init
        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            using (ProxyHms proxy = new ProxyHms())
            {
                lstQnRecords = proxy.Service.GetQnRecords();
                lstClientInfo = proxy.Service.GetClientInfos();
            }
            this.gcNormalQnRecord.DataSource = lstQnRecords;
            this.gcNormalQnRecord.RefreshDataSource();
        }

        #endregion

        #endregion

        #region events
        private void frm20202_Load(object sender, EventArgs e)
        {
            Init();
        }
        #endregion

        private void gcNormalQnRecord_DoubleClick(object sender, EventArgs e)
        {
            this.Edit();
        }
    }
}

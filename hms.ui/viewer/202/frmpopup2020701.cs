using Common.Controls;
using Hms.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using weCare.Core.Entity;

namespace Hms.Ui
{
    public partial class frmpopup2020701 : frmBasePopup
    {
        public frmpopup2020701(List<EntityClientInfo> _lstClientInfo )
        {
            InitializeComponent();
            lstClientInfo = _lstClientInfo;
        }

        #region var
        List<EntityClientInfo> lstClientInfo { get; set; }
        List<EntityDicQnMain> lstQnMain { get; set; }

        #endregion

        #region methods
        void Init()
        {
            using (ProxyHms proxy = new ProxyHms())
            {
                lstQnMain = proxy.Service.GetQnMain(null);
            }

            this.gridControl.DataSource = lstQnMain;
        }



        public EntityDicQnMain GetRowObject()
        {
            EntityDicQnMain vo = null;
            if (this.cardView.FocusedRowHandle >= 0)
                vo = this.cardView.GetRow(this.cardView.FocusedRowHandle) as EntityDicQnMain;

            return vo;
        }
        #endregion


        #region events

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmPopup2020702 frm = new frmPopup2020702(GetRowObject(), lstClientInfo);
            frm.ShowDialog();
            this.Close();
        }

        private void frmpopup2020701_Load(object sender, EventArgs e)
        {
            Init();
        }
        #endregion

        private void lstChkQn_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

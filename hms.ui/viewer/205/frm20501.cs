using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Hms.Entity;

namespace Hms.Ui
{
    /// <summary>
    /// 慢病-高血压
    /// </summary>
    public partial class frm20501 : frmBaseMdi
    {
        #region ctor
        /// <summary>
        /// ctor
        /// </summary>
        public frm20501()
        {
            InitializeComponent();
        }
        #endregion

        #region override

        /// <summary>
        /// 添加人员
        /// </summary>
        public override void New()
        {
            frmPopup2050103 frm = new frmPopup2050103();
            frm.ShowDialog();
        }
        /// <summary>
        /// 添加计划
        /// </summary>
        public override void Copy()
        {
            frmPopup2050104 frm = new frmPopup2050104();
            frm.ShowDialog();
        }
        /// <summary>
        /// 随访
        /// </summary>
        public override void Remind()
        {
            if (this.gridView1.SelectedRowsCount > 0)
            {
                frmPopup2050101 frm = new frmPopup2050101(this.gridView1.GetRow(this.gridView1.GetSelectedRows()[0]) as EntityHmsSF);
                frm.ShowDialog();
                if (frm.IsRequireRefresh)
                {
                    this.RefreshData();
                    this.ScrollRow(1, frm.sfVo.sfId);
                }
            }
            else
            {
                DialogBox.Msg("请选择要编辑的记录.");
            }
        }
        /// <summary>
        /// 评估
        /// </summary>
        public override void Capture()
        {
            if (this.gridView2.SelectedRowsCount > 0)
            {
                frmPopup2050102 frm = new frmPopup2050102(this.gridView2.GetRow(this.gridView2.GetSelectedRows()[0]) as EntityHmsSF);
                frm.ShowDialog();
                if (frm.IsRequireRefresh)
                {
                    this.RefreshData();
                    this.ScrollRow(2, frm.pgVo.pgId);
                }
            }
            else
            {
                DialogBox.Msg("请选择要编辑的记录.");
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        public override void Edit()
        {

        }
        /// <summary>
        /// 删除
        /// </summary>
        public override void Delete()
        {

        }
        /// <summary>
        /// 查询
        /// </summary>
        public override void Search()
        {

        }
        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshData()
        {

        }
        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {

        }
        /// <summary>
        /// 导出
        /// </summary>
        public override void Export()
        {

        }

        #endregion

        #region var/property


        #endregion

        #region method

        #region ScrollRow
        /// <summary>
        /// ScrollRow
        /// </summary>
        /// <param name="flagId"></param>
        /// <param name="id"></param>
        void ScrollRow(int flagId, string id)
        {
            for (int i = 0; i < this.gridView.RowCount; i++)
            {
                this.gridView.UnselectRow(i);
            }
            for (int i = 0; i < this.gridView.RowCount; i++)
            {
                //if ((this.gridView.GetRow(i) as EntityDicSportItem).sId == sId)
                //{
                //    this.gridView.FocusedRowHandle = i;
                //    this.gridView.SelectRow(i);
                //    break;
                //}
            }
        }
        #endregion

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        void Init()
        {
            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyHms proxy = new ProxyHms())
                {
                    this.gridControl.DataSource = proxy.Service.GetGxyPatients(null);
                    this.gridControl1.DataSource = proxy.Service.GetGxySfRecords(null);
                    this.gridControl2.DataSource = proxy.Service.GetGxyPgRecords(null);
                }

            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }
        #endregion

        #endregion

        #region event

        private void frm20501_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.ForeColor = Color.Gray;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.ForeColor = Color.Gray;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.ForeColor = Color.Gray;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            this.Remind();
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            this.Capture();
        }

        #endregion


    }
}

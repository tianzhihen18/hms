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
    public partial class frm20401 : frmBaseMdi
    {
        public frm20401()
        {
            InitializeComponent();
        }

        #region var/property
        List<EntityClientInfo> lstClientInfo { get; set; }
        List<EntityPromotionTemplate> lstPromotionTemplate { get; set; }
        List<EntityPromotionTemplateConfig> lstPromotionTemplateConfig { get; set; }
        List<EntityPromotionTemplateConfig> lstPromotionPlans { get; set; }
        #endregion

        #region override

        #region new
        /// <summary>
        /// 
        /// </summary>
        public override void New()
        {
            frmPopup2040101 frm = new frmPopup2040101();
            frm.ShowDialog();

            if(frm.IsRequireRefresh)
            {
                EntityPromotionTemplateConfig vo = frm.promotionTemplateConfig;

                if(!lstPromotionPlans.Exists(r=>r.planPeriod == vo.planPeriod && r.planWay == vo.planWay && r.planContent == vo.planContent))
                {
                    lstPromotionPlans.Add(vo);
                    gcPlan.DataSource = lstPromotionPlans;
                    gcPlan.RefreshDataSource();
                }
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// 
        /// </summary>
        public override void Delete()
        {
            if (gvPlan.SelectedRowsCount > 0)
            {
                for (int i = 0; i < gvPlan.RowCount; i++)
                {
                    if (gvPlan.IsRowSelected(i))
                    {
                        EntityPromotionTemplateConfig ptPlan = gvPlan.GetRow(i) as EntityPromotionTemplateConfig;
                        lstPromotionPlans.Remove(ptPlan);
                    }
                }

                gcPlan.DataSource = lstPromotionPlans;
                gcPlan.RefreshDataSource();
            }
        }
        #endregion

        #endregion

        #region methods
        void Init()
        {
            try
            {
                uiHelper.BeginLoading(this);
                LoadQnDataSource();
                lstPromotionPlans = new List<EntityPromotionTemplateConfig>();
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }


        #region LoadQnDataSource
        /// <summary>
        /// LoadQnDataSource
        /// </summary>
        void LoadQnDataSource()
        {
            lstClientInfo = null;
            using (ProxyHms proxy = new ProxyHms())
            {
                lstClientInfo = proxy.Service.GetClientInfos();
                gcClient.DataSource = lstClientInfo;
                gcClient.RefreshDataSource();

                lstPromotionTemplate = proxy.Service.GetPromotionTemplates();
                gcPromotionTemplate.DataSource = lstPromotionTemplate;
                gcPromotionTemplate.RefreshDataSource();

                lstPromotionTemplateConfig = proxy.Service.GetPromotionTemplateConfigs();
                gcPromotionTemplateConfig.DataSource = lstPromotionTemplateConfig;
                gcPromotionTemplateConfig.RefreshDataSource();
            }
        }
        #endregion

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityClientInfo GetRowObject()
        {
            if (this.gvClient.FocusedRowHandle < 0) return null;
            return this.gvClient.GetRow(this.gvClient.FocusedRowHandle) as EntityClientInfo;
        }
        #endregion


        #region
        List<EntityClientInfo> GetSelectData()
        {
            List<EntityClientInfo> data = new List<EntityClientInfo>();

            for (int i = 0;i< this.gvClient.RowCount ; i++)
            {
                if (this.gvClient.IsRowSelected(i))
                {
                    data.Add((this.gvClient.GetRow(i) as EntityClientInfo));
                }
            }

            return data;
        }
        #endregion

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityPromotionTemplate GetRowTemplateObject()
        {
            if (this.gvPromotionTemplate.FocusedRowHandle < 0) return null;
            return this.gvPromotionTemplate.GetRow(this.gvPromotionTemplate.FocusedRowHandle) as EntityPromotionTemplate;
        }
        #endregion


        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityPromotionTemplateConfig GetRowTemplateConfigObject()
        {
            if (this.gvPromotionTemplateConfig.FocusedRowHandle < 0) return null;
            return this.gvPromotionTemplateConfig.GetRow(this.gvPromotionTemplateConfig.FocusedRowHandle) as EntityPromotionTemplateConfig;
        }
        #endregion

        #endregion

        #region event
        private void frm20401_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void lblNav_Click(object sender, EventArgs e)
        {
            MessageBox.Show((sender as DevExpress.XtraEditors.LabelControl).Text);
        }

        private void gvClient_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                if (gvClient.IsRowSelected(e.RowHandle))
                    gvClient.UnselectRow(e.RowHandle);
                else
                    gvClient.SelectRow(e.RowHandle);
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {

        }

        

        private void timer_Tick(object sender, EventArgs e)
        {
            this.gridControl.DataSource = GetSelectData();
            this.gridControl.RefreshDataSource();
            lblSelect.Text = "共 " + GetSelectData().Count.ToString() + " 人";
        }

        private void gvPromotionTemplate_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if(e.RowHandle >= 0)
            {
                EntityPromotionTemplate vo = GetRowTemplateObject();
                gcPromotionTemplateConfig.DataSource = lstPromotionTemplateConfig.FindAll(r=>r.templateId == vo.id);
                gcPromotionTemplateConfig.RefreshDataSource();
                List<EntityPromotionTemplateConfig>  lstTmp = lstPromotionPlans.FindAll(r=>r.templateId ==vo.id);
                if(lstTmp != null && lstTmp.Count> 0)
                {
                    foreach(var tmp in lstTmp)
                    {
                        for(int  i =0;i<gvPromotionTemplateConfig.RowCount;i++)
                        {
                            EntityPromotionTemplateConfig ptConfig = gvPromotionTemplateConfig.GetRow(i)  as EntityPromotionTemplateConfig;
                            if (tmp.id == ptConfig.id)
                                gvPromotionTemplateConfig.SelectRow(i);
                        }
                    }
                }
            }
        }

        private void gvPromotionTemplateConfig_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                EntityPromotionTemplateConfig vo = GetRowTemplateConfigObject();
                if (gvPromotionTemplateConfig.IsRowSelected(e.RowHandle))
                {
                    gvPromotionTemplateConfig.UnselectRow(e.RowHandle);
                    lstPromotionPlans.Remove(vo);
                }
                else
                {
                    gvPromotionTemplateConfig.SelectRow(e.RowHandle);
                    if(!lstPromotionPlans.Exists(r=>r.id == vo.id))
                        lstPromotionPlans.Add(vo);
                }

                gcPlan.DataSource = lstPromotionPlans;
                gcPlan.RefreshDataSource();
            }
        }

        #endregion
    }
}

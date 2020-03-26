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

namespace Hms.Ui
{
    public partial class frmPopup2060301 : frmBasePopup
    {
        public frmPopup2060301(EntityDietTemplate _dietTemplate = null)
        {
            InitializeComponent();
            dietTemplate = _dietTemplate;
        }

        #region var/propery
        public EntityDietTemplate dietTemplate = null;
        public List<EntityDietTemplatetype> lstDietTemplatetype = null;
        public bool IsRequireRefresh = false;
        #endregion

        #region method
        void Init()
        {
            using (ProxyHms proxy = new ProxyHms())
            {
                lstDietTemplatetype = proxy.Service.GetDietTemplatetype();
            }

            if(lstDietTemplatetype.Count > 0)
            {
                foreach (var vo in lstDietTemplatetype)
                    this.cboType.Properties.Items.Add(vo.typeName);
            }
        }
        #endregion

        #region event
        private void frmPopup2060301_Load(object sender, EventArgs e)
        {
            Init();

            if(dietTemplate != null)
            {
                this.Text = "查看/编辑饮食菜谱模板";
                this.txtName.Text = dietTemplate.templateName;
                this.cboType.Text = lstDietTemplatetype.FindAll(r=>r.typeId == dietTemplate.typeid).FirstOrDefault().typeName;
                this.memDescriptions.Text = dietTemplate.descriptions;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int affect = -1;
            if (string.IsNullOrEmpty(dietTemplate.templateId))
                dietTemplate = new EntityDietTemplate();

            dietTemplate.templateName = this.txtName.Text;
            dietTemplate.descriptions = memDescriptions.Text;
            dietTemplate.typeid = lstDietTemplatetype.FindAll(r=>r.typeName== cboType.Text).FirstOrDefault().typeId;
            dietTemplate.creator = "00";
            dietTemplate.createName = "系统管理员";

            using (ProxyHms proxy = new ProxyHms())
            {
                affect = proxy.Service.SaveDietTemplate(ref dietTemplate);
            }

            if (affect < 0)
            {
                DialogBox.Msg("保存失败 !");
            }
            else
                this.IsRequireRefresh = true;
        }
        #endregion
    }
}

using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Hms.Entity;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Hms.Ui
{
    public partial class frm20301 : frmBaseMdi
    {
        public frm20301()
        {
            InitializeComponent();
        }


        #region var/property
        List<EntityDisplayClientRpt> lstClientInfo { get; set; }
        #endregion

        #region  override
        /// <summary>
        /// 
        /// </summary>
        public override void Search()
        {
            string name = this.txtName.Text;

            if (!string.IsNullOrEmpty(name))
            {
                this.gridControl.DataSource = this.lstClientInfo.FindAll(r => r.clientName.Contains(name));
            }
            else
            {
                this.gridControl.DataSource = this.lstClientInfo;
            }
            this.gridControl.RefreshDataSource();
        }

        public override void Edit()
        {
            EntityDisplayClientRpt rpt = GetRowObject();
            rpt.image01 = ReadImageFile("pic01.png");
            rpt.image02 = ReadImageFile("pic02.jpg");
            rpt.image03 = ReadImageFile("pic03.png");
            rpt.image04 = ReadImageFile("pic04.png");
            rpt.image05 = ReadImageFile("pic05.png");
            rpt.imageFx = ReadImageFile("picFx.png");
            rpt.imageTip = ReadImageFile("picTip.png");
            rpt.image07 = ReadImageFile("pic07.png");
            rpt.lstGxy = GetGxy();
            rpt.lstMainIndicate = GetMainIndicate();
            rpt.lstAdPeItemBse = GetAdPebse();
            rpt.lstAdPeItemSpecial = GetAdPeSpecial();
            rpt.lstMedAdvices = GetMedicalAdvicecs();
            frmPopup2030101 frm = new frmPopup2030101(rpt);
            frm.ShowDialog();
        }

        #endregion

        #region methods

        #region Init
        internal void Init()
        {
            try
            {
                uiHelper.BeginLoading(this);
                RefreshData();
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }
        #endregion

        #region RefreshData
        /// <summary>
        /// RefreshData
        /// </summary>
        public override void RefreshData()
        {
            uiHelper.BeginLoading(this);
            this.LoadQnDataSource();
            this.gridControl.DataSource = this.lstClientInfo;
            this.gridControl.RefreshDataSource();
            uiHelper.CloseLoading(this);
        }
        #endregion

        #region LoadQnDataSource
        /// <summary>
        /// LoadQnDataSource
        /// </summary>
        void LoadQnDataSource()
        {
            lstClientInfo = null;
            using (ProxyHms proxy = new ProxyHms())
            {
                lstClientInfo = proxy.Service.GetClientReports();
            }
        }
        #endregion

        #region 重要指标
        /// <summary>
        /// 重要指标
        /// </summary>
        /// <returns></returns>
        internal List<EntityEvaluateParams> GetMainIndicate()
        {
            List<EntityEvaluateParams> data = new List<EntityEvaluateParams>();
            EntityEvaluateParams vo1 = new EntityEvaluateParams();
            vo1.item = "身高";
            vo1.result = "176.5";
            vo1.range = "";
            vo1.unit = "cm";
            data.Add(vo1);
            EntityEvaluateParams vo2 = new EntityEvaluateParams();
            vo2.item = "体重";
            vo2.result = "72.2";
            vo2.range = "";
            vo2.unit = "kg";
            data.Add(vo2);
            EntityEvaluateParams vo3 = new EntityEvaluateParams();
            vo3.item = "身高体重指数";
            vo3.result = "23.05";
            vo3.range = "";
            vo3.unit = "";
            data.Add(vo3);

            EntityEvaluateParams vo4 = new EntityEvaluateParams();
            vo4.item = "收缩压";
            vo4.result = "114";
            vo4.range = "130 - 139";
            vo4.unit = "mmHg";
            data.Add(vo4);

            EntityEvaluateParams vo5 = new EntityEvaluateParams();
            vo5.item = "舒张压";
            vo5.result = "75";
            vo5.range = "85 - 89";
            vo5.unit = "mmHg";
            data.Add(vo5);

            EntityEvaluateParams vo6 = new EntityEvaluateParams();
            vo6.pic = ReadImageFile("pic06.png");
            vo6.item = "尿酸（UA）";
            vo6.result = "488 ↑";
            vo6.range = "90 - 420";
            vo6.unit = "umol/L";
            data.Add(vo6);

            EntityEvaluateParams vo7 = new EntityEvaluateParams();
            vo7.item = "总胆固醇(TCHO)";
            vo7.result = "3.76";
            vo7.range = "3.76";
            vo7.unit = "mmol/L";
            data.Add(vo7);

            EntityEvaluateParams vo8 = new EntityEvaluateParams();
            vo8.item = "甘油三酯(TG)";
            vo8.result = "1.74";
            vo8.range = "0.41-1.86";
            vo8.unit = "mmol/L";
            data.Add(vo8);

            EntityEvaluateParams vo9 = new EntityEvaluateParams();
            vo9.item = "高密度脂蛋白胆固醇(HDL - C)";
            vo9.result = "1.29";
            vo9.range = "0.83-2.16";
            vo9.unit = "mmol/L";
            data.Add(vo9);

            EntityEvaluateParams vo10 = new EntityEvaluateParams();
            vo10.item = "低密度脂蛋白胆固醇(LDL - C)";
            vo10.result = "2.18";
            vo10.range = "0-3.10";
            vo10.unit = "mmol/L";
            data.Add(vo10);

            EntityEvaluateParams vo11 = new EntityEvaluateParams();
            vo11.item = "糖化血红蛋白(HbA1c))";
            vo11.result = "4.6";
            vo11.range = "3-6.2";
            vo11.unit = "%";
            data.Add(vo11);

            EntityEvaluateParams vo12 = new EntityEvaluateParams();
            vo12.item = "空腹血糖(GLU)";
            vo12.result = "4.92";
            vo12.range = "3.9-6.1";
            vo12.unit = "mmol/L";
            data.Add(vo12);

            EntityEvaluateParams vo13 = new EntityEvaluateParams();
            vo13.item = "同型半胱氨酸(HCY)";
            vo13.result = "9.1";
            vo13.range = "4.0-15.0";
            vo13.unit = "mmol/L";
            data.Add(vo13);

            return data;
        }
        #endregion

        #region 高血压
        /// <summary>
        /// 高血压
        /// </summary>
        /// <returns></returns>
        internal List<EntityEvaluateParams> GetGxy()
        {
            List<EntityEvaluateParams> data = new List<EntityEvaluateParams>();
            EntityEvaluateParams vo1 = new EntityEvaluateParams();
            vo1.item = "身高体重指数";
            vo1.result = "23.05";
            vo1.range = "";
            vo1.unit = "";
            data.Add(vo1);
            EntityEvaluateParams vo2 = new EntityEvaluateParams();
            vo2.item = "收缩压";
            vo2.result = "114";
            vo2.range = "130－139";
            vo2.unit = "mmHg";
            data.Add(vo2);
            EntityEvaluateParams vo3 = new EntityEvaluateParams();
            vo3.item = "舒张压";
            vo3.result = "舒张压";
            vo3.range = "85－89";
            vo3.unit = "";
            data.Add(vo3);

            EntityEvaluateParams vo4 = new EntityEvaluateParams();
            vo4.item = "饮食喜好咸";
            vo4.result = "未填";
            vo4.range = "否";
            vo4.unit = "";
            data.Add(vo4);

            EntityEvaluateParams vo5 = new EntityEvaluateParams();
            vo5.item = "每次运动锻炼时间";
            vo5.result = "未填";
            vo5.range = "30~<60分钟";
            vo5.unit = "";
            data.Add(vo5);

            EntityEvaluateParams vo6 = new EntityEvaluateParams();
            vo6.item = "饮酒状态";
            vo6.result = "未填";
            vo6.range = "从不";
            vo6.unit = "";
            data.Add(vo6);

            EntityEvaluateParams vo7 = new EntityEvaluateParams();
            vo7.item = "总胆固醇(TCHO)";
            vo7.result = "3.76";
            vo7.range = "2.9~5.7";
            vo7.unit = "mmol/L";
            data.Add(vo7);


            EntityEvaluateParams vo8 = new EntityEvaluateParams();
            vo8.item = "低密度脂蛋白胆固醇(LDL-C)";
            vo8.result = "2.18";
            vo8.range = "2.18";
            vo8.unit = "mmol/L";
            data.Add(vo8);

            EntityEvaluateParams vo9 = new EntityEvaluateParams();
            vo9.item = "糖尿病";
            vo9.result = "无";
            vo9.range = "无";
            vo9.unit = "";
            data.Add(vo9);

            return data;
        }
        #endregion

        #region 就医检查建议
        internal List<EntityAdPeItem> GetAdPebse()
        {
            List<EntityAdPeItem> data = new List<EntityAdPeItem>();

            EntityAdPeItem vo1 = new EntityAdPeItem();
            vo1.item1 = "健康自测问卷";
            vo1.item2 = "一般检查（身高、体重、腰围、臀围、血压、脉搏）";
            vo1.item3 = "内科";
            data.Add(vo1);

            EntityAdPeItem vo2 = new EntityAdPeItem();
            vo2.item1 = "外科";
            vo2.item2 = "眼科";
            vo2.item3 = "耳鼻咽喉科";
            data.Add(vo2);

            EntityAdPeItem vo3 = new EntityAdPeItem();
            vo3.item1 = "口腔科";
            vo3.item2 = "血常规";
            vo3.item3 = "尿常规";
            data.Add(vo3);

            EntityAdPeItem vo4 = new EntityAdPeItem();
            vo4.item1 = "便常规+潜血";
            vo4.item2 = "肝功能";
            vo4.item3 = "肾功能";
            data.Add(vo4);

            EntityAdPeItem vo5 = new EntityAdPeItem();
            vo5.item1 = "血脂";
            vo5.item2 = "血糖";
            vo5.item3 = "心电图检查";
            data.Add(vo5);

            EntityAdPeItem vo6 = new EntityAdPeItem();
            vo6.item1 = "DR胸片";
            vo6.item2 = "腹部超声（肝胆脾胰肾）";
            vo6.item3 = "";
            data.Add(vo6);

            return data;
        }
        #endregion

        #region 专项检查
        internal List<EntityAdPeItem> GetAdPeSpecial()
        {
            List<EntityAdPeItem> data = new List<EntityAdPeItem>();

            EntityAdPeItem vo1 = new EntityAdPeItem();
            vo1.item1 = "肺功能";
            vo1.item2 = "骨密度检测";
            vo1.item3 = "血液流变学";
            data.Add(vo1);

            EntityAdPeItem vo2 = new EntityAdPeItem();
            vo2.item1 = "前列腺彩超";
            vo2.item2 = "心脏彩超";
            vo2.item3 = "颈动脉彩超";
            data.Add(vo2);

            EntityAdPeItem vo3 = new EntityAdPeItem();
            vo3.item1 = "甲状腺彩超";
            vo3.item2 = "甲状腺激素";
            vo3.item3 = "幽门螺杆菌（呼气法）";
            data.Add(vo3);

            EntityAdPeItem vo4 = new EntityAdPeItem();
            vo4.item1 = "颈椎片";
            vo4.item2 = "腰椎片";
            vo4.item3 = "经颅彩色多普勒超声";
            data.Add(vo4);

            EntityAdPeItem vo5 = new EntityAdPeItem();
            vo5.item1 = "动脉硬化检测";
            vo5.item2 = "经颅彩色多普勒超声";
            vo5.item3 = "心理测试";
            data.Add(vo5);

            return data;
        }
        #endregion

        #region 就医建议
        internal List<EntityMedicalAdvicecs> GetMedicalAdvicecs()
        {
            List<EntityMedicalAdvicecs> data = new List<EntityMedicalAdvicecs>();
            EntityMedicalAdvicecs vo1 = new EntityMedicalAdvicecs();
            vo1.important = "★★★";
            vo1.unnormal = "右肾结石";
            vo1.medDate = "近期就医";
            vo1.refferDept = "泌尿外科";
            data.Add(vo1);

            EntityMedicalAdvicecs vo2 = new EntityMedicalAdvicecs();
            vo2.important = "★★★";
            vo2.unnormal = "双眼屈光不正";
            vo2.medDate = "择期就医";
            vo2.refferDept = "眼科";
            data.Add(vo2);

            EntityMedicalAdvicecs vo3 = new EntityMedicalAdvicecs();
            vo3.important = "★★★";
            vo3.unnormal = "尿酸(UA)偏高";
            vo3.medDate = "择期就医";
            vo3.refferDept = "肾内科";
            data.Add(vo3);

            EntityMedicalAdvicecs vo4 = new EntityMedicalAdvicecs();
            vo4.important = "★★★";
            vo4.unnormal = "鼻甲肥大";
            vo4.medDate = "择期就医";
            vo4.refferDept = "耳鼻喉科";
            data.Add(vo4);

            EntityMedicalAdvicecs vo5 = new EntityMedicalAdvicecs();
            vo5.unnormal = "心内结构大致正常，CDFI：三尖瓣、主动脉瓣轻度返流";
            vo5.medDate = "未指定";
            data.Add(vo5);

            return data;
        }
        #endregion

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityDisplayClientRpt GetRowObject()
        {
            if (this.gridView.FocusedRowHandle < 0) return null;
            return this.gridView.GetRow(this.gridView.FocusedRowHandle) as EntityDisplayClientRpt;
        }
        #endregion

        #region ReadImageFile
        /// <summary>
        /// 
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public static Bitmap ReadImageFile(string picName)
        {
            Bitmap bitmap = null;
            try
            {
                string strFile = Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName + @"\reportPicture\" + picName;
                if (!System.IO.File.Exists(strFile))
                {
                    try
                    {
                        strFile = System.AppDomain.CurrentDomain.BaseDirectory + @"\reportPicture\" + picName;
                        if (!System.IO.File.Exists(strFile))
                        {
                            strFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\reportPicture\" + picName;
                        }
                    }
                    catch
                    {

                        strFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\reportPicture\" + picName;
                    }
                }
                FileStream fileStream = File.OpenRead(strFile);
                Int32 filelength = 0;
                filelength = (int)fileStream.Length;
                Byte[] image = new Byte[filelength];
                fileStream.Read(image, 0, filelength);
                System.Drawing.Image result = System.Drawing.Image.FromStream(fileStream);
                fileStream.Close();
                bitmap = new Bitmap(result);
            }
            catch (Exception ex)
            {
                bitmap = null;
            }
            finally
            {
            }
            return bitmap;
        }
        #endregion

        #endregion

        #region events
        private void frm20301_Load(object sender, EventArgs e)
        {
            using (ProxyHms proxy = new ProxyHms())
            {
                Init();
            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            this.Edit();
        }
        #endregion
    }
}

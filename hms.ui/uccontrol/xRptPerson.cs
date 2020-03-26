using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Windows.Forms;
using Hms.Entity;

namespace Hms.Ui
{
    public partial class xRptPerson : DevExpress.XtraReports.UI.XtraReport
    {
        public xRptPerson(EntityDisplayClientRpt rpt = null)
        {
            InitializeComponent();
            SetDataBind(rpt);
        }
        private void SetDataBind(EntityDisplayClientRpt report)//绑定数据源  
        {
            try
            {
                if (report == null)
                    return;
                pic01.Image = report.image01;
                pic02.Image = report.image02;
                pic03.Image = report.image03;
                pic04.Image = report.image04;
                pic05.Image = report.image05;
                picFx02.Image = report.imageFx;
                pic07.Image = report.image07;
                picGxyTip.Image = report.imageTip;
                picGzssTip.Image = report.imageTip;
                picAzhmzTip.Image = report.imageTip;
                picFaTip.Image = report.imageTip;
                picGaTip.Image = report.imageTip;
                picGxbTip.Image = report.imageTip;
                picQlxaTip.Image = report.imageTip;
                picTnbTip.Image = report.imageTip;
                picWaTip.Image = report.imageTip;
                picXzycTip.Image = report.imageTip;
                picNzzTip.Image = report.imageTip;
                picFpzTip.Image = report.imageTip;

                lblPageHeader.Text = report.reportNo;
                lblClientName.Text = report.clientName;
                lblClientNo.Text = report.clientNo;
                lblCompany.Text = report.company;
                lblReportDate.Text = report.reportDate;
                lblSex.Text = report.sex;
                lblAge.Text = report.age;

                #region  重要指标
                this.picMainIdicate.DataBindings.Add("Image", report.lstMainIndicate, "pic");
                this.cellItem.DataBindings.Add("Text", report.lstMainIndicate, "item");//
                this.cellResult.DataBindings.Add("Text", report.lstMainIndicate, "result");//
                this.cellRange.DataBindings.Add("Text", report.lstMainIndicate, "range");//
                this.cellUnit.DataBindings.Add("Text", report.lstMainIndicate, "unit");//
                this.dtRptMainIndicate.DataSource = report.lstMainIndicate;
                #endregion

                #region 高血压
                this.picGxy.DataBindings.Add("Image", report.lstGxy, "pic");
                this.cellGxyItem.DataBindings.Add("Text", report.lstGxy, "item");//
                this.cellGxyResult.DataBindings.Add("Text", report.lstGxy, "result");//
                this.cellGxyRange.DataBindings.Add("Text", report.lstGxy, "range");//
                this.cellGxyUnit.DataBindings.Add("Text", report.lstGxy, "unit");//
                this.dtRptGxyGroup.DataSource = report.lstGxy;
                #endregion

                #region 高血压风险评估结果
                this.xrChartGxy.DataSource = report.lstEvaluateGxy;
                this.xrChartGxy.Series[0].SetDataMembers("evaluationName", "result");
                #endregion

                #region 就医检查建议
                this.cellPeBseItem1.DataBindings.Add("Text", report.lstAdPeItemBse, "item1");//
                this.cellPeBseItem2.DataBindings.Add("Text", report.lstAdPeItemBse, "item2");//
                this.cellPeBseItem3.DataBindings.Add("Text", report.lstAdPeItemBse, "item3");//
                this.dtRptPeBseGroup.DataSource = report.lstAdPeItemBse;

                this.cellPeSpeialItem1.DataBindings.Add("Text", report.lstAdPeItemSpecial, "item1");//
                this.cellPeSpeialItem2.DataBindings.Add("Text", report.lstAdPeItemSpecial, "item2");//
                this.cellPeSpeialItem3.DataBindings.Add("Text", report.lstAdPeItemSpecial, "item3");//
                this.dtRptPeSpecialGroup.DataSource = report.lstAdPeItemSpecial;

                this.cellAdImportant.DataBindings.Add("Text", report.lstMedAdvices, "important");//
                this.cellAdUnormal.DataBindings.Add("Text", report.lstMedAdvices, "unnormal");//
                this.cellAdMedDate.DataBindings.Add("Text", report.lstMedAdvices, "medDate");//
                this.cellAdRefferDept.DataBindings.Add("Text", report.lstMedAdvices, "refferDept");//
                this.dtRptAdviceGroup.DataSource = report.lstMedAdvices;
                #endregion


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

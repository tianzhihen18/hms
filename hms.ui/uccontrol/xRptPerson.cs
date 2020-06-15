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
        public xRptPerson(EntityClientReport rpt = null)
        {
            InitializeComponent();
            SetDataBind(rpt);
        }
        private void SetDataBind(EntityClientReport report)//绑定数据源  
        {
            try
            {
                if (report == null)
                    return;
                //pic01.Image = report.image01;
                pic02.Image = report.image02;
                pic03.Image = report.image03;
                pic04.Image = report.image04;
                pic05.Image = report.image05;
                
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
                #region 健康汇总
                lblTjSumup.Text = string.IsNullOrEmpty(report.tjSumup.Trim()) ? "" : report.tjSumup;
                #endregion

                #region  重要指标
                this.picMainIdicate.DataBindings.Add("Image", report.lstMainItem, "pic");
                this.cellItem.DataBindings.Add("Text", report.lstMainItem, "itemName");//
                this.cellResult.DataBindings.Add("Text", report.lstMainItem, "itemValue");//
                this.cellRange.DataBindings.Add("Text", report.lstMainItem, "itemRefrange");//
                this.cellUnit.DataBindings.Add("Text", report.lstMainItem, "itemUnits");//
                this.dtRptMainIndicate.DataSource = report.lstMainItem;
                #endregion

                #region 高血压
                this.picGxy.DataBindings.Add("Image", report.lstGxyModelParam, "pic");
                this.cellGxyItem.DataBindings.Add("Text", report.lstGxyModelParam, "itemName");//
                this.cellGxyResult.DataBindings.Add("Text", report.lstGxyModelParam, "result");//
                this.cellGxyRange.DataBindings.Add("Text", report.lstGxyModelParam, "range");//
                this.cellGxyUnit.DataBindings.Add("Text", report.lstGxyModelParam, "unit");//
                this.dtRptGxyGroup.DataSource = report.lstGxyModelParam;
                this.lblGxyDf.Text = report.gxyDf.ToString("0.00");
                this.lblGxyAbasableDf.Text = report.gxyAbasableDf.ToString("0.00");
                this.lblGxyBestDf.Text = report.gxyBestDf.ToString("0.00");

                this.picGxyFx01.Image = report.imgGxyFx01;
                this.picGxyFx02.Image = report.imgGxyFx02;
                this.picGxyFx03.Image = report.imgGxyFx03;
                this.picGxyFx04.Image = report.imgGxyFx04;

                if (report.gxyDf >5 && report.gxyDf < 18)
                    this.lblGxyTip.Text = "【说明】您在未来5~10年高血压的患病风险为" + this.lblGxyDf.Text + "%，有一定风险，但仍然低于同年龄、同性别人群的平均水平。";
                if(report.gxyDf <= 5 )
                    this.lblGxyTip.Text = "【说明】您在未来5~10年高血压的患病风险为" + this.lblGxyDf.Text + "%，远低于同年龄、同性别人群的平均水平。";
                if(report.gxyDf >= 18 && report.gxyDf < 50)
                    this.lblGxyTip.Text = "【说明】您在未来5~10年高血压的患病风险为" + this.lblGxyDf.Text + "%，高于同年龄、同性别人群的平均水平。";
                if (report.gxyDf >= 50)
                    this.lblGxyTip.Text = "【说明】您在未来5~10年高血压的患病风险为" + this.lblGxyDf.Text + "%，远高于同年龄、同性别人群的平均水平。";

                this.xrChartGxy.DataSource = report.lstEvaluateGxy;
                this.xrChartGxy.Series[0].SetDataMembers("evaluationName", "result");
                this.lblGxyPoint1.Text = string.IsNullOrEmpty(report.gxyPoint1) ? "" : "1、" + report.gxyPoint1;
                this.lblGxyPoint2.Text = string.IsNullOrEmpty(report.gxyPoint2) ? "" : "2、" + report.gxyPoint2;
                this.lblGxyPoint3.Text = string.IsNullOrEmpty(report.gxyPoint3) ? "" : "3、" + report.gxyPoint3;
                this.lblGxyPoint4.Text = string.IsNullOrEmpty(report.gxyPoint4) ? "" : "4、" + report.gxyPoint4;
                this.lblGxyPoint5.Text = string.IsNullOrEmpty(report.gxyPoint5) ? "" : "5、" + report.gxyPoint5;
                this.lblGxyPoint6.Text = string.IsNullOrEmpty(report.gxyPoint6) ? "" : "6、" + report.gxyPoint6;
                this.lblGxyPoint7.Text = string.IsNullOrEmpty(report.gxyPoint7) ? "" : "7、" + report.gxyPoint7;
                this.lblGxyPoint8.Text = string.IsNullOrEmpty(report.gxyPoint8) ? "" : "8、" + report.gxyPoint8;
                #endregion

                #region 糖尿病
                this.picHintTnb.DataBindings.Add("Image", report.lstTnbModelParam, "pic");
                this.cellTnbItem.DataBindings.Add("Text", report.lstTnbModelParam, "itemName");//
                this.cellTnbResult.DataBindings.Add("Text", report.lstTnbModelParam, "result");//
                this.cellTnbRange.DataBindings.Add("Text", report.lstTnbModelParam, "range");//
                this.cellTnbUnit.DataBindings.Add("Text", report.lstTnbModelParam, "unit");//
                this.dtRptTnbGroup.DataSource = report.lstTnbModelParam;
                this.lblTnbDf.Text = report.tnbDf.ToString("0.00");
                this.lblTnbAbasableDf.Text = report.tnbAbasableDf.ToString("0.00");
                this.lblTnbBestDf.Text = report.tnbBestDf.ToString("0.00");

                this.picTnbFx01.Image = report.imgTnbFx01;
                this.picTnbFx02.Image = report.imgTnbFx02;
                this.picTnbFx03.Image = report.imgTnbFx03;
                this.picTnbFx04.Image = report.imgTnbFx04;

                if (report.tnbDf > 5 && report.tnbDf < 18)
                    this.lblTnbTip.Text = "【说明】您在未来5~10年糖尿病的患病风险为" + this.lblTnbDf.Text + "%，有一定风险，但仍然低于同年龄、同性别人群的平均水平。";
                if (report.tnbDf <= 5)
                    this.lblTnbTip.Text = "【说明】您在未来5~10年糖尿病的患病风险为" + this.lblTnbDf.Text + "%，远低于同年龄、同性别人群的平均水平。";
                if (report.tnbDf >= 18 && report.tnbDf < 50)
                    this.lblTnbTip.Text = "【说明】您在未来5~10年糖尿病的患病风险为" + this.lblTnbDf.Text + "%，高于同年龄、同性别人群的平均水平。";
                if (report.tnbDf >= 50)
                    this.lblTnbTip.Text = "【说明】您在未来5~10年糖尿病的患病风险为" + this.lblTnbDf.Text + "%，远高于同年龄、同性别人群的平均水平。";

                //this.xrChartTnb.DataSource = report.lstEvaluateTnb;
                this.xrChartTnb.Series[0].SetDataMembers("evaluationName", "result");
                this.xrChartTnb.Series[0].DataSource = report.lstEvaluateTnb[0];
                this.xrChartTnb.Series[1].SetDataMembers("evaluationName", "result");
                this.xrChartTnb.Series[1].DataSource = report.lstEvaluateTnb[1];
                this.xrChartTnb.Series[2].SetDataMembers("evaluationName", "result");
                this.xrChartTnb.Series[2].DataSource = report.lstEvaluateTnb[2];

                this.lblTnbPoint1.Text = string.IsNullOrEmpty(report.tnbPoint1) ? "" : "1、" + report.tnbPoint1;
                this.lblTnbPoint2.Text = string.IsNullOrEmpty(report.tnbPoint2) ? "" : "2、" + report.tnbPoint2;
                this.lblTnbPoint3.Text = string.IsNullOrEmpty(report.tnbPoint3) ? "" : "3、" + report.tnbPoint3;
                this.lblTnbPoint4.Text = string.IsNullOrEmpty(report.tnbPoint4) ? "" : "4、" + report.tnbPoint4;
                this.lblTnbPoint5.Text = string.IsNullOrEmpty(report.tnbPoint5) ? "" : "5、" + report.tnbPoint5;
                this.lblTnbPoint6.Text = string.IsNullOrEmpty(report.tnbPoint6) ? "" : "6、" + report.tnbPoint6;
                this.lblTnbPoint7.Text = string.IsNullOrEmpty(report.tnbPoint7) ? "" : "7、" + report.tnbPoint7;
                this.lblTnbPoint8.Text = string.IsNullOrEmpty(report.tnbPoint8) ? "" : "8、" + report.tnbPoint8;
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

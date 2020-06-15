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
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.Data;
using weCare.Core.Utils;
using System.Xml;
using System.Linq;

namespace Hms.Ui
{
    public partial class frm20301 : frmBaseMdi
    {
        public frm20301()
        {
            InitializeComponent();
        }


        #region var/property
        //疾病模型预防要求
        List<EntityModelAnalysisPoint> lstModelPoint { get; set; }
        //疾病模型主要评估参数
        List<EntityModelParam> lstModelParam { get; set; }
        //疾病模型
        List<EntityModelAccess> lstModelAccess { get; set; }
        List<EntityDisplayClientRpt> lstClientInfo { get; set; }
        //体检小结信息
        List<EntityTjResult> lstXjResult;
        //体检结果
        List<EntityTjResult> lstTjResult;
        //体检结论建议
        EntityTjjljy tjjljyVo;
        #endregion

        #region  override
        /// <summary>
        /// 
        /// </summary>
        public override void Search()
        {
            string search = this.txtSearch.Text;
            List<EntityParm> dicParm = new List<EntityParm>();
            string beginDate = this.dteBegin.Text.Replace('-', '.') + " 00:00:00";
            string endDate = this.dteEnd.Text.Replace('-', '.') + " 23:59:59";
            if (beginDate != string.Empty && endDate != string.Empty)
            {
                dicParm.Add(Function.GetParm("reportDate", beginDate + "|" + endDate));
            }
            if (!string.IsNullOrEmpty(search))
            {
                dicParm.Add(Function.GetParm("search", search));
            }
            using (ProxyHms proxy = new ProxyHms())
            {
                this.gridControl.DataSource = proxy.Service.GetClientReports(dicParm);
            }

            this.gridControl.RefreshDataSource();
        }

        #region 生成个人报告
        public override void Edit()
        {
            EntityDisplayClientRpt disClientRpt = GetRowObject();
            EntityClientReport rpt = new EntityClientReport();
            rpt.clientName = disClientRpt.clientName;
            rpt.clientNo = disClientRpt.clientNo;
            rpt.reportDate = disClientRpt.reportDate;
            rpt.reportNo = disClientRpt.reportNo;
            rpt.sex = disClientRpt.sex;
            rpt.image01 = ReadImageFile("pic01.png");
            rpt.image02 = ReadImageFile("pic02.jpg");
            rpt.image03 = ReadImageFile("pic03.png");
            rpt.image04 = ReadImageFile("pic04.png");
            rpt.image05 = ReadImageFile("pic05.png");
            rpt.imageTip = ReadImageFile("picTip.png");
            rpt.image07 = ReadImageFile("pic07.png");

            #region 健康汇总及重要指标
            rpt.lstMainItem = GetMainIndicate();
            if (tjjljyVo != null)
                rpt.tjSumup = tjjljyVo.sumup;
            #endregion

            List<int> lstPoint = null;

            #region 高血压
            rpt.lstGxyModelParam = GetModelParam(1);
            //预防要点
            lstPoint = new List<int>();
            if (rpt.lstGxyModelParam !=null)
            {
                foreach(var pVo in rpt.lstGxyModelParam)
                {
                    EntityEvaluateParams vo = rpt.lstGxyModelParam.Find(r=>r.paramNo == pVo.paramNo);
                    if(vo != null && !lstPoint.Contains(vo.pointId))
                    {
                        lstPoint.Add(vo.pointId);
                    }
                }

                for(int i =0;i<lstPoint.Count;i++)
                {
                    if (i == 0)
                        rpt.gxyPoint1 = lstModelPoint.Find(r=>r.id == lstPoint[0]).pintAdvice;
                    if (i == 1)
                        rpt.gxyPoint2 = lstModelPoint.Find(r => r.id == lstPoint[1]).pintAdvice;
                    if (i == 2)
                        rpt.gxyPoint3 = lstModelPoint.Find(r => r.id == lstPoint[2]).pintAdvice;
                    if (i == 3)
                        rpt.gxyPoint4 = lstModelPoint.Find(r => r.id == lstPoint[3]).pintAdvice;
                    if (i == 4)
                        rpt.gxyPoint5 = lstModelPoint.Find(r => r.id == lstPoint[4]).pintAdvice;
                    if (i == 5)
                        rpt.gxyPoint6 = lstModelPoint.Find(r => r.id == lstPoint[5]).pintAdvice;
                    if (i == 6)
                        rpt.gxyPoint7 = lstModelPoint.Find(r => r.id == lstPoint[6]).pintAdvice;
                    if (i == 7)
                        rpt.gxyPoint8 = lstModelPoint.Find(r => r.id == lstPoint[7]).pintAdvice;
                }
            }
            
            decimal gxyBestDf = 0;
            rpt.gxyDf = CalcModelResult(1,out gxyBestDf);
            rpt.gxyBestDf = gxyBestDf;
            rpt.gxyAbasableDf = rpt.gxyDf - rpt.gxyBestDf;
            if (rpt.gxyDf <= 5)
                rpt.imgGxyFx01 = ReadImageFile("picFx.png");
            else if(rpt.gxyDf > 5 && rpt.gxyDf < 20)
                rpt.imgGxyFx02 = ReadImageFile("picFx.png");
            else if (rpt.gxyDf > 20 && rpt.gxyDf < 50)
                rpt.imgGxyFx03 = ReadImageFile("picFx.png");
            else if (rpt.gxyDf >= 50)
                rpt.imgGxyFx04 = ReadImageFile("picFx.png");

            rpt.lstEvaluateGxy = new List<EntityEvaluateResult>();
            EntityEvaluateResult voEr = new EntityEvaluateResult();
            voEr.result = Function.Double(rpt.gxyDf.ToString("0.00")) ;
            voEr.evaluationName = "本次结果";
            rpt.lstEvaluateGxy.Add(voEr);
            EntityEvaluateResult voEb = new EntityEvaluateResult();
            voEb.result = Function.Double(rpt.gxyBestDf.ToString("0.00"));
            voEb.evaluationName = "最佳状态";
            rpt.lstEvaluateGxy.Add(voEb);
            EntityEvaluateResult voEa = new EntityEvaluateResult();
            voEa.result = 18;
            voEa.evaluationName = "平均水平";
            rpt.lstEvaluateGxy.Add(voEa);
            #endregion

            #region  糖尿病
            rpt.lstTnbModelParam = GetModelParam(2);
            //预防要点
            lstPoint = new List<int>();
            if (rpt.lstTnbModelParam != null)
            {
                foreach (var pVo in rpt.lstTnbModelParam)
                {
                    EntityEvaluateParams vo = rpt.lstTnbModelParam.Find(r => r.paramNo == pVo.paramNo);
                    if (vo != null && !lstPoint.Contains(vo.pointId))
                    {
                        lstPoint.Add(vo.pointId);
                    }
                }

                for (int i = 0; i < lstPoint.Count; i++)
                {
                    if (i == 0)
                        rpt.tnbPoint1 = lstModelPoint.Find(r => r.id == lstPoint[0]).pintAdvice;
                    if (i == 1)
                        rpt.tnbPoint2 = lstModelPoint.Find(r => r.id == lstPoint[1]).pintAdvice;
                    if (i == 2)
                        rpt.tnbPoint3 = lstModelPoint.Find(r => r.id == lstPoint[2]).pintAdvice;
                    if (i == 3)
                        rpt.tnbPoint4 = lstModelPoint.Find(r => r.id == lstPoint[3]).pintAdvice;
                    if (i == 4)
                        rpt.tnbPoint5 = lstModelPoint.Find(r => r.id == lstPoint[4]).pintAdvice;
                    if (i == 5)
                        rpt.tnbPoint6 = lstModelPoint.Find(r => r.id == lstPoint[5]).pintAdvice;
                    if (i == 6)
                        rpt.tnbPoint7 = lstModelPoint.Find(r => r.id == lstPoint[6]).pintAdvice;
                    if (i == 7)
                        rpt.tnbPoint8 = lstModelPoint.Find(r => r.id == lstPoint[7]).pintAdvice;
                }
            }

            decimal tnbBestDf = 0;
            rpt.tnbDf = CalcModelResult(1, out tnbBestDf);
            rpt.tnbBestDf = tnbBestDf;
            rpt.tnbAbasableDf = rpt.tnbDf - rpt.tnbBestDf;
            if (rpt.tnbDf <= 5)
                rpt.imgTnbFx01 = ReadImageFile("picFx.png");
            else if (rpt.tnbDf > 5 && rpt.tnbDf < 20)
                rpt.imgTnbFx02 = ReadImageFile("picFx.png");
            else if (rpt.tnbDf > 20 && rpt.tnbDf < 50)
                rpt.imgTnbFx03 = ReadImageFile("picFx.png");
            else if (rpt.tnbDf >= 50)
                rpt.imgTnbFx04 = ReadImageFile("picFx.png");

            rpt.lstEvaluateTnb = new List<EntityEvaluateResult>();
            EntityEvaluateResult voTnbEr = new EntityEvaluateResult();
            voTnbEr.result = Function.Double(rpt.tnbDf.ToString("0.00"));
            voTnbEr.evaluationName = "本次结果";
            rpt.lstEvaluateTnb.Add(voTnbEr);
            EntityEvaluateResult voTnbEb = new EntityEvaluateResult();
            voTnbEb.result = Function.Double(rpt.tnbBestDf.ToString("0.00"));
            voTnbEb.evaluationName = "最佳状态";
            rpt.lstEvaluateTnb.Add(voTnbEb);
            EntityEvaluateResult voTnbEa = new EntityEvaluateResult();
            voTnbEa.result = 5;
            voTnbEa.evaluationName = "平均水平";
            rpt.lstEvaluateTnb.Add(voTnbEa);
            #endregion

            #region 冠心病
            rpt.lstGxbModelParam = GetModelParam(3);
            //预防要点
            lstPoint = new List<int>();
            if (rpt.lstGxbModelParam != null)
            {
                foreach (var pVo in rpt.lstGxbModelParam)
                {
                    EntityEvaluateParams vo = rpt.lstGxbModelParam.Find(r => r.paramNo == pVo.paramNo);
                    if (vo != null && !lstPoint.Contains(vo.pointId))
                    {
                        lstPoint.Add(vo.pointId);
                    }
                }

                for (int i = 0; i < lstPoint.Count; i++)
                {
                    if (i == 0)
                        rpt.gxbPoint1 = lstModelPoint.Find(r => r.id == lstPoint[0]).pintAdvice;
                    if (i == 1)
                        rpt.gxbPoint2 = lstModelPoint.Find(r => r.id == lstPoint[1]).pintAdvice;
                    if (i == 2)
                        rpt.gxbPoint3 = lstModelPoint.Find(r => r.id == lstPoint[2]).pintAdvice;
                    if (i == 3)
                        rpt.gxbPoint4 = lstModelPoint.Find(r => r.id == lstPoint[3]).pintAdvice;
                    if (i == 4)
                        rpt.gxbPoint5 = lstModelPoint.Find(r => r.id == lstPoint[4]).pintAdvice;
                    if (i == 5)
                        rpt.gxbPoint6 = lstModelPoint.Find(r => r.id == lstPoint[5]).pintAdvice;
                    if (i == 6)
                        rpt.gxbPoint7 = lstModelPoint.Find(r => r.id == lstPoint[6]).pintAdvice;
                    if (i == 7)
                        rpt.gxbPoint8 = lstModelPoint.Find(r => r.id == lstPoint[7]).pintAdvice;
                }
            }

            decimal gxbBestDf = 0;
            rpt.gxbDf = CalcModelResult(1, out gxbBestDf);
            rpt.gxbBestDf = tnbBestDf;
            rpt.tnbAbasableDf = rpt.tnbDf - rpt.gxbBestDf;
            if (rpt.tnbDf <= 5)
                rpt.imgGxbFx01 = ReadImageFile("picFx.png");
            else if (rpt.gxbDf > 5 && rpt.gxbDf < 20)
                rpt.imgGxbFx02 = ReadImageFile("picFx.png");
            else if (rpt.gxbDf > 20 && rpt.gxbDf < 50)
                rpt.imgGxbFx03 = ReadImageFile("picFx.png");
            else if (rpt.gxbDf >= 50)
                rpt.imgGxbFx04 = ReadImageFile("picFx.png");

            rpt.lstEvaluateGxb = new List<EntityEvaluateResult>();
            EntityEvaluateResult voGxbEr = new EntityEvaluateResult();
            voGxbEr.result = Function.Double(rpt.gxbDf.ToString("0.00"));
            voGxbEr.evaluationName = "本次结果";
            rpt.lstEvaluateGxb.Add(voGxbEr);
            EntityEvaluateResult voGxbEb = new EntityEvaluateResult();
            voGxbEb.result = Function.Double(rpt.gxbBestDf.ToString("0.00"));
            voGxbEb.evaluationName = "最佳状态";
            rpt.lstEvaluateGxb.Add(voGxbEb);
            EntityEvaluateResult voGxbEa = new EntityEvaluateResult();
            voGxbEa.result = 5;
            voGxbEa.evaluationName = "平均水平";
            rpt.lstEvaluateGxb.Add(voGxbEa);
            #endregion

            frmPopup2030101 frm = new frmPopup2030101(rpt);
            frm.ShowDialog();

        }
        #endregion

        #region 问卷
        /// <summary>
        /// 问卷
        /// </summary>

        public override void Remind()
        {
            List<EntityQnRecord> dataQn = null;
            EntityDisplayClientRpt vo = GetRowObject();
            if (vo != null)
            {
                List<EntityParm> lstParms = new List<EntityParm>();
                EntityParm parm = new EntityParm();
                parm.key = "clientNo";
                parm.value = vo.clientNo;
                lstParms.Add(parm);
                using (ProxyHms proxy = new ProxyHms())
                {
                    dataQn = proxy.Service.GetQnRecords(lstParms);
                }

                frmPopup2030102 frm = new frmPopup2030102(dataQn);
                frm.ShowDialog();

                if (frm.isSelect)
                {
                    vo.strQnDate = frm.qnRecord.strQnDate;
                    vo.qnRecord = frm.qnRecord;
                }
            }
        }
        #endregion

        #endregion

        #region methods

        #region Init
        internal void Init()
        {
            try
            {
                uiHelper.BeginLoading(this);
                this.dteBegin.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                this.dteEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");

                using (ProxyHms proxy = new ProxyHms())
                {
                    lstModelParam = proxy.Service.GetModelParam();
                    lstModelPoint = proxy.Service.GetModelAnalysisPoint();
                    lstModelAccess = proxy.Service.GetModelAccess();
                }

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
            List<EntityParm> dicParm = new List<EntityParm>();
            string beginDate = this.dteBegin.Text + " 00:00:00";
            string endDate = this.dteEnd.Text + " 23:59:59";
            if (beginDate != string.Empty && endDate != string.Empty)
            {
                dicParm.Add(Function.GetParm("reportDate", beginDate + "|" + endDate));
            }
            using (ProxyHms proxy = new ProxyHms())
            {
                lstClientInfo = proxy.Service.GetClientReports(dicParm);
            }
        }
        #endregion

        #region 重要指标
        /// <summary>
        /// 重要指标
        /// </summary>
        /// <returns></returns>
        internal List<EntityReportMainItem> GetMainIndicate()
        {
            List<EntityReportMainItem> data = new List<EntityReportMainItem>();
            List<EntityReportMainItemConfig> lstMainItemConfig;
            EntityDisplayClientRpt vo = GetRowObject();
            using (ProxyHms proxy = new ProxyHms())
            {
                proxy.Service.GetTjResult(vo.reportNo, out lstTjResult, out lstXjResult, out tjjljyVo);
                lstMainItemConfig = proxy.Service.GetReportMainItemConfig();
            }
            if (lstTjResult == null)
                return null;
            EntityReportMainItem mainItem;
            foreach (var mConfig in lstMainItemConfig)
            {
                EntityTjResult result = lstTjResult.Find(r => r.itemCode == mConfig.itemCode);
                if (result != null)
                {
                    mainItem = new EntityReportMainItem();
                    mainItem.reportId = result.regNo;
                    mainItem.sectionName = result.itemName;
                    mainItem.itemName = result.itemName;
                    mainItem.itemValue = result.itemResult;
                    mainItem.itemUnits = result.unit;
                    mainItem.itemRefrange = result.range;
                    mainItem.isNormal = result.hint;
                    if (!string.IsNullOrEmpty(result.hint))
                        mainItem.pic = ReadImageFile("picHint.png");
                    if (result.ttop == "2" && !string.IsNullOrEmpty(result.examinationNo))
                    {
                        EntityTjResult resultTmp = lstTjResult.Find(r => r.itemCode == result.examinationNo);
                        mainItem.sectionName = resultTmp.itemName;
                    }
                    data.Add(mainItem);
                }
            }

            return data;
        }
        #endregion

        #region 疾病模型主要评估参数
        /// <summary>
        /// 疾病模型主要评估参数
        /// </summary>
        /// <returns></returns>
        internal List<EntityEvaluateParams> GetModelParam(int modelId)
        {
            List<EntityEvaluateParams> data = new List<EntityEvaluateParams>();
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            EntityEvaluateParams param = null;
            List<EntityModelParam> lstModelParamGxy = lstModelParam.FindAll(r => r.modelId == modelId && r.isMain == "1");
            EntityDisplayClientRpt vo = GetRowObject();
            if (lstModelParamGxy != null )
            {
                foreach (var model in lstModelParamGxy)
                {
                    if (model.paramNo.Contains("F"))        //问卷
                    {
                        if (!string.IsNullOrEmpty(vo.qnRecord.xmlData))
                        {
                            XmlDocument document = new XmlDocument();
                            document.LoadXml(vo.qnRecord.xmlData);
                            XmlNodeList list = document["FormData"].ChildNodes;
                            dicData = Function.ReadXML(vo.qnRecord.xmlData);
                            if (dicData.ContainsKey(model.paramNo))
                            {
                                string parentFieldId = model.parentFieldId;
                                param = data.Find(r => r.itemCode == parentFieldId);
                                if (param == null)
                                {
                                    param = new EntityEvaluateParams();
                                    param.paramNo = model.paramNo;
                                    param.itemCode = model.parentFieldId;
                                    param.itemName = model.paramName;
                                    param.range = model.normalRange;
                                    param.judeValue = model.judgeRange;
                                    
                                    param.pointId = model.pointId;
                                    if (model.judgeType == 2)
                                    {
                                        if (Function.Dec(dicData[model.paramNo]) == model.judgeValue)
                                            param.result = model.judgeRange;
                                        else
                                            param.result = "未填";
                                    }
                                    else
                                    {
                                        param.result = dicData[model.paramNo];
                                    }

                                    data.Add(param);
                                }
                                else
                                {
                                    if (model.judgeType == 2)
                                    {
                                        if (Function.Dec(dicData[model.paramNo]) == model.judgeValue)
                                            param.result = model.judgeRange;
                                    }
                                    else
                                    {
                                        param.result = dicData[model.paramNo];
                                    }
                                }
                                if (param.result != param.range && param.result != "未填")
                                    param.pic = ReadImageFile("picHint.png");
                                else
                                    param.pic = null;
                            }
                        }
                    }
                    else                            //体检报告
                    {
                        if (lstTjResult == null)
                            continue;
                        param = new EntityEvaluateParams();
                        param.itemCode = model.paramNo;
                        if (data.Any(r => r.itemCode.Contains(model.paramNo)))
                            continue;
                        if (!lstTjResult.Any(r => r.itemCode == param.itemCode))
                            continue;
                        param.pointId = model.pointId;
                        param.itemName = lstTjResult.Find(r => r.itemCode == param.itemCode).itemName;
                        param.result = lstTjResult.Find(r => r.itemCode == param.itemCode).itemResult;
                        param.range = lstTjResult.Find(r => r.itemCode == param.itemCode).range;
                        data.Add(param);
                    }
                }
            }

            return data;
        }
        #endregion

        #region 计算疾病风险评估得分
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        internal decimal  CalcModelResult(int modelId,out decimal bestDf)
        {
            decimal result = 0;
            bestDf = 0;
            bool ageFlag = false;
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            EntityDisplayClientRpt vo = GetRowObject();
            List<EntityModelParam> lstModelParamGxy = lstModelParam.FindAll(r => r.modelId == modelId);
            
            if (lstModelParamGxy != null)
            {
                foreach (var model in lstModelParamGxy)
                {
                    if (model.paramNo.Contains("F") || model.paramNo == "Age") //问卷
                    {
                        if (!string.IsNullOrEmpty(vo.qnRecord.xmlData))
                        {
                            XmlDocument document = new XmlDocument();
                            document.LoadXml(vo.qnRecord.xmlData);
                            XmlNodeList list = document["FormData"].ChildNodes;
                            dicData = Function.ReadXML(vo.qnRecord.xmlData);

                            //评估得分
                            if (dicData.ContainsKey(model.paramNo))
                            {
                                decimal score = 0;
                                if (dicData[model.paramNo] == "1")
                                {
                                    score += Function.Dec(model.score);
                                }

                                result += score;
                            }
                            if(model.paramNo == "Age")
                            {
                                decimal df = 0;
                                decimal bDf = 0;
                                decimal age = 0;
                                if(dicData.ContainsKey("Birthday") && !ageFlag)
                                {
                                    if(!string.IsNullOrEmpty(dicData["Birthday"]))
                                    {
                                        age = CalcAge.GetAgeInt(Function.Datetime(dicData["Birthday"]));
                                        df = CalcDf(age, model.paramNo, out bDf);
                                        result += df;
                                        ageFlag = true;
                                    }
                                }
                            }

                            //最佳状态 得分
                            string parentFieldId = model.parentFieldId;
                            EntityModelParam modelBest = lstModelParamGxy.FindAll(r => r.parentFieldId == parentFieldId && r.isBest == "1").FirstOrDefault();
                            if(modelBest != null)
                            {
                                bestDf += Function.Dec(modelBest.score);
                            }
                        }
                    }
                    else                            //体检报告
                    {
                        if (lstTjResult == null)
                            continue;
                        EntityTjResult tjVo = lstTjResult.Find(r => r.itemCode == model.paramNo);
                        if (tjVo == null)
                            continue;
                        decimal tjValue = Function.Dec(tjVo.itemResult);
                        decimal df = 0;
                        decimal bDf = 0;
                        df = CalcDf(tjValue, model.paramNo, out bDf);
                        result += df;
                        bestDf += bDf;
                    }
                }
            }
            
            return result;
        }
        #endregion

        #region  体检 、年龄 计算分值
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramNo"></param>
        /// <param name="bestDf"></param>
        /// <returns></returns>
        internal decimal CalcDf(decimal value ,string paramNo,out decimal bestDf)
        {
            decimal result = 0;
            bestDf = 0;
            List<EntityModelParam> lstModelTmp = lstModelParam.FindAll(r => r.paramNo == paramNo);
            if (lstModelTmp != null)
            {
                foreach (var mVo in lstModelTmp)
                {
                    decimal minValue = 0;
                    decimal maxValue = 0;
                    decimal score = 0;

                    if (mVo.judgeRange.Contains("~<"))
                    {
                        minValue = Function.Dec(mVo.judgeRange.Replace("<", "").Trim().Split('~')[0]);
                        maxValue = Function.Dec(mVo.judgeRange.Replace("<", "").Trim().Split('~')[1]);
                        if (value >= minValue && value <= maxValue)
                        {
                            score += (value - mVo.judgeValue) * Function.Dec(mVo.modulus) + mVo.score;
                        }
                    }
                    else if (mVo.judgeRange.Contains("≥"))
                    {
                        maxValue = Function.Dec(mVo.judgeRange.Replace("≥", "").Trim());
                        if (value >= maxValue)
                        {
                            score += (value - mVo.judgeValue) * Function.Dec(mVo.modulus) + mVo.score;
                        }
                    }
                    else if (mVo.judgeRange.Contains("<"))
                    {
                        minValue = Function.Dec(mVo.judgeRange.Replace("<", "").Trim());
                        if (value < minValue)
                        {
                            score += (value - mVo.judgeValue) * Function.Dec(mVo.modulus) + mVo.score;
                        }
                    }
                    else if (mVo.judgeRange.Contains("≤"))
                    {
                        minValue = Function.Dec(mVo.judgeRange.Replace("≤", "").Trim());
                        if (value < minValue)
                        {
                            score += (value - mVo.judgeValue) * Function.Dec(mVo.modulus) + mVo.score;
                        }
                    }
                    result += score;

                    //最佳状态 得分
                    if (mVo.isBest == "1")
                    {
                        bestDf += (value - mVo.judgeValue) * Function.Dec(mVo.modulus) + mVo.score;
                    }
                }
            }

            return result;
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

﻿using Common.Controls;
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
        //疾病模型参数
        List<EntityModelGroupItem> lstModelGroupItem { get; set; }
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

        #region Search
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
        #endregion

        #region 生成个人报告
        public override void Edit()
        {
            try
            {
                this.BeginLoading();
                EntityDisplayClientRpt disClientRpt = GetRowObject();
                List<EntityModelParamCalc> lstMdParamCalc = null;
                if (disClientRpt.qnRecord == null)
                {
                    DialogBox.Msg("请选择问卷");
                    return;
                }

                EntityClientReport rpt = GneralPersonalReport(disClientRpt,out lstMdParamCalc);
                frmPopup2030101 frm = new frmPopup2030101(rpt);
                frm.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.CloseLoading();
            }
            

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

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        public override void Confirm()
        {
            try
            {
                this.BeginLoading();
                EntityDisplayClientRpt disClientRpt = GetRowObject();
                List<EntityModelParamCalc> lstMdParamCalc = null;
                if (disClientRpt.qnRecord == null)
                {
                    DialogBox.Msg("请选择问卷");
                    return;
                }

                EntityClientReport rpt = GneralPersonalReport(disClientRpt,out lstMdParamCalc);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.CloseLoading();
            }
        }
        #endregion

        #region 反审核
        /// <summary>
        /// 反审核
        /// </summary>
        public override void Cancel()
        {
            
        }
        #endregion

        #region 
        /// <summary>
        /// 查看
        /// </summary>
        public override void LoadData()
        {
            
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
                    lstModelGroupItem = proxy.Service.GetModelGroup();
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

        #region 生成报告
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disClientRpt"></param>
        /// <returns></returns>
        internal EntityClientReport GneralPersonalReport(EntityDisplayClientRpt disClientRpt,out List<EntityModelParamCalc> lstMdParamCalcData)
        {
            EntityClientReport rpt = new EntityClientReport();
            //lstMdResult = new List<EntityClientModelResult>();
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

            rpt.lstRptModelAcess = new List<EntityRptModelAcess>();
            lstMdParamCalcData = new List<EntityModelParamCalc>();
            List<EntityModelParamCalc> lstMdParamCalc = new List<EntityModelParamCalc>();
            List<EntityModelAccess> lstMdAcess = GetModelAccess(disClientRpt);

            #region 健康汇总及重要指标
            rpt.lstMainItem = GetMainIndicate();
            if (tjjljyVo != null)
                rpt.tjSumup = tjjljyVo.sumup;
            #endregion

            #region 疾病评估
            if (lstMdAcess != null )
            {
                foreach (var mdAcess in lstMdAcess)
                {
                    rpt.lstRptModelAcess.Add(GetRptModelParam(mdAcess.modelId,out lstMdParamCalc));

                    if(lstMdParamCalc != null && lstMdParamCalc.Count > 0)
                    {
                        lstMdParamCalcData.AddRange(lstMdParamCalc);
                    }
                }
            }
            #endregion

            return rpt;
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

        #region 疾病模型评估
        /// <summary>
        /// 疾病模型评估
        /// </summary>
        /// <returns></returns>
        internal EntityRptModelAcess GetRptModelParam(decimal modelId,out List<EntityModelParamCalc> lstMdParamCalc)
        {
            EntityRptModelAcess modelAcess = new EntityRptModelAcess();
            lstMdParamCalc = new List<EntityModelParamCalc>();
            modelAcess.lstModelParam = new List<EntityRptModelParam>();
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            EntityRptModelParam param = null;
            try
            {
                modelAcess.modelId = modelId;
                List<EntityModelGroupItem> lstGxyModel = lstModelGroupItem.FindAll(r => r.modelId == modelId && r.isMain == 1);
                EntityDisplayClientRpt vo = GetRowObject();
                if (!string.IsNullOrEmpty(vo.qnRecord.xmlData))
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(vo.qnRecord.xmlData);
                    XmlNodeList list = document["FormData"].ChildNodes;
                    dicData = Function.ReadXML(vo.qnRecord.xmlData);
                }

                #region 主要评估参数
                if (lstGxyModel != null)
                {
                    bool rFlag = false;
                    foreach (var model in lstGxyModel)
                    {
                        rFlag = false;
                        param = new EntityRptModelParam();
                        param.itemCode = model.paramNo;
                        param.itemName = model.paramName;
                        param.pointId = model.pointId;
                        //问卷
                        if (model.paramType == 3 || model.paramType == 4)
                        {
                            List<EntityModelParam> lstModelParamGxy = lstModelParam.FindAll(r => r.parentFieldId == model.paramNo && r.modelId == modelId);
                            if (lstModelParamGxy != null)
                            {
                                foreach (var gxyModel in lstModelParamGxy)
                                {
                                    rFlag = false;
                                    if (gxyModel.isNormal == "1")
                                        param.range = gxyModel.judgeRange;
                                    if (dicData.ContainsKey(gxyModel.paramNo))
                                    {
                                        if (gxyModel.judgeType == 2)
                                        {
                                            if (Function.Dec(dicData[gxyModel.paramNo]) == gxyModel.judgeValue)
                                                param.result = gxyModel.judgeRange;
                                            if (string.IsNullOrEmpty(param.result))
                                                param.result = "无";
                                        }
                                        else
                                        {
                                            param.result = dicData[model.paramNo];
                                        }

                                        rFlag = true;
                                    }
                                }
                            }
                        }
                        //体检项目
                        else if (model.paramType == 2)
                        {
                            if (lstTjResult == null)
                                continue;
                            if (!lstTjResult.Any(r => r.itemCode == param.itemCode))
                                continue;
                            EntityTjResult tjR = lstTjResult.Find(r => r.itemCode == param.itemCode);
                            param.itemName = tjR.itemName;
                            param.result = tjR.itemResult;
                            param.range = tjR.range;
                            param.unit = tjR.unit;
                            if (!string.IsNullOrEmpty(tjR.hint))
                                param.pic = ReadImageFile("picHint.png");
                            rFlag = true;
                        }
                        if (rFlag)
                            modelAcess.lstModelParam.Add(param);
                    }
                }
                #endregion

                #region 预防要点
                //预防要点
                List<int> lstPoint = new List<int>();
                modelAcess.lstPoint = new List<string>();
                if (modelAcess.lstModelParam != null)
                {
                    foreach (var pVo in modelAcess.lstModelParam)
                    {
                        if (!lstPoint.Contains(pVo.pointId))
                        {
                            lstPoint.Add(pVo.pointId);
                        }
                    }

                    for (int i = 0; i < lstPoint.Count; i++)
                    {
                        EntityModelAnalysisPoint pointVo= lstModelPoint.Find(r => r.id == lstPoint[i]);
                        if (pointVo != null)
                        {
                            modelAcess.lstPoint.Add(pointVo.pintAdvice);
                        } 
                    }
                }
                #endregion

                #region 风险评估
                string resultStr = string.Empty;
                decimal bestDf = 0;
                decimal df = 0;
                lstMdParamCalc = CalcModelResult(modelId, out df, out bestDf);
                modelAcess.df = df;
                modelAcess.bestDf = bestDf;
                modelAcess.reduceDf = modelAcess.df - modelAcess.bestDf;
                if (modelAcess.df <= 5)
                {
                    modelAcess.imgFx01 = ReadImageFile("picFx.png");
                    resultStr = "低危";
                }
                else if (modelAcess.df > 5 && modelAcess.df < 20)
                {
                    modelAcess.imgFx02 = ReadImageFile("picFx.png");
                    resultStr = "中危";
                }
                else if (modelAcess.df > 20 && modelAcess.df < 50)
                {
                    modelAcess.imgFx03 = ReadImageFile("picFx.png");
                    resultStr = "高危";
                }
                else if (modelAcess.df >= 50)
                {
                    modelAcess.imgFx04 = ReadImageFile("picFx.png");
                    resultStr = "很高危";
                }

                modelAcess.lstEvaluate = new List<EntityEvaluateResult>();
                EntityEvaluateResult voEr = new EntityEvaluateResult();
                voEr.result = Function.Double(modelAcess.df.ToString("0.00"));
                voEr.evaluationName = "本次结果";
                modelAcess.lstEvaluate.Add(voEr);
                EntityEvaluateResult voEb = new EntityEvaluateResult();
                voEb.result = Function.Double(modelAcess.bestDf.ToString("0.00"));
                voEb.evaluationName = "最佳状态";
                modelAcess.lstEvaluate.Add(voEb);
                EntityEvaluateResult voEa = new EntityEvaluateResult();
                voEa.result = 18;
                voEa.evaluationName = "平均水平";
                modelAcess.lstEvaluate.Add(voEa);

                #endregion
               
            }
            catch(Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }

            return modelAcess;
        }

        #endregion

        #region 计算疾病风险评估得分
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        internal List<EntityModelParamCalc> CalcModelResult(decimal modelId,out decimal result, out decimal bestDf)
        {
            result = 0;
            bestDf = 0;
            bool ageFlag = false;
            List<EntityModelParamCalc> data = null;
            EntityModelParamCalc paramCalcVo = null;
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            List<EntityModelGroupItem> lstGxyModel = lstModelGroupItem.FindAll(r => r.modelId == modelId && r.isMain == 1);
            EntityDisplayClientRpt vo = GetRowObject();
            if (!string.IsNullOrEmpty(vo.qnRecord.xmlData))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(vo.qnRecord.xmlData);
                XmlNodeList list = document["FormData"].ChildNodes;
                dicData = Function.ReadXML(vo.qnRecord.xmlData);
            }

            if (lstGxyModel != null)
            {
                data = new List<EntityModelParamCalc>();
                foreach (var model in lstGxyModel)
                {
                    List<EntityModelParam> lstModelParamGxy = lstModelParam.FindAll(r => r.parentFieldId == model.paramNo && r.modelId == modelId);
                    if (lstModelParamGxy != null)
                    {
                        //问卷
                        if (model.paramType == 3 || model.paramType == 4)
                        {
                            foreach (var modelGxy in lstModelParamGxy)
                            {
                                if (!string.IsNullOrEmpty(vo.qnRecord.xmlData))
                                {
                                    //评估得分
                                    if (dicData.ContainsKey(modelGxy.paramNo))
                                    {
                                        decimal score = 0;
                                        if (dicData[modelGxy.paramNo] == "1")
                                        {
                                            score += Function.Dec(modelGxy.score);
                                        }

                                        paramCalcVo = new EntityModelParamCalc();
                                        paramCalcVo.modelId = modelId;
                                        paramCalcVo.paramNo = model.paramNo;
                                        paramCalcVo.paramName = model.paramName;
                                        paramCalcVo.calcScore = score;
                                        paramCalcVo.paramValue = dicData[modelGxy.paramNo];
                                        data.Add(paramCalcVo);

                                        result += score;
                                    }
                                    if (modelGxy.paramNo == "Age")
                                    {
                                        decimal df = 0;
                                        decimal bDf = 0;
                                        decimal age = 0;
                                        if (dicData.ContainsKey("Birthday") && !ageFlag)
                                        {
                                            if (!string.IsNullOrEmpty(dicData["Birthday"]))
                                            {
                                                age = CalcAge.GetAgeInt(Function.Datetime(dicData["Birthday"]));
                                                df = CalcDf(age, modelId, modelGxy.paramNo, out bDf);
                                                result += df;
                                                ageFlag = true;
                                                paramCalcVo = new EntityModelParamCalc();
                                                paramCalcVo.modelId = modelId;
                                                paramCalcVo.paramNo = model.paramNo;
                                                paramCalcVo.paramName = model.paramName;
                                                paramCalcVo.calcScore = df;
                                                paramCalcVo.paramValue = dicData["Birthday"];
                                                data.Add(paramCalcVo);
                                            }
                                        }
                                    }
                                    //最佳状态 得分
                                    if (modelGxy.isBest == "1")
                                    {
                                        bestDf += Function.Dec(modelGxy.score);
                                    }
                                }
                            }
                        }
                        else if (model.paramType == 2)
                        {
                            if (lstTjResult == null)
                                continue;
                            EntityTjResult tjVo = lstTjResult.Find(r => r.itemCode == model.paramNo);
                            if (tjVo == null)
                                continue;
                            decimal tjValue = Function.Dec(tjVo.itemResult);
                            decimal df = 0;
                            decimal bDf = 0;
                            df = CalcDf(tjValue, modelId, model.paramNo, out bDf);
                            paramCalcVo = new EntityModelParamCalc();
                            paramCalcVo.modelId = modelId;
                            paramCalcVo.paramNo = model.paramNo;
                            paramCalcVo.paramName = model.paramName;
                            paramCalcVo.calcScore = df;
                            paramCalcVo.paramValue = tjVo.itemResult.ToString();
                            data.Add(paramCalcVo);
                            result += df;
                            bestDf += bDf;
                        }
                    }
                }
            }
            return data;
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
        internal decimal CalcDf(decimal value, decimal modelId, string paramNo, out decimal bestDf)
        {
            decimal result = 0;
            bestDf = 0;
            List<EntityModelParam> lstModelTmp = lstModelParam.FindAll(r => r.paramNo == paramNo && r.modelId == modelId);
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

        #region 根据年龄、性别获取模型
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disClientRpt"></param>
        /// <returns></returns>
        internal List<EntityModelAccess>  GetModelAccess(EntityDisplayClientRpt disClientRpt)
        {
            List<EntityModelAccess> data = new List<EntityModelAccess>();
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(disClientRpt.qnRecord.xmlData))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(disClientRpt.qnRecord.xmlData);
                XmlNodeList list = document["FormData"].ChildNodes;
                dicData = Function.ReadXML(disClientRpt.qnRecord.xmlData);
            }
            int age = 0;
            if(dicData.ContainsKey("Birthday"))
                age = CalcAge.GetAgeInt(Function.Datetime(dicData["Birthday"]));
            string sexStr = dicData["Sex"];
            int sex = 0;
            if (sexStr == "男")
                sex = 1;
            if(sexStr == "女")
                sex = 2;

            if (lstModelAccess !=null)
            {
                List<EntityModelAccess> tmpList = lstModelAccess.FindAll(r => (r.modelSex == sex || r.modelSex == 0));
                foreach (var maVo in tmpList)
                {
                    decimal minAge = maVo.minAge;
                    decimal maxAge = maVo.maxAge;

                    if (age >= minAge && age <= maxAge)
                        data.Add(maVo);
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

using Common.Entity;
using weCare.Core.Dac;
using weCare.Core.Entity;
using weCare.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using System.Transactions;
using Hms.Entity;

namespace Hms.Biz
{
    public class Biz203 : IDisposable
    {
        #region 个人报告

        #region 列表
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityDisplayClientRpt> GetClientReports(List<EntityParm> parms = null)
        {
            List<EntityDisplayClientRpt> data = null;
            List<EntityQnRecord> dataQn = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select clientName,
                           clientNo,
                           gender,
                           age,
                           company,
                           gradeName,
                           reportNo,
	                       regTimes,
	                       idcard,
	                       reportDate
                       from v_tjxx  a where a.clientNo is not null ";

            string strSub = string.Empty;
            List<IDataParameter> lstParm = new List<IDataParameter>();
            if (parms != null)
            {
                foreach (var po in parms)
                {
                    switch (po.key)
                    {
                        case "search":
                            strSub += " and (a.clientName like '%" + po.value + "%' or a.clientNo like '" + po.value + "%' or a.reportNo like '%" + po.value + "%' )";
                            break;
                        case "reportDate":
                            IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = po.value.Split('|')[0];
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = po.value.Split('|')[1];
                            lstParm.Add(parm2);
                            strSub += " and (a.reportDate between ? and ? )";
                            break;
                        default:
                            break;
                    }
                }
            }

            Sql += strSub;
            Sql += " order by reportDate";
            string strClientNo = string.Empty;

            DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityDisplayClientRpt>();
                EntityDisplayClientRpt vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityDisplayClientRpt();
                    vo.clientName = dr["clientName"].ToString();
                    vo.clientNo = dr["clientNo"].ToString();
                    vo.gender = Function.Int(dr["gender"]);
                    vo.reportNo = dr["reportNo"].ToString();
                    vo.reportDate = Function.Datetime(dr["reportDate"]).ToString("yyyy-MM-dd");
                    vo.company = dr["company"].ToString();
                    vo.gradeName = dr["gradeName"].ToString();
                    vo.age = dr["age"].ToString();
                    vo.reportCount = Function.Int(dr["regTimes"]);
                    data.Add(vo);
                }
            }


            return data;
        }
        #endregion

        #region dic

        /// <summary>
        /// 重要指标字典
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityReportMainItemConfig> GetReportMainItemConfig(List<EntityParm> parms = null)
        {
            List<EntityReportMainItemConfig> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select itemCode,
                           itemName,
                           sort
                       from reportMainItemConfig  order by sort ";
            string strClientNo = string.Empty;

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityReportMainItemConfig>();
                EntityReportMainItemConfig vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityReportMainItemConfig();
                    vo.itemCode = dr["itemCode"].ToString();
                    vo.itemName = dr["itemName"].ToString();
                    data.Add(vo);
                }
            }

            return data;
        }

        /// <summary>
        /// modelParam 模型参数
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityModelParam> GetModelParam(List<EntityParm> parms = null)
        {
            List<EntityModelParam> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select * from modelParam  order by parentFieldId asc ";
            string strClientNo = string.Empty;

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityModelParam>();
                EntityModelParam vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityModelParam();
                    vo.id = Function.Int(dr["id"]);
                    vo.modelId = Function.Dec(dr["modelId"]);
                    vo.judgeType = Function.Dec(dr["judgeType"]);
                    vo.paramType = Function.Dec(dr["paramType"]);
                    vo.gender = Function.Dec(dr["gender"]);
                    vo.isChange = Function.Dec(dr["isChange"]);
                    vo.paramNo = dr["paramNo"].ToString();
                    vo.paramName = dr["paramName"].ToString();
                    vo.judgeValue = Function.Dec(dr["judgeValue"]);
                    vo.judgeRange = dr["judgeRange"].ToString();
                    vo.score = Function.Dec(dr["score"]);
                    vo.modulus = Function.Dec(dr["modulus"]);
                    vo.remarks = dr["remarks"].ToString();
                    vo.normalRange = dr["normalRange"].ToString();
                    vo.isMain = dr["isMain"].ToString();
                    vo.parentFieldId = dr["parentFieldId"].ToString();
                    vo.isBest = dr["isBest"].ToString();
                    vo.pointId = Function.Int(dr["pointId"]);
                    data.Add(vo);

                }
            }

            return data;
        }

        /// <summary>
        /// 疾病模型分析要点
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityModelAnalysisPoint> GetModelAnalysisPoint(List<EntityParm> parms = null)
        {
            List<EntityModelAnalysisPoint> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select  id,
                        paramType,
                        paramNo,
                        paramName,
                        judgeWay,
                        judgeValue,
                        pintAdvice,
                        remarks,
                        bakField1,
                        bakField2
                        from modelAnalysisPoint  ";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityModelAnalysisPoint>();
                EntityModelAnalysisPoint vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityModelAnalysisPoint();
                    vo.id = Function.Int(dr["id"]);
                    vo.paramType = Function.Dec(dr["paramType"]);
                    vo.paramNo = dr["paramType"].ToString();
                    vo.paramName = dr["paramNo"].ToString();
                    vo.judgeWay = dr["judgeWay"].ToString();
                    vo.judgeValue = dr["judgeValue"].ToString();
                    vo.pintAdvice = dr["pintAdvice"].ToString();
                    vo.remarks = dr["remarks"].ToString();
                    vo.bakField1 = dr["bakField1"].ToString();
                    vo.bakField2 = dr["bakField2"].ToString();
                    data.Add(vo);
                }
            }

            return data;
        }

        /// <summary>
        /// 疾病模型
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityModelAccess> GetModelAccess(List<EntityParm> parms = null)
        {
            List<EntityModelAccess> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string sql = @"select modelId,
                                  modelName ,
                                  modelIntro ,
                                  modelExplan ,
                                  modelAdvice ,
                                  lowDanger ,
                                  minAge ,
                                  maxAge ,
                                  modelSex ,
                                  isNeedQuestion from modelAccess ";
            DataTable dt = svc.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityModelAccess>();
                EntityModelAccess vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityModelAccess();
                    vo.modelId = Function.Int(dr["modelId"]);
                    vo.modelName = dr["modelName"].ToString();
                    vo.modelIntro = dr["modelIntro"].ToString();
                    vo.modelExplan = dr["modelExplan"].ToString();
                    vo.modelAdvice = dr["modelAdvice"].ToString();
                    vo.lowDanger = Function.Dec(dr["lowDanger"]) ;
                    vo.minAge = Function.Dec(dr["minAge"]);
                    vo.maxAge = Function.Dec(dr["maxAge"]);
                    vo.modelSex = Function.Dec(dr["modelSex"]);
                    data.Add(vo);
                }
            }

            return data;
        }

        #endregion

        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

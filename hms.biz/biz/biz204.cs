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
    public class Biz204 : IDisposable
    {
        #region 模板
        /// <summary>
        /// 模板
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityPromotionTemplate> GetPromotionTemplates(List<EntityParm> parms = null)
        {
            List<EntityPromotionTemplate> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select  id,
                            templateName,
                            templateCondition,
                            isEnabled,
                            bakField1,
                            bakField2,
                            createDate,
                            cretateId,
                            creator,
                            modifyDate,
                            modifyId,
                            modifyName
                      from promotionTemplate  ";
            DataTable dt = svc.GetDataTable(Sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityPromotionTemplate>();
                EntityPromotionTemplate vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityPromotionTemplate();
                    vo.id = dr["id"].ToString();
                    vo.templateName = dr["templateName"].ToString();
                    vo.templateCondition = dr["templateCondition"].ToString();
                    vo.isEnabled = dr["isEnabled"].ToString();
                    vo.bakField1 = dr["bakField1"].ToString();
                    vo.bakField2 = dr["bakField2"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 模板配置
        /// <summary>
        /// 模板配置
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityPromotionTemplateConfig> GetPromotionTemplateConfigs(List<EntityParm> parms = null)
        {
            List<EntityPromotionTemplateConfig> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select  a.id,
                            templateId,
                            planMonth,
                            planDay,
                            b.planWay,
                            c.planContent,
                            planRemind,
                            isEnabled,
                            bakField1,
                            bakField2,
                            createDate,
                            createId,
                            creator,
                            modifyDate,
                            modifyId,
                            modifyName
                      from promotionTemplateConfig a
                            left join promotionWayConfig b
                            on a.planWay = b.id
                            left join promotionContentConfig c
                            on a.planContent = c.id  order by planMonth,planDay";
            DataTable dt = svc.GetDataTable(Sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityPromotionTemplateConfig>();
                EntityPromotionTemplateConfig vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityPromotionTemplateConfig();
                    vo.id = dr["id"].ToString();
                    vo.templateId = dr["templateId"].ToString();
                    vo.planMonth = dr["planMonth"].ToString();
                    vo.planPeriod = (DateTime.Now.AddMonths(Function.Int(dr["planMonth"])-1).AddDays(Function.Int(dr["planDay"]))).ToString("yyyy-MM-dd"); ;
                    vo.planDay = dr["planDay"].ToString();
                    vo.planWay = dr["planWay"].ToString();
                    vo.planContent = dr["planContent"].ToString();
                    vo.planRemind = dr["planRemind"].ToString();
                    vo.isEnabled = dr["isEnabled"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region  干预形式
        /// <summary>
        /// 干预形式
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityPromotionWayConfig> GetPromotionWayConfigs()
        {
            List<EntityPromotionWayConfig> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select  id,
                            planWay
                      from promotionWayConfig  ";
            DataTable dt = svc.GetDataTable(Sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityPromotionWayConfig>();
                EntityPromotionWayConfig vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityPromotionWayConfig();
                    vo.id = dr["id"].ToString();
                    vo.planWay = dr["planWay"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region  主要内容
        /// <summary>
        /// 主要内容
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityPromotionContentConfig> GetPromotionContentConfigs()
        {
            List<EntityPromotionContentConfig> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select  id,
                            planContent
                      from promotionContentConfig  ";
            DataTable dt = svc.GetDataTable(Sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityPromotionContentConfig>();
                EntityPromotionContentConfig vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityPromotionContentConfig();
                    vo.id = dr["id"].ToString();
                    vo.planContent = dr["planContent"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region  待执行计划
        /// <summary>
        /// 待执行计划
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityDisplayPromotionPlan> GetPromotionPlans()
        {
            List<EntityDisplayPromotionPlan> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select b.clientName,
                            b.clientNo,
                            b.gender,
                            b.birthday,
                            b.company,
                            b.mobile,
                            c.gradeName,
                            d.planWay,
                            e.planContent,
                            a.planRemind,
                            a.planDate,
                            a.executeTime,
                            a.createName
                            from promotionPlan a
                            left join clientInfo b
                            on a.clientId = b.id
                            left join userGrade c
                            on b.gradeId = c.id
                            left join promotionWayConfig d
                            on a.planWay = d.id
                            left join promotionContentConfig e
                            on a.planContent = e.id  order by b.clientNo,a.planDate";
            DataTable dt = svc.GetDataTable(Sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityDisplayPromotionPlan>();
                EntityDisplayPromotionPlan vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityDisplayPromotionPlan();
                    vo.clientName = dr["clientName"].ToString();
                    vo.clientNo = dr["clientNo"].ToString();
                    vo.gender = Function.Int(dr["gender"]);
                    vo.age = Function.CalcAge(Function.Datetime(dr["birthday"]));
                    vo.company = dr["company"].ToString();
                    vo.mobile = dr["mobile"].ToString();
                    vo.gradeName = dr["gradeName"].ToString();
                    vo.planWay = dr["planWay"].ToString();
                    vo.planContent = dr["planContent"].ToString();
                    vo.planRemind = dr["planRemind"].ToString();
                    vo.planDate = Function.Datetime(dr["planDate"]).ToString("yyyy-MM-dd");
                    if(dr["executeTime"] != DBNull.Value)
                        vo.executeTime = Function.Datetime(dr["executeTime"]).ToString("yyyy-MM-dd");
                    vo.createName = dr["createName"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
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

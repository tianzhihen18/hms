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
    public class Biz201 : IDisposable
    {
        #region 我的客户

        #region 客户列表
        /// <summary>
        /// 客户列表
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityClientInfo> GetClientInfos(List<EntityParm> parms)
        {
            List<EntityClientInfo> data = null;
            List<EntityReportRecorde> lstReportRecord = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select  a.id,
                            a.gradeId,
                            b.gradeName,
                            a.clientNo,
                            a.clientName,
                            a.gender,
                            a.birthday,
                            a.mobile,
                            a.telephone,
                            a.email,
                            a.qq,
                            a.cardNo,
                            a.company,
                            a.regionId,
                            a.address,
                            a.booldType,
                            a.profession,
                            a.marriage,
                            a.ehtnicGroup,
                            a.eduationLevel,
                            a.clientTag,
                            a.contactName,
                            a.contactNameMobile,
                            a.clientRemarks,
                            a.dataSource,
                            a.upTag,
                            a.serverDate,
                            a.bakfileld1,
                            a.bakfileld2,
                            a.createDate,
                            a.creatorId,
                            a.createName,
                            a.modifyDate,
                            a.modifyId,
                            a.modifyName
                      from clientInfo a 
                        left join userGrade b
                            on a.gradeId = b.id";

            DataTable dt = svc.GetDataTable(Sql);
            Sql = @"select id,
                            reportName,
                            clientId,
                            reportId,
                            conventionId,
                            psychologcaId,
                            tcmpysiqueId,
                            suditState,
                            reportStatc,
                            needTime,
                            upTag,
                            bakfield1,
                            bakfield2,
                            createDate,
                            creator,
                            createName
                            from reportRecorde ";
            DataTable dtReport = svc.GetDataTable(Sql);
            if (dtReport != null && dtReport.Rows.Count > 0)
            {
                lstReportRecord = new List<EntityReportRecorde>();
                EntityReportRecorde vo = null;
                foreach (DataRow dr in dtReport.Rows)
                {
                    vo = new EntityReportRecorde();
                    vo.id = dr["id"].ToString();
                    vo.reportName = dr["reportName"].ToString();
                    vo.clientId = dr["clientId"].ToString();
                    vo.reportId = dr["reportId"].ToString();
                    vo.conventionId = dr["conventionId"].ToString();
                    vo.psychologcaId = dr["psychologcaId"].ToString();
                    vo.tcmpysiqueId = dr["tcmpysiqueId"].ToString();
                    vo.suditState = dr["suditState"].ToString();
                    vo.reportStatc = dr["reportStatc"].ToString();
                    vo.needTime = dr["needTime"].ToString();
                    vo.upTag = dr["upTag"].ToString();
                    vo.bakfield1 = dr["bakfield1"].ToString();
                    vo.bakfield2 = dr["bakfield2"].ToString();
                    vo.createDate = Function.Datetime(dr["createDate"]);
                    vo.creator = dr["creator"].ToString();
                    vo.createName = dr["createName"].ToString();  
                    lstReportRecord.Add(vo);
                }
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityClientInfo>();
                EntityClientInfo vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityClientInfo();
                    vo.id = dr["id"].ToString();
                    vo.gradeId = dr["gradeId"].ToString();
                    vo.gradeName = dr["gradeName"].ToString();
                    vo.clientNo = dr["clientNo"].ToString();
                    vo.clientName = dr["clientName"].ToString();
                    vo.age = Function.CalcAge(Function.Datetime(dr["birthday"]));
                    vo.gender = Function.Int( dr["gender"]);
                    vo.birthday = Function.Datetime(dr["birthday"]);
                    vo.mobile = dr["mobile"].ToString();
                    vo.telephone = dr["telephone"].ToString();
                    vo.email = dr["email"].ToString();
                    vo.qq = dr["qq"].ToString();
                    vo.company = dr["company"].ToString();
                    vo.regionId = dr["regionId"].ToString();
                    vo.address = dr["address"].ToString();
                    vo.booldType = Function.Int(dr["booldType"]);
                    vo.profession = Function.Int(dr["profession"]);
                    vo.marriage = Function.Int(dr["marriage"]);
                    vo.ehtnicGroup = Function.Int(dr["ehtnicGroup"]);
                    vo.eduationLevel = Function.Int(dr["eduationLevel"]);
                    vo.clientTag = dr["clientTag"].ToString();
                    vo.contactName = dr["contactName"].ToString();
                    vo.contactNameMobile = dr["contactNameMobile"].ToString();
                    vo.clientRemarks = dr["clientRemarks"].ToString();
                    vo.dataSource = dr["dataSource"].ToString();
                    vo.upTag = dr["upTag"].ToString();
                    vo.serverDate = Function.Datetime(dr["serverDate"]);
                    vo.bakfileld1 = dr["bakfileld1"].ToString();
                    vo.bakfileld2 = dr["bakfileld2"].ToString();
                    vo.createDate = Function.Datetime(dr["createDate"]);
                    vo.creatorId = dr["creatorId"].ToString();
                    vo.createName = dr["createName"].ToString();
                    vo.modifyDate = Function.Datetime(dr["modifyDate"]);
                    vo.modifyId = dr["modifyId"].ToString();
                    vo.modifyName = dr["modifyName"].ToString();

                    vo.lstReportRecord = lstReportRecord.FindAll(r=>r.clientId == vo.id);
                    if(vo.lstReportRecord != null)
                    {
                        vo.reportCount = vo.lstReportRecord.Count;
                        vo.conventionCount = vo.lstReportRecord.Count(r => !string.IsNullOrEmpty(r.conventionId));
                    }

                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 类别列表
        /// <summary>
        /// 类别列表
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityUserGrade> GetUserGrades(List<EntityParm> parms)
        {
            List<EntityUserGrade> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select  a.id,
                            a.gradeName,
                            a.reportName,
                            a.severPrice,
                            a.serverTime,
                            a.description,
                            a.isEnable,
                            a.upTag,
                            a.bakfield1,
                            a.bakfield2,
                            a.createDate,
                            a.creator,
                            a.createName,
                            a.modifyDate,
                            a.modifyrId,
                            a.modifyName
                      from userGrade a ";
            DataTable dt = svc.GetDataTable(Sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityUserGrade>();
                EntityUserGrade vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityUserGrade();
                    vo.id = dr["id"].ToString();
                    vo.gradeName = dr["gradeName"].ToString();
                    vo.reportName = dr["reportName"].ToString();
                    vo.severPrice = dr["severPrice"].ToString();
                    vo.serverTime = dr["serverTime"].ToString();
                    vo.description = dr["description"].ToString();
                    vo.isEnable = Function.Int(dr["isEnable"]);
                    vo.upTag = dr["upTag"].ToString();
                    vo.createDate = Function.Datetime(dr["createDate"]);
                    vo.creator = dr["creator"].ToString();
                    vo.createName = dr["createName"].ToString();
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

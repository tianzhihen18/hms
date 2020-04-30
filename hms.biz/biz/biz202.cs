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
    public class Biz202 : IDisposable
    {

        #region  获取表单控件
        /// <summary>
        /// 获取表单控件
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntitySetQnControl> GetQnControl(List<EntityParm> parms = null)
        {
            List<EntitySetQnControl> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.qnId,
                            a.qnName,
                            a.classId,
                            b.fieldId as parentFieldId,
                            c.fieldName as parentFieldName,
                            c.typeId,
                            d.fieldId as childFieldId,
                            d.fieldName as childFieldName
                            from dicQnMain a 
                            left join dicQnDetail b
                            on a.qnId = b.qnId
                            left join dicQnSetting c
                            on b.fieldId = c.fieldId
                            left join dicQnSetting d
                            on b.fieldId = d.parentFieldId
                            where a.qnId = 2 order by c.sortNo,d.sortNo ";
            DataTable dt = svc.GetDataTable(Sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntitySetQnControl>();
                EntitySetQnControl vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntitySetQnControl();
                    vo.qnId = dr["qnId"].ToString();
                    vo.qnName = dr["qnName"].ToString();
                    vo.classId = dr["classId"].ToString();
                    vo.parentFieldId = dr["parentFieldId"].ToString();
                    vo.parentFieldName = dr["parentFieldName"].ToString();
                    vo.typeId = dr["typeId"].ToString();
                    vo.childFieldId = dr["childFieldId"].ToString();
                    vo.childFieldName = dr["childFieldName"].ToString();

                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 获取原始表单控件信息
        /// <summary>
        /// 获取原始表单控件信息
        /// </summary>
        /// <param name="qnCtlFiledId"></param>
        /// <returns></returns>
        public List<EntityCtrlLocation> GetQnCtrlLocation(string qnCtlFiledId)
        {
            if (string.IsNullOrEmpty(qnCtlFiledId))
                return null;
            List<EntityCtrlLocation> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select * from dicQnCtlLocation a where a.qnCtlFiledId = '" + qnCtlFiledId + "'" ;
            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityCtrlLocation>();
                EntityCtrlLocation vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    string xml = dr["xmlData"].ToString();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlNodeList nodelist = doc["eflayout"].GetElementsByTagName("ctrl");
                    if (nodelist != null && nodelist.Count > 0)
                    {
                        foreach (XmlNode node in nodelist)
                        {
                            vo = new EntityCtrlLocation();
                            vo.name = node.Attributes["ctrlname"].Value;
                            vo.text = node.Attributes["ctrlText"].Value;
                            vo.width = Function.Int(node.Attributes["width"].Value);
                            vo.height = Function.Int(node.Attributes["height"].Value);
                            vo.locationX = Function.Int( node.Attributes["locationX"].Value);
                            vo.locationY = Function.Int(node.Attributes["locationY"].Value);
                            string type = node.Attributes["ctrltype"].Value;
                            if (type.Contains("LabelControl"))
                                vo.type = 1;
                            if (type.Contains("CheckEdit"))
                                vo.type = 2;

                            data.Add(vo);
                        }
                    }
                }
            }
            return data.OrderBy(r=>r.name).ToList();
        }
        #endregion

        #region 获取常规问卷记录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityQnRecord> GetQnRecords(List<EntityParm> parms = null)
        {
            List<EntityQnRecord> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.recId,
                           b.clientNo,
                           b.clientName,
                           b.gender,
						   c.gradeName,
                           b.birthday,
                           a.qnName,
						   a.qnType,
                           a.qnDate,
                           a.qnSource,
                           d.oper_name as recorder,
                           q.xmlData
                        from qnRecord a
                     left join qnData q
                        on a.recId = q.recId
                     inner join clientInfo b
                        on a.clientNo = b.clientNo
					 left join userGrade c
						on b.gradeId = c.id
                      left join code_operator d
                        on a.recorder = d.oper_code";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityQnRecord>();
                EntityQnRecord vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityQnRecord();
                    vo.recId = Function.Dec(dr["recId"]) ;
                    vo.gender = Function.Int(dr["gender"]);
                    vo.clientNo = dr["clientNo"].ToString();
                    vo.clientName = dr["clientName"].ToString();
                    vo.gradeName = dr["gradeName"].ToString();
                    vo.age = dr["birthday"] == DBNull.Value ? "" : CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                    vo.qnName = dr["qnName"].ToString();
                    vo.qnSource = Function.Dec(dr["qnSource"]);
                    vo.qnDate = Function.Datetime(dr["qnDate"]);
                    vo.strQnDate = Function.Datetime(dr["qnDate"]).ToString("yyyy-MM-dd");
                    vo.recorder = dr["recorder"].ToString();
                    vo.xmlData = dr["xmlData"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 常规问卷-保存
        /// <summary>
        /// 常规问卷-保存
        /// </summary>
        /// <param name="sfData"></param>
        /// <param name="sfId"></param>
        /// <returns></returns>
        public int SaveQnRecord(EntityQnRecord qnRecord,EntityQnData qnData, out decimal recId)
        {
            int affectRows = 0;
            recId = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                svc = new SqlHelper(EnumBiz.onlineDB);
                if (qnRecord.recId <= 0)
                {
                    recId = svc.GetNextID("qnRecord", "recId");
                    qnRecord.recId = recId;
                    qnRecord.recorder = "00";
                    qnRecord.recordDate = DateTime.Now;
                    qnRecord.status = 0;
                    qnData.recId = recId;
                    
                    lstParm.Add(svc.GetInsertParm(qnRecord));
                    lstParm.Add(svc.GetInsertParm(qnData));
                }
                else
                {
                    recId = qnRecord.recId;
                    lstParm.Add(svc.GetUpdateParm(qnRecord, 
                        new List<string> { EntityQnRecord.Columns.clientNo,
                            EntityQnRecord.Columns.qnType,
                            EntityQnRecord.Columns.qnSource}, 
                        new List<string> { EntityQnRecord.Columns.recId }));
                    lstParm.Add(svc.GetUpdateParm(qnData, 
                        new List<string> { EntityQnData.Columns.xmlData},
                        new List<string> { EntityQnData.Columns.recId }));
                }

                if(lstParm.Count > 0)
                {
                    affectRows = svc.Commit(lstParm);
                }
               
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }
            return affectRows;
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

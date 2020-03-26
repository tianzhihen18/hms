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
    public class Biz205 : IDisposable
    {
        #region 高血压

        #region 人员列表
        /// <summary>
        /// 人员列表
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityHmsSF> GetGxyPatients(List<EntityParm> parms)
        {
            List<EntityHmsSF> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.recId,
                           b.clientNo,
                           b.patName,
                           b.sex,
                           b.birthday,
                           b.classId,
                           a.beginDate,
                           '' as manageLevel,
                           0 as sfTimes,
                           0 as evaTimes,
                           a.nextSfDate,
                           0 as planTimes
                      from gxyRecord a
                     inner join hmsPatient b
                        on a.patId = b.patId";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityHmsSF>();
                EntityHmsSF vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityHmsSF();
                    vo.recId = dr["recId"].ToString();
                    vo.clientNo = dr["clientNo"].ToString();
                    vo.patName = dr["patName"].ToString();
                    vo.sex = dr["sex"].ToString();
                    vo.age = dr["birthday"] == DBNull.Value ? "" : CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                    vo.patClass = dr["classId"].ToString() == "01" ? "普通" : "";
                    vo.manageBeginDate = dr["beginDate"] == DBNull.Value ? "" : Function.Datetime(dr["beginDate"]).ToString("yyyy-MM-dd HH:mm");
                    vo.manageLevel = dr["manageLevel"].ToString();
                    vo.sfTimes = dr["sfTimes"].ToString();
                    vo.evaTimes = dr["evaTimes"].ToString();
                    vo.sfNextDate = dr["nextSfDate"] == DBNull.Value ? "" : Function.Datetime(dr["nextSfDate"]).ToString("yyyy-MM-dd HH:mm");
                    vo.planTimes = dr["planTimes"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 随访记录
        #region 随访记录-获取
        /// <summary>
        /// 随访记录-获取
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityHmsSF> GetGxySfRecords(List<EntityParm> parms)
        {
            List<EntityHmsSF> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.recId,
                           b.clientNo,
                           b.patName,
                           b.sex,
                           b.birthday,
                           b.classId,
                           c.sfId,
                           c.sfDate,
                           c.sfMethod,
                           c.sfClass,
                           d.oper_name as sfRecorder
                      from gxyRecord a
                     inner join hmsPatient b
                        on a.patId = b.patId
                     inner join gxySf c
                        on a.recId = c.recId
                       and c.sfStatus = 1
                      left join code_operator d
                        on c.sfRecorder = d.oper_code";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityHmsSF>();
                EntityHmsSF vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityHmsSF();
                    vo.recId = dr["recId"].ToString();
                    vo.clientNo = dr["clientNo"].ToString();
                    vo.patName = dr["patName"].ToString();
                    vo.sex = dr["sex"].ToString();
                    vo.age = dr["birthday"] == DBNull.Value ? "" : CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                    vo.patClass = dr["classId"].ToString() == "01" ? "普通" : "";
                    vo.sfId = dr["sfId"].ToString();
                    vo.sfDate = dr["sfDate"] == DBNull.Value ? "" : Function.Datetime(dr["sfDate"]).ToString("yyyy-MM-dd HH:mm");
                    vo.sfMethod = (dr["sfMethod"].ToString() == "01" ? "电话随访" : "其他");
                    vo.sfClass = (dr["sfClass"].ToString() == "01" ? "普通" : "其他");
                    vo.sfRecorder = dr["sfRecorder"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion
        #region 随访记录-保存
        /// <summary>
        /// 随访记录-保存
        /// </summary>
        /// <param name="sfData"></param>
        /// <param name="sfId"></param>
        /// <returns></returns>
        public int SaveGxySfRecord(EntityGxySfData sfData, out decimal sfId)
        {
            int affectRows = 0;
            sfId = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                if (sfData.sfId <= 0)
                {
                    Sql = @"select max(t.sfId) as maxId from gxySfData t";
                    DataTable dt = svc.GetDataTable(Sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["maxId"] != DBNull.Value)
                        {
                            sfData.sfId = Convert.ToDecimal(dt.Rows[0]["maxId"]) + 1;
                        }
                    }
                    if (sfData.sfId <= 0)
                        sfData.sfId = 1;
                }
                List<DacParm> lstParm = new List<DacParm>();
                lstParm.Add(svc.GetDelParmByPk(sfData));
                lstParm.Add(svc.GetInsertParm(sfData));
                affectRows = svc.Commit(lstParm);
                sfId = sfData.sfId;
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
        #endregion

        #region 评估记录
        #region 评估记录-获取
        /// <summary>
        /// 评估记录-获取
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityHmsSF> GetGxyPgRecords(List<EntityParm> parms)
        {
            List<EntityHmsSF> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.recId,
                           b.clientNo,
                           b.patName,
                           b.sex,
                           b.birthday,
                           b.classId,
                           c.pgId,
                           c.evaDate,
                           c.bloodPressLevel,
                           c.dangerLevel,
                           c.manageLevel,
                           d.oper_name as evaluator
                      from gxyRecord a
                     inner join hmsPatient b
                        on a.patId = b.patId
                     inner join gxyPg c
                        on a.recId = c.recId
                       and c.Status = 1
                      left join code_operator d
                        on c.evaluator = d.oper_code";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityHmsSF>();
                EntityHmsSF vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityHmsSF();
                    vo.recId = dr["recId"].ToString();
                    vo.clientNo = dr["clientNo"].ToString();
                    vo.patName = dr["patName"].ToString();
                    vo.sex = dr["sex"].ToString();
                    vo.age = dr["birthday"] == DBNull.Value ? "" : CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                    vo.patClass = dr["classId"].ToString() == "01" ? "普通" : "";
                    vo.pgId = dr["pgId"].ToString();
                    vo.evaDate = dr["evaDate"] == DBNull.Value ? "" : Function.Datetime(dr["evaDate"]).ToString("yyyy-MM-dd HH:mm");
                    vo.bloodPressLevel = (dr["bloodPressLevel"].ToString() == "01" ? "I级" : "/");
                    vo.dangerLevel = (dr["dangerLevel"].ToString() == "01" ? "一般" : "/");
                    vo.manageLevel = (dr["manageLevel"].ToString() == "01" ? "I级" : "/");
                    vo.evaluator = dr["evaluator"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion
        #region 评估记录-保存
        /// <summary>
        /// 评估记录-保存
        /// </summary>
        /// <param name="pgData"></param>
        /// <param name="pgId"></param>
        /// <returns></returns>
        public int SaveGxyPgRecord(EntityGxyPgData pgData, out decimal pgId)
        {
            int affectRows = 0;
            pgId = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                if (pgData.pgId <= 0)
                {
                    Sql = @"select max(t.pgId) as maxId from gxyPgData t";
                    DataTable dt = svc.GetDataTable(Sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["maxId"] != DBNull.Value)
                        {
                            pgData.pgId = Convert.ToDecimal(dt.Rows[0]["maxId"]) + 1;
                        }
                    }
                    if (pgData.pgId <= 0)
                        pgData.pgId = 1;
                }
                List<DacParm> lstParm = new List<DacParm>();
                lstParm.Add(svc.GetDelParmByPk(pgData));
                lstParm.Add(svc.GetInsertParm(pgData));
                affectRows = svc.Commit(lstParm);
                pgId = pgData.pgId;
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
        #endregion
        #endregion

        #region 糖尿病
        
        #region 人员列表
        /// <summary>
        /// 人员列表
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityHmsSF> GetTnbPatients(List<EntityParm> parms)
        {
            List<EntityHmsSF> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.recId,
                           b.clientNo,
                           b.patName,
                           b.sex,
                           b.birthday,
                           b.classId,
                           a.beginDate,
                           '' as manageLevel,
                           0 as sfTimes,
                           0 as evaTimes,
                           a.nextSfDate,
                           0 as planTimes
                      from tnbRecord a
                     inner join hmsPatient b
                        on a.patId = b.patId";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityHmsSF>();
                EntityHmsSF vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityHmsSF();
                    vo.recId = dr["recId"].ToString();
                    vo.clientNo = dr["clientNo"].ToString();
                    vo.patName = dr["patName"].ToString();
                    vo.sex = dr["sex"].ToString();
                    vo.age = dr["birthday"] == DBNull.Value ? "" : CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                    vo.patClass = dr["classId"].ToString() == "01" ? "普通" : "";
                    vo.manageBeginDate = dr["beginDate"] == DBNull.Value ? "" : Function.Datetime(dr["beginDate"]).ToString("yyyy-MM-dd HH:mm");
                    vo.manageLevel = dr["manageLevel"].ToString();
                    vo.sfTimes = dr["sfTimes"].ToString();
                    vo.evaTimes = dr["evaTimes"].ToString();
                    vo.sfNextDate = dr["nextSfDate"] == DBNull.Value ? "" : Function.Datetime(dr["nextSfDate"]).ToString("yyyy-MM-dd HH:mm");
                    vo.planTimes = dr["planTimes"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 随访记录
        #region 随访记录-获取
        /// <summary>
        /// 随访记录-获取
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityHmsSF> GetTnbSfRecords(List<EntityParm> parms)
        {
            List<EntityHmsSF> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.recId,
                           b.clientNo,
                           b.patName,
                           b.sex,
                           b.birthday,
                           b.classId,
                           c.sfId,
                           c.sfDate,
                           c.sfMethod,
                           c.sfClass,
                           d.oper_name as sfRecorder
                      from tnbRecord a
                     inner join hmsPatient b
                        on a.patId = b.patId
                     inner join tnbSf c
                        on a.recId = c.recId
                       and c.sfStatus = 1
                      left join code_operator d
                        on c.sfRecorder = d.oper_code";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityHmsSF>();
                EntityHmsSF vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityHmsSF();
                    vo.recId = dr["recId"].ToString();
                    vo.clientNo = dr["clientNo"].ToString();
                    vo.patName = dr["patName"].ToString();
                    vo.sex = dr["sex"].ToString();
                    vo.age = dr["birthday"] == DBNull.Value ? "" : CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                    vo.patClass = dr["classId"].ToString() == "01" ? "普通" : "";
                    vo.sfId = dr["sfId"].ToString();
                    vo.sfDate = dr["sfDate"] == DBNull.Value ? "" : Function.Datetime(dr["sfDate"]).ToString("yyyy-MM-dd HH:mm");
                    vo.sfMethod = (dr["sfMethod"].ToString() == "01" ? "电话随访" : "其他");
                    vo.sfClass = (dr["sfClass"].ToString() == "01" ? "普通" : "其他");
                    vo.sfRecorder = dr["sfRecorder"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion
        #region 随访记录-保存
        /// <summary>
        /// 随访记录-保存
        /// </summary>
        /// <param name="sfData"></param>
        /// <param name="sfId"></param>
        /// <returns></returns>
        public int SaveTnbSfRecord(EntityTnbSfData sfData, out decimal sfId)
        {
            int affectRows = 0;
            sfId = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                if (sfData.sfId <= 0)
                {
                    Sql = @"select max(t.sfId) as maxId from TnbSfData t";
                    DataTable dt = svc.GetDataTable(Sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["maxId"] != DBNull.Value)
                        {
                            sfData.sfId = Convert.ToDecimal(dt.Rows[0]["maxId"]) + 1;
                        }
                    }
                    if (sfData.sfId <= 0)
                        sfData.sfId = 1;
                }
                List<DacParm> lstParm = new List<DacParm>();
                lstParm.Add(svc.GetDelParmByPk(sfData));
                lstParm.Add(svc.GetInsertParm(sfData));
                affectRows = svc.Commit(lstParm);
                sfId = sfData.sfId;
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
        #endregion

        #region 评估记录
        #region 评估记录-获取
        /// <summary>
        /// 评估记录-获取
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityHmsSF> GetTnbPgRecords(List<EntityParm> parms)
        {
            List<EntityHmsSF> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.recId,
                           b.clientNo,
                           b.patName,
                           b.sex,
                           b.birthday,
                           b.classId,
                           c.pgId,
                           c.evaDate,
                           c.dangerLevel,
                           c.manageLevel,
                           d.oper_name as evaluator
                      from tnbRecord a
                     inner join hmsPatient b
                        on a.patId = b.patId
                     inner join tnbPg c
                        on a.recId = c.recId
                       and c.Status = 1
                      left join code_operator d
                        on c.evaluator = d.oper_code";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityHmsSF>();
                EntityHmsSF vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityHmsSF();
                    vo.recId = dr["recId"].ToString();
                    vo.clientNo = dr["clientNo"].ToString();
                    vo.patName = dr["patName"].ToString();
                    vo.sex = dr["sex"].ToString();
                    vo.age = dr["birthday"] == DBNull.Value ? "" : CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                    vo.patClass = dr["classId"].ToString() == "01" ? "普通" : "";
                    vo.pgId = dr["pgId"].ToString();
                    vo.evaDate = dr["evaDate"] == DBNull.Value ? "" : Function.Datetime(dr["evaDate"]).ToString("yyyy-MM-dd HH:mm");
                    vo.dangerLevel = (dr["dangerLevel"].ToString() == "01" ? "一般" : "/");
                    vo.manageLevel = (dr["manageLevel"].ToString() == "01" ? "I级" : "/");
                    vo.evaluator = dr["evaluator"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion
        #region 评估记录-保存
        /// <summary>
        /// 评估记录-保存
        /// </summary>
        /// <param name="pgData"></param>
        /// <param name="pgId"></param>
        /// <returns></returns>
        public int SaveTnbPgRecord(EntityTnbPgData pgData, out decimal pgId)
        {
            int affectRows = 0;
            pgId = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                if (pgData.pgId <= 0)
                {
                    Sql = @"select max(t.pgId) as maxId from tnbPgData t";
                    DataTable dt = svc.GetDataTable(Sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["maxId"] != DBNull.Value)
                        {
                            pgData.pgId = Convert.ToDecimal(dt.Rows[0]["maxId"]) + 1;
                        }
                    }
                    if (pgData.pgId <= 0)
                        pgData.pgId = 1;
                }
                List<DacParm> lstParm = new List<DacParm>();
                lstParm.Add(svc.GetDelParmByPk(pgData));
                lstParm.Add(svc.GetInsertParm(pgData));
                affectRows = svc.Commit(lstParm);
                pgId = pgData.pgId;
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

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hms.Entity;
using weCare.Core.Dac;
using weCare.Core.Entity;
using weCare.Core.Utils;

namespace Hms.Biz
{
    public class Biz206 : IDisposable
    {
        #region 膳食原则

        #region 膳食原则列表
        /// <summary>
        /// 膳食原则列表
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<EntityDietPrinciple> GetDietPrinciple(List<EntityParm> parms = null)
        {
            List<EntityDietPrinciple> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.principleId,
                           a.principleName,
                           a.contents
                      from dietPrinciple a ";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityDietPrinciple>();
                EntityDietPrinciple vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityDietPrinciple();
                    vo.principleId = dr["principleId"].ToString();
                    vo.principleName = dr["principleName"].ToString();
                    vo.contents = dr["contents"].ToString();

                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int SaveDietPrinciple(ref EntityDietPrinciple dietPrinciple)
        {
            SqlHelper svc = null;
            int affect = -1;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                List<DacParm> lstParm = new List<DacParm>();
                if (string.IsNullOrEmpty(dietPrinciple.principleId))
                {
                    dietPrinciple.principleId = svc.GetNextID("dietPrinciple", "dietPrincipleId").ToString().PadLeft(6, '0');
                    dietPrinciple.createDate = DateTime.Now;
                    dietPrinciple.createUserId = "00";
                    lstParm.Add(svc.GetInsertParm(dietPrinciple));
                }
                else
                {
                    dietPrinciple.modifyDate = DateTime.Now;
                    lstParm.Add(svc.GetUpdateParm(dietPrinciple, new List<string>() {
                    EntityDietPrinciple.Columns.principleName,
                    EntityDietPrinciple.Columns.contents,
                    EntityDietPrinciple.Columns.modifyDate,
                    EntityDietPrinciple.Columns.modifyUserId},
                        new List<string>() { EntityDietPrinciple.Columns.principleId }));
                }

                if (lstParm.Count > 0)
                    affect = svc.Commit(lstParm);
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
                affect = -1;
            }
            finally
            {
                svc = null;
            }


            return affect;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int DeleteDietPrinciple(List<EntityDietPrinciple> data)
        {
            int affectRows = 0;
            SqlHelper svc = null;
            List<DacParm> lstParm = new List<DacParm>();
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                foreach (var vo in data)
                {
                    lstParm.Add(svc.GetDelParmByPk(new EntityDietPrinciple() { principleId = vo.principleId }));
                }

                if (lstParm.Count > 0)
                    affectRows = svc.Commit(lstParm);
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

        #region 饮食菜谱模板

        #region 获取类型列表
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EntityDietTemplatetype> GetDietTemplatetype()
        {
            List<EntityDietTemplatetype> data = null;

            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.typeid,
                           a.typeName
                      from diettemplatetype a ";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityDietTemplatetype>();
                EntityDietTemplatetype vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityDietTemplatetype();
                    vo.typeId = dr["typeid"].ToString();
                    vo.typeName = dr["typeName"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 获取模板列表
        public List<EntityDietTemplate> GetDietTemplate()
        {
            List<EntityDietTemplate> data = null;
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.templateId,
                           a.templateName,
                           a.descriptions,
                           a.days,
                           a.typeid
                      from diettemplate a ";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityDietTemplate>();
                EntityDietTemplate vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityDietTemplate();
                    vo.templateId = dr["templateId"].ToString();
                    vo.templateName = dr["templateName"].ToString();
                    vo.descriptions = dr["descriptions"].ToString();
                    vo.days = Function.Int(dr["days"]);
                    vo.typeid = dr["typeid"].ToString();
                    data.Add(vo);
                }
            }

            return data;
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int SaveDietTemplate(ref EntityDietTemplate dietTemplate)
        {
            SqlHelper svc = null;
            int affect = -1;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                List<DacParm> lstParm = new List<DacParm>();
                if (string.IsNullOrEmpty(dietTemplate.templateId))
                {
                    dietTemplate.templateId = svc.GetNextID("dietTemplate", "templateId").ToString().PadLeft(6, '0');
                    dietTemplate.createDate = DateTime.Now;
                    dietTemplate.creator = "00";
                    dietTemplate.createName = "系统管理员";
                    lstParm.Add(svc.GetInsertParm(dietTemplate));
                }
                else
                {
                    dietTemplate.modifyDate = DateTime.Now;
                    lstParm.Add(svc.GetUpdateParm(dietTemplate, new List<string>() {
                    EntityDietTemplate.Columns.templateName,
                    EntityDietTemplate.Columns.descriptions,
                    EntityDietTemplate.Columns.modifyDate,
                    EntityDietTemplate.Columns.modifytor,
                    EntityDietTemplate.Columns.modifyName},
                        new List<string>() { EntityDietTemplate.Columns.templateId }));
                }

                if (lstParm.Count > 0)
                    affect = svc.Commit(lstParm);
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
                affect = -1;
            }
            finally
            {
                svc = null;
            }


            return affect;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int DeleteDietTemplate(List<EntityDietTemplate> data)
        {
            int affectRows = 0;
            SqlHelper svc = null;
            List<DacParm> lstParm = new List<DacParm>();
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                foreach (var vo in data)
                {
                    lstParm.Add(svc.GetDelParmByPk(new EntityDietTemplate() { templateId = vo.templateId }));
                }

                if (lstParm.Count > 0)
                    affectRows = svc.Commit(lstParm);
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

        #region 成品菜

        #region 获取菜谱列表
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EntityDisplayDicCaiRecipe> GetDicCaiRecipe()
        {
            List<EntityDisplayDicCaiRecipe> data = null;

            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select c.caiMasterName,c.caiMasterId,a.caiSlaveId,a.caiSlaveName 
                            from dicCaiRecipeSlave a 
                            left join dicCaiRecipeConfig b
                            on a.caiSlaveId = b.caiSlaveId
                            left join dicCaiRecipeMaster c
                            on b.caiMasterId= c.caiMasterId  order by c.caiMasterId  ";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityDisplayDicCaiRecipe>();
                EntityDisplayDicCaiRecipe vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityDisplayDicCaiRecipe();
                    vo.caiMasterId = Function.Int(dr["caiMasterId"]);
                    vo.caiMasterName = dr["caiMasterName"].ToString();
                    vo.caiSlaveId = dr["caiSlaveId"].ToString();
                    vo.caiSlaveName = dr["caiSlaveName"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 获取列表
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EntityDicCai> GetDicCai()
        {
            List<EntityDicCai> data = null;
            List<EntityDicCaiConfig> lstCaiRecipe = new List<EntityDicCaiConfig>();

            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty; ;

            //菜分类
            Sql = @"select a.caiSlaveId,a.caiSlaveName, b.caiId from dicCaiRecipeSlave a 
                            left join dicCaiConfig b 
                            on a.caiSlaveId = b.caiSlaveId ";
            DataTable dtRecipe = svc.GetDataTable(Sql);
            if (dtRecipe != null && dtRecipe.Rows.Count > 0)
            {
                EntityDicCaiConfig caiRecipe = null;
                foreach (DataRow dr in dtRecipe.Rows)
                {
                    caiRecipe = new EntityDicCaiConfig();
                    caiRecipe.caiId = dr["caiId"].ToString();
                    caiRecipe.caiSlaveId = dr["caiSlaveId"].ToString();
                    caiRecipe.caiSlaveNameStr = dr["caiSlaveName"].ToString();
                    lstCaiRecipe.Add(caiRecipe);
                }
            }

            //菜
            Sql = @"select a.* from dicCai a  ";

            DataTable dtCai = svc.GetDataTable(Sql);
            if (dtCai != null && dtCai.Rows.Count > 0)
            {
                data = new List<EntityDicCai>();
                EntityDicCai vo = null;
                foreach (DataRow dr in dtCai.Rows)
                {
                    vo = new EntityDicCai();
                    vo.lstCaiSlaveId = new List<string>();
                    vo.id = dr["id"].ToString();
                    vo.names = dr["names"].ToString();
                    vo.breakfast = dr["breakfast"].ToString();
                    if (vo.breakfast == "1")
                    {
                        vo.mealStr += "早餐、";
                    }
                    vo.lunch = dr["LUNCH"].ToString();
                    if (vo.lunch == "1")
                    {
                        vo.mealStr += "中餐、";
                    }
                    vo.dinner = dr["DINNER"].ToString();
                    if (vo.dinner == "1")
                    {
                        vo.mealStr += "晚餐、";
                    }
                    vo.other = dr["other"].ToString();
                    if (vo.other == "1")
                    {
                        vo.mealStr += "其他、";
                    }
                    if (!string.IsNullOrEmpty(vo.mealStr))
                    {
                        vo.mealStr = vo.mealStr.TrimEnd('、');
                    }

                    vo.kCal = Function.Dec(dr["KCAL"]);
                    vo.KJ = Function.Dec(dr["KJ"]);
                    vo.PROTEIN = Function.Dec(dr["PROTEIN"]);
                    vo.FAT = Function.Dec(dr["FAT"]);
                    vo.CHO = Function.Dec(dr["CHO"]);
                    vo.BRXXW = Function.Dec(dr["BRXXW"]);
                    vo.DGC = Function.Dec(dr["DGC"]);
                    vo.ASH = Function.Dec(dr["ASH"]);
                    vo.vitaminA = Function.Dec(dr["vitaminA"]);
                    vo.THIAMIN = Function.Dec(dr["THIAMIN"]);
                    vo.RIBOFLAVIN = Function.Dec(dr["RIBOFLAVIN"]);
                    vo.NIACIN = Function.Dec(dr["NIACIN"]);
                    vo.vitaminC = Function.Dec(dr["vitaminC"]);
                    vo.vitaminE = Function.Dec(dr["vitaminE"]);
                    vo.CA = Function.Dec(dr["CA"]);
                    vo.P = Function.Dec(dr["P"]);
                    vo.K = Function.Dec(dr["K"]);
                    vo.NA = Function.Dec(dr["NA"]);
                    vo.MG = Function.Dec(dr["MG"]);
                    vo.ZN = Function.Dec(dr["ZN"]);
                    vo.SE = Function.Dec(dr["SE"]);
                    vo.CU = Function.Dec(dr["CU"]);
                    vo.MN = Function.Dec(dr["MN"]);
                    vo.I = Function.Dec(dr["I"]);
                    vo.F = Function.Dec(dr["F"]);
                    vo.CR = Function.Dec(dr["CR"]);
                    vo.MU = Function.Dec(dr["MU"]);
                    vo.vitaminD = Function.Dec(dr["vitaminD"]);
                    vo.vitaminB6 = Function.Dec(dr["vitaminB6"]);
                    vo.vitaminB12 = Function.Dec(dr["vitaminB12"]);
                    vo.vitaminB5 = Function.Dec(dr["vitaminB5"]);
                    vo.vitaminB9 = Function.Dec(dr["vitaminB9"]);
                    vo.DANJIAN = Function.Dec(dr["DANJIAN"]);
                    vo.vitaminH = Function.Dec(dr["vitaminH"]);

                    var list = lstCaiRecipe.FindAll(u => u.caiId == vo.id);
                    if (list != null && list.Count > 0)
                    {
                        foreach (var str in list)
                        {
                            vo.caiSlaveNameStr += str.caiSlaveNameStr + "、";
                            vo.lstCaiSlaveId.Add(str.caiSlaveId);
                        }
                        if (!string.IsNullOrEmpty(vo.caiSlaveNameStr))
                            vo.caiSlaveNameStr = vo.caiSlaveNameStr.TrimEnd('、');
                    }
                    vo.methods = dr["methods"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 菜原料
        /// <summary>
        /// 
        /// </summary>
        /// <param name="caiId"></param>
        /// <returns></returns>
        public List<EntityDicCaiIngredient> GetCaiIngredient(string caiId)
        {
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            List<EntityDicCaiIngredient> latCaiIngredient = null;
            string Sql = string.Empty;
            //菜原料
            Sql = @"select id,caiId, ingredietId,ingredietName,weight from dicCaiIngredient where caiId = '" + caiId + "'";
            DataTable dtCaiIngredient = svc.GetDataTable(Sql);
            if (dtCaiIngredient != null && dtCaiIngredient.Rows.Count > 0)
            {
                latCaiIngredient = new List<EntityDicCaiIngredient>();
                foreach (DataRow dr in dtCaiIngredient.Rows)
                {
                    EntityDicCaiIngredient vo = new EntityDicCaiIngredient();
                    vo.id = dr["id"].ToString();
                    vo.caiId = dr["caiId"].ToString();
                    vo.ingredietId = dr["ingredietId"].ToString();
                    vo.ingredietName = dr["ingredietName"].ToString();
                    vo.weight = Function.Dec(dr["weight"]);
                    latCaiIngredient.Add(vo);
                }
            }

            return latCaiIngredient;
        }
        #endregion

        #region 获取原料分类
        /// <summary>
        /// 获取原料分类 
        /// </summary>
        /// <returns></returns>
        public List<EntityDicIngredientClassify> GetIngredientClassify()
        {
            List<EntityDicIngredientClassify> data = null;

            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select * from dicIngredientClassify  ";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityDicIngredientClassify>();
                EntityDicIngredientClassify vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityDicIngredientClassify();
                    vo.classifyId = Function.Int(dr["classifyId"]);
                    vo.classifyName = dr["classifyName"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int SaveDicCai(ref EntityDicCai cai)
        {
            SqlHelper svc = null;
            int affect = -1;
            string sql = string.Empty;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                List<DacParm> lstParm = new List<DacParm>();
                if (string.IsNullOrEmpty(cai.id))
                {
                    cai.id = "C" + svc.GetNextID("dicCai", "id").ToString().PadLeft(6, '0');
                    cai.createDate = DateTime.Now;
                    cai.creator = "00";
                    cai.creatorName = "系统管理员";
                    lstParm.Add(svc.GetInsertParm(cai));
                }
                else
                {
                    cai.modifedDate = DateTime.Now;
                    lstParm.Add(svc.GetUpdateParm(cai, new List<string>() {
                    EntityDicCai.Columns.names,
                    EntityDicCai.Columns.breakfast,
                    EntityDicCai.Columns.lunch,
                    EntityDicCai.Columns.dinner,
                    EntityDicCai.Columns.other,
                    EntityDicCai.Columns.methods,
                    EntityDicCai.Columns.kCal,
                    EntityDicCai.Columns.KJ,
                    EntityDicCai.Columns.PROTEIN,
                    EntityDicCai.Columns.FAT,
                    EntityDicCai.Columns.CHO,
                    EntityDicCai.Columns.BRXXW,
                    EntityDicCai.Columns.DGC,
                    EntityDicCai.Columns.ASH,
                    EntityDicCai.Columns.vitaminA,
                    EntityDicCai.Columns.THIAMIN,
                    EntityDicCai.Columns.RIBOFLAVIN,
                    EntityDicCai.Columns.NIACIN,
                    EntityDicCai.Columns.vitaminC,
                    EntityDicCai.Columns.vitaminE,
                    EntityDicCai.Columns.CA,
                    EntityDicCai.Columns.P,
                    EntityDicCai.Columns.K,
                    EntityDicCai.Columns.NA,
                    EntityDicCai.Columns.MG,
                    EntityDicCai.Columns.FE,
                    EntityDicCai.Columns.ZN,
                    EntityDicCai.Columns.SE,
                    EntityDicCai.Columns.CU,
                    EntityDicCai.Columns.MN,
                    EntityDicCai.Columns.I,
                    EntityDicCai.Columns.F,
                    EntityDicCai.Columns.CR,
                    EntityDicCai.Columns.MU,
                    EntityDicCai.Columns.vitaminD,
                    EntityDicCai.Columns.vitaminB6,
                    EntityDicCai.Columns.vitaminB12,
                    EntityDicCai.Columns.vitaminB5,
                    EntityDicCai.Columns.vitaminB9,
                    EntityDicCai.Columns.DANJIAN,
                    EntityDicCai.Columns.vitaminH,
                    EntityDicCai.Columns.bakfield1,
                    EntityDicCai.Columns.bakfield2,
                    EntityDicCai.Columns.modifedDate,
                    EntityDicCai.Columns.modifedor,
                    EntityDicCai.Columns.modifedName},
                        new List<string>() { EntityDicCai.Columns.id }));
                }

                //菜分类
                if (cai.lstCaiSlaveId != null )
                {
                    sql = "delete  from dicCaiConfig where caiId = '" + cai.id + "'";
                    lstParm.Add(svc.GetDacParm(EnumExecType.ExecSql, sql));
                    foreach (var slaveid in cai.lstCaiSlaveId)
                    {
                        EntityDicCaiConfig vo = new EntityDicCaiConfig();
                        vo.caiId = cai.id;
                        vo.caiSlaveId = slaveid;
                        lstParm.Add(svc.GetInsertParm(vo));
                    }
                }
                //菜原料
                if (cai.lstCaiIngredient != null)
                {
                    sql = "delete  from dicCaiIngredient where caiId = '" + cai.id + "'";
                    lstParm.Add(svc.GetDacParm(EnumExecType.ExecSql, sql));
                    foreach (var caiIngredient in cai.lstCaiIngredient)
                    {
                        EntityDicCaiIngredient vo = new EntityDicCaiIngredient();
                        vo.id = svc.GetNextID("dicCaiIngredient", "id").ToString().PadLeft(6, '0');
                        vo.caiId = cai.id;
                        vo.ingredietId = caiIngredient.ingredietId;
                        vo.ingredietName = caiIngredient.ingredietName;
                        vo.weight = caiIngredient.weight;
                        lstParm.Add(svc.GetInsertParm(vo));
                    }
                }

                if (lstParm.Count > 0)
                    affect = svc.Commit(lstParm);
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
                affect = -1;
            }
            finally
            {
                svc = null;
            }


            return affect;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="cai"></param>
        /// <returns></returns>
        public int DeleteDicCai(EntityDicCai cai)
        {
            int affectRows = 0;
            string sql = string.Empty;
            SqlHelper svc = null;
            List<DacParm> lstParm = new List<DacParm>();
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                //菜
                lstParm.Add(svc.GetDelParmByPk(new EntityDicCai() { id = cai.id }));
                //菜分类
                if(cai.lstCaiSlaveId != null)
                {
                    string subStr = string.Empty;
                    foreach(var strId in cai.lstCaiSlaveId)
                    {
                        subStr += "'" + strId + "',";
                    }
                    if (!string.IsNullOrEmpty(subStr))
                        subStr = "(" + subStr.TrimEnd(',') + ")";
                    sql = @"delete from diccaiconfig where caiSlaveId in " + subStr;
                    lstParm.Add(svc.GetDacParm(EnumExecType.ExecSql,sql));
                }
                //菜原料
                if(cai.lstCaiIngredient != null)
                {                  
                    lstParm.Add(svc.GetDelParm(cai.lstCaiIngredient[0], EntityDicCaiIngredient.Columns.caiId));
                }

                if (lstParm.Count > 0)
                    affectRows = svc.Commit(lstParm);
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

        #region 原料

        #region 获取原料分类
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EntityDicIngredientClassify> GetDicIngredientClassify()
        {
            List<EntityDicIngredientClassify> data = null;

            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select a.classifyId,a.classifyName from dicIngredientClassify a  order by a.classifyId  ";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityDicIngredientClassify>();
                EntityDicIngredientClassify vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityDicIngredientClassify();
                    vo.classifyId = Function.Int(dr["classifyId"]);
                    vo.classifyName = dr["classifyName"].ToString();
                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 获取原料
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EntityDicDientIngredient> GetDicDietIngredient()
        {
            List<EntityDicDientIngredient> data = null;
            List<EntityDicIngredientConfig> lstClassify = new List<EntityDicIngredientConfig>();
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select * from dicDietIngredient  ";
            DataTable dt = svc.GetDataTable(Sql);

            Sql = @"select b.classifyId,a.id from dicDietIngredient a 
                    left join dicIngredientConfig b 
                    on a.id = b.ingredientId ";
            DataTable dtClassify = svc.GetDataTable(Sql);

            if (dtClassify != null && dtClassify.Rows.Count > 0)
            {
                foreach (DataRow dr in dtClassify.Rows)
                {
                    EntityDicIngredientConfig configVo = new EntityDicIngredientConfig();
                    configVo.classifyId = Function.Int(dr["classifyId"]);
                    configVo.ingredientId = dr["id"].ToString();
                    lstClassify.Add(configVo);
                }
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityDicDientIngredient>();
                EntityDicDientIngredient vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityDicDientIngredient();
                    vo.lstClassify = new List<int>();
                    vo.id = dr["id"].ToString();
                    vo.names = dr["names"].ToString();
                    vo.otherName = dr["otherName"].ToString();
                    vo.foodSort = dr["foodSort"].ToString();
                    vo.ingredientId = dr["ingredientId"].ToString();
                    vo.remaks = dr["remaks"].ToString();
                    vo.eatPercent = Function.Dec(dr["eatPercent"]);
                    vo.water = Function.Dec(dr["water"]);
                    vo.kCal = Function.Dec(dr["kCal"]);
                    vo.kj = Function.Dec(dr["kj"]);
                    vo.proteint = Function.Dec(dr["proteint"]);
                    vo.fat = Function.Dec(dr["fat"]);
                    vo.cho = Function.Dec(dr["cho"]);
                    vo.brxxw = Function.Dec(dr["brxxw"]);
                    vo.dgc = Function.Dec(dr["dgc"]);
                    vo.ash = Function.Dec(dr["ash"]);
                    vo.vitaminsA = Function.Dec(dr["vitaminsA"]);
                    vo.thiamin = Function.Dec(dr["thiamin"]);
                    vo.niacin = Function.Dec(dr["niacin"]);
                    vo.vitaminsC = Function.Dec(dr["vitaminsC"]);
                    vo.vitaminsE = Function.Dec(dr["vitaminsE"]);
                    vo.ca = Function.Dec(dr["ca"]);
                    vo.p = Function.Dec(dr["p"]);
                    vo.k = Function.Dec(dr["k"]);
                    vo.na = Function.Dec(dr["na"]);
                    vo.mg = Function.Dec(dr["mg"]);
                    vo.fe = Function.Dec(dr["fe"]);
                    vo.zn = Function.Dec(dr["zn"]);
                    vo.se = Function.Dec(dr["se"]);
                    vo.cu = Function.Dec(dr["cu"]);
                    vo.mn = Function.Dec(dr["mn"]);
                    vo.i = Function.Dec(dr["i"]);
                    vo.f = Function.Dec(dr["f"]);
                    vo.cr = Function.Dec(dr["cr"]);
                    vo.mu = Function.Dec(dr["mu"]);
                    vo.vitaminsD = Function.Dec(dr["vitaminsD"]);
                    vo.vitaminsB6 = Function.Dec(dr["vitaminsB6"]);
                    vo.vitaminsB12 = Function.Dec(dr["vitaminsB12"]);
                    vo.vitaminsB5 = Function.Dec(dr["vitaminsB5"]);
                    vo.vitaminsB9 = Function.Dec(dr["vitaminsB9"]);
                    vo.danjian = Function.Dec(dr["danjian"]);
                    vo.vitaminsH = Function.Dec(dr["vitaminsH"]);
                    vo.bakfield1 = dr["bakfield1"].ToString();
                    vo.bakfield2 = dr["bakfield2"].ToString();

                    var list = lstClassify.FindAll(u => u.ingredientId == vo.id);
                    if (list != null && list.Count > 0)
                    {
                        foreach (var str in list)
                        {
                            vo.lstClassify.Add(str.classifyId);
                        }
                    }

                    data.Add(vo);
                }
            }
            return data;
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public int SaveDicIngredient(ref EntityDicDientIngredient dicDientIngredient)
        {
            SqlHelper svc = null;
            int affect = -1;
            string sql = string.Empty;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                List<DacParm> lstParm = new List<DacParm>();
                if (string.IsNullOrEmpty(dicDientIngredient.id))
                {
                    dicDientIngredient.id = dicDientIngredient.lstClassify[0].ToString().PadLeft(2,'0')+ "-" + svc.GetNextID("dicDietIngredient", "id").ToString().PadLeft(6, '0');
                    lstParm.Add(svc.GetInsertParm(dicDientIngredient));
                }
                else
                {
                    dicDientIngredient.ingredientId = dicDientIngredient.id;
                    lstParm.Add(svc.GetUpdateParm(dicDientIngredient, new List<string>() {
                    EntityDicDientIngredient.Columns.foodSort,
                    EntityDicDientIngredient.Columns.ingredientId,
                    EntityDicDientIngredient.Columns.names,
                    EntityDicDientIngredient.Columns.otherName,
                    EntityDicDientIngredient.Columns.remaks,
                    EntityDicDientIngredient.Columns.eatPercent,
                    EntityDicDientIngredient.Columns.water,
                    EntityDicDientIngredient.Columns.kCal,
                    EntityDicDientIngredient.Columns.kj,
                    EntityDicDientIngredient.Columns.proteint,
                    EntityDicDientIngredient.Columns.fat,
                    EntityDicDientIngredient.Columns.cho,
                    EntityDicDientIngredient.Columns.brxxw,
                    EntityDicDientIngredient.Columns.dgc,
                    EntityDicDientIngredient.Columns.ash,
                    EntityDicDientIngredient.Columns.vitaminsA,
                    EntityDicDientIngredient.Columns.thiamin,
                    EntityDicDientIngredient.Columns.riboflavin,
                    EntityDicDientIngredient.Columns.niacin,
                    EntityDicDientIngredient.Columns.vitaminsC,
                    EntityDicDientIngredient.Columns.vitaminsE,
                    EntityDicDientIngredient.Columns.p,
                    EntityDicDientIngredient.Columns.k,
                    EntityDicDientIngredient.Columns.na,
                    EntityDicDientIngredient.Columns.mg,
                    EntityDicDientIngredient.Columns.fe,
                    EntityDicDientIngredient.Columns.zn,
                    EntityDicDientIngredient.Columns.se,
                    EntityDicDientIngredient.Columns.cu,
                    EntityDicDientIngredient.Columns.mn,
                    EntityDicDientIngredient.Columns.i,
                    EntityDicDientIngredient.Columns.f,
                    EntityDicDientIngredient.Columns.cr,
                    EntityDicDientIngredient.Columns.mu,
                    EntityDicDientIngredient.Columns.vitaminsD,
                    EntityDicDientIngredient.Columns.vitaminsB6,
                    EntityDicDientIngredient.Columns.vitaminsB12,
                    EntityDicDientIngredient.Columns.vitaminsB5,
                    EntityDicDientIngredient.Columns.vitaminsB9,
                    EntityDicDientIngredient.Columns.danjian,
                    EntityDicDientIngredient.Columns.vitaminsH,
                    EntityDicDientIngredient.Columns.bakfield1,
                    EntityDicDientIngredient.Columns.bakfield2 },
                        new List<string>() { EntityDicDientIngredient.Columns.id }));
                }

                //保存 分类
                if(dicDientIngredient.lstClassify != null)
                {
                    foreach(int id in dicDientIngredient.lstClassify)
                    {
                        EntityDicIngredientConfig vo = new EntityDicIngredientConfig();
                        vo.classifyId = id;
                        vo.ingredientId = dicDientIngredient.ingredientId;
                        lstParm.Add(svc.GetInsertParm(vo));
                    }
                }

                if (lstParm.Count > 0)
                    affect = svc.Commit(lstParm);
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
                affect = -1;
            }
            finally
            {
                svc = null;
            }


            return affect;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="cai"></param>
        /// <returns></returns>
        public int DeleteDicIngredient(EntityDicDientIngredient dicDientIngredienti)
        {
            int affectRows = 0;
            string sql = string.Empty;
            SqlHelper svc = null;
            List<DacParm> lstParm = new List<DacParm>();
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                
                lstParm.Add(svc.GetDelParmByPk(new EntityDicDientIngredient() { id = dicDientIngredienti.id }));
                
                if (dicDientIngredienti.lstClassify != null)
                {
                    string subStr = string.Empty;
                    foreach (var strId in dicDientIngredienti.lstClassify)
                    {
                        subStr += "" + strId + ",";
                    }
                    if (!string.IsNullOrEmpty(subStr))
                        subStr = "(" + subStr.TrimEnd(',') + ")";
                    sql = @"delete from dicIngredientConfig where ingredientId in " + subStr;
                    lstParm.Add(svc.GetDacParm(EnumExecType.ExecSql, sql));
                }
                
                if (lstParm.Count > 0)
                    affectRows = svc.Commit(lstParm);
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

        #region 中医食疗

        #region 获取列表
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EntityDietTreatment> GetDietTreatment()
        {
            List<EntityDietTreatment> data = null;

            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            string Sql = string.Empty;
            Sql = @"select * from dicdiettreatment   ";

            DataTable dt = svc.GetDataTable(Sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                data = new List<EntityDietTreatment>();
                EntityDietTreatment vo = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vo = new EntityDietTreatment();
                    vo.id = dr["id"].ToString() ;
                    vo.names = dr["names"].ToString();
                    vo.configs = dr["configs"].ToString();
                    vo.fuctions = dr["fuctions"].ToString();
                    vo.usage = dr["usage"].ToString();
                    vo.attention = dr["attention"].ToString();
                    vo.methods = dr["methods"].ToString();
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

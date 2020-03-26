using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Drawing;
using weCare.Core.Entity;

namespace Hms.Entity
{
    [DataContract,Serializable]
    public class EntityDisplayClientRpt : BaseDataContract
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string clientName { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember]
        public string clientNo { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public int gender { get; set; }
        /// <summary>
        /// 体检编号
        /// </summary>
        [DataMember]
        public string reportNo { get; set; }
        /// <summary>
        /// 报告日期
        /// </summary>
        [DataMember]
        public string reportDate { get; set; }
        /// <summary>
        /// 未配备异常
        /// </summary>
        [DataMember]
        public int unMatch { get; set; }
        /// <summary>
        /// 打印状态
        /// </summary>
        [DataMember]
        public int reportStatc { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        [DataMember]
        public int suditState { get; set; }
        /// <summary>
        /// 报告份数
        /// </summary>
        [DataMember]
        public int reportCount { get; set; }
        /// <summary>
        /// 人员类别
        /// </summary>
        [DataMember]
        public string gradeName { get; set; }

        [DataMember]
        public string sex
        {
            get
            {
                if (gender == 1)
                    return "男";
                else if (gender == 2)
                    return "女";
                else
                    return "不限";
            }
        }

        [DataMember]
        public string confirmState
        {
            get
            {
                if (suditState == 1)
                    return "无需审核";
                if (suditState == 2)
                    return "审核通过";
                if (suditState == 3)
                    return "等等审核";
                if (suditState == 4)
                    return "已分配审核";
                if (suditState == 5)
                    return "审核不通过";
                return "";
            }
        }

        [DataMember]
        public string printState
        {
            get
            {
                if (reportStatc == 1)
                    return "已打印";
                if (reportStatc == 2)
                    return "未打印";
                return "";
            }
        }
        /// <summary>
        /// 年龄
        /// </summary>
        [DataMember]
        public string age { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string company { get; set; }
        [DataMember]
        public Image image01 { get; set; }
        [DataMember]
        public Image image02 { get; set; }
        [DataMember]
        public Image image03 { get; set; }
        [DataMember]
        public Image image04 { get; set; }
        [DataMember]
        public Image image05 { get; set; }
        [DataMember]
        public Image image06 { get; set; }
        [DataMember]
        public Image image07 { get; set; }
        [DataMember]
        public Image image08 { get; set; }
        [DataMember]
        public Image image09 { get; set; }
        [DataMember]
        public Image image10 { get; set; }
        [DataMember]
        public Image image11 { get; set; }
        [DataMember]
        public Image image12 { get; set; }
        [DataMember]
        public Image imageFx { get; set; }
        [DataMember]
        public Image imageTip { get; set; }
        
        /// <summary>
        /// 重要指标
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstMainIndicate { get; set; }
        /// <summary>
        /// 高血压
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstGxy { get; set; }
        /// <summary>
        /// 高血压风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateGxy { get; set; }

        /// <summary>
        /// 糖尿病
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstTnb { get; set; }
        /// <summary>
        /// 糖尿病风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateGxb { get; set; }

        /// <summary>
        /// 冠心病
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstGxb { get; set; }
        /// <summary>
        /// 冠心病风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateTnb { get; set; }

        /// <summary>
        /// 血脂异常
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstXzyc { get; set; }
        /// <summary>
        /// 血脂异常风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateXzyc { get; set; }

        /// <summary>
        /// 肥胖症
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams>lstFpz { get; set; }
        /// <summary>
        /// 肥胖症风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateFpz { get; set; }

        /// <summary>
        /// 脑卒中
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams>lstNzz { get; set; }
        /// <summary>
        /// 脑卒中风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateNzz { get; set; }


        /// <summary>
        /// 阿尔茨海默病
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstAzhmz { get; set; }
        /// <summary>
        /// 阿尔茨海默病风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateAzhmz { get; set; }

        /// <summary>
        /// 肺癌
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstFa { get; set; }
        /// <summary>
        /// 肺癌风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateFa { get; set; }

        /// <summary>
        /// 肝癌
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstGa { get; set; }
        /// <summary>
        /// 肝癌风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateGa { get; set; }

        /// <summary>
        /// 胃癌
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstWa { get; set; }
        /// <summary>
        /// 胃癌风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateWa { get; set; }

        /// <summary>
        /// 前列腺癌
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstQlxa { get; set; }
        /// <summary>
        ///前列腺癌风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateQlxa { get; set; }
        public List<EntityAdPeItem> lstAdPeItemBse { get; set; }
        [DataMember]
        public List<EntityAdPeItem> lstAdPeItemSpecial { get; set; }
        [DataMember]
        public List<EntityMedicalAdvicecs> lstMedAdvices { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;
using System.Drawing;

namespace Hms.Entity
{
    public class EntityClientReport : BaseDataContract
    {

        public string reportNo { get; set; }
        public string clientName { get; set; }
        public string clientNo { get; set; }
        public string company { get; set; }
        public string reportDate { get; set; }
        public string sex { get; set; }
        public string age { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        [DataMember]
        public string tjSumup { get; set; }
        /// <summary>
        /// 现患疾病
        /// </summary>
        [DataMember]
        public string illness { get; set; }
        /// <summary>
        /// 家族病史
        /// </summary>
        [DataMember]
        public string familyIllness { get; set; }

        /// <summary>
        /// 重要指标
        /// </summary>
        public  List<EntityReportMainItem> lstMainItem { get; set; }

        #region  个人报告内容
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
        public Image imageTip { get; set; }


        #region 高血压
        /// <summary>
        /// 高血压
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstGxyModelParam { get; set; }
        /// <summary>
        /// 高血压得分
        /// </summary>
        [DataMember]
        public decimal  gxyDf { get; set; }
        [DataMember]
        public decimal gxyBestDf { get; set; }
        [DataMember]
        public decimal gxyAbasableDf { get; set; }
        /// <summary>
        /// 高血压风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateGxy { get; set; }

        [DataMember]
        public Image imgGxyFx01 { get; set; }
        [DataMember]
        public Image imgGxyFx02 { get; set; }
        [DataMember]
        public Image imgGxyFx03 { get; set; }
        [DataMember]
        public Image imgGxyFx04 { get; set; }

        [DataMember]
        public string gxyPoint1 { get; set; }
        [DataMember]
        public string gxyPoint2 { get; set; }
        [DataMember]
        public string gxyPoint3 { get; set; }
        [DataMember]
        public string gxyPoint4 { get; set; }
        [DataMember]
        public string gxyPoint5 { get; set; }
        [DataMember]
        public string gxyPoint6 { get; set; }
        [DataMember]
        public string gxyPoint7 { get; set; }
        [DataMember]
        public string gxyPoint8 { get; set; }

        #endregion

        #region 糖尿病
        /// <summary>
        /// 糖尿病
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstTnbModelParam { get; set; }
        /// <summary>
        /// 糖尿病风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateTnb { get; set; }
        /// <summary>
        /// 糖尿病得分
        /// </summary>
        [DataMember]
        public decimal tnbDf { get; set; }
        [DataMember]
        public decimal tnbBestDf { get; set; }
        [DataMember]
        public decimal tnbAbasableDf { get; set; }

        [DataMember]
        public Image imgTnbFx01 { get; set; }
        [DataMember]
        public Image imgTnbFx02 { get; set; }
        [DataMember]
        public Image imgTnbFx03 { get; set; }
        [DataMember]
        public Image imgTnbFx04 { get; set; }

        [DataMember]
        public string tnbPoint1 { get; set; }
        [DataMember]
        public string tnbPoint2 { get; set; }
        [DataMember]
        public string tnbPoint3 { get; set; }
        [DataMember]
        public string tnbPoint4 { get; set; }
        [DataMember]
        public string tnbPoint5 { get; set; }
        [DataMember]
        public string tnbPoint6 { get; set; }
        [DataMember]
        public string tnbPoint7 { get; set; }
        [DataMember]
        public string tnbPoint8 { get; set; }
        #endregion

        #region 冠心病
        /// <summary>
        /// 冠心病
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstGxbModelParam { get; set; }
        /// <summary>
        /// 冠心病风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateGxb { get; set; }
        /// <summary>
        /// 冠心病得分
        /// </summary>
        [DataMember]
        public decimal gxbDf { get; set; }
        [DataMember]
        public decimal gxbBestDf { get; set; }
        [DataMember]
        public decimal gxAbasableDf { get; set; }

        [DataMember]
        public Image imgGxbFx01 { get; set; }
        [DataMember]
        public Image imgGxbFx02 { get; set; }
        [DataMember]
        public Image imgGxbFx03 { get; set; }
        [DataMember]
        public Image imgGxbFx04 { get; set; }

        [DataMember]
        public string gxbPoint1 { get; set; }
        [DataMember]
        public string gxbPoint2 { get; set; }
        [DataMember]
        public string gxbPoint3 { get; set; }
        [DataMember]
        public string gxbPoint4 { get; set; }
        [DataMember]
        public string gxbPoint5 { get; set; }
        [DataMember]
        public string gxbPoint6 { get; set; }
        [DataMember]
        public string gxbPoint7 { get; set; }
        [DataMember]
        public string gxbPoint8 { get; set; }

        #endregion

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
        public List<EntityEvaluateParams> lstFpz { get; set; }
        /// <summary>
        /// 肥胖症风险评估 
        /// </summary>
        [DataMember]
        public List<EntityEvaluateResult> lstEvaluateFpz { get; set; }

        /// <summary>
        /// 脑卒中
        /// </summary>
        [DataMember]
        public List<EntityEvaluateParams> lstNzz { get; set; }
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

        #endregion  
    }
}

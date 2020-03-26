using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;

namespace Hms.Entity
{
    /// <summary>
    /// 重要指标
    /// </summary>
    [DataContract,Serializable]
    public class EntityEvaluateParams : BaseDataContract
    {
        [DataMember]
        public Image pic { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember]
        public string item { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        [DataMember]
        public string result { get; set; }
        /// <summary>
        /// 参考范围
        /// </summary>
        [DataMember]
        public string range { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [DataMember]
        public string unit { get; set; }
    }
}

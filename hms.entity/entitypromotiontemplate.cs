using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;

namespace Hms.Entity
{
    [DataContract,Serializable]
    public class EntityPromotionTemplate :BaseDataContract
    {
        public string id { get; set; }
        public string templateName { get; set; }
        public string templateCondition { get; set; }
        public string isEnabled { get; set; }
        public string bakField1 { get; set; }
        public string bakField2 { get; set; }
        public DateTime ? createDate { get; set; }
        public string cretateId { get; set; }
        public string creator { get; set; }
        public DateTime ? modifyDate { get; set; }
        public string modifyId { get; set; }
        public string modifyName { get; set; }
    }
}

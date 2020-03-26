using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;

namespace Hms.Entity
{
    [DataContract, Serializable]
    [Entity(TableName = "userGrade")]
    public class EntityUserGrade : BaseDataContract
    {
        [DataMember]
        [Entity(FieldName = "id", DbType = DbType.String, IsPK = true, IsSeq = false, SerNo = 1)]
        public string id { get; set; }
        [DataMember]
        [Entity(FieldName = "gradeName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 2)]
        public string gradeName { get; set; }
        [DataMember]
        [Entity(FieldName = "reportName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 3)]
        public string reportName { get; set; }
        [DataMember]
        [Entity(FieldName = "severPrice", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 4)]
        public string severPrice { get; set; }
        [DataMember]
        [Entity(FieldName = "serverTime", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 5)]
        public string serverTime { get; set; }
        [DataMember]
        [Entity(FieldName = "description", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 6)]
        public string description { get; set; }
        [DataMember]
        [Entity(FieldName = "isEnable", DbType = DbType.Int16, IsPK = false, IsSeq = false, SerNo = 7)]
        public int isEnable { get; set; }
        [DataMember]
        [Entity(FieldName = "upTag", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 8)]
        public string upTag { get; set; }
        [DataMember]
        [Entity(FieldName = "bakfield1", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 9)]
        public string bakfield1 { get; set; }
        [DataMember]
        [Entity(FieldName = "bakfield2", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 10)]
        public string bakfield2 { get; set; }
        [DataMember]
        [Entity(FieldName = "createDate", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 11)]
        public DateTime? createDate { get; set; }
        [DataMember]
        [Entity(FieldName = "creator", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 12)]
        public string creator { get; set; }
        [DataMember]
        [Entity(FieldName = "createName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 13)]
        public string createName { get; set; }
        [DataMember]
        [Entity(FieldName = "modifyDate", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 14)]
        public DateTime? modifyDate { get; set; }
        [DataMember]
        [Entity(FieldName = "modifyrId", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 15)]
        public string modifyrId { get; set; }
        [DataMember]
        [Entity(FieldName = "modifyName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 16)]

        public string isEnableStr
        {
            get
            {
                if (isEnable == 1)
                    return "是";
                return "否"; ;
            }
        }
        public string modifyName { get; set; }
        public static EnumCols Columns = new EnumCols();

        public class EnumCols
        {
            public string id = "id";
            public string gradeName = "gradeName";
            public string reportName = "reportName";
            public string severPrice = "severPrice";
            public string serverTime = "serverTime";
            public string description = "description";
            public string isEnable = "isEnable";
            public string upTag = "upTag";
            public string bakfield1 = "bakfield1";
            public string bakfield2 = "bakfiedld2";
            public string createDate = "createDate";
            public string creator = "creator";
            public string createName = "createName";
            public string modifyDate = "modifyDate";
            public string modifyrId = "modifyrId";
            public string modifyName = "modifyName";

        }
    }
}

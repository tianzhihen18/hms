using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;
using weCare.Core.Utils;

namespace Hms.Entity
{
    [DataContract,Serializable]
    [Entity(TableName ="clientInfo")]
    public class EntityClientInfo : BaseDataContract
    {
        [DataMember]
        [Entity(FieldName = "id", DbType = DbType.String, IsPK = true, IsSeq = false, SerNo = 1)]
        public string  id { get; set; }
        [DataMember]
        [Entity(FieldName = "gradeId", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 2)]
        public string gradeId { get; set; }
        [DataMember]
        [Entity(FieldName = "clientNo", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 3)]
        public string clientNo { get; set; }
        [DataMember]
        [Entity(FieldName = "clientName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 4)]
        public string clientName { get; set; }
        [DataMember]
        [Entity(FieldName = "gender", DbType = DbType.Int16, IsPK = false, IsSeq = false, SerNo = 5)]
        public int gender { get; set; }
        [DataMember]
        [Entity(FieldName = "birthday", DbType = DbType.Date, IsPK = false, IsSeq = false, SerNo = 6)]
        public DateTime? birthday { get; set; }
        [DataMember]
        [Entity(FieldName = "mobile", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 7)]
        public string mobile { get; set; }
        [DataMember]
        [Entity(FieldName = "telephone", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 8)]
        public string telephone { get; set; }
        [DataMember]
        [Entity(FieldName = "email", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 9)]
        public string email { get; set; }
        [DataMember]
        [Entity(FieldName = "qq", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 10)]
        public string qq { get; set; }
        [DataMember]
        [Entity(FieldName = "cardNo", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 11)]
        public string cardNo { get; set; }
        [DataMember]
        [Entity(FieldName = "company", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 12)]
        public string company { get; set; }
        [DataMember]
        [Entity(FieldName = "regionId", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 13)]
        public string regionId { get; set; }
        [DataMember]
        [Entity(FieldName = "address", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 14)]
        public string address { get; set; }
        [DataMember]
        [Entity(FieldName = "booldType", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 15)]
        public int booldType { get; set; }
        [DataMember]
        [Entity(FieldName = "profession", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 16)]
        public int profession { get; set; }
        [DataMember]
        [Entity(FieldName = "marriage", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 17)]
        public int marriage { get; set; }
        [DataMember]
        [Entity(FieldName = "ehtnicGroup", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 18)]
        public int ehtnicGroup { get; set; }
        [DataMember]
        [Entity(FieldName = "eduationLevel", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 19)]
        public int eduationLevel { get; set; }
        [DataMember]
        [Entity(FieldName = "clientTag", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 20)]
        public string clientTag { get; set; }
        [DataMember]
        [Entity(FieldName = "contactName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 21)]
        public string contactName { get; set; }
        [DataMember]
        [Entity(FieldName = "contactNameMobile", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 22)]
        public string contactNameMobile { get; set; }
        [DataMember]
        [Entity(FieldName = "clientRemarks", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 23)]
        public string clientRemarks { get; set; }
        [DataMember]
        [Entity(FieldName = "dataSource", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 24)]
        public string dataSource { get; set; }
        [DataMember]
        [Entity(FieldName = "upTag", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 25)]
        public string upTag { get; set; }
        [DataMember]
        [Entity(FieldName = "serverDate", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 26)]
        public DateTime? serverDate { get; set; }
        [DataMember]
        [Entity(FieldName = "bakfileld1", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 27)]
        public string bakfileld1 { get; set; }
        [DataMember]
        [Entity(FieldName = "bakfileld2", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 28)]
        public string bakfileld2 { get; set; }
        [DataMember]
        [Entity(FieldName = "createDate", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 29)]
        public DateTime? createDate { get; set; }
        [DataMember]
        [Entity(FieldName = "creatorId", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 30)]
        public string creatorId { get; set; }
        [DataMember]
        [Entity(FieldName = "createName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 31)]
        public string createName { get; set; }
        [DataMember]
        [Entity(FieldName = "modifyDate", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 32)]
        public DateTime? modifyDate { get; set; }
        [DataMember]
        [Entity(FieldName = "modifyId", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 33)]
        public string modifyId { get; set; }
        [DataMember]
        [Entity(FieldName = "modifyName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 34)]
        public string modifyName { get; set; }
        [DataMember]
        public List<EntityReportRecorde> lstReportRecord { get; set; }
        [DataMember]
        public int reportCount { get; set; }
        [DataMember]
        public int conventionCount { get; set; }
        [DataMember]
        public string gradeName { get; set; }

        public string strBirthday
        {
            get
            {
                if (birthday == null)
                    return "";
                else
                    return Function.Datetime(birthday).ToString("yyyy-MM-dd");
            }
        }

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
        public string age { get; set; }

        public static EnumCols Columns = new EnumCols();

        public class EnumCols
        {
            public string id = "id";
            public string gradeId = "gradeId";
            public string clientNo = "clientNo";
            public string clientName = "clientName";
            public string gender = "gender";
            public string birthday = "birthday";
            public string mobile = "mobile";
            public string telephone = "telephone";
            public string email = "email";
            public string qq = "qq";
            public string cardNo = "cardNo";
            public string company = "company";
            public string regionId = "regionId";
            public string address = "address";
            public string booldType = "booldType";
            public string profession = "profession";
            public string marriage = "marriage";
            public string ehtnicGroup = "ehtnicGroup";
            public string eduationLevel = "eduationLevel";
            public string clientTag = "clientTag";
            public string contactName = "contactName";
            public string contactNameMobile = "contactNameMobile";
            public string dataSource = "dataSource";
            public string upTag = "upTag";
            public string serverDate = "serverDate";
            public string bakfileld1 = "bakfileld1";
            public string bakfileld2 = "bakfileld2";
            public string createDate = "createDate";
            public string creatorId = "creatorId";
            public string createName = "createName";
            public string modifyDate = "modifyDate";
            public string modifyId = "modifyId";
            public string modifyName = "modifyName";
        }
    }
}

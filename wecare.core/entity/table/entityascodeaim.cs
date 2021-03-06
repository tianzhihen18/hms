﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace weCare.Core.Entity
{
    /// <summary>
    /// AS_CODE_AIM
    /// </summary>
    [DataContract, Serializable]
    [EntityAttribute(TableName = "AS_CODE_AIM")]
    public class EntityAsCodeAim : BaseDataContract
    {
        /// <summary>
        /// AIM_CODE
        /// </summary>
        [DataMember]
        [EntityAttribute(FieldName = "AIM_CODE", DbType = DbType.AnsiString, IsPK = true, IsSeq = false, SerNo = 1)]
        public System.String aimCode { get; set; }

        /// <summary>
        /// AIM_NAME
        /// </summary>
        [DataMember]
        [EntityAttribute(FieldName = "AIM_NAME", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 2)]
        public System.String aimName { get; set; }

        /// <summary>
        /// T
        /// </summary>
        [DataMember]
        [EntityAttribute(FieldName = "T", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 3)]
        public System.String T { get; set; }

        /// <summary>
        /// ROOM_CODE
        /// </summary>
        [DataMember]
        [EntityAttribute(FieldName = "ROOM_CODE", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 4)]
        public System.String roomCode { get; set; }

        /// <summary>
        /// Columns
        /// </summary>
        public static EnumCols Columns = new EnumCols();

        /// <summary>
        /// EnumCols
        /// </summary>
        public class EnumCols
        {
            public string aimCode = "AIM_CODE";
            public string aimName = "AIM_NAME";
            public string T = "T";
            public string roomCode = "ROOM_CODE";
        }
    }

}

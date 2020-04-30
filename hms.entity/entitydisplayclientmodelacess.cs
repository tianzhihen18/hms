using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using weCare.Core.Entity;

namespace Hms.Entity
{
    public class EntityDisplayClientModelAcess : BaseDataContract
    {
        public string modelName { get; set; }
        public string modelScore { get; set; }
        public string modelResult { get; set; }
        public string modelScoreAndResult { get; set; }
        public string modelResultStr
        {
            get
            {
                if (modelResult == "1")
                    return "已患病";
                if (modelResult == "2")
                    return "低危";
                if (modelResult == "3")
                    return "中危";
                if (modelResult == "4")
                    return "高危";
                if (modelResult == "5")
                    return "很高危";
                if (modelResult == "6")
                    return "不评估";
                return "";
            }
         }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using weCare.Core.Dac;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Hms.Entity;
using Hms.Ui;

namespace peDataSys
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            sysDicPeItem();
        }

        #region 同步体检项目
        /// <summary>
        /// 同步体检项目
        /// </summary>
        public void sysDicPeItem()
        {
            List<EntityDicPeItem> data = null;
            //SqlHelper svcPe = new SqlHelper(EnumBiz.peDB);
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            List<DacParm> lstParm = new List<DacParm>();
            string Sql = @"select b.item_code,
                              b.item_name,
                              a.cls_code,
                              b.ref_lower,
                              b.ref_upper,
                              ref_result ,
                              b.sex ,
                              0 as displayposition,
                              b.unit
                              from zdXmfl a 
                              left join zdXm b
                              on a.cls_code = b.cls_code where item_code is not null and item_code not in(select itemid from dicPeItem)";

            try
            {
                DataTable dt = svc.GetDataTable(Sql);
                //Sql = @"delete from dicpeitem";
                //svc.ExecSql(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int n = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityDicPeItem vo = new EntityDicPeItem();
                        vo.itemId = dr["item_code"].ToString();
                        vo.itemName = dr["item_name"].ToString();
                        vo.deptId = dr["cls_code"].ToString();

                        if (dr["ref_lower"] != DBNull.Value)
                        {
                            if (Function.IsNumber(dr["ref_lower"].ToString()))
                                vo.minValue = Function.Dec(dr["ref_lower"]);
                        }

                        if (dr["ref_upper"] != DBNull.Value)
                        {
                            if (Function.IsNumber(dr["ref_upper"].ToString()))
                                vo.maxValue = Function.Dec(dr["ref_upper"]);
                        }

                        vo.refRange = dr["ref_result"].ToString();
                        if (dr["sex"].ToString().Contains("%") || dr["sex"] == DBNull.Value)
                            vo.gender = 3;
                        else if (dr["sex"].ToString().Trim() == "1")
                            vo.gender = 1;
                        else if (dr["sex"].ToString().Trim() == "2")
                            vo.gender = 2;
                        vo.unit = dr["unit"].ToString();
                        ++n;
                        vo.sortNo = n;
                        //data.Add(vo);


                        lstParm.Add(svc.GetInsertParm(vo));
                    }

                    if (svc.Commit(lstParm) > 0)
                    {
                        MessageBox.Show("success!");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                svc = null;
            }
        }
        #endregion


        public void sysReportDetail()
        {
            SqlHelper svcPe = new SqlHelper(EnumBiz.peDB);
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            List<DacParm> lstParm = new List<DacParm>();
            string Sql = @"select a.clus_code,a.clus_name,e.item_code,e.item_name from zdTc a 
                        left join zdTcmx b 
                        on a.clus_code  = b.clus_code
                        left join zdZhxm c 
                        on b.comb_code = c.comb_code
                        left join zdXmfl d
                        on c.cls_code = d.cls_code
                        left join zdXm e 
                        on d.cls_code = e.cls_code where e.item_code is not null";
            try
            {
                DataTable dt = svcPe.GetDataTable(Sql);
                Sql = @"delete from dicRptTemplateConfig";
                svc.ExecSql(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int n = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityRpttemplateConfig vo = new EntityRpttemplateConfig();

                        //data.Add(vo);
                        vo.templateId = dr["clus_code"].ToString();
                        vo.itemCode = dr["item_code"].ToString();

                        //lstParm.Add(svc.GetInsertParm(vo));
                        Sql = @"insert into dicRptTemplateConfig (templateId,itemCode) values('{0}','{1}')";
                        Sql = string.Format(Sql, vo.templateId, vo.itemCode);
                        svc.ExecSql(Sql);
                    }


                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                svc = null;
                MessageBox.Show("complete!");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            sysReportDetail();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SqlHelper svcOra = new SqlHelper(EnumBiz.interfaceDB);
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            try
            { 
                List<DacParm> lstParm = new List<DacParm>();
                int affectRows = -1;
                string Sql = @"select * from diet_principle";

                DataTable dt = svcOra.GetDataTable(Sql);
                Sql = @"delete from dietPrinciple";
                svc.ExecSql(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int n = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityDietPrinciple vo = new EntityDietPrinciple();

                        vo.principleId = svc.GetNextID("dietPrinciple", "dietPrincipleId").ToString().PadLeft(6, '0');
                        vo.principleName = dr["names"].ToString();
                        vo.contents = dr["contents"].ToString();
                        vo.createDate = DateTime.Now;
                        vo.createUserId = "00";


                        lstParm.Add(svc.GetInsertParm(vo));
                    }
                    if(lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);

                    if(affectRows > 0)
                    {
                        MessageBox.Show("success!");
                    }


                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                svc = null;
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            SqlHelper svcOra = new SqlHelper(EnumBiz.interfaceDB);
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                int affectRows = -1;
                string Sql = @"select * from diet_template_type";

                DataTable dt = svcOra.GetDataTable(Sql);
                Sql = @"delete from dietTemplatetype";
                svc.ExecSql(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityDietTemplatetype vo = new EntityDietTemplatetype();

                        vo.typeId = svc.GetNextID("dietTemplateType", "typeId").ToString().PadLeft(6, '0');
                        vo.typeName = dr["template_name"].ToString();
                        vo.createDate = DateTime.Now;
                        vo.creator = "00";
                        vo.createName = "系统管理员";

                        lstParm.Add(svc.GetInsertParm(vo));
                    }
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);

                    if (affectRows > 0)
                    {
                        MessageBox.Show("success!");
                    }


                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                svc = null;
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            
            
            
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {

            SqlHelper svcOra = new SqlHelper(EnumBiz.interfaceDB);
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                int affectRows = -1;
                string Sql = @"select * from cai";

                DataTable dt = svcOra.GetDataTable(Sql);
                Sql = @"delete from diccai";
                svc.ExecSql(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityDicCai vo = new EntityDicCai();

                        vo.id = dr["id"].ToString();
                        vo.names = dr["names"].ToString();
                        vo.breakfast = dr["breakfast"].ToString();
                        vo.lunch = dr["lunch"].ToString();
                        vo.dinner = dr["dinner"].ToString();
                        vo.other = dr["other"].ToString();
                        vo.methods = dr["methods"].ToString();
                        vo.kCal = Function.Dec( dr["kCal"].ToString());
                        vo.KJ = Function.Dec(dr["KJ"].ToString());
                        vo.PROTEIN = Function.Dec(dr["PROTEIN"].ToString());
                        vo.FAT = Function.Dec(dr["FAT"].ToString());
                        vo.CHO = Function.Dec(dr["CHO"].ToString());
                        vo.BRXXW = Function.Dec(dr["BRXXW"].ToString());
                        vo.DGC = Function.Dec(dr["DGC"].ToString());
                        vo.ASH = Function.Dec(dr["ASH"].ToString());
                        vo.vitaminA = Function.Dec(dr["vitaminA"].ToString());
                        vo.THIAMIN = Function.Dec(dr["THIAMIN"].ToString());
                        vo.RIBOFLAVIN = Function.Dec(dr["RIBOFLAVIN"].ToString());
                        vo.NIACIN = Function.Dec(dr["NIACIN"].ToString());
                        vo.vitaminC = Function.Dec(dr["vitaminC"].ToString());
                        vo.vitaminE = Function.Dec(dr["vitaminE"].ToString());
                        vo.CA = Function.Dec(dr["CA"].ToString());
                        vo.P = Function.Dec(dr["P"].ToString());
                        vo.K = Function.Dec(dr["K"].ToString());
                        vo.NA = Function.Dec(dr["NA"].ToString());
                        vo.MG = Function.Dec(dr["MG"].ToString());
                        vo.FE = Function.Dec(dr["FE"].ToString());
                        vo.ZN = Function.Dec(dr["ZN"].ToString());
                        vo.SE = Function.Dec(dr["SE"].ToString());
                        vo.CU = Function.Dec(dr["CU"].ToString());
                        vo.MN = Function.Dec(dr["MN"].ToString());
                        vo.I = Function.Dec(dr["I"].ToString());
                        vo.F = Function.Dec(dr["F"].ToString());
                        vo.CR = Function.Dec(dr["CR"].ToString());
                        vo.MU = Function.Dec(dr["MU"].ToString());
                        vo.vitaminD = Function.Dec(dr["vitaminD"].ToString());
                        vo.vitaminB6 = Function.Dec(dr["vitaminB6"].ToString());
                        vo.vitaminB12 = Function.Dec(dr["vitaminB12"].ToString());
                        vo.vitaminB5 = Function.Dec(dr["vitaminB5"].ToString());
                        vo.vitaminB9 = Function.Dec(dr["vitaminB9"].ToString());
                        vo.DANJIAN = Function.Dec(dr["DANJIAN"].ToString());
                        vo.vitaminH = Function.Dec(dr["vitaminH"].ToString());
                        vo.bakfield1 = dr["bakfield1"].ToString();
                        vo.bakfield2 = dr["bakfield2"].ToString();
                        vo.createDate = Function.Datetime(dr["createDate"].ToString());
                        vo.creator = "00";
                        vo.creatorName = "系统管理员";
                        

                        lstParm.Add(svc.GetInsertParm(vo));
                    }
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);

                    if (affectRows > 0)
                    {
                        MessageBox.Show("success!");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                svc = null;
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                int affectRows = -1;
                List<string> lstStr = new List<string>{"海带汤",
"烤茄串",
"酸辣乌鱼蛋汤",
"果汁肉干",
"麻茸鹌鹑蛋",
"豉油皇烙鸡肫",
"山楂冻",
"拌鱼丝",
"蚕豆松",
"奶油苹果",
"糯米蒸桃脯",
"川味回锅肉",
"梅汁泡河蚬",
"蛋蓉玉米羹",
"咸蛋黄拌豆腐",
"脆松糖",
"草莓果冻",
"芝麻红薯饼",
"辣拌芥蓝菜",
"辣油耳丝",
"藕香露",
"姜糖饮",
"大枣养血汤",
"红枣花生汁",
"韭菜生姜汁",
"咸蛋羹",
"葱白香菇人乳汤",
"白肉鱼泥",
"干荔枝煲粥",
"腌芥菜头",
"酥皮家常饼",
"金线吊葫芦",
"冰拌哈密瓜",
"糖拌葡萄",
"雪绵豆沙",
"五仁炸盒子",
"蛋黄茶碗蒸",
"苹果粥",
"甜豆浆",
"蔬菜云吞汤",
"黄金菇炖椰奶",
"红枣玉玉粥",
"蜇皮拌白菜",
"酱拌菠菜",
"菠菜拌猪肝",
"佛手海蜇",
"葱油豆腐",
"榨菜拌腐皮",
"番茄拌白菜心",
"糖拌西红柿",
"糖拌梨丝",
"青椒炒豆腐丁",
"红薯蒸板栗",
"夹沙豆腐",
"蜜汁红枣",
"葱白红枣汤",
"双雪汤",
"莲子淮山炖薏米",
"鲜莲汤",
"绿豆薏苡仁汤",
"卤味鲜海带",
"卤猪血糕",
"烫空心菜",
"芝麻南瓜饼",
"白灼鲜鱿鱼",
"豉椒牛柳",
"凉拌豆腐",
"葱油豌豆",
"菠萝水果冻",
"川味辣豆花",
"烩菠萝羹",
"核桃仁豌豆羹",
"鹌鹑蛋奶露",
"鸡蓉奶油汤",
"醋煮鸡蛋",
"红炖羊肉",
"鲍翅炖鸡",
"燕窝炖双鸭",
"马蹄炖羊肉",
"胡椒炖猪肚",
"清炖猪肚",
"椒酱肉",
"北菇炖鸽",
"白片羊肉",
"冬笋牛肉煲",
"牛肚煲(二)",
"大肠鸡红煲",
"虾鱼酿豆腐煲",
"五香大肉煲",
"火腿荔芋煲",
"南乳猪手煲",
"芋头猪手煲",
"发菜圆蹄煲",
"糖醋黄花鱼煲",
"腐竹鲩鱼煲",
"鲫鱼煲",
"姜葱鲤鱼煲",
"鱼球粉丝绍菜煲",
"白鳝豉汁煲",
"瑶柱蒜子煲",
"牛蹄筋煲",
"芥菜白肉片",
"子姜皮蛋",
"矿泉水腌脆条",
"油焖尖椒",
"冰糖三色瓜球",
"卤鸡三件",
"五柳炸鸡",
"竹笙炒鸡片",
"咸鸭蛋蒸豆腐",
"京糕山药卷",
"香面炒牛肉",
"抓炒鱼条",
"豆粉生鱼",
"怪味兔肉丝",
"白瓜咸蛋汤",
"珊瑚菊花",
"三鲜酿黄瓜",
"蜜汁糯米藕",
"胡萝卜香橙色拉",
"鲜虾蟹柳色拉",
"糖醋马蹄",
"拌蛏子",
"双色萝卜",
"鹑蛋紫菜汤",
"素菜泥汤",
"花生仁拌金笋干",
"清拌白菜心",
"海参桂圆粥",
"兰花泥肠",
"核桃牛奶饮",
"酸辣扁豆",
"回锅蛋",
"莲子桂圆汤",
"朝天耳丝",
"柠檬汁拌水果",
"豆芽面筋汤",
"清热鲜菇银芽汤",
"咖喱滚鱼片",
"槌鱼",
"酱味牛肚",
"真不同熏肉",
"黑椒烤猪肉",
"红果肉干",
"凤凰鸡煎牛里脊",
"新式牛肉干",
"牛干巴",
"片羊肉",
"五香卤鸭",
"炒羊肚片",
"冷甜菜汤",
"蜜枣青豆泥",
"熘排骨",
"酸菜肉丝",
"味菜牛柳",
"胡萝卜羊肝粥",
"酱油龙须菜",
"花生辣酱",
"熘变蛋",
"炸熘仔鸡",
"炸溜子鸡块",
"烩豆干丝",
"素炒黄花菜",
"大蒜炖生鱼",
"酸菜熬土豆",
"番薯麦米粥",
"粉葛煲鲮鱼",
"冰花合桃露",
"冰花炖雪耳",
"熟藕",
"肉茸榨菜粉皮",
"青龙卧雪",
"红净鸡",
"清汤卧果",
"黄甫蛋",
"冬笋炖鸡",
"咖喱火鸡",
"牛奶煮平菇",
"香酥腰花",
"银耳番茄羹",
"清拌茄条",
"双蛋彩拼",
"甜酱杏仁",
"广州腊羊蹄筋",
"胡萝卜拌鸡丝",
"清暑豆汤",
"冻粉鸡丝",
"黄瓜丝皮蛋",
"佛手蜇卷",
"油拌辣白菜",
"大酱拌芹菜",
"炝冬笋油菜",
"冰糖山药羹",
"清蒸山药段",
"酱羊腱子",
"黄瓜拌白肉",
"黄瓜拌皮丝",
"拌口条",
"黄瓜拌猪肝",
"黄瓜拌腰片",
"生菜拌腰片",
"凉拌白肚",
"黄瓜拌肚丝",
"麻酱拌肚丝",
"海参大虾拌鸡肉",
"韭菜烤鸭",
"豆腐拌咸鸭蛋",
"虾皮拌松花蛋",
"双腐拌双蛋",
"姜拌皮蛋",
"黄瓜拌虾片",
"青椒虾皮",
"海米拌雪菜",
"韭香海蜇",
"蛋皮海蜇",
"蜇米瓜菜",
"香菜拌海螺",
"糖豆",
"拌蚕豆沙",
"辣油豇豆",
"香菜芽拌豆腐",
"绿豆芽拌豆腐",
"花生米拌香干",
"核桃丁拌香干",
"白菜心拌干丝",
"酥仁干丁",
"酱豉香干",
"蜜汁土豆",
"酱拌土豆",
"酱拌白菜",
"芥末拌白菜",
"蒜辣白菜丁",
"香肠拌菠菜",
"糖醋芥菜丝",
"辣豆雪里蕻",
"糖拌萝卜丝",
"甜萝卜",
"花生拌酱萝卜",
"蒸酱茄子",
"姜末拌黄瓜",
"糖姜片",
"蒜酱笋片",
"双椒拌藕片",
"姜丝拌藕片",
"糖拌花生米",
"琥珀花生",
"脆甜花生米",
"拌西瓜瓤",
"冰淇淋双色丁",
"蜜桃果泥",
"冰糖蜜桃",
"糖拌杨梅",
"密瓜刨冰",
"雪菜炒黄豆芽",
"咸菜炒粉皮",
"核桃泥",
"麻酱拌白菜",
"大众蘸酱菜",
"拌瓜菜",
"凉拌冬瓜片",
"爽口西瓜盅",
"蘸茄子",
"香辣四季豆",
"榨菜拌腐竹",
"柠檬泡藕",
"八宝瓤西红柿",
"苹果煎蛋饼",
"三味酸菜",
"糖醋蒜",
"酸菜蒸豆腐",
"蜜汁蒸山药",
"龙眼蒸芋泥",
"红枣糯米饭",
"红枣花生汤",
"山楂麦芽汤",
"凤梨山楂汤",
"雪梨鲜奶炖木瓜",
"豆腐青葱汤",
"红苋豆腐汤",
"酸枣汤",
"莲子黑枣小麦汤",
"榨菜笋丝汤",
"蜜枣薏米煲",
"绿茶杏仁汤",
"卤水花生米",
"蒸臭豆腐",
"炝麻线鳝鱼",
"麻辣三丝",
"蒸猪肝",
"青蒸茶鲫鱼",
"白饭鱼蒸蛋",
"红枣煲栗子",
"雪梨莲藕汁",
"芝麻猪蹄糊",
"水果煎蛋",
"红烧五花肉",
"葡萄柠檬香醋饮",
"柠檬银耳冰醋饮",
"豆汁",
"炖鸭肾",
"梅菜扣肉煲",
"膏蟹煲",
"豆花鱼片",
"麻香口条",
"橙汁瓜脯",
"白切鸭",
"川味棒棒鸡",
"五味白肉",
"蜜汁鲜果",
"鲍鱼生菜",
"叉烤鸭",
"姜汁活蟹",
"核桃炖牛肉",
"青豆肉茸炒嫩蛋",
"油炒糖栗子",
"炖银耳肉",
"麻辣萝卜干",
"蒜茸蒸大竹蛏",
"凉拌皮蛋",
"松脆海蜇",
"肚丝拉皮",
"粉皮拌鸡丝",
"豆干牛肉丝",
"红椒皮蛋",
"拌鱿鱼丝",
"蒜茸海带",
"多味凉粉",
"凉拌芹菜叶",
"醋拌莴笋",
"糖拌莲菜",
"冰凉西瓜丁",
"香梨沙拉",
"糖拌双丝",
"拌金针菇",
"香干拌花生米",
"烧拌春笋",
"油汆臭豆腐",
"醋溜茭白",
"百合绿豆汤",
"百合汤",
"浸五香花生米",
"粉蒸芋球",
"凉拌莲藕",
"青蒜烧豆腐",
"辣乳拌腐竹",
"肉酱豆腐",
"煎荷包蛋",
"清蒸膏蟹",
"老厨白菜",
"煎熘腐皮卷",
"胡萝卜苹果醋蛋饮",
"绿茶番茄汤"
};

                string sqlSub = string.Empty;

                foreach(var str in lstStr)
                {
                    sqlSub += "'" + str + "',"; 
                }

                if (!string.IsNullOrEmpty(sqlSub))
                {
                    sqlSub = "(" + sqlSub.TrimEnd(',') + ")"; 
                }

                string Sql = @"select * from diccai a where a.names in " + sqlSub;

                DataTable dt = svc.GetDataTable(Sql);
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityDicCaiConfig vo = new EntityDicCaiConfig();
                        vo.caiSlaveId = "000022";
                        vo.caiId = dr["id"].ToString();

                        lstParm.Add(svc.GetInsertParm(vo));
                    }
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);

                    if (affectRows > 0)
                    {
                        MessageBox.Show("success!");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                svc = null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            this.gridLookUpEdit.Properties.DataSource = GetDicCaiRecipe();
           
            if (gridLookUpEdit1View.DataRowCount > 0)
            {
                MessageBox.Show("gridLookUpEdit1View.RowCount");
            }
        }

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

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                int affectRows = -1;
                int classifyId = 21;
                List<string> lstStr = new List<string>{"蜂蛹",
"蜂胚，蜂仔",
"蜂蛹（油蜂）",
"蜂蛹（大黄蜂）",
"葛根",
"花椒叶（干）",
"花粉（油菜）",
"花粉（油松）",
"松花粉",
"花粉（荞麦）",
"牛蛙",
"仙人掌（食用）",
"仙人掌果",
"仙桃",
"仙人掌籽",
"高丽参"};

                string sqlSub = string.Empty;

                foreach (var str in lstStr)
                {
                    sqlSub += "'" + str + "',";
                }

                if (!string.IsNullOrEmpty(sqlSub))
                {
                    sqlSub = "(" + sqlSub.TrimEnd(',') + ")";
                }

                string Sql = @"select * from dicDietIngredient a where a.names in " + sqlSub;

                DataTable dt = svc.GetDataTable(Sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityDicIngredientConfig vo = new EntityDicIngredientConfig();
                        vo.classifyId = classifyId;
                        vo.ingredientId = dr["id"].ToString();

                        lstParm.Add(svc.GetInsertParm(vo));
                    }
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);

                    if (affectRows > 0)
                    {
                        MessageBox.Show("success!");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                svc = null;
            }
        }

        private void gridLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void gridLookUpEdit_Click(object sender, EventArgs e)
        {
            if (gridLookUpEdit1View.RowCount > 0)
            {
                MessageBox.Show("gridLookUpEdit1View.RowCount");
            }
        }

        private void gridLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            
        }

        private void gridLookUpEdit1View_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            string caiSlaveStr = string.Empty;
            if (this.gridLookUpEdit1View.SelectedRowsCount > 0)
            {
                int[] selectArr = this.gridLookUpEdit1View.GetSelectedRows();
                for (int i = 0; i < selectArr.Length; i++)
                {
                    EntityDisplayDicCaiRecipe vo = (this.gridLookUpEdit1View.GetRow(selectArr[i]) as EntityDisplayDicCaiRecipe);

                    caiSlaveStr += vo.caiSlaveName + "、";
                }
            }
            if (!string.IsNullOrEmpty(caiSlaveStr))
                caiSlaveStr = caiSlaveStr.TrimEnd('、');
            this.textEdit1.Text = caiSlaveStr;
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            SqlHelper svcOra = new SqlHelper(EnumBiz.interfaceDB);
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                int affectRows = -1;
                string Sql = @"select * from diet_treatment";

                DataTable dt = svcOra.GetDataTable(Sql);
                Sql = @"delete from dicdiettreatment";
                svc.ExecSql(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityDietTreatment vo = new EntityDietTreatment();

                        vo.id = dr["id"].ToString();
                        vo.names = dr["names"].ToString();
                        vo.configs = dr["configs"].ToString();
                        vo.fuctions = dr["fuctions"].ToString();
                        vo.usage = dr["usage"].ToString();

                        vo.attention = dr["attention"].ToString();
                        vo.methods = dr["methods"].ToString();
                        vo.bakfield1 = dr["BAK_FIELD1"].ToString();
                        vo.bakfield2 = dr["BAK_FIELD2"].ToString();
                        vo.createDate = DateTime.Now;
                        vo.createName = "系统管理员";
                        vo.creator = "00";

                        lstParm.Add(svc.GetInsertParm(vo));
                    }
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);

                    if (affectRows > 0)
                    {
                        MessageBox.Show("success!");
                    }


                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                svc = null;
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            SqlHelper svcOra = new SqlHelper(EnumBiz.interfaceDB);
            SqlHelper svc = new SqlHelper(EnumBiz.onlineDB);
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                int affectRows = -1;
                string Sql = @"select * from client_info";

                DataTable dt = svcOra.GetDataTable(Sql);
                Sql = @"delete from clientInfo";
                svc.ExecSql(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityClientInfo vo = new EntityClientInfo();;
                        vo.id = dr["ID"].ToString();
                        vo.gradeId = dr["GRADE_ID"].ToString();
                        vo.clientNo = dr["CLIENT_NO"].ToString();
                        vo.clientName = dr["CLIENT_NAME"].ToString();
                        vo.gender = Function.Int( dr["GENDER"]);
                        vo.birthday = Function.Datetime(dr["BIRTHDAY"]);
                        vo.mobile = dr["MOBILE"].ToString();
                        vo.telephone = dr["TELEPHONE"].ToString();
                        vo.email = dr["EMAIL"].ToString();
                        vo.qq = dr["QQ"].ToString();
                        vo.cardNo = dr["CARDNO"].ToString();
                        vo.company = dr["COMPANY"].ToString();
                        vo.regionId = dr["REGION_ID"].ToString();
                        vo.address = dr["ADDRESS"].ToString();
                        vo.booldType = Function.Int(dr["BOOLD_TYPE"]);
                        vo.profession = Function.Int(dr["PROFESSION"]);
                        vo.marriage = Function.Int(dr["MARRIAGE"]);
                        vo.ehtnicGroup = Function.Int(dr["ETHNIC_GROUP"]);
                        vo.eduationLevel = Function.Int(dr["EDUCATION_LEVEL"]);
                        vo.clientTag = dr["CLIENT_TAG"].ToString();
                        vo.contactName = dr["CONTACT_NAME"].ToString();
                        vo.contactNameMobile = dr["CONTACT_NAME_MOBILE"].ToString();
                        vo.clientRemarks = dr["CLIENT_REMARKS"].ToString();
                        vo.dataSource = dr["DATA_SOURCE"].ToString();
                        vo.upTag = dr["UP_TAG"].ToString();
                        vo.serverDate = Function.Datetime(dr["SERVER_DATE"]);
                        vo.bakfileld1 = dr["BAK_FIELD1"].ToString();
                        vo.bakfileld2 = dr["BAK_FIELD2"].ToString();
                        vo.createDate = Function.Datetime(dr["CREATE_ON"]);
                        vo.creatorId = "00";
                        vo.createName = "系统管理员";
                        lstParm.Add(svc.GetInsertParm(vo));
                    }
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);

                    if (affectRows > 0)
                    {
                        MessageBox.Show("success!");
                    }


                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                svc = null;
            }
        }
    }
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Hms.Entity;
using Hms.Itf;
using weCare.Core;
using Hms.Ui;
using weCare.Core.Entity;
using System.Collections.Generic;
using weCare.Core.Utils;
using Hms.Biz;

namespace peDataSys
{
    public partial class XtraTest : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraTest()
        {
            InitializeComponent();
            AddQuestCtrl();
        }

        List<string> lstQuest = new List<string>() { "quest01", "quest02", "quest03", "quest04", "quest05", "quest06", "quest07", "quest08", "quest09", "quest10" };
        Dictionary<int, string> dicXmlData = new Dictionary<int, string>();
        Dictionary<int, IQuest> dicQuestCtrl { get; set; }

        public List<EntityClientInfo> lstClientInfo { get; set; }

        List<EntityDicQnSetting> lstTopic = null;
        List<EntityCtrlLocation> lstCtrlLocation = null;
        List<EntityDicQnSetting> lstItems = null;
        List<EntityQnRecord> lstQnRecords { get; set; }

        #region AddQuestCtrl
        /// <summary>
        /// AddQuestCtrl
        /// </summary>
        void AddQuestCtrl()
        {
            try
            {
                //this.plUserCtrl.Controls.Clear();
                using (Biz202 biz = new Biz202())
                {
                    lstCtrlLocation = biz.GetQnCtrlLocation(lstQuest[0]);
                    lstQnRecords = biz.GetQnRecords();
                }

                using (Biz209 biz = new Biz209())
                {
                    lstTopic = new List<EntityDicQnSetting>();
                    lstItems = new List<EntityDicQnSetting>();
                    biz.GetQnCustom(1, out lstTopic, out lstItems);
                }
                int locationX = 0;
                int locationY = 0;
                int idx = 0;
                int glbLocationY = 0;

                if (idx == 0)
                {
                    if (lstCtrlLocation != null && lstCtrlLocation.Count > 0)
                    {
                        foreach (var clVo in lstCtrlLocation)
                        {
                            if (clVo.name.Contains("FT"))
                            {
                                DevExpress.XtraReports.UI.XRLabel lblTopic = new DevExpress.XtraReports.UI.XRLabel();
                                lblTopic.Name = clVo.name;
                                lblTopic.Text = clVo.text;
                                lblTopic.Font = new System.Drawing.Font("宋体", 9.5F);
                                locationX = clVo.locationX;
                                locationY = clVo.locationY;
                                lblTopic.LocationFloat = new DevExpress.Utils.PointFloat(locationX, locationY);
                                //this.plUserCtrl.Controls.Add(lblTopic);
                                xrTable1.Rows[0].Cells[0].Controls.Add(lblTopic);
                            }
                            if (clVo.name.Contains("FM"))
                            {
                                DevExpress.XtraReports.UI.XRLabel lblTopic = new DevExpress.XtraReports.UI.XRLabel();
                                lblTopic.Name = clVo.name;
                                lblTopic.Text = clVo.text;
                                lblTopic.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold);
                                locationX = clVo.locationX;
                                locationY = clVo.locationY;
                                lblTopic.LocationFloat = new DevExpress.Utils.PointFloat(locationX, locationY);
                                //this.plUserCtrl.Controls.Add(lblTopic);
                                xrTable1.Rows[0].Cells[0].Controls.Add(lblTopic);
                            }
                        }
                    }
                    int parentCount = 0;
                    List<EntityDicQnSetting> lstChildSettings = new List<EntityDicQnSetting>();
                    if (lstTopic != null && lstTopic.Count > 0)
                    {
                        for (int i = 0; i < lstTopic.Count; i++)
                        {
                            EntityDicQnSetting item = lstTopic[i];

                            #region quest01
                            if (item.questName == "quest01")
                            {
                                if (string.IsNullOrEmpty(item.parentFieldId))
                                {
                                    DevExpress.XtraReports.UI.XRLabel lblTopic = new DevExpress.XtraReports.UI.XRLabel();
                                    lblTopic.Name = item.fieldId;
                                    lblTopic.Text = lstTopic.Find(r => r.fieldId == item.fieldId).fieldName;
                                    lblTopic.Font = new System.Drawing.Font("宋体", 9.5F);
                                    EntityCtrlLocation ctrLocat = lstCtrlLocation.FindAll(r => r.type == 1)[parentCount];
                                    locationX = ctrLocat.locationX;
                                    locationY = ctrLocat.locationY;
                                    lblTopic.LocationFloat = new DevExpress.Utils.PointFloat(locationX, locationY);
                                    //this.plUserCtrl.Controls.Add(lblTopic);
                                    xrTable1.Rows[0].Cells[0].Controls.Add(lblTopic);

                                    lstChildSettings = lstTopic.FindAll(r => r.parentFieldId == item.fieldId);
                                    if (lstChildSettings.Count > 0)
                                    {
                                        foreach (var childVo in lstChildSettings)
                                        {
                                            DevExpress.XtraReports.UI.XRCheckBox chkAns = new DevExpress.XtraReports.UI.XRCheckBox();
                                            string strEndWith = childVo.fieldId.Substring(4, 2);
                                            chkAns.Text = "";
                                            chkAns.Font = new System.Drawing.Font("宋体", 9.5F);
                                            chkAns.Name = childVo.fieldId;
                                            EntityCtrlLocation ctrLocatChild = lstCtrlLocation.Find(r => r.name.Contains(ctrLocat.name) && r.type == 2 && r.name.EndsWith(strEndWith));
                                            locationX = ctrLocatChild.locationX;
                                            locationY = ctrLocatChild.locationY;
                                            chkAns.Dpi = 100F;
                                            chkAns.LocationFloat = new DevExpress.Utils.PointFloat(locationX, locationY);
                                            chkAns.SizeF = new System.Drawing.SizeF(15.625F, 23F);
                                            xrTable1.Rows[0].Cells[0].Controls.Add(chkAns);
                                        }
                                    }

                                    parentCount++;
                                }
                            }
                            #endregion
                        }
                    }
                }
                idx = 1;
                #region quest02
                if (idx == 1)
                {
                    using (Biz202 biz = new Biz202())
                    {
                        lstCtrlLocation = biz.GetQnCtrlLocation(lstQuest[1]);
                    }
                    int F35Count = 0;
                    int F35Row = (lstTopic.FindAll(r => r.fieldId.Contains("F035")).Count) / 6;
                    int F35Y = 0;
                    List<EntityDicQnSetting> lstChildSettings = new List<EntityDicQnSetting>();
                    if (lstTopic != null && lstTopic.Count > 0)
                    {
                        for (int i = 0; i < lstTopic.Count; i++)
                        {
                            EntityDicQnSetting item = lstTopic[i];
                            if (item.questName == lstQuest[idx])
                            {
                                if (item.fieldId.Contains("F035"))
                                {
                                    DevExpress.XtraReports.UI.XRCheckBox chkAns = new DevExpress.XtraReports.UI.XRCheckBox();
                                    chkAns.Text = item.fieldName;
                                    chkAns.Font = new System.Drawing.Font("宋体", 9.5F);
                                    chkAns.Name = item.fieldId;
                                    EntityCtrlLocation ctrLocat = lstCtrlLocation.FindAll(r => r.name.Contains("F035"))[F35Count];
                                    locationX = ctrLocat.locationX;
                                    locationY = ctrLocat.locationY + glbLocationY;
                                    F35Y += locationY;
                                    chkAns.Width = ctrLocat.width;
                                    chkAns.Height = ctrLocat.height;
                                    chkAns.Location = new System.Drawing.Point(locationX, locationY);
                                    xrTable1.Rows[0].Cells[0].Controls.Add(chkAns);
                                    F35Count++;
                                }
                            }
                        }

                        for (int i2 = 0; i2 < lstTopic.Count; i2++)
                        {
                            EntityDicQnSetting itemVo = lstTopic[i2];
                            if (itemVo.questName == lstQuest[idx])
                            {
                                if (string.IsNullOrEmpty(itemVo.parentFieldId) && !itemVo.fieldId.Contains("F035"))
                                {
                                    DevExpress.XtraReports.UI.XRLabel lblTopic = new DevExpress.XtraReports.UI.XRLabel();
                                    lblTopic.Name = itemVo.fieldId;
                                    lblTopic.Text = itemVo.fieldName;
                                    lblTopic.Font = new System.Drawing.Font("宋体", 9.5F);
                                    EntityCtrlLocation ctrLocat = lstCtrlLocation.Find(r => r.name == itemVo.fieldId);
                                    locationX = ctrLocat.locationX;
                                    locationY = ctrLocat.locationY - F35Y + 80 + glbLocationY;
                                    lblTopic.Location = new System.Drawing.Point(locationX, locationY);
                                    //this.plUserCtrl2.Controls.Add(lblTopic);
                                    xrTable1.Rows[0].Cells[0].Controls.Add(lblTopic);

                                    lstChildSettings = lstTopic.FindAll(r => r.parentFieldId == itemVo.fieldId);
                                    if (lstChildSettings.Count > 0)
                                    {
                                        foreach (var childVo in lstChildSettings)
                                        {
                                            DevExpress.XtraReports.UI.XRCheckBox chkAns = new DevExpress.XtraReports.UI.XRCheckBox();
                                            string strEndWith = childVo.fieldId.Substring(4, 2);
                                            chkAns.Text = childVo.fieldName;
                                            chkAns.Font = new System.Drawing.Font("宋体", 9.5F);
                                            chkAns.Name = childVo.fieldId;
                                            EntityCtrlLocation ctrLocatChild = lstCtrlLocation.Find(r => r.name == childVo.fieldId);
                                            locationX = ctrLocatChild.locationX;
                                            locationY = ctrLocatChild.locationY - F35Y + 80 + glbLocationY;
                                            chkAns.Width = ctrLocatChild.width;
                                            chkAns.Height = ctrLocatChild.height;
                                            chkAns.Location = new System.Drawing.Point(locationX, locationY);
                                            //this.plUserCtrl2.Controls.Add(chkAns);
                                            xrTable1.Rows[0].Cells[0].Controls.Add(chkAns);
                                        }
                                    }
                                }
                            }
                        }

                    }

                    if (lstCtrlLocation != null && lstCtrlLocation.Count > 0)
                    {
                        foreach (var clVo in lstCtrlLocation)
                        {
                            DevExpress.XtraReports.UI.XRLabel lblTopic = new DevExpress.XtraReports.UI.XRLabel();
                            lblTopic.Name = clVo.name;
                            lblTopic.Text = clVo.text;
                            lblTopic.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold);
                            locationX = clVo.locationX;
                            if (clVo.name == "FM0201")
                            {
                                locationY += clVo.locationY + glbLocationY;
                            }
                            else if (clVo.name.Contains("FM"))
                            {
                                locationY += clVo.locationY - F35Y + 80 + glbLocationY;
                            }
                            else
                                continue;

                            lblTopic.Location = new System.Drawing.Point(locationX, locationY);
                            //this.plUserCtrl2.Controls.Add(lblTopic);
                            xrTable1.Rows[0].Cells[0].Controls.Add(lblTopic);
                        }
                    }
                }

                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {

            }
        }
    }
}

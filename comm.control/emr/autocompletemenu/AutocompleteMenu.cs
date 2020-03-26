﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections;
using System.Linq;
using weCare.Core.Entity;
using weCare.Core.Utils;

namespace Common.Controls.Emr
{
    [ProvideProperty("AutocompleteMenu", typeof(Control))]
    public class AutocompleteMenu : Component, IExtenderProvider
    {
        private static readonly Dictionary<Control, AutocompleteMenu> AutocompleteMenuByControls =
            new Dictionary<Control, AutocompleteMenu>();
        private static readonly Dictionary<Control, ITextBoxWrapper> WrapperByControls =
            new Dictionary<Control, ITextBoxWrapper>();

        private ITextBoxWrapper targetControlWrapper;
        //private readonly Timer timer = new Timer();

        private IEnumerable<AutocompleteItem> sourceItems = new List<AutocompleteItem>();
        [Browsable(false)]
        public IList<AutocompleteItem> VisibleItems { get { return Host.ListView.VisibleItems; } private set { Host.ListView.VisibleItems = value;} }
        private Size maximumSize;

        /// <summary>
        /// Duration (ms) of tooltip showing
        /// </summary>
        public int ToolTipDuration
        {
            get { return Host.ListView.ToolTipDuration; }
            set { Host.ListView.ToolTipDuration = value; }
        }

        [Browsable(false)]
        public bool IsOpen
        {
            get 
            {
                return ListView.Visible;
            }
        }

        public AutocompleteMenu()
        {
            Host = new AutocompleteMenuHost(this);
            Host.ListView.ItemSelected += new EventHandler(ListView_ItemSelected);
            Host.ListView.ItemHovered += new EventHandler<HoveredEventArgs>(ListView_ItemHovered);
            VisibleItems = new List<AutocompleteItem>();
            Enabled = true;
            MaximumSize = new Size(180, 200);
            AutoPopup = true;

            SearchPattern = @"[^\s^\r^、]";
            MinFragmentLength = 2;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Host.Dispose();
            }
            base.Dispose(disposing);
        }

        void ListView_ItemSelected(object sender, EventArgs e)
        {
            OnSelecting();
        }

        void ListView_ItemHovered(object sender, HoveredEventArgs e)
        {
            OnHovered(e);
        }

        public void OnHovered(HoveredEventArgs e)
        {
            if (Hovered != null)
                Hovered(this, e);
        }

        [Browsable(false)]
        public int SelectedItemIndex { get { return Host.ListView.SelectedItemIndex; }
            internal set { Host.ListView.SelectedItemIndex = value; } 
        }

        internal AutocompleteMenuHost Host { get; set; }

        /// <summary>
        /// Called when user selected the control and needed wrapper over it.
        /// You can assign own Wrapper for target control.
        /// </summary>
        [Description("Called when user selected the control and needed wrapper over it. You can assign own Wrapper for target control.")]
        public event EventHandler<WrapperNeededEventArgs> WrapperNeeded;

        protected void OnWrapperNeeded(WrapperNeededEventArgs args)
        {
            if (WrapperNeeded != null)
                WrapperNeeded(this, args);
            if (args.Wrapper == null)
                args.Wrapper = TextBoxWrapper.Create(args.TargetControl);
        }

        ITextBoxWrapper CreateWrapper(Control control, ref bool flags)
        {
            if (WrapperByControls.ContainsKey(control))
            {
                flags = true;
                return WrapperByControls[control];
            }

            var args = new WrapperNeededEventArgs(control);
            OnWrapperNeeded(args);
            if (args.Wrapper != null)
                WrapperByControls[control] = args.Wrapper;

            return args.Wrapper;
        }

        /// <summary>
        /// Current target control wrapper
        /// </summary>
        [Browsable(false)]
        public ITextBoxWrapper TargetControlWrapper
        {
            get { return targetControlWrapper; }
            set { 
                targetControlWrapper = value;
                if (value != null && !WrapperByControls.ContainsKey(value.TargetControl))
                {
                    WrapperByControls[value.TargetControl] = value;
                    SetAutocompleteMenu(value.TargetControl, this);
                }
            }
        }

        /// <summary>
        /// Maximum size of popup menu
        /// </summary>
        [DefaultValue(typeof(Size), "180, 200")]
        [Description("Maximum size of popup menu")]
        public Size MaximumSize { 
            get { return maximumSize; }
            set { 
                maximumSize = value;
                (Host.ListView as Control).MaximumSize = maximumSize;
                (Host.ListView as Control).Size = maximumSize;
                Host.CalcSize();
            }
        }

        /// <summary>
        /// Font
        /// </summary>
        public Font Font
        {
            get { return (Host.ListView as Control).Font; }
            set { (Host.ListView as Control).Font = value; }
        }

        /// <summary>
        /// Left padding of text
        /// </summary>
        [DefaultValue(18)]
        [Description("Left padding of text")]
        public int LeftPadding
        {
            get {
                if (Host.ListView is AutocompleteListView)
                    return (Host.ListView as AutocompleteListView).LeftPadding;
                else
                    return 0;
            }
            set {
                if (Host.ListView is AutocompleteListView)
                    (Host.ListView as AutocompleteListView).LeftPadding = value;
            }
        }

        /// <summary>
        /// AutocompleteMenu will popup automatically (when user writes text). Otherwise it will popup only programmatically or by Ctrl-Space.
        /// </summary>
        [DefaultValue(true)]
        [Description("AutocompleteMenu will popup automatically (when user writes text). Otherwise it will popup only programmatically or by Ctrl-Space.")]
        public bool AutoPopup { get; set; }

        /// <summary>
        /// AutocompleteMenu will capture focus when opening.
        /// </summary>
        [DefaultValue(false)]
        [Description("AutocompleteMenu will capture focus when opening.")]
        public bool CaptureFocus { get; set; }

        /// <summary>
        /// Indicates whether the component should draw right-to-left for RTL languages.
        /// </summary>
        [DefaultValue(typeof(RightToLeft), "No")]
        [Description("Indicates whether the component should draw right-to-left for RTL languages.")]
        public RightToLeft RightToLeft {
            get { return Host.RightToLeft; }
            set { Host.RightToLeft = value; }
        }

        /// <summary>
        /// Image list
        /// </summary>
        public ImageList ImageList { 
            get { return Host.ListView.ImageList; }
            set { Host.ListView.ImageList = value; }
        }

        /// <summary>
        /// Fragment
        /// </summary>
        [Browsable(false)]
        public Range Fragment { get; internal set; }

        /// <summary>
        /// Regex pattern for serach fragment around caret
        /// </summary>
        [Description("Regex pattern for serach fragment around caret")]
        [DefaultValue(@"[\w\.]")]
        public string SearchPattern { get; set; }

        /// <summary>
        /// Minimum fragment length for popup
        /// </summary>
        [Description("Minimum fragment length for popup")]
        [DefaultValue(2)]
        public int MinFragmentLength { get; set; }

        /// <summary>
        /// Allows TAB for select menu item
        /// </summary>
        [Description("Allows TAB for select menu item")]
        [DefaultValue(false)]
        public bool AllowsTabKey { get; set; }

        /// <summary>
        /// Interval of menu appear (ms)
        /// </summary>
        //[Description("Interval of menu appear (ms)")]
        //[DefaultValue(500)]
        //public int AppearInterval { get; set; }

        [DefaultValue(null)]
        public string[] Items
        {
            get
            {
                if (sourceItems == null)
                    return null;
                var list = new List<string>();
                foreach (AutocompleteItem item in sourceItems)
                    list.Add(item.ToString());
                return list.ToArray();
            }
            set { SetAutocompleteItems(value); }
        }

        /// <summary>
        /// The control for menu displaying.
        /// Set to null for restore default ListView (AutocompleteListView).
        /// </summary>
        [Browsable(false)]
        public IAutocompleteListView ListView
        {
            get { return Host.ListView; }
            set
            {
                if (ListView != null)
                {
                    var ctrl = value as Control;
                    value.ImageList = ImageList;
                    ctrl.RightToLeft = RightToLeft;
                    ctrl.Font = Font;
                    ctrl.MaximumSize = ctrl.MaximumSize;
                }
                Host.ListView = value;
                Host.ListView.ItemSelected += new EventHandler(ListView_ItemSelected);
                Host.ListView.ItemHovered += new EventHandler<HoveredEventArgs>(ListView_ItemHovered);
            }
        }


        [DefaultValue(true)]
        public bool Enabled { get; set; }

        /// <summary>
        /// Updates size of the menu
        /// </summary>
        public void Update()
        {
            Host.CalcSize();
        }

        #region IExtenderProvider Members

        bool IExtenderProvider.CanExtend(object extendee)
        {
            //find  AutocompleteMenu with lowest hashcode
            if (Container != null)
                foreach (object comp in Container.Components)
                    if (comp is AutocompleteMenu)
                        if (comp.GetHashCode() < GetHashCode())
                            return false;
            //we are main autocomplete menu on form ...
            //check extendee as TextBox
            if (!(extendee is Control))
                return false;
            var temp = TextBoxWrapper.Create(extendee as Control);
            return temp != null;
        }

        public void SetAutocompleteMenu(Control control, AutocompleteMenu menu)
        {
            if (menu != null)
            {
                bool flags = false;
                var wrapper = menu.CreateWrapper(control, ref flags);
                if (wrapper == null) return;

                if (!flags)
                {
                    menu.SubscribeForm(wrapper);
                    AutocompleteMenuByControls[control] = this;

                    wrapper.LostFocus += menu.control_LostFocus;
                    wrapper.Scroll += menu.control_Scroll;
                    wrapper.KeyDown += menu.control_KeyDown;
                    wrapper.MouseDown += menu.control_MouseDown;
                    wrapper.TextChanged += menu.control_TextChanged;
                }
            }
            else
            {
                AutocompleteMenuByControls.TryGetValue(control, out menu);
                AutocompleteMenuByControls.Remove(control);
                ITextBoxWrapper wrapper = null;
                WrapperByControls.TryGetValue(control, out wrapper);
                WrapperByControls.Remove(control);
                if (wrapper != null && menu != null)
                {
                    wrapper.LostFocus -= menu.control_LostFocus;
                    wrapper.Scroll -= menu.control_Scroll;
                    wrapper.KeyDown -= menu.control_KeyDown;
                    wrapper.MouseDown -= menu.control_MouseDown;
                    wrapper.TextChanged -= menu.control_TextChanged;
                }
            }
        }

        void control_TextChanged(object sender, EventArgs e)
        {
            if (TargetControlWrapper != null)
                ShowAutocomplete(false);
        }

        #endregion

        /// <summary>
        /// User selects item
        /// </summary>
        [Description("Occurs when user selects item.")]
        public event EventHandler<SelectingEventArgs> Selecting;

        /// <summary>
        /// It fires after item was inserting
        /// </summary>
        [Description("Occurs after user selected item.")]
        public event EventHandler<SelectedEventArgs> Selected;

        /// <summary>
        /// It fires when item was hovered
        /// </summary>
        [Description("Occurs when user hovered item.")]
        public event EventHandler<HoveredEventArgs> Hovered;

        /// <summary>
        /// Occurs when popup menu is opening
        /// </summary>
        public event EventHandler<CancelEventArgs> Opening;


        private System.Windows.Forms.Form myForm;

        void SubscribeForm(ITextBoxWrapper wrapper)
        {
            if (wrapper == null) return;
            var form = wrapper.TargetControl.FindForm();
            if (form == null) return;
            if (myForm != null)
            {
                if (myForm == form)
                    return;
                UnsubscribeForm(wrapper);
            }

            myForm = form;

            form.LocationChanged += new EventHandler(form_LocationChanged);
            form.ResizeBegin += new EventHandler(form_LocationChanged);
            form.FormClosing += new FormClosingEventHandler(form_FormClosing);
            form.LostFocus += new EventHandler(form_LocationChanged);
        }

        void UnsubscribeForm(ITextBoxWrapper wrapper)
        {
            if (wrapper == null) return;
            var form = wrapper.TargetControl.FindForm();
            if (form == null) return;

            form.LocationChanged -= new EventHandler(form_LocationChanged);
            form.ResizeBegin -= new EventHandler(form_LocationChanged);
            form.FormClosing -= new FormClosingEventHandler(form_FormClosing);
            form.LostFocus -= new EventHandler(form_LocationChanged);
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Close();
        }

        private void form_LocationChanged(object sender, EventArgs e)
        {
            Close();
        }

        private void control_MouseDown(object sender, MouseEventArgs e)
        {
            Close();
        }

        public ITextBoxWrapper FindWrapper(Control sender)
        {
            while (sender != null)
            {
                if (WrapperByControls.ContainsKey(sender))
                    return WrapperByControls[sender];

                sender = sender.Parent;
            }

            return null;
        }

        private void control_KeyDown(object sender, KeyEventArgs e)
        {
            TargetControlWrapper = FindWrapper(sender as Control);

            bool backspaceORdel = e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete;

            if (Host.Visible)
            {
                if (ProcessKey((char)e.KeyCode, Control.ModifierKeys))
                    e.SuppressKeyPress = true;

                return;
            }

            if (!Host.Visible)
                if (Control.ModifierKeys == (Keys.Control | Keys.Shift) && e.KeyCode == Keys.Space)
                {
                    ShowAutocomplete(true);
                    e.SuppressKeyPress = true;
                    return;
                }
        }

        private void control_Scroll(object sender, ScrollEventArgs e)
        {
            Close();
        }

        private void control_LostFocus(object sender, EventArgs e)
        {
            if (!Host.Focused) Close();
        }

        public AutocompleteMenu GetAutocompleteMenu(Control control)
        {
            if (AutocompleteMenuByControls.ContainsKey(control))
                return AutocompleteMenuByControls[control];
            else
                return null;
        }

        bool forcedOpened = false;

        internal void ShowAutocomplete(bool forced)
        {
            if (forced)
                forcedOpened = true;

            if (!Enabled)
            {
                Close();
                return;
            }

            if (!forcedOpened && !AutoPopup)
            {
                Close();
                return;
            }

            //build list
            BuildAutocompleteList(forcedOpened);

            //show popup menu
            if (VisibleItems.Count > 0)
            {
                ShowMenu();
            }
            else
            {
                Close();
            }
        }

        private void ShowMenu()
        {
            if (!Host.Visible)
            {
                var args = new CancelEventArgs();
                OnOpening(args);
                if (!args.Cancel)
                {
                    //calc screen point for popup menu
                    Point point = TargetControlWrapper.GetPositionFromCharIndex(Fragment.Start);

                    point = TargetControlWrapper.TargetControl.PointToScreen(point);
                    ToolStripDropDownDirection tsddd = ToolStripDropDownDirection.Default;

                    if (point.Y + Host.Size.Height > Screen.PrimaryScreen.Bounds.Height - 60)
                    {
                        tsddd = ToolStripDropDownDirection.AboveRight;
                        point = TargetControlWrapper.GetPositionFromCharIndex(Fragment.Start);
                        point.Offset(2,10);
                    }
                    else
                    {
                        point = TargetControlWrapper.GetPositionFromCharIndex(Fragment.Start);
                        point.Offset(2, TargetControlWrapper.TargetControl.Font.Height + 10);
                    }
                    Host.evtHostSizeChanged += new dlgHandleHostSizeChanged(Host_evtHostSizeChanged);
                    Host.Show(TargetControlWrapper.TargetControl, point, tsddd);

                    if (CaptureFocus)
                    {
                        (Host.ListView  as Control).Focus();
                    }
                }
            }
            else
                (Host.ListView as Control).Invalidate();
        }

        void Host_evtHostSizeChanged()
        {
            Close();
            ShowAutocomplete(true);
        }


        private void BuildAutocompleteList(bool forced)
        {
            var visibleItems = new List<AutocompleteItem>();
            
            bool foundSelected = false;
            int selectedIndex = -1;
            //get fragment around caret

            Range fragment = GetFragment(SearchPattern);
            string text = fragment.Text;
            //
            
            if (sourceItems != null)
            if (forced || (text.Length >= MinFragmentLength))
            {
                Fragment = fragment;
                //build popup menu
                foreach (AutocompleteItem item in sourceItems)
                {
                    item.Parent = this;
                    CompareResult res = item.Compare(text);
                    if (res != CompareResult.Hidden)
                        visibleItems.Add(item);
                    if (res == CompareResult.VisibleAndSelected && !foundSelected)
                    {
                        foundSelected = true;
                        selectedIndex = visibleItems.Count - 1;
                    }
                }
            }
            visibleItems.Sort();
            VisibleItems = visibleItems;

            if (foundSelected)
                SelectedItemIndex = selectedIndex;
            else
                SelectedItemIndex = 0;
            
            Host.CalcSize();
        }

        internal void OnOpening(CancelEventArgs args)
        {
            if (Opening != null)
                Opening(this, args);
        }

        private Range GetFragment(string searchPattern)
        {
            var tb = TargetControlWrapper;

            //if (tb.SelectionLength > 0)
            //    return new Range(tb);

            string text = tb.Text;

            var regex = new Regex(searchPattern);
            var result = new Range(tb);

            int startPos = tb.SelectionStart;
            //go forward
            int i = startPos;
            while (i >= 0 && i < text.Length)
            {
                if (!regex.IsMatch(text[i].ToString()))
                    break;
                i++;
            }
            result.End = i;

            int intTemp = 0;
            ctlRichTextBox objRichTextBox = (ctlRichTextBox)tb.TargetControl;
            string strCaption = string.Empty;// objRichTextBox.FirstlineCaption;
            if (strCaption != null)
            {
                intTemp = strCaption.Length;
            }

            List<EntityDstInfo> lstDSTInfo = null;// objRichTextBox.m_lstDoubleStrikeThrough.FindAll(t => t.m_intEndIndex < startPos);
            if (lstDSTInfo != null && lstDSTInfo.Count > 0)
            {
                intTemp = Math.Max(intTemp, lstDSTInfo.Max((obj) => { return obj.EndIndex + 1; }));
            }

            //go backward
            i = startPos;
            while (i > intTemp && (i - 1) < text.Length)
            {
                if (!regex.IsMatch(text[i - 1].ToString()))
                    break;
                i--;
            }
            result.Start = i;

            return result;
        }

        public void Close()
        {
            Host.evtHostSizeChanged -= new dlgHandleHostSizeChanged(Host_evtHostSizeChanged);
            Host.Close();
            forcedOpened = false;
        }

        public void SetAutocompleteItems(IEnumerable<string> items)
        {
            var list = new List<AutocompleteItem>();
            if (items == null)
            {
                sourceItems = null;
                return;
            }
            foreach (string item in items)
                list.Add(new AutocompleteItem(item));
            SetAutocompleteItems(list);
        }

        public void SetAutocompleteItems(IEnumerable<AutocompleteItem> items)
        {
            sourceItems = items;
        }

        public void AddItem(string item)
        {
            AddItem(new AutocompleteItem(item));
        }

        public void AddItem(AutocompleteItem item)
        {
            if (sourceItems == null)
                sourceItems = new List<AutocompleteItem>();

            if (sourceItems is IList)
                (sourceItems as IList).Add(item);
            else
                throw new Exception("Current autocomplete items does not support adding");
        }

        /// <summary>
        /// Shows popup menu immediately
        /// </summary>
        /// <param name="forced">If True - MinFragmentLength will be ignored</param>
        public void Show(Control control, bool forced)
        {
            SetAutocompleteMenu(control, this);
            this.TargetControlWrapper = FindWrapper(control);
            ShowAutocomplete(forced);
        }

        internal virtual void OnSelecting()
        {
            if (SelectedItemIndex < 0 || SelectedItemIndex >= VisibleItems.Count)
                return;

            AutocompleteItem item = VisibleItems[SelectedItemIndex];
            var args = new SelectingEventArgs
                           {
                               Item = item,
                               SelectedIndex = SelectedItemIndex
                           };

            OnSelecting(args);

            if (args.Cancel)
            {
                SelectedItemIndex = args.SelectedIndex;
                (Host.ListView as Control).Invalidate(true);
                return;
            }

            if (!args.Handled)
            {
                Range fragment = Fragment;
                ApplyAutocomplete(item, fragment);
            }

            Close();

            var args2 = new SelectedEventArgs
                            {
                                Item = item,
                                Control = TargetControlWrapper.TargetControl
                            };
            item.OnSelected(args2);
            OnSelected(args2);
        }

        private void ApplyAutocomplete(AutocompleteItem item, Range fragment)
        {
            string newText = item.GetTextForReplace();
            //replace text of fragment
            fragment.Text = newText;
            fragment.TargetWrapper.TargetControl.Focus();
        }

        internal void OnSelecting(SelectingEventArgs args)
        {
            if (Selecting != null)
                Selecting(this, args);
        }

        public void OnSelected(SelectedEventArgs args)
        {
            if (Selected != null)
                Selected(this, args);
        }

        public void SelectNext(int shift)
        {
            SelectedItemIndex = Math.Max(0, Math.Min(SelectedItemIndex + shift, VisibleItems.Count - 1));

            (Host.ListView as Control).Invalidate();
        }

        public bool ProcessKey(char c, Keys keyModifiers)
        {
            var page = Host.Height / (Font.Height + 4);
            if (keyModifiers == Keys.None)
                switch ((Keys) c)
                {
                    case Keys.Down:
                        SelectNext(+1);
                        return true;
                    case Keys.PageDown:
                        SelectNext(+page);
                        return true;
                    case Keys.Up:
                        SelectNext(-1);
                        return true;
                    case Keys.PageUp:
                        SelectNext(-page);
                        return true;
                    case Keys.Enter:
                        OnSelecting();
                        return true;
                    case Keys.Tab:
                        if (!AllowsTabKey)
                            break;
                        OnSelecting();
                        return true;
                    case Keys.Left:
                    case Keys.Right:
                        Close();
                        return false;
                    case Keys.Escape:
                        Close();
                        return true;
                }

            return false;
        }
    }
}
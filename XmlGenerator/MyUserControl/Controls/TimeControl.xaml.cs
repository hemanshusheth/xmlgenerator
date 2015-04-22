using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FormProperty;
using Xceed.Wpf.Toolkit;

namespace MyUserControl.Controls
{
    /// <summary>
    /// Interaction logic for TimeControl.xaml
    /// </summary>
    public partial class TimeControl : IControl
    {
        private ComboBox comboBox;
        #region Constructors
        public TimeControl()
        {
            InitializeComponent();
            comboBox = new ComboBox(); 
            comboBox.Items.Add("Long Time");
            comboBox.Items.Add("Short Time");
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="d">A datetimepicker object</param>
        /// <param name="content"></param>
        public TimeControl(TimeControl d, string content)
        {
            InitializeComponent();
            mytimepicker.Height = d.mytimepicker.Height;
            mytimepicker.Width = d.mytimepicker.Width;
            mytimepicker.Value = DateTime.Now;
            mytimepicker.Format = content == "Long Time" ? TimeFormat.LongTime : TimeFormat.ShortTime;
        }

        #endregion

        #region MouseDragEvents
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsInMainPanel(e))
            {
                return;
            }
            
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Package the data.
                DataObject data = new DataObject();
                data.SetData("Object", this);

                // Inititate the drag-and-drop operation.
                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
            }
        }

        private bool IsInMainPanel(MouseEventArgs e)
        {
            TimeControl timeControl = e.Source as TimeControl;
            if (timeControl != null)
            {
                Grid grid = timeControl.Parent as Grid;
                if (grid != null)
                {
                    if(grid.Name.Equals("panel_UserControl"))
                    {
                        return false;
                    }
                    StackPanel stackPanel = grid.Parent as StackPanel;
                    if (stackPanel == null)
                    {
                        return true;
                    }
                    if (stackPanel.Name.Equals("panel_main"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }
        #endregion

        public IControl GenerateControl(string labelName)
        {
            TimeControl myDateTimePicker = new TimeControl(new TimeControl(), labelName);
            
            return myDateTimePicker;
        }

        public UIElement GetUIElement()
        {
            comboBox.Height = 25;
            comboBox.Width = 120;
            comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            comboBox.Loaded += SelectFirst;
            comboBox.Margin = new Thickness(5);
            return comboBox;
        }

        public string GetContentLabel()
        {
            return "Format";
        }

        public string GetContents()
        {
            return comboBox.SelectedItem.ToString();
        }

        public void SetContents(string value)
        {
            comboBox.SelectedItem = value == "Long Time" ? TimeFormat.LongTime : TimeFormat.ShortTime;
        }

        public IControl GenerateControl(FieldType fieldType,string labelName)
        {
           return new TimeControl(new TimeControl(),labelName);
        }

        private void SelectFirst(object sender ,RoutedEventArgs e)
        {
            if (e.Source != null)
                ((ComboBox)e.Source).SelectedIndex = 0;
        }
        public string GetTitle()
        {
            return "TimePicker Control";
        }
    }
}

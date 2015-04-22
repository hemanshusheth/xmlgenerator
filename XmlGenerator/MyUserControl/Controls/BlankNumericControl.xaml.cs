using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace MyUserControl.Controls
{
    /// <summary>
    /// Interaction logic for BlankNumericControl.xaml
    /// </summary>
    public partial class BlankNumericControl : IControl
    {
        private DecimalUpDown numericUpDown;
        #region Constructors
        public BlankNumericControl()
        {
            InitializeComponent();
            numericUpDown = new DecimalUpDown();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="b">A mybutton object</param>
        /// <param name="content"> </param>
        public BlankNumericControl(BlankNumericControl b, string content)
        {
            InitializeComponent();
            mydecimalupdown.Height = b.mydecimalupdown.Height;
            mydecimalupdown.Width = b.mydecimalupdown.Width;
            mydecimalupdown.Maximum = Convert.ToInt32(content);
        }
        #endregion

        #region MouseDragEvents
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsInMainPanel(e))
                return;

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
            BlankNumericControl blankNumericControl = e.Source as BlankNumericControl;
            if (blankNumericControl != null)
            {
                Grid grid = blankNumericControl.Parent as Grid;
                if (grid != null)
                {
                    StackPanel stackPanel = grid.Parent as StackPanel;
                    if (stackPanel != null && (stackPanel.Name.Equals("panel_main")))
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
            BlankNumericControl myNumericUpDown = new BlankNumericControl(new BlankNumericControl(), labelName);
            return myNumericUpDown;
        }

        public UIElement GetUIElement()
        {
            numericUpDown.Height = 25;
            numericUpDown.Width = 120;
            numericUpDown.HorizontalAlignment = HorizontalAlignment.Left;
            numericUpDown.Increment = 1;
            numericUpDown.Value = 0;
            numericUpDown.Margin = new Thickness(5);
            return numericUpDown;
        }

        public string GetContentLabel()
        {
            return " Set Maximum";
        }

        public string GetContents()
        {
            return numericUpDown.Value.ToString();
        }

        public void SetContents(string value)
        {
            numericUpDown.Value = Convert.ToDecimal(value);
        }
        public string GetTitle()
        {
            return "Numeric Control";
        }
    }
}

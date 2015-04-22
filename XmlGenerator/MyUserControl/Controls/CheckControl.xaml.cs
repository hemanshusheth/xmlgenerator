using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyUserControl.Controls
{
    /// <summary>
    /// Interaction logic for CheckControl.xaml
    /// </summary>
    public partial class CheckControl : IControl
    {
        private TextBox textBox;
        #region Constructors
        public CheckControl()
        {
            InitializeComponent();
            textBox = new TextBox();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="c">A mycheckbox object</param>
        /// <param name="content"> </param>
        public CheckControl(CheckControl c,string content)
        {
            InitializeComponent();
            mycheckbox.Height = c.mycheckbox.Height;
            mycheckbox.Width = c.mycheckbox.Width;
            mycheckbox.Content = content;
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
            CheckControl checkControl = e.Source as CheckControl;
            if (checkControl != null)
            {
                Grid grid = checkControl.Parent as Grid;
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
            CheckControl myCheckBox = new CheckControl(new CheckControl(), labelName);
            return myCheckBox;
        }

        public UIElement GetUIElement()
        {
            textBox.Height = 25;
            textBox.Width = 120;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.IsEnabled = false;
            textBox.Margin = new Thickness(5);
            return textBox;
        }

        public string GetContentLabel()
        {
            return "Contents";
        }

        public string GetContents()
        {
            return textBox.Text;
        }

        public void SetContents(string value)
        {
            textBox.Text = value;
        }

        public string GetTitle()
        {
            return "CheckBox Control";
        }
    }
}
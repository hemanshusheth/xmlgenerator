using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MyUserControl.Controls
{
    /// <summary>
    /// Interaction logic for ListBoxControl.xaml
    /// </summary>
    public partial class ListBoxControl : IControl
    {
        private TextBox textBox;

        #region Constructors
        public ListBoxControl()
        {
            InitializeComponent();
            textBox = new TextBox();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="l">A lisbox object</param>
        /// <param name="content"></param>
        public ListBoxControl(ListBoxControl l, string content)
        {
            InitializeComponent();
            mylistbox.Height = l.mylistbox.Height;
            mylistbox.Width = l.mylistbox.Width;
            FrameworkElement e = new FrameworkElement {Height = 10};
            string[] items = content.Split(',');
            foreach (var item in items)
            {
                mylistbox.Items.Add(item);
                mylistbox.Height += e.Height;
            }
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

                //Inititate the drag-and-drop operation.
                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
            }
        }

        private bool IsInMainPanel(MouseEventArgs e)
        {
            ListBoxControl listBoxControl = e.Source as ListBoxControl;
            if (listBoxControl != null)
            {
                Grid grid = listBoxControl.Parent as Grid;
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
            ListBoxControl myDropDownBox = new ListBoxControl(new ListBoxControl(), labelName);
            return myDropDownBox;
        }

        public UIElement GetUIElement()
        {
            textBox.Height = 25;
            textBox.Width = 120;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.Margin = new Thickness(5);
            return textBox;
        }

        public string GetContentLabel()
        {
            return "Items";
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
            return "ListBox Control";
        }
    }
}
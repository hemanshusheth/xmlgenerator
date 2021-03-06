﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyUserControl.Controls
{
    /// <summary>
    /// Interaction logic for DropListControl.xaml
    /// </summary>
    public partial class DropListControl :IControl
    {
        private TextBox textBox;
        
        #region Constructors
        public DropListControl()
        {
            InitializeComponent();
            textBox = new TextBox();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="d">A dropdown object</param>
        /// <param name="content"></param>
        public DropListControl(DropListControl d, string content)
        {
            InitializeComponent();
            mycombobox.Height = d.mycombobox.Height;
            mycombobox.Width = d.mycombobox.Width;
            string[] items = content.Split(',');
            foreach (var item in items)
            {
                mycombobox.Items.Add(item);
            }
            mycombobox.Loaded += SelectFirst;
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
            DropListControl dropListControl = e.Source as DropListControl;
            if (dropListControl != null)
            {
                Grid grid = dropListControl.Parent as Grid;
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

        public IControl GenerateControl(string content)
        {
            DropListControl myDropDownBox = new DropListControl(new DropListControl(), content);
            return myDropDownBox;
        }

        public UIElement GetUIElement()
        {
            
            textBox.Name = "Content";
            textBox.Height = 25;
            textBox.Width = 120;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.Margin = new Thickness(5);
            return textBox;
        }

        public string GetContents()
        {
            return textBox.Text;
        }

        public void SetContents(string value)
        {
            textBox.Text = value;
        }

        public string GetContentLabel()
        {
            return "Items";
        }

        public void SelectFirst(object sender, RoutedEventArgs e)
        {
            if(e.Source!=null)
                ((ComboBox) e.Source).SelectedIndex = 0;
        }
        public string GetTitle()
        {
            return "DropList Control";
        }
    }
}
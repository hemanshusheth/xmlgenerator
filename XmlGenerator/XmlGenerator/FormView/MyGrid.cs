using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using FormProperty;
using MyUserControl.Controls;
using Xceed.Wpf.Toolkit;
using Brushes = System.Windows.Media.Brushes;
using FontFamily = System.Windows.Media.FontFamily;

namespace XmlGenerator.FormView
{
    public class MyGrid
    {
        #region Fields
        private UIElement _element;
        private Grid _grid;
        
        #endregion

        #region Properties
        public UIElement Element
        {
            get { return _element; }
            set { _element = value; }
        }

        public Grid Grid
        {
            get { return _grid; }
            set { _grid = value; }
        }

        #endregion

        #region Constructor
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates grid for a given group
        /// </summary>
        /// <param name="group">A group with controls</param>        
        public void CreateGrid(Group group)
        {
            //Grid definition for each new form field
            Grid groupGrid = new Grid();
            groupGrid.Name = group.UniqueName;
            ColumnDefinition titleColumn = new ColumnDefinition();
            ColumnDefinition controlsColumn = new ColumnDefinition();
            ColumnDefinition labelColumn = new ColumnDefinition();
            ColumnDefinition selectorColumn = new ColumnDefinition();
            ColumnDefinition removeColumn = new ColumnDefinition();

            groupGrid.ColumnDefinitions.Add(titleColumn);
            groupGrid.ColumnDefinitions.Add(selectorColumn);
            groupGrid.ColumnDefinitions.Add(controlsColumn);
            groupGrid.ColumnDefinitions.Add(labelColumn);
            groupGrid.ColumnDefinitions.Add(removeColumn);

            titleColumn.Width = new GridLength(120, GridUnitType.Pixel);
            controlsColumn.Width = new GridLength(150, GridUnitType.Pixel);
            labelColumn.Width = new GridLength(80, GridUnitType.Pixel);
            selectorColumn.Width = new GridLength(25, GridUnitType.Pixel);
            removeColumn.Width = new GridLength(70, GridUnitType.Pixel);

            for (int i = 0; i < group.Property.Count; i++)
            {
                //add new row 
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(0,GridUnitType.Auto);
                groupGrid.RowDefinitions.Add(gridRow);

                if (group.Property.SelectorType != FieldType.NoType)
                {
                    //Adding selector to column 1 for i number of rows
                    if (group.Property.SelectorType == FieldType.CheckControl)
                    {
                        CheckControl checkbox = new CheckControl(new CheckControl(), string.Empty);
                        checkbox.Width = 50;
                        checkbox.VerticalAlignment = VerticalAlignment.Center;
                        Grid.SetColumn(checkbox, 1);
                        Grid.SetRow(checkbox, i);
                        groupGrid.Children.Add(checkbox);
                    }
                    else
                    {
                        RadioControl radioButton = new RadioControl(new RadioControl(), string.Empty);
                        radioButton.Width = 50;
                        radioButton.VerticalAlignment= VerticalAlignment.Center;
                        Grid.SetColumn(radioButton, 1);
                        Grid.SetRow(radioButton, i);
                        groupGrid.Children.Add(radioButton);
                    }
                }

                //Add label to column 0 ,all rows
                Label titleLabel = new Label();
                titleLabel.Content = group.Property.Title;
                titleLabel.Height = 25;
                titleLabel.Width = 120;
                titleLabel.FontWeight = FontWeights.Bold;
                titleLabel.VerticalAlignment= VerticalAlignment.Center;
                titleLabel.FontFamily = new FontFamily("Candara");
                titleLabel.FontSize = 12;

                Grid.SetColumn(titleLabel, 0);
                Grid.SetRow(titleLabel, 0);
                Grid.SetRowSpan(titleLabel, group.Property.Count);
                groupGrid.Children.Add(titleLabel);

                //Add label to column 3 ,all rows
                Button removeButton = new Button();
                removeButton.Content = "Remove";
                removeButton.Name = "RemoveButton";
                removeButton.Height = 25;
                removeButton.Width = 60;
                //removeButton.Click += RemoveRow;
                removeButton.HorizontalAlignment=HorizontalAlignment.Right;

                removeButton.Background = Brushes.Transparent;
                Grid.SetColumn(removeButton, 4);
                Grid.SetRow(removeButton, 0);
                Grid.SetRowSpan(removeButton, group.Property.Count);

                groupGrid.Children.Add(removeButton);

                string content = group.Property.Content;

                string label = (i >= group.Property.LabelNames.Length)
                                    ? string.Empty
                                    : group.Property.LabelNames[i];
                if(group.Property.Fieldtype == FieldType.RadioControl ||
                               group.Property.Fieldtype == FieldType.CheckControl)
                {
                    content = label;
                }
                
                //Adding usercontrol to column 2 for i number of rows
                IControl control = GenerateControlFromGroup(group);
                UIElement generatedControl = (UIElement)control.GenerateControl(content);

                if (generatedControl != null)
                {
                    Grid.SetColumn( generatedControl, 2);
                    Grid.SetRow( generatedControl, i);
                    groupGrid.Children.Add( generatedControl);
                }
                if (group.Property.Fieldtype != FieldType.RadioControl &&
                               group.Property.Fieldtype != FieldType.CheckControl)
                {
                    //Adding label for usecontrol to column 3 for i number of rows
                    Label fieldLabel = new Label();
                    fieldLabel.Content = label;
                    fieldLabel.VerticalAlignment = VerticalAlignment.Center;
                    Grid.SetColumn(fieldLabel, 3);
                    Grid.SetRow(fieldLabel, i);
                    Grid.SetRowSpan(fieldLabel, group.Property.Count);
                   
                    groupGrid.Children.Add(fieldLabel);
                    
                }
            }
            groupGrid.Background = Brushes.Linen;
            Grid = groupGrid;
        }

        /// <summary>
        /// Creates grid for TextBoxControl Popup
        /// </summary>
        /// <param name="count"></param>
        /// <param name="labelGrid"> </param>
        /// <returns></returns>
        public Grid CreateTextBox(int count,Grid labelGrid)
        {
            if (count == 0)
                return labelGrid;

            if(labelGrid.Children.Count > 4)
            {
                labelGrid.Children.RemoveRange(4,labelGrid.Children.Count -4);
            }

            for (int i = 1; i <= count; i++)
            {
                TextBox textLabel = new TextBox();
                TextBox textUniqueName = new TextBox();
                DecimalUpDown numericPrefferedWidth = new DecimalUpDown();
                DecimalUpDown numericMaxLen = new DecimalUpDown();

                numericPrefferedWidth.Value = 0;
                numericMaxLen.Value = 0;

                textUniqueName.Margin = new Thickness(5);
                textLabel.Margin = new Thickness(5);
                numericPrefferedWidth.Margin = new Thickness(5);
                numericMaxLen.Margin = new Thickness(5);
                
                textLabel.Name = "txtlabel"+i;
                textUniqueName.Name = "textUniqueName" + i;
                numericPrefferedWidth.Name = "numericPrefferedWidth" + i;
                numericMaxLen.Name = "numericMaxLen" + i;

                RowDefinition gridRow = new RowDefinition();
                labelGrid.RowDefinitions.Add(gridRow);

                Grid.SetColumn(textLabel, 0);
                Grid.SetColumn(textUniqueName, 1);
                Grid.SetColumn(numericPrefferedWidth, 2);
                Grid.SetColumn(numericMaxLen, 3);

                Grid.SetRow(textLabel, i);
                Grid.SetRow(textUniqueName, i);
                Grid.SetRow(numericPrefferedWidth, i);
                Grid.SetRow(numericMaxLen, i);

                labelGrid.Children.Add(textLabel);
                labelGrid.Children.Add(textUniqueName);
                labelGrid.Children.Add(numericPrefferedWidth);
                labelGrid.Children.Add(numericMaxLen);
            }
            return labelGrid;
        }

        /// <summary>
        /// Generate a user control from group object
        /// </summary>
        /// <param name="group">A Group</param>
        /// <returns>a generated user control</returns>
        public IControl GenerateControlFromGroup(Group group)
        {
            string controlPath = "MyUserControl.Controls." + group.Property.Fieldtype;
            return (IControl) Assembly.GetAssembly(typeof(IControl)).CreateInstance(controlPath);
        }

        #endregion

        /// <summary>
        /// Creates grid for timeControl Popup
        /// </summary>
        /// <param name="count"></param>
        /// <param name="labelGrid"></param>
        /// <returns></returns>
        internal Grid CreateTimeControl(int count, Grid labelGrid)
        {
            if (count == 0)
                return labelGrid;

            if (labelGrid.Children.Count > 4)
            {
                labelGrid.Children.RemoveRange(4, labelGrid.Children.Count - 4);
            }

            for (int i = 1; i <= count; i++)
            {
                TextBox textLabel = new TextBox();
                TextBox textUniqueName = new TextBox();
                CheckBox checkAutoNextDay = new CheckBox();
                TimePicker timeCurrentTime = new TimePicker();

                timeCurrentTime.Value = DateTime.Now;

                textUniqueName.Margin = new Thickness(5);
                textLabel.Margin = new Thickness(5);
                checkAutoNextDay.Margin = new Thickness(5);
                timeCurrentTime.Margin = new Thickness(5);

                textLabel.Name = "txtlabel" + i;
                textUniqueName.Name = "textUniqueName" + i;
                checkAutoNextDay.Name = "numericPrefferedWidth" + i;
                timeCurrentTime.Name = "numericMaxLen" + i;

                RowDefinition gridRow = new RowDefinition();
                labelGrid.RowDefinitions.Add(gridRow);

                Grid.SetColumn(textLabel, 0);
                Grid.SetColumn(textUniqueName, 1);
                Grid.SetColumn(checkAutoNextDay, 2);
                Grid.SetColumn(timeCurrentTime, 3);

                Grid.SetRow(textLabel, i);
                Grid.SetRow(textUniqueName, i);
                Grid.SetRow(checkAutoNextDay, i);
                Grid.SetRow(timeCurrentTime, i);

                labelGrid.Children.Add(textLabel);
                labelGrid.Children.Add(textUniqueName);
                labelGrid.Children.Add(checkAutoNextDay);
                labelGrid.Children.Add(timeCurrentTime);
            }
            return labelGrid;
        }

        internal Grid CreateDropListGrid(int count, Grid labelGrid)
        {
            if (count == 0)
                return labelGrid;

            if (labelGrid.Children.Count > 3)
            {
                labelGrid.Children.RemoveRange(3, labelGrid.Children.Count - 3);
            }

            for (int i = 1; i <= count; i++)
            {
                TextBox textLabel = new TextBox();
                TextBox textUniqueName = new TextBox();
                DecimalUpDown numericSelectedItem = new DecimalUpDown();

                numericSelectedItem.Value = 0;
                numericSelectedItem.Minimum = 0;
                numericSelectedItem.Maximum = 10;

                textUniqueName.Margin = new Thickness(5);
                textLabel.Margin = new Thickness(5);
                numericSelectedItem.Margin = new Thickness(5);

                textLabel.Name = "txtlabel" + i;
                textUniqueName.Name = "textUniqueName" + i;
                numericSelectedItem.Name = "textMessageValue" + i;

                RowDefinition gridRow = new RowDefinition();
                labelGrid.RowDefinitions.Add(gridRow);

                Grid.SetColumn(textLabel, 0);
                Grid.SetColumn(textUniqueName, 1);
                Grid.SetColumn(numericSelectedItem, 2);
               
                Grid.SetRow(textLabel, i);
                Grid.SetRow(textUniqueName, i);
                Grid.SetRow(numericSelectedItem, i);
             
                labelGrid.Children.Add(textLabel);
                labelGrid.Children.Add(textUniqueName);
                labelGrid.Children.Add(numericSelectedItem);
            }
            return labelGrid;
        }

        internal Grid CreateDropListItemsGrid(int count, Grid itemsGrid)
        {
            if (count == 0)
                return itemsGrid;

            if (itemsGrid.Children.Count > 3)
            {
                itemsGrid.Children.RemoveRange(3, itemsGrid.Children.Count - 3);
            }

            for (int i = 1; i <= count; i++)
            {
                TextBox textLabel = new TextBox();
                TextBox textUniqueName = new TextBox();
                TextBox textMessageValue = new TextBox();

                textUniqueName.Margin = new Thickness(5);
                textLabel.Margin = new Thickness(5);
                textMessageValue.Margin = new Thickness(5);

                textLabel.Name = "txtlabel" + i;
                textUniqueName.Name = "textUniqueName" + i;
                textMessageValue.Name = "textMessageValue" + i;

                RowDefinition gridRow = new RowDefinition();
                itemsGrid.RowDefinitions.Add(gridRow);

                Grid.SetColumn(textLabel, 0);
                Grid.SetColumn(textUniqueName, 1);
                Grid.SetColumn(textMessageValue, 2);

                Grid.SetRow(textLabel, i);
                Grid.SetRow(textUniqueName, i);
                Grid.SetRow(textMessageValue, i);

                itemsGrid.Children.Add(textLabel);
                itemsGrid.Children.Add(textUniqueName);
                itemsGrid.Children.Add(textMessageValue);
            }
            return itemsGrid;
        }
    }
}
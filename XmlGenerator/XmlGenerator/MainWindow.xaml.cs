using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FormProperty;
using MyUserControl.Controls;
using Xceed.Wpf.Toolkit;
using XmlGenerator.FormView;
using XmlGenerator.PopUp;
using Group = FormProperty.Group;

namespace XmlGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Fields
        private bool _isDown;
        private bool _isDragging;
        private Point _point;
        private Panel _realDragSource;
        private static Panel _mainpanel;
        private static ObservableCollection<Strategy> _strategyList;
        private static Strategy _currentStrategy;
        private static ComboBox _strategyCombobox;
        private static Group _workingGroup;
        
        #endregion

        #region Properties
        public static Panel Mainpanel
        {
            get { return _mainpanel; }
            set { _mainpanel = value; }
        }

        public static ObservableCollection<Strategy> StrategyList
        {
            get { return _strategyList; }
            set { _strategyList = value; }
        }

        public static Strategy CurrentStrategy
        {
            get { return _currentStrategy; }
            set { _currentStrategy = value; }
        }

        public static ComboBox StrategyCombobox
        {
            get { return _strategyCombobox; }
            set { _strategyCombobox = value; }
        }

        public static Group WorkingGroup
        {
            get { return _workingGroup; }
            set { _workingGroup = value; }
        }

        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            Mainpanel = panel_main;
            StrategyCombobox = mystrategycombobox;
            LoadStrategy();
            DataContext = this;
        }
        #endregion

        #region MouseEvents
        /// <summary>
        /// Performs action when Mouse Left button is pressed down. 
        /// Putting the left button is an indication that a drag event is going to begin on the panel_main
        /// Get the start position of the drag event
        /// </summary>
        /// <param name="sender">Panel_main</param>
        /// <param name="e">Mouse event arguments</param>
        private void PanelMain_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
             if (e.Source.GetType() == typeof(Button))
             {
                 Button removeButton = e.Source as Button;
                 if(removeButton != null && removeButton.Name.Equals("RemoveButton"))
                 {
                     RemoveRow(sender,e);
                 }
                 return;
             }

            _isDown = true;
            _point = e.GetPosition(Mainpanel);

            if (_point.X < 0 || _point.Y < 0)
            {
                ClearProperty();
                return;
            }
                
            try
            {
                Visual v = (Visual) e.Source;
                Grid temp = GetGridFromPoint(v);
                if (temp != null)
                {
                    if (CurrentStrategy.HasEntry(temp.Name))
                    {
                        ClearProperty(); 
                        Group group = CurrentStrategy.ReturnEntry(temp.Name);
                        WorkingGroup = group;
                        DisplayProperty(group);
                    }
                }
                else
                {
                    ClearProperty();
                }
            }
            catch (Exception)
            {

            }
        }

        private Grid GetGridFromPoint(Visual v)
        {
            Grid grid = new Grid();

            if (v.GetType() == typeof(Grid))
            {
                grid = v as Grid;
            }
            else if (v.GetType() == typeof(StackPanel))
            {
                StackPanel s = v as StackPanel;
                if (s != null)
                {
                    double height = 0;
                    foreach (var child in s.Children)
                    {
                        if (child.GetType() == typeof(Grid))
                        {
                            Grid tempGrid = child as Grid;
                            if (tempGrid != null)
                            {
                                height += tempGrid.ActualHeight;
                                double width = tempGrid.ActualWidth;

                                if (_point.X < width && _point.Y < height)
                                {
                                    grid = tempGrid;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Visual visual = (Visual)VisualTreeHelper.GetParent(v);
                if (visual.GetType() == typeof(Grid))
                {
                    grid = visual as Grid;
                }
                else
                {
                    grid = null;
                }
            }
            return grid;
        }

        public void RemoveRow(object sender, RoutedEventArgs e)
        {
            Grid grid = VisualTreeHelper.GetParent((UIElement)e.Source) as Grid;
            if (grid != null)
            {
                StackPanel panel = VisualTreeHelper.GetParent(grid) as StackPanel;
                if (Mainpanel.Equals(panel) && panel != null)
                {
                    ClearProperty();
                    Mainpanel.Children.Remove(grid);
                }
                CurrentStrategy.DeleteEntry(grid.Name);
            }
        }

        /// <summary>
        /// Performs action when Mouse Left button is released up. 
        /// Triggers when a drag object is released
        /// </summary>
        /// <param name="sender">Panel_main</param>
        /// <param name="e">Mouse event arguments</param>
        private void PanelMain_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_realDragSource != null)
            {
                _isDown = false;
                _isDragging = false;
                _realDragSource.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// triggered when the mouse is moved across the panel.
        /// To perform the drag drop of rows within the panel_main
        /// </summary>
        /// <param name="sender">panel_main</param>
        /// <param name="e">Mouse event arguements</param>
        private void PanelMain_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                try
                {
                    if ((_isDragging == false) &&
                        ((Math.Abs(e.GetPosition(panel_main).X - _point.X) >
                          SystemParameters.MinimumHorizontalDragDistance) ||
                         (Math.Abs(e.GetPosition(panel_main).Y - _point.Y) >
                          SystemParameters.MinimumVerticalDragDistance)))
                    {
                        _isDragging = true;
                        Panel panel = VisualTreeHelper.GetParent((UIElement) e.Source) as Panel;
                        //_realDragSource = (Panel) VisualTreeHelper.GetParent((UIElement) e.Source);

                        if (panel != null)
                        {
                            _realDragSource = panel;
                            _realDragSource.CaptureMouse();
                            DragDrop.DoDragDrop(this,new DataObject("UIElement",VisualTreeHelper.GetParent((UIElement) e.Source), true),
                                                DragDropEffects.Move);
                        }
                    }
                }
                catch(Exception)
                {
                    _isDragging = false;
                }
            }
        }
        ///// <summary>
        ///// Handle object entering the panel.
        ///// Copy the object if it is dragged from user control.
        ///// Move the object if it is dragged from the same panel.
        ///// do nothing for any other thing dragged
        ///// </summary>
        ///// <param name="sender">panel_main</param>
        ///// <param name="e">Mouse event arguements</param>
        //private void PanelMain_DragEnter(object sender, DragEventArgs e)
        //{
        //    if (sender == panel_usercontrols)
        //    {
        //        if (e.Data.GetDataPresent("UIElement"))
        //        {
        //            e.Effects = DragDropEffects.Move;
        //        }
        //        else if (e.Data.GetDataPresent("Object"))
        //        {
        //            e.Effects = DragDropEffects.Copy;
        //        }
        //        else
        //            e.Effects = DragDropEffects.None;
        //    }
        //}

        ///// <summary>
        ///// Handles event of drag event. 
        ///// Drag should be from User controls panel or Main Panel
        ///// For Main Panel drag should be Move
        ///// For User Control panel drag should be Copy
        ///// </summary>
        ///// <param name="sender">User controls panel or Main Panel</param>
        ///// <param name="e">Dragevent arguements</param>
        //private void PanelMain_DragOver(object sender, DragEventArgs e)
        //{
        //    if (sender == panel_usercontrols)
        //        e.Effects = DragDropEffects.Copy;
        //}

        /// <summary>
        /// Handles the Drop event on the main panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelMain_Drop(object sender, DragEventArgs e)
        {
            // If an element in the panel has already handled the drop,
            // the panel should not also handle it.
            if (e.Handled == false)
            {
                if (e.Data.GetDataPresent("UIElement"))
                {
                    Grid droptarget = VisualTreeHelper.GetParent((UIElement) e.Source) as Grid;
                    int droptargetIndex = -1, i = 0;
                    foreach (UIElement element in Mainpanel.Children)
                    {
                        if (element.Equals(droptarget))
                        {
                            droptargetIndex = i;
                            break;
                        }
                        i++;
                    }
                    if (droptargetIndex != -1)
                    {
                        Mainpanel.Children.Remove(_realDragSource);
                        Mainpanel.Children.Insert(droptargetIndex, _realDragSource);
                    }

                    _isDown = false;
                    _isDragging = false;
                    _realDragSource.ReleaseMouseCapture();
                    e.Effects=DragDropEffects.Move;
                }
                else if (e.Data.GetDataPresent("Object"))
                {
                    Panel _panel = (Panel) sender;
                    if (Mainpanel.Equals(_panel) && _panel!=null)
                    {
                        IControl _element = (IControl)e.Data.GetData("Object");
                        if (CurrentStrategy != null)
                        {
                            if (StrategyCombobox.SelectedIndex == -1)
                            {
                                StrategyCombobox.SelectedItem = CurrentStrategy;
                            }
                            if (_element != null)
                            {
                                FormPopUp popup = new FormPopUp(_element);
                                popup.ShowDialog();
                                e.Effects = DragDropEffects.Copy;
                            }
                        }
                        else
                        {
                            ShowError("Select a Strategy First");
                        }
                    }
                }
            }
        }

        #endregion

        #region PropertyWindow
        /// <summary>
        /// Clears the Property window
        /// </summary>
        public void ClearProperty()
        {
            WorkingGroup = null;
            uniqueNameTextBox.Text = string.Empty;
            labelNameTextBox.Text = string.Empty;
            groupNameTextBox.Text = string.Empty;
            var control = (from d in panel_property.Children.OfType<UIElement>()
                           where Grid.GetColumn(d) == 1 && Grid.GetRow(d) == 4
                           select d).FirstOrDefault();
            panel_property.Children.Remove(control);
            countComboBox.SelectedIndex = 0;
            selectorComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Displays the property window
        /// </summary>
        /// <param name="group"></param>
        private void DisplayProperty(Group group)
        {
            uniqueNameTextBox.Text = group.UniqueName;
            labelNameTextBox.Text = string.Join(",", group.Property.LabelNames);
            groupNameTextBox.Text = group.Property.Title;

            string controlPath = "MyUserControl.Controls." + group.Property.Fieldtype;
            IControl control = (IControl)Assembly.GetAssembly(typeof(IControl)).CreateInstance(controlPath);

            if (control != null)
            {
                itemTextBlock.Text = control.GetContentLabel();
                control.SetContents(group.Property.Content);
                UIElement uiElement = control.GetUIElement();
                SetSelectionChangeActionEvent(uiElement);

            }
            countComboBox.Loaded += countComboBox_Loaded;
            selectorComboBox.Loaded += selectorComboBox_Loaded;
            selectorComboBox.SelectedItem = group.Property.SelectorType;
            countComboBox.SelectedItem = group.Property.Count;
        }
       
        private void SetSelectionChangeActionEvent(UIElement uiElement)
        {
            if (uiElement.GetType() == typeof(DecimalUpDown))
            {
                DecimalUpDown decimalUpDown = uiElement as DecimalUpDown;
                if (decimalUpDown != null)
                {
                    decimalUpDown.ValueChanged += decimalUpDown_SelectionChanged;
                    Grid.SetColumn(decimalUpDown, 1);
                    Grid.SetRow(decimalUpDown, 4);
                    decimalUpDown.Width = Double.NaN;
                    decimalUpDown.Height = Double.NaN;
                    panel_property.Children.Add(decimalUpDown);
                }
            }
            else if (uiElement.GetType() == typeof(TextBox))
            {
                TextBox textBox = uiElement as TextBox;
                if (textBox != null)
                {
                    textBox.SelectionChanged += textBox_SelectionChanged;
                    Grid.SetColumn(textBox, 1);
                    Grid.SetRow(textBox, 4);
                    textBox.Width = 100;
                    textBox.Height = Double.NaN;
                    panel_property.Children.Add(textBox);
                }
            }
            else if (uiElement.GetType() == typeof(ComboBox))
            {
                ComboBox comboBox = uiElement as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectionChanged += ComboBox_selectionChanged;
                    Grid.SetColumn(comboBox, 1);
                    Grid.SetRow(comboBox, 4);
                    comboBox.Width = Double.NaN;
                    comboBox.Height = Double.NaN;
                    panel_property.Children.Add(comboBox);
                }
            }
        }
        private void countComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            countComboBox.ItemsSource = new ArrayList
                                            {
                                                1,2,3,4,5,6,7,8,9,10
                                            };
        }

        private void selectorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            selectorComboBox.ItemsSource = new ArrayList
                                            {
                                                FieldType.NoType,FieldType.RadioControl,FieldType.CheckControl
                                            };
        }

        private void uniqueNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = e.Source as TextBox;
            if (WorkingGroup != null)
            {
                if (textBox != null && WorkingGroup.UniqueName != textBox.Text)
                {
                    if (String.IsNullOrEmpty(textBox.Text))
                    {
                        ShowError("Unique Name Cannot be blank");
                        uniqueNameTextBox.Text = WorkingGroup.UniqueName;
                        return;
                    }

                    if (!Regex.IsMatch(textBox.Text, "^[a-zA-Z0-9_]+$") && !textBox.Text.StartsWith("_"))
                    {
                        ShowError("Unique Name cannot contains Special Chars");
                        uniqueNameTextBox.Text = WorkingGroup.UniqueName;
                        return;
                    }
                    if (!CurrentStrategy.HasEntry(textBox.Text))
                    {
                        CurrentStrategy.DeleteEntry(WorkingGroup.UniqueName);
                        WorkingGroup.UniqueName = textBox.Text;
                        CurrentStrategy.CreateEntry(WorkingGroup);
                        ReCreateMainPanel(CurrentStrategy);
                    }
                    else
                    {
                        ShowError("Already Exists");
                        textBox.Text = WorkingGroup.UniqueName;
                    }
                }
            }
        }

        private void groupNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WorkingGroup != null)
            {
                if (WorkingGroup.Property.Title != groupNameTextBox.Text)
                {
                    WorkingGroup.Property.Title = groupNameTextBox.Text;
                    ReCreateMainPanel(CurrentStrategy);
                }
            }
        }

        private void labelNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WorkingGroup != null)
            {
                if (WorkingGroup.Property.LabelNames != labelNameTextBox.Text.Split(','))
                {
                    WorkingGroup.Property.LabelNames = labelNameTextBox.Text.Split(',');
                    ReCreateMainPanel(CurrentStrategy);
                }
            }
        }

        private void countComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WorkingGroup != null)
            {
                if (WorkingGroup.Property.Count != (int)countComboBox.SelectedItem)
                {
                    WorkingGroup.Property.Count = (int)countComboBox.SelectedItem;
                    ReCreateMainPanel(CurrentStrategy);
                }
            }
        }

        private void selectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WorkingGroup != null)
            {
                if (WorkingGroup.Property.SelectorType != (FieldType)selectorComboBox.SelectedItem)
                {
                    WorkingGroup.Property.SelectorType = (FieldType)selectorComboBox.SelectedItem;
                    ReCreateMainPanel(CurrentStrategy);
                }
            }
        }

        public void decimalUpDown_SelectionChanged(object sender, RoutedEventArgs e)
        {
            DecimalUpDown decimalUpDown = e.Source as DecimalUpDown;
            if (WorkingGroup != null)
            {
                if (decimalUpDown != null && WorkingGroup.Property.Content != decimalUpDown.Value.ToString())
                {
                    WorkingGroup.Property.Content = decimalUpDown.Value.ToString();
                    ReCreateMainPanel(CurrentStrategy);
                }
            }
        }

        public void textBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = e.Source as TextBox;
            if (WorkingGroup != null)
            {
                if (textBox != null && WorkingGroup.Property.Content != textBox.Text)
                {
                    WorkingGroup.Property.Content = textBox.Text;
                    ReCreateMainPanel(CurrentStrategy);
                }
            }
        }

        public void ComboBox_selectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = e.Source as ComboBox;
            if (WorkingGroup != null)
            {
                if (comboBox != null)//&& //WorkingGroup.Property.Content != comboBox.Items)
                {
                    WorkingGroup.Property.Content = comboBox.Text;
                    ReCreateMainPanel(CurrentStrategy);
                }
            }
        }

        #endregion

        #region Strategy
        /// <summary>
        /// loads Strategy
        /// </summary>
        private void LoadStrategy()
        {
            //todo
            StrategyList = new ObservableCollection<Strategy>()
                               {
                                   new Strategy{StrategyName = "Strategy1"}
                               };
        }

        /// <summary>
        /// Set the selected strategy to Current Strategy
        /// </summary>
        /// <param name="strategy"></param>
        private void SetSelectedStrategy(Strategy strategy)
        {

            if (CurrentStrategy == null)
            {
                CurrentStrategy = strategy;
                return;
            }

            if (CurrentStrategy == strategy)
                return;

            foreach (var s in StrategyList.Where(s => s == strategy))
            {
                mystrategycombobox.SelectedItem = s;
                ReCreateMainPanel(strategy);
                CurrentStrategy = s;
                break;
            }

        }

        /// <summary>
        /// Replace old strategy object with new
        /// </summary>
        public void ReplaceStrategy(Strategy strategy)
        {
            foreach (var s in StrategyList.Where(s => s == strategy))
            {
                int index = StrategyList.IndexOf(s);
                StrategyList.RemoveAt(index);
                StrategyList.Insert(index, CurrentStrategy);
                break;
            }
        }
        /// <summary>
        /// Set selected strategy as Current Strategy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mystrategycombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ignore changes that are made during initialization
            if (e.AddedItems.Count != e.RemovedItems.Count)
                return;

            if (StrategyCombobox.IsLoaded)
            {
                var s = StrategyCombobox.SelectedItem as Strategy;
                SetSelectedStrategy(s);
                ClearProperty();
            }
        }
        /// <summary>
        /// Always load the first item in the DropList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mystrategycombobox_Loaded(object sender, RoutedEventArgs e)
        {
            if (StrategyCombobox.SelectedIndex == -1)
            {
                StrategyCombobox.SelectedIndex = 0;
                CurrentStrategy = StrategyCombobox.SelectedItem as Strategy;
            }
        }

        /// <summary>
        /// Create and Save Stragety
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveStrategy_Click(object sender, RoutedEventArgs e)
        {
            string strategyName = StrategyCombobox.Text;

            if (StrategyList.Any(s => s.StrategyName == strategyName))
            {
                ShowError("Strategy " + strategyName + " already exists. Please try a new name");
                return;
            }
            CreateNewStrategy(strategyName.Trim());
        }

        /// <summary>
        /// Creates a new Strategy for given name
        /// </summary>
        /// <param name="strategyName">Name of Strategy</param>
        private void CreateNewStrategy(string strategyName)
        {
            if (!string.IsNullOrEmpty(strategyName) && strategyName!="")
            {
                Strategy newStartegy = new Strategy { StrategyName = strategyName };
                StrategyList.Add(newStartegy);
                SetSelectedStrategy(newStartegy);
                ClearProperty();
            }
            else
            {
                ShowError("Please Enter A Valid Strategy Name");
            }
        }

        /// <summary>
        /// Deletes a selected strategy from the ComboBox
        /// After deletion set the first strategy by 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteStrategy_Click(object sender, RoutedEventArgs e)
        {
            var strategyDel = StrategyCombobox.SelectedItem as Strategy;
            if(strategyDel==null)
            {
                ShowError("Theres no Strategy to be Deleted");
                return;
            }

            if (CurrentStrategy != null)
            {
                StrategyList.Remove(strategyDel);
                if (StrategyCombobox.Items.Count > 0)
                {
                    Strategy s = (Strategy) StrategyCombobox.Items[0];
                    SetSelectedStrategy(s);
                }
                else
                {
                    CurrentStrategy = null;
                    ClearCurrentPanel();
                }
            }
            
        }
        #endregion

        #region Helper methods
        
        /// <summary>
        /// Recreates the Main panel according to the CurrentStrategy selected
        /// </summary>
        private void ReCreateMainPanel(Strategy strategy)
        {
            if (strategy == null)
                return;
            ClearCurrentPanel();

            foreach (var group in strategy.GroupDictionary.Values)
            {
                MyGrid mygrid = new MyGrid();
                mygrid.CreateGrid(group);
                Mainpanel.Children.Add(mygrid.Grid);
            }
        }

        private void ClearCurrentPanel()
        {
            Mainpanel.Children.Clear();
        }

        #endregion

        #region MenuActions
        private void saveMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void openMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Error
        public void ShowError(string errorMessage)
        {
            ErrorPop errorPop = new ErrorPop(errorMessage);
            errorPop.ShowDialog();
        }
        #endregion
    }
}
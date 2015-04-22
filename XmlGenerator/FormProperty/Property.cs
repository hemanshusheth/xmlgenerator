namespace FormProperty
{
    #region Enums
    public enum FieldType
    {
        ButtonControl,
        CheckControl,
        TimeControl,
        DropListControl,
        BlankNumericControl,
        RadioControl,
        TextBoxControl,
        ListBoxControl,
        NoType
    }
    #endregion

    public class Property
    {
        #region Fields
        private string _title;
        private int _count;
        private string[] _labelNames;
        private FieldType _selectorType;
        private FieldType _fieldtype;
        private string _content ;

        private string[] _uniqueName;
        private string _fieldName;
        private bool _showByDefault;
        //text
        private string _message;
        //text drop radio
        private string _messageValue;
        private bool ignoreIfEmptyBlank;
        private bool _default;
        //check
        private int _falseValue;
        //check
        private int _trueValue;
        private int _decimals;
        //numeric
        private double _increment;
        //numeric
        private double _max;
        //numeric
        private double _min;
        //drop
        private int _selectedItem;
        private bool _labelPosition;
        private bool _doNotSave;
        private string _defaultTime;
        private bool _autoNextDay;
        private string _attachedGroup;
        private decimal[] _prefferedWidth;
        private decimal[] _maxLen;
        private bool _isOptionalField;
        
        #endregion

        #region Properties
        
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public string[] LabelNames
        {
            get { return _labelNames; }
            set { _labelNames = value; }
        }

        public FieldType Fieldtype
        {
            get { return _fieldtype; }
            set { _fieldtype = value; }
        }

        public FieldType SelectorType
        {
            get { return _selectorType; }
            set { _selectorType = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public string[] UniqueName
        {
            get { return _uniqueName; }
            set { _uniqueName = value; }
        }

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public bool ShowByDefault
        {
            get { return _showByDefault; }
            set { _showByDefault = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string MessageValue
        {
            get { return _messageValue; }
            set { _messageValue = value; }
        }

        public bool IgnoreIfEmptyBlank
        {
            get { return ignoreIfEmptyBlank; }
            set { ignoreIfEmptyBlank = value; }
        }

        public bool Default
        {
            get { return _default; }
            set { _default = value; }
        }

        public int FalseValue
        {
            get { return _falseValue; }
            set { _falseValue = value; }
        }

        public int TrueValue
        {
            get { return _trueValue; }
            set { _trueValue = value; }
        }

        public int Decimals
        {
            get { return _decimals; }
            set { _decimals = value; }
        }

        public double Increment
        {
            get { return _increment; }
            set { _increment = value; }
        }

        public double Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public double Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }

        public bool LabelPosition
        {
            get { return _labelPosition; }
            set { _labelPosition = value; }
        }

        public bool DoNotSave
        {
            get { return _doNotSave; }
            set { _doNotSave = value; }
        }

        public string DefaultTime
        {
            get { return _defaultTime; }
            set { _defaultTime = value; }
        }

        public bool AutoNextDay
        {
            get { return _autoNextDay; }
            set { _autoNextDay = value; }
        }

        public string AttachedGroup
        {
            get { return _attachedGroup; }
            set { _attachedGroup = value; }
        }

        public decimal[] PrefferedWidth
        {
            get { return _prefferedWidth; }
            set { _prefferedWidth = value; }
        }

        public decimal[] MaxLen
        {
            get { return _maxLen; }
            set { _maxLen = value; }
        }

        public bool IsOptionalField
        {
            get { return _isOptionalField; }
            set { _isOptionalField = value; }
        }

        #endregion

    }
}

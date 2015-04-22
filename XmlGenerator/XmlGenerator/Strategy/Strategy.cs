using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FormProperty;

namespace XmlGenerator.Strategy
{
    public class Strategy
    {
        #region Fields
        private string _strategyName;
        private Dictionary<int,Group> _groupDictionary;

        #endregion

        #region Properties
        public string StrategyName
        {
            get { return _strategyName; }
            set { _strategyName = value; }
        }

        public Dictionary<int,Group> GroupDictionary
        {
            get { return _groupDictionary; }
            set { _groupDictionary = value; }
        }
        #endregion

        #region Constructor
        internal Strategy()
        {
            GroupDictionary = new Dictionary<int, Group>();
        }
        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
       
        
    }
}

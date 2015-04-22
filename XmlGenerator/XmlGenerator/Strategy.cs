using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FormProperty;

namespace XmlGenerator
{
    public class Strategy
    {
        #region Fields
        private string _strategyName;
        private Dictionary<string,Group> _groupDictionary;

        #endregion

        #region Properties
        public string StrategyName
        {
            get { return _strategyName; }
            set { _strategyName = value; }
        }

        public Dictionary<string,Group> GroupDictionary
        {
            get { return _groupDictionary; }
            set { _groupDictionary = value; }
        }
        #endregion

        #region Constructor
        internal Strategy()
        {
            GroupDictionary = new Dictionary<string, Group>();
        }
        #endregion

        #region Public Methods
        public void CreateEntry(Group group)
        {
            GroupDictionary.Add(group.UniqueName,group);
        }

        public Group ReturnEntry(string name)
        {
            return GroupDictionary[name];
        }

        public bool HasEntry(string name)
        {
            return GroupDictionary.ContainsKey(name);
        }

        public void DeleteEntry(string uniqueName)
        {
            if(GroupDictionary.ContainsKey(uniqueName))
                GroupDictionary.Remove(uniqueName);
        }

        public void DeleteEntryTitle(string title)
        {
            foreach (Group group in GroupDictionary.Values)
            {
               if(group.Property.Title==title)
               {
                    DeleteEntry(group.UniqueName);
                   break;
               }
            }
        }

        public void UpdateEntry(Group WorkingGroup)
        {
            GroupDictionary[WorkingGroup.UniqueName] = WorkingGroup;
        }

        #endregion

        #region Private Methods

        #endregion

       
    }
}

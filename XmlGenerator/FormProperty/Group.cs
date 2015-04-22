using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormProperty
{
    public class Group
    {
        #region Fields

        private string _uniqueName;
        private Property _property;
       
        #endregion

        #region Properties
        public string UniqueName
        {
            get { return _uniqueName; }
            set { _uniqueName = value; }
        }

        public Property Property
        {
            get { return _property; }
            set { _property = value; }
        }

        #endregion

        #region Constructor
        public Group(Property property)
        {
            _property = property;
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

    }
}

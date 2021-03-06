﻿using SQLite;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SQLiteManager.DataModels
{
    /// <summary>
    /// Base class for your DataModels. The default properties for a model are initialized in here and will by default be kept up-to-date.
    /// </summary>
    public class BaseDataModel : INotifyPropertyChanged
    {
        private Guid _id;
        private bool _isDeleted;
        private DateTime _insertTimestamp;
        private DateTime _updateTimestamp;
        private DateTime _deletedTimestamp;

        /// <summary>
        /// The Primary Key for the data model.
        /// </summary>
        [PrimaryKey]
        public Guid Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Indicates whether the data model is registered as "Deleted" or not.
        /// </summary>
        public Boolean IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                _isDeleted = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The timestamp on which the current data model has been inserted into the database.
        /// </summary>
        public DateTime InsertTimestamp
        {
            get { return _insertTimestamp; }
            set
            {
                _insertTimestamp = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The timestamp on which the current data model has been updated.
        /// </summary>
        public DateTime UpdateTimestamp
        {
            get { return _updateTimestamp; }
            set
            {
                _updateTimestamp = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The timestamp on which the current data model has been deleted.
        /// </summary>
        public DateTime DeletedTimestamp
        {
            get { return _deletedTimestamp; }
            set
            {
                _deletedTimestamp = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Base class for your DataModels. The default properties for a model are initialized in here and will by default be kept up-to-date.
        /// </summary>
        public BaseDataModel()
        {
            Id = Guid.NewGuid();
            IsDeleted = false;

            DateTime now = DateTime.Now.ToLocalTime();
            InsertTimestamp = now;
            UpdateTimestamp = now;
            DeletedTimestamp = DateTime.MinValue;
        }

        /// <summary>
        /// The PropertyChanged-event for your model.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Call this method to invoke the PropertyChanged-event and notify any listener of changes to a property.
        /// </summary>
        /// <param name="propertyName">The name of the property that has been changed. If no parameter is given the name of the caller will be used instead.</param>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

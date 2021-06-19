using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GalaxyMerge.Client.Wrappers.Base
{
    public class ChangeTrackingCollection<T> : ObservableCollection<T>, IValidatableChangeTracking
        where T : class, IValidatableChangeTracking
    {
        private IList<T> _original;
        private readonly ObservableCollection<T> _added;
        private readonly ObservableCollection<T> _removed;
        private readonly ObservableCollection<T> _modified;

        public ChangeTrackingCollection(ICollection<T> collection) : base (collection)
        {
            _original = this.ToList();

            AttachPropertyChangeHandler(collection);

            _added = new ObservableCollection<T>();
            _removed = new ObservableCollection<T>();
            _modified = new ObservableCollection<T>();

            Added = new ReadOnlyObservableCollection<T>(_added);
            Removed = new ReadOnlyObservableCollection<T>(_removed);
            Modified = new ReadOnlyObservableCollection<T>(_modified);
        }

        public ReadOnlyObservableCollection<T> Added { get; }
        public ReadOnlyObservableCollection<T> Removed { get; }
        public ReadOnlyObservableCollection<T> Modified { get; }
        
        public bool IsChanged => Added.Count > 0 || Removed.Count > 0 || Modified.Count > 0;

        public bool IsValid => this.All(t => t.IsValid);

        public void AcceptChanges()
        {
            _added.Clear();
            _removed.Clear();
            _modified.Clear();

            foreach (var item in this)
                item.AcceptChanges();

            _original = this.ToList();
            RaisePropertyChanged(nameof(IsChanged));
        }

        public void RejectChanges()
        {
            foreach (var addedItem in _added.ToList())
                Remove(addedItem);

            foreach (var removedItem in _removed.ToList())
            {
                removedItem.RejectChanges();
                Add(removedItem);
            }
            
            foreach (var modifiedItem in _modified.ToList())
                modifiedItem.RejectChanges();

            RaisePropertyChanged(nameof(IsChanged));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var added = this.Where(current => _original.All(o => o != current)).ToList();
            var removed = _original.Where(o => this.All(current => current != o)).ToList();
            var modified = this.Except(added).Except(removed).Where(i => i.IsChanged).ToList();
            
            AttachPropertyChangeHandler(added);
            DetachPropertyChangeHandler(removed);

            UpdateObservableCollection(_added, added);
            UpdateObservableCollection(_removed, removed);
            UpdateObservableCollection(_modified, modified);
            
            base.OnCollectionChanged(e);
            
            RaisePropertyChanged(nameof(IsChanged));
            RaisePropertyChanged(nameof(IsValid));
        }

        private static void UpdateObservableCollection(ICollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();

            foreach (var item in items)
                collection.Add(item);
        }

        private void AttachPropertyChangeHandler(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                item.PropertyChanged -= ItemPropertyChanged;
                item.PropertyChanged += ItemPropertyChanged;
            }
        }

        private void DetachPropertyChangeHandler(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                item.PropertyChanged -= ItemPropertyChanged;
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsValid))
            {
                RaisePropertyChanged(nameof(IsValid));
                return;
            }
            
            var item = (T) sender;

            if (_added.Contains(item)) return;

            if (item.IsChanged)
            {
                if (!_modified.Contains(item))
                    _modified.Add(item);
            }
            else
            {
                if (_modified.Contains(item))
                    _modified.Remove(item);
            }

            RaisePropertyChanged(nameof(IsChanged));
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
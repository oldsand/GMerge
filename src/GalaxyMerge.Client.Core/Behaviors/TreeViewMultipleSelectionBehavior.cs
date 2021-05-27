using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaxyMerge.Client.Core.Extensions;
using Microsoft.Xaml.Behaviors;

namespace GalaxyMerge.Client.Core.Behaviors
{
    /// <summary>
    /// A behavior that extends a <see cref="TreeView"/> with multiple selection capabilities.
    /// </summary>
    /// <remarks>
    /// Credit to Steve Cadwallader. Based on post from https://www.codecadwallader.com/2015/11/22/wpf-treeview-with-multi-select/
    /// </remarks>
    public class TreeViewMultipleSelectionBehavior : Behavior<TreeView>
    {
        /// <summary>
        /// The dependency property definition for the SelectedItems property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(IList),
                typeof(TreeViewMultipleSelectionBehavior));

        ///<summary>
        /// Gets or sets the selected items.
        /// </summary>
        public IList SelectedItems
        {
            get => (IList) GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }


        ///<summary>
        /// The dependency property definition for the AnchorItem property.
        /// </summary>
        public static readonly DependencyProperty AnchorItemProperty =
            DependencyProperty.Register(nameof(AnchorItem), typeof(TreeViewItem),
                typeof(TreeViewMultipleSelectionBehavior));

        /// <summary>
        /// Gets or sets the anchor item.
        /// </summary>
        public TreeViewItem AnchorItem
        {
            get => (TreeViewItem) GetValue(AnchorItemProperty);
            set => SetValue(AnchorItemProperty, value);
        }


        /// <summary>
        /// The dependency property definition for the IsITemSelected attached property.
        /// </summary>
        public static readonly DependencyProperty IsItemSelectedProperty = DependencyProperty.RegisterAttached(
            "IsItemSelected", typeof(bool), typeof(TreeViewMultipleSelectionBehavior),
            new PropertyMetadata(OnIsItemSelectedChanged));

        /// <summary>
        /// Sets the IsItemsSelected value on the specified target.
        /// </summary>
        /// <param name="target">Target dependency object</param>
        /// <param name="value">Target value</param>
        public static void SetIsItemSelected(TreeViewItem target, bool value)
        {
            target.SetValue(IsItemSelectedProperty, value);
        }

        /// <summary>
        /// Gets the IsItemsSelected value from the specified target.
        /// </summary>
        /// <param name="target">Target dependency object.</param>
        public static bool GetIsItemSelected(TreeViewItem target)
        {
            return (bool) target.GetValue(IsItemSelectedProperty);
        }

        /// <summary>
        /// Called when the IsItemSelected dependency property has changed.
        /// </summary>
        /// <param name="d">The dependency object where the value has changed.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsItemSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Get the current item's tree view parent
            var treeViewItem = d as TreeViewItem;
            var treeView = treeViewItem?.FindVisualAncestor<TreeView>();
            if (treeView == null) return;

            //Get the tree view's selected items collection from it's attached behavior
            var behavior = Interaction.GetBehaviors(treeView).OfType<TreeViewMultipleSelectionBehavior>().FirstOrDefault();
            var selectedItems = behavior?.SelectedItems;
            if (selectedItems == null) return;

            //Add or remove the selected item
            if (GetIsItemSelected(treeViewItem))
                selectedItems.Add(treeViewItem.DataContext);
            else
                selectedItems.Remove(treeViewItem.DataContext);
        }

        /// <summary>
        /// Add event handlers for key down and mouse left button up to enable selection interaction.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(OnTreeViewItemKeyDown), true);
            AssociatedObject.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnTreeViewItemMouseUp), true);
        }

        /// <summary>
        /// Remove event handlers for key down and mouse left button up for clean up.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            
            AssociatedObject.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(OnTreeViewItemKeyDown));
            AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(OnTreeViewItemMouseUp));
        }

        /// <summary>
        /// Logic for handling tree view item selection when the key down event is called on a UI element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        [SuppressMessage("ReSharper", "SwitchStatementMissingSomeEnumCasesNoDefault")]
        private void OnTreeViewItemKeyDown(object sender, KeyEventArgs e)
        {
            if (!(e.OriginalSource is TreeViewItem treeViewItem)) return;
            
            TreeViewItem targetItem = null;
 
            switch (e.Key)
            {
                case Key.Down:
                    targetItem = GetRelativeItem(treeViewItem, 1);
                    break;
 
                case Key.Space:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                        ToggleSingleItem(treeViewItem);
                    break;
 
                case Key.Up:
                    targetItem = GetRelativeItem(treeViewItem, -1);
                    break;
            }

            if (targetItem == null) return;
            
            switch (Keyboard.Modifiers)
            {
                case ModifierKeys.Control:
                    Keyboard.Focus(targetItem);
                    break;
 
                case ModifierKeys.Shift:
                    SelectMultipleItemsContinuously(targetItem);
                    break;
 
                case ModifierKeys.None:
                    SelectSingleItem(targetItem);
                    break;
            }
        }

        /// <summary>
        /// Logic for handling tree view item selection when the mouse left button up event is called on tree view item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
        private void OnTreeViewItemMouseUp(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = FindParentTreeViewItem(e.OriginalSource);
            if (treeViewItem == null) return;
            
            switch (Keyboard.Modifiers)
            {
                case ModifierKeys.Control:
                    ToggleSingleItem(treeViewItem);
                    break;
 
                case ModifierKeys.Shift:
                    SelectMultipleItemsContinuously(treeViewItem);
                    break;
 
                default:
                    SelectSingleItem(treeViewItem);
                    break;
            }
        }
        
        /// <summary>
        /// Selects a range of consecutive items from the specified tree view item to the anchor (if exists).
        /// </summary>
        /// <param name="treeViewItem">The triggering tree view item.</param>
        private void SelectMultipleItemsContinuously(TreeViewItem treeViewItem)
        {
            if (AnchorItem == null) return;
            
            if (ReferenceEquals(AnchorItem, treeViewItem))
            {
                SelectSingleItem(treeViewItem);
                return;
            }
 
            var isBetweenAnchors = false;
            var items = DeSelectAll();
 
            foreach (var item in items)
            {
                if (ReferenceEquals(item, treeViewItem) || ReferenceEquals(item, AnchorItem))
                {
                    // Toggle isBetweenAnchors when first item is found, and back again when last item is found.
                    isBetweenAnchors = !isBetweenAnchors;
 
                    SetIsItemSelected(item, true);
                }
                else if (isBetweenAnchors)
                {
                    SetIsItemSelected(item, true);
                }
            }
        }
        
        /// <summary>
        /// Selects the specified tree view item, removing any other selections.
        /// </summary>
        /// <param name="treeViewItem">The triggering tree view item.</param>
        private void SelectSingleItem(TreeViewItem treeViewItem)
        {
            DeSelectAll();
            SetIsItemSelected(treeViewItem, true);
            AnchorItem = treeViewItem;
        }
        
        /// <summary>
        /// Toggles the selection state of the specified tree view item.
        /// </summary>
        /// <param name="treeViewItem">The triggering tree view item.</param>
        private void ToggleSingleItem(TreeViewItem treeViewItem)
        {
            SetIsItemSelected(treeViewItem, !GetIsItemSelected(treeViewItem));
 
            if (AnchorItem == null)
            {
                if (GetIsItemSelected(treeViewItem))
                {
                    AnchorItem = treeViewItem;
                }
            }
            else if (SelectedItems.Count == 0)
            {
                AnchorItem = null;
            }
        }
        
        ///<summary>
        /// Clears all selections.
        /// </summary>
        /// <remarks>
        /// The list of all items is returned as a convenience to avoid multiple iterations.
        /// </remarks>
        /// <returns>The list of all items.</returns>
        private IEnumerable<TreeViewItem> DeSelectAll()
        {
            var items = GetItemsRecursively<TreeViewItem>(AssociatedObject);
            foreach (var item in items)
            {
                SetIsItemSelected(item, false);
            }
 
            return items;
        }
        
        ///<summary>
        /// Attempts to find the parent TreeViewItem from the specified event source.
        /// </summary>
        /// <param name="eventSource">The event source.</param>
        /// <returns>The parent TreeViewItem, otherwise null.</returns>
        private static TreeViewItem FindParentTreeViewItem(object eventSource)
        {
            var source = eventSource as DependencyObject;
 
            var treeViewItem = source?.FindVisualAncestor<TreeViewItem>();
 
            return treeViewItem;
        }
        
        /// <summary>
        /// Gets items of the specified type recursively from the specified parent item.
        /// </summary>
        /// <typeparam name="T">The type of item to retrieve.</typeparam>
        /// <param name="parentItem">The parent item.</param>
        /// <returns>The list of items within the parent item, may be empty.</returns>
        private static IList<T> GetItemsRecursively<T>(ItemsControl parentItem)
            where T : ItemsControl
        {
            if (parentItem == null)
                throw new ArgumentNullException(nameof(parentItem));

            var items = new List<T>();
 
            for (var i = 0; i < parentItem.Items.Count; i++)
            {
                var item = parentItem.ItemContainerGenerator.ContainerFromIndex(i) as T;
                if (item == null) continue;
                items.Add(item);
                items.AddRange(GetItemsRecursively<T>(item));
            }
 
            return items;
        }
        
        /// <summary>
        /// Gets an item with a relative position (e.g. +1, -1) to the specified item.
        /// </summary>
        /// <remarks>This deliberately works against a flattened collection (i.e. no hierarchy).</remarks>
        /// <typeparam name="T">The type of item to retrieve.</typeparam>
        /// <param name="item">The item.</param>
        /// <param name="relativePosition">The relative position offset (e.g. +1, -1).</param>
        /// <returns>The item in the relative position, otherwise null.</returns>
        private T GetRelativeItem<T>(T item, int relativePosition)
            where T : ItemsControl
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            var items = GetItemsRecursively<T>(AssociatedObject);
            var index = items.IndexOf(item);
            if (index < 0) return null;
            
            var relativeIndex = index + relativePosition;
            if (relativeIndex >= 0 && relativeIndex < items.Count)
                return items[relativeIndex];

            return null;
        }
    }
}
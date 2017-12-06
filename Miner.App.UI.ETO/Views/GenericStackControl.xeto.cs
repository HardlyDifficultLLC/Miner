﻿using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Xaml;
using System.Collections;
using System.Collections.Specialized;

namespace HD.Views
{
    public class GenericStackControl : Scrollable
    {
        protected StackLayout Stack;

        internal Type ControlType { get; set; } = typeof(Label);

        public GenericStackControl()
        {
            XamlReader.Load(this);
            DataContextChanged += GenericStackControl_DataContextChanged;
        }

        private void GenericStackControl_DataContextChanged(object sender, EventArgs e)
        {
            if (DataContext is ICollection collection)
            {
                Stack.Items.Clear();
                AddControls(collection);
                if (DataContext is INotifyCollectionChanged notifyChanged)
                {
                    notifyChanged.CollectionChanged += Collection_CollectionChanged;
                }
            }
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AddControls(e.NewItems);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                RemoveControls(e.OldItems);
            }
        }

        private void AddControls(ICollection collection)
        {
            foreach (var item in collection)
            {
                Control ctrl = (Control)Activator.CreateInstance(ControlType);
                if (ctrl is TextControl txtCtrl && item is string strItem)
                {
                    txtCtrl.Text = strItem;
                }
                else
                {
                    ctrl.DataContext = item;
                }
                Stack.Items.Add(ctrl);
            }

        }

        private void RemoveControls(ICollection collection)
        {
            foreach (Control item in collection)
            {               
                Stack.Items.Remove(item);
            }
        }


    }
}
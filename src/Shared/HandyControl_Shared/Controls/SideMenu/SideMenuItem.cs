﻿using System.Linq;
using System.Windows;
using System.Windows.Input;
using HandyControl.Data;
using HandyControl.Tools.Extension;

namespace HandyControl.Controls
{
    public class SideMenuItem : HeaderedSimpleItemsControl, ISelectable
    {
        private bool _isMouseLeftButtonDown;

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon", typeof(object), typeof(SideMenuItem), new PropertyMetadata(default(object)));

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        protected virtual void OnSelected(RoutedEventArgs e) => RaiseEvent(e);

        public static readonly RoutedEvent SelectedEvent =
            EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(SideMenuItem));

        public event RoutedEventHandler Selected
        {
            add => AddHandler(SelectedEvent, value);
            remove => RemoveHandler(SelectedEvent, value);
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(SideMenuItem), new PropertyMetadata(ValueBoxes.FalseBox));

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty RoleProperty = DependencyProperty.Register(
            "Role", typeof(SideMenuItemRole), typeof(SideMenuItem), new PropertyMetadata(default(SideMenuItemRole)));

        public SideMenuItemRole Role
        {
            get => (SideMenuItemRole) GetValue(RoleProperty);
            internal set => SetValue(RoleProperty, value);
        }

        protected override DependencyObject GetContainerForItemOverride() => new SideMenuItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is SideMenuItem;

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            _isMouseLeftButtonDown = false;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            _isMouseLeftButtonDown = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (_isMouseLeftButtonDown)
            {
                IsSelected = true;
                OnSelected(new RoutedEventArgs(SelectedEvent, this));
                _isMouseLeftButtonDown = false;
            }
        }

        internal void SelectDefaultItem()
        {
            if (Role == SideMenuItemRole.Header && ItemsHost.Children.Count > 0)
            {
                var item = ItemsHost.Children.OfType<SideMenuItem>().FirstOrDefault();
                if (item != null && !item.IsSelected)
                {
                    item.OnSelected(new RoutedEventArgs(SelectedEvent, item));
                }
            }
        }

        internal void SwitchPanelArea(bool close)
        {
            if (Role == SideMenuItemRole.Header)
            {
                ItemsHost.Show(close);
            }
        }
    }
}
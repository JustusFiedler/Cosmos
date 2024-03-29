﻿using System.Windows;
using System.Windows.Controls;
using static Cosmos.Classes.CColor;

namespace Cosmos
{
    public class CLabel : Label
    {
        public static readonly DependencyProperty ForegroundThemeProperty = DependencyProperty.Register("C_ForegroundTheme", typeof(Theme), typeof(CLabel), new PropertyMetadata(new PropertyChangedCallback(ForegroundThemeValueChanged)));

        /// <summary>
        /// customized label in style, no extra features
        /// </summary>
        static CLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CLabel), new FrameworkPropertyMetadata(typeof(CLabel)));
        }

        public Theme C_ForegroundTheme
        {
            get
            {
                return (Theme)GetValue(ForegroundThemeProperty);
            }
            set
            {
                SetValue(ForegroundThemeProperty, value);
            }
        }

        private static void ForegroundThemeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CLabel)d;

            control.Foreground = GetColorBrush((Theme)e.NewValue);
        }
    }
}

﻿using Expenses.TestApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Expenses.TestApp
{
    public class ViewSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is RejestracjaVm)
                return Rejestracja;
            if (item is LogowanieVm)
                return Logowanie;
            return base.SelectTemplate(item, container);
        }

        public DataTemplate Rejestracja { get; set; }
        public DataTemplate Logowanie { get; set; }
    }
}
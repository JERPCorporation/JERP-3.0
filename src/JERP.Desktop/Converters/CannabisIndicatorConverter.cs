/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using JERP.Application.DTOs.Finance;

namespace JERP.Desktop.Converters;

public class CannabisIndicatorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AccountTemplateSummaryDto template)
        {
            var searchableText = $"{template.Industry}|{template.Code}".ToLowerInvariant();
            var hasCannabis = searchableText.Contains("cannabis") || searchableText.Contains("280e") || searchableText.Contains("marijuana");
            return hasCannabis ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

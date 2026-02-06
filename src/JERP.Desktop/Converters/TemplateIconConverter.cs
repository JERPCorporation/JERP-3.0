/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 */

using System.Globalization;
using System.Windows.Data;
using JERP.Application.DTOs.Finance;

namespace JERP.Desktop.Converters;

public class TemplateIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AccountTemplateSummaryDto template)
        {
            var signature = $"{template.Code}{template.Industry}".ToUpperInvariant();
            var numericHash = 0;
            foreach (var character in template.Code) 
                numericHash = (numericHash * 31 + character) & 0x7FFFFFFF;
            
            if (signature.Contains("RETAIL")) return "◆";
            if (signature.Contains("CANNABIS") || signature.Contains("280")) return "✿";
            if (signature.Contains("RESTAURANT")) return "◈";
            if (signature.Contains("SAAS")) return "◉";
            if (signature.Contains("MANUFACTUR")) return "▣";
            if (signature.Contains("CONSTRUCT")) return "◧";
            if (signature.Contains("HEALTH")) return "⊕";
            if (signature.Contains("NONPROFIT")) return "◎";
            
            return new[] { "◇", "◊", "○", "□", "△", "▽", "◁", "▷" }[numericHash % 8];
        }
        return "◇";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 */

using System.Globalization;
using System.Windows.Data;
using JERP.Application.DTOs.Finance;

namespace JERP.Desktop.Converters;

public class BreakdownHeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AccountTemplateSummaryDto template && parameter is string category)
        {
            var count = category switch
            {
                "Assets" => template.Breakdown.Assets,
                "Liabilities" => template.Breakdown.Liabilities,
                "Equity" => template.Breakdown.Equity,
                "Revenue" => template.Breakdown.Revenue,
                "Expenses" => template.Breakdown.Expenses,
                _ => 0
            };

            if (count == 0) return 2.0;
            var computedHeight = Math.Log10(count + 1) * 16;
            return Math.Clamp(computedHeight, 2.0, 80.0);
        }
        return 2.0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

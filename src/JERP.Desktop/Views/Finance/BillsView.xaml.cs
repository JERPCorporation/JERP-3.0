/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 ninoyerbas. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of ninoyerbas.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using System.Windows.Controls;
using JERP.Desktop.ViewModels.Finance;

namespace JERP.Desktop.Views.Finance;

public partial class BillsView : System.Windows.Controls.UserControl
{
    public BillsView(BillsViewModel billsViewModel)
    {
        InitializeComponent();
        DataContext = billsViewModel;
    }
}

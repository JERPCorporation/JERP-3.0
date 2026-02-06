/*
 * JERP 3.0 - Payroll & ERP System
 * Copyright (c) 2026 Julio Cesar Mendez Tobar. All Rights Reserved.
 * 
 * PROPRIETARY AND CONFIDENTIAL
 * 
 * This source code is the confidential and proprietary information of Julio Cesar Mendez Tobar.
 * Unauthorized copying, modification, distribution, or use is strictly prohibited.
 * 
 * For licensing inquiries: licensing@jerp.io
 */

using JERP.Application.Services.Payroll.Pdf.Models;
using JERP.Core.Enums;
using JERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace JERP.Application.Services.Payroll.Pdf;

/// <summary>
/// Implementation of PDF generation services using QuestPDF
/// </summary>
public class PdfGenerationService : IPdfGenerationService
{
    private readonly JerpDbContext _context;
    private readonly ILogger<PdfGenerationService> _logger;

    public PdfGenerationService(
        JerpDbContext context,
        ILogger<PdfGenerationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GeneratePayStubAsync(Guid payrollRecordId)
    {
        _logger.LogInformation("Generating pay stub for payroll record: {RecordId}", payrollRecordId);

        var document = await BuildPayStubDocumentAsync(payrollRecordId);
        var pdfBytes = GeneratePdf(document);

        _logger.LogInformation("Pay stub generated: {RecordId}, Size: {Size} bytes", payrollRecordId, pdfBytes.Length);

        return pdfBytes;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateBulkPayStubsAsync(Guid payPeriodId)
    {
        _logger.LogInformation("Generating bulk pay stubs for pay period: {PayPeriodId}", payPeriodId);

        var records = await _context.PayrollRecords
            .Include(r => r.Employee)
            .Include(r => r.PayPeriod)
            .Include(r => r.Details)
            .Where(r => r.PayPeriodId == payPeriodId && !r.IsDeleted)
            .ToListAsync();

        if (!records.Any())
        {
            throw new InvalidOperationException($"No payroll records found for pay period {payPeriodId}");
        }

        // Generate combined PDF with all pay stubs
        var pdfBytes = Document.Create(container =>
        {
            foreach (var record in records)
            {
                var document = BuildPayStubDocumentFromRecord(record);
                container.Page(page => ComposePayStubPage(page, document));
            }
        }).GeneratePdf();

        _logger.LogInformation("Bulk pay stubs generated: {PayPeriodId}, Count: {Count}, Size: {Size} bytes",
            payPeriodId, records.Count, pdfBytes.Length);

        return pdfBytes;
    }

    private async Task<PayStubDocument> BuildPayStubDocumentAsync(Guid payrollRecordId)
    {
        var record = await _context.PayrollRecords
            .Include(r => r.Employee)
                .ThenInclude(e => e.Company)
            .Include(r => r.Employee.Department)
            .Include(r => r.PayPeriod)
            .Include(r => r.Details)
            .FirstOrDefaultAsync(r => r.Id == payrollRecordId && !r.IsDeleted);

        if (record == null)
        {
            throw new InvalidOperationException($"Payroll record {payrollRecordId} not found");
        }

        return BuildPayStubDocumentFromRecord(record);
    }

    private PayStubDocument BuildPayStubDocumentFromRecord(Core.Entities.PayrollRecord record)
    {
        var document = new PayStubDocument
        {
            Company = new CompanyInfo
            {
                Name = record.Employee.Company?.Name ?? "Company Name",
                Address = record.Employee.Company?.Address,
                Phone = record.Employee.Company?.Phone
            },
            Employee = new EmployeeInfo
            {
                Name = $"{record.Employee.FirstName} {record.Employee.LastName}",
                EmployeeNumber = record.Employee.EmployeeNumber,
                MaskedSSN = MaskSSN(record.Employee.SSN),
                Department = record.Employee.Department?.Name
            },
            PayPeriod = new PayPeriodInfo
            {
                StartDate = record.PayPeriod.StartDate,
                EndDate = record.PayPeriod.EndDate,
                PayDate = record.PayPeriod.PayDate
            },
            Summary = new PaySummary
            {
                GrossPay = record.GrossPay,
                TotalTaxes = record.TotalTaxes,
                TotalDeductions = record.TotalDeductions,
                NetPay = record.NetPay
            },
            YTD = new YTDSummary
            {
                GrossPay = record.YTDGrossPay,
                FederalTax = record.YTDFederalTax,
                StateTax = record.YTDStateTax,
                SocialSecurity = record.YTDSocialSecurity,
                Medicare = record.YTDMedicare,
                NetPay = record.YTDNetPay
            }
        };

        // Build earnings
        if (record.RegularPay > 0)
        {
            document.Earnings.Add(new EarningLine
            {
                Description = "Regular Pay",
                Hours = record.RegularHours,
                Rate = record.Employee.HourlyRate ?? 0,
                Amount = record.RegularPay
            });
        }

        if (record.OvertimePay > 0)
        {
            document.Earnings.Add(new EarningLine
            {
                Description = "Overtime Pay (1.5x)",
                Hours = record.OvertimeHours,
                Rate = record.Employee.HourlyRate.HasValue ? record.Employee.HourlyRate.Value * 1.5m : 0,
                Amount = record.OvertimePay
            });
        }

        if (record.DoubleTimePay > 0)
        {
            document.Earnings.Add(new EarningLine
            {
                Description = "Double Time Pay (2x)",
                Hours = record.DoubleTimeHours,
                Rate = record.Employee.HourlyRate.HasValue ? record.Employee.HourlyRate.Value * 2m : 0,
                Amount = record.DoubleTimePay
            });
        }

        if (record.BonusPay > 0)
        {
            document.Earnings.Add(new EarningLine
            {
                Description = "Bonus",
                Amount = record.BonusPay
            });
        }

        if (record.CommissionPay > 0)
        {
            document.Earnings.Add(new EarningLine
            {
                Description = "Commission",
                Amount = record.CommissionPay
            });
        }

        // Build taxes
        document.Taxes.Add(new TaxLine
        {
            Description = "Federal Income Tax",
            Current = record.FederalTax,
            YTD = record.YTDFederalTax
        });

        document.Taxes.Add(new TaxLine
        {
            Description = "State Income Tax",
            Current = record.StateTax,
            YTD = record.YTDStateTax
        });

        document.Taxes.Add(new TaxLine
        {
            Description = "Social Security",
            Current = record.SocialSecurityTax,
            YTD = record.YTDSocialSecurity
        });

        document.Taxes.Add(new TaxLine
        {
            Description = "Medicare",
            Current = record.MedicareTax,
            YTD = record.YTDMedicare
        });

        // Build deductions from details
        var deductionDetails = record.Details
            .Where(d => d.Type == PayrollRecordDetailType.Deduction)
            .ToList();

        foreach (var detail in deductionDetails)
        {
            document.Deductions.Add(new DeductionLine
            {
                Description = detail.Description,
                Current = detail.Amount,
                YTD = detail.Amount // TODO: Calculate actual YTD for deductions
            });
        }

        return document;
    }

    private byte[] GeneratePdf(PayStubDocument document)
    {
        return Document.Create(container =>
        {
            container.Page(page => ComposePayStubPage(page, document));
        }).GeneratePdf();
    }

    private void ComposePayStubPage(PageDescriptor page, PayStubDocument document)
    {
        page.Size(PageSizes.Letter);
        page.Margin(40);
        page.DefaultTextStyle(x => x.FontSize(10));

        page.Header().Element(container => ComposeHeader(container, document));
        page.Content().Element(container => ComposeContent(container, document));
        page.Footer().Element(container => ComposeFooter(container));
    }

    private void ComposeHeader(IContainer container, PayStubDocument document)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text(document.Company.Name).Bold().FontSize(16);
                    if (!string.IsNullOrEmpty(document.Company.Address))
                    {
                        col.Item().Text(document.Company.Address).FontSize(9);
                    }
                    if (!string.IsNullOrEmpty(document.Company.Phone))
                    {
                        col.Item().Text(document.Company.Phone).FontSize(9);
                    }
                });

                row.RelativeItem().AlignRight().Column(col =>
                {
                    col.Item().Text("PAY STUB").Bold().FontSize(18);
                    col.Item().Text($"Pay Date: {document.PayPeriod.PayDate:MM/dd/yyyy}").FontSize(9);
                });
            });

            column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Employee Information").Bold().FontSize(11);
                    col.Item().PaddingTop(5).Text($"Name: {document.Employee.Name}");
                    col.Item().Text($"Employee #: {document.Employee.EmployeeNumber}");
                    if (!string.IsNullOrEmpty(document.Employee.MaskedSSN))
                    {
                        col.Item().Text($"SSN: {document.Employee.MaskedSSN}");
                    }
                    if (!string.IsNullOrEmpty(document.Employee.Department))
                    {
                        col.Item().Text($"Department: {document.Employee.Department}");
                    }
                });

                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Pay Period").Bold().FontSize(11);
                    col.Item().PaddingTop(5).Text($"From: {document.PayPeriod.StartDate:MM/dd/yyyy}");
                    col.Item().Text($"To: {document.PayPeriod.EndDate:MM/dd/yyyy}");
                    col.Item().Text($"Pay Date: {document.PayPeriod.PayDate:MM/dd/yyyy}");
                });
            });
        });
    }

    private void ComposeContent(IContainer container, PayStubDocument document)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Earnings section
            column.Item().Element(c => ComposeEarningsTable(c, document));

            // Taxes section
            column.Item().PaddingTop(15).Element(c => ComposeTaxesTable(c, document));

            // Deductions section
            if (document.Deductions.Any())
            {
                column.Item().PaddingTop(15).Element(c => ComposeDeductionsTable(c, document));
            }

            // Summary section
            column.Item().PaddingTop(20).Element(c => ComposeSummary(c, document));

            // YTD section
            column.Item().PaddingTop(15).Element(c => ComposeYTD(c, document));
        });
    }

    private void ComposeEarningsTable(IContainer container, PayStubDocument document)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(3);
                columns.RelativeColumn(1);
                columns.RelativeColumn(1);
                columns.RelativeColumn(1);
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("EARNINGS").Bold();
                header.Cell().Element(CellStyle).AlignRight().Text("Hours").Bold();
                header.Cell().Element(CellStyle).AlignRight().Text("Rate").Bold();
                header.Cell().Element(CellStyle).AlignRight().Text("Amount").Bold();
            });

            foreach (var earning in document.Earnings)
            {
                table.Cell().Element(CellStyle).Text(earning.Description);
                table.Cell().Element(CellStyle).AlignRight().Text(earning.Hours?.ToString("N2") ?? "-");
                table.Cell().Element(CellStyle).AlignRight().Text(earning.Rate?.ToString("C") ?? "-");
                table.Cell().Element(CellStyle).AlignRight().Text(earning.Amount.ToString("C"));
            }

            table.Cell().ColumnSpan(3).Element(CellStyle).AlignRight().Text("Total Gross Pay:").Bold();
            table.Cell().Element(CellStyle).AlignRight().Text(document.Summary.GrossPay.ToString("C")).Bold();
        });
    }

    private void ComposeTaxesTable(IContainer container, PayStubDocument document)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(3);
                columns.RelativeColumn(1);
                columns.RelativeColumn(1);
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("TAXES").Bold();
                header.Cell().Element(CellStyle).AlignRight().Text("Current").Bold();
                header.Cell().Element(CellStyle).AlignRight().Text("YTD").Bold();
            });

            foreach (var tax in document.Taxes)
            {
                table.Cell().Element(CellStyle).Text(tax.Description);
                table.Cell().Element(CellStyle).AlignRight().Text(tax.Current.ToString("C"));
                table.Cell().Element(CellStyle).AlignRight().Text(tax.YTD.ToString("C"));
            }

            table.Cell().ColumnSpan(1).Element(CellStyle).AlignRight().Text("Total Taxes:").Bold();
            table.Cell().Element(CellStyle).AlignRight().Text(document.Summary.TotalTaxes.ToString("C")).Bold();
            table.Cell().Element(CellStyle);
        });
    }

    private void ComposeDeductionsTable(IContainer container, PayStubDocument document)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(3);
                columns.RelativeColumn(1);
                columns.RelativeColumn(1);
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("DEDUCTIONS").Bold();
                header.Cell().Element(CellStyle).AlignRight().Text("Current").Bold();
                header.Cell().Element(CellStyle).AlignRight().Text("YTD").Bold();
            });

            foreach (var deduction in document.Deductions)
            {
                table.Cell().Element(CellStyle).Text(deduction.Description);
                table.Cell().Element(CellStyle).AlignRight().Text(deduction.Current.ToString("C"));
                table.Cell().Element(CellStyle).AlignRight().Text(deduction.YTD.ToString("C"));
            }

            table.Cell().ColumnSpan(1).Element(CellStyle).AlignRight().Text("Total Deductions:").Bold();
            table.Cell().Element(CellStyle).AlignRight().Text(document.Summary.TotalDeductions.ToString("C")).Bold();
            table.Cell().Element(CellStyle);
        });
    }

    private void ComposeSummary(IContainer container, PayStubDocument document)
    {
        container.Border(1).Padding(10).Background(Colors.Grey.Lighten4).Column(column =>
        {
            column.Item().Text("PAY SUMMARY").Bold().FontSize(12);
            column.Item().PaddingTop(5).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text($"Gross Pay: {document.Summary.GrossPay:C}");
                    col.Item().Text($"Total Taxes: {document.Summary.TotalTaxes:C}");
                    col.Item().Text($"Total Deductions: {document.Summary.TotalDeductions:C}");
                });
                row.RelativeItem().AlignRight().Column(col =>
                {
                    col.Item().Text("NET PAY").Bold().FontSize(14);
                    col.Item().Text(document.Summary.NetPay.ToString("C")).Bold().FontSize(18).FontColor(Colors.Green.Darken2);
                });
            });
        });
    }

    private void ComposeYTD(IContainer container, PayStubDocument document)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(3);
                columns.RelativeColumn(1);
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("YEAR-TO-DATE SUMMARY").Bold();
                header.Cell().Element(CellStyle).AlignRight().Text("Amount").Bold();
            });

            table.Cell().Element(CellStyle).Text("Gross Pay");
            table.Cell().Element(CellStyle).AlignRight().Text(document.YTD.GrossPay.ToString("C"));

            table.Cell().Element(CellStyle).Text("Federal Tax");
            table.Cell().Element(CellStyle).AlignRight().Text(document.YTD.FederalTax.ToString("C"));

            table.Cell().Element(CellStyle).Text("State Tax");
            table.Cell().Element(CellStyle).AlignRight().Text(document.YTD.StateTax.ToString("C"));

            table.Cell().Element(CellStyle).Text("Social Security");
            table.Cell().Element(CellStyle).AlignRight().Text(document.YTD.SocialSecurity.ToString("C"));

            table.Cell().Element(CellStyle).Text("Medicare");
            table.Cell().Element(CellStyle).AlignRight().Text(document.YTD.Medicare.ToString("C"));

            table.Cell().Element(CellStyle).Text("Net Pay").Bold();
            table.Cell().Element(CellStyle).AlignRight().Text(document.YTD.NetPay.ToString("C")).Bold();
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Column(column =>
        {
            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            column.Item().PaddingTop(5).Text($"This is a computer-generated document. No signature required. Generated: {DateTime.UtcNow:MM/dd/yyyy HH:mm:ss} UTC")
                .FontSize(8).FontColor(Colors.Grey.Medium);
        });
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
    }

    private string? MaskSSN(string? ssn)
    {
        if (string.IsNullOrEmpty(ssn) || ssn.Length < 4)
        {
            return null;
        }

        var last4 = ssn.Substring(ssn.Length - 4);
        return $"XXX-XX-{last4}";
    }
}

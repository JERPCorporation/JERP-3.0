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

using JERP.Core.Entities.Finance;
using JERP.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace JERP.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds FASB ASC topics and subtopics into the database
/// </summary>
public static class FASBDataSeeder
{
    /// <summary>
    /// Seed all FASB topics and subtopics
    /// </summary>
    public static void SeedFASBData(ModelBuilder modelBuilder)
    {
        var topics = new List<FASBTopic>();
        var subtopics = new List<FASBSubtopic>();

        // CATEGORY 1: PRESENTATION TOPICS (200s)
        SeedPresentationTopics(topics, subtopics);

        // CATEGORY 2: ASSETS (300s)
        SeedAssetsTopics(topics, subtopics);

        // CATEGORY 3: LIABILITIES (400s)
        SeedLiabilitiesTopics(topics, subtopics);

        // CATEGORY 4: EQUITY (500s)
        SeedEquityTopics(topics, subtopics);

        // CATEGORY 5: REVENUE (600s)
        SeedRevenueTopics(topics, subtopics);

        // CATEGORY 6: EXPENSES (700s)
        SeedExpensesTopics(topics, subtopics);

        // CATEGORY 7: BROAD TRANSACTIONS (800s)
        SeedBroadTransactionsTopics(topics, subtopics);

        // CATEGORY 8: INDUSTRY TOPICS (900s)
        SeedIndustryTopics(topics, subtopics);

        modelBuilder.Entity<FASBTopic>().HasData(topics);
        modelBuilder.Entity<FASBSubtopic>().HasData(subtopics);
    }

    private static void SeedPresentationTopics(List<FASBTopic> topics, List<FASBSubtopic> subtopics)
    {
        AddTopic(topics, subtopics, "205", "Presentation of Financial Statements", FASBCategory.Presentation, 
            new[] { "10", "20", "30" });
        AddTopic(topics, subtopics, "210", "Balance Sheet", FASBCategory.Presentation, 
            new[] { "10", "20" });
        AddTopic(topics, subtopics, "220", "Income Statement", FASBCategory.Presentation, 
            new[] { "10", "20" });
        AddTopic(topics, subtopics, "225", "Income Statement—Discontinued Operations", FASBCategory.Presentation, 
            new[] { "10", "20" });
        AddTopic(topics, subtopics, "230", "Statement of Cash Flows", FASBCategory.Presentation, 
            new[] { "10" });
        AddTopic(topics, subtopics, "235", "Notes to Financial Statements", FASBCategory.Presentation, 
            new[] { "10" });
        AddTopic(topics, subtopics, "250", "Accounting Changes and Error Corrections", FASBCategory.Presentation, 
            new[] { "10" });
        AddTopic(topics, subtopics, "260", "Earnings Per Share", FASBCategory.Presentation, 
            new[] { "10" });
        AddTopic(topics, subtopics, "270", "Interim Reporting", FASBCategory.Presentation, 
            new[] { "10" });
        AddTopic(topics, subtopics, "272", "Limited Liability Entities", FASBCategory.Presentation, 
            new[] { "10" });
        AddTopic(topics, subtopics, "273", "Corporate Joint Ventures", FASBCategory.Presentation, 
            new[] { "10" });
        AddTopic(topics, subtopics, "274", "Personal Financial Statements", FASBCategory.Presentation, 
            new[] { "10" });
        AddTopic(topics, subtopics, "280", "Segment Reporting", FASBCategory.Presentation, 
            new[] { "10" });
    }

    private static void SeedAssetsTopics(List<FASBTopic> topics, List<FASBSubtopic> subtopics)
    {
        AddTopic(topics, subtopics, "305", "Cash and Cash Equivalents", FASBCategory.Assets, 
            new[] { "10" });
        AddTopic(topics, subtopics, "310", "Receivables", FASBCategory.Assets, 
            new[] { "10", "20", "30" });
        AddTopic(topics, subtopics, "320", "Investments—Debt Securities", FASBCategory.Assets, 
            new[] { "10" }, isSuperseded: true, supersededBy: "ASC 321 and ASC 326");
        AddTopic(topics, subtopics, "321", "Investments—Equity Securities", FASBCategory.Assets, 
            new[] { "10" });
        AddTopic(topics, subtopics, "323", "Investments—Equity Method and Joint Ventures", FASBCategory.Assets, 
            new[] { "10", "30" });
        AddTopic(topics, subtopics, "325", "Investments—Other", FASBCategory.Assets, 
            new[] { "10", "20", "30", "40" });
        AddTopic(topics, subtopics, "326", "Financial Instruments—Credit Losses", FASBCategory.Assets, 
            new[] { "10", "20", "30" });
        AddTopic(topics, subtopics, "330", "Inventory", FASBCategory.Assets, 
            new[] { "10" });
        AddTopic(topics, subtopics, "340", "Other Assets and Deferred Costs", FASBCategory.Assets, 
            new[] { "10", "20", "30", "40" });
        AddTopic(topics, subtopics, "350", "Intangibles—Goodwill and Other", FASBCategory.Assets, 
            new[] { "10", "20", "30", "40", "50", "60" });
        AddTopic(topics, subtopics, "360", "Property, Plant, and Equipment", FASBCategory.Assets, 
            new[] { "10", "20" });
    }

    private static void SeedLiabilitiesTopics(List<FASBTopic> topics, List<FASBSubtopic> subtopics)
    {
        AddTopic(topics, subtopics, "405", "Liabilities", FASBCategory.Liabilities, 
            new[] { "10", "20", "30", "40", "50" });
        AddTopic(topics, subtopics, "410", "Asset Retirement and Environmental Obligations", FASBCategory.Liabilities, 
            new[] { "10", "20", "30" });
        AddTopic(topics, subtopics, "420", "Exit or Disposal Cost Obligations", FASBCategory.Liabilities, 
            new[] { "10" });
        AddTopic(topics, subtopics, "430", "Deferred Revenue", FASBCategory.Liabilities, 
            new[] { "10" }, isSuperseded: true, supersededBy: "ASC 606");
        AddTopic(topics, subtopics, "440", "Commitments", FASBCategory.Liabilities, 
            new[] { "10" });
        AddTopic(topics, subtopics, "450", "Contingencies", FASBCategory.Liabilities, 
            new[] { "10", "20", "30" });
        AddTopic(topics, subtopics, "460", "Guarantees", FASBCategory.Liabilities, 
            new[] { "10" });
        AddTopic(topics, subtopics, "470", "Debt", FASBCategory.Liabilities, 
            new[] { "10", "20", "30", "40", "50", "60" });
        AddTopic(topics, subtopics, "480", "Distinguishing Liabilities from Equity", FASBCategory.Liabilities, 
            new[] { "10" });
    }

    private static void SeedEquityTopics(List<FASBTopic> topics, List<FASBSubtopic> subtopics)
    {
        AddTopic(topics, subtopics, "505", "Equity", FASBCategory.Equity, 
            new[] { "10", "20", "30", "40", "50", "60" });
    }

    private static void SeedRevenueTopics(List<FASBTopic> topics, List<FASBSubtopic> subtopics)
    {
        AddTopic(topics, subtopics, "605", "Revenue Recognition", FASBCategory.Revenue, 
            new[] { "10", "15", "20", "25", "28", "30", "35", "40", "45", "50" }, isSuperseded: true, supersededBy: "ASC 606");
        AddTopic(topics, subtopics, "606", "Revenue from Contracts with Customers", FASBCategory.Revenue, 
            new[] { "10", "20" });
        AddTopic(topics, subtopics, "610", "Other Income", FASBCategory.Revenue, 
            new[] { "10", "20", "30" });
    }

    private static void SeedExpensesTopics(List<FASBTopic> topics, List<FASBSubtopic> subtopics)
    {
        AddTopic(topics, subtopics, "705", "Cost of Sales and Services", FASBCategory.Expenses, 
            new[] { "10", "20" });
        AddTopic(topics, subtopics, "710", "Compensation—General", FASBCategory.Expenses, 
            new[] { "10", "20" });
        AddTopic(topics, subtopics, "712", "Compensation—Nonretirement Postemployment Benefits", FASBCategory.Expenses, 
            new[] { "10", "20" });
        AddTopic(topics, subtopics, "715", "Compensation—Retirement Benefits", FASBCategory.Expenses, 
            new[] { "10", "20", "30", "40", "50", "60", "70", "80" });
        AddTopic(topics, subtopics, "718", "Compensation—Stock Compensation", FASBCategory.Expenses, 
            new[] { "10", "20", "30", "40", "50", "60", "740" });
        AddTopic(topics, subtopics, "720", "Other Expenses", FASBCategory.Expenses, 
            new[] { "10", "15", "20", "25", "30", "35", "40", "45", "50" });
        AddTopic(topics, subtopics, "730", "Research and Development", FASBCategory.Expenses, 
            new[] { "10", "20" });
        AddTopic(topics, subtopics, "740", "Income Taxes", FASBCategory.Expenses, 
            new[] { "10", "20", "30", "270", "323" });
    }

    private static void SeedBroadTransactionsTopics(List<FASBTopic> topics, List<FASBSubtopic> subtopics)
    {
        AddTopic(topics, subtopics, "805", "Business Combinations", FASBCategory.BroadTransactions, 
            new[] { "10", "20", "30", "40", "50", "60", "740", "810", "920" });
        AddTopic(topics, subtopics, "808", "Collaborative Arrangements", FASBCategory.BroadTransactions, 
            new[] { "10" });
        AddTopic(topics, subtopics, "810", "Consolidation", FASBCategory.BroadTransactions, 
            new[] { "10" });
        AddTopic(topics, subtopics, "815", "Derivatives and Hedging", FASBCategory.BroadTransactions, 
            new[] { "10", "15", "20", "25", "30", "35", "40", "45" });
        AddTopic(topics, subtopics, "820", "Fair Value Measurement", FASBCategory.BroadTransactions, 
            new[] { "10" });
        AddTopic(topics, subtopics, "825", "Financial Instruments", FASBCategory.BroadTransactions, 
            new[] { "10" });
        AddTopic(topics, subtopics, "830", "Foreign Currency Matters", FASBCategory.BroadTransactions, 
            new[] { "10", "20", "30", "230" });
        AddTopic(topics, subtopics, "835", "Interest", FASBCategory.BroadTransactions, 
            new[] { "10", "20", "30" });
        AddTopic(topics, subtopics, "840", "Leases", FASBCategory.BroadTransactions, 
            new[] { "10", "20", "30", "40" }, isSuperseded: true, supersededBy: "ASC 842");
        AddTopic(topics, subtopics, "842", "Leases", FASBCategory.BroadTransactions, 
            new[] { "10", "20", "30", "40", "50" });
        AddTopic(topics, subtopics, "845", "Nonmonetary Transactions", FASBCategory.BroadTransactions, 
            new[] { "10" });
        AddTopic(topics, subtopics, "848", "Reference Rate Reform", FASBCategory.BroadTransactions, 
            new[] { "10" });
        AddTopic(topics, subtopics, "850", "Related Party Disclosures", FASBCategory.BroadTransactions, 
            new[] { "10" });
        AddTopic(topics, subtopics, "852", "Reorganizations", FASBCategory.BroadTransactions, 
            new[] { "10", "20" });
        AddTopic(topics, subtopics, "855", "Subsequent Events", FASBCategory.BroadTransactions, 
            new[] { "10" });
        AddTopic(topics, subtopics, "860", "Transfers and Servicing", FASBCategory.BroadTransactions, 
            new[] { "10", "20", "30", "40", "50" });
    }

    private static void SeedIndustryTopics(List<FASBTopic> topics, List<FASBSubtopic> subtopics)
    {
        AddTopic(topics, subtopics, "905", "Agriculture", FASBCategory.Industry, 
            new[] { "10", "330", "360" });
        AddTopic(topics, subtopics, "908", "Airlines", FASBCategory.Industry, 
            new[] { "10", "360" });
        AddTopic(topics, subtopics, "910", "Contractors—Construction", FASBCategory.Industry, 
            new[] { "10", "605" });
        AddTopic(topics, subtopics, "912", "Contractors—Federal Government", FASBCategory.Industry, 
            new[] { "10", "330", "360", "605" });
        AddTopic(topics, subtopics, "920", "Entertainment—Broadcasters", FASBCategory.Industry, 
            new[] { "10", "350", "605" });
        AddTopic(topics, subtopics, "922", "Entertainment—Cable Television", FASBCategory.Industry, 
            new[] { "10", "20" });
        AddTopic(topics, subtopics, "924", "Entertainment—Casinos", FASBCategory.Industry, 
            new[] { "10", "605" });
        AddTopic(topics, subtopics, "926", "Entertainment—Films", FASBCategory.Industry, 
            new[] { "10", "20", "605" });
        AddTopic(topics, subtopics, "928", "Entertainment—Music", FASBCategory.Industry, 
            new[] { "10", "340" });
        AddTopic(topics, subtopics, "930", "Extractive Activities—Mining", FASBCategory.Industry, 
            new[] { "10", "330", "360" });
        AddTopic(topics, subtopics, "932", "Extractive Activities—Oil and Gas", FASBCategory.Industry, 
            new[] { "10", "235", "360" });
        AddTopic(topics, subtopics, "940", "Financial Services—Brokers and Dealers", FASBCategory.Industry, 
            new[] { "10", "310", "320", "405", "605" });
        AddTopic(topics, subtopics, "942", "Financial Services—Depository and Lending", FASBCategory.Industry, 
            new[] { "10", "210", "220", "230", "235", "320", "325", "405", "470", "505", "715", "825" });
        AddTopic(topics, subtopics, "944", "Financial Services—Insurance", FASBCategory.Industry, 
            new[] { "10", "20", "30", "40", "210", "220", "235", "310", "405", "470", "505", "605", "715", "825" });
        AddTopic(topics, subtopics, "946", "Financial Services—Investment Companies", FASBCategory.Industry, 
            new[] { "10", "20", "205", "210", "220", "235", "310", "320", "405", "505", "605", "715", "830" });
        AddTopic(topics, subtopics, "948", "Financial Services—Mortgage Banking", FASBCategory.Industry, 
            new[] { "10", "310", "605" });
        AddTopic(topics, subtopics, "950", "Financial Services—Title Plant", FASBCategory.Industry, 
            new[] { "10", "360" });
        AddTopic(topics, subtopics, "954", "Health Care Entities", FASBCategory.Industry, 
            new[] { "10", "205", "210", "220", "225", "235", "310", "360", "440", "450", "605", "715", "810", "958" });
        AddTopic(topics, subtopics, "958", "Not-for-Profit Entities", FASBCategory.Industry, 
            new[] { "10", "205", "210", "220", "225", "230", "235", "310", "320", "325", "360", "405", "450", "605", "715", "720", "810", "815", "825", "954" });
        AddTopic(topics, subtopics, "960", "Plan Accounting—Defined Benefit Pension Plans", FASBCategory.Industry, 
            new[] { "10", "20", "205", "310", "320", "325" });
        AddTopic(topics, subtopics, "962", "Plan Accounting—Defined Contribution Pension Plans", FASBCategory.Industry, 
            new[] { "10", "205", "310", "320", "325" });
        AddTopic(topics, subtopics, "965", "Plan Accounting—Health and Welfare Benefit Plans", FASBCategory.Industry, 
            new[] { "10", "20", "205", "310", "320", "325" });
        AddTopic(topics, subtopics, "970", "Real Estate—General", FASBCategory.Industry, 
            new[] { "10", "20", "30", "310", "323", "340", "360", "405", "470", "605", "606", "810", "835" });
        AddTopic(topics, subtopics, "972", "Real Estate—Common Interest Realty Associations", FASBCategory.Industry, 
            new[] { "10", "360", "605" });
        AddTopic(topics, subtopics, "974", "Real Estate—Real Estate Investment Trusts", FASBCategory.Industry, 
            new[] { "10", "205", "210", "220", "235", "310", "320", "323", "325", "360", "470", "605", "715", "810", "815" });
        AddTopic(topics, subtopics, "976", "Real Estate—Retail Land", FASBCategory.Industry, 
            new[] { "10", "605" }, isSuperseded: true, supersededBy: "ASC 606");
        AddTopic(topics, subtopics, "978", "Real Estate—Time-Sharing Activities", FASBCategory.Industry, 
            new[] { "10", "310", "360", "605" });
        AddTopic(topics, subtopics, "980", "Regulated Operations", FASBCategory.Industry, 
            new[] { "10", "20", "30", "310", "340", "360", "405", "605" });
        AddTopic(topics, subtopics, "985", "Software", FASBCategory.Industry, 
            new[] { "10", "20", "605" });
    }

    private static void AddTopic(List<FASBTopic> topics, List<FASBSubtopic> subtopics, 
        string topicCode, string topicName, FASBCategory category, string[] subtopicCodes, 
        bool isSuperseded = false, string? supersededBy = null)
    {
        var topicId = Guid.NewGuid();
        var now = DateTime.UtcNow;

        topics.Add(new FASBTopic
        {
            Id = topicId,
            TopicCode = topicCode,
            TopicName = topicName,
            Category = category,
            Description = null,
            IsSuperseded = isSuperseded,
            SupersededBy = supersededBy,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        });

        foreach (var subtopicCode in subtopicCodes)
        {
            subtopics.Add(new FASBSubtopic
            {
                Id = Guid.NewGuid(),
                FASBTopicId = topicId,
                SubtopicCode = subtopicCode,
                SubtopicName = GetSubtopicName(subtopicCode),
                FullReference = $"ASC {topicCode}-{subtopicCode}",
                Description = null,
                IsRepealed = false,
                CreatedAt = now,
                UpdatedAt = now,
                IsDeleted = false
            });
        }
    }

    private static string GetSubtopicName(string subtopicCode)
    {
        return subtopicCode switch
        {
            "10" => "Overall",
            "15" => "Scope and Scope Exceptions",
            "20" => "Specialized Industry Requirements",
            "25" => "Recognition",
            "30" => "Initial Measurement",
            "35" => "Subsequent Measurement",
            "40" => "Derecognition",
            "45" => "Other Presentation Matters",
            "50" => "Disclosure",
            "60" => "Relationships",
            "70" => "Grandfathered Guidance",
            "80" => "Multiemployer Plans",
            "205" => "Presentation",
            "210" => "Balance Sheet",
            "220" => "Income Statement",
            "225" => "Income Statement—Discontinued Operations",
            "230" => "Statement of Cash Flows",
            "235" => "Notes to Financial Statements",
            "270" => "Interim Reporting",
            "310" => "Receivables",
            "320" => "Investments—Debt and Equity Securities",
            "323" => "Investments—Equity Method and Joint Ventures",
            "325" => "Investments—Other",
            "330" => "Inventory",
            "340" => "Other Assets and Deferred Costs",
            "350" => "Intangibles—Goodwill and Other",
            "360" => "Property, Plant, and Equipment",
            "405" => "Liabilities",
            "440" => "Commitments",
            "450" => "Contingencies",
            "470" => "Debt",
            "505" => "Equity",
            "605" => "Revenue Recognition",
            "606" => "Revenue from Contracts with Customers",
            "715" => "Compensation—Retirement Benefits",
            "720" => "Other Expenses",
            "740" => "Income Taxes",
            "810" => "Consolidation",
            "815" => "Derivatives and Hedging",
            "825" => "Financial Instruments",
            "830" => "Foreign Currency Matters",
            "835" => "Interest",
            "920" => "Entertainment—Broadcasters",
            "954" => "Health Care Entities",
            "958" => "Not-for-Profit Entities",
            _ => $"Subtopic {subtopicCode}"
        };
    }
}

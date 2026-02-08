using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FASBTopics",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopicCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TopicName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsSuperseded = table.Column<bool>(type: "bit", nullable: false),
                    SupersededBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FASBTopics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicenseKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Plan = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxEmployees = table.Column<int>(type: "int", nullable: false),
                    CurrentEmployees = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxCompanies = table.Column<int>(type: "int", nullable: false),
                    CurrentCompanies = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MachineId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StripeCustomerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StripeSubscriptionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsAnnualBilling = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastValidatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LockoutUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JournalEntryNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalDebit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalCredit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PostedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntries_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCategories_ProductCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalSchema: "dbo",
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Zip = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    WarehouseType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CapacityUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsSecureVault = table.Column<bool>(type: "bit", nullable: false),
                    IsClimateControlled = table.Column<bool>(type: "bit", nullable: false),
                    Has24HourSecurity = table.Column<bool>(type: "bit", nullable: false),
                    HasAccessControl = table.Column<bool>(type: "bit", nullable: false),
                    CannabisLicense = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouses_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FASBSubtopics",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FASBTopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubtopicCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SubtopicName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FullReference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsRepealed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FASBSubtopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FASBSubtopics_FASBTopics_FASBTopicId",
                        column: x => x.FASBTopicId,
                        principalSchema: "dbo",
                        principalTable: "FASBTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCountTracking",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCountTracking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCountTracking_Licenses_LicenseId",
                        column: x => x.LicenseId,
                        principalSchema: "dbo",
                        principalTable: "Licenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanFeatures",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FeatureName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanFeatures_Licenses_LicenseId",
                        column: x => x.LicenseId,
                        principalSchema: "dbo",
                        principalTable: "Licenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionHistory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Plan = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionHistory_Licenses_LicenseId",
                        column: x => x.LicenseId,
                        principalSchema: "dbo",
                        principalTable: "Licenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                schema: "dbo",
                columns: table => new
                {
                    PermissionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.PermissionsId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalSchema: "dbo",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RolesId",
                        column: x => x.RolesId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Resource = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreviousHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CurrentHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    SequenceNumber = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "dbo",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RolesId",
                        column: x => x.RolesId,
                        principalSchema: "dbo",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAdjustments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdjustmentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AdjustmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdjustedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustments_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "dbo",
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustments_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockTransfers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransferNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromWarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToWarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransferredByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceivedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Warehouses_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Warehouses_ToWarehouseId",
                        column: x => x.ToWarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemAccount = table.Column<bool>(type: "bit", nullable: false),
                    IsCOGS = table.Column<bool>(type: "bit", nullable: false),
                    IsNonDeductible = table.Column<bool>(type: "bit", nullable: false),
                    TaxCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FASBTopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FASBSubtopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FASBReference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_FASBSubtopics_FASBSubtopicId",
                        column: x => x.FASBSubtopicId,
                        principalSchema: "dbo",
                        principalTable: "FASBSubtopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_FASBTopics_FASBTopicId",
                        column: x => x.FASBTopicId,
                        principalSchema: "dbo",
                        principalTable: "FASBTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalCounts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CountDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalItemsCounted = table.Column<int>(type: "int", nullable: false),
                    ItemsWithVariance = table.Column<int>(type: "int", nullable: false),
                    TotalVarianceValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AccuracyRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AdjustmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalCounts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalCounts_InventoryAdjustments_AdjustmentId",
                        column: x => x.AdjustmentId,
                        principalSchema: "dbo",
                        principalTable: "InventoryAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalCounts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentTerms = table.Column<int>(type: "int", nullable: false),
                    AccountsReceivableAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CannabisLicense = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsCannabisCustomer = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Accounts_AccountsReceivableAccountId",
                        column: x => x.AccountsReceivableAccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneralLedgerEntries",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DebitAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreditAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Is280EDeductible = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralLedgerEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralLedgerEntries_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralLedgerEntries_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralLedgerEntries_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "dbo",
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsCannabisProduct = table.Column<bool>(type: "bit", nullable: false),
                    THCPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    CBDPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    StrainType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RequiresBatchTracking = table.Column<bool>(type: "bit", nullable: false),
                    RequiresTestingCertificate = table.Column<bool>(type: "bit", nullable: false),
                    RequiresLicense = table.Column<bool>(type: "bit", nullable: false),
                    CannabisLicense = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TracksExpiration = table.Column<bool>(type: "bit", nullable: false),
                    ShelfLifeDays = table.Column<int>(type: "int", nullable: true),
                    StandardCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RetailPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    WholesalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ReorderPoint = table.Column<int>(type: "int", nullable: false),
                    ReorderQuantity = table.Column<int>(type: "int", nullable: false),
                    LeadTimeDays = table.Column<int>(type: "int", nullable: false),
                    SafetyStock = table.Column<int>(type: "int", nullable: false),
                    MinOrderQuantity = table.Column<int>(type: "int", nullable: false),
                    InventoryAssetAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    COGSAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevenueAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Is280EDeductible = table.Column<bool>(type: "bit", nullable: false),
                    ValuationMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultWarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StorageConditions = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDiscontinued = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Accounts_COGSAccountId",
                        column: x => x.COGSAccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Accounts_InventoryAssetAccountId",
                        column: x => x.InventoryAssetAccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Accounts_RevenueAccountId",
                        column: x => x.RevenueAccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Warehouses_DefaultWarehouseId",
                        column: x => x.DefaultWarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentTerms = table.Column<int>(type: "int", nullable: false),
                    AccountsPayableAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CannabisLicense = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsCannabisVendor = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendors_Accounts_AccountsPayableAccountId",
                        column: x => x.AccountsPayableAccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vendors_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInvoices",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerInvoices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerInvoices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerInvoices_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "dbo",
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SONumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedShipDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromisedShipDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShippingTerms = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipToAddressLine1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShipToAddressLine2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShipToCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShipToState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShipToPostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ShipToCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalShipped = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalInvoiced = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsFullyShipped = table.Column<bool>(type: "bit", nullable: false),
                    IsFullyInvoiced = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SalesRepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesQuoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesQuoteNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerPONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    InternalNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RequiresMetrcTracking = table.Column<bool>(type: "bit", nullable: false),
                    MetrcManifestNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrders_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrders_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryLevels",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityOnHand = table.Column<int>(type: "int", nullable: false),
                    QuantityReserved = table.Column<int>(type: "int", nullable: false),
                    QuantityOnOrder = table.Column<int>(type: "int", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AverageCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastStockDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastCountDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryLevels_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryLevels_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductBatches",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TestingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RemainingQuantity = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualTHC = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ActualCBD = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    TestingPassed = table.Column<bool>(type: "bit", nullable: false),
                    TestingLab = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TestCertificateUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MetrcUID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceLicense = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsQuarantined = table.Column<bool>(type: "bit", nullable: false),
                    QuarantineReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBatches_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductBatches_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VendorBills",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VendorInvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BillDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorBills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorBills_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VendorBills_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "dbo",
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VendorBills_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "dbo",
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLineItems",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLineItems_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceLineItems_CustomerInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "dbo",
                        principalTable: "CustomerInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoicePayments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoicePayments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoicePayments_CustomerInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "dbo",
                        principalTable: "CustomerInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoicePayments_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "dbo",
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderLines",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxPercent = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    QuantityShipped = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    QuantityInvoiced = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RevenueAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_Accounts_RevenueAccountId",
                        column: x => x.RevenueAccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_Products_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_SalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalSchema: "dbo",
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesReturns",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RMANumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReturnType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesReturns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesReturns_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesReturns_CustomerInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: "dbo",
                        principalTable: "CustomerInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesReturns_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesReturns_SalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalSchema: "dbo",
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SOShipments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipmentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Carrier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MetrcManifestNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MetrcManifestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PackedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShippedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ShippedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOShipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SOShipments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SOShipments_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "dbo",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SOShipments_SalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalSchema: "dbo",
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SOShipments_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAdjustmentLines",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdjustmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QuantityBefore = table.Column<int>(type: "int", nullable: false),
                    QuantityAdjustment = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAdjustmentLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustmentLines_InventoryAdjustments_AdjustmentId",
                        column: x => x.AdjustmentId,
                        principalSchema: "dbo",
                        principalTable: "InventoryAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustmentLines_ProductBatches_BatchId",
                        column: x => x.BatchId,
                        principalSchema: "dbo",
                        principalTable: "ProductBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryAdjustmentLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuantityChange = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    QuantityAfter = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TransactedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_ProductBatches_BatchId",
                        column: x => x.BatchId,
                        principalSchema: "dbo",
                        principalTable: "ProductBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransactions_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalCountLines",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SystemQuantity = table.Column<int>(type: "int", nullable: false),
                    CountedQuantity = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalCountLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalCountLines_PhysicalCounts_CountId",
                        column: x => x.CountId,
                        principalSchema: "dbo",
                        principalTable: "PhysicalCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhysicalCountLines_ProductBatches_BatchId",
                        column: x => x.BatchId,
                        principalSchema: "dbo",
                        principalTable: "ProductBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhysicalCountLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockTransferLines",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransferLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransferLines_ProductBatches_BatchId",
                        column: x => x.BatchId,
                        principalSchema: "dbo",
                        principalTable: "ProductBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransferLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransferLines_StockTransfers_TransferId",
                        column: x => x.TransferId,
                        principalSchema: "dbo",
                        principalTable: "StockTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillLineItems",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsCOGS = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillLineItems_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "dbo",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillLineItems_VendorBills_BillId",
                        column: x => x.BillId,
                        principalSchema: "dbo",
                        principalTable: "VendorBills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillPayments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillPayments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillPayments_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "dbo",
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillPayments_VendorBills_BillId",
                        column: x => x.BillId,
                        principalSchema: "dbo",
                        principalTable: "VendorBills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VendorPONumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VendorBillId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_VendorBills_VendorBillId",
                        column: x => x.VendorBillId,
                        principalSchema: "dbo",
                        principalTable: "VendorBills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "dbo",
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesReturnLines",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesReturnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesOrderLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RestockingFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesReturnLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesReturnLines_Products_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesReturnLines_SalesOrderLines_SalesOrderLineId",
                        column: x => x.SalesOrderLineId,
                        principalSchema: "dbo",
                        principalTable: "SalesOrderLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesReturnLines_SalesReturns_SalesReturnId",
                        column: x => x.SalesReturnId,
                        principalSchema: "dbo",
                        principalTable: "SalesReturns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SOShipmentLines",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesOrderLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityShipped = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BatchLotId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BinLocation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOShipmentLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SOShipmentLines_ProductBatches_BatchLotId",
                        column: x => x.BatchLotId,
                        principalSchema: "dbo",
                        principalTable: "ProductBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SOShipmentLines_Products_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SOShipmentLines_SOShipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalSchema: "dbo",
                        principalTable: "SOShipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SOShipmentLines_SalesOrderLines_SalesOrderLineId",
                        column: x => x.SalesOrderLineId,
                        principalSchema: "dbo",
                        principalTable: "SalesOrderLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLines",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    QuantityOrdered = table.Column<int>(type: "int", nullable: false),
                    QuantityReceived = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ExpectedExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "dbo",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockReceipts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    QCPassed = table.Column<bool>(type: "bit", nullable: false),
                    QCNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    JournalEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockReceipts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockReceipts_JournalEntries_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "dbo",
                        principalTable: "JournalEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockReceipts_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "dbo",
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockReceipts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalSchema: "dbo",
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockReceiptLines",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    POLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuantityReceived = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualTHC = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ActualCBD = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    TestingPassed = table.Column<bool>(type: "bit", nullable: true),
                    TestCertificateUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockReceiptLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockReceiptLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockReceiptLines_PurchaseOrderLines_POLineId",
                        column: x => x.POLineId,
                        principalSchema: "dbo",
                        principalTable: "PurchaseOrderLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockReceiptLines_StockReceipts_ReceiptId",
                        column: x => x.ReceiptId,
                        principalSchema: "dbo",
                        principalTable: "StockReceipts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplianceViolations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViolationType = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialImpact = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DetectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplianceViolations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deductions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeductionType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    IsPreTax = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deductions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SSN = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EmploymentType = table.Column<int>(type: "int", nullable: false),
                    Classification = table.Column<int>(type: "int", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SalaryAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PayFrequency = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "dbo",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayPeriods",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    TotalGrossPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalNetPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalTaxes = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalDeductions = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPeriods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayPeriods_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "dbo",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayPeriods_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaxWithholdings",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxYear = table.Column<int>(type: "int", nullable: false),
                    FilingStatus = table.Column<int>(type: "int", nullable: false),
                    FederalAllowances = table.Column<int>(type: "int", nullable: false),
                    FederalExtraWithholding = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StateAllowances = table.Column<int>(type: "int", nullable: false),
                    StateExtraWithholding = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsExemptFederal = table.Column<bool>(type: "bit", nullable: false),
                    IsExemptState = table.Column<bool>(type: "bit", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxWithholdings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxWithholdings_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Timesheets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClockIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClockOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BreakMinutes = table.Column<int>(type: "int", nullable: false),
                    TotalHours = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RegularHours = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OvertimeHours = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DoubleTimeHours = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedById = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timesheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timesheets_Employees_ApprovedById",
                        column: x => x.ApprovedById,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timesheets_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollRecords",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RegularHours = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OvertimeHours = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DoubleTimeHours = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    GrossPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RegularPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OvertimePay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DoubleTimePay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BonusPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CommissionPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FederalTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StateTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SocialSecurityTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MedicareTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalTaxes = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PreTaxDeductions = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PostTaxDeductions = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalDeductions = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NetPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    YTDGrossPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    YTDFederalTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    YTDStateTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    YTDSocialSecurity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    YTDMedicare = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    YTDNetPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollRecords_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dbo",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollRecords_PayPeriods_PayPeriodId",
                        column: x => x.PayPeriodId,
                        principalSchema: "dbo",
                        principalTable: "PayPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollRecordDetails",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollRecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsYTD = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollRecordDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollRecordDetails_PayrollRecords_PayrollRecordId",
                        column: x => x.PayrollRecordId,
                        principalSchema: "dbo",
                        principalTable: "PayrollRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "FASBTopics",
                columns: new[] { "Id", "Category", "CreatedAt", "DeletedAt", "Description", "IsDeleted", "IsSuperseded", "SupersededBy", "TopicCode", "TopicName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("02850fcf-a040-4b84-a68d-4ac86a7983de"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7069), null, null, false, false, null, "928", "Entertainment—Music", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7069) },
                    { new Guid("14971929-85b4-434c-b13c-e83f97e4950f"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3216), null, null, false, false, null, "273", "Corporate Joint Ventures", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3216) },
                    { new Guid("168e5d04-6fc7-425b-a346-f1980d5feda7"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6672), null, null, false, false, null, "908", "Airlines", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6672) },
                    { new Guid("1966c860-0152-4939-b0f6-e784f094344f"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6157), null, null, false, true, "ASC 842", "840", "Leases", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6157) },
                    { new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, false, false, null, "958", "Not-for-Profit Entities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("1b7a6f4c-83f5-4a02-a4ea-5c7536801979"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6911), null, null, false, false, null, "922", "Entertainment—Cable Television", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6911) },
                    { new Guid("1c4739e2-ae7f-461b-8c47-29464fbd6fb0"), "Revenue", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4787), null, null, false, false, null, "610", "Other Income", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4787) },
                    { new Guid("1cfb178a-1edc-4c5f-aaee-26e8cc8ab752"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2851), null, null, false, false, null, "210", "Balance Sheet", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2851) },
                    { new Guid("1d163fec-da53-466a-a7c8-548ee374ec4a"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6094), null, null, false, false, null, "835", "Interest", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6094) },
                    { new Guid("1fce2e37-c411-41ed-86eb-5fd96b14e9e0"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3671), null, null, false, false, null, "330", "Inventory", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3671) },
                    { new Guid("26012a5a-e9fb-46ae-88e9-fe2263a84756"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3086), null, null, false, false, null, "250", "Accounting Changes and Error Corrections", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3086) },
                    { new Guid("2748d8fb-5f4d-4d71-8746-54f3af8ae9af"), "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295), null, null, false, false, null, "470", "Debt", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295) },
                    { new Guid("27de3b74-99d3-42e3-9841-8bd22c5b9a06"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 651, DateTimeKind.Utc).AddTicks(24), null, null, false, false, null, "985", "Software", new DateTime(2026, 2, 8, 1, 39, 11, 651, DateTimeKind.Utc).AddTicks(24) },
                    { new Guid("2ff1fdc5-3b41-4992-b088-4f41a27ad085"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5979), null, null, false, false, null, "825", "Financial Instruments", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5979) },
                    { new Guid("37895a36-59b0-4024-8aaa-84c2678aeecd"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506), null, null, false, false, null, "860", "Transfers and Servicing", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506) },
                    { new Guid("3f2d3526-cb74-4fbb-aaa8-e04944684c96"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3150), null, null, false, false, null, "270", "Interim Reporting", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3150) },
                    { new Guid("40344988-bb55-4cdd-b217-806ac8dec4f5"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9797), null, null, false, false, null, "978", "Real Estate—Time-Sharing Activities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9797) },
                    { new Guid("41315aea-79db-49e3-8516-feec92acb635"), "Equity", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448), null, null, false, false, null, "505", "Equity", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448) },
                    { new Guid("417c4dcb-ee80-4595-9386-eebb125b0026"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7006), null, null, false, false, null, "926", "Entertainment—Films", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7006) },
                    { new Guid("458d7d23-2fb6-4f50-9924-3a93cc4f3b4a"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3442), null, null, false, false, null, "321", "Investments—Equity Securities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3442) },
                    { new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, false, false, null, "974", "Real Estate—Real Estate Investment Trusts", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("480a7ebf-6cd4-4b31-8986-009e191c82b2"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3606), null, null, false, false, null, "326", "Financial Instruments—Credit Losses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3606) },
                    { new Guid("49355be8-175e-4b24-92c8-41e69a8842f8"), "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4169), null, null, false, false, null, "440", "Commitments", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4169) },
                    { new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, false, false, null, "946", "Financial Services—Investment Companies", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, false, false, null, "942", "Financial Services—Depository and Lending", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("4f539477-7b39-486a-8a56-8772de9aab8a"), "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4263), null, null, false, false, null, "460", "Guarantees", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4263) },
                    { new Guid("533dd027-683c-4d18-9f3b-6285bf543401"), "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4138), null, null, false, true, "ASC 606", "430", "Deferred Revenue", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4138) },
                    { new Guid("53c01ca9-976f-4519-a1e2-e0d1d82852d0"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3407), null, null, false, true, "ASC 321 and ASC 326", "320", "Investments—Debt Securities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3407) },
                    { new Guid("566ac39b-be81-4743-a8be-6cd1891ed317"), "Expenses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4898), null, null, false, false, null, "710", "Compensation—General", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4898) },
                    { new Guid("592aaf19-12e0-41e8-813c-f6978ff4566b"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084), null, null, false, false, null, "965", "Plan Accounting—Health and Welfare Benefit Plans", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084) },
                    { new Guid("5a349f7c-7ed1-4398-a35f-8701783ad8d6"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804), null, null, false, false, null, "815", "Derivatives and Hedging", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804) },
                    { new Guid("5b03f40f-6175-4de1-aba9-980588495b4e"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3344), null, null, false, false, null, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3344) },
                    { new Guid("5c1c7c81-3afd-4460-9933-b29e23ae483c"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238), null, null, false, false, null, "842", "Leases", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238) },
                    { new Guid("5d8a495c-89c5-4d74-a69f-0e66b4d18374"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3523), null, null, false, false, null, "325", "Investments—Other", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3523) },
                    { new Guid("62f462c0-31df-4bc9-bf36-34530af5ad62"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6428), null, null, false, false, null, "852", "Reorganizations", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6428) },
                    { new Guid("64347846-2d4a-4f8f-836e-7e4aa5ebbc13"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3013), null, null, false, false, null, "230", "Statement of Cash Flows", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3013) },
                    { new Guid("6543bfbd-914a-4ebf-9dbc-b659da965a65"), "Expenses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269), null, null, false, false, null, "720", "Other Expenses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269) },
                    { new Guid("66a0e322-ad7a-492f-a96a-70d5f5422f17"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6365), null, null, false, false, null, "848", "Reference Rate Reform", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6365) },
                    { new Guid("6b0ec7f5-edb0-445f-86de-cfe630e0e92a"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6333), null, null, false, false, null, "845", "Nonmonetary Transactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6333) },
                    { new Guid("6cdc0bfc-d83b-424e-9be9-d8890647fa99"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9749), null, null, false, true, "ASC 606", "976", "Real Estate—Retail Land", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9749) },
                    { new Guid("6e20c679-339e-434d-974d-c4a3d842712b"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5773), null, null, false, false, null, "810", "Consolidation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5773) },
                    { new Guid("6f278363-8ff4-4ee5-95f9-41aa77805d28"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6766), null, null, false, false, null, "912", "Contractors—Federal Government", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6766) },
                    { new Guid("70237a87-3516-4a85-b2ea-f9561ab7b377"), "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4043), null, null, false, false, null, "410", "Asset Retirement and Environmental Obligations", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4043) },
                    { new Guid("74547aca-c44a-4f42-aaba-c0c10397db51"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3247), null, null, false, false, null, "274", "Personal Financial Statements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3247) },
                    { new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, false, false, null, "944", "Financial Services—Insurance", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("79722770-7977-4825-8e68-6fa9880c89c9"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6011), null, null, false, false, null, "830", "Foreign Currency Matters", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6011) },
                    { new Guid("7cbf4a57-548d-45ca-9aad-96e991104fc6"), "Revenue", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4739), null, null, false, false, null, "606", "Revenue from Contracts with Customers", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4739) },
                    { new Guid("7f4c709c-39ba-49b6-8722-f61f6e8ecd3d"), "Expenses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4851), null, null, false, false, null, "705", "Cost of Sales and Services", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4851) },
                    { new Guid("827fd89a-5eca-4cc3-b206-07186f134963"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249), null, null, false, false, null, "940", "Financial Services—Brokers and Dealers", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249) },
                    { new Guid("893deb2d-9c5c-4e6e-98de-5c261c02109f"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3476), null, null, false, false, null, "323", "Investments—Equity Method and Joint Ventures", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3476) },
                    { new Guid("8980ce4d-95a9-4576-a1a8-8f2948a333dd"), "Expenses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5429), null, null, false, false, null, "730", "Research and Development", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5429) },
                    { new Guid("8a1e39ec-a7ca-47b0-b586-55931ac93001"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7121), null, null, false, false, null, "930", "Extractive Activities—Mining", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7121) },
                    { new Guid("8d28c94e-7389-4766-b69b-88e473161adc"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7185), null, null, false, false, null, "932", "Extractive Activities—Oil and Gas", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7185) },
                    { new Guid("8d585a5a-6fa4-4b4b-99f5-b564c1efcee8"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8091), null, null, false, false, null, "950", "Financial Services—Title Plant", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8091) },
                    { new Guid("8ff77034-41f5-4149-8f5c-bb891ba401dc"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5947), null, null, false, false, null, "820", "Fair Value Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5947) },
                    { new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, false, false, null, "954", "Health Care Entities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("983687a9-c988-441d-807c-728062d0b327"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731), null, null, false, false, null, "960", "Plan Accounting—Defined Benefit Pension Plans", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731) },
                    { new Guid("9879d9c7-8bd3-466e-ac86-41812598eacf"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6719), null, null, false, false, null, "910", "Contractors—Construction", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6719) },
                    { new Guid("9de4b167-0cfa-4694-8933-a6b6cb53e4b6"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2737), null, null, false, false, null, "205", "Presentation of Financial Statements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2737) },
                    { new Guid("9ebbda86-0b68-45a8-8642-72f591e2f62d"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574), null, null, false, false, null, "805", "Business Combinations", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574) },
                    { new Guid("aadfc73e-a03f-45fd-9308-32637f75951f"), "Expenses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992), null, null, false, false, null, "715", "Compensation—Retirement Benefits", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992) },
                    { new Guid("b0aed4af-8064-42ab-a79a-64590c07fb08"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2959), null, null, false, false, null, "225", "Income Statement—Discontinued Operations", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2959) },
                    { new Guid("b1d1f905-76a1-4848-bb6f-6e4b44736e5f"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5741), null, null, false, false, null, "808", "Collaborative Arrangements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5741) },
                    { new Guid("b4b56f83-da49-4b36-bf61-368bb56d4fac"), "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4201), null, null, false, false, null, "450", "Contingencies", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4201) },
                    { new Guid("b4e7a434-175e-45ec-80de-3490478782d6"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8027), null, null, false, false, null, "948", "Financial Services—Mortgage Banking", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8027) },
                    { new Guid("b4f105da-bb4f-43e8-8059-c8d488d7349d"), "Expenses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478), null, null, false, false, null, "740", "Income Taxes", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478) },
                    { new Guid("ba654bf9-01d6-451e-be3b-971df5fb52cd"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3280), null, null, false, false, null, "280", "Segment Reporting", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3280) },
                    { new Guid("bc994793-83a0-4ec1-bf6a-96275e6e0c5e"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6475), null, null, false, false, null, "855", "Subsequent Events", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6475) },
                    { new Guid("bca62dfe-e7ae-425d-a504-778623129c18"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3312), null, null, false, false, null, "305", "Cash and Cash Equivalents", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3312) },
                    { new Guid("bdf61523-544b-4d76-b526-387bdb87de08"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881), null, null, false, false, null, "980", "Regulated Operations", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881) },
                    { new Guid("c583f5bb-3f80-4609-b96d-c4a90a26539c"), "Expenses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141), null, null, false, false, null, "718", "Compensation—Stock Compensation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141) },
                    { new Guid("c755c00c-8bea-49e8-95f6-8da115ebc267"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3703), null, null, false, false, null, "340", "Other Assets and Deferred Costs", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3703) },
                    { new Guid("c8d394ea-ec5d-472c-9dab-f5b9ec701884"), "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4409), null, null, false, false, null, "480", "Distinguishing Liabilities from Equity", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4409) },
                    { new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "Revenue", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, false, true, "ASC 606", "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("d2a4cc6b-6b92-4157-9f69-eabcd79b3238"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3054), null, null, false, false, null, "235", "Notes to Financial Statements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3054) },
                    { new Guid("da357f3d-e650-4e53-a4ec-d819f762f13d"), "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4106), null, null, false, false, null, "420", "Exit or Disposal Cost Obligations", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4106) },
                    { new Guid("da8b03d2-cdd6-4a38-b2c1-125e7938c102"), "Expenses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4945), null, null, false, false, null, "712", "Compensation—Nonretirement Postemployment Benefits", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4945) },
                    { new Guid("dad8f023-e82d-4f14-a3e4-51b77d6fa62d"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3183), null, null, false, false, null, "272", "Limited Liability Entities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3183) },
                    { new Guid("e5d42e70-8a50-428b-97a5-7ceee33e96cb"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783), null, null, false, false, null, "350", "Intangibles—Goodwill and Other", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783) },
                    { new Guid("e763fa56-9d5b-4479-b260-77465d3c4e59"), "Assets", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3894), null, null, false, false, null, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3894) },
                    { new Guid("e9bc16e8-83e8-4ed7-bf20-e888b5e94ba3"), "BroadTransactions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6396), null, null, false, false, null, "850", "Related Party Disclosures", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6396) },
                    { new Guid("eb343ca1-3615-407c-8661-0adcf1957c03"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3118), null, null, false, false, null, "260", "Earnings Per Share", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3118) },
                    { new Guid("ed1f0540-5952-4343-b57c-de52e1c80a4f"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6848), null, null, false, false, null, "920", "Entertainment—Broadcasters", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6848) },
                    { new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, false, false, null, "970", "Real Estate—General", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("f3d8e7cd-dda8-4530-8581-be231a94f80e"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988), null, null, false, false, null, "962", "Plan Accounting—Defined Contribution Pension Plans", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988) },
                    { new Guid("f4e6d3e7-77d5-4e6b-ae6f-d88ccdc94e8f"), "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2911), null, null, false, false, null, "220", "Income Statement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2911) },
                    { new Guid("f547e1b2-e99a-4a6d-a2a4-5d6cc1ae64e6"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6958), null, null, false, false, null, "924", "Entertainment—Casinos", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6958) },
                    { new Guid("f6ca923d-5ad8-462c-9a60-71544249ca18"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9427), null, null, false, false, null, "972", "Real Estate—Common Interest Realty Associations", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9427) },
                    { new Guid("f89b4ec5-f1b9-483b-8769-bb97609d3063"), "Industry", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6607), null, null, false, false, null, "905", "Agriculture", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6607) },
                    { new Guid("f9e27196-8bd4-4b03-8627-ee6862ee919a"), "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942), null, null, false, false, null, "405", "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942) }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "FASBSubtopics",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Description", "FASBTopicId", "FullReference", "IsDeleted", "IsRepealed", "SubtopicCode", "SubtopicName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00b53c84-c987-44bf-ac81-f581678ca406"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2737), null, null, new Guid("9de4b167-0cfa-4694-8933-a6b6cb53e4b6"), "ASC 205-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2737) },
                    { new Guid("00f6239b-7fef-4690-95da-c99f77fe2af0"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-405", false, false, "405", "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("01195ae3-4685-44ab-b708-8580a2405e5d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6958), null, null, new Guid("f547e1b2-e99a-4a6d-a2a4-5d6cc1ae64e6"), "ASC 924-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6958) },
                    { new Guid("013cbbc3-815c-45d4-b4a7-dc26f1da17a5"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3216), null, null, new Guid("14971929-85b4-434c-b13c-e83f97e4950f"), "ASC 273-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3216) },
                    { new Guid("02e8a244-26c8-4757-9467-808f3d44d7b5"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6766), null, null, new Guid("6f278363-8ff4-4ee5-95f9-41aa77805d28"), "ASC 912-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6766) },
                    { new Guid("03cd2bc8-de10-47e1-82ee-f995b710996a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5741), null, null, new Guid("b1d1f905-76a1-4848-bb6f-6e4b44736e5f"), "ASC 808-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5741) },
                    { new Guid("0400d448-a8af-4af7-8d7c-0c3ec3cc3c50"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4106), null, null, new Guid("da357f3d-e650-4e53-a4ec-d819f762f13d"), "ASC 420-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4106) },
                    { new Guid("04d97fda-64cb-4140-872a-6ff9fd108977"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4898), null, null, new Guid("566ac39b-be81-4743-a8be-6cd1891ed317"), "ASC 710-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4898) },
                    { new Guid("05425698-104b-45d3-8a85-2dcd9593029d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "ASC 605-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("0542631d-0585-4008-9cf7-a95b40d89161"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269), null, null, new Guid("6543bfbd-914a-4ebf-9dbc-b659da965a65"), "ASC 720-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269) },
                    { new Guid("07ffa2dd-8790-4e4d-ab97-d20c5103e2a2"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2851), null, null, new Guid("1cfb178a-1edc-4c5f-aaee-26e8cc8ab752"), "ASC 210-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2851) },
                    { new Guid("08470254-a768-4f8b-8331-bc6f08ab998d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804), null, null, new Guid("5a349f7c-7ed1-4398-a35f-8701783ad8d6"), "ASC 815-25", false, false, "25", "Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804) },
                    { new Guid("08ac03e3-4a25-4723-b811-196dd8f8c1a3"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6396), null, null, new Guid("e9bc16e8-83e8-4ed7-bf20-e888b5e94ba3"), "ASC 850-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6396) },
                    { new Guid("095f1d80-2468-4ffb-8261-2d2dc3ca499f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4201), null, null, new Guid("b4b56f83-da49-4b36-bf61-368bb56d4fac"), "ASC 450-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4201) },
                    { new Guid("09d0ebc0-99ed-4005-8035-d70fefb3e097"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("0abaac99-7c7d-4984-be5a-6ea4ded3380c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992), null, null, new Guid("aadfc73e-a03f-45fd-9308-32637f75951f"), "ASC 715-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992) },
                    { new Guid("0b3d2256-402f-4dbe-8c1f-9618a016b408"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6766), null, null, new Guid("6f278363-8ff4-4ee5-95f9-41aa77805d28"), "ASC 912-330", false, false, "330", "Inventory", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6766) },
                    { new Guid("0b442371-8930-4029-aeae-3404331de5ed"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295), null, null, new Guid("2748d8fb-5f4d-4d71-8746-54f3af8ae9af"), "ASC 470-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295) },
                    { new Guid("0cf91eb5-0981-4697-b815-32e4aa574046"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-405", false, false, "405", "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("0e0957b4-7def-49a7-9664-f9bf5f8f5459"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4739), null, null, new Guid("7cbf4a57-548d-45ca-9aad-96e991104fc6"), "ASC 606-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4739) },
                    { new Guid("0e5d5fe8-3974-4884-a496-3d1f3b7f9098"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3312), null, null, new Guid("bca62dfe-e7ae-425d-a504-778623129c18"), "ASC 305-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3312) },
                    { new Guid("0e8e4a44-ae2b-49d2-8571-1f0b4818a6c8"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574), null, null, new Guid("9ebbda86-0b68-45a8-8642-72f591e2f62d"), "ASC 805-920", false, false, "920", "Entertainment—Broadcasters", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574) },
                    { new Guid("0efa2f86-5d7d-4160-830e-473eae5b626b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9797), null, null, new Guid("40344988-bb55-4cdd-b217-806ac8dec4f5"), "ASC 978-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9797) },
                    { new Guid("0f40e1c3-a802-4fb9-908b-cdd0523a5186"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295), null, null, new Guid("2748d8fb-5f4d-4d71-8746-54f3af8ae9af"), "ASC 470-60", false, false, "60", "Relationships", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295) },
                    { new Guid("0f5fa995-0c9e-43f1-8335-ffdfb0f655f2"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("10ead3b3-5b2f-417e-b72f-51694dc42794"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2911), null, null, new Guid("f4e6d3e7-77d5-4e6b-ae6f-d88ccdc94e8f"), "ASC 220-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2911) },
                    { new Guid("11306b61-ef47-4b58-9be4-39a0a75cfd46"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6094), null, null, new Guid("1d163fec-da53-466a-a7c8-548ee374ec4a"), "ASC 835-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6094) },
                    { new Guid("1177f488-9538-4179-8cd7-c90dbdfe46c9"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "ASC 605-15", false, false, "15", "Scope and Scope Exceptions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("12b16693-3cc5-49a1-a389-f7002080da6c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4138), null, null, new Guid("533dd027-683c-4d18-9f3b-6285bf543401"), "ASC 430-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4138) },
                    { new Guid("1384fb9a-2077-4036-b4df-8e18b5edbc92"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574), null, null, new Guid("9ebbda86-0b68-45a8-8642-72f591e2f62d"), "ASC 805-740", false, false, "740", "Income Taxes", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574) },
                    { new Guid("13f23e64-f1ff-4c5c-a24d-a085aec4611e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988), null, null, new Guid("f3d8e7cd-dda8-4530-8581-be231a94f80e"), "ASC 962-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988) },
                    { new Guid("16ecb36e-26d6-4c17-91cd-ac6d859e7ec4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506), null, null, new Guid("37895a36-59b0-4024-8aaa-84c2678aeecd"), "ASC 860-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506) },
                    { new Guid("16f2415a-2598-4c96-9dd0-b73e856e74f3"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-205", false, false, "205", "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("182e4e05-4150-4de5-9611-54c07868370d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-810", false, false, "810", "Consolidation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("190ce7f8-ff94-48f7-b3d3-8acd9f847ac3"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4043), null, null, new Guid("70237a87-3516-4a85-b2ea-f9561ab7b377"), "ASC 410-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4043) },
                    { new Guid("191ee717-4db1-4985-8662-66729f419932"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295), null, null, new Guid("2748d8fb-5f4d-4d71-8746-54f3af8ae9af"), "ASC 470-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295) },
                    { new Guid("198cb15f-5af8-4899-85f5-27183848118b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("19a542ba-fd3d-4caa-93c1-d810338a7e27"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-715", false, false, "715", "Compensation—Retirement Benefits", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("1b9124eb-6640-43f5-bb84-7f01250f2dc2"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881), null, null, new Guid("bdf61523-544b-4d76-b526-387bdb87de08"), "ASC 980-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881) },
                    { new Guid("1dc83d44-75fb-494e-8e1b-2bd9f916200b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804), null, null, new Guid("5a349f7c-7ed1-4398-a35f-8701783ad8d6"), "ASC 815-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804) },
                    { new Guid("1ff006aa-49b2-4f85-8aec-f55793b2dad0"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-320", false, false, "320", "Investments—Debt and Equity Securities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("20b138f0-a2e7-4103-acf0-de551141cc8f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4043), null, null, new Guid("70237a87-3516-4a85-b2ea-f9561ab7b377"), "ASC 410-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4043) },
                    { new Guid("21af1392-3302-4b27-b26d-1a73b670371b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3703), null, null, new Guid("c755c00c-8bea-49e8-95f6-8da115ebc267"), "ASC 340-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3703) },
                    { new Guid("222da3d8-2047-4c15-b0b6-2c9ca9ca7d00"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3054), null, null, new Guid("d2a4cc6b-6b92-4157-9f69-eabcd79b3238"), "ASC 235-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3054) },
                    { new Guid("25c47668-3d31-4b98-9842-47cf82017dac"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7185), null, null, new Guid("8d28c94e-7389-4766-b69b-88e473161adc"), "ASC 932-235", false, false, "235", "Notes to Financial Statements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7185) },
                    { new Guid("265388e0-849f-43f8-98b8-d69d58dbd242"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3344), null, null, new Guid("5b03f40f-6175-4de1-aba9-980588495b4e"), "ASC 310-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3344) },
                    { new Guid("26d092ad-61ae-4629-bb33-c066d0a061d7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6157), null, null, new Guid("1966c860-0152-4939-b0f6-e784f094344f"), "ASC 840-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6157) },
                    { new Guid("26e44ec3-9d6e-4718-beb5-b1f8c3037b8e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6011), null, null, new Guid("79722770-7977-4825-8e68-6fa9880c89c9"), "ASC 830-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6011) },
                    { new Guid("2710afe1-7267-4b62-bff9-2aec53030423"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9797), null, null, new Guid("40344988-bb55-4cdd-b217-806ac8dec4f5"), "ASC 978-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9797) },
                    { new Guid("274ea04c-1fb8-483c-83bf-104e9ed31dfb"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881), null, null, new Guid("bdf61523-544b-4d76-b526-387bdb87de08"), "ASC 980-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881) },
                    { new Guid("27cf801c-7815-4e1f-8ad5-7c05b97ed848"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295), null, null, new Guid("2748d8fb-5f4d-4d71-8746-54f3af8ae9af"), "ASC 470-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295) },
                    { new Guid("27e49312-e3c7-476f-bf92-3741dc83cd3b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269), null, null, new Guid("6543bfbd-914a-4ebf-9dbc-b659da965a65"), "ASC 720-25", false, false, "25", "Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269) },
                    { new Guid("2805c66c-5a62-47d5-a50c-f6324de02e0e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3894), null, null, new Guid("e763fa56-9d5b-4479-b260-77465d3c4e59"), "ASC 360-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3894) },
                    { new Guid("283452b6-7074-4247-b8d9-71785f4da072"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141), null, null, new Guid("c583f5bb-3f80-4609-b96d-c4a90a26539c"), "ASC 718-740", false, false, "740", "Income Taxes", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141) },
                    { new Guid("28a18f0d-5a8b-46f7-87e9-176c3222595f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4851), null, null, new Guid("7f4c709c-39ba-49b6-8722-f61f6e8ecd3d"), "ASC 705-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4851) },
                    { new Guid("2919157e-95c1-41f3-8919-2a38c2f81db4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992), null, null, new Guid("aadfc73e-a03f-45fd-9308-32637f75951f"), "ASC 715-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992) },
                    { new Guid("29665446-7b83-4da5-a3fc-df7306889cc7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-210", false, false, "210", "Balance Sheet", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("297a0bd8-8ca0-457e-81a9-cfc67b55f00c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783), null, null, new Guid("e5d42e70-8a50-428b-97a5-7ceee33e96cb"), "ASC 350-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783) },
                    { new Guid("2b2f0b02-2481-438f-b827-c0ed7ac817ef"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238), null, null, new Guid("5c1c7c81-3afd-4460-9933-b29e23ae483c"), "ASC 842-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238) },
                    { new Guid("2b5ade3e-cddc-4fad-b79b-c804708697b6"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6475), null, null, new Guid("bc994793-83a0-4ec1-bf6a-96275e6e0c5e"), "ASC 855-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6475) },
                    { new Guid("2ca2028b-75ed-4a0b-bdba-eaa5465f19d0"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249), null, null, new Guid("827fd89a-5eca-4cc3-b206-07186f134963"), "ASC 940-405", false, false, "405", "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249) },
                    { new Guid("2cca34ac-964e-4cd4-8e0e-721d74cd0263"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3523), null, null, new Guid("5d8a495c-89c5-4d74-a69f-0e66b4d18374"), "ASC 325-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3523) },
                    { new Guid("2cde94ed-4152-44d2-9e3f-736391eaa82f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-325", false, false, "325", "Investments—Other", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("2de0c6cc-787c-428b-9836-437701f8d294"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-323", false, false, "323", "Investments—Equity Method and Joint Ventures", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("2f2c57c2-5618-4ffa-b759-c93b336e0291"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-320", false, false, "320", "Investments—Debt and Equity Securities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("2f44d9fb-445e-44d1-b982-dca825fca988"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-835", false, false, "835", "Interest", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("2f49ed1c-58dd-4199-992c-201e2520411a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-505", false, false, "505", "Equity", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("3001d827-82aa-40f0-899e-ca33a4d1b0cc"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("30a76aac-870a-4269-b453-9cf32bc192bd"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6719), null, null, new Guid("9879d9c7-8bd3-466e-ac86-41812598eacf"), "ASC 910-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6719) },
                    { new Guid("30d2a0e8-5a35-4aae-9243-6126baa64b2d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-320", false, false, "320", "Investments—Debt and Equity Securities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("31ab2696-0e0c-4cb4-a293-f4825c81f2d2"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731), null, null, new Guid("983687a9-c988-441d-807c-728062d0b327"), "ASC 960-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731) },
                    { new Guid("31cbfe72-fe1b-478b-b3be-66c72962350a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4263), null, null, new Guid("4f539477-7b39-486a-8a56-8772de9aab8a"), "ASC 460-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4263) },
                    { new Guid("33a37130-25ab-4dd2-9e4b-dca32bd44c2a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-220", false, false, "220", "Income Statement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("34b4eec3-1ecc-44ba-b634-381c2ea0e2ce"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4201), null, null, new Guid("b4b56f83-da49-4b36-bf61-368bb56d4fac"), "ASC 450-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4201) },
                    { new Guid("34fff8b8-5d85-4317-8d2a-f1d062917728"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "ASC 605-45", false, false, "45", "Other Presentation Matters", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("35286200-c109-4501-bc51-d912e6fa1165"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6911), null, null, new Guid("1b7a6f4c-83f5-4a02-a4ea-5c7536801979"), "ASC 922-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6911) },
                    { new Guid("36628551-5f6b-4233-9a79-ffb916c81aa4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6672), null, null, new Guid("168e5d04-6fc7-425b-a346-f1980d5feda7"), "ASC 908-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6672) },
                    { new Guid("366ff5f9-ead7-411c-a8a1-d149f89ba64e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3476), null, null, new Guid("893deb2d-9c5c-4e6e-98de-5c261c02109f"), "ASC 323-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3476) },
                    { new Guid("36fbca02-0309-46f0-bbdf-3cfdf8836a2e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506), null, null, new Guid("37895a36-59b0-4024-8aaa-84c2678aeecd"), "ASC 860-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506) },
                    { new Guid("3760ca2c-11c9-41c4-9dec-665dd50de751"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9427), null, null, new Guid("f6ca923d-5ad8-462c-9a60-71544249ca18"), "ASC 972-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9427) },
                    { new Guid("37a64ad2-6d7f-4581-8963-5df437fb50f9"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("37ed7f2b-82c8-46b3-a83a-38764fedf3fc"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3523), null, null, new Guid("5d8a495c-89c5-4d74-a69f-0e66b4d18374"), "ASC 325-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3523) },
                    { new Guid("396b3bc5-0790-4709-8b03-a2a22b61412d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-320", false, false, "320", "Investments—Debt and Equity Securities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("39b46fe9-3b12-4aab-b021-2cab495b7e06"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269), null, null, new Guid("6543bfbd-914a-4ebf-9dbc-b659da965a65"), "ASC 720-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269) },
                    { new Guid("39c99eff-9b01-47b8-8a50-cffd2c6c0e8c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("39dbbba0-c33a-4895-86b3-8d13cf3b9680"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992), null, null, new Guid("aadfc73e-a03f-45fd-9308-32637f75951f"), "ASC 715-80", false, false, "80", "Multiemployer Plans", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992) },
                    { new Guid("3a1d6800-76d4-42b8-b88e-b64b91931f50"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8027), null, null, new Guid("b4e7a434-175e-45ec-80de-3490478782d6"), "ASC 948-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8027) },
                    { new Guid("3a36e6d1-1415-4d68-a5ec-4dab527f40f1"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4787), null, null, new Guid("1c4739e2-ae7f-461b-8c47-29464fbd6fb0"), "ASC 610-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4787) },
                    { new Guid("3ab280f1-63dd-46ec-95dd-ee8c0e369f0c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-830", false, false, "830", "Foreign Currency Matters", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("3b8b696f-bc7a-4972-9c6b-599cc4afed0b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6607), null, null, new Guid("f89b4ec5-f1b9-483b-8769-bb97609d3063"), "ASC 905-330", false, false, "330", "Inventory", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6607) },
                    { new Guid("3c184109-a80c-4f44-95f6-de5c84a4d65e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-220", false, false, "220", "Income Statement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("3c8c3f89-f56e-49d7-8cb1-1855e3540576"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3183), null, null, new Guid("dad8f023-e82d-4f14-a3e4-51b77d6fa62d"), "ASC 272-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3183) },
                    { new Guid("3d80652b-e914-449f-bfe0-2542c895188a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("3d9a3dc9-2630-453f-a7e1-754bcc784c37"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574), null, null, new Guid("9ebbda86-0b68-45a8-8642-72f591e2f62d"), "ASC 805-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574) },
                    { new Guid("3d9a9588-b19e-49ab-acb8-40ce4087a530"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881), null, null, new Guid("bdf61523-544b-4d76-b526-387bdb87de08"), "ASC 980-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881) },
                    { new Guid("3e3f2632-c51d-406e-8117-455b426dcbdd"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-210", false, false, "210", "Balance Sheet", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("3f860153-a0ee-4dfb-b69e-f734e2eb7fc8"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8091), null, null, new Guid("8d585a5a-6fa4-4b4b-99f5-b564c1efcee8"), "ASC 950-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8091) },
                    { new Guid("40ede6ea-83e8-4d44-9bbc-98eeb7b56383"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("41b6edb0-f3ac-4a11-93a5-169b60c8cf35"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7006), null, null, new Guid("417c4dcb-ee80-4595-9386-eebb125b0026"), "ASC 926-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7006) },
                    { new Guid("43b3ea6d-cb97-47f0-b3b1-2bace81189a2"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4945), null, null, new Guid("da8b03d2-cdd6-4a38-b2c1-125e7938c102"), "ASC 712-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4945) },
                    { new Guid("46be626a-a0f3-42db-8673-23908198cd1c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141), null, null, new Guid("c583f5bb-3f80-4609-b96d-c4a90a26539c"), "ASC 718-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141) },
                    { new Guid("4704f11d-770e-46c2-ac21-e5a870ef0153"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-220", false, false, "220", "Income Statement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("4740e5c4-4a0e-4d3b-b136-c60e533705d6"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448), null, null, new Guid("41315aea-79db-49e3-8516-feec92acb635"), "ASC 505-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448) },
                    { new Guid("47502fb3-8383-42e1-b14f-0ab2aa2e1e9f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-715", false, false, "715", "Compensation—Retirement Benefits", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("485ca423-d11b-4c4a-965c-4f9e41940cb4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-954", false, false, "954", "Health Care Entities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("4a0cece8-a58e-4b85-9501-9cf831a1172b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478), null, null, new Guid("b4f105da-bb4f-43e8-8059-c8d488d7349d"), "ASC 740-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478) },
                    { new Guid("4a8b7317-5041-4c89-90f8-c79f049d7e6d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9797), null, null, new Guid("40344988-bb55-4cdd-b217-806ac8dec4f5"), "ASC 978-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9797) },
                    { new Guid("4aa0b28e-f0f0-467f-a20f-e054e89b7d4c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-815", false, false, "815", "Derivatives and Hedging", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("4b5e4ce1-4328-4912-93e2-2ba9f9315bb3"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4851), null, null, new Guid("7f4c709c-39ba-49b6-8722-f61f6e8ecd3d"), "ASC 705-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4851) },
                    { new Guid("4c4adf05-0a94-4f6b-8b17-553957f691aa"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-220", false, false, "220", "Income Statement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("4c6aa8f8-96d3-4b66-88f6-b42017f226df"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804), null, null, new Guid("5a349f7c-7ed1-4398-a35f-8701783ad8d6"), "ASC 815-35", false, false, "35", "Subsequent Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804) },
                    { new Guid("4cce0a3a-b2d1-4028-bd25-fedd16892b85"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3247), null, null, new Guid("74547aca-c44a-4f42-aaba-c0c10397db51"), "ASC 274-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3247) },
                    { new Guid("4d08e2ed-c6f3-49b7-a9cc-0f59ddfd51fa"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("4edb3a77-de70-4cc0-b75e-7d1342ef9aac"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6848), null, null, new Guid("ed1f0540-5952-4343-b57c-de52e1c80a4f"), "ASC 920-350", false, false, "350", "Intangibles—Goodwill and Other", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6848) },
                    { new Guid("4f3301ae-e1b4-4136-8938-df0db0d5838b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992), null, null, new Guid("aadfc73e-a03f-45fd-9308-32637f75951f"), "ASC 715-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992) },
                    { new Guid("4f66dc10-f71f-416e-89a7-520df3619cda"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249), null, null, new Guid("827fd89a-5eca-4cc3-b206-07186f134963"), "ASC 940-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249) },
                    { new Guid("503204d9-0717-43c6-8141-76940bc7210c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6011), null, null, new Guid("79722770-7977-4825-8e68-6fa9880c89c9"), "ASC 830-230", false, false, "230", "Statement of Cash Flows", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6011) },
                    { new Guid("50e05231-1811-4d63-8951-9fbe0733df3a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("5129d7ce-721a-4c99-820f-154ffb0b8a3e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574), null, null, new Guid("9ebbda86-0b68-45a8-8642-72f591e2f62d"), "ASC 805-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574) },
                    { new Guid("51b6e8fb-b233-4cda-b438-8101030c75db"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084), null, null, new Guid("592aaf19-12e0-41e8-813c-f6978ff4566b"), "ASC 965-325", false, false, "325", "Investments—Other", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084) },
                    { new Guid("51dcf5cc-d8f7-4e71-856a-b15ca5d4113b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7185), null, null, new Guid("8d28c94e-7389-4766-b69b-88e473161adc"), "ASC 932-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7185) },
                    { new Guid("538b4d1c-acea-4708-b4a4-8f363d3ba185"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881), null, null, new Guid("bdf61523-544b-4d76-b526-387bdb87de08"), "ASC 980-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881) },
                    { new Guid("538cddbe-f030-47be-880b-310cfa31dc09"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448), null, null, new Guid("41315aea-79db-49e3-8516-feec92acb635"), "ASC 505-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448) },
                    { new Guid("539182fa-3d00-4bee-a606-99aff35f48c4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7069), null, null, new Guid("02850fcf-a040-4b84-a68d-4ac86a7983de"), "ASC 928-340", false, false, "340", "Other Assets and Deferred Costs", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7069) },
                    { new Guid("53b3892a-0bcc-49ce-9ef8-01844134b572"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9427), null, null, new Guid("f6ca923d-5ad8-462c-9a60-71544249ca18"), "ASC 972-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9427) },
                    { new Guid("55089e7d-46c8-4710-9872-cdeb03f11d2b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5773), null, null, new Guid("6e20c679-339e-434d-974d-c4a3d842712b"), "ASC 810-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5773) },
                    { new Guid("56f4e943-faa8-4f43-bd34-4317bbbc1002"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("58b8fa3a-72ef-4789-bec2-9d7845741457"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("58c34bf0-59d5-46a0-87dd-f312ae54e94e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881), null, null, new Guid("bdf61523-544b-4d76-b526-387bdb87de08"), "ASC 980-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881) },
                    { new Guid("5928672e-4e68-4815-b381-ca60063da15b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084), null, null, new Guid("592aaf19-12e0-41e8-813c-f6978ff4566b"), "ASC 965-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084) },
                    { new Guid("5a9e22e4-0c3b-430e-a179-e0ea2131b258"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988), null, null, new Guid("f3d8e7cd-dda8-4530-8581-be231a94f80e"), "ASC 962-205", false, false, "205", "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988) },
                    { new Guid("5aa50992-e268-4626-a48f-1fa4b2971c5c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506), null, null, new Guid("37895a36-59b0-4024-8aaa-84c2678aeecd"), "ASC 860-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506) },
                    { new Guid("5b372cfe-ae82-4123-9f5b-27fdb801ed94"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8027), null, null, new Guid("b4e7a434-175e-45ec-80de-3490478782d6"), "ASC 948-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8027) },
                    { new Guid("5c460e76-4236-432a-94df-3680cdc201cc"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7006), null, null, new Guid("417c4dcb-ee80-4595-9386-eebb125b0026"), "ASC 926-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7006) },
                    { new Guid("5cd4697b-b1f4-43d1-b16d-b38431a8a4dd"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8027), null, null, new Guid("b4e7a434-175e-45ec-80de-3490478782d6"), "ASC 948-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8027) },
                    { new Guid("5cfdc949-6bb9-4dec-a909-0a7f6b041070"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-210", false, false, "210", "Balance Sheet", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("5dcfeab1-e6c2-4e1d-b4e3-27e3bfe505ed"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3703), null, null, new Guid("c755c00c-8bea-49e8-95f6-8da115ebc267"), "ASC 340-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3703) },
                    { new Guid("5e0e7132-d3c3-43f8-a68a-ce4dd854d247"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2959), null, null, new Guid("b0aed4af-8064-42ab-a79a-64590c07fb08"), "ASC 225-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2959) },
                    { new Guid("5ea81ce0-0ac9-465c-bd26-05a047417b69"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-220", false, false, "220", "Income Statement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("6091b7b8-0e33-4070-872a-70e353e5f894"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448), null, null, new Guid("41315aea-79db-49e3-8516-feec92acb635"), "ASC 505-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448) },
                    { new Guid("619baa74-9f11-4c41-8887-91a14fee6e8b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295), null, null, new Guid("2748d8fb-5f4d-4d71-8746-54f3af8ae9af"), "ASC 470-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295) },
                    { new Guid("61e4e0fa-f296-4791-8083-c5ed6bbde298"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6157), null, null, new Guid("1966c860-0152-4939-b0f6-e784f094344f"), "ASC 840-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6157) },
                    { new Guid("623a3de0-5dbe-45d8-9db1-e43fb4363058"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "ASC 605-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("62f3dda1-6ac0-403a-a62b-602d19b1c362"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-440", false, false, "440", "Commitments", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("6347510b-6be1-4d94-991e-c5f2e7cfc661"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3407), null, null, new Guid("53c01ca9-976f-4519-a1e2-e0d1d82852d0"), "ASC 320-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3407) },
                    { new Guid("637952a9-032b-4a45-868e-bd1e5c8a6d21"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-235", false, false, "235", "Notes to Financial Statements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("6522021a-449c-4437-9383-1efdc96997b0"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-715", false, false, "715", "Compensation—Retirement Benefits", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("65a28439-ef3e-4d9c-9974-8c2c3bec68e8"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("6668f806-4800-40d7-b0aa-fbefa0fcc554"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("6746ef16-2e1d-4548-9a83-d4b463298a2c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3442), null, null, new Guid("458d7d23-2fb6-4f50-9924-3a93cc4f3b4a"), "ASC 321-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3442) },
                    { new Guid("6776dc8f-dd8c-4a8c-a139-3a420d90a81f"), new DateTime(2026, 2, 8, 1, 39, 11, 651, DateTimeKind.Utc).AddTicks(24), null, null, new Guid("27de3b74-99d3-42e3-9841-8bd22c5b9a06"), "ASC 985-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 651, DateTimeKind.Utc).AddTicks(24) },
                    { new Guid("68b0c9e4-b511-462a-85e7-c1ab467e2287"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3344), null, null, new Guid("5b03f40f-6175-4de1-aba9-980588495b4e"), "ASC 310-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3344) },
                    { new Guid("68d5cc02-11c1-499a-917c-262e803e59d7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6607), null, null, new Guid("f89b4ec5-f1b9-483b-8769-bb97609d3063"), "ASC 905-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6607) },
                    { new Guid("68f1a0c7-7259-4e16-81b3-37663b625be7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238), null, null, new Guid("5c1c7c81-3afd-4460-9933-b29e23ae483c"), "ASC 842-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238) },
                    { new Guid("68f5afe5-c6b1-49a7-8bc8-c5ee29a78027"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574), null, null, new Guid("9ebbda86-0b68-45a8-8642-72f591e2f62d"), "ASC 805-60", false, false, "60", "Relationships", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574) },
                    { new Guid("69f07ad4-f42d-4b48-869c-0c98ad77ee20"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141), null, null, new Guid("c583f5bb-3f80-4609-b96d-c4a90a26539c"), "ASC 718-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141) },
                    { new Guid("6b22066d-1978-41cb-b7c9-c0f0cfe34679"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-470", false, false, "470", "Debt", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("6b35b5a1-ff08-41b2-b838-bbed97a9a358"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4739), null, null, new Guid("7cbf4a57-548d-45ca-9aad-96e991104fc6"), "ASC 606-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4739) },
                    { new Guid("6c957901-0270-44f9-8324-61d6e2b41080"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6094), null, null, new Guid("1d163fec-da53-466a-a7c8-548ee374ec4a"), "ASC 835-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6094) },
                    { new Guid("6d6696ac-9210-41ad-af30-7fb4f2f8975c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-715", false, false, "715", "Compensation—Retirement Benefits", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("6eec5704-62ef-4361-88c2-b40a62dedca2"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992), null, null, new Guid("aadfc73e-a03f-45fd-9308-32637f75951f"), "ASC 715-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992) },
                    { new Guid("6ef74f3a-cfb0-4c60-a001-bfcefe96f1c1"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9797), null, null, new Guid("40344988-bb55-4cdd-b217-806ac8dec4f5"), "ASC 978-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9797) },
                    { new Guid("70100ff9-f12e-4370-b81c-203a47c43297"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269), null, null, new Guid("6543bfbd-914a-4ebf-9dbc-b659da965a65"), "ASC 720-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269) },
                    { new Guid("7072a768-b38e-459a-8875-918da276f0c8"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3671), null, null, new Guid("1fce2e37-c411-41ed-86eb-5fd96b14e9e0"), "ASC 330-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3671) },
                    { new Guid("70f93321-5b98-43c0-afa2-ac99f6e5b520"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3606), null, null, new Guid("480a7ebf-6cd4-4b31-8986-009e191c82b2"), "ASC 326-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3606) },
                    { new Guid("713c2d20-ccdf-4d1e-8fe1-ae314176c40d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "ASC 605-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("715784d2-6ef9-4016-b5b9-41cfe7e3a2c6"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574), null, null, new Guid("9ebbda86-0b68-45a8-8642-72f591e2f62d"), "ASC 805-810", false, false, "810", "Consolidation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574) },
                    { new Guid("73d8164d-ae7e-4f08-921e-9567000072da"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269), null, null, new Guid("6543bfbd-914a-4ebf-9dbc-b659da965a65"), "ASC 720-45", false, false, "45", "Other Presentation Matters", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269) },
                    { new Guid("748316f3-2d77-4934-8b3a-a5aea474ca62"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-235", false, false, "235", "Notes to Financial Statements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("74cc6e02-b386-41cb-ab77-03f2be75d041"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7069), null, null, new Guid("02850fcf-a040-4b84-a68d-4ac86a7983de"), "ASC 928-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7069) },
                    { new Guid("74cf9159-f34b-4511-8db4-2fdbbc1123ae"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783), null, null, new Guid("e5d42e70-8a50-428b-97a5-7ceee33e96cb"), "ASC 350-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783) },
                    { new Guid("74d0fa3c-a61d-4cb7-b7f3-af02532469e1"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574), null, null, new Guid("9ebbda86-0b68-45a8-8642-72f591e2f62d"), "ASC 805-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574) },
                    { new Guid("7606eb33-1992-4e32-b93a-ee4cd16bfb54"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3703), null, null, new Guid("c755c00c-8bea-49e8-95f6-8da115ebc267"), "ASC 340-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3703) },
                    { new Guid("7807427f-9aff-44b7-aa6b-f24868e02764"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804), null, null, new Guid("5a349f7c-7ed1-4398-a35f-8701783ad8d6"), "ASC 815-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804) },
                    { new Guid("787a5330-5df4-438f-a6f2-b0e5db4ed0f8"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6848), null, null, new Guid("ed1f0540-5952-4343-b57c-de52e1c80a4f"), "ASC 920-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6848) },
                    { new Guid("78b812c1-42ec-46f2-b71c-d3a1958450f0"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("79269d88-f4e9-4cc7-8084-f3f5525fd3c0"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-325", false, false, "325", "Investments—Other", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("7a9403d8-3f15-44d3-b658-d62646263ab4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9749), null, null, new Guid("6cdc0bfc-d83b-424e-9be9-d8890647fa99"), "ASC 976-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9749) },
                    { new Guid("7b0ceb93-56bc-48cf-a4f3-0cd9c2d7523f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-235", false, false, "235", "Notes to Financial Statements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("7be40145-fc1c-4b52-a461-5dd40b9fc16f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("7c0ba2d4-537c-458f-a77a-13fac2548761"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5429), null, null, new Guid("8980ce4d-95a9-4576-a1a8-8f2948a333dd"), "ASC 730-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5429) },
                    { new Guid("7d283e13-7a65-414e-bf15-648015d40200"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731), null, null, new Guid("983687a9-c988-441d-807c-728062d0b327"), "ASC 960-325", false, false, "325", "Investments—Other", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731) },
                    { new Guid("7d9dcdfb-3076-4c9c-ba23-e77802bd9988"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4787), null, null, new Guid("1c4739e2-ae7f-461b-8c47-29464fbd6fb0"), "ASC 610-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4787) },
                    { new Guid("7daec215-17fa-4789-b551-3deee9b023bc"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-810", false, false, "810", "Consolidation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("7e75b5b5-5f1a-45c4-92f8-e29121b05512"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-235", false, false, "235", "Notes to Financial Statements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("7fd6fecb-ee2b-4ea1-8f91-c9a024d82539"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804), null, null, new Guid("5a349f7c-7ed1-4398-a35f-8701783ad8d6"), "ASC 815-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804) },
                    { new Guid("81972da2-8192-437a-a9db-b977e849582f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-405", false, false, "405", "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("81ed6f45-4cf8-4f0a-ac2b-e8c7096edf8d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141), null, null, new Guid("c583f5bb-3f80-4609-b96d-c4a90a26539c"), "ASC 718-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141) },
                    { new Guid("821410b9-d50d-4bcc-b2e3-40e1beb55a19"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-825", false, false, "825", "Financial Instruments", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("823c6619-c206-4a61-b7d9-d7b8284a0a52"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141), null, null, new Guid("c583f5bb-3f80-4609-b96d-c4a90a26539c"), "ASC 718-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141) },
                    { new Guid("82c67225-6175-4aa9-8077-6f383d58f1ef"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6911), null, null, new Guid("1b7a6f4c-83f5-4a02-a4ea-5c7536801979"), "ASC 922-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6911) },
                    { new Guid("82c9ece8-9ad4-47c7-b1d2-57f40f5d8acd"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478), null, null, new Guid("b4f105da-bb4f-43e8-8059-c8d488d7349d"), "ASC 740-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478) },
                    { new Guid("838d0397-f32e-4b24-93e8-f1f3ed02250f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804), null, null, new Guid("5a349f7c-7ed1-4398-a35f-8701783ad8d6"), "ASC 815-15", false, false, "15", "Scope and Scope Exceptions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804) },
                    { new Guid("83d7935b-a775-4227-8da2-8fe13aef426c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8091), null, null, new Guid("8d585a5a-6fa4-4b4b-99f5-b564c1efcee8"), "ASC 950-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8091) },
                    { new Guid("85771918-4fad-4ed1-8184-7bbdba187ddc"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141), null, null, new Guid("c583f5bb-3f80-4609-b96d-c4a90a26539c"), "ASC 718-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141) },
                    { new Guid("85aed8f0-da72-47e0-8c1b-bee242dd97db"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3606), null, null, new Guid("480a7ebf-6cd4-4b31-8986-009e191c82b2"), "ASC 326-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3606) },
                    { new Guid("86475d1e-b54d-4f15-b2fa-1a133d6ff29d"), new DateTime(2026, 2, 8, 1, 39, 11, 651, DateTimeKind.Utc).AddTicks(24), null, null, new Guid("27de3b74-99d3-42e3-9841-8bd22c5b9a06"), "ASC 985-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 651, DateTimeKind.Utc).AddTicks(24) },
                    { new Guid("877578b2-3a83-4953-b3bb-66aa1516d055"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("88a60a94-32b5-4727-846c-921c9f093796"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783), null, null, new Guid("e5d42e70-8a50-428b-97a5-7ceee33e96cb"), "ASC 350-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783) },
                    { new Guid("88c9cbfe-e4bd-406e-b7b7-d4fb2bc879e3"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6719), null, null, new Guid("9879d9c7-8bd3-466e-ac86-41812598eacf"), "ASC 910-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6719) },
                    { new Guid("8ae3d6c6-df54-4211-8650-90d1ef01e521"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4043), null, null, new Guid("70237a87-3516-4a85-b2ea-f9561ab7b377"), "ASC 410-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4043) },
                    { new Guid("8ea3f5cb-032f-4839-b515-a053bc9a7d79"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3476), null, null, new Guid("893deb2d-9c5c-4e6e-98de-5c261c02109f"), "ASC 323-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3476) },
                    { new Guid("8f5ea539-1518-45fd-8f23-7371caa42957"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6672), null, null, new Guid("168e5d04-6fc7-425b-a346-f1980d5feda7"), "ASC 908-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6672) },
                    { new Guid("8fb38b3e-9515-4b89-a6ca-1b1747d5e94f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-810", false, false, "810", "Consolidation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("8fdab407-6f84-41c3-8037-75903aac4c71"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3523), null, null, new Guid("5d8a495c-89c5-4d74-a69f-0e66b4d18374"), "ASC 325-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3523) },
                    { new Guid("909373a5-ba86-4924-8eb4-3abb6a6d89d4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2959), null, null, new Guid("b0aed4af-8064-42ab-a79a-64590c07fb08"), "ASC 225-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2959) },
                    { new Guid("912f3903-fbe9-469e-846d-4c5252cf71f7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("92107b7b-1eac-46e0-bcbb-7deabdb07eb6"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-205", false, false, "205", "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("921a9879-fbf1-4a76-8a0a-dc6a5ee36840"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6428), null, null, new Guid("62f462c0-31df-4bc9-bf36-34530af5ad62"), "ASC 852-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6428) },
                    { new Guid("9220c35e-4d41-4d93-b46f-170519ced6a8"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9749), null, null, new Guid("6cdc0bfc-d83b-424e-9be9-d8890647fa99"), "ASC 976-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9749) },
                    { new Guid("93a492fa-7f77-4a56-b3f2-6b6aaa0b2611"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6848), null, null, new Guid("ed1f0540-5952-4343-b57c-de52e1c80a4f"), "ASC 920-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6848) },
                    { new Guid("94b843dc-6301-4f05-b22c-3605ac081a59"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "ASC 605-28", false, false, "28", "Subtopic 28", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("95a0bc1e-f100-44dc-9432-c1c97bc8f2c9"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-210", false, false, "210", "Balance Sheet", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("96c6241c-3806-45b7-af8a-13ad8f669a19"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("97654883-c393-4971-b0d3-c041360c8439"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448), null, null, new Guid("41315aea-79db-49e3-8516-feec92acb635"), "ASC 505-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448) },
                    { new Guid("97ce123f-6608-4606-b567-1b6409ecac41"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("98b94bce-9093-4acc-8974-be9bb82202b4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5429), null, null, new Guid("8980ce4d-95a9-4576-a1a8-8f2948a333dd"), "ASC 730-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5429) },
                    { new Guid("9a4cc363-5755-4729-8755-6931bbecfd15"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-205", false, false, "205", "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("9a5daba2-cf76-470a-9a12-f766d2e6cc6f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-825", false, false, "825", "Financial Instruments", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("9a7baea8-70c7-4259-8cf0-ba1d267eba77"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-225", false, false, "225", "Income Statement—Discontinued Operations", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("9b92ff60-a6d0-42ba-97db-441b58360bcf"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783), null, null, new Guid("e5d42e70-8a50-428b-97a5-7ceee33e96cb"), "ASC 350-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783) },
                    { new Guid("9bcefa32-7b43-41ea-b722-a206665c562e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478), null, null, new Guid("b4f105da-bb4f-43e8-8059-c8d488d7349d"), "ASC 740-270", false, false, "270", "Interim Reporting", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478) },
                    { new Guid("9c083f90-6df2-45cd-81cf-4db9a0f43a9a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448), null, null, new Guid("41315aea-79db-49e3-8516-feec92acb635"), "ASC 505-60", false, false, "60", "Relationships", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448) },
                    { new Guid("9c133f2f-a827-44bd-a0ff-ae81b58441dc"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-450", false, false, "450", "Contingencies", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("9c20f5fc-774e-499b-b7b3-fedd5a16e723"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506), null, null, new Guid("37895a36-59b0-4024-8aaa-84c2678aeecd"), "ASC 860-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506) },
                    { new Guid("9ca64863-659a-4f1c-904a-2acfc363761b"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-230", false, false, "230", "Statement of Cash Flows", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("9cd81a98-be99-469d-9364-494b68e22e53"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7121), null, null, new Guid("8a1e39ec-a7ca-47b0-b586-55931ac93001"), "ASC 930-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7121) },
                    { new Guid("9d9a095d-5901-42d5-92fb-d35eb895f5d6"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("9ee9ea3f-68f8-43d9-baec-8578722c1088"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("9f074fbe-5184-49c8-ae6d-8d409742a11f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269), null, null, new Guid("6543bfbd-914a-4ebf-9dbc-b659da965a65"), "ASC 720-35", false, false, "35", "Subsequent Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269) },
                    { new Guid("9f779561-00fa-43cf-9170-e3897f50cac5"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3523), null, null, new Guid("5d8a495c-89c5-4d74-a69f-0e66b4d18374"), "ASC 325-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3523) },
                    { new Guid("9fef26e0-c9c6-49f5-9827-cfd093647bb9"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506), null, null, new Guid("37895a36-59b0-4024-8aaa-84c2678aeecd"), "ASC 860-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6506) },
                    { new Guid("a0efaba0-904b-4653-9f89-ea9626c41b00"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-450", false, false, "450", "Contingencies", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("a3961ac6-197d-4622-9754-806ca26e33b7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269), null, null, new Guid("6543bfbd-914a-4ebf-9dbc-b659da965a65"), "ASC 720-15", false, false, "15", "Scope and Scope Exceptions", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269) },
                    { new Guid("a516bfad-ca7f-43de-af22-9e0b9aed12c5"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3150), null, null, new Guid("3f2d3526-cb74-4fbb-aaa8-e04944684c96"), "ASC 270-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3150) },
                    { new Guid("a5826db8-44fa-4867-8852-91a2296eafa4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6157), null, null, new Guid("1966c860-0152-4939-b0f6-e784f094344f"), "ASC 840-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6157) },
                    { new Guid("a5bf0e7a-7cd8-4578-84f5-ef8685555972"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084), null, null, new Guid("592aaf19-12e0-41e8-813c-f6978ff4566b"), "ASC 965-320", false, false, "320", "Investments—Debt and Equity Securities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084) },
                    { new Guid("a5dfbc31-837d-4cfb-8b61-6cd4cad4125a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2737), null, null, new Guid("9de4b167-0cfa-4694-8933-a6b6cb53e4b6"), "ASC 205-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2737) },
                    { new Guid("a61b5f8a-5d17-4d8e-9b09-c3d2785869ce"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992), null, null, new Guid("aadfc73e-a03f-45fd-9308-32637f75951f"), "ASC 715-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992) },
                    { new Guid("a6f7fbbb-1452-4a13-a571-156fa5760715"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3086), null, null, new Guid("26012a5a-e9fb-46ae-88e9-fe2263a84756"), "ASC 250-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3086) },
                    { new Guid("a858a579-cfa1-4517-bd43-fdc32105eb95"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5947), null, null, new Guid("8ff77034-41f5-4149-8f5c-bb891ba401dc"), "ASC 820-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5947) },
                    { new Guid("a862ae35-85d8-415b-8b5f-0871b67b0d62"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("aa073ec9-e014-49b7-a8b5-1e72d84ea03a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-405", false, false, "405", "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("aa9400cb-0120-4ee0-b675-a64074c280e8"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7121), null, null, new Guid("8a1e39ec-a7ca-47b0-b586-55931ac93001"), "ASC 930-330", false, false, "330", "Inventory", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7121) },
                    { new Guid("ab327cda-9681-4b8a-9bc1-d99b7d511fd4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6428), null, null, new Guid("62f462c0-31df-4bc9-bf36-34530af5ad62"), "ASC 852-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6428) },
                    { new Guid("abbf4f5a-4299-4762-bacc-ce7fb70fa159"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084), null, null, new Guid("592aaf19-12e0-41e8-813c-f6978ff4566b"), "ASC 965-205", false, false, "205", "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084) },
                    { new Guid("ac764775-80b5-4b9c-ade7-9396c37a2f89"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881), null, null, new Guid("bdf61523-544b-4d76-b526-387bdb87de08"), "ASC 980-340", false, false, "340", "Other Assets and Deferred Costs", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881) },
                    { new Guid("ad450c93-b3e5-470b-b312-482f41514326"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6094), null, null, new Guid("1d163fec-da53-466a-a7c8-548ee374ec4a"), "ASC 835-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6094) },
                    { new Guid("ae295c3b-d010-4113-bd14-16e556fa588c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295), null, null, new Guid("2748d8fb-5f4d-4d71-8746-54f3af8ae9af"), "ASC 470-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4295) },
                    { new Guid("ae837e60-fe60-41a9-a519-f2a278753ef6"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269), null, null, new Guid("6543bfbd-914a-4ebf-9dbc-b659da965a65"), "ASC 720-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269) },
                    { new Guid("af3a78be-5af5-41ab-a156-0da4c4d67610"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238), null, null, new Guid("5c1c7c81-3afd-4460-9933-b29e23ae483c"), "ASC 842-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238) },
                    { new Guid("af6fe54f-6db4-404d-a64e-943f68613085"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4409), null, null, new Guid("c8d394ea-ec5d-472c-9dab-f5b9ec701884"), "ASC 480-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4409) },
                    { new Guid("b0c4bf56-726a-4c4e-a7b5-2cbc0417109f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942), null, null, new Guid("f9e27196-8bd4-4b03-8627-ee6862ee919a"), "ASC 405-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942) },
                    { new Guid("b0dfc30a-d475-4467-aad4-936b47e5928f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "ASC 605-35", false, false, "35", "Subsequent Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("b1e6b4ca-e6c1-4efb-8c67-4bd88976399f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-235", false, false, "235", "Notes to Financial Statements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("b2231718-1a6b-4aa6-8bff-7199026a6066"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-505", false, false, "505", "Equity", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("b2449063-1ac6-4042-8db4-0c2ea4ffaec4"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141), null, null, new Guid("c583f5bb-3f80-4609-b96d-c4a90a26539c"), "ASC 718-60", false, false, "60", "Relationships", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5141) },
                    { new Guid("b3f01b7d-1498-45b2-9359-5fad7523b298"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992), null, null, new Guid("aadfc73e-a03f-45fd-9308-32637f75951f"), "ASC 715-70", false, false, "70", "Grandfathered Guidance", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992) },
                    { new Guid("b488dbdd-2c05-471b-a1f5-7708ccb7f649"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988), null, null, new Guid("f3d8e7cd-dda8-4530-8581-be231a94f80e"), "ASC 962-325", false, false, "325", "Investments—Other", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988) },
                    { new Guid("b4b0b21b-1d35-4cdd-8ce7-4008291d5308"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574), null, null, new Guid("9ebbda86-0b68-45a8-8642-72f591e2f62d"), "ASC 805-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574) },
                    { new Guid("b54c514e-64ae-4dd4-b855-373347c88ea7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3344), null, null, new Guid("5b03f40f-6175-4de1-aba9-980588495b4e"), "ASC 310-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3344) },
                    { new Guid("b62bec2c-5279-4827-9a7d-a937f37aeb7d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783), null, null, new Guid("e5d42e70-8a50-428b-97a5-7ceee33e96cb"), "ASC 350-60", false, false, "60", "Relationships", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783) },
                    { new Guid("b6c09240-e994-43bd-ad86-8e564bdcb72f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-505", false, false, "505", "Equity", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("b835d701-e14b-4984-b1fc-7204dd304600"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084), null, null, new Guid("592aaf19-12e0-41e8-813c-f6978ff4566b"), "ASC 965-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084) },
                    { new Guid("b8b6fef5-8420-4a9b-bab9-12682c3ab011"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2911), null, null, new Guid("f4e6d3e7-77d5-4e6b-ae6f-d88ccdc94e8f"), "ASC 220-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2911) },
                    { new Guid("b9b58644-3ae4-4207-99ca-0142e06ec8ef"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6333), null, null, new Guid("6b0ec7f5-edb0-445f-86de-cfe630e0e92a"), "ASC 845-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6333) },
                    { new Guid("b9ca31c1-ba4e-4a7f-b683-15471fabc2dc"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-815", false, false, "815", "Derivatives and Hedging", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("bc04a3ac-625f-484b-9ef2-8844d9158496"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("bc2daf3d-23b8-4105-89c0-8d6f5d907632"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-470", false, false, "470", "Debt", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("bc42b663-cb3b-47f2-b571-2bfcd98e50bd"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269), null, null, new Guid("6543bfbd-914a-4ebf-9dbc-b659da965a65"), "ASC 720-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5269) },
                    { new Guid("bc62ae41-4515-4836-9d7d-69e99e5de662"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804), null, null, new Guid("5a349f7c-7ed1-4398-a35f-8701783ad8d6"), "ASC 815-45", false, false, "45", "Other Presentation Matters", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804) },
                    { new Guid("be2cfd2e-c362-4298-90a4-21835c9653ea"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731), null, null, new Guid("983687a9-c988-441d-807c-728062d0b327"), "ASC 960-320", false, false, "320", "Investments—Debt and Equity Securities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731) },
                    { new Guid("bf5f0375-ce78-48d8-92e6-5c1e8a9371f7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-606", false, false, "606", "Revenue from Contracts with Customers", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("bf8bf86c-65a2-45c4-8215-6b0d425b9f5a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942), null, null, new Guid("f9e27196-8bd4-4b03-8627-ee6862ee919a"), "ASC 405-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942) },
                    { new Guid("c0146b37-e885-4c0b-a678-13a8100e5166"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881), null, null, new Guid("bdf61523-544b-4d76-b526-387bdb87de08"), "ASC 980-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881) },
                    { new Guid("c22ef2ce-42b2-40b7-9e2c-ee2d233c0952"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9427), null, null, new Guid("f6ca923d-5ad8-462c-9a60-71544249ca18"), "ASC 972-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9427) },
                    { new Guid("c2ba7c4a-45fb-4776-9653-31e0744772c1"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942), null, null, new Guid("f9e27196-8bd4-4b03-8627-ee6862ee919a"), "ASC 405-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942) },
                    { new Guid("c568b2d6-31d2-4bf7-acb4-1952c95aef58"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-210", false, false, "210", "Balance Sheet", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("c7e64be7-5404-4437-9cf2-70afb864dda5"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249), null, null, new Guid("827fd89a-5eca-4cc3-b206-07186f134963"), "ASC 940-320", false, false, "320", "Investments—Debt and Equity Securities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249) },
                    { new Guid("c8545e44-e87a-4f45-975a-29762bff2028"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("c85bfac6-24c5-431d-abd2-3f390570aca2"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731), null, null, new Guid("983687a9-c988-441d-807c-728062d0b327"), "ASC 960-205", false, false, "205", "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731) },
                    { new Guid("c9d16fd5-9176-47d7-bf50-eb0028fedd40"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6157), null, null, new Guid("1966c860-0152-4939-b0f6-e784f094344f"), "ASC 840-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6157) },
                    { new Guid("c9fb9e31-1ab6-4cfd-b621-661cf746940f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478), null, null, new Guid("b4f105da-bb4f-43e8-8059-c8d488d7349d"), "ASC 740-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478) },
                    { new Guid("ca7f3885-7582-4e0c-bbe0-8e2a236e7d80"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6958), null, null, new Guid("f547e1b2-e99a-4a6d-a2a4-5d6cc1ae64e6"), "ASC 924-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6958) },
                    { new Guid("cc0db1bb-6a4c-468c-9c0a-563be8c863a6"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4945), null, null, new Guid("da8b03d2-cdd6-4a38-b2c1-125e7938c102"), "ASC 712-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4945) },
                    { new Guid("ce169fb7-4211-417a-a169-36c06a69ce2e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4169), null, null, new Guid("49355be8-175e-4b24-92c8-41e69a8842f8"), "ASC 440-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4169) },
                    { new Guid("ceb91611-af31-4d9d-8134-9615fe918428"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238), null, null, new Guid("5c1c7c81-3afd-4460-9933-b29e23ae483c"), "ASC 842-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238) },
                    { new Guid("cefcca41-d6c6-415d-a7ab-18c378a6c74e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249), null, null, new Guid("827fd89a-5eca-4cc3-b206-07186f134963"), "ASC 940-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249) },
                    { new Guid("cf871af6-219d-45cd-a626-2fc375ae012d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-810", false, false, "810", "Consolidation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("cf895ccc-3995-4669-9ca1-0e7a5284fb48"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731), null, null, new Guid("983687a9-c988-441d-807c-728062d0b327"), "ASC 960-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731) },
                    { new Guid("cfd6412a-4a37-4779-9ff6-5a61931107fa"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-720", false, false, "720", "Other Expenses", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("cfff8182-fa90-4891-a7a1-6b89ebdefa5f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-325", false, false, "325", "Investments—Other", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("d02c549e-ffe4-40b1-a3ca-9dc77a57680e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3013), null, null, new Guid("64347846-2d4a-4f8f-836e-7e4aa5ebbc13"), "ASC 230-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3013) },
                    { new Guid("d0e7e724-f806-4f95-aabb-46dce642725f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "ASC 605-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("d392c0b2-13d7-4940-99b3-5744f9da469a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881), null, null, new Guid("bdf61523-544b-4d76-b526-387bdb87de08"), "ASC 980-405", false, false, "405", "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9881) },
                    { new Guid("d40de4a3-e885-4353-bce3-44b5256d6dda"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2851), null, null, new Guid("1cfb178a-1edc-4c5f-aaee-26e8cc8ab752"), "ASC 210-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2851) },
                    { new Guid("d7c10a58-0997-4cff-a7e0-d4068d53b472"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-210", false, false, "210", "Balance Sheet", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("d84f4df5-677a-4064-bfba-db9be8a2866d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-470", false, false, "470", "Debt", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("d8b3f16f-e5f2-4e2f-847e-22bc0c0e07f3"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("d8d05df4-6297-454f-a3af-c9f14ba8e3d3"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3894), null, null, new Guid("e763fa56-9d5b-4479-b260-77465d3c4e59"), "ASC 360-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3894) },
                    { new Guid("d96a746c-bf7a-449d-8c5c-7d30b76e5015"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6766), null, null, new Guid("6f278363-8ff4-4ee5-95f9-41aa77805d28"), "ASC 912-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6766) },
                    { new Guid("d9a00f84-ac03-4d89-b48a-0883542f612d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3606), null, null, new Guid("480a7ebf-6cd4-4b31-8986-009e191c82b2"), "ASC 326-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3606) },
                    { new Guid("da1afd4c-6c56-4a80-a7f4-5a215dc11337"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783), null, null, new Guid("e5d42e70-8a50-428b-97a5-7ceee33e96cb"), "ASC 350-50", false, false, "50", "Disclosure", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3783) },
                    { new Guid("da214900-aaf9-4b1c-a248-67499a294884"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2737), null, null, new Guid("9de4b167-0cfa-4694-8933-a6b6cb53e4b6"), "ASC 205-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(2737) },
                    { new Guid("dac3a967-393a-41d8-a21c-971899d2a1e7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3703), null, null, new Guid("c755c00c-8bea-49e8-95f6-8da115ebc267"), "ASC 340-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3703) },
                    { new Guid("dafa60a8-63cf-48a1-b873-8c818399e0ce"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "ASC 605-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("db26fe81-1012-4879-8941-7c27c1a5b839"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-715", false, false, "715", "Compensation—Retirement Benefits", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("dd2d9be3-7438-47eb-bd09-f7fd20bf4b8e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988), null, null, new Guid("f3d8e7cd-dda8-4530-8581-be231a94f80e"), "ASC 962-320", false, false, "320", "Investments—Debt and Equity Securities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988) },
                    { new Guid("dda579a4-2d9d-4815-bf3e-172928be944a"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4201), null, null, new Guid("b4b56f83-da49-4b36-bf61-368bb56d4fac"), "ASC 450-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4201) },
                    { new Guid("e0264091-b542-459b-86b8-688cdd345b87"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942), null, null, new Guid("f9e27196-8bd4-4b03-8627-ee6862ee919a"), "ASC 405-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942) },
                    { new Guid("e0d47c2d-ecdb-4e06-841c-824b5ee4f537"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6011), null, null, new Guid("79722770-7977-4825-8e68-6fa9880c89c9"), "ASC 830-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6011) },
                    { new Guid("e1652ccf-b3e0-489a-9f22-a2973b1c0dc3"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-323", false, false, "323", "Investments—Equity Method and Joint Ventures", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("e19133c4-0bf4-4226-ab97-a941e7718956"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5979), null, null, new Guid("2ff1fdc5-3b41-4992-b088-4f41a27ad085"), "ASC 825-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5979) },
                    { new Guid("e32d0fec-ec12-42a6-b3df-ec2521e7196c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238), null, null, new Guid("5c1c7c81-3afd-4460-9933-b29e23ae483c"), "ASC 842-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6238) },
                    { new Guid("e83e9951-fe5e-4db9-a8af-5a06d1b6c949"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-958", false, false, "958", "Not-for-Profit Entities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("e9860c4f-a5ca-45e7-8324-974d1091ca2f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6365), null, null, new Guid("66a0e322-ad7a-492f-a96a-70d5f5422f17"), "ASC 848-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6365) },
                    { new Guid("e9b997e3-9ef7-4e6f-8221-6b9892f5911d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-715", false, false, "715", "Compensation—Retirement Benefits", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("ea8e2016-3161-42ec-9ad2-1214cd2de656"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561), null, null, new Guid("c95d5647-c98f-44ab-8c93-f0a55a30320d"), "ASC 605-25", false, false, "25", "Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4561) },
                    { new Guid("eb4bbfcd-8044-48de-acc5-1ea8900025b5"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-405", false, false, "405", "Liabilities", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("eb81d492-65fb-4c74-819d-116c37a14079"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478), null, null, new Guid("b4f105da-bb4f-43e8-8059-c8d488d7349d"), "ASC 740-323", false, false, "323", "Investments—Equity Method and Joint Ventures", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5478) },
                    { new Guid("ebb593a5-db25-4b22-9379-f52f33a3ecc8"), new DateTime(2026, 2, 8, 1, 39, 11, 651, DateTimeKind.Utc).AddTicks(24), null, null, new Guid("27de3b74-99d3-42e3-9841-8bd22c5b9a06"), "ASC 985-20", false, false, "20", "Specialized Industry Requirements", new DateTime(2026, 2, 8, 1, 39, 11, 651, DateTimeKind.Utc).AddTicks(24) },
                    { new Guid("ec8957bc-cd48-4972-b4fb-432b5d7481fa"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6011), null, null, new Guid("79722770-7977-4825-8e68-6fa9880c89c9"), "ASC 830-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6011) },
                    { new Guid("edf4353a-3cee-4240-9547-6026f3cf8584"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-40", false, false, "40", "Derecognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("ee7da4c9-2852-4c51-a6fa-6babfa283052"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7121), null, null, new Guid("8a1e39ec-a7ca-47b0-b586-55931ac93001"), "ASC 930-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7121) },
                    { new Guid("ef04e21d-b408-4427-914b-87a42b141987"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448), null, null, new Guid("41315aea-79db-49e3-8516-feec92acb635"), "ASC 505-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4448) },
                    { new Guid("f0d1e1e5-82b3-43cb-91df-f562724b08de"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804), null, null, new Guid("5a349f7c-7ed1-4398-a35f-8701783ad8d6"), "ASC 815-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5804) },
                    { new Guid("f0f85c3c-8f2b-4ed4-8c4f-2150d4d07383"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988), null, null, new Guid("f3d8e7cd-dda8-4530-8581-be231a94f80e"), "ASC 962-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8988) },
                    { new Guid("f23c7564-3ebb-4242-b3ae-431381a3f731"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249), null, null, new Guid("827fd89a-5eca-4cc3-b206-07186f134963"), "ASC 940-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7249) },
                    { new Guid("f2a2ac24-04d1-4e98-bdd1-8bfe954dc7da"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3118), null, null, new Guid("eb343ca1-3615-407c-8661-0adcf1957c03"), "ASC 260-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3118) },
                    { new Guid("f34bdb16-5084-44a1-8f57-9b8260031c40"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140), null, null, new Guid("9034bd9e-d80c-4ba1-9b79-41902a4a9d20"), "ASC 954-225", false, false, "225", "Income Statement—Discontinued Operations", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8140) },
                    { new Guid("f36418b7-f9ee-4f2c-beeb-d2ccbf4e3b69"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574), null, null, new Guid("9ebbda86-0b68-45a8-8642-72f591e2f62d"), "ASC 805-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(5574) },
                    { new Guid("f4738d12-ad24-4528-a246-b2fa8202f820"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3280), null, null, new Guid("ba654bf9-01d6-451e-be3b-971df5fb52cd"), "ASC 280-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3280) },
                    { new Guid("f5a8db0d-df42-4144-9ced-4e0d41763928"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-605", false, false, "605", "Revenue Recognition", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("f669f298-aee7-4599-8c6c-b6d0458df271"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4898), null, null, new Guid("566ac39b-be81-4743-a8be-6cd1891ed317"), "ASC 710-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4898) },
                    { new Guid("f6a8515d-edb1-4f22-b9a6-a0d452cf5b93"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347), null, null, new Guid("4d789e26-f8d0-470a-820b-4aa2b108fde0"), "ASC 942-825", false, false, "825", "Financial Instruments", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7347) },
                    { new Guid("f6ff5909-d975-4d72-8e2f-7ca2b39ad049"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942), null, null, new Guid("f9e27196-8bd4-4b03-8627-ee6862ee919a"), "ASC 405-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(3942) },
                    { new Guid("f790efd5-915d-43b4-bf3d-66470781fb30"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084), null, null, new Guid("592aaf19-12e0-41e8-813c-f6978ff4566b"), "ASC 965-310", false, false, "310", "Receivables", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9084) },
                    { new Guid("f7a3a613-268e-4386-a57d-18806a8a680d"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556), null, null, new Guid("77bdd9c5-ae8b-4ec4-b773-dd69bcee5b05"), "ASC 944-470", false, false, "470", "Debt", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7556) },
                    { new Guid("f88ca7fd-9c56-4deb-a9a9-a903755c0935"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-220", false, false, "220", "Income Statement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("f9a7f5ae-958f-4730-9378-855726b4bd8f"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4787), null, null, new Guid("1c4739e2-ae7f-461b-8c47-29464fbd6fb0"), "ASC 610-30", false, false, "30", "Initial Measurement", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4787) },
                    { new Guid("f9c86ba0-89cc-4256-b150-66888d8efe64"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6607), null, null, new Guid("f89b4ec5-f1b9-483b-8769-bb97609d3063"), "ASC 905-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6607) },
                    { new Guid("f9e4eeca-4cb7-463c-bc8a-34777eb15f90"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196), null, null, new Guid("f26466a0-1fde-4e61-8217-21d3674a4a5c"), "ASC 970-340", false, false, "340", "Other Assets and Deferred Costs", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9196) },
                    { new Guid("fa5802d2-e851-43b3-8a68-6582d6a558f7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7006), null, null, new Guid("417c4dcb-ee80-4595-9386-eebb125b0026"), "ASC 926-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7006) },
                    { new Guid("fb96abe2-872e-48ec-8b1c-482096635e97"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7185), null, null, new Guid("8d28c94e-7389-4766-b69b-88e473161adc"), "ASC 932-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7185) },
                    { new Guid("fbd740e7-4183-43d9-9dcc-c02f6b2dcf9c"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804), null, null, new Guid("4cacbddf-249e-4db9-ae8a-1f317ea4cd3c"), "ASC 946-205", false, false, "205", "Presentation", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(7804) },
                    { new Guid("fe0529fd-cd59-49d6-bc8e-d93721684939"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731), null, null, new Guid("983687a9-c988-441d-807c-728062d0b327"), "ASC 960-10", false, false, "10", "Overall", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8731) },
                    { new Guid("fe2c2d8a-63de-4a18-bda6-e037bda2b602"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992), null, null, new Guid("aadfc73e-a03f-45fd-9308-32637f75951f"), "ASC 715-60", false, false, "60", "Relationships", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(4992) },
                    { new Guid("feba2a93-fe69-4929-bd86-940f28b1112e"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388), null, null, new Guid("1b24f6c1-1813-4f1b-b2d2-3bcd83b49651"), "ASC 958-230", false, false, "230", "Statement of Cash Flows", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(8388) },
                    { new Guid("ffa80d34-c568-40fe-a19c-56317b8e3ae7"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6766), null, null, new Guid("6f278363-8ff4-4ee5-95f9-41aa77805d28"), "ASC 912-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(6766) },
                    { new Guid("ffb85677-106f-4b99-abcc-c927942a9810"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-235", false, false, "235", "Notes to Financial Statements", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) },
                    { new Guid("ffe308a2-e50d-4360-8d98-bd07d2e07261"), new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494), null, null, new Guid("45f8189a-febf-4545-85f1-f36939386ebd"), "ASC 974-360", false, false, "360", "Property, Plant, and Equipment", new DateTime(2026, 2, 8, 1, 39, 11, 650, DateTimeKind.Utc).AddTicks(9494) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CompanyId",
                schema: "dbo",
                table: "Accounts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CompanyId_AccountNumber",
                schema: "dbo",
                table: "Accounts",
                columns: new[] { "CompanyId", "AccountNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_FASBReference",
                schema: "dbo",
                table: "Accounts",
                column: "FASBReference");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_FASBSubtopicId",
                schema: "dbo",
                table: "Accounts",
                column: "FASBSubtopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_FASBTopicId",
                schema: "dbo",
                table: "Accounts",
                column: "FASBTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Type",
                schema: "dbo",
                table: "Accounts",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CompanyId",
                schema: "dbo",
                table: "AuditLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CompanyId_SequenceNumber",
                schema: "dbo",
                table: "AuditLogs",
                columns: new[] { "CompanyId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CompanyId_Timestamp",
                schema: "dbo",
                table: "AuditLogs",
                columns: new[] { "CompanyId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityId",
                schema: "dbo",
                table: "AuditLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType",
                schema: "dbo",
                table: "AuditLogs",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Timestamp",
                schema: "dbo",
                table: "AuditLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                schema: "dbo",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BillLineItems_AccountId",
                schema: "dbo",
                table: "BillLineItems",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BillLineItems_BillId",
                schema: "dbo",
                table: "BillLineItems",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_BillId",
                schema: "dbo",
                table: "BillPayments",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_CompanyId",
                schema: "dbo",
                table: "BillPayments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_CompanyId_PaymentNumber",
                schema: "dbo",
                table: "BillPayments",
                columns: new[] { "CompanyId", "PaymentNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_JournalEntryId",
                schema: "dbo",
                table: "BillPayments",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_PaymentDate",
                schema: "dbo",
                table: "BillPayments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_TaxId",
                schema: "dbo",
                table: "Companies",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceViolations_DetectedAt",
                schema: "dbo",
                table: "ComplianceViolations",
                column: "DetectedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceViolations_ResolvedById",
                schema: "dbo",
                table: "ComplianceViolations",
                column: "ResolvedById");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceViolations_Severity",
                schema: "dbo",
                table: "ComplianceViolations",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceViolations_Status",
                schema: "dbo",
                table: "ComplianceViolations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ComplianceViolations_ViolationType",
                schema: "dbo",
                table: "ComplianceViolations",
                column: "ViolationType");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoices_CompanyId",
                schema: "dbo",
                table: "CustomerInvoices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoices_CompanyId_InvoiceNumber",
                schema: "dbo",
                table: "CustomerInvoices",
                columns: new[] { "CompanyId", "InvoiceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoices_CustomerId",
                schema: "dbo",
                table: "CustomerInvoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoices_DueDate",
                schema: "dbo",
                table: "CustomerInvoices",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoices_JournalEntryId",
                schema: "dbo",
                table: "CustomerInvoices",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AccountsReceivableAccountId",
                schema: "dbo",
                table: "Customers",
                column: "AccountsReceivableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                schema: "dbo",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId_CustomerNumber",
                schema: "dbo",
                table: "Customers",
                columns: new[] { "CompanyId", "CustomerNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_EmployeeId",
                schema: "dbo",
                table: "Deductions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deductions_IsActive",
                schema: "dbo",
                table: "Deductions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CompanyId",
                schema: "dbo",
                table: "Departments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ManagerId",
                schema: "dbo",
                table: "Departments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCountTracking_LicenseId_RecordedAt",
                schema: "dbo",
                table: "EmployeeCountTracking",
                columns: new[] { "LicenseId", "RecordedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                schema: "dbo",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                schema: "dbo",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeNumber",
                schema: "dbo",
                table: "Employees",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerId",
                schema: "dbo",
                table: "Employees",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Status",
                schema: "dbo",
                table: "Employees",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FASBSubtopics_FASBTopicId",
                schema: "dbo",
                table: "FASBSubtopics",
                column: "FASBTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_FASBSubtopics_FullReference",
                schema: "dbo",
                table: "FASBSubtopics",
                column: "FullReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FASBSubtopics_TopicId_SubtopicCode",
                schema: "dbo",
                table: "FASBSubtopics",
                columns: new[] { "FASBTopicId", "SubtopicCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FASBTopics_Category",
                schema: "dbo",
                table: "FASBTopics",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_FASBTopics_TopicCode",
                schema: "dbo",
                table: "FASBTopics",
                column: "TopicCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralLedgerEntries_AccountId",
                schema: "dbo",
                table: "GeneralLedgerEntries",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralLedgerEntries_CompanyId",
                schema: "dbo",
                table: "GeneralLedgerEntries",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralLedgerEntries_CompanyId_AccountId_TransactionDate",
                schema: "dbo",
                table: "GeneralLedgerEntries",
                columns: new[] { "CompanyId", "AccountId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralLedgerEntries_JournalEntryId",
                schema: "dbo",
                table: "GeneralLedgerEntries",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralLedgerEntries_SourceEntityId",
                schema: "dbo",
                table: "GeneralLedgerEntries",
                column: "SourceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralLedgerEntries_TransactionDate",
                schema: "dbo",
                table: "GeneralLedgerEntries",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustmentLines_AdjustmentId",
                schema: "dbo",
                table: "InventoryAdjustmentLines",
                column: "AdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustmentLines_BatchId",
                schema: "dbo",
                table: "InventoryAdjustmentLines",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustmentLines_ProductId",
                schema: "dbo",
                table: "InventoryAdjustmentLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_AdjustmentDate",
                schema: "dbo",
                table: "InventoryAdjustments",
                column: "AdjustmentDate");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_CompanyId",
                schema: "dbo",
                table: "InventoryAdjustments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_CompanyId_AdjustmentNumber",
                schema: "dbo",
                table: "InventoryAdjustments",
                columns: new[] { "CompanyId", "AdjustmentNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_JournalEntryId",
                schema: "dbo",
                table: "InventoryAdjustments",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_Status",
                schema: "dbo",
                table: "InventoryAdjustments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAdjustments_WarehouseId",
                schema: "dbo",
                table: "InventoryAdjustments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLevels_ProductId",
                schema: "dbo",
                table: "InventoryLevels",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLevels_ProductId_WarehouseId",
                schema: "dbo",
                table: "InventoryLevels",
                columns: new[] { "ProductId", "WarehouseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLevels_WarehouseId",
                schema: "dbo",
                table: "InventoryLevels",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_BatchId",
                schema: "dbo",
                table: "InventoryTransactions",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_CompanyId",
                schema: "dbo",
                table: "InventoryTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ProductId",
                schema: "dbo",
                table: "InventoryTransactions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_Source",
                schema: "dbo",
                table: "InventoryTransactions",
                columns: new[] { "SourceType", "SourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_TransactionDate",
                schema: "dbo",
                table: "InventoryTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_Type",
                schema: "dbo",
                table: "InventoryTransactions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_WarehouseId",
                schema: "dbo",
                table: "InventoryTransactions",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLineItems_AccountId",
                schema: "dbo",
                table: "InvoiceLineItems",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLineItems_InvoiceId",
                schema: "dbo",
                table: "InvoiceLineItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePayments_CompanyId",
                schema: "dbo",
                table: "InvoicePayments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePayments_CompanyId_ReceiptNumber",
                schema: "dbo",
                table: "InvoicePayments",
                columns: new[] { "CompanyId", "ReceiptNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePayments_InvoiceId",
                schema: "dbo",
                table: "InvoicePayments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePayments_JournalEntryId",
                schema: "dbo",
                table: "InvoicePayments",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoicePayments_PaymentDate",
                schema: "dbo",
                table: "InvoicePayments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_CompanyId",
                schema: "dbo",
                table: "JournalEntries",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_CompanyId_JournalEntryNumber",
                schema: "dbo",
                table: "JournalEntries",
                columns: new[] { "CompanyId", "JournalEntryNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_Source",
                schema: "dbo",
                table: "JournalEntries",
                column: "Source");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_Status",
                schema: "dbo",
                table: "JournalEntries",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_TransactionDate",
                schema: "dbo",
                table: "JournalEntries",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_LicenseKey",
                schema: "dbo",
                table: "Licenses",
                column: "LicenseKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayPeriods_ApprovedById",
                schema: "dbo",
                table: "PayPeriods",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_PayPeriods_CompanyId",
                schema: "dbo",
                table: "PayPeriods",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PayPeriods_EndDate",
                schema: "dbo",
                table: "PayPeriods",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_PayPeriods_StartDate",
                schema: "dbo",
                table: "PayPeriods",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_PayPeriods_Status",
                schema: "dbo",
                table: "PayPeriods",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecordDetails_PayrollRecordId",
                schema: "dbo",
                table: "PayrollRecordDetails",
                column: "PayrollRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecords_EmployeeId",
                schema: "dbo",
                table: "PayrollRecords",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecords_PayPeriodId",
                schema: "dbo",
                table: "PayrollRecords",
                column: "PayPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecords_Status",
                schema: "dbo",
                table: "PayrollRecords",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Code",
                schema: "dbo",
                table: "Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCountLines_BatchId",
                schema: "dbo",
                table: "PhysicalCountLines",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCountLines_CountId",
                schema: "dbo",
                table: "PhysicalCountLines",
                column: "CountId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCountLines_ProductId",
                schema: "dbo",
                table: "PhysicalCountLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCounts_AdjustmentId",
                schema: "dbo",
                table: "PhysicalCounts",
                column: "AdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCounts_CompanyId",
                schema: "dbo",
                table: "PhysicalCounts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCounts_CompanyId_CountNumber",
                schema: "dbo",
                table: "PhysicalCounts",
                columns: new[] { "CompanyId", "CountNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCounts_CountDate",
                schema: "dbo",
                table: "PhysicalCounts",
                column: "CountDate");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCounts_Status",
                schema: "dbo",
                table: "PhysicalCounts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalCounts_WarehouseId",
                schema: "dbo",
                table: "PhysicalCounts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFeatures_LicenseId_FeatureCode",
                schema: "dbo",
                table: "PlanFeatures",
                columns: new[] { "LicenseId", "FeatureCode" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatches_BatchNumber",
                schema: "dbo",
                table: "ProductBatches",
                column: "BatchNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatches_ExpirationDate",
                schema: "dbo",
                table: "ProductBatches",
                column: "ExpirationDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatches_ProductId",
                schema: "dbo",
                table: "ProductBatches",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatches_WarehouseId",
                schema: "dbo",
                table: "ProductBatches",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CompanyId",
                schema: "dbo",
                table: "ProductCategories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CompanyId_Name",
                schema: "dbo",
                table: "ProductCategories",
                columns: new[] { "CompanyId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ParentCategoryId",
                schema: "dbo",
                table: "ProductCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Barcode",
                schema: "dbo",
                table: "Products",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "dbo",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_COGSAccountId",
                schema: "dbo",
                table: "Products",
                column: "COGSAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyId",
                schema: "dbo",
                table: "Products",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyId_SKU",
                schema: "dbo",
                table: "Products",
                columns: new[] { "CompanyId", "SKU" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_DefaultWarehouseId",
                schema: "dbo",
                table: "Products",
                column: "DefaultWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_InventoryAssetAccountId",
                schema: "dbo",
                table: "Products",
                column: "InventoryAssetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsActive",
                schema: "dbo",
                table: "Products",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Products_RevenueAccountId",
                schema: "dbo",
                table: "Products",
                column: "RevenueAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_ProductId",
                schema: "dbo",
                table: "PurchaseOrderLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_PurchaseOrderId",
                schema: "dbo",
                table: "PurchaseOrderLines",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CompanyId",
                schema: "dbo",
                table: "PurchaseOrders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CompanyId_PONumber",
                schema: "dbo",
                table: "PurchaseOrders",
                columns: new[] { "CompanyId", "PONumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_OrderDate",
                schema: "dbo",
                table: "PurchaseOrders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_Status",
                schema: "dbo",
                table: "PurchaseOrders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorBillId",
                schema: "dbo",
                table: "PurchaseOrders",
                column: "VendorBillId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorId",
                schema: "dbo",
                table: "PurchaseOrders",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_WarehouseId",
                schema: "dbo",
                table: "PurchaseOrders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RolesId",
                schema: "dbo",
                table: "RolePermissions",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "dbo",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_InventoryItemId",
                schema: "dbo",
                table: "SalesOrderLines",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_RevenueAccountId",
                schema: "dbo",
                table: "SalesOrderLines",
                column: "RevenueAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_SalesOrderId",
                schema: "dbo",
                table: "SalesOrderLines",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CompanyId",
                schema: "dbo",
                table: "SalesOrders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CompanyId_SONumber",
                schema: "dbo",
                table: "SalesOrders",
                columns: new[] { "CompanyId", "SONumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CustomerId",
                schema: "dbo",
                table: "SalesOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_OrderDate",
                schema: "dbo",
                table: "SalesOrders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_Status",
                schema: "dbo",
                table: "SalesOrders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_WarehouseId",
                schema: "dbo",
                table: "SalesOrders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturnLines_InventoryItemId",
                schema: "dbo",
                table: "SalesReturnLines",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturnLines_SalesOrderLineId",
                schema: "dbo",
                table: "SalesReturnLines",
                column: "SalesOrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturnLines_SalesReturnId",
                schema: "dbo",
                table: "SalesReturnLines",
                column: "SalesReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturns_CompanyId",
                schema: "dbo",
                table: "SalesReturns",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturns_CompanyId_RMANumber",
                schema: "dbo",
                table: "SalesReturns",
                columns: new[] { "CompanyId", "RMANumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturns_CustomerId",
                schema: "dbo",
                table: "SalesReturns",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturns_InvoiceId",
                schema: "dbo",
                table: "SalesReturns",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturns_ReturnDate",
                schema: "dbo",
                table: "SalesReturns",
                column: "ReturnDate");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturns_SalesOrderId",
                schema: "dbo",
                table: "SalesReturns",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SOShipmentLines_BatchLotId",
                schema: "dbo",
                table: "SOShipmentLines",
                column: "BatchLotId");

            migrationBuilder.CreateIndex(
                name: "IX_SOShipmentLines_InventoryItemId",
                schema: "dbo",
                table: "SOShipmentLines",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SOShipmentLines_SalesOrderLineId",
                schema: "dbo",
                table: "SOShipmentLines",
                column: "SalesOrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_SOShipmentLines_ShipmentId",
                schema: "dbo",
                table: "SOShipmentLines",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SOShipments_CompanyId",
                schema: "dbo",
                table: "SOShipments",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SOShipments_CompanyId_ShipmentNumber",
                schema: "dbo",
                table: "SOShipments",
                columns: new[] { "CompanyId", "ShipmentNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SOShipments_CustomerId",
                schema: "dbo",
                table: "SOShipments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SOShipments_SalesOrderId",
                schema: "dbo",
                table: "SOShipments",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SOShipments_ShipDate",
                schema: "dbo",
                table: "SOShipments",
                column: "ShipDate");

            migrationBuilder.CreateIndex(
                name: "IX_SOShipments_WarehouseId",
                schema: "dbo",
                table: "SOShipments",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockReceiptLines_POLineId",
                schema: "dbo",
                table: "StockReceiptLines",
                column: "POLineId");

            migrationBuilder.CreateIndex(
                name: "IX_StockReceiptLines_ProductId",
                schema: "dbo",
                table: "StockReceiptLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockReceiptLines_ReceiptId",
                schema: "dbo",
                table: "StockReceiptLines",
                column: "ReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_StockReceipts_CompanyId",
                schema: "dbo",
                table: "StockReceipts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StockReceipts_CompanyId_ReceiptNumber",
                schema: "dbo",
                table: "StockReceipts",
                columns: new[] { "CompanyId", "ReceiptNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockReceipts_JournalEntryId",
                schema: "dbo",
                table: "StockReceipts",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_StockReceipts_PurchaseOrderId",
                schema: "dbo",
                table: "StockReceipts",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockReceipts_ReceiptDate",
                schema: "dbo",
                table: "StockReceipts",
                column: "ReceiptDate");

            migrationBuilder.CreateIndex(
                name: "IX_StockReceipts_WarehouseId",
                schema: "dbo",
                table: "StockReceipts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferLines_BatchId",
                schema: "dbo",
                table: "StockTransferLines",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferLines_ProductId",
                schema: "dbo",
                table: "StockTransferLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferLines_TransferId",
                schema: "dbo",
                table: "StockTransferLines",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_CompanyId",
                schema: "dbo",
                table: "StockTransfers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_CompanyId_TransferNumber",
                schema: "dbo",
                table: "StockTransfers",
                columns: new[] { "CompanyId", "TransferNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_FromWarehouseId",
                schema: "dbo",
                table: "StockTransfers",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_Status",
                schema: "dbo",
                table: "StockTransfers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_ToWarehouseId",
                schema: "dbo",
                table: "StockTransfers",
                column: "ToWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_TransferDate",
                schema: "dbo",
                table: "StockTransfers",
                column: "TransferDate");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionHistory_LicenseId_OccurredAt",
                schema: "dbo",
                table: "SubscriptionHistory",
                columns: new[] { "LicenseId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxWithholdings_EmployeeId",
                schema: "dbo",
                table: "TaxWithholdings",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxWithholdings_TaxYear",
                schema: "dbo",
                table: "TaxWithholdings",
                column: "TaxYear");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_ApprovedById",
                schema: "dbo",
                table: "Timesheets",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_EmployeeId",
                schema: "dbo",
                table: "Timesheets",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_Status",
                schema: "dbo",
                table: "Timesheets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_WorkDate",
                schema: "dbo",
                table: "Timesheets",
                column: "WorkDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UsersId",
                schema: "dbo",
                table: "UserRoles",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "dbo",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "dbo",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorBills_CompanyId",
                schema: "dbo",
                table: "VendorBills",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorBills_CompanyId_BillNumber",
                schema: "dbo",
                table: "VendorBills",
                columns: new[] { "CompanyId", "BillNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorBills_DueDate",
                schema: "dbo",
                table: "VendorBills",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_VendorBills_JournalEntryId",
                schema: "dbo",
                table: "VendorBills",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorBills_VendorId",
                schema: "dbo",
                table: "VendorBills",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_AccountsPayableAccountId",
                schema: "dbo",
                table: "Vendors",
                column: "AccountsPayableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CompanyId",
                schema: "dbo",
                table: "Vendors",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CompanyId_VendorNumber",
                schema: "dbo",
                table: "Vendors",
                columns: new[] { "CompanyId", "VendorNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CompanyId",
                schema: "dbo",
                table: "Warehouses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CompanyId_Code",
                schema: "dbo",
                table: "Warehouses",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplianceViolations_Employees_ResolvedById",
                schema: "dbo",
                table: "ComplianceViolations",
                column: "ResolvedById",
                principalSchema: "dbo",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deductions_Employees_EmployeeId",
                schema: "dbo",
                table: "Deductions",
                column: "EmployeeId",
                principalSchema: "dbo",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_ManagerId",
                schema: "dbo",
                table: "Departments",
                column: "ManagerId",
                principalSchema: "dbo",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Companies_CompanyId",
                schema: "dbo",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies_CompanyId",
                schema: "dbo",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_ManagerId",
                schema: "dbo",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "AuditLogs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BillLineItems",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BillPayments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ComplianceViolations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Deductions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EmployeeCountTracking",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GeneralLedgerEntries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryAdjustmentLines",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryLevels",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryTransactions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InvoiceLineItems",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InvoicePayments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PayrollRecordDetails",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PhysicalCountLines",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PlanFeatures",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RolePermissions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SalesReturnLines",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SOShipmentLines",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StockReceiptLines",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StockTransferLines",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SubscriptionHistory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TaxWithholdings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Timesheets",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PayrollRecords",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PhysicalCounts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SalesReturns",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SOShipments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SalesOrderLines",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLines",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StockReceipts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductBatches",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "StockTransfers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Licenses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PayPeriods",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "InventoryAdjustments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CustomerInvoices",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SalesOrders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "VendorBills",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ProductCategories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Warehouses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "JournalEntries",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Vendors",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FASBSubtopics",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FASBTopics",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "dbo");
        }
    }
}

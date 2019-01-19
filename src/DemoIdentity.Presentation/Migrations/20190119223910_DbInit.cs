using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoIdentity.Presentation.Migrations
{
    public partial class DbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SecurityAppClaim",
                columns: table => new
                {
                    AppClaimId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    CanEdit = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityAppClaim", x => x.AppClaimId);
                });

            migrationBuilder.CreateTable(
                name: "SecurityClaimGroup",
                columns: table => new
                {
                    ClaimGroupId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentClaimGroupId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    CanEdit = table.Column<bool>(nullable: false),
                    InPosition = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityClaimGroup", x => x.ClaimGroupId);
                    table.ForeignKey(
                        name: "FK_SecurityClaimGroup_SecurityClaimGroup_ParentClaimGroupId",
                        column: x => x.ParentClaimGroupId,
                        principalTable: "SecurityClaimGroup",
                        principalColumn: "ClaimGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecurityRole",
                columns: table => new
                {
                    RoleId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    EndedBy = table.Column<long>(nullable: true),
                    EndedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityRole", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "SecurityUser",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    EndedBy = table.Column<long>(nullable: true),
                    EndedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityUser", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "SecurityClaimInClaimGroup",
                columns: table => new
                {
                    ClaimInClaimGroupId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimGroupId = table.Column<long>(nullable: false),
                    AppClaimId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityClaimInClaimGroup", x => x.ClaimInClaimGroupId);
                    table.ForeignKey(
                        name: "FK_SecurityClaimInClaimGroup_SecurityAppClaim_AppClaimId",
                        column: x => x.AppClaimId,
                        principalTable: "SecurityAppClaim",
                        principalColumn: "AppClaimId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecurityClaimInClaimGroup_SecurityClaimGroup_ClaimGroupId",
                        column: x => x.ClaimGroupId,
                        principalTable: "SecurityClaimGroup",
                        principalColumn: "ClaimGroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecurityClaimInRole",
                columns: table => new
                {
                    ClaimInRoleId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppClaimId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityClaimInRole", x => x.ClaimInRoleId);
                    table.ForeignKey(
                        name: "FK_SecurityClaimInRole_SecurityAppClaim_AppClaimId",
                        column: x => x.AppClaimId,
                        principalTable: "SecurityAppClaim",
                        principalColumn: "AppClaimId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecurityClaimInRole_SecurityRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SecurityRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecurityRoleClaim",
                columns: table => new
                {
                    RoleClaimId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    EndedBy = table.Column<long>(nullable: true),
                    EndedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityRoleClaim", x => x.RoleClaimId);
                    table.ForeignKey(
                        name: "FK_SecurityRoleClaim_SecurityRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SecurityRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityUserClaim",
                columns: table => new
                {
                    UserClaimId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    EndedBy = table.Column<long>(nullable: true),
                    EndedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityUserClaim", x => x.UserClaimId);
                    table.ForeignKey(
                        name: "FK_SecurityUserClaim_SecurityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SecurityUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityUserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_SecurityUserLogin_SecurityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SecurityUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityUserRole",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    EndedBy = table.Column<long>(nullable: true),
                    EndedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_SecurityUserRole_SecurityRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SecurityRole",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecurityUserRole_SecurityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SecurityUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityUserToken",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_SecurityUserToken_SecurityUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SecurityUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SecurityClaimGroup_ParentClaimGroupId",
                table: "SecurityClaimGroup",
                column: "ParentClaimGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityClaimInClaimGroup_AppClaimId",
                table: "SecurityClaimInClaimGroup",
                column: "AppClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityClaimInClaimGroup_ClaimGroupId",
                table: "SecurityClaimInClaimGroup",
                column: "ClaimGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityClaimInRole_AppClaimId",
                table: "SecurityClaimInRole",
                column: "AppClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityClaimInRole_RoleId",
                table: "SecurityClaimInRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "SecurityRole",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityRoleClaim_RoleId",
                table: "SecurityRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "SecurityUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "SecurityUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityUserClaim_UserId",
                table: "SecurityUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityUserLogin_UserId",
                table: "SecurityUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityUserRole_RoleId",
                table: "SecurityUserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecurityClaimInClaimGroup");

            migrationBuilder.DropTable(
                name: "SecurityClaimInRole");

            migrationBuilder.DropTable(
                name: "SecurityRoleClaim");

            migrationBuilder.DropTable(
                name: "SecurityUserClaim");

            migrationBuilder.DropTable(
                name: "SecurityUserLogin");

            migrationBuilder.DropTable(
                name: "SecurityUserRole");

            migrationBuilder.DropTable(
                name: "SecurityUserToken");

            migrationBuilder.DropTable(
                name: "SecurityClaimGroup");

            migrationBuilder.DropTable(
                name: "SecurityAppClaim");

            migrationBuilder.DropTable(
                name: "SecurityRole");

            migrationBuilder.DropTable(
                name: "SecurityUser");
        }
    }
}

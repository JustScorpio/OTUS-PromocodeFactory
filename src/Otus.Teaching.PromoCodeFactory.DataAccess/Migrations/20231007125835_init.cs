using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    first_name = table.Column<string>(nullable: true),
                    last_name = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "partners",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    number_issued_promo_codes = table.Column<int>(nullable: false),
                    is_active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "preferences",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_preferences", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "partner_promo_code_limit",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    partner_id = table.Column<Guid>(nullable: false),
                    create_date = table.Column<DateTime>(nullable: false),
                    cancel_date = table.Column<DateTime>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: false),
                    limit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_promo_code_limit", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_promo_code_limit_partners_partner_id",
                        column: x => x.partner_id,
                        principalTable: "partners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_preference",
                columns: table => new
                {
                    customer_id = table.Column<Guid>(nullable: false),
                    preference_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_preference", x => new { x.customer_id, x.preference_id });
                    table.ForeignKey(
                        name: "fk_customer_preference_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_customer_preference_preferences_preference_id",
                        column: x => x.preference_id,
                        principalTable: "preferences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    first_name = table.Column<string>(nullable: true),
                    last_name = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    role_id = table.Column<Guid>(nullable: false),
                    applied_promocodes_count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.ForeignKey(
                        name: "fk_employees_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promo_codes",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    code = table.Column<string>(nullable: true),
                    service_info = table.Column<string>(nullable: true),
                    begin_date = table.Column<DateTime>(nullable: false),
                    end_date = table.Column<DateTime>(nullable: false),
                    partner_name = table.Column<string>(nullable: true),
                    partner_manager_id = table.Column<Guid>(nullable: true),
                    preference_id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promo_codes", x => x.id);
                    table.ForeignKey(
                        name: "fk_promo_codes_employees_partner_manager_id",
                        column: x => x.partner_manager_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_promo_codes_preferences_preference_id",
                        column: x => x.preference_id,
                        principalTable: "preferences",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_customer_preference_preference_id",
                table: "customer_preference",
                column: "preference_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_role_id",
                table: "employees",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_partner_promo_code_limit_partner_id",
                table: "partner_promo_code_limit",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "ix_promo_codes_partner_manager_id",
                table: "promo_codes",
                column: "partner_manager_id");

            migrationBuilder.CreateIndex(
                name: "ix_promo_codes_preference_id",
                table: "promo_codes",
                column: "preference_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_preference");

            migrationBuilder.DropTable(
                name: "partner_promo_code_limit");

            migrationBuilder.DropTable(
                name: "promo_codes");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "partners");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "preferences");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}

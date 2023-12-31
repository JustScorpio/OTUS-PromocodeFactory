﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Otus.Teaching.PromoCodeFactory.DataAccess;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231007125835_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4");

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.Administration.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AppliedPromocodesCount")
                        .HasColumnName("applied_promocodes_count")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnName("first_name")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnName("last_name")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RoleId")
                        .HasColumnName("role_id")
                        .HasColumnType("TEXT");

                    b.HasKey("Id")
                        .HasName("pk_employees");

                    b.HasIndex("RoleId")
                        .HasName("ix_employees_role_id");

                    b.ToTable("employees");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.Administration.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnName("first_name")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnName("last_name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id")
                        .HasName("pk_customers");

                    b.ToTable("customers");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.CustomerPreference", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnName("customer_id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PreferenceId")
                        .HasColumnName("preference_id")
                        .HasColumnType("TEXT");

                    b.HasKey("CustomerId", "PreferenceId")
                        .HasName("pk_customer_preference");

                    b.HasIndex("PreferenceId")
                        .HasName("ix_customer_preference_preference_id");

                    b.ToTable("customer_preference");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Partner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnName("is_active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberIssuedPromoCodes")
                        .HasColumnName("number_issued_promo_codes")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id")
                        .HasName("pk_partners");

                    b.ToTable("partners");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.PartnerPromoCodeLimit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CancelDate")
                        .HasColumnName("cancel_date")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnName("create_date")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnName("end_date")
                        .HasColumnType("TEXT");

                    b.Property<int>("Limit")
                        .HasColumnName("limit")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PartnerId")
                        .HasColumnName("partner_id")
                        .HasColumnType("TEXT");

                    b.HasKey("Id")
                        .HasName("pk_partner_promo_code_limit");

                    b.HasIndex("PartnerId")
                        .HasName("ix_partner_promo_code_limit_partner_id");

                    b.ToTable("partner_promo_code_limit");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Preference", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id")
                        .HasName("pk_preferences");

                    b.ToTable("preferences");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.PromoCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("BeginDate")
                        .HasColumnName("begin_date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .HasColumnName("code")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnName("end_date")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PartnerManagerId")
                        .HasColumnName("partner_manager_id")
                        .HasColumnType("TEXT");

                    b.Property<string>("PartnerName")
                        .HasColumnName("partner_name")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("PreferenceId")
                        .HasColumnName("preference_id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ServiceInfo")
                        .HasColumnName("service_info")
                        .HasColumnType("TEXT");

                    b.HasKey("Id")
                        .HasName("pk_promo_codes");

                    b.HasIndex("PartnerManagerId")
                        .HasName("ix_promo_codes_partner_manager_id");

                    b.HasIndex("PreferenceId")
                        .HasName("ix_promo_codes_preference_id");

                    b.ToTable("promo_codes");
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.Administration.Employee", b =>
                {
                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.Administration.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_employees_roles_role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.CustomerPreference", b =>
                {
                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer", "Customer")
                        .WithMany("Preferences")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("fk_customer_preference_customers_customer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Preference", "Preference")
                        .WithMany()
                        .HasForeignKey("PreferenceId")
                        .HasConstraintName("fk_customer_preference_preferences_preference_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.PartnerPromoCodeLimit", b =>
                {
                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Partner", "Partner")
                        .WithMany("PartnerLimits")
                        .HasForeignKey("PartnerId")
                        .HasConstraintName("fk_partner_promo_code_limit_partners_partner_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.PromoCode", b =>
                {
                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.Administration.Employee", "PartnerManager")
                        .WithMany()
                        .HasForeignKey("PartnerManagerId")
                        .HasConstraintName("fk_promo_codes_employees_partner_manager_id");

                    b.HasOne("Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Preference", "Preference")
                        .WithMany()
                        .HasForeignKey("PreferenceId")
                        .HasConstraintName("fk_promo_codes_preferences_preference_id");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using CouncilsManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CouncilsManagmentSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CouncilsManagmentSystem.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Discription")
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("OTP")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("academic_degree")
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("administrative_degree")
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("functional_characteristic")
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("img")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Collage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("collages");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.CouncilMembers", b =>
                {
                    b.Property<string>("MemberId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<int>("CouncilId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<bool>("IsAttending")
                        .HasColumnType("bit");

                    b.Property<string>("Pdf")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReasonNonAttendance")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MemberId", "CouncilId");

                    b.HasIndex("CouncilId");

                    b.ToTable("CouncilMembers");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Councils", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("HallId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeCouncilId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HallId");

                    b.HasIndex("TypeCouncilId");

                    b.ToTable("Councils");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Department", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int>("collage_id")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("collage_id");

                    b.ToTable("departments");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Hall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfSeats")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Halls");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Permissionss", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("AddCollage")
                        .HasColumnType("bit");

                    b.Property<bool>("AddCouncil")
                        .HasColumnType("bit");

                    b.Property<bool>("AddDepartment")
                        .HasColumnType("bit");

                    b.Property<bool>("AddHall")
                        .HasColumnType("bit");

                    b.Property<bool>("AddMembers")
                        .HasColumnType("bit");

                    b.Property<bool>("AddMembersByExcil")
                        .HasColumnType("bit");

                    b.Property<bool>("AddResult")
                        .HasColumnType("bit");

                    b.Property<bool>("AddTopic")
                        .HasColumnType("bit");

                    b.Property<bool>("Arrange")
                        .HasColumnType("bit");

                    b.Property<bool>("CreateTypeCouncil")
                        .HasColumnType("bit");

                    b.Property<bool>("DeactiveUser")
                        .HasColumnType("bit");

                    b.Property<bool>("EditCouncil")
                        .HasColumnType("bit");

                    b.Property<bool>("EditTypeCouncil")
                        .HasColumnType("bit");

                    b.Property<bool>("UpdateUser")
                        .HasColumnType("bit");

                    b.Property<bool>("Updatepermission")
                        .HasColumnType("bit");

                    b.Property<string>("userId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("userId")
                        .IsUnique();

                    b.ToTable("permissionss");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Attachment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CouncilId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTimeCreated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDiscussed")
                        .HasColumnType("bit");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CouncilId");

                    b.ToTable("topics");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.TypeCouncil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ChairmanCouncilId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecretaryCouncilId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ChairmanCouncilId")
                        .IsUnique();

                    b.HasIndex("DepartmentId");

                    b.HasIndex("SecretaryCouncilId")
                        .IsUnique();

                    b.ToTable("typeCouncils");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.ApplicationUser", b =>
                {
                    b.HasOne("CouncilsManagmentSystem.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.CouncilMembers", b =>
                {
                    b.HasOne("CouncilsManagmentSystem.Models.Councils", "Council")
                        .WithMany("CouncilMembers")
                        .HasForeignKey("CouncilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CouncilsManagmentSystem.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("CouncilMembers")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Council");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Councils", b =>
                {
                    b.HasOne("CouncilsManagmentSystem.Models.Hall", "Hall")
                        .WithMany("Councils")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CouncilsManagmentSystem.Models.TypeCouncil", "TypeCouncil")
                        .WithMany("Councils")
                        .HasForeignKey("TypeCouncilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hall");

                    b.Navigation("TypeCouncil");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Department", b =>
                {
                    b.HasOne("CouncilsManagmentSystem.Models.Collage", "Collage")
                        .WithMany("Department")
                        .HasForeignKey("collage_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collage");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Permissionss", b =>
                {
                    b.HasOne("CouncilsManagmentSystem.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("Permissionss")
                        .HasForeignKey("CouncilsManagmentSystem.Models.Permissionss", "userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Topic", b =>
                {
                    b.HasOne("CouncilsManagmentSystem.Models.Councils", "Council")
                        .WithMany("TopicS")
                        .HasForeignKey("CouncilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Council");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.TypeCouncil", b =>
                {
                    b.HasOne("CouncilsManagmentSystem.Models.ApplicationUser", "ChairmanCouncil")
                        .WithOne("Chairmanship")
                        .HasForeignKey("CouncilsManagmentSystem.Models.TypeCouncil", "ChairmanCouncilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CouncilsManagmentSystem.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CouncilsManagmentSystem.Models.ApplicationUser", "SecretaryCouncil")
                        .WithOne("Secretaryship")
                        .HasForeignKey("CouncilsManagmentSystem.Models.TypeCouncil", "SecretaryCouncilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChairmanCouncil");

                    b.Navigation("Department");

                    b.Navigation("SecretaryCouncil");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CouncilsManagmentSystem.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CouncilsManagmentSystem.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CouncilsManagmentSystem.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CouncilsManagmentSystem.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.ApplicationUser", b =>
                {
                    b.Navigation("Chairmanship")
                        .IsRequired();

                    b.Navigation("CouncilMembers");

                    b.Navigation("Permissionss")
                        .IsRequired();

                    b.Navigation("Secretaryship")
                        .IsRequired();
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Collage", b =>
                {
                    b.Navigation("Department");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Councils", b =>
                {
                    b.Navigation("CouncilMembers");

                    b.Navigation("TopicS");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.Hall", b =>
                {
                    b.Navigation("Councils");
                });

            modelBuilder.Entity("CouncilsManagmentSystem.Models.TypeCouncil", b =>
                {
                    b.Navigation("Councils");
                });
#pragma warning restore 612, 618
        }
    }
}

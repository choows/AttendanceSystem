﻿// <auto-generated />
using System;
using AttendanceSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AttendanceSystem.Migrations
{
    [DbContext(typeof(AttendanceContext))]
    partial class AttendanceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("AttendanceSystem.Models.CheckIn", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CheckinDateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ClassID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("IpAddress")
                        .HasColumnType("varchar(25)");

                    b.Property<string>("UserID")
                        .HasColumnType("varchar(100)");

                    b.HasKey("ID");

                    b.HasIndex("ClassID");

                    b.HasIndex("UserID");

                    b.ToTable("checkIns");
                });

            modelBuilder.Entity("AttendanceSystem.Models.Class", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("AllowCheckin")
                        .HasColumnType("bit");

                    b.Property<string>("CourseID")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("CreateByID")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(400)");

                    b.Property<DateTime>("FromTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("ToTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Type")
                        .HasColumnType("varchar(50)");

                    b.HasKey("ID");

                    b.HasIndex("CourseID");

                    b.HasIndex("CreateByID");

                    b.ToTable("classes");
                });

            modelBuilder.Entity("AttendanceSystem.Models.Course", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Faculty")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(200)");

                    b.HasKey("ID");

                    b.ToTable("courses");
                });

            modelBuilder.Entity("AttendanceSystem.Models.Registered_Course", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CourseID")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UserID")
                        .HasColumnType("varchar(100)");

                    b.HasKey("ID");

                    b.HasIndex("CourseID");

                    b.HasIndex("UserID");

                    b.ToTable("Registered_course");
                });

            modelBuilder.Entity("AttendanceSystem.Models.Users", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("IpAddress")
                        .HasColumnType("varchar(25)");

                    b.Property<DateTime?>("LastLoginTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Password")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Role")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(100)");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AttendanceSystem.Models.CheckIn", b =>
                {
                    b.HasOne("AttendanceSystem.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassID");

                    b.HasOne("AttendanceSystem.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserID");

                    b.Navigation("Class");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AttendanceSystem.Models.Class", b =>
                {
                    b.HasOne("AttendanceSystem.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseID");

                    b.HasOne("AttendanceSystem.Models.Users", "CreateBy")
                        .WithMany()
                        .HasForeignKey("CreateByID");

                    b.Navigation("Course");

                    b.Navigation("CreateBy");
                });

            modelBuilder.Entity("AttendanceSystem.Models.Registered_Course", b =>
                {
                    b.HasOne("AttendanceSystem.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseID");

                    b.HasOne("AttendanceSystem.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserID");

                    b.Navigation("Course");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UdelarOnlineApi.Entities;

namespace UdelarOnlineApi.Migrations
{
  [DbContext(typeof(UdelarOnlineContext))]
  [Migration("20200909230919_initialCreate")]
  partial class initialCreate
  {
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("ProductVersion", "3.1.7")
          .HasAnnotation("Relational:MaxIdentifierLength", 128)
          .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

      modelBuilder.Entity("UdelarOnlineApi.Models.Course", b =>
          {
            b.Property<long>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("bigint")
                      .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            b.Property<long>("FacultyId")
                      .HasColumnType("bigint");

            b.Property<string>("Name")
                      .HasColumnType("nvarchar(max)");

            b.HasKey("Id");

            b.HasIndex("FacultyId");

            b.ToTable("Courses");
          });

      modelBuilder.Entity("UdelarOnlineApi.Models.CourseUser", b =>
          {
            b.Property<long>("CourseId")
                      .HasColumnType("bigint");

            b.Property<long>("UserId")
                      .HasColumnType("bigint");

            b.HasKey("CourseId", "UserId");

            b.HasIndex("UserId");

            b.ToTable("CourseUsers");
          });

      modelBuilder.Entity("UdelarOnlineApi.Models.Faculty", b =>
          {
            b.Property<long>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("bigint")
                      .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            b.Property<string>("Name")
                      .HasColumnType("nvarchar(max)");

            b.HasKey("Id");

            b.ToTable("Faculties");
          });

      modelBuilder.Entity("UdelarOnlineApi.Models.User", b =>
          {
            b.Property<long>("Id")
                      .ValueGeneratedOnAdd()
                      .HasColumnType("bigint")
                      .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            b.Property<long>("FacultyId")
                      .HasColumnType("bigint");

            b.Property<string>("Name")
                      .HasColumnType("nvarchar(max)");

            b.HasKey("Id");

            b.HasIndex("FacultyId");

            b.ToTable("Users");
          });

      modelBuilder.Entity("UdelarOnlineApi.Models.Course", b =>
          {
            b.HasOne("UdelarOnlineApi.Models.Faculty", "Faculty")
                      .WithMany("Courses")
                      .HasForeignKey("FacultyId")
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();
          });

      modelBuilder.Entity("UdelarOnlineApi.Models.CourseUser", b =>
          {
            b.HasOne("UdelarOnlineApi.Models.Course", "Course")
                      .WithMany("CourseUsers")
                      .HasForeignKey("CourseId")
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired();

            b.HasOne("UdelarOnlineApi.Models.User", "User")
                      .WithMany("CourseUsers")
                      .HasForeignKey("UserId")
                      .OnDelete(DeleteBehavior.Restrict)
                      .IsRequired();
          });

      modelBuilder.Entity("UdelarOnlineApi.Models.User", b =>
          {
            b.HasOne("UdelarOnlineApi.Models.Faculty", "Faculty")
                      .WithMany("Users")
                      .HasForeignKey("FacultyId")
                      .OnDelete(DeleteBehavior.Cascade)
                      .IsRequired();
          });
#pragma warning restore 612, 618
    }
  }
}

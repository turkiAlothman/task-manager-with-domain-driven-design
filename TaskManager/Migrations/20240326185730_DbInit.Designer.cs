﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using infrastructure.Persistence;

#nullable disable

namespace TaskManager.Migrations
{
    [DbContext(typeof(TaskManagerDbContext))]
    [Migration("20240326185730_DbInit")]
    partial class DbInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Domain.Activitiy.Activities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("actorId")
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("taskId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("actorId");

                    b.HasIndex("taskId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Domain.Comment.Comments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MessageTaskId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("MessageTaskId");

                    b.HasIndex("SenderId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Domain.DomainModels.Employee.Invites", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("AsManager")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SecretKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("inviteeEmail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("inviterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("inviterId");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("Domain.DomainModels.Task.Attachments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PhysicalPath")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("taskId")
                        .HasColumnType("int");

                    b.Property<string>("url")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("taskId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("Domain.Employee.Employees", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Manager")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("teamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("teamId");

                    b.ToTable("employees");
                });

            modelBuilder.Entity("Domain.Project.Projects", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("projects");
                });

            modelBuilder.Entity("Domain.ResetPasswords.ResetPassword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SecretKey")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("resetPassword");
                });

            modelBuilder.Entity("Domain.Task.Tasks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasMaxLength(400)
                        .HasColumnType("varchar(400)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Priority")
                        .HasColumnType("longtext");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int?>("ReporterId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Type")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ReporterId");

                    b.ToTable("tasks");
                });

            modelBuilder.Entity("Domain.Team.Teams", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("teams");
                });

            modelBuilder.Entity("EmployeesProjects", b =>
                {
                    b.Property<int>("EmployeesId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectsId")
                        .HasColumnType("int");

                    b.HasKey("EmployeesId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("EmployeesProjects");
                });

            modelBuilder.Entity("EmployeesTasks", b =>
                {
                    b.Property<int>("AsigneesId")
                        .HasColumnType("int");

                    b.Property<int>("TasksId")
                        .HasColumnType("int");

                    b.HasKey("AsigneesId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("EmployeesTasks");
                });

            modelBuilder.Entity("Domain.Activitiy.Activities", b =>
                {
                    b.HasOne("Domain.Employee.Employees", "actor")
                        .WithMany("activities")
                        .HasForeignKey("actorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Task.Tasks", "task")
                        .WithMany("Activities")
                        .HasForeignKey("taskId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("actor");

                    b.Navigation("task");
                });

            modelBuilder.Entity("Domain.Comment.Comments", b =>
                {
                    b.HasOne("Domain.Task.Tasks", "MessageTask")
                        .WithMany("Comments")
                        .HasForeignKey("MessageTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Employee.Employees", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MessageTask");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Domain.DomainModels.Employee.Invites", b =>
                {
                    b.HasOne("Domain.Employee.Employees", "inviter")
                        .WithMany()
                        .HasForeignKey("inviterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("inviter");
                });

            modelBuilder.Entity("Domain.DomainModels.Task.Attachments", b =>
                {
                    b.HasOne("Domain.Task.Tasks", "task")
                        .WithMany("Attachments")
                        .HasForeignKey("taskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("task");
                });

            modelBuilder.Entity("Domain.Employee.Employees", b =>
                {
                    b.HasOne("Domain.Team.Teams", "team")
                        .WithMany("Members")
                        .HasForeignKey("teamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("team");
                });

            modelBuilder.Entity("Domain.Task.Tasks", b =>
                {
                    b.HasOne("Domain.Project.Projects", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId");

                    b.HasOne("Domain.Employee.Employees", "Reporter")
                        .WithMany("tasksReported")
                        .HasForeignKey("ReporterId");

                    b.Navigation("Project");

                    b.Navigation("Reporter");
                });

            modelBuilder.Entity("EmployeesProjects", b =>
                {
                    b.HasOne("Domain.Employee.Employees", null)
                        .WithMany()
                        .HasForeignKey("EmployeesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Project.Projects", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EmployeesTasks", b =>
                {
                    b.HasOne("Domain.Employee.Employees", null)
                        .WithMany()
                        .HasForeignKey("AsigneesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Task.Tasks", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Employee.Employees", b =>
                {
                    b.Navigation("activities");

                    b.Navigation("tasksReported");
                });

            modelBuilder.Entity("Domain.Project.Projects", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Domain.Task.Tasks", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("Attachments");

                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Domain.Team.Teams", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
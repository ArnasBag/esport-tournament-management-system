﻿// <auto-generated />
using System;
using ESTMS.API.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ESTMS.API.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230421134349_MakeTournamentManager")]
    partial class MakeTournamentManager
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Invitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ReceiverId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.HasIndex("TeamId");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TournamentId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.MatchWinner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MatchId")
                        .HasColumnType("integer");

                    b.Property<int>("WinnerTeamId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MatchId")
                        .IsUnique();

                    b.HasIndex("WinnerTeamId");

                    b.ToTable("MatchWinners");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AboutMeText")
                        .HasColumnType("text");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("text");

                    b.Property<string>("PicturePath")
                        .HasColumnType("text");

                    b.Property<int?>("Points")
                        .HasColumnType("integer");

                    b.Property<int?>("Rank")
                        .HasColumnType("integer");

                    b.Property<int?>("TeamId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TeamManagerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TeamManagerId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.TeamManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("TeamManagers");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Tournament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ManagerId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.TournamentManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("TournamentManagers");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.TournamentWinner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("TournamentId")
                        .HasColumnType("integer");

                    b.Property<int?>("WinnerTeamId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId")
                        .IsUnique();

                    b.HasIndex("WinnerTeamId");

                    b.ToTable("TournamentWinners");
                });

            modelBuilder.Entity("MatchTeam", b =>
                {
                    b.Property<int>("CompetitorsId")
                        .HasColumnType("integer");

                    b.Property<int>("MatchesId")
                        .HasColumnType("integer");

                    b.HasKey("CompetitorsId", "MatchesId");

                    b.HasIndex("MatchesId");

                    b.ToTable("MatchTeam");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TeamTournament", b =>
                {
                    b.Property<int>("TeamsId")
                        .HasColumnType("integer");

                    b.Property<int>("TournamentsId")
                        .HasColumnType("integer");

                    b.HasKey("TeamsId", "TournamentsId");

                    b.HasIndex("TournamentsId");

                    b.ToTable("TeamTournament");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Invitation", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.ApplicationUser", "Receiver")
                        .WithMany("ReceivedInvitations")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESTMS.API.DataAccess.Entities.ApplicationUser", "Sender")
                        .WithMany("SentInvitations")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESTMS.API.DataAccess.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Match", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.Tournament", "Tournament")
                        .WithMany("Matches")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.MatchWinner", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.Match", "Match")
                        .WithOne("Winner")
                        .HasForeignKey("ESTMS.API.DataAccess.Entities.MatchWinner", "MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESTMS.API.DataAccess.Entities.Team", "WinnerTeam")
                        .WithMany()
                        .HasForeignKey("WinnerTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("WinnerTeam");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Player", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("ESTMS.API.DataAccess.Entities.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");

                    b.Navigation("ApplicationUser");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Team", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.TeamManager", "TeamManager")
                        .WithMany("Teams")
                        .HasForeignKey("TeamManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TeamManager");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.TeamManager", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Tournament", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.TournamentManager", "Manager")
                        .WithMany("Tournaments")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.TournamentManager", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.TournamentWinner", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.Tournament", "Tournament")
                        .WithOne("Winner")
                        .HasForeignKey("ESTMS.API.DataAccess.Entities.TournamentWinner", "TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESTMS.API.DataAccess.Entities.Team", "WinnerTeam")
                        .WithMany()
                        .HasForeignKey("WinnerTeamId");

                    b.Navigation("Tournament");

                    b.Navigation("WinnerTeam");
                });

            modelBuilder.Entity("MatchTeam", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.Team", null)
                        .WithMany()
                        .HasForeignKey("CompetitorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESTMS.API.DataAccess.Entities.Match", null)
                        .WithMany()
                        .HasForeignKey("MatchesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
                    b.HasOne("ESTMS.API.DataAccess.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.ApplicationUser", null)
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

                    b.HasOne("ESTMS.API.DataAccess.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TeamTournament", b =>
                {
                    b.HasOne("ESTMS.API.DataAccess.Entities.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESTMS.API.DataAccess.Entities.Tournament", null)
                        .WithMany()
                        .HasForeignKey("TournamentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.ApplicationUser", b =>
                {
                    b.Navigation("ReceivedInvitations");

                    b.Navigation("SentInvitations");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Match", b =>
                {
                    b.Navigation("Winner");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Team", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.TeamManager", b =>
                {
                    b.Navigation("Teams");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.Tournament", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("ESTMS.API.DataAccess.Entities.TournamentManager", b =>
                {
                    b.Navigation("Tournaments");
                });
#pragma warning restore 612, 618
        }
    }
}

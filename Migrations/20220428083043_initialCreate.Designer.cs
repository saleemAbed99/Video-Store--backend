﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VideoStore_Backend.Data;

namespace VideoStore_Backend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220428083043_initialCreate")]
    partial class initialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("FavoriteListVideo", b =>
                {
                    b.Property<int>("FavoriteListId")
                        .HasColumnType("int");

                    b.Property<int>("VideosId")
                        .HasColumnType("int");

                    b.HasKey("FavoriteListId", "VideosId");

                    b.HasIndex("VideosId");

                    b.ToTable("FavoriteListVideo");
                });

            modelBuilder.Entity("VideoStore_Backend.Models.FavoriteList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserUsername")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "UserUsername")
                        .IsUnique()
                        .HasFilter("[UserUsername] IS NOT NULL");

                    b.ToTable("FavoriteLists");
                });

            modelBuilder.Entity("VideoStore_Backend.Models.Gallery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "Username");

                    b.ToTable("Galleries");
                });

            modelBuilder.Entity("VideoStore_Backend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id", "Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VideoStore_Backend.Models.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("GalleryId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Uri")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GalleryId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("FavoriteListVideo", b =>
                {
                    b.HasOne("VideoStore_Backend.Models.FavoriteList", null)
                        .WithMany()
                        .HasForeignKey("FavoriteListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VideoStore_Backend.Models.Video", null)
                        .WithMany()
                        .HasForeignKey("VideosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VideoStore_Backend.Models.FavoriteList", b =>
                {
                    b.HasOne("VideoStore_Backend.Models.User", "User")
                        .WithOne("FavoriteList")
                        .HasForeignKey("VideoStore_Backend.Models.FavoriteList", "UserId", "UserUsername");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VideoStore_Backend.Models.Gallery", b =>
                {
                    b.HasOne("VideoStore_Backend.Models.User", "User")
                        .WithMany("Galleries")
                        .HasForeignKey("UserId", "Username");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VideoStore_Backend.Models.Video", b =>
                {
                    b.HasOne("VideoStore_Backend.Models.Gallery", "Gallery")
                        .WithMany("Videos")
                        .HasForeignKey("GalleryId");

                    b.Navigation("Gallery");
                });

            modelBuilder.Entity("VideoStore_Backend.Models.Gallery", b =>
                {
                    b.Navigation("Videos");
                });

            modelBuilder.Entity("VideoStore_Backend.Models.User", b =>
                {
                    b.Navigation("FavoriteList");

                    b.Navigation("Galleries");
                });
#pragma warning restore 612, 618
        }
    }
}
﻿// <auto-generated />
using KidsList.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace KidsList.Data.Migrations
{
    [DbContext(typeof(KidsListContext))]
    partial class KidsListContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("citext")
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("KidsList.Data.Family", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.HasKey("Id")
                        .HasName("pk_families");

                    b.ToTable("families");
                });

            modelBuilder.Entity("KidsList.Data.Kid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("FamilyId")
                        .HasColumnType("integer")
                        .HasColumnName("family_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_kids");

                    b.HasIndex("FamilyId")
                        .HasDatabaseName("ix_kids_family_id");

                    b.ToTable("kids");
                });

            modelBuilder.Entity("KidsList.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("citext")
                        .HasColumnName("email");

                    b.Property<int>("FamilyId")
                        .HasColumnType("integer")
                        .HasColumnName("family_id");

                    b.Property<string>("FullName")
                        .HasColumnType("text")
                        .HasColumnName("full_name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("FamilyId")
                        .HasDatabaseName("ix_users_family_id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("KidsList.Data.WishList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("KidId")
                        .HasColumnType("integer")
                        .HasColumnName("kid_id");

                    b.HasKey("Id")
                        .HasName("pk_wish_lists");

                    b.HasIndex("KidId")
                        .HasDatabaseName("ix_wish_lists_kid_id");

                    b.ToTable("wish_lists");
                });

            modelBuilder.Entity("KidsList.Data.WishListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("WishListId")
                        .HasColumnType("integer")
                        .HasColumnName("wish_list_id");

                    b.HasKey("Id")
                        .HasName("pk_wish_list_items");

                    b.HasIndex("WishListId")
                        .HasDatabaseName("ix_wish_list_items_wish_list_id");

                    b.ToTable("wish_list_items");
                });

            modelBuilder.Entity("KidsList.Data.Kid", b =>
                {
                    b.HasOne("KidsList.Data.Family", "Family")
                        .WithMany("Kids")
                        .HasForeignKey("FamilyId")
                        .HasConstraintName("fk_kids_families_family_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Family");
                });

            modelBuilder.Entity("KidsList.Data.User", b =>
                {
                    b.HasOne("KidsList.Data.Family", "Family")
                        .WithMany("Users")
                        .HasForeignKey("FamilyId")
                        .HasConstraintName("fk_users_families_family_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Family");
                });

            modelBuilder.Entity("KidsList.Data.WishList", b =>
                {
                    b.HasOne("KidsList.Data.Kid", "Kid")
                        .WithMany()
                        .HasForeignKey("KidId")
                        .HasConstraintName("fk_wish_lists_kids_kid_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kid");
                });

            modelBuilder.Entity("KidsList.Data.WishListItem", b =>
                {
                    b.HasOne("KidsList.Data.WishList", "WishList")
                        .WithMany("WishListItems")
                        .HasForeignKey("WishListId")
                        .HasConstraintName("fk_wish_list_items_wish_lists_wish_list_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WishList");
                });

            modelBuilder.Entity("KidsList.Data.Family", b =>
                {
                    b.Navigation("Kids");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("KidsList.Data.WishList", b =>
                {
                    b.Navigation("WishListItems");
                });
#pragma warning restore 612, 618
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutuspage> Aboutuspages { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Cridtcard> Cridtcards { get; set; }

    public virtual DbSet<Homepage> Homepages { get; set; }

    public virtual DbSet<Loginpic> Loginpics { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sharedlayout> Sharedlayouts { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<TrainerWorkout> TrainerWorkouts { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=localhost:1521;USER ID=C##Anas;PASSWORD=Test321;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##ANAS")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutuspage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008570");

            entity.ToTable("ABOUTUSPAGE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Aboutusheadertext)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("ABOUTUSHEADERTEXT");
            entity.Property(e => e.Aboutusparagraph)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("ABOUTUSPARAGRAPH");
            entity.Property(e => e.Headerpic)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("HEADERPIC");
            entity.Property(e => e.Headpic1)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("HEADPIC1");
            entity.Property(e => e.Headpic2)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("HEADPIC2");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("SYS_C008564");

            entity.ToTable("CONTACT");

            entity.Property(e => e.NoteId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("NOTE_ID");
            entity.Property(e => e.Notedate)
                .HasColumnType("DATE")
                .HasColumnName("NOTEDATE");
            entity.Property(e => e.Notetext)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("NOTETEXT");
            entity.Property(e => e.Useremail)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("USEREMAIL");
        });

        modelBuilder.Entity<Cridtcard>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("SYS_C008551");

            entity.ToTable("CRIDTCARD");

            entity.Property(e => e.CardId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CARD_ID");
            entity.Property(e => e.CardNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CARD_NUMBER");
            entity.Property(e => e.Cardbalance)
                .HasColumnType("NUMBER")
                .HasColumnName("CARDBALANCE");
            entity.Property(e => e.CodeCvv)
                .HasColumnType("NUMBER")
                .HasColumnName("CODE_CVV");
            entity.Property(e => e.Expirationdate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIRATIONDATE");
        });

        modelBuilder.Entity<Homepage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008568");

            entity.ToTable("HOMEPAGE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Discountheader)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("DISCOUNTHEADER");
            entity.Property(e => e.Discountpic)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("DISCOUNTPIC");
            entity.Property(e => e.Feedbackpic)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("FEEDBACKPIC");
            entity.Property(e => e.Joinusparagraph)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("JOINUSPARAGRAPH");
            entity.Property(e => e.Joinuspic)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("JOINUSPIC");
            entity.Property(e => e.Mainpic)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("MAINPIC");
            entity.Property(e => e.Mainstatement)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("MAINSTATEMENT");
        });

        modelBuilder.Entity<Loginpic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008553");

            entity.ToTable("LOGINPIC");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Adminid)
                .HasColumnType("NUMBER")
                .HasColumnName("ADMINID");
            entity.Property(e => e.Loginimagepath)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("LOGINIMAGEPATH");

            entity.HasOne(d => d.Admin).WithMany(p => p.Loginpics)
                .HasForeignKey(d => d.Adminid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008554");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("SYS_C008524");

            entity.ToTable("MEMBERS");

            entity.Property(e => e.MemberId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("MEMBER_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.FirstName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.Image)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.JoinDate)
                .HasColumnType("DATE")
                .HasColumnName("JOIN_DATE");
            entity.Property(e => e.LastName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.WorkoutPlaneId)
                .HasColumnType("NUMBER")
                .HasColumnName("WORKOUT_PLANE_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.Members)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008537");

            entity.HasOne(d => d.WorkoutPlane).WithMany(p => p.Members)
                .HasForeignKey(d => d.WorkoutPlaneId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008525");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("SYS_C008532");

            entity.ToTable("PAYMENTS");

            entity.Property(e => e.PaymentId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENT_ID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.MemberId)
                .HasColumnType("NUMBER")
                .HasColumnName("MEMBER_ID");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("DATE")
                .HasColumnName("PAYMENT_DATE");

            entity.HasOne(d => d.Member).WithMany(p => p.Payments)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008533");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("SYS_C008516");

            entity.ToTable("ROLE");

            entity.Property(e => e.RoleId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("ROLE_NAME");
        });

        modelBuilder.Entity<Sharedlayout>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008566");

            entity.ToTable("SHAREDLAYOUT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Copywritestatement)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COPYWRITESTATEMENT");
            entity.Property(e => e.Facebooklink)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("FACEBOOKLINK");
            entity.Property(e => e.Githublink)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("GITHUBLINK");
            entity.Property(e => e.Homelocation)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("HOMELOCATION");
            entity.Property(e => e.Logo)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("LOGO");
            entity.Property(e => e.Photerparagraph)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("PHOTERPARAGRAPH");
            entity.Property(e => e.Twitterlink)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("TWITTERLINK");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("SYS_C008518");

            entity.ToTable("STAFF");

            entity.Property(e => e.StaffId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("STAFF_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.FirstName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.Image)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.JoinDate)
                .HasColumnType("DATE")
                .HasColumnName("JOIN_DATE");
            entity.Property(e => e.LastName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008519");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.TestemonialId).HasName("SYS_C008535");

            entity.ToTable("TESTIMONIALS");

            entity.Property(e => e.TestemonialId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTEMONIAL_ID");
            entity.Property(e => e.Approved)
                .IsRequired()
                .HasPrecision(1)
                .HasDefaultValueSql("0 ")
                .HasColumnName("APPROVED");
            entity.Property(e => e.MemberId)
                .HasColumnType("NUMBER")
                .HasColumnName("MEMBER_ID");
            entity.Property(e => e.TestimonialsDate)
                .HasColumnType("DATE")
                .HasColumnName("TESTIMONIALS_DATE");
            entity.Property(e => e.TestimonialsText)
                .HasMaxLength(450)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIALS_TEXT");

            entity.HasOne(d => d.Member).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008536");
        });

        modelBuilder.Entity<TrainerWorkout>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008528");

            entity.ToTable("TRAINER_WORKOUT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.TrinerId)
                .HasColumnType("NUMBER")
                .HasColumnName("TRINER_ID");
            entity.Property(e => e.WorkoutId)
                .HasColumnType("NUMBER")
                .HasColumnName("WORKOUT_ID");

            entity.HasOne(d => d.Triner).WithMany(p => p.TrainerWorkouts)
                .HasForeignKey(d => d.TrinerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008529");

            entity.HasOne(d => d.Workout).WithMany(p => p.TrainerWorkouts)
                .HasForeignKey(d => d.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("SYS_C008530");
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("SYS_C008521");

            entity.ToTable("WORKOUT");

            entity.Property(e => e.WorkoutId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("WORKOUT_ID");
            entity.Property(e => e.Image)
                .HasMaxLength(350)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER")
                .HasColumnName("PRICE");
            entity.Property(e => e.Shift)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("SHIFT");
            entity.Property(e => e.WorkoutDuration)
                .HasColumnType("NUMBER")
                .HasColumnName("WORKOUT_DURATION");
            entity.Property(e => e.WorkoutName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("WORKOUT_NAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConversationApp.Data;

public partial class CaDbContext : DbContext
{
    public CaDbContext()
    {
    }

    public CaDbContext(DbContextOptions<CaDbContext> options)
        : base(options)
    {
    }

      public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<ConversationContent> ConversationContents { get; set; }

    public virtual DbSet<Hint> Hints { get; set; }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
       #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
       optionsBuilder.UseSqlite("DataSource=/Users/fareedkhan/Desktop/ConversationApp (11)/DB/ConversationApp-Local.db");
       optionsBuilder.EnableSensitiveDataLogging();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.ToTable("Conversation");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            
            entity.Property(e => e.Level)
                .HasColumnType(" INTEGER (3)");
            
            entity.Property(e => e.Context)
                .HasColumnName("Context");
            
            entity.Property(e => e.Subcontext)
                .HasColumnName("Subcontext");


            entity.HasOne(d => d.LearningLanguage).WithMany(p => p.ConversationLearningLanguages)
                .HasForeignKey(d => d.LearningLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.NativeLanguage).WithMany(p => p.ConversationNativeLanguages)
                .HasForeignKey(d => d.NativeLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ConversationContent>(entity =>
        {
            entity.ToTable("ConversationContent");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.ConversationId).HasColumnName("ConversationId");
            entity.Property(e => e.Line).HasColumnName("Line");
            entity.Property(e => e.Person).HasColumnName("Person");
        });

        modelBuilder.Entity<Hint>(entity =>
        {
            entity.ToTable("Hint");

            entity.HasOne(h => h.Conversation).WithMany(cc => cc.WordHints)
               .HasForeignKey(h => h.ConversationId)
               .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(h => h.ConversationContent).WithMany(cc => cc.Hints)
               .HasForeignKey(h => h.ConversationContentId)
               .OnDelete(DeleteBehavior.Cascade); 

            entity.Property(e => e.Language).HasColumnName("Language");

            entity.Property(e => e.Word).HasColumnName("Word");

            entity.Property(e => e.Content).HasColumnName("Content");

            entity.HasIndex(e => e.TargetLanguage, "TargetLanguage");
            
            entity.HasIndex(e => e.TargetLanguageWord, "TargetLanguageWord");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity.ToTable("History");
            
            entity.Property(e => e.DateTime).HasColumnType("NUMERIC");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasIndex(e => e.EmailAddress, "IX_User_EmailAddress").IsUnique();
            entity.Property(e => e.EmailAddress).HasColumnType("TEXT (100)");
            entity.Property(e => e.Level).HasColumnType("INTEGER (3)");
            entity.Property(e => e.Name).HasColumnType("TEXT (50)");
            entity.Property(e => e.JoinDate).HasColumnType("DATE");
        });

        modelBuilder.Entity<Language>(entity => 
        {
               entity.ToTable("Language");
               entity.Property(e => e.Name).HasColumnName("Name");
               entity.Property(e => e.Id)
               .ValueGeneratedNever()
               .HasColumnName("Id");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

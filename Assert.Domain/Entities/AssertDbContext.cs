using Assert.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assert.Domain.Flag;

public partial class AssertdbContext : DbContext
{
    public AssertdbContext()
    {
    }

    public AssertdbContext(DbContextOptions<AssertdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TLanguage> TLanguages { get; set; }

    public virtual DbSet<TuEmergencyContact> TuEmergencyContacts { get; set; }

    public virtual DbSet<TuUser> TuUsers { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=35.193.165.194;Database=assertdb;User Id=assertdb-user;Password=Fdgsh2025%;Trusted_Connection=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TLanguage>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__T_Langua__12696A625F093F0B");

            entity.ToTable("T_Language");

            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Detail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("detail");
        });

        modelBuilder.Entity<TuEmergencyContact>(entity =>
        {
            entity.HasKey(e => e.EmergencyContactId).HasName("PK__TU_Emerg__7394A15DB7FEDEA4");

            entity.ToTable("TU_EmergencyContact");

            entity.Property(e => e.EmergencyContactId).HasColumnName("emergencyContactId");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.LstName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("lstName");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PhoneCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phoneCode");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.Relationship)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("relationship");

            entity.HasOne(d => d.Language).WithMany(p => p.TuEmergencyContacts)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("FK_EmergencyContact_Language");
        });

        modelBuilder.Entity<TuUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("TU_User");

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.AccountType).HasColumnName("accountType");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.FavoriteName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("favoriteName");
            entity.Property(e => e.GenderTypeId).HasColumnName("genderTypeId");
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PhotoLink)
                .IsUnicode(false)
                .HasColumnName("photoLink");
            entity.Property(e => e.PlatformId).HasColumnName("platformId");
            entity.Property(e => e.RegisterDate)
                .HasColumnType("datetime")
                .HasColumnName("registerDate");
            entity.Property(e => e.SocialId)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("socialId");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TimeZoneId).HasColumnName("timeZoneId");
            entity.Property(e => e.TitleTypeId).HasColumnName("titleTypeId");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("userName");
            entity.Property(e => e.UserStatusTypeId).HasColumnName("userStatusTypeId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

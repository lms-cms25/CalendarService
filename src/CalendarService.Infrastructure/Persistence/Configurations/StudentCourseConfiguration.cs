using CalendarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarService.Infrastructure.Persistence.Configurations;

public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
{
    public void Configure(EntityTypeBuilder<StudentCourse> builder)
    {
        // 1. Döp tabellen i databasen (notera underscore enligt din korrigerade standard!)
        builder.ToTable("student_courses");

        // 2. Sätt primärnyckel
        builder.HasKey(sc => sc.Id);

        // 3. Konfigurera UserId (Guid-strängen)
        builder.Property(sc => sc.UserId)
            .IsRequired()
            .HasMaxLength(100); // En standard Guid är 36 tecken, så 100 ger marginal för framtida ID-system

        // 4. Konfigurera CourseId
        builder.Property(sc => sc.CourseId)
            .IsRequired();

        // 5. 🧠 PRESTANDA-BOOST: Skapa ett index på UserId!
        builder.HasIndex(sc => sc.UserId);
    }
}
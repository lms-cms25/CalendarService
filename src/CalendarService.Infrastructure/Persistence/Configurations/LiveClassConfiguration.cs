using CalendarService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarService.Infrastructure.Persistence.Configurations;

public class LiveClassConfiguration : IEntityTypeConfiguration<LiveClass>
{
    public void Configure(EntityTypeBuilder<LiveClass> builder)
    {
        // 1. Sätt tabellnamn och primärnyckel
        builder.ToTable("LiveClass");
        builder.HasKey(lc => lc.Id);

        // 2. Konfigurera relationen till Event
        // Vi säger: Den här LiveClass har ett Event, och det Eventet har en LiveClass.
        // Vi talar också om att Foreign Key är 'EventId'
        builder.HasOne<Event>()
               .WithOne() // Om du lägger till 'public LiveClass LiveClass { get; set; }' i Event.cs sätter du den här
               .HasForeignKey<LiveClass>(lc => lc.EventId)
               .OnDelete(DeleteBehavior.Cascade); // Om ett Event raderas, försvinner live-länken automatiskt!
    }
}
using Anonymous_Topics.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anonymous_Topics.Database.Configurations
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {

            //builder.HasMany(c => c.TopicCategory);


        }
    }
}

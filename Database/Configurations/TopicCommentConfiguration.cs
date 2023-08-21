using Anonymous_Topics.Database.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Anonymous_Topics.Database.Configurations
{
    public class TopicCommentConfiguration : IEntityTypeConfiguration<TopicComment>
    {
        public void Configure(EntityTypeBuilder<TopicComment> builder)
        {
           // builder.Property(m => m.ParentTopicCommentId).IsRequired(false);
          


        }
    }
}

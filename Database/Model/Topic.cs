﻿using System.Data;

namespace Anonymous_Topics.Database.Model
{
    public class Topic
    {
        public Topic()
        {
            Id = Guid.NewGuid();
            SecurityKey = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public Guid TopicCatergoryId { get; set; }
        public TopicCategory? TopicCategory { get; set; }
        public  ICollection<TopicComment>?  TopicComment { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid SecurityKey { get; set; }

        

    
    }
}

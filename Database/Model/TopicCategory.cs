using Anonymous_Topics.Services.Intefaces;

namespace Anonymous_Topics.Database.Model
{
    public class TopicCategory
    {
        public TopicCategory()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

    
    }
}

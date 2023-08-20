namespace Anonymous_Topics.Database.Model
{
    public class Topic
    {
        public Topic()
        {
            Id=Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TopicCatergoryId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

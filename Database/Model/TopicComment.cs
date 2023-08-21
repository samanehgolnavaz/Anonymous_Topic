namespace Anonymous_Topics.Database.Model
{
    public class TopicComment
    {
        public TopicComment()
        {
            Id=Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string? Description { get; set; }
        public Guid? ParentTopicCommentId { get; set; }
        public Guid TopicId { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}

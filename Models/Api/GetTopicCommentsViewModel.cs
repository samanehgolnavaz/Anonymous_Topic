namespace Anonymous_Topics.Models.Api
{
    public record GetTopicCommentsViewModel(Guid CommentId,string Description,string UserName ,Guid TopicId,DateTime CreatedDate);
  
}

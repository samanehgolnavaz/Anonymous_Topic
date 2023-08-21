namespace Anonymous_Topics.Models.Api
{
    public record GetTopicCommentsViewModel(Guid Id,string Description,string UserName ,Guid TopicId,DateTime CreatedDate);
  
}

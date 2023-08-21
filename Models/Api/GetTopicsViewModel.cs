using Anonymous_Topics.Database.Model;

namespace Anonymous_Topics.Models.Api
{
    public record GetTopicsViewModel(Guid Id,string Title ,string Description,Guid TopicCategoryId,string Image,bool IsClosed,DateTime CreatedDate);
 
}

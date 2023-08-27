using Anonymous_Topics.Database;
using Anonymous_Topics.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Anonymous_Topics.Pages
{
    [AutoValidateAntiforgeryToken] 
    public class TopicsGridModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TopicsGridModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<GetTopicCommentsViewModel> TopicComments { get; set; } = new();
        public List<GetTopicsViewModel> Topics { get; set; } = new();
        public List<GetCategoriesViewModel> Categories { get; set; } = new();


        public async Task OnGetAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                Topics = await _context
                 .Topics
                 .AsNoTracking()
                 .Where(n => n.TopicCatergoryId == id)
                 .Select(c => new GetTopicsViewModel
                    (c.Id
                      , c.Title
                      , c.Description
                      , c.TopicCatergoryId
                      , c.TopicCategory.Name
                      , c.Image
                      , c.IsClosed
                      , c.CreatedDate
                      , c.SecurityKey
                      
                    ))
                      
                 .ToListAsync();
            }
            else
            {
                Topics = await _context
                   .Topics
                   .AsNoTracking()
                   .Select(c => new GetTopicsViewModel
                     (c.Id
                       , c.Title
                       , c.Description
                       , c.TopicCatergoryId
                       , c.TopicCategory.Name
                       , c.Image
                       , c.IsClosed
                       , c.CreatedDate
                       , c.SecurityKey
                     ))
                   .ToListAsync();
            }
            Categories = await _context
                .TopicCategories
                .AsNoTracking()
                .Select(c => new GetCategoriesViewModel
                    (c.Id
                      , c.Name
                ))
                .ToListAsync();

            TopicComments = await _context
                 .TopicComments
                 .AsNoTracking()
                 .OrderByDescending(c => c.CreatedDate)
                 .Select(s => new GetTopicCommentsViewModel
                   (s.Id,
                     s.Description,
                     s.UserName,
                     s.TopicId,
                     s.CreatedDate,
                     s.ParentTopicCommentId
                   ))
                   .ToListAsync();
        }
    }
}

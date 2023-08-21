using Anonymous_Topics.Database;
using Anonymous_Topics.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Anonymous_Topics.Pages
{
    public class TopicsGridModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TopicsGridModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]

        public List<GetTopicsViewModel> Topics { get; set; } = new();
        public List<GetCategoriesViewModel> Categories { get; set; } = new();


        public async Task OnGetAsync()
        {
            Topics = await _context
                .Topics
                .AsNoTracking()
                .Select(c => new GetTopicsViewModel
                    (c.Id
                      , c.Title
                      , c.Description
                      , c.TopicCatergoryId
                      , c.Image
                      , c.IsClosed
                      , c.CreatedDate

                ))
                .ToListAsync();
            Categories=await _context
                .TopicCategories
                .AsNoTracking()
                .Select(c => new GetCategoriesViewModel
                    (c.Id
                      , c.Name
               

                ))
                .ToListAsync();
        }
    }
}

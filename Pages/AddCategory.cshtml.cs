using System.ComponentModel.DataAnnotations;
using Anonymous_Topics.Database;
using Anonymous_Topics.Database.Model;
using Anonymous_Topics.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;

namespace Anonymous_Topics.Pages
{
    [AutoValidateAntiforgeryToken]

    public class AddCategoryModel : PageModel
    {

        [BindProperty]
        [Required]
        public string Name { get; set; }
        public List<GetCategoriesViewModel> TopicCategories { get; set; } = new();
        private readonly ApplicationDbContext _context;

        public AddCategoryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        
        {
            TopicCategories = await _context
                .TopicCategories
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                       .Select(c => new GetCategoriesViewModel
                    (c.Id
                        , c.Name
                    )
                )
                .ToListAsync();
        }
        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            var topicCaterory = new TopicCategory()
            {
                Name = Name
            };
            _context.TopicCategories.Add(topicCaterory);
            var result = await _context.SaveChangesAsync(cancellationToken);
            ViewData["Success"] = $"Category Added Successfully .";
            return RedirectToPage("/AddCategory");

        }
    }
}

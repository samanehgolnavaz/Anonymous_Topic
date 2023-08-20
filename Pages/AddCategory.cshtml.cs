using Anonymous_Topics.Database;
using Anonymous_Topics.Database.Model;
using Anonymous_Topics.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Anonymous_Topics.Pages
{
    public class AddCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        [Required]
        public string Name { get; set; }
        public List<GetCategoriesViewModel> TopicCategories { get; set; } = new();
        public AddCategoryModel(string name, ApplicationDbContext context)
        {
            Name = name;
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
        public async Task OnPost(CancellationToken cancellationToken)
        {
            var topicCaterory = new TopicCategory()
            {
                Name = Name
            };
            _context.TopicCategories.Add(topicCaterory);
            var result = await _context.SaveChangesAsync(cancellationToken);
            ViewData["Success"] = $"Category Added Successfully .";

        }
    }
}

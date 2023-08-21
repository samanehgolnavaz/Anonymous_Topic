using Anonymous_Topics.Database;
using Anonymous_Topics.Database.Model;
using Anonymous_Topics.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net;

namespace Anonymous_Topics.Pages
{
    public class AddTopicModel : PageModel
    {
        [BindProperty]
        [Required]
        public string Title { get; set; }
        [BindProperty]
        [Required]
        public string Description { get; set; }
        [BindProperty]
        [Required]
        public Guid TopicCategoryId { get; set; }
        [BindProperty]
        [Required]
        public int Images { get; set; }
        private readonly ApplicationDbContext _context;

        public AddTopicModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<GetTopicsViewModel> Topics { get; set; } = new();

        public List<TopicCategory> AvailableTopicCategories { get; set; }

  
        public async Task  OnGetAsync(CancellationToken cancellationToken)
        {
            AvailableTopicCategories = await _context.TopicCategories.AsNoTracking().ToListAsync(cancellationToken);

            Topics = await _context
              .Topics
              .AsNoTracking()
              .OrderByDescending(x => x.Id)
                     .Select(c => new GetTopicsViewModel
                  (c.Id
                      , c.Title
                      , c.Description
                      , c.TopicCatergoryId
                      , c.Image
                      , c.IsClosed
                      ,c.CreatedDate

                  
              ))
              .ToListAsync();
        }
        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            var topic = new Topic()
            {
              Title = Title,
              Description = Description,
              TopicCatergoryId=TopicCategoryId,
              Image="",
              IsClosed=false

             
            };
            _context.Topics.Add(topic);
            var result = await _context.SaveChangesAsync(cancellationToken);
            AvailableTopicCategories = await _context.TopicCategories.AsNoTracking().ToListAsync(cancellationToken);
            ViewData["Success"] = $"Topic Added Successfully .";

            return RedirectToPage("/AddTopic");

        }


    }
}

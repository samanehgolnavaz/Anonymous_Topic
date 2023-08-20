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
        public int TopicCategoryId { get; set; }

        public List<GetTopicsViewModel>  Topics { get; set; }
        public List<TopicCategory> AvailableTopicCategories { get; set; }

        private readonly ApplicationDbContext _context;

        public AddTopicModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task  OnGetAsync(CancellationToken cancellationToken)
        {
            AvailableTopicCategories = await _context.TopicCategories.AsNoTracking().ToListAsync(cancellationToken);

        }
        public async Task OnPost(CancellationToken cancellationToken)
        {
            var topic = new Topic()
            {
              Title = Title,
              Description = Description
             
            };
            _context.Topics.Add(topic);
            var result = await _context.SaveChangesAsync(cancellationToken);
            AvailableTopicCategories = await _context.TopicCategories.AsNoTracking().ToListAsync(cancellationToken);
            ViewData["Success"] = $"Topic Added Successfully .";



        }


    }
}

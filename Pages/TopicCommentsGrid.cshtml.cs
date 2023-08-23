using Anonymous_Topics.Database;
using Anonymous_Topics.Database.Model;
using Anonymous_Topics.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Anonymous_Topics.Pages
{
    public class TopicCommentsGridModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TopicCommentsGridModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Guid? Id { get; set; }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public string Image { get; set; }
        [BindProperty]
        public bool IsClosed { get; set; }
        [BindProperty]
        public List<GetTopicsViewModel> Topics { get; set; } = new();
        public List<GetTopicCommentsViewModel> TopicComments { get; set; } = new();
        [BindProperty]
        public Guid CommentId { get; set; }

        [BindProperty]
        [Required]
        public string  CommentDescription { get; set; }

        [BindProperty]
        [Required]
        public string UserName { get; set; }

        [BindProperty]
        public Guid TopicId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var topic = await _context.Topics.FindAsync(id);
        
            TopicComments = await _context
                .TopicComments
                .AsNoTracking()
                .OrderByDescending(c => c.CreatedDate)
                .Where(a => a.TopicId == id)
                .Select(s => new GetTopicCommentsViewModel(
                s.Id,
                s.Description,
                s.UserName,
                s.TopicId,
                s.CreatedDate,
                s.ParentTopicCommentId
                )).ToListAsync();
            
            if (topic is null)
            {
                TempData["Error"] = "User not found";
                return RedirectToPage("TopicsGrid");
            }
            ViewData["Id"] = topic.Id;
            Id = topic.Id;
            Title = topic.Title;
            Description = topic.Description;
            Image = topic.Image;
            IsClosed= topic.IsClosed;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var topicId = new Guid(Request.Query["id"].ToString());
            var topicComment = new TopicComment()
            {
                Description= CommentDescription,
                UserName = UserName,
                TopicId = topicId

            };
            _context.TopicComments.Add(topicComment);
            await _context.SaveChangesAsync();
            return RedirectToPage("/TopicCommentsGrid", new { id = topicId });
        }

        public async Task<IActionResult> OnPostCloseCommentAsync()
        {
            //var guidValue = Request.Query["id"].ToString();
            if (Id==TopicId)
            {
                var topic = await _context.Topics.FindAsync(Id);
                if (topic != null)
                {
                    topic.IsClosed=true;
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                TempData["Error"] = "You are not Allowed to Close the Topic";

            }
            return RedirectToPage("/TopicCommentsGrid", new { id = Id });
        }
        public async  Task<IActionResult> OnPostAddNestedCommentAsync()
        {
            var topicId = new Guid(Request.Query["id"].ToString());
            //var topicId = Guid.Parse("464BBB59-ECBE-4626-A14F-2D382F61AFB4");
            if (Guid.TryParse(Request.Query["id"], out Guid id))
            {
                Guid guidFromQueryString = id;

            }
            var topicComment = new TopicComment()
            {
                Description = CommentDescription,
                UserName = UserName,
                TopicId =topicId,
                ParentTopicCommentId = CommentId

            };
            _context.TopicComments.Add(topicComment);
            await _context.SaveChangesAsync();
            return RedirectToPage("/TopicCommentsGrid", new { id = topicId });

        }
    }
}

using Anonymous_Topics.Database;
using Anonymous_Topics.Database.Model;
using Anonymous_Topics.Models.Api;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Anonymous_Topics.Pages
{
    [AutoValidateAntiforgeryToken]

    public class TopicCommentsGridModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly DNTCaptchaOptions _captchaOptions;


        public TopicCommentsGridModel(ApplicationDbContext context, IDNTCaptchaValidatorService validatorService, IOptions<DNTCaptchaOptions> options)
        {
            _context = context;
            _validatorService = validatorService;
            _captchaOptions = options == null ? throw new ArgumentNullException(nameof(options)) : options.Value;
        }
        [BindProperty]
        public Guid? Id { get; set; }
        [BindProperty]
        public string? Title { get; set; }
        [BindProperty]
        public string? Description { get; set; }
        [BindProperty]
        public string? Image { get; set; }
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
        public Guid SecurityKey { get; set; }
        [BindProperty]
        public string DNTCaptchaInputText { get; set; }

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
            SecurityKey= topic.SecurityKey;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var topicId = new Guid(Request.Query["id"].ToString());           
            if (!_validatorService.HasRequestValidCaptchaEntry())
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName,
                    "Please enter the security code as a number.");
                return RedirectToPage("/TopicCommentsGrid", new { id = topicId });
            }
            else
            {
                var topicComment = new TopicComment()
                {
                    Description = CommentDescription,
                    UserName = UserName,
                    TopicId = topicId
                };
                _context.TopicComments.Add(topicComment);
                await _context.SaveChangesAsync();
                return RedirectToPage("/TopicCommentsGrid", new { id = topicId });
            }
        }

        public async Task<IActionResult> OnPostCloseCommentAsync()
        {
            var topic = await _context.Topics.FindAsync(Id);
            //var guidValue = Request.Query["id"].ToString();           
            if (topic != null)
            {
                    var securityKey = await _context.Topics
                    .Where(s => s.Id == Id)
                    .Select(s => s.SecurityKey)
                    .FirstOrDefaultAsync();
                if (securityKey==SecurityKey && securityKey!=Guid.Empty)
                {
                    topic.IsClosed = true;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    TempData["Error"] = "You are not Allowed to Close the Topic";

                }

            }

            return RedirectToPage("/TopicCommentsGrid", new { id = Id });
        }
    
        public async  Task<IActionResult> OnPostAddNestedCommentAsync()
        {
            if (!_validatorService.HasRequestValidCaptchaEntry())
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName,
                    "Please enter the security code as a number.");
                return RedirectToPage("/TopicCommentsGrid", new { id = Id });
            }
            else
            {
                var topicComment = new TopicComment()
                {
                    Description = CommentDescription,
                    UserName = UserName,
                    TopicId = (Guid)Id,
                    ParentTopicCommentId = CommentId
                };
                _context.TopicComments.Add(topicComment);
                await _context.SaveChangesAsync();
                return RedirectToPage("/TopicCommentsGrid", new { id = Id });
            }
        }
    }
}

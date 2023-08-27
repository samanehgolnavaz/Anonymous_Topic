using Anonymous_Topics.Database;
using Anonymous_Topics.Database.Model;
using Anonymous_Topics.Models.Api;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;

namespace Anonymous_Topics.Pages
{
    [AutoValidateAntiforgeryToken]

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
        public Guid TopicCategoryName { get; set; }
        [BindProperty]
        [Required]
        public int SecurityKey { get; set; }
 
        [BindProperty]
        [Required]
        public int? Images { get; set; }
        private readonly ApplicationDbContext _context;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly DNTCaptchaOptions _captchaOptions;


        public AddTopicModel(ApplicationDbContext context, IDNTCaptchaValidatorService validatorService, IOptions<DNTCaptchaOptions> options)
        {
            _context = context;
            _validatorService = validatorService;
            _captchaOptions = options == null ? throw new ArgumentNullException(nameof(options)) : options.Value;

        }
        public List<GetTopicsViewModel> Topics { get; set; } = new();
        public List<TopicCategory> AvailableTopicCategories { get; set; }


        public async Task OnGetAsync(CancellationToken cancellationToken)
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
                      , c.TopicCategory.Name
                      , c.Image
                      , c.IsClosed
                      , c.CreatedDate
                      , c.SecurityKey
                     

              ))
              .ToListAsync();
        }
   
        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            if (!_validatorService.HasRequestValidCaptchaEntry())
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName,
                    "Please enter the security code as a number.");
                return RedirectToPage("/AddTopic");
            }
            else
            {
                var topic = new Topic()
                {
                    Title = Title,
                    Description = Description,
                    TopicCatergoryId = TopicCategoryId,
                    Image = "",
                    IsClosed = false

                };
                _context.Topics.Add(topic);
                var result = await _context.SaveChangesAsync(cancellationToken);
                AvailableTopicCategories = await _context.TopicCategories.AsNoTracking().ToListAsync(cancellationToken);
                ViewData["Success"] = $"Topic Added Successfully Please Save Your SecurityKey:{topic.SecurityKey}.";
                return RedirectToPage("/AddTopic");
                //ViewData["Success"] = $"Topic Failed Successfully Please Save Your SecurityKey:{topic.SecurityKey}.";
            }
        }


    }
}

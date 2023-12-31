using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TiedAppLocal.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment env;

        public IndexModel(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public string ReadMe { get; set; }
        public IActionResult OnGet()
        {
            string readmePath = Path.Combine(env.ContentRootPath, "README.md");

            string stringToReturn = null;
            using (TextReader textReader = new StreamReader(readmePath))
            {
                stringToReturn = textReader.ReadToEnd();
                textReader.Dispose();
                textReader.Close();
            }
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            ReadMe = Markdown.ToHtml(stringToReturn, pipeline);

            return Page();
        }
    }
}

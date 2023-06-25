using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace ClientsCRUD.Pages
{
    public class UploadFileModel : PageModel
    {
        public void OnGet()
        {
        }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public string fileName="";
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                fileName = Path.GetFileName(ImageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", fileName);

                using (var fileStream = new FileStream(uploadPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                Message = "Image uploaded successfully!";
            }
            else
            {
                Message = "Please select a valid image file.";
            }

            return Page();
            }
        }

}


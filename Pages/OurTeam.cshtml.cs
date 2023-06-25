using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientsCRUD.Pages
{
	public class OurTeamModel : PageModel
    {
        public List<string> ImageFiles { get; set; }

        public void OnGet()
        {
            string imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image");
            string[] files = Directory.GetFiles(imagesFolderPath);

            ImageFiles = new List<string>();

            foreach (var file in files)
            {

                string imageUrl = "/image/" + Path.GetFileName(file);
                ImageFiles.Add(imageUrl);
            }
        }
    }
}

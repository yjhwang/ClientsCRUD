using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientsCRUD.Models;

namespace ClientsCRUD.Pages
{
	public class LoginModel : PageModel
    {
            private readonly SignInManager<MainUser> _signInManager;

            public LoginModel(SignInManager<MainUser> signInManager)
            {
                _signInManager = signInManager;
            }

            [BindProperty]
            public LoginInputModel Input { get; set; }

            public string ReturnUrl { get; set; }

            public class LoginInputModel
            {
                [Required]
                [EmailAddress]
                public string Email { get; set; }

                [Required]
                [DataType(DataType.Password)]
                public string Password { get; set; }

                [Display(Name = "Remember me?")]
                public bool RememberMe { get; set; }
            }

            public void OnGet(string returnUrl = null)
            {
                ReturnUrl = returnUrl;
            }

            public async Task<IActionResult> OnPostAsync(string returnUrl = null)
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return LocalRedirect(returnUrl ?? Url.Content("~/"));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }
                return Page();
            }
        }

    
}

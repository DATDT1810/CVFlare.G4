using CV_Flare.RazorPage.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace CV_Flare.RazorPage.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public RegisterVM RegisterVM { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var model = new
                {
                    email = registerVM.email,
                    password = registerVM.password
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7000/api/User/Register");
                request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var client = _httpClientFactory.CreateClient();
                try
                {
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "true";
                        //return RedirectToPage("/Login");
                        return Page();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid register attempt.");
                        return Page();
                    }

                }
                catch (HttpRequestException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    return Page();
                }
            }
            return Page();
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mongo.Web.Models;
using Mongo.Web.Service.IService;
using Mongo.Web.Utility;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mongo.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View(loginRequestDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Admin",Value="Admin"} ,
                new SelectListItem(){Text = "Customer",Value="Customer"} ,

            };
            ViewBag.RoleList = roleList;
            RegistrationRequestDto registerationRequestDto = new RegistrationRequestDto();
            return View(registerationRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto model)
        {
            ResponseDto responseDto= await _authService.RegisterAsync(model);
            if(responseDto!=null && responseDto.IsSuccess)
            {
                if(string.IsNullOrEmpty(model.Role))
                {
                    model.Role = "Customer"; 
                }
                var assignRole = await _authService.AssignRoleAsync(model);
                if(assignRole!=null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem(){Text = "Admin",Value="Admin"} ,
                new SelectListItem(){Text = "Customer",Value="Customer"} ,

            };
            ViewBag.RoleList = roleList;
            return View(model);



        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            ResponseDto responseDto = await _authService.LoginAsync(model);
            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("CustomError", responseDto.Message);
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
            
        }

        


    }
}

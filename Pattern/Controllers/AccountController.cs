using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Simplification;
using Pattern.DTO;
using Pattern.Models;
using Pattern.Service.Interface;
using Pattern.Utility;

namespace Pattern.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAccountService _service;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountService service, IMapper mapper, IConfiguration configuration)
        {
            _service = service;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string? returnUrl="")
        {
            if(!string.IsNullOrWhiteSpace(returnUrl))
            {
                TempData["returnUrl"] = returnUrl;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] LoginVM user)
        {
            LoginDTO loginDTO = _mapper.Map<LoginDTO>(user);
            var userDTO = await _service.LoginAsync(loginDTO);
            if (userDTO == null)
            {
                TempData["error"] = "Try again";
                return View(user);
            }
            GenerateCookie(userDTO);

            TempData["success"] = "Logged in!";

            string returnUrl = TempData["returnUrl"]?.ToString() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Redirect(returnUrl);
            }


            if (userDTO.Role == "admin" && userDTO.Position=="admin") 
            {
                return RedirectToAction("Index", "Skill");
            }   
            return RedirectToAction("Index","Home");
        }

        /// <summary>
        /// Take returnUrl as queryString
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout(string? returnUrl)
        {
            if (Request.Cookies["auth"] != null)
            {
                TempData["warning"] = "Logout successful!";
                Response.Cookies.Delete("auth");
            }
            if(!string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction(returnUrl);
            }
            return RedirectToAction("Index", new LoginVM());

        }


        private void GenerateCookie(UserDTO userDTO)
        {
            JwtSetting jwtSetting = _configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();

            var authToken = JwtHelper.GenerateToken(jwtSetting, userDTO);

            CookieOptions cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                MaxAge = TimeSpan.FromMinutes(120),
            };

            HttpContext.Response.Cookies.Append("auth", authToken, cookieOptions);

        }
    }
}

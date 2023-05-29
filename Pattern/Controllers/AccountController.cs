using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] LoginVM user, [FromQuery] string returnUrl = "")
        {
            LoginDTO loginDTO = _mapper.Map<LoginDTO>(user);
            var userDTO = await _service.LoginAsync(loginDTO);
            if (userDTO == null)
            {
                TempData["error"] = "Try again";
                return View(user);
            }
            GenerateCookie(userDTO);

            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction(returnUrl);
            }

            if(userDTO.Role == "admin" && userDTO.Position=="admin") 
            {
                return RedirectToAction("Index", "Skill");
            }
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (Request.Cookies["auth"] != null)
            {
                TempData["warning"] = "Logout successful!";
                Response.Cookies.Delete("auth");
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

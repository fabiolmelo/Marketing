using Marketing.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    // Injeção de Dependência via Construtor
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(LoginDto model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Login, Email = model.Login };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Opcional: Logar o usuário automaticamente após o registro
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            
            // Adiciona erros do Identity ao ModelState para exibição na View
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // Se falhou, retorna para o formulário com erros
        return View(model);
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            // Tenta fazer o login com senha. isPersistent: false = login por sessão (cookie temporário)
            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                // Redireciona para a URL original que o usuário tentou acessar, ou para a Home
                return LocalRedirect(returnUrl ?? "/");
            }
            if (result.IsLockedOut)
            {
                // Tratar caso de usuário bloqueado
                return RedirectToAction("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
                return View(model);
            }
        }

        // Se falhou, retorna para o formulário
        return View(model);
    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}

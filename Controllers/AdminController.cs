using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // Gerenciar usuarios
    public async Task<IActionResult> Usuarios()
    {
        var usuarios = _userManager.Users.ToList();
        var rolesPorUsuario = new Dictionary<string, string>();

        foreach (var usuario in usuarios)
        {
            var roles = await _userManager.GetRolesAsync(usuario);
            var role = roles.FirstOrDefault() ?? "Sem Perfil";
            rolesPorUsuario.Add(usuario.Id, role);
        }
        ViewBag.RolesUsuarios = rolesPorUsuario;
        return View(usuarios);
    }

    // Gerenciar roles
    public async Task<IActionResult> Roles()
    {
        var roles = _roleManager.Roles.ToList();
        return View(roles);
    }

    public async Task<IActionResult> ExcluirUsuario(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario != null)
        {
            var resultado = await _userManager.DeleteAsync(usuario);
            if (resultado.Succeeded)
            {
                return RedirectToAction("Usuarios");
            }
            else
            {
                ModelState.AddModelError("", "ERRO AO EXCLUIR O USUÁRIO");
            }
        }
        else
        {
            ModelState.AddModelError("", "USUÁRIO NÃO ENCONTRADO");
        }
        return RedirectToAction("Usuarios");
    }

    public async Task<IActionResult> TornarAdmin(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario != null)
        {
            var roles = await _userManager.GetRolesAsync(usuario);
            await _userManager.RemoveFromRolesAsync(usuario, roles);
            await _userManager.AddToRoleAsync(usuario, "Admin");

        }
        return RedirectToAction("Usuarios");
    }

    [HttpGet]
    public async Task<IActionResult> TrocarPerfil(string id)
    {
        var rolesDisponiveis = _roleManager.Roles.ToList();
        var roles = _roleManager.Roles.ToList();
        var usuario = await _userManager.FindByIdAsync(id);
        ViewBag.rolesDisponiveis = rolesDisponiveis;
        return View(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> TrocarPerfil(string idUser, string role)
    {
        var usuario = await _userManager.FindByIdAsync(idUser);

        if (usuario == null)
        {
            return NotFound();
        }

        var rolesAtuais = await _userManager.GetRolesAsync(usuario);

        if (rolesAtuais.Any())
        {
            await _userManager.RemoveFromRolesAsync(usuario, rolesAtuais);
        }

        await _userManager.AddToRoleAsync(usuario, role);

        return RedirectToAction("Usuarios");
    }

    //editar

    [HttpGet]
public async Task<IActionResult> EditarUsuario(string id)
{
    var usuario = await _userManager.FindByIdAsync(id);

    if (usuario is null)
        return NotFound();

    return View(usuario);
}

[HttpPost]
public async Task<IActionResult> EditarUsuario(string id, string email)
{
    var usuario = await _userManager.FindByIdAsync(id);

    if (usuario is null)
        return NotFound();

    usuario.Email = email;
    usuario.UserName = email;

    await _userManager.UpdateAsync(usuario);

    return RedirectToAction("Usuarios");
}

//deletar

[HttpGet]
public async Task<IActionResult> DeletarUsuario(string id)
{
    var usuario = await _userManager.FindByIdAsync(id);

    if (usuario is null)
        return NotFound();

    return View(usuario);
}

[HttpPost]
public async Task<IActionResult> ExcluirUsuarioConfirmado(string id)
{
    var usuario = await _userManager.FindByIdAsync(id);

    if (usuario is null)
        return NotFound();

    await _userManager.DeleteAsync(usuario);

    return RedirectToAction("Usuarios");
}
}
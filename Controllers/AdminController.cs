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

    #region Usuários

    // Listar usuários
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

    // Criar usuário GET
    public async Task<IActionResult> CriarUsuario()
    {
        ViewBag.Roles = _roleManager.Roles.ToList();
        return View();
    }

    // Criar usuário POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CriarUsuario(string email, string senha, string role)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
        {
            ModelState.AddModelError("", "Email e senha são obrigatórios.");
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View();
        }

        var usuario = new IdentityUser { UserName = email, Email = email };
        var resultado = await _userManager.CreateAsync(usuario, senha);

        if (resultado.Succeeded)
        {
            if (!string.IsNullOrWhiteSpace(role) && await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(usuario, role);
            }

            TempData["Sucesso"] = "Usuário criado com sucesso!";
            return RedirectToAction("Usuarios");
        }

        foreach (var erro in resultado.Errors)
        {
            var mensagemPt = TraduzirErroIdentity(erro.Description);
            ModelState.AddModelError("", mensagemPt);
        }

        ViewBag.Roles = _roleManager.Roles.ToList();
        return View();
    }

    // Editar usuário GET
    public async Task<IActionResult> EditarUsuario(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(usuario);
        ViewBag.RolesDisponiveis = _roleManager.Roles.ToList();
        ViewBag.RoleAtual = roles.FirstOrDefault() ?? "";

        return View(usuario);
    }

    // Editar usuário POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarUsuario(string id, string email, string role)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            usuario.Email = email;
            usuario.UserName = email;
            var resultado = await _userManager.UpdateAsync(usuario);

            if (!resultado.Succeeded)
            {
                foreach (var erro in resultado.Errors)
                {
                    var mensagemPt = TraduzirErroIdentity(erro.Description);
                    ModelState.AddModelError("", mensagemPt);
                }
                ViewBag.RolesDisponiveis = _roleManager.Roles.ToList();
                var rolesAtuais = await _userManager.GetRolesAsync(usuario);
                ViewBag.RoleAtual = rolesAtuais.FirstOrDefault() ?? "";
                return View(usuario);
            }
        }

        // Atualizar role
        var rolesAtuas = await _userManager.GetRolesAsync(usuario);
        if (!rolesAtuas.Contains(role) || rolesAtuas.Count > 0)
        {
            await _userManager.RemoveFromRolesAsync(usuario, rolesAtuas);
            if (!string.IsNullOrWhiteSpace(role) && await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(usuario, role);
            }
        }

        TempData["Sucesso"] = "Usuário atualizado com sucesso!";
        return RedirectToAction("Usuarios");
    }

    // Excluir usuário
    public async Task<IActionResult> ExcluirUsuario(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario != null)
        {
            var resultado = await _userManager.DeleteAsync(usuario);
            if (resultado.Succeeded)
            {
                TempData["Sucesso"] = "Usuário excluído com sucesso!";
                return RedirectToAction("Usuarios");
            }
            else
            {
                TempData["Erro"] = "Erro ao excluir o usuário.";
            }
        }
        else
        {
            TempData["Erro"] = "Usuário não encontrado.";
        }
        return RedirectToAction("Usuarios");
    }

    // Tornar Admin
    public async Task<IActionResult> TornarAdmin(string id)
    {
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario != null)
        {
            var roles = await _userManager.GetRolesAsync(usuario);
            await _userManager.RemoveFromRolesAsync(usuario, roles);
            await _userManager.AddToRoleAsync(usuario, "Admin");
            TempData["Sucesso"] = "Usuário promovido a Admin!";
        }
        return RedirectToAction("Usuarios");
    }

    // Trocar perfil GET
    public async Task<IActionResult> TrocarPerfil(string id)
    {
        var rolesDisponiveis = _roleManager.Roles.ToList();
        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        ViewBag.RolesDisponiveis = rolesDisponiveis;
        return View(usuario);
    }

    // Trocar perfil POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TrocarPerfil(string idUser, string role)
    {
        var usuario = await _userManager.FindByIdAsync(idUser);
        if (usuario == null)
        {
            return NotFound();
        }

        var roles = await _userManager.GetRolesAsync(usuario);
        await _userManager.RemoveFromRolesAsync(usuario, roles);

        if (!string.IsNullOrWhiteSpace(role) && await _roleManager.RoleExistsAsync(role))
        {
            await _userManager.AddToRoleAsync(usuario, role);
        }

        TempData["Sucesso"] = "Perfil do usuário alterado com sucesso!";
        return RedirectToAction("Usuarios");
    }

    #endregion

    #region Roles

    // Listar roles
    public async Task<IActionResult> Roles()
    {
        var roles = _roleManager.Roles.ToList();
        return View(roles);
    }

    // Criar role GET
    public IActionResult CriarRole()
    {
        return View();
    }

    // Criar role POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CriarRole(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            ModelState.AddModelError("nome", "Nome da role é obrigatório.");
            return View();
        }

        var role = new IdentityRole { Name = nome };
        var resultado = await _roleManager.CreateAsync(role);

        if (resultado.Succeeded)
        {
            TempData["Sucesso"] = "Role criada com sucesso!";
            return RedirectToAction("Roles");
        }

        foreach (var erro in resultado.Errors)
        {
            var mensagemPt = TraduzirErroIdentity(erro.Description);
            ModelState.AddModelError("", mensagemPt);
        }

        return View();
    }

    private string TraduzirErroIdentity(string descricao)
    {
        // Traduzir erros padrão do Identity para português
        if (descricao.Contains("already in use", StringComparison.OrdinalIgnoreCase) ||
            descricao.Contains("already taken", StringComparison.OrdinalIgnoreCase))
        {
            // Extrai o nome do campo (ex: "Role name 'Aluno' is already taken." -> "Aluno")
            var match = System.Text.RegularExpressions.Regex.Match(descricao, "'([^']+)'");
            if (match.Success)
            {
                var valor = match.Groups[1].Value;
                return $"O perfil '{valor}' já está em uso no sistema.";
            }
            return descricao.Replace("already in use", "já está em uso")
                           .Replace("already taken", "já está em uso");
        }

        if (descricao.Contains("Email", StringComparison.OrdinalIgnoreCase))
            return descricao.Replace("Email", "E-mail");

        if (descricao.Contains("Password", StringComparison.OrdinalIgnoreCase))
            return descricao.Replace("Password", "Senha");

        if (descricao.Contains("Passwords must have at least one non alphanumeric character", StringComparison.OrdinalIgnoreCase))
            return "A senha deve conter pelo menos um caractere especial (ex: @, #, $, %, etc).";

        if (descricao.Contains("Passwords must have at least one digit", StringComparison.OrdinalIgnoreCase))
            return "A senha deve conter pelo menos um número.";

        if (descricao.Contains("Passwords must have at least one uppercase", StringComparison.OrdinalIgnoreCase))
            return "A senha deve conter pelo menos uma letra maiúscula.";

        if (descricao.Contains("Passwords must have at least one lowercase", StringComparison.OrdinalIgnoreCase))
            return "A senha deve conter pelo menos uma letra minúscula.";

        if (descricao.Contains("Passwords must be at least", StringComparison.OrdinalIgnoreCase) ||
            descricao.Contains("too short", StringComparison.OrdinalIgnoreCase))
            return "A senha é muito curta. Deve ter no mínimo 4 caracteres.";

        if (descricao.Contains("invalid", StringComparison.OrdinalIgnoreCase))
            return "Valor inválido fornecido.";

        return descricao;
    }

    // Editar role GET
    public async Task<IActionResult> EditarRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        return View(role);
    }

    // Editar role POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarRole(string id, string nome)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(nome))
        {
            ModelState.AddModelError("nome", "Nome da role é obrigatório.");
            return View(role);
        }

        role.Name = nome;
        var resultado = await _roleManager.UpdateAsync(role);

        if (resultado.Succeeded)
        {
            TempData["Sucesso"] = "Role atualizada com sucesso!";
            return RedirectToAction("Roles");
        }

        foreach (var erro in resultado.Errors)
        {
            var mensagemPt = TraduzirErroIdentity(erro.Description);
            ModelState.AddModelError("", mensagemPt);
        }

        return View(role);
    }

    // Excluir role
    public async Task<IActionResult> ExcluirRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            // Verificar se há usuários com essa role
            var usuariosComRole = _userManager.Users.ToList();
            int usuariosAssociados = 0;

            foreach (var usuario in usuariosComRole)
            {
                if (!string.IsNullOrEmpty(role.Name) && await _userManager.IsInRoleAsync(usuario, role.Name))
                {
                    usuariosAssociados++;
                }
            }

            if (usuariosAssociados > 0)
            {
                TempData["Erro"] = $"Não é possível excluir a role '{role.Name}' pois há {usuariosAssociados} usuário(s) associado(s).";
                return RedirectToAction("Roles");
            }

            var resultado = await _roleManager.DeleteAsync(role);
            if (resultado.Succeeded)
            {
                TempData["Sucesso"] = "Role excluída com sucesso!";
                return RedirectToAction("Roles");
            }
            else
            {
                TempData["Erro"] = "Erro ao excluir a role.";
            }
        }
        else
        {
            TempData["Erro"] = "Role não encontrada.";
        }
        return RedirectToAction("Roles");
    }

    #endregion
}
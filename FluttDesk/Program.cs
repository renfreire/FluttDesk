using FluttDesk;
using FluttDesk.ContextDB;
using FluttDesk.Models;
using FluttDesk.Repositories;
using FluttDesk.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ContextDB>
    (options => options.UseSqlServer("Data Source=DESKTOP-HUUV3H9\\SQLEXPRESS;Initial Catalog=FluttDeskDB;Integrated Security=False;User ID=sa;Password=123456;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False"));

builder.Services.AddSwaggerGen();

var key = Encoding.ASCII.GetBytes(Settings.Secret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
});

var app = builder.Build();

app.UseSwagger();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", (Users model) => {
    var user = UserRepository.Get(model.UserName, model.UserPwd);
    if (user == null)
        return Results.NotFound(new { message = "Usuário ou Senha Inválidos" });

    var token = TokenService.GenerateToken(user);
    user.UserPwd = "";
    return Results.Ok(new
    {
        user = user,
        token = token,
    });
});

app.MapGet("/anonymous", () => { Results.Ok("anonymous"); });

app.MapGet("/authenticated", (ClaimsPrincipal user) =>
{
    Results.Ok(new { message = $"Authenticated as {user.Identity.Name}" });
}).RequireAuthorization();


/* *******************************************
 *              USUARIOS 
 ********************************************/
app.MapPost("AdicionarUsuario", async (Users user, ContextDB contexto) =>
{
    user.UserDtCreation = DateTime.Now;
    contexto.Users.Add(user);
    await contexto.SaveChangesAsync();
    return Results.Created($"AdicionarUsuario/{user.UserId}", user);
});

app.MapDelete("ExcluirUsuario/{id}", async (int userid, ContextDB contexto) =>
    {
        var userexcluir = await contexto.Users.FirstOrDefaultAsync(p => p.UserId == userid);
        if (userexcluir != null)
        {
            contexto.Users.Remove(userexcluir);
            await contexto.SaveChangesAsync();
        }
        else
        {
            Results.NotFound();
        }
    });

app.MapGet("ListarUsuarios", async (ContextDB contexto) =>
{
    return await contexto.Users.ToListAsync();
});

app.MapGet("ObterUsuario/{id}", async (int userid, ContextDB contexto) =>
{
    return await contexto.Users.FirstOrDefaultAsync(p => p.UserId == userid);
});

/* *******************************************
 *              ROLES 
 ********************************************/
app.MapPost("AdicionarRoles", async( Roles roles, ContextDB contexto) =>
{
    contexto.Roles.Add(roles);
    await contexto.SaveChangesAsync();
    return Results.Created($"AdicionarRoles/{roles.Roleid}", roles);
});

app.MapDelete("ExcluirRoles/{id}", async (int roleid, ContextDB contexto) =>
{
    var roleexcluir = await contexto.Roles.FirstOrDefaultAsync(p => p.Roleid == roleid);
    if (roleexcluir != null)
    {
        contexto.Roles.Remove(roleexcluir);
        await contexto.SaveChangesAsync();
    }
    else
    {
        Results.NotFound();
    }
});

app.MapGet("ListarRoles", async (ContextDB contexto) =>
{
   return await contexto.Roles.ToListAsync();
});

app.MapGet("ObterRoles/{id}", async (int roleid, ContextDB contexto) =>
{
    return await contexto.Roles.FirstOrDefaultAsync(p => p.Roleid == roleid);   
});

/* *******************************************
 *             PROJETOS
 ********************************************/
app.MapPost("AdicionarProjeto", async (Projects project, ContextDB contexto) =>
{
    project.PrjDtCreation = DateTime.Now;

    if ( project.PrjStatus is null )
    {
        project.PrjStatus = 0;
    }

    if (project.PrjPPR is null)
    {
        project.PrjPPR = false;
    }
    contexto.Projects.Add(project);
    await contexto.SaveChangesAsync();
    return Results.Created($"AdicionarProjeto/{project.Projectid}", project);
});

app.MapPut("AlterarProjeto/{id}", async (int projectid, Projects project, ContextDB contexto) =>
{
    if ( project != null)
    {
        var registro = await contexto.Projects.FindAsync(projectid);
         if ( registro != null) {
            registro.PrjRequester = project.PrjRequester != null ? project.PrjRequester : registro.PrjRequester ;
            registro.PrjTeamid = project.PrjTeamid != null ? project.PrjTeamid : registro.PrjTeamid ;
            registro.PrjTitle = project.PrjTitle != null ? project.PrjTitle : registro.PrjTitle;
            registro.PrjStatus = project.PrjStatus != null ? project.PrjStatus : registro.PrjStatus;
            registro.PrjDeptoRequester = project.PrjDeptoRequester != null ? project.PrjDeptoRequester : registro.PrjDeptoRequester;
            registro.PrjPPR = project.PrjPPR != null ? project.PrjPPR : registro.PrjPPR;
            registro.PrjDtClosed = project.PrjDtClosed != null ? project.PrjDtClosed : registro.PrjDtClosed;
            registro.PrjDeptoti = project.PrjDeptoti != null ? project.PrjDeptoti : registro.PrjDeptoti;
            registro.PrjEscope = project.PrjEscope != null ? project.PrjEscope : registro.PrjEscope;
            registro.PrjEstimatedHours = project.PrjEstimatedHours != null ? project.PrjEstimatedHours : registro.PrjEstimatedHours;
            registro.PrjLastChange = DateTime.Now;
            registro.PrjSystem = project.PrjSystem != null ? project.PrjSystem : registro.PrjSystem;
            registro.PrjTeamid = project.PrjTeamid != null ? project.PrjTeamid : registro.PrjTeamid;
            registro.PrjTechnicalResponsableid = project.PrjTechnicalResponsableid != null ? project.PrjTechnicalResponsableid : registro.PrjTechnicalResponsableid;
        } else   {
            return Results.NotFound();
        }
    } else  {
        return Results.NotFound();
    }

    await contexto.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("ExcluirProjeto/{id}", async (int projectid, ContextDB contexto) =>
{
    var projectexcluir = await contexto.Projects.FirstOrDefaultAsync(p => p.Projectid == projectid);
    if (projectexcluir != null)
    {
        contexto.Projects.Remove(projectexcluir);
        await contexto.SaveChangesAsync();
    }
    else
    {
        Results.NotFound();
    }
});

app.MapGet("ListarProjetos", async (ContextDB contexto) =>
{
    return await contexto.Projects.ToListAsync();
});

app.MapGet("ObterProjeto/{id}", async (int projectid, ContextDB contexto) =>
{
    return await contexto.Projects.FirstOrDefaultAsync(p => p.Projectid == projectid);
});

/* *******************************************
 *             ATIVIDADES
 ********************************************/
app.MapPost("AdicionarAtividade", async (ActivitiesProject activities, ContextDB contexto) =>
{
    contexto.ActivitiesProject.Add(activities);
    await contexto.SaveChangesAsync();
    return Results.Created($"AdicionarAtividade/{activities.Activityid}", activities);
});

app.MapDelete("ExcluirAtividade/{id}", async (int activityid, ContextDB contexto) =>
{
    var actexcluir = await contexto.ActivitiesProject.FirstOrDefaultAsync(p => p.Activityid == activityid);
    if (actexcluir != null)
    {
        contexto.ActivitiesProject.Remove(actexcluir);
        await contexto.SaveChangesAsync();
    }
    else
    {
        Results.NotFound();
    }
});

app.MapGet("ListarTodasAtividades", async (ContextDB contexto) =>
{
    return await contexto.ActivitiesProject.ToListAsync();
});

app.MapGet("ObterAtividadesPorId/{id}", async (int activityid, ContextDB contexto) =>
{
    return await contexto.ActivitiesProject.FirstOrDefaultAsync(p => p.Activityid == activityid);
});

app.MapGet("ObterAtividadesPorProjeto/{id}", async (int projectid, ContextDB contexto) =>
{
    return await contexto.ActivitiesProject.FirstOrDefaultAsync(p => p.ActProjectid == projectid);
});

/* *******************************************
 *             EQUIPES -TEAMS
 ********************************************/
app.MapPost("AdicionarEquipe", async (Teams teams, ContextDB contexto) =>
{
    contexto.Teams.Add(teams);
    await contexto.SaveChangesAsync();
    return Results.Created($"AdicionarEquipe/{teams.TeamId}", teams);
});

app.MapDelete("ExcluirEquipe/{id}", async (int teamid, ContextDB contexto) =>
{
    var teamexcluir = await contexto.Teams.FirstOrDefaultAsync(p => p.TeamId == teamid);
    if (teamexcluir != null)
    {
        contexto.Teams.Remove(teamexcluir);
        await contexto.SaveChangesAsync();
    }
});

app.MapGet("ListarEquipes", async (ContextDB contexto) =>
{
    return await contexto.Teams.ToListAsync();
});

app.MapGet("ObterEquipesPorId/{id}", async (int teamsid, ContextDB contexto) =>
{
    return await contexto.Teams.FirstOrDefaultAsync(p => p.TeamId == teamsid);
});

/* *******************************************
 *             MEMBROS -MEMBERS
 ********************************************/
app.MapPost("AdicionarMembros", async (Members members, ContextDB contexto) =>
{
    contexto.Members.Add(members);
    await contexto.SaveChangesAsync();
    return Results.Created($"AdicionarMembros/{members.Memberid}", members);
});

app.MapDelete("ExcluirMembros/{id}", async (int memberid, ContextDB contexto) =>
{
    var memberexcluir = await contexto.Members.FirstOrDefaultAsync(p => p.Memberid == memberid);
    if (memberexcluir != null)
    {
        contexto.Members.Remove(memberexcluir);
        await contexto.SaveChangesAsync();
    }
});

app.MapGet("ListarTodosMembros", async (ContextDB contexto) =>
{
    return await contexto.Members.ToListAsync();
});

app.MapGet("ObterMembrosPorId/{id}", async (int memberid, ContextDB contexto) =>
{
    return await contexto.Members.FirstOrDefaultAsync(p => p.Memberid == memberid);
});

app.MapGet("ObterMembrosPorUserId/{id}", async (int userid, ContextDB contexto) =>
{
    return await contexto.Members.FirstOrDefaultAsync(p => p.UserId == userid);
});

app.MapGet("ObterMembrosPorEquipe/{id}", async (int teamsid, ContextDB contexto) =>
{
    return await contexto.Members.FirstOrDefaultAsync(p => p.Teamid == teamsid);
});


/* *******************************************
 *   DEDICACAO DIARIA = DAIYLYDEDICATION
 ********************************************/
app.MapPost("AdicionarTempoDedicacao", async (DailyDedication dailyDedication, ContextDB contexto) =>
{
    contexto.DailyDedications.Add(dailyDedication);
    await contexto.SaveChangesAsync();
    return Results.Created($"AdicionarAtividade/{dailyDedication.Dailyid}", dailyDedication);
});

app.MapDelete("ExcluirTempoDedicacao/{id}", async (int dedicationid, ContextDB contexto) =>
{
    var dayliexcluir = await contexto.DailyDedications.FirstOrDefaultAsync(p => p.Dailyid == dedicationid);
    if (dayliexcluir != null)
    {
        contexto.DailyDedications.Remove(dayliexcluir);
        await contexto.SaveChangesAsync();
    }
});

app.MapGet("ListarTempoDedicacao", async (ContextDB contexto) =>
{
    return await contexto.DailyDedications.ToListAsync();
});

app.MapGet("ObterTempoDedicacaoPorId/{id}", async (int dedicationid, ContextDB contexto) =>
{
    return await contexto.DailyDedications.FirstOrDefaultAsync(p => p.Dailyid == dedicationid);
});

/* *******************************************
 *   STATUS ACTIVITIES
 ********************************************/
app.MapPost("AdicionarStatusAtividades", async (StatusActivities statusActivities, ContextDB contexto) =>
{
    contexto.StatusActivities.Add(statusActivities);
    await contexto.SaveChangesAsync();
    return Results.Created($"AdicionarAtividade/{statusActivities.Statusid}", statusActivities);
});

app.MapDelete("ExcluirStatusAtividades/{id}", async (int statusid, ContextDB contexto) =>
{
    var statusexcluir = await contexto.StatusActivities.FirstOrDefaultAsync(p => p.Statusid == statusid);
    if (statusexcluir != null)
    {
        contexto.StatusActivities.Remove(statusexcluir);
        await contexto.SaveChangesAsync();
    }
});

app.MapGet("ListarStatusAtividades", async (ContextDB contexto) =>
{
    return await contexto.StatusActivities.ToListAsync();
});

app.MapGet("ObterStatusPorId/{id}", async (int statusid, ContextDB contexto) =>
{
    return await contexto.StatusActivities.FirstOrDefaultAsync(p => p.Statusid == statusid);
});

/* *******************************************
 *           SISTEMAS - SYSTEMS
 ********************************************/
app.MapPost("AdicionarSistemas", async (Systems systems, ContextDB contexto) =>
{
    contexto.Systems.Add(systems);
    await contexto.SaveChangesAsync();
    return Results.Created($"AdicionarAtividade/{systems.Systemid}", systems);
});

app.MapDelete("ExcluirSistemas/{id}", async (int systemsid, ContextDB contexto) =>
{
    var systemexcluir = await contexto.Systems.FirstOrDefaultAsync(p => p.Systemid == systemsid);
    if (systemexcluir != null)
    {
        contexto.Systems.Remove(systemexcluir);
        await contexto.SaveChangesAsync();
    }
});

app.MapGet("ListarSistemas", async (ContextDB contexto) =>
{
    return await contexto.Systems.ToListAsync();
});

app.MapGet("ObterSistemasPorId/{id}", async (int systemid, ContextDB contexto) =>
{
    return await contexto.Systems.FirstOrDefaultAsync(p => p.Systemid == systemid);
});

app.UseSwaggerUI();
app.Run();

// Copyright (c) Duende Software. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServerHost.Pages.ServerSideSessions;

public class IndexModel : PageModel
{
    private readonly ISessionManagementService _sessionManagementService;

    public IndexModel(ISessionManagementService sessionManagementService)
    {
        _sessionManagementService = sessionManagementService;
    }

    public QueryResult<UserSession> UserSessions { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Filter { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Token { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Prev { get; set; }

    public async Task OnGet()
    {
        UserSessions = await _sessionManagementService.QuerySessionsAsync(new SessionQuery
        {
            ResultsToken = Token,
            RequestPriorResults = Prev == "true",
            DisplayName = Filter,
            SessionId = Filter,
            SubjectId = Filter,
        });
    }

    [BindProperty]
    public string SessionId { get; set; }

    public async Task<IActionResult> OnPost()
    {
        await _sessionManagementService.RemoveSessionsAsync(new RemoveSessionsContext
        {
            SessionId = SessionId,
        });
        return RedirectToPage("/ServerSideSessions/Index", new { Token, Filter, Prev });
    }
}

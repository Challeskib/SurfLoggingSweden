﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfLoggingSweden.Data;
using SurfLoggingSweden.Entities;

namespace SurfLoggingSweden.Controllers;

[Route("api/surfsessions")]
[ApiController]
public class SurfSessionController : ControllerBase
{
    private readonly DataContext _context;

    public SurfSessionController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<SurfSession>>> GetSurfSessions()
    {
        return await _context.SurfSessions.ToListAsync();
    }
}
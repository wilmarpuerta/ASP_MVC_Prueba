using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP_MVC_Prueba.Data;
using ASP_MVC_Prueba.Models;

namespace ASP_MVC_Prueba.Controllers;

public class JobsController : Controller
{
    public readonly DBContext _context;

    public JobsController(DBContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Jobs.ToListAsync());
    }

    public async Task<IActionResult> Show(int? id)
    {
        return View(await _context.Jobs.FirstOrDefaultAsync(m => m.ID == id));
    }

    public IActionResult Create()
    {
        return View();
    }

   
}
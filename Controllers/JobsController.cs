using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP_MVC_Prueba.Data;
using ASP_MVC_Prueba.Models;

namespace ASP_MVC_Prueba.Controllers;

public class JobsController : Controller
{
    public readonly DBContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public JobsController(DBContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Jobs.ToListAsync());
    }

    public async Task<IActionResult> Show(int? id)
    {
        return View(await _context.Jobs.FirstOrDefaultAsync(m => m.ID == id));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        var job = await _context.Jobs.FindAsync(id);
        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult>Create(Job j, IFormFile file, string status)
    {
        string nameFile = file.FileName;
        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath + "\\" + "img");

        if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

        string filePath = Path.Combine(uploadFolder, nameFile);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

        j.LogoCompany = nameFile;
        j.Status = status;
        _context.Jobs.Add(j);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int? id)
    {
        return View(await _context.Jobs.FirstOrDefaultAsync(m => m.ID == id));
    }

    [HttpPost]
    public async Task<IActionResult>Update(Job j, IFormFile file, string status)
    {
        string nameFile = file.FileName;
        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath + "\\" + "img");

        if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

        string filePath = Path.Combine(uploadFolder, nameFile);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

        j.LogoCompany = nameFile;
        j.Status = status;
        _context.Jobs.Update(j);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Search(string? SearchString)
    {
        var jobs = _context.Jobs.AsQueryable();

        if (!string.IsNullOrEmpty(SearchString))
        {
            jobs = jobs.Where(u => u.NameCompany.Contains(SearchString) || u.OfferTitle.Contains(SearchString) || u.Description.Contains(SearchString) || u.Status.Contains(SearchString) || u.Country.Contains(SearchString) || u.Languages.Contains(SearchString));
        }

        return View("Index", jobs.ToList());
    }
   
}
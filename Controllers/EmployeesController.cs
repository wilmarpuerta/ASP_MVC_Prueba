using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP_MVC_Prueba.Data;
using ASP_MVC_Prueba.Models;

namespace ASP_MVC_Prueba.Controllers;

public class EmployeesController : Controller
{
    public readonly DBContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public EmployeesController(DBContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Employees.ToListAsync());
    }

    public async Task<IActionResult> Show(int? id)
    {
        return View(await _context.Employees.FirstOrDefaultAsync(m => m.ID == id));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        var Emp = await _context.Employees.FindAsync(id);
        _context.Employees.Remove(Emp);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult>Create(Employe e, IFormFile img, IFormFile pdf, string Genero, string EstadoC)
    {
        string nameFileImg = img.FileName;
        string nameFileDocs = pdf.FileName;
        string uploadFolderImg = Path.Combine(_webHostEnvironment.WebRootPath + "\\" + "img");
        string uploadFolderDocs = Path.Combine(_webHostEnvironment.WebRootPath + "\\" + "docs");

        if (!Directory.Exists(uploadFolderImg))
                {
                    Directory.CreateDirectory(uploadFolderImg);
                }

        if (!Directory.Exists(uploadFolderDocs))
                {
                    Directory.CreateDirectory(uploadFolderDocs);
                }

        string filePathImg = Path.Combine(uploadFolderImg, nameFileImg);

                using (FileStream stream = new FileStream(filePathImg, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

        string filePathDocs = Path.Combine(uploadFolderDocs, nameFileDocs);

                using (FileStream stream = new FileStream(filePathDocs, FileMode.Create))
                {
                    await pdf.CopyToAsync(stream);
                }

        e.ProfilePicture = nameFileImg;
        e.Cv = nameFileDocs;
        e.CivilStatus = EstadoC;
        e.Gender = Genero;
        _context.Employees.Add(e);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int? id)
    {
        return View(await _context.Employees.FirstOrDefaultAsync(m => m.ID == id));
    }

    [HttpPost]
    public async Task<IActionResult>Update(Employe e, IFormFile img, IFormFile pdf, string Genero, string EstadoC)
    {
        string nameFileImg = img.FileName;
        string nameFileDocs = pdf.FileName;
        string uploadFolderImg = Path.Combine(_webHostEnvironment.WebRootPath + "\\" + "img");
        string uploadFolderDocs = Path.Combine(_webHostEnvironment.WebRootPath + "\\" + "docs");

        if (!Directory.Exists(uploadFolderImg))
                {
                    Directory.CreateDirectory(uploadFolderImg);
                }

        if (!Directory.Exists(uploadFolderDocs))
                {
                    Directory.CreateDirectory(uploadFolderDocs);
                }

        string filePathImg = Path.Combine(uploadFolderImg, nameFileImg);

                using (FileStream stream = new FileStream(filePathImg, FileMode.Create))
                {
                    await img.CopyToAsync(stream);
                }

        string filePathDocs = Path.Combine(uploadFolderDocs, nameFileDocs);

                using (FileStream stream = new FileStream(filePathDocs, FileMode.Create))
                {
                    await pdf.CopyToAsync(stream);
                }
        
        e.ProfilePicture = nameFileImg;
        e.Cv = nameFileDocs;
        e.CivilStatus = EstadoC;
        e.Gender = Genero;
        _context.Employees.Update(e);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Search(string? SearchString)
    {
        var emp = _context.Employees.AsQueryable();

        if (!string.IsNullOrEmpty(SearchString))
        {
            emp = emp.Where(u => u.Names.Contains(SearchString) || u.LastNames.Contains(SearchString) || u.Email.Contains(SearchString) || u.About.Contains(SearchString));
        }

        return View("Index", emp.ToList());
    }
   
}
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
    public async Task<IActionResult>Create(Employe e, IFormFile file, string Genero, string EstadoC)
    {
        string nameFile = file.FileName;
        string nameFileImg = "";
        string nameFileDocs = "";
        string uploadFolder = "";
        if (nameFile.Contains(".jpg") ||nameFile.Contains(".png") )
        {
            uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath + "\\" + "img");
            nameFileImg = file.FileName;
        }
        else if (nameFile.Contains(".pdf"))
        {
            uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath + "\\" + "docs");
            nameFileDocs = file.FileName;
        }

        if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

        string filePath = Path.Combine(uploadFolder, nameFile);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

        e.ProfilePicture = nameFileImg;
        e.Cv = nameFileDocs;
        // _context.Jobs.Add(e);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }


    // public async Task<IActionResult> Update(int? id)
    // {
    //     return View(await _context.Employees.FirstOrDefaultAsync(m => m.ID == id));
    // }

    // [HttpPost]
    // public async Task<IActionResult>Update(Employe j, IFormFile file, string status)
    // {
    //     string nameFile = file.FileName;
    //     string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath + "\\" + "img");

    //     if (!Directory.Exists(uploadFolder))
    //             {
    //                 Directory.CreateDirectory(uploadFolder);
    //             }

    //     string filePath = Path.Combine(uploadFolder, nameFile);

    //             using (FileStream stream = new FileStream(filePath, FileMode.Create))
    //             {
    //                 await file.CopyToAsync(stream);
    //             }

    //     j.LogoCompany = nameFile;
    //     j.Status = status;
    //     _context.Jobs.Update(j);
    //     _context.SaveChanges();
    //     return RedirectToAction("Index");
    // }
   
}
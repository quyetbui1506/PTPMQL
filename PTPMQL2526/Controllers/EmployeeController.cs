using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTPMQL2526.Data;
using PTPMQL2526.Models;
using PTPMQL2526.Models.Program;

namespace PTPMQL2526.Controllers;

public class EmployeeController : Controller
{
    public IActionResult privacy()
    {
        return View();
    }
    // ---------------------------------------------------------------------------------------------
    private readonly ApplicationDbContext _context;
    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }
    // index
    public async Task<IActionResult> Index()
    {
        var model = await _context.Employee.ToListAsync();
        return View(model);
    }
    // create
    public async Task<IActionResult> Create()
    {
        var lastPerson = await _context.Person.OrderByDescending(e => e.PersonID).FirstOrDefaultAsync();

        string newId = AutoCodeGenerator.GenerateNextCode(lastPerson?.PersonID);

        // gửi mã mới qua ViewBag
        ViewBag.NewPersonId = newId;

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PersonID, FullName, Address, Gender, EmployeeID, Age")] Employee employee)
    {
        var lastPerson = await _context.Person.OrderByDescending(e => e.PersonID).FirstOrDefaultAsync();

        string newId = AutoCodeGenerator.GenerateNextCode(lastPerson?.PersonID);

        // gán mã mới cho person
        employee.PersonID = newId;

        // điều kiện cho tuổi
        if (employee.Age < 0)
        {
            ModelState.AddModelError("Age", "Độ tuổi không hợp lệ");
        }

        if (employee.Age < 18)
        {
            ModelState.AddModelError("Age", "Em chưa 18");
        }
        
        // điều kiện : id employee là khác nhau
        if (_context.Employee.Any(e => e.EmployeeID == employee.EmployeeID))
        {
            ModelState.AddModelError("EmployeeID", "EmployeeID này đã tồn tại!");
            ViewBag.NewPersonId = employee.PersonID;
            return View(employee);
        }
    
        if (ModelState.IsValid)
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    
        // nếu có lỗi thì vẫn gửi lại mã để hiện trong View
        ViewBag.NewPersonId = newId;
        return View(employee);
    }
    // Edit
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null || _context.Employee == null)
        {
            return NotFound();
        }

        var employee = await _context.Employee.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("PersonID, FullName, Address, Gender, EmployeeID, Age")] Employee employee)
    {
        if (id != employee.PersonID)
        {
            return NotFound();
        }
        // điều kiện cho tuổi
        if (employee.Age < 0)
        {
            ModelState.AddModelError("Age", "Độ tuổi không hợp lệ");
        }

        if (employee.Age < 18)
        {
            ModelState.AddModelError("Age", "Em chưa 18");
        }
        
        // điều kiện : id employee là khác nhau
        if (_context.Employee.Any(e => e.EmployeeID == employee.EmployeeID))
        {
            ModelState.AddModelError("EmployeeID", "EmployeeID này đã tồn tại!");
            return View(employee);
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(employee.PersonID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(employee);
    }
    // Delete
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null || _context.Employee == null)
        {
            return NotFound();
        }
        var employee = await _context.Employee.FirstOrDefaultAsync(m => m.PersonID == id);
        if (employee == null)
        {
            return NotFound();
        }
        return View(employee);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (_context.Employee == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Employee' is null ");

        }
        var employee = await _context.Employee.FindAsync(id);
        if (employee != null)
        {
            _context.Employee.Remove(employee);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    // tạo phương thức PersonExists
    private bool PersonExists(string id)
    {
        return ( _context.Employee?.Any (e => e.PersonID == id) ).GetValueOrDefault();
    }
}
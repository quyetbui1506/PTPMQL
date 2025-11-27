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

        string newPersonId = AutoCodeGenerator.GenerateNextCode(lastPerson?.PersonID, "PS");

        // gửi mã mới qua ViewBag
        ViewBag.NewPersonId = newPersonId;

        var lastEmployee = await _context.Employee.OrderByDescending(e => e.EmployeeID).FirstOrDefaultAsync();

        // Kiểm tra xem bảng Employee có dữ liệu chưa
        string newEmployeeId;

        if (lastEmployee == null)
        {
            // Nếu chưa có nhân viên nào → bắt đầu NV001
            newEmployeeId = "NV001";
        }
        else
        {
            // Nếu đã có → tăng tiếp mã cuối
            newEmployeeId = AutoCodeGenerator.GenerateNextCode(lastEmployee.EmployeeID, "NV");
        }

        // gửi mã mới qua ViewBag
        ViewBag.NewEmployeeId = newEmployeeId;

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PersonID, FullName, Address, Gender, EmployeeID, Age")] Employee employee)
    {
        // điều kiện cho tuổi
        if (employee.Age < 0)
        {
            ModelState.AddModelError("Age", "Độ tuổi không hợp lệ");
        }

        if (employee.Age < 18)
        {
            ModelState.AddModelError("Age", "Em chưa 18");
        }

        
        if (ModelState.IsValid)
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // nếu có lỗi thì vẫn gửi lại mã để hiện trong View
        ViewBag.NewPersonId = employee.PersonID;
        ViewBag.NewEmployeeId = employee.EmployeeID;
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
        if (!ModelState.IsValid)
        {
            return View(employee);
        }

        try
        {
            _context.Update(employee);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTPMQL2526.Data;
using PTPMQL2526.Models;
using PTPMQL2526.Models.Program;

namespace PTPMQL2526.Controllers;

public class PersonController : Controller
{

    public IActionResult index1()
    {
        return View();
    }

    public IActionResult privacy()
    {
        return View();
    }
    [HttpPost]
    public IActionResult index1(Person ps)
    {
        string strOutput = "Hello " + ps.PersonID + " - " + ps.FullName + " - " + ps.Address;
        ViewBag.infoPerson = strOutput;
        return View();  
    }
    // ---------------------------------------------------------------------------------------------
    private readonly ApplicationDbContext _context;
    public PersonController(ApplicationDbContext context)
    {
        _context = context;
    }
    // index
    public async Task<IActionResult> Index()
    {
        var model = await _context.Person.ToListAsync();
        return View(model);
    }
    // create
    public async Task<IActionResult> Create()
    {
        var lastPerson = await _context.Person.OrderByDescending(p => p.PersonID).FirstOrDefaultAsync();

        string newId = AutoCodeGenerator.GenerateNextCode(lastPerson?.PersonID);

        // gửi mã mới qua ViewBag
        ViewBag.NewPersonId = newId;

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PersonID, FullName, Address")] Person person)
    {
        var lastPerson = await _context.Person.OrderByDescending(p => p.PersonID).FirstOrDefaultAsync();

        string newId = AutoCodeGenerator.GenerateNextCode(lastPerson?.PersonID);
    
        // gán mã mới cho person
        person.PersonID = newId;
    
        if (ModelState.IsValid)
        {
            _context.Add(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    
        // nếu có lỗi thì vẫn gửi lại mã để hiện trong View
        ViewBag.NewPersonId = newId;
        return View(person);
    }
    // Edit
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null || _context.Person == null)
        {
            return NotFound();
        }

        var person = await _context.Person.FindAsync(id);
        if (person == null)
        {
            return NotFound();
        }
        return View(person);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("PersonID, FullName, Address")] Person person)
    {
        if (id != person.PersonID)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(person);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(person.PersonID))
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
        return View(person);
    }
    // Delete
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null || _context.Person == null)
        {
            return NotFound();
        }
        var person = await _context.Person.FirstOrDefaultAsync(m => m.PersonID == id);
        if (person == null)
        {
            return NotFound();
        }
        return View(person);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (_context.Person == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Person' is null ");

        }
        var person = await _context.Person.FindAsync(id);
        if (person != null)
        {
            _context.Person.Remove(person);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    // tạo phương thức PersonExists
    private bool PersonExists(string id)
    {
        return ( _context.Person?.Any (e => e.PersonID == id) ).GetValueOrDefault();
    }
}
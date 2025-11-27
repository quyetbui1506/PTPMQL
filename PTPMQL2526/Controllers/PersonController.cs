using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTPMQL2526.Data;
using PTPMQL2526.Models;
using PTPMQL2526.Models.Program;
using PTPMQL2526.Models.Process;
using OfficeOpenXml;


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
    private ExcelProcess _excelProcess = new ExcelProcess();
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

        string newId = AutoCodeGenerator.GenerateNextCode(lastPerson?.PersonID, "PS");

        // gửi mã mới qua ViewBag
        ViewBag.NewPersonId = newId;

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PersonID, FullName, Address, Gender")] Person person)
    {
        if (ModelState.IsValid)
        {
            _context.Add(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

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
    public async Task<IActionResult> Edit(string id, [Bind("PersonID, FullName, Address, Gender")] Person person)
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
    // Upload 
    public async Task<IActionResult> Upload()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if(file != null)
        {
            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                ModelState.AddModelError("", "Please choose excel file to upload!");
            }
            else
            //đổi tên file khi tải lên server
            {
                var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName);
                var fileLocation = new FileInfo(filePath).ToString();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    //lưu file vào server
                    await file.CopyToAsync(stream);
                    //đọc dữ liệu và điền vào data
                    var dt = _excelProcess.ExcelToDataTable(fileLocation);
                    //sử dụng vòng lặp đọc data
                    for (int i=0; i<dt.Rows.Count; i++)
                    {
                        //tạo đối tượng Person mới
                        var ps = new Person();
                        //gán giá trị cho thuộc tính
                        ps.PersonID = dt.Rows[i][0].ToString();
                        ps.FullName = dt.Rows[i][1].ToString();
                        ps.Address = dt.Rows[i][2].ToString();
                        //thêm đối tượng
                        _context.Add(ps); 
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        return View();
    }
    //Download
    public IActionResult Download()
    {
        // Name the file when downloading
        var fileName = "YourFileName.xlsx";

        using (ExcelPackage excelPackage = new ExcelPackage())
        {
            // Add some text to cell A1
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
            worksheet.Cells["A1"].Value = "PersonID";
            worksheet.Cells["B1"].Value = "FullName";
            worksheet.Cells["C1"].Value = "Address";

            // Get all Person
            var personList = _context.Person.ToList();

            // fill data to worksheet
            worksheet.Cells["A2"].LoadFromCollection(personList);
            var stream = new MemoryStream(excelPackage.GetAsByteArray());

            // download file
            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
    }
}
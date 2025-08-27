using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PTPMQL2526.Models;

namespace PTPMQL2526.Controllers;

public class EmployeeController : Controller
{

    public IActionResult index()
    {
        return View();
    }

    public IActionResult privacy()
    {
        return View();
    }
}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PTPMQL2526.Models;

namespace PTPMQL2526.Controllers;

public class DemoController : Controller
{
    public IActionResult index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult index(string FullName, string Address)
    {
        string strOutput = "Hello " + FullName + " from " + Address;
        ViewBag.Message = strOutput;
        return View();
    }
}
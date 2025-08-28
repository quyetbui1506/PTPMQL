using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using baithuchanhso1.Models;

namespace baithuchanhso1.Controllers;

public class NameController : Controller
{

    public IActionResult index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult index(string Name, int NamSinh)
    {
        string strOutput = "họ tên " + Name + " - " + (2025 - NamSinh) + " tuổi";
        ViewBag.Message = strOutput;
        return View();
    }

}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PTPMQL2526.Models;

namespace PTPMQL2526.Controllers;

public class NameController : Controller
{

    public IActionResult index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult index(string Name, int NamSinh)
    {
        string strOutput = "Họ tên : " + Name + " - " + (2025 - NamSinh) + " tuổi";
        ViewBag.Message = strOutput;
        return View();
    }

}
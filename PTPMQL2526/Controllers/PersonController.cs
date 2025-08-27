using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PTPMQL2526.Models;

namespace PTPMQL2526.Controllers;

public class PersonController : Controller
{

    public IActionResult index()
    {
        return View();
    }

    public IActionResult privacy()
    {
        return View();
    }
    [HttpPost]
    public IActionResult index(Person ps)
    {
        string strOutput = "Hello " +ps.PersonID +" - "+ ps.FullName + " - " + ps.Address;
        ViewBag.infoPerson = strOutput;
        return View();
    }
}
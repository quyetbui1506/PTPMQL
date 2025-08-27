using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PTPMQL2526.Models;

namespace PTPMQL2526.Controllers;

public class HelloWorldController : Controller
{
    //GET: /HelloWorld/
    public IActionResult index()
    {
        return View();
    }
    //GET: /HelloWorld/Welcome/
    public string Welcome()
    {
        return "This is welcome action method...";
    }
}

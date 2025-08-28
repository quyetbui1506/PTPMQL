using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using baithuchanhso1.Models;

namespace baithuchanhso1.Controllers;

public class BMIController : Controller
{

    public IActionResult index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult index(float weight, float height)
    {
        string strOutput = "Chỉ số BMI với chỉ số cân nặng: " + weight +"(kg)"+ " và chiều cao: " + height +"(cm)"+ " là " + (weight/(height/100 *height/100));
        ViewBag.Message = strOutput;
        return View();
    }

}
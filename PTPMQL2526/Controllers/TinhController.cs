using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PTPMQL2526.Models;

namespace PTPMQL2526.Controllers;

public class TinhController : Controller
{

    public IActionResult index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(double so1, double so2, string pheptoan)
    {
        string ketQua = "";
        switch (pheptoan)
        {
            case "cong":
                ketQua = "Tổng:" + (so1 + so2);
                break;
            case "tru":
                ketQua = "Hiệu:" + (so1 - so2);
                break;
            case "nhan":
                ketQua = "Tích:" + (so1 * so2);
                break;
            case "chia":
                ketQua = "Thương:" + (so1 / so2);
                break;
        }
        ViewBag.Message = ketQua;
        return View();
    }

}
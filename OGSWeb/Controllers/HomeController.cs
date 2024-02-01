using Microsoft.AspNetCore.Mvc;
using OGSWeb.Models;
using System.Diagnostics;
using System.IO;
using SautinSoft;
using System.Reflection.PortableExecutable;
using System.Text;

namespace OGSWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UploadPdf()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadPdf(IFormFile pdfFile)
        {
            if (pdfFile != null && pdfFile.Length > 0)
            {
                try
                {                
                    var pdfname = pdfFile.FileName;
                    string pdfFilePath = Path.Combine(@"C:\Users\PC\Desktop\", pdfname);
                    var a2 = pdfFile.Headers;

                
                    PdfFocus pdfFocus = new PdfFocus();
                    pdfFocus.OpenPdf(pdfFilePath);

                    if (pdfFocus.PageCount > 0)
                    {
                        using (MemoryStream htmlStream = new MemoryStream())
                        {
                            int result = pdfFocus.ToHtml(htmlStream);

                            if (result == 0)
                            {
                                htmlStream.Seek(0, SeekOrigin.Begin);
                                using (StreamReader reader = new StreamReader(htmlStream, Encoding.UTF8))
                                {
                                    string htmlString = await reader.ReadToEndAsync();

                           
                                    return Content(htmlString, "text/html");
                                }
                            }
                        }
                    }

                    return Content("PDF dönüştürme hatası");
                }
                catch (Exception ex)
                {
                    return Content($"Hata: {ex.Message}");
                }
            }

            return Content("PDF dosyası seçilmedi");
        }


    }
}
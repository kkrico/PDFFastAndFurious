using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Image = System.Drawing.Image;

namespace PDFFastAndFurious.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult ParsePdf(HttpPostedFileBase arquivo, string texto)
        {
            using (Stream inputPdfStream = new FileStream(Server.MapPath("~/App_Data/pdf.pdf"), FileMode.Open, FileAccess.Read))
            using (var outputPdfStream = new MemoryStream())
            {
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(arquivo.InputStream);
                image.ScaleToFit(100, 100);

                var reader = new PdfReader(inputPdfStream);
                var stamper = new PdfStamper(reader, outputPdfStream);

                PdfContentByte content = stamper.GetUnderContent(1);
                image.SetAbsolutePosition(50, 50);
                content.AddImage(image);

                stamper.Close();

                reader.Close();

                var bytes = outputPdfStream.ToArray();
                string data64Data = Convert.ToBase64String(bytes);

                System.IO.File.WriteAllBytes("C:\\Users\\ramos\\Desktop\\arquivo.pdf", bytes);
                return Json(new { sucess = true, data = data64Data });

            }
        }
    }
}
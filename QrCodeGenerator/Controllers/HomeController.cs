using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QrCodeGenerator.Models;
using QRCoder;

namespace QrCodeGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QRCodeGenerator _qRCodeGenerator;

        public HomeController(ILogger<HomeController> logger, QRCodeGenerator qRCodeGenerator)
        {
            _logger = logger;
            _qRCodeGenerator = qRCodeGenerator;
        }

        public IActionResult Index(string nome,string link,string frase)
        {
            var data = new
            {
                nomeQr = nome != null ? nome : "Nome",
                linkQr = link != null ? link : "www.qualuqerlink.com.br",
                fraseQr = frase != null ? link : "Teste"
            };

            QRCodeData qrCodeData = _qRCodeGenerator.CreateQrCode(JsonConvert.SerializeObject(data), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(5);

            var qrCodeRet = new byte[7000];

            using (var memoryStream = new MemoryStream())
            {
                qrCodeImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                qrCodeRet = memoryStream.ToArray();
            }

            var result = new QrCodeViewModel { QrCodeData = qrCodeRet, Nome = nome,Link = link,Frase = frase };
            var base64 = Convert.ToBase64String(result.QrCodeData);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

            ViewBag.Nome = nome != null ? nome : "Nome";
            ViewBag.Link = link != null ? link : "www.qualuqerlink.com.br";
            ViewBag.Frase = frase != null ? frase : "Teste";
            ViewBag.ImgQrCode = imgSrc;         

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

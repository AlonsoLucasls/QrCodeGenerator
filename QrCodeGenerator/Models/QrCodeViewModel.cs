using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QrCodeGenerator.Models
{
    public class QrCodeViewModel
    {
        public string Nome { get; set; }
        public string Link { get; set; }
        public byte[] Img { get; set; }
        public string Frase { get; set; }
        public byte[] QrCodeData { get; set; }
    }
}

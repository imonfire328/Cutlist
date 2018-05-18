using System;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows;
using CLGenerator.MD;

namespace CLGenerator
{
    public class Reader
    {
        public Document Doc { get; set; }

        private const String PATH = "/Users/tamariancarpets/Desktop";
        private float Multiplier = 5;
        private MdList _list { get; set; }
        private string _fileName { get; set; } 

        public Reader(MdList list, string fileName = "c1.pdf", float size = 5)
        {
            Multiplier = size;
            _list = list;
            _fileName = fileName;
        }


        public void CreatePdf()
        {
            var fullPath = Path.Combine(PATH, _fileName);
            using (System.IO.FileStream fs = System.IO.File.Create(fullPath))
            {
                Doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(Doc, fs);
                Doc.Open();

                PdfContentByte cb = writer.DirectContent;
                Doc = _list.Write(cb, Doc);

                Doc.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ModImpresion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Report");
            ImpresionServicio mImp = new ImpresionServicio();

            /**** Reporte No. Control y Compania ****/
            mImp.NoControl = 2019907;
            mImp.Compania = 44;

            /*
            Cod_Servicio	Descripcion_Servicio
               1	        Servicio búsqueda y rescate
               2	        Servicios varios
               3	        Servicio ambulancia
               4	        Servicio incendio
            */

            Console.WriteLine("Create Report");
            string resBase64Report = mImp.GeneraPDFBase64(3 /*Cod_Servicio*/);
            string PathArchivo = System.AppDomain.CurrentDomain.BaseDirectory;
            PathArchivo = PathArchivo + string.Format("Temp\\{0}_{1}.pdf", mImp.Compania, mImp.NoControl);

            using (System.IO.FileStream stream = System.IO.File.Create(PathArchivo))
            {
                Console.WriteLine("Write to PDF");
                System.Byte[] byteArray = System.Convert.FromBase64String(resBase64Report);
                stream.Write(byteArray, 0, byteArray.Length);
            }

            if (System.IO.File.Exists(PathArchivo))
            {
                Console.WriteLine("Open PDF");
                Process process = Process.Start(PathArchivo);
                process.WaitForExit();
            }

            Console.WriteLine("End Report");
        }
    }
}

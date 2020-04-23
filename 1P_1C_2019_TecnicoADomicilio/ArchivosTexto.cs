using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _1P_1C_2019_TecnicoADomicilio
{
    class ArchivosTexto
    {
        const char Separador = ';';
        public void Leer(string nomArchivo, List<string[]> retorno)
        {
            //referencia: https://www.codeproject.com/Questions/1084390/Import-from-csv-in-csharp
            //referencia: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1?view=netframework-4.8
            //la ruta comienza con ..\..\..\ para subir de la ruta del assembly (bin\debug)
            string ruta = @"..\..\..\";
            string path = ruta + nomArchivo;
            string[] campos;
            // Open the file to read from.
            using (StreamReader sr = File.OpenText(path))
            {
                string s;
                
                while (!sr.EndOfStream)
                {
                    s = sr.ReadLine();
                    campos =s.Split(Separador);
                    retorno.Add(campos);
                }
            }
        }

        public void Escribir(string nomArchivo, string contenido)
        {
            //referencia: https://docs.microsoft.com/en-us/dotnet/api/system.io.file?view=netframework-4.8
            //la ruta comienza con ..\..\..\ para subir de la ruta del assembly (bin\debug)
            string ruta = @"..\..\..\";
            string path = ruta + nomArchivo;
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write(contenido);
            }
        }

        public void Agregar(string nomArchivo, string contenido)
        {
            //referencia: https://docs.microsoft.com/en-us/dotnet/api/system.io.file?view=netframework-4.8
            //la ruta comienza con ..\..\..\ para subir de la ruta del assembly (bin\debug)
            string ruta = @"..\..\..\";
            string path = ruta + nomArchivo;
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.Write(contenido);
            }
        }
    }
}

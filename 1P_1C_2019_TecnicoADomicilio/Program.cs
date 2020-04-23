using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1P_1C_2019_TecnicoADomicilio
{
    class Program
    {
        static void Main(string[] args)
        {
            const int cantidad = 100;

            string opcion = "";
            ListaVisitas listaVis = new ListaVisitas(cantidad);
            listaVis.CargaInicial();
            do
            {
                listaVis.CompletarVisita();
                opcion = ServValidac.PedirSoN("¿Desea continuar? S/N");
            } while (opcion == "S");
            listaVis.GuardadoFinal();
        }
    }

    class Visita
    {
        public const string EstPend = "P";
        public const string EstVisi = "V";
        public const long NumMin = 1;
        public const long NumMax = 999999;
        public long Numero { get; private set; }
        public string Domicilio { get; set; }
        public string NombreCliente { get; set; }
        public string Sintoma { get; set; }
        public string CausaSolucion { get; private set; }
        public string Estado { get; private set; }

        public Visita(long numero, string direccion, string cliente, string problema)
        {
            //si numero es 0, establece Numero = 0 y el resto de los atributos como ""
            //caso contrario, toma los datos recibidos e inicia el 
            if (numero == 0)
            {
                Numero = 0;
                Domicilio = "";
                NombreCliente = "";
                Sintoma = "";
                Estado = "";
            }
            else
            {
                Numero = numero;
                Domicilio = direccion.ToUpper();
                NombreCliente = cliente.ToUpper();
                Sintoma = problema.ToUpper();
                Estado = EstPend;
            }
        }

        public void Reportar(string causaSoluc)
        {
            CausaSolucion = causaSoluc;
            Estado = Visita.EstVisi;
        }

        public string Resumir()
        {
            return Numero + "\t"
                   + Domicilio + "\t"
                   + NombreCliente + "\t"
                   + Sintoma + "\n";
        }
    }

    class ListaVisitas
    {
        //constantes para el manejo de archivos - no van en el enunciado - inicio
        public const char Separador = ';';
        public const string NomArchVPend = "visitas_pend.csv";
        public const string NomArchVVisi = "visitas_visi.csv";
        //constantes para el manejo de archivos - no van en el enunciado - fin
        private Visita[] visitas;

        public ListaVisitas(int cantidad)
        {
            //establece el tamaño del arreglo visitas según la "cantidad"
            //recibida, e inicializa todas las posiciones como vacías
            int vis = 0;
            if (cantidad >= 1)
            {
                visitas = new Visita[cantidad];
                //inicializa todos los elementos 
                while (vis <= visitas.GetUpperBound(0))
                {
                    visitas[vis] = new Visita(0, "", "", "");
                    vis++;
                }
            }
        }

        public void CargaInicial()
        {
            //carga las visitas pendientes del archivo
            //"visitas_pend.csv" en el arreglo visitas
            ArchivosTexto archivo = new ArchivosTexto();
            List<string[]> retorno = new List<string[]>();
            int fila = 0;
            long numVisita = 0;

            archivo.Leer(NomArchVPend, retorno);
            foreach(string[] reg in retorno)
            {
                long.TryParse(reg[0], out numVisita);
                visitas[fila] = new Visita(numVisita, reg[1], reg[2], reg[3]);
                fila++;
            }
        }

        public void GuardadoFinal()
        {
            //guarda las visitas pendientes del arreglo visitas
            //en el archivo "visitas_pend.csv", y las visitadas
            //en el archivo "visitas_visi.csv"
            ArchivosTexto archivo = new ArchivosTexto();
            string registro = "";
            string listaPend = "";
            string listaVisi = "";
            int fila = 0;

            while (fila <= visitas.GetUpperBound(0))
            {
                if (visitas[fila].Estado == "")
                {
                    fila = visitas.GetUpperBound(0) + 1;
                }
                else
                {
                    registro = visitas[fila].Numero.ToString() + Separador
                            + visitas[fila].Domicilio + Separador
                            + visitas[fila].NombreCliente + Separador
                            + visitas[fila].Sintoma + Separador
                            + visitas[fila].CausaSolucion + Separador
                            + visitas[fila].Estado
                            + "\n";
                    if (visitas[fila].Estado == Visita.EstVisi)
                    {
                        listaVisi = listaVisi + registro;
                    } else
                    {
                        listaPend = listaPend + registro;
                    }
                    fila++;
                }
            }

            archivo.Escribir(NomArchVPend, listaPend);
            archivo.Agregar(NomArchVVisi, listaVisi);
        }

        //SOLUCION - INICIO

        public void CompletarVisita()
        {
            String listadoDeVisitasPendientes;
            long numeroDeVisita;
            int posicionDeVisita;
            String causa;
            String completarVisita;

            //Armar una lista de las visitas pendientes
            listadoDeVisitasPendientes = armarListadoVisitasPendientes();
            //Si no hay ninguna, mostrar mensaje de error
            if (listadoDeVisitasPendientes == "")
            {
                Console.WriteLine("No hay visitas pendientes");
            }
            else
            {
                listadoDeVisitasPendientes = "Ingrese numero de vista:\nNúmero\tDomicilio\tCliente\tProblema\n" + listadoDeVisitasPendientes;
                numeroDeVisita = ServValidac.PedirLong(listadoDeVisitasPendientes, Visita.NumMin, Visita.NumMax);
                posicionDeVisita = buscarVisitaPorNumero(numeroDeVisita);
                if (posicionDeVisita == -1)
                {
                    Console.WriteLine("No hay una visita con el número ingresado");
                }
                else
                {
                    if (visitas[posicionDeVisita].Estado != Visita.EstPend)
                    {
                        Console.WriteLine("La visita no está pendiente");
                    }
                    else
                    {

                    }
                }
            }
        }

        private String armarListadoVisitasPendientes() {
            String valorARetornar = "";
            for (int i = 0; i < this.visitas.Length; i++) {
                if (this.visitas[i].Estado == Visita.EstPend) {
                    valorARetornar = valorARetornar + this.visitas[i].Resumir();
                }
            }
            return (valorARetornar);
        }

        private int buscarVisitaPorNumero(long numeroABuscar) {
            int valorADevolver = 0;

            do
            {
                valorADevolver = valorADevolver + 1;
            } while (this.visitas[valorADevolver].Numero != numeroABuscar && valorADevolver < this.visitas.Length);

            if (this.visitas[valorADevolver].Numero != numeroABuscar) {
                valorADevolver = -1;
            }

            return (valorADevolver);
        }





        //SOLUCION - FIN
    }

    static class ServValidac
    {
        public static string PedirStrNoVac(string mensaje)
        {
            string valor;
            do
            {
                Console.WriteLine(mensaje);
                valor = Console.ReadLine().ToUpper();
                if (valor == "")
                {
                    Console.WriteLine("No puede ser vacío");
                }
            } while (valor == "");

            return valor;
        }
        public static string PedirSoN(string mensaje)
        {
            string valor = "";
            string mensError = "Debe ingresar S o N";
            do
            {
                valor = PedirStrNoVac(mensaje);
                if (valor != "S" && valor != "N")
                {
                    Console.WriteLine(mensError);
                }
            } while (valor != "S" && valor != "N");

            return valor;
        }

        public static long PedirLong(string mensaje, long min, long max)
        {
            long valor;
            bool valido = false;
            string mensError = "Debe ingresar un valor entre " + min + " y " + max;

            do
            {
                Console.WriteLine(mensaje);
                if (!long.TryParse(Console.ReadLine(), out valor))
                {
                    Console.WriteLine(mensError);
                }
                else
                {
                    if (valor < min || valor > max)
                    {
                        Console.WriteLine(mensError);
                    }
                    else
                    {
                        valido = true;
                    }
                }
            } while (!valido);

            return valor;
        }
    }
}

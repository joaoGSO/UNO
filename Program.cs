using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    class Program
    {
        static void Main(string[] args)
        {
            Partida partida = new Partida(6);

            partida.PlayGame();

            Console.ReadKey();
        }
    }
}

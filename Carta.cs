using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    public class Carta
    {
        public CorCarta Cor { get; set; }
        public ValorCarta Valor { get; set; }

        public string MostrarValor
        {
            get
            {
                if (Valor == ValorCarta.Coringa)
                {
                    return Valor.ToString();
                }
                return Valor.ToString() + " " + Cor.ToString();
            }
        }
    }
}

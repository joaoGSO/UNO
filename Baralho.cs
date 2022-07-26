using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    public class Baralho
    {
        public List<Carta> Cartas { get; set; }

        public Baralho()
        {
            Cartas = new List<Carta>();

            foreach (CorCarta cor in Enum.GetValues(typeof(CorCarta)))
            {
                if (cor != CorCarta.Coringa)
                {
                    foreach (ValorCarta val in Enum.GetValues(typeof(ValorCarta)))
                    {
                        switch (val)
                        {
                            case ValorCarta.Um:
                            case ValorCarta.Dois:
                            case ValorCarta.Tres:
                            case ValorCarta.Quatro:
                            case ValorCarta.Cinco:
                            case ValorCarta.Seis:
                            case ValorCarta.Sete:
                            case ValorCarta.Oito:
                            case ValorCarta.Nove:
                            case ValorCarta.Bloquear:
                            case ValorCarta.Inverter:
                            case ValorCarta.CompraDuas:

                                Cartas.Add(new Carta()
                                {
                                    Cor = cor,
                                    Valor = val
                                });
                                Cartas.Add(new Carta()
                                {
                                    Cor = cor,
                                    Valor = val
                                });
                                break;


                            case ValorCarta.Zero:
                                Cartas.Add(new Carta()
                                {
                                    Cor = cor,
                                    Valor = val
                                });
                                break;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        Cartas.Add(new Carta()
                        {
                            Cor = cor,
                            Valor = ValorCarta.Coringa
                        });
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        Cartas.Add(new Carta()
                        {
                            Cor = cor,
                            Valor = ValorCarta.CompraQuatro
                        });
                    }
                }
            }
        }

        public Baralho(List<Carta> cartas)
        {
            Cartas = cartas;
        }

        public void Embaralhar()
        {
            Random r = new Random();

            List<Carta> cartas = Cartas;

            for (int n = cartas.Count - 1; n > 0; --n)
            {
                int k = r.Next(n + 1);
                Carta temp = cartas[n];
                cartas[n] = cartas[k];
                cartas[k] = temp;
            }
        }

        public List<Carta> Comprar(int count)
        {
            var compraCartas = Cartas.Take(count).ToList();
            Cartas.RemoveAll(x => compraCartas.Contains(x));
            return compraCartas;
        }
    }
}



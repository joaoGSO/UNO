using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    public class Partida
    {
        public List<Jogador> Jogadores { get; set; }
        public Baralho PilhaCompra { get; set; }
        public List<Carta> PilhaDescarte { get; set; }

        public Partida(int numJogadores)
        {
            Jogadores = new List<Jogador>();
            PilhaCompra = new Baralho();
            PilhaCompra.Embaralhar();

            for (int i = 1; i <= numJogadores; i++)
            {
                Jogadores.Add(new Jogador()
                {
                    Posicao = i
                });
            }

            int maxCartas = 7 * Jogadores.Count;
            int distribuidas = 0;

            while (distribuidas < maxCartas)
            {
                for (int i = 0; i < numJogadores; i++)
                {
                    Jogadores[i].Mao.Add(PilhaCompra.Cartas.First());
                    PilhaCompra.Cartas.RemoveAt(0);
                    distribuidas++;
                }
            }

            PilhaDescarte = new List<Carta>();
            PilhaDescarte.Add(PilhaCompra.Cartas.First());
            PilhaCompra.Cartas.RemoveAt(0);

            while (PilhaDescarte.First().Valor == ValorCarta.Coringa || PilhaDescarte.First().Valor == ValorCarta.CompraQuatro)
            {
                PilhaDescarte.Insert(0, PilhaCompra.Cartas.First());
                PilhaCompra.Cartas.RemoveAt(0);
            }
        }

        public void PlayGame()
        {
            int i = 0;
            bool sentHorario = true;

            foreach (var jogador in Jogadores)
            {
                jogador.MostraMao();
            }

            Console.ReadLine();

            TurnoJogador turnoAtual = new TurnoJogador()
            {
                Resultado = ResultadoTurno.GameStart,
                Carta = PilhaDescarte.First(),
                CorAtual = PilhaDescarte.First().Cor
            };

            Console.WriteLine("Valor da primeira carta é: " + turnoAtual.Carta.MostrarValor + ".");
            Console.ReadKey();

            while (!Jogadores.Any(x => !x.Mao.Any()))
            {
                if (PilhaCompra.Cartas.Count < 4)
                {
                    var cartaAtual = PilhaDescarte.First();

                    //Pega as descartadas e embaralha, criando uma nova pilha de compra.
                    PilhaCompra.Cartas = PilhaDescarte.Skip(1).ToList();
                    PilhaCompra.Embaralhar();

                    //Deixa apenas a carta atual na pilha de descarte.
                    PilhaDescarte = new List<Carta>();
                    PilhaDescarte.Add(cartaAtual);

                    Console.WriteLine("Embaralhando!");
                }

                var jogadorAtual = Jogadores[i];

                turnoAtual = Jogadores[i].TurnoJogada(turnoAtual, PilhaCompra);

                Jogadores[i].MostraMao();

                AddParaPilhaDescarte(turnoAtual);

                if (turnoAtual.Resultado == ResultadoTurno.Inverter)
                {
                    sentHorario = !sentHorario;
                }

                if (sentHorario)
                {
                    i++;
                    if (i >= Jogadores.Count) //Reseta o contador de jogadores
                    {
                        i = 0;
                    }
                }
                else
                {
                    i--;
                    if (i < 0)
                    {
                        i = Jogadores.Count - 1;
                    }
                }
            }

            var jogadorVencedor = Jogadores.Where(x => !x.Mao.Any()).First();
            Console.WriteLine("Jogador " + jogadorVencedor.Posicao.ToString() + " Venceu!");
            Console.ReadKey();
        }

        private void AddParaPilhaDescarte(TurnoJogador turnoAtual)
        {
            if (turnoAtual.Resultado == ResultadoTurno.CartaJogada
                    || turnoAtual.Resultado == ResultadoTurno.CompraDuas
                    || turnoAtual.Resultado == ResultadoTurno.Bloquear
                    || turnoAtual.Resultado == ResultadoTurno.Coringa
                    || turnoAtual.Resultado == ResultadoTurno.CoringaCompraQuatro
                    || turnoAtual.Resultado == ResultadoTurno.Inverter)
            {
                PilhaDescarte.Insert(0, turnoAtual.Carta);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    public class Jogador
    {
        public List<Carta> Mao { get; set; }
        public int Posicao { get; set; }

        public Jogador()
        {
            Mao = new List<Carta>();
        }

        public TurnoJogador TurnoJogada(TurnoJogador turnoAnterior, Baralho pilhaCompra)
        {
            TurnoJogador turno = new TurnoJogador();
            if (turnoAnterior.Resultado == ResultadoTurno.Bloquear
                || turnoAnterior.Resultado == ResultadoTurno.CompraDuas
                || turnoAnterior.Resultado == ResultadoTurno.CoringaCompraQuatro)
            {
                return ProcessaAtaque(turnoAnterior.Carta, pilhaCompra);
            }
            else if ((turnoAnterior.Resultado == ResultadoTurno.Coringa
                        || turnoAnterior.Resultado == ResultadoTurno.Atacado
                        || turnoAnterior.Resultado == ResultadoTurno.CompraForcada)
                        && Compativel(turnoAnterior.CorAtual))
            {
                turno = JogadaCartaCompativel(turnoAnterior.CorAtual);
            }
            else if (Compativel(turnoAnterior.Carta))
            {
                turno = JogadaCartaCompativel(turnoAnterior.Carta);
            }
            else //Compra uma carta e checa se ela pode ser jogada
            {
                turno = CompraCarta(turnoAnterior, pilhaCompra);
            }

            MostraTurno(turno);
            return turno;
        }

        private TurnoJogador CompraCarta(TurnoJogador turnoAnterior, Baralho pilhaCompra)
        {
            TurnoJogador turno = new TurnoJogador();
            var cartaComprada = pilhaCompra.Comprar(1);
            Mao.AddRange(cartaComprada);

            if (Compativel(turnoAnterior.Carta))
            {
                turno = JogadaCartaCompativel(turnoAnterior.Carta);
                turno.Resultado = ResultadoTurno.CompraJogada;
            }
            else
            {
                turno.Resultado = ResultadoTurno.CompraForcada;
                turno.Carta = turnoAnterior.Carta;
            }

            return turno;
        }

        private void MostraTurno(TurnoJogador turnoAtual)
        {
            if (turnoAtual.Resultado == ResultadoTurno.CompraForcada)
            {
                Console.WriteLine("Jogador " + Posicao.ToString() + " precisou comprar uma carta.");
                Console.ReadKey();
            }
            if (turnoAtual.Resultado == ResultadoTurno.CompraJogada)
            {
                Console.WriteLine("Jogador " + Posicao.ToString() + " precisou comprar uma carta e pode jogar ela!");
                Console.ReadKey();
            }

            if (turnoAtual.Resultado == ResultadoTurno.CartaJogada
                || turnoAtual.Resultado == ResultadoTurno.Bloquear
                || turnoAtual.Resultado == ResultadoTurno.CompraDuas
                || turnoAtual.Resultado == ResultadoTurno.Coringa
                || turnoAtual.Resultado == ResultadoTurno.CoringaCompraQuatro
                || turnoAtual.Resultado == ResultadoTurno.Inverter
                || turnoAtual.Resultado == ResultadoTurno.CompraJogada)
            {
                Console.WriteLine("Jogador " + Posicao.ToString() + " jogou uma carta: " + turnoAtual.Carta.MostrarValor + ".");
                Console.ReadKey();
                if (turnoAtual.Carta.Cor == CorCarta.Coringa)
                {
                    Console.WriteLine("Jogador " + Posicao.ToString() + " escolheu " + turnoAtual.CorAtual.ToString() + " como a cor atual.");
                    Console.ReadKey();
                }
                if (turnoAtual.Resultado == ResultadoTurno.Inverter)
                {
                    Console.WriteLine("Ordem de jogada invertida!");
                    Console.ReadKey();
                }
            }

            if (Mao.Count == 1)
            {
                Console.WriteLine("Jogador " + Posicao.ToString() + " gritou UNO!");
                Console.ReadKey();
            }
        }

        private TurnoJogador ProcessaAtaque(Carta descarteAtual, Baralho pilhaCompra)
        {
            TurnoJogador turno = new TurnoJogador();
            turno.Resultado = ResultadoTurno.Atacado;
            turno.Carta = descarteAtual;
            turno.CorAtual = descarteAtual.Cor;
            if (descarteAtual.Valor == ValorCarta.Bloquear)
            {
                Console.WriteLine("Jogador " + Posicao.ToString() + " foi bloqueado!");
                Console.ReadKey();
                return turno;
            }
            else if (descarteAtual.Valor == ValorCarta.CompraDuas)
            {
                Console.WriteLine("Jogador " + Posicao.ToString() + " precisa comprar duas cartas!");
                Console.ReadKey();
                Mao.AddRange(pilhaCompra.Comprar(2));
            }
            else if (descarteAtual.Valor == ValorCarta.CompraQuatro)
            {
                Console.WriteLine("Jogador " + Posicao.ToString() + " precisa comprar quatro cartas!");
                Console.ReadKey();
                Mao.AddRange(pilhaCompra.Comprar(4));
            }

            return turno;
        }

        private bool Compativel(Carta carta)
        {
            return Mao.Any(x => x.Cor == carta.Cor || x.Valor == carta.Valor || x.Cor == CorCarta.Coringa);
        }

        private bool Compativel(CorCarta cor)
        {
            return Mao.Any(x => x.Cor == cor || x.Cor == CorCarta.Coringa);
        }

        //Checa compatibilidade pela cor da carta.
        private TurnoJogador JogadaCartaCompativel(CorCarta cor)
        {
            var turno = new TurnoJogador();
            turno.Resultado = ResultadoTurno.CartaJogada;
            var compatibilidade = Mao.Where(x => x.Cor == cor || x.Cor == CorCarta.Coringa).ToList();

            //Só se pode jogar um coringa +4 se não existirem outras opções.
            if (compatibilidade.All(x => x.Valor == ValorCarta.CompraQuatro))
            {
                turno.Carta = compatibilidade.First();
                turno.CorAtual = SelecionaCorDominante();
                turno.Resultado = ResultadoTurno.Coringa;
                Mao.Remove(compatibilidade.First());

                return turno;
            }

            //Assim, é jogada uma carta que possa atrapalhar o adversário.
            if (compatibilidade.Any(x => x.Valor == ValorCarta.CompraDuas))
            {
                turno.Carta = compatibilidade.First(x => x.Valor == ValorCarta.CompraDuas);
                turno.Resultado = ResultadoTurno.CompraDuas;
                turno.CorAtual = turno.Carta.Cor;
                Mao.Remove(turno.Carta);

                return turno;
            }

            if (compatibilidade.Any(x => x.Valor == ValorCarta.Bloquear))
            {
                turno.Carta = compatibilidade.First(x => x.Valor == ValorCarta.Bloquear);
                turno.Resultado = ResultadoTurno.Bloquear;
                turno.CorAtual = turno.Carta.Cor;
                Mao.Remove(turno.Carta);

                return turno;
            }

            if (compatibilidade.Any(x => x.Valor == ValorCarta.Inverter))
            {
                turno.Carta = compatibilidade.First(x => x.Valor == ValorCarta.Inverter);
                turno.Resultado = ResultadoTurno.Inverter;
                turno.CorAtual = turno.Carta.Cor;
                Mao.Remove(turno.Carta);

                return turno;
            }

            var corCompativel = compatibilidade.Where(x => x.Cor == cor);
            if (corCompativel.Any())
            {
                turno.Carta = corCompativel.First();
                turno.CorAtual = turno.Carta.Cor;
                Mao.Remove(corCompativel.First());

                return turno;
            }

            if (compatibilidade.Any(x => x.Valor == ValorCarta.Coringa))
            {
                turno.Carta = compatibilidade.First(x => x.Valor == ValorCarta.Coringa);
                turno.CorAtual = SelecionaCorDominante();
                turno.Resultado = ResultadoTurno.Coringa;
                Mao.Remove(turno.Carta);

                return turno;
            }

            turno.Resultado = ResultadoTurno.CompraForcada;
            return turno;
        }

        //Checa compatibilidade pelo valor da carta.
        private TurnoJogador JogadaCartaCompativel(Carta descarteAtual)
        {
            var turno = new TurnoJogador();
            turno.Resultado = ResultadoTurno.CartaJogada;
            var compatibilidade = Mao.Where(x => x.Cor == descarteAtual.Cor || x.Valor == descarteAtual.Valor || x.Cor == CorCarta.Coringa).ToList();

            //Só se pode jogar um coringa +4 se não existirem outras opções.
            if (compatibilidade.All(x => x.Valor == ValorCarta.CompraQuatro))
            {
                turno.Carta = compatibilidade.First();
                turno.CorAtual = SelecionaCorDominante();
                turno.Resultado = ResultadoTurno.Coringa;
                Mao.Remove(compatibilidade.First());

                return turno;
            }

            //Assim, é jogada uma carta que possa atrapalhar o adversário.
            if (compatibilidade.Any(x => x.Valor == ValorCarta.CompraDuas))
            {
                turno.Carta = compatibilidade.First(x => x.Valor == ValorCarta.CompraDuas);
                turno.Resultado = ResultadoTurno.CompraDuas;
                turno.CorAtual = turno.Carta.Cor;
                Mao.Remove(turno.Carta);

                return turno;
            }

            if (compatibilidade.Any(x => x.Valor == ValorCarta.Bloquear))
            {
                turno.Carta = compatibilidade.First(x => x.Valor == ValorCarta.Bloquear);
                turno.Resultado = ResultadoTurno.Bloquear;
                turno.CorAtual = turno.Carta.Cor;
                Mao.Remove(turno.Carta);

                return turno;
            }

            if (compatibilidade.Any(x => x.Valor == ValorCarta.Inverter))
            {
                turno.Carta = compatibilidade.First(x => x.Valor == ValorCarta.Inverter);
                turno.Resultado = ResultadoTurno.Inverter;
                turno.CorAtual = turno.Carta.Cor;
                Mao.Remove(turno.Carta);

                return turno;
            }

            //Nesse ponto, o jogador tem uma escolha
            //Se o jogador tem cartas compativeis a pilha de descarte tanto em cor quanto em valor, ele pode escolher qual jogar
            //Nesse caso, assumiremos que o jogador deseja jogar o máximo de cartas compativeis entre si primeiro.

            var corCompativel = compatibilidade.Where(x => x.Cor == descarteAtual.Cor);
            var valorCompativel = compatibilidade.Where(x => x.Valor == descarteAtual.Valor);
            if (corCompativel.Any() && valorCompativel.Any())
            {
                var corCorrespondente = Mao.Where(x => x.Cor == corCompativel.First().Cor);
                var valorCorrespondente = Mao.Where(x => x.Valor == valorCompativel.First().Valor);
                if (corCorrespondente.Count() >= valorCorrespondente.Count())
                {
                    turno.Carta = corCompativel.First();
                    turno.CorAtual = turno.Carta.Cor;
                    Mao.Remove(corCompativel.First());

                    return turno;
                }
                else //Corresponde ao valor
                {
                    turno.Carta = valorCompativel.First();
                    turno.CorAtual = turno.Carta.Cor;
                    Mao.Remove(valorCompativel.First());

                    return turno;
                }
                //Descobre qual seria a melhor opção
            }
            else if (corCompativel.Any())
            {
                turno.Carta = corCompativel.First();
                turno.CorAtual = turno.Carta.Cor;
                Mao.Remove(corCompativel.First());

                return turno;
            }
            else if (valorCompativel.Any())
            {
                turno.Carta = valorCompativel.First();
                turno.CorAtual = turno.Carta.Cor;
                Mao.Remove(valorCompativel.First());

                return turno;
            }

            //Joga as cartas coringa comuns por último. Se a carta coringa comum for a última, se ganha a partida no próximo turno.
            if (compatibilidade.Any(x => x.Valor == ValorCarta.Coringa))
            {
                turno.Carta = compatibilidade.First(x => x.Valor == ValorCarta.Coringa);
                turno.CorAtual = SelecionaCorDominante();
                turno.Resultado = ResultadoTurno.Coringa;
                Mao.Remove(turno.Carta);

                return turno;
            }

            turno.Resultado = ResultadoTurno.CompraForcada;
            return turno;
        }

        private CorCarta SelecionaCorDominante()
        {
            if (!Mao.Any())
            {
                return CorCarta.Coringa;
            }
            var cores = Mao.GroupBy(x => x.Cor).OrderByDescending(x => x.Count());
            return cores.First().First().Cor;
        }

        private void OrdenaMao()
        {
            this.Mao = this.Mao.OrderBy(x => x.Cor).ThenBy(x => x.Valor).ToList();
        }

        public void MostraMao()
        {
            OrdenaMao();
            Console.WriteLine("Mao do jogador " + Posicao + ": ");
            foreach (var carta in Mao)
            {
                Console.Write(Enum.GetName(typeof(ValorCarta), carta.Valor) + "  " + Enum.GetName(typeof(CorCarta), carta.Cor) + " - ");
            }
            Console.WriteLine("");
            Console.ReadKey();
        }
    }
}

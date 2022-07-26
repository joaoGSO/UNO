using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNO
{
    public enum CorCarta
    {
        Vermelho,
        Azul,
        Amarelo,
        Verde,
        Coringa
    }

    public enum ValorCarta
    {
        Zero,
        Um,
        Dois,
        Tres,
        Quatro,
        Cinco,
        Seis,
        Sete,
        Oito,
        Nove,
        Inverter,
        Bloquear,
        CompraDuas,
        CompraQuatro,
        Coringa
    }

    public enum ResultadoTurno
    {
        //Inicio da partida.
        GameStart,

        //Quando jogada uma carta comum.
        CartaJogada,

        //Quando jogada uma carta de bloqueio.
        Bloquear,

        //Quando jogada uma carta +2.
        CompraDuas,

        //Quando jogador é forçado a comprar cartas por causa de outro jogador.
        Atacado,

        //Quando o jogador é forçado a comprar cartas por não conseguir descartar.
        CompraForcada,

        //Quando o jogador é forçado a comprar cartas por não conseguir descartar, mas a carta comprada pode ser jogada.
        CompraJogada,

        //Quando jogada uma carta coringa comum.
        Coringa,

        //Quando jogada uma carta coringa +4.
        CoringaCompraQuatro,

        //Quando jogada uma carta de inverter.
        Inverter
    }
}
using System;
using System.Threading;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes
{
    public class MissaoPantanoSombrio : MissaoBase
    {
        private bool bruxaDerrotada = false;

        public MissaoPantanoSombrio(Personagem jogador)
            : base("O Pântano Sombrio", "Pantano Maldito de Grelth", 600, 1200, jogador)
        {
            Descricao = "Você chegou ao Pântano Sombrio, um lugar cheio de névoa e criaturas perigosas. Uma bruxa poderosa habita o local.";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            DigitarTexto("💬 Narrador: O pântano se estende à sua frente. A névoa é densa e sons estranhos ecoam ao longe.", 40);
            Thread.Sleep(3000);
            ExecutarObjetivos(jogador);

            if (VerificarConclusao())
                CompletarMissao(jogador);
            else
                Console.WriteLine("\n❌ Missão falhou! Tente novamente.");
        }

        protected override void ExecutarObjetivos(Personagem jogador)
        {
            bool missaoAbandonada = false;

            while (!bruxaDerrotada && !missaoAbandonada)
            {
                DigitarTexto("\n🕵️ Onde você quer ir?", 30);
                DigitarTexto("1. Seguir a trilha de lírios negros", 20);
                DigitarTexto("2. Entrar na cabana da bruxa", 20);
                DigitarTexto("0. Abandonar a missão", 20);

                Console.Write("\nDigite sua escolha (1, 2 ou 0): ");
                string escolha = Console.ReadLine();

                if (jogador.Nivel < 12)
                {
                    DigitarTexto("\n⚠️ Você ainda não está forte o suficiente para enfrentar a bruxa.", 40);
                    return;
                }

                if (escolha == "1")
                {
                    DigitarTexto("\nA trilha leva você a um lago escuro e pegajoso, mas nada de ameaçador surge. Você volta para o início.", 40);
                }
                else if (escolha == "2")
                {
                    DigitarTexto("\nVocê entra na cabana e encontra a Bruxa do Pântano! Ela está preparando poções perigosas.", 40);

                    Personagem bruxa = PersonagemFactory.Criar(TipoPersonagem.OgroDoPantano, "Bruxa do Pântano");

                    Combate combate = new Combate(jogador, bruxa);
                    combate.Iniciar();

                    if (!bruxa.EstaVivo)
                    {
                        DigitarTexto($"\n🎉 Você derrotou a {bruxa.Nome}!", 40);
                        bruxaDerrotada = true;
                    }
                    else
                    {
                        DigitarTexto($"\n💀 Você foi derrotado pela {bruxa.Nome}.", 40);
                        return;
                    }
                }
                else if (escolha == "0")
                {
                    DigitarTexto("\nVocê decide que o pântano é perigoso demais e retorna à cidade.", 40);
                    missaoAbandonada = true;
                }
                else
                {
                    DigitarTexto("\nEscolha inválida. Digite 1, 2 ou 0.", 40);
                }
            }
        }

        protected override bool VerificarConclusao()
        {
            return bruxaDerrotada;
        }

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            int aumentoDefesa = 30;
            DigitarTexto("\n🎉 Você encontra o Cetro Místico! Sua defesa aumentou.", 40);
            jogador.Defesa += aumentoDefesa;
            jogador.Nivel += 5;
        }
    }
}

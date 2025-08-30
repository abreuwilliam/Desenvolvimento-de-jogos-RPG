using System;
using System.Threading;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes
{
    public class MissaoMarProfundo : MissaoBase
    {
        private bool krakenDerrotado = false;

        public MissaoMarProfundo(Personagem jogador)
            : base("O Mar Profundo", "Abismo Oceânico de Thalassor", 800, 1500, jogador)
        {
            Descricao = "Você mergulhou no Mar Profundo. Lendas falam de um Kraken gigante que guarda tesouros submarinos.";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            DigitarTexto("💬 Narrador: As águas são escuras e frias. O silêncio é perturbador, interrompido apenas pelo som das ondas.", 40);
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

            while (!krakenDerrotado && !missaoAbandonada)
            {
                DigitarTexto("\n🕵️ O que você quer fazer?", 30);
                DigitarTexto("1. Explorar o navio naufragado", 20);
                DigitarTexto("2. Mergulhar até o covil do Kraken", 20);
                DigitarTexto("0. Abandonar a missão", 20);

                Console.Write("\nDigite sua escolha (1, 2 ou 0): ");
                string escolha = Console.ReadLine();

                if (jogador.Nivel < 15)
                {
                    DigitarTexto("\n⚠️ Você não está preparado para enfrentar o Kraken ainda.", 40);
                    return;
                }

                if (escolha == "1")
                {
                    DigitarTexto("\nVocê encontra alguns tesouros menores, mas o Kraken ainda não aparece.", 40);
                }
                else if (escolha == "2")
                {
                    DigitarTexto("\nO Kraken surge do abismo! Seus tentáculos gigantes bloqueiam o caminho.", 40);

                    Personagem kraken = PersonagemFactory.Criar(TipoPersonagem.PovoKraken, "Kraken do Mar Profundo");

                    Combate combate = new Combate(jogador, kraken);
                    combate.Iniciar();

                    if (!kraken.EstaVivo)
                    {
                        DigitarTexto($"\n🎉 Você derrotou o {kraken.Nome}!", 40);
                        krakenDerrotado = true;
                    }
                    else
                    {
                        DigitarTexto($"\n💀 Você foi derrotado pelo {kraken.Nome}.", 40);
                        return;
                    }
                }
                else if (escolha == "0")
                {
                    DigitarTexto("\nVocê decide voltar à superfície em segurança.", 40);
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
            return krakenDerrotado;
        }

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            int aumentoDefesa = 60;
            DigitarTexto("\n🎉 Você encontra a Armadura do Mar! Sua defesa aumentou.", 40);
            jogador.Defesa += aumentoDefesa;
            jogador.Nivel += 5;
        }
    }
}

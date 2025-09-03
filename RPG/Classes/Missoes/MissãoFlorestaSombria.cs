using System;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes
{
    public class MissaoFlorestaSombria : MissaoBase
    {
        private bool aranhaDerrotada = false;

        public MissaoFlorestaSombria(Personagem jogador)
            : base("A Floresta Sombria", "Profundezas da Floresta de Eldor", 400, 900, jogador)
        {
            Descricao = "A floresta sombria é temida por viajantes. Há rumores sobre uma aranha colossal que tece teias envenenadas e captura qualquer um que ouse entrar.";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            DigitarTexto("💬 Narrador: Você adentra a floresta. O vento uiva entre as árvores retorcidas, e a lua quase não ilumina o caminho. O silêncio é perturbado apenas pelo som de galhos quebrando sob seus pés.", 40);
            Thread.Sleep(5000);

            ExecutarObjetivos(jogador);

            if (VerificarConclusao())
            {
                CompletarMissao(jogador);
            }
            else
            {
                Console.WriteLine("\n❌ Missão falhou! Tente novamente.");
            }
        }

        protected override void ExecutarObjetivos(Personagem jogador)
        {
            bool missaoAbandonada = false;

            while (!aranhaDerrotada && !missaoAbandonada)
            {
                DigitarTexto("\n🕵️ Onde você quer ir?", 30);
                DigitarTexto("1. Explorar um tronco oco", 20);
                DigitarTexto("2. Seguir os fios de teia brilhantes", 20);
                DigitarTexto("0. Abandonar a missão", 20);

                Console.Write("\nDigite sua escolha (1, 2 ou 0): ");
                string escolha = Console.ReadLine();

                if (jogador.Nivel < 8)
                {
                    DigitarTexto("\n⚠️ Você sente um calafrio... ainda não está forte o suficiente para enfrentar o perigo que habita esta floresta.", 40);
                    return;
                }

                if (escolha == "1")
                {
                    DigitarTexto("\nVocê se aproxima de um tronco oco. Dentro, encontra teias antigas e ossos ressecados de viajantes. Nada útil aqui... apenas a lembrança de que o perigo é real.", 40);
                }
                else if (escolha == "2")
                {
                    DigitarTexto("\nVocê segue os fios de teia que brilham sob a lua. Eles levam até uma clareira, onde uma Aranha Gigante desce lentamente do alto das árvores.", 40);

                    AranhaGigante aranha = new AranhaGigante();

                    Console.WriteLine($"\n\n⚔️ ENCONTRO COM {aranha.Nome.ToUpper()} ⚔️");

                    Combate combate = new Combate(jogador, aranha);
                    combate.Iniciar();

                    if (!aranha.EstaVivo)
                    {
                        DigitarTexto($"\n🎉 Você derrotou a {aranha.Nome}!", 40);
                        aranhaDerrotada = true;
                    }
                    else
                    {
                        DigitarTexto($"\n💀 Você foi derrotado pela {aranha.Nome}. A missão falhou.", 40);
                        return;
                    }
                }
                else if (escolha == "0")
                {
                    DigitarTexto("\nVocê decide que é melhor não arriscar sua vida. Volta para a cidade em segurança.", 40);
                    missaoAbandonada = true;
                }
                else
                {
                    DigitarTexto("\nEscolha inválida. Por favor, digite 1, 2 ou 0.", 40);
                }
            }
        }

        protected override bool VerificarConclusao()
        {
            return aranhaDerrotada;
        }

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            int aumentoAgilidade = 40;
            DigitarTexto($"\n🎉 Dentro de um casulo preso nas teias, você encontra Botas Élficas! Elas foram adicionadas ao seu inventário.", 40);
         
            jogador.Nivel += 3;
        }
    }
}

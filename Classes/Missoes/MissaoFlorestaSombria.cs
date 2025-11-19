using System;
using RPG.Classes.Abstracts.Personagens;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes
{
    public class MissaoFlorestaSombria : MissaoBase
    {
        private bool aranhaDerrotada = false;
        private AudioPlayer Som;

        private AudioPlayer audioPlayer; // adicionando o player de áudio

        public MissaoFlorestaSombria(Personagem jogador)
            : base("A Floresta Sombria", "Profundezas da Floresta de Eldor", 400, 900, jogador)
        {
            Descricao = "A floresta sombria é temida por viajantes. Há rumores sobre uma aranha colossal.";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            audioPlayer = new AudioPlayer();
            audioPlayer.PlayLoop("floresta.mp3"); 
            Console.WriteLine($"--- MISSÃO: {Titulo} ---");
            Console.WriteLine(Descricao);
            Console.WriteLine();

            ExecutarObjetivos(jogador);

            audioPlayer.Stop(); 

            if (VerificarConclusao())
            {
                CompletarMissao(jogador);
            }
            else
            {
                Console.WriteLine("Missão falhou.");
                Console.WriteLine($"Status: Vida {jogador.Vida} | Nível {jogador.Nivel}");
            }
        }

        protected override void ExecutarObjetivos(Personagem jogador)
        {
            bool missaoAbandonada = false;

            while (!aranhaDerrotada && !missaoAbandonada)
            {
                Console.WriteLine("\nO que deseja fazer?");
                Console.WriteLine("1 - Explorar um tronco oco");
                Console.WriteLine("2 - Seguir os fios de teia brilhantes");
                Console.WriteLine("0 - Abandonar a missão");
                Console.Write("Escolha: ");
                string escolha = Console.ReadLine();

                if (jogador.Nivel < 8)
                {
                    Console.WriteLine("\nVocê sente que ainda não está forte o suficiente (nível mínimo 8).");
                    return;
                }

                switch (escolha)
                {
                    case "1":
                        Console.WriteLine("\nVocê examina um tronco oco, mas não encontra nada útil.");
                        break;

                    case "2":
                        var aranha = PersonagemFactory.Criar(TipoPersonagem.AranhaGigante, "Aranha Gigante");
                        Console.WriteLine("\nUma Aranha Gigante aparece! Prepare-se para lutar!");

                        audioPlayer.Stop();

                        var combate = new Combate(jogador, aranha);
                        combate.Iniciar();

                
                        audioPlayer.PlayLoop("floresta.mp3");

                        if (!aranha.EstaVivo)
                        {
                            aranhaDerrotada = true;
                            Console.WriteLine("Você derrotou a Aranha Gigante!");
                            Console.WriteLine($"Status: Vida {jogador.Vida} | Nível {jogador.Nivel}");
                        }
                        else
                        {
                            Console.WriteLine("Você foi derrotado...");
                            return;
                        }
                        break;

                    case "0":
                        missaoAbandonada = true;
                        Console.WriteLine("\nVocê abandonou a missão e retornou à cidade.");
                        break;

                    default:
                        Console.WriteLine("\nEscolha inválida. Tente novamente.");
                        break;
                }
            }
        }

        protected override bool VerificarConclusao() => aranhaDerrotada;

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            int aumentoDefesa = 40;
            Console.WriteLine($"\nVocê encontrou Botas Élficas! Defesa +{aumentoDefesa} e +3 níveis.");
            jogador.Defesa += aumentoDefesa;
            jogador.Nivel += 3;
        }
    }
}

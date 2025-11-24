using System;
using RPG.Classes.Abstracts.Personagens;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes
{
    public class MissaoMarProfundo : MissaoBase
    {
        private bool krakenDerrotado = false;

        private AudioPlayer Som;


        public MissaoMarProfundo(Personagem jogador)
            : base("O Mar Profundo", "Abismo Oceânico de Thalassor", 800, 1500, jogador)
        {
            Descricao = "Você mergulhou no Mar Profundo. Lendas falam de um Kraken gigante que guarda tesouros submarinos.";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            Som = new AudioPlayer();
            Som.PlayLoop("mar_profundo.mp3");

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Missão: {Titulo}");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(" Narrador: As águas são escuras e frias; o peso do oceano comprime seus pulmões...");
            Console.WriteLine("Sombras colossais dançam ao longe, e algo antigo desperta nas profundezas.");
            Console.WriteLine();

            ExecutarObjetivos(jogador);

            if (VerificarConclusao())
            {
                CompletarMissao(jogador);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Missão falhou!");
                Console.ResetColor();
             
            }

            Som.Stop();
        }

        protected override void ExecutarObjetivos(Personagem jogador)
        {
            bool missaoAbandonada = false;

            while (!krakenDerrotado && !missaoAbandonada)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("O que você quer fazer?");
                Console.ResetColor();
                Console.WriteLine("1. Explorar o navio naufragado");
                Console.WriteLine("2. Mergulhar até o covil do Kraken");
                Console.WriteLine("0. Abandonar a missão");
                Console.Write("\nDigite sua escolha (1, 2 ou 0): ");
                string escolha = Console.ReadLine();

                if (jogador.Nivel < 15)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Você não está preparado para enfrentar o Kraken.");
                    Console.WriteLine("Volte quando alcançar o nível 15 ou superior!");
                    Console.ResetColor();
                    return;
                }

                if (escolha == "1")
                {
                    Console.Clear();
                    Console.WriteLine("Você vasculha as tábuas retorcidas de um navio antigo...");
                    Console.WriteLine("Encontra pérolas menores e moedas corroídas, mas nenhuma pista do Kraken.");
                    Console.WriteLine("Você retorna para a área principal do abismo.");
                }
                else if (escolha == "2")
                {
                    var kraken = PersonagemFactory.Criar(TipoPersonagem.PovoKraken, "Kraken do Mar Profundo");

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" ENCONTRO COM {kraken.Nome.ToUpper()");
                    Console.ResetColor();
                    Console.WriteLine("Tentáculos colossais surgem do abismo, bloqueando toda rota de fuga!");
                    Console.WriteLine();

                    Som.Push("batalha_kraken.mp3");

                    var combate = new Combate(jogador, kraken);
                    combate.Iniciar();

                    Som.Pop();
                    Console.Clear();

                    if (!kraken.EstaVivo)
                    {
                        krakenDerrotado = true;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Você derrotou o {kraken.Nome}!");
                        Console.ResetColor();
                       
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($" Você foi derrotado pelo {kraken.Nome}.");
                        Console.ResetColor();
                        return;
                    }
                }
                else if (escolha == "0")
                {
                    missaoAbandonada = true;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n Você decide subir à superfície em segurança.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nEscolha inválida! Digite 1, 2 ou 0.");
                    Console.ResetColor();
                }

                if (!missaoAbandonada && !krakenDerrotado)
                {
                    Console.WriteLine("\nPressione ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        protected override bool VerificarConclusao() => krakenDerrotado;

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            int aumentoDefesa = 60;
            Console.WriteLine($"\n Você saqueia o tesouro abissal e encontra a Armadura do Mar! Defesa +{aumentoDefesa}.");
            jogador.Defesa += aumentoDefesa;
            jogador.Nivel += 5;
        }
    }
}

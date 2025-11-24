using System;
using RPG.Classes.Abstracts.Personagens;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes
{
    public class MissaoCavernaPerdida : MissaoBase
    {
        private bool ogroDerrotado = false;
        private AudioPlayer Som;


        public MissaoCavernaPerdida(Personagem jogador)
            : base("A Caverna Perdida", "Antiga Caverna de Zeltor", 500, 1000, jogador)
        {
            Descricao = "Você encontrou a caverna perdida, mas há uma lenda sobre um ogro terrível que a habita...";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            var Som = new AudioPlayer();
            Som.PlayLoop("caverna_perdida.mp3");

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($" Missão: {Titulo}");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(" Narrador: Chegando à entrada da caverna, um calafrio percorre sua espinha...");
            Console.WriteLine("O ar denso e úmido carrega o cheiro de pedra e algo... podre.");
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

            while (!ogroDerrotado && !missaoAbandonada)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n Onde você quer ir?");
                Console.ResetColor();
                Console.WriteLine("1. Explorar a câmara escura");
                Console.WriteLine("2. Seguir a trilha de pegadas");
                Console.WriteLine("0. Abandonar a missão");
                Console.Write("\nDigite sua escolha (1, 2 ou 0): ");
                string escolha = Console.ReadLine();

                if (jogador.Nivel < 10)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Você não tem nível suficiente para enfrentar o ogro.");
                    Console.WriteLine("Volte quando estiver mais forte!");
                    Console.ResetColor();
                    return;
                }

                if (escolha == "1")
                {
                    Console.Clear();
                    Console.WriteLine("Você se esgueira pela câmara escura...");
                    Console.WriteLine("Encontra ossos, cheiro forte, mas nada de valor.");
                    Console.WriteLine("Você retorna para a entrada da caverna.");
                }
                else if (escolha == "2")
                {
                    var ogro = PersonagemFactory.Criar(TipoPersonagem.OgroCaverna, "Ogro da Caverna");

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($" ENCONTRO COM {ogro.Nome.ToUpper()} ");
                    Console.ResetColor();
                    Console.WriteLine("Um enorme Ogro desperta, rugindo e protegendo o baú!");
                    Console.WriteLine();

                    Som.Push("batalha_ogro.mp3");

                    var combate = new Combate(jogador, ogro);
                    combate.Iniciar();

                    Som.Pop();
                    Console.Clear();

                    if (!ogro.EstaVivo)
                    {
                        ogroDerrotado = true;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($" Você derrotou o {ogro.Nome}!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($" Você foi derrotado pelo {ogro.Nome}.");
                        Console.ResetColor();
                        return;
                    }
                }
                else if (escolha == "0")
                {
                    missaoAbandonada = true;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n Você abandona a missão e retorna à cidade em segurança.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Escolha inválida! Digite 1, 2 ou 0.");
                    Console.ResetColor();
                }

                if (!missaoAbandonada && !ogroDerrotado)
                {
                    Console.WriteLine("\nPressione ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        protected override bool VerificarConclusao() => ogroDerrotado;

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            int aumentoDefesa = 50;
            Console.WriteLine($"\n Você abre o baú e encontra a Armadura Mágica! Defesa +{aumentoDefesa}.");
            jogador.Defesa += aumentoDefesa;
            jogador.Nivel += 5;
        }
    }
}

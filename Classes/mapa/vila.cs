using System;
using RPG.Classes.Abstracts.Personagens;
using Rpg.Classes.Personagens;

namespace RPG.Mapa
{
    public class Vila
    {
        private Personagem _heroi;

        public Vila(Personagem heroi)
        {
            _heroi = heroi;
            EntrarNaVila();
        }

        public void Executar()
        {
            EntrarNaVila();
        }

        private void EntrarNaVila()
        {
            var Som = new AudioPlayer();
            Som.PlayLoop("vila.mp3");

            bool sair = false;
            while (!sair)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" Vila dos Ventos Serenos");
                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine($"Herói: {_heroi.Nome} | Ouro: {_heroi.Ouro}");
                Console.WriteLine();
                Console.WriteLine("O que deseja fazer?");
                Console.WriteLine("[1] Visitar o Bar do Jhon");
                Console.WriteLine("[2] Ir à Loja de Armas");
                Console.WriteLine("[3] Falar com Henry, o Velho Guerreiro");
                Console.WriteLine("[0] Sair da Vila");
                Console.WriteLine();
                Console.Write("Escolha: ");
                var escolha = Console.ReadLine()?.Trim();

                switch (escolha)
                {
                    case "1":
                        using (Som.Push("bar.mp3"))
                        {
                            EntrarNoBar();
                        }
                        break;

                    case "2":
                        using (Som.Push("loja.mp3"))
                        {
                            Console.Clear();
                            Console.WriteLine("A Loja de Armas está em construção no momento...");
                            Console.WriteLine("Volte mais tarde!");
                        }
                        break;

                    case "3":
                        using (Som.Push("henry.mp3"))
                        {
                            FalarComHenry();
                        }
                        break;

                    case "0":
                        Console.WriteLine("\nVocê deixa a vila e volta ao mapa principal...");
                        sair = true;
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Opção inválida. Tente novamente.");
                        Console.ResetColor();
                        break;
                }

                if (!sair)
                {
                    Console.WriteLine("\nPressione ENTER para continuar...");
                    Console.ReadLine();
                }
            }

            Som.Stop();
        }

        private void EntrarNoBar()
        {
            Console.Clear();
            Console.WriteLine("🍺 Bar do Jhon");
            Console.WriteLine("Você entra no bar e sente o cheiro forte de cerveja artesanal.");
            Console.WriteLine("O dono, Jhon, limpa um copo e sorri: 'Herói! Sempre bom ver você por aqui.'");
            Console.WriteLine("\nJhon: 'Quer ouvir as novidades ou só relaxar um pouco?'");
            Console.WriteLine("[1] Ouvir as novidades");
            Console.WriteLine("[2] Pedir uma bebida (50 ouro)");
            Console.WriteLine("[0] Sair");
            Console.Write("\nEscolha: ");
            var escolha = Console.ReadLine()?.Trim();

            switch (escolha)
            {
                case "1":
                    Console.WriteLine("\nJhon: 'Dizem que criaturas estranhas andam rondando a Floresta Sombria... cuidado lá!'");
                    break;

                case "2":
                    if (_heroi.Ouro >= 50)
                    {
                        _heroi.Ouro -= 50;
                        Console.WriteLine("\nVocê bebe a cerveja artesanal de Jhon. Sente-se revigorado!");
                        _heroi.Vida = Math.Min(_heroi.Vida + 20, _heroi.VidaMaxima);
                    }
                    else
                    {
                        Console.WriteLine("\nJhon: 'Haha! Parece que está sem moedas hoje, herói!'");
                    }
                    break;

                default:
                    Console.WriteLine("\nVocê sai do bar e volta para a rua principal da vila.");
                    break;
            }
        }

        private void FalarComHenry()
        {
            Console.Clear();
            Console.WriteLine(" Henry, o Velho Guerreiro");
            Console.WriteLine("Você encontra Henry sentado em frente à forja antiga.");
            Console.WriteLine("Ele afia sua espada desgastada enquanto olha para você com olhos experientes.");
            Console.WriteLine();

            Console.WriteLine("Henry: 'Ah... então é você o jovem herói que todos comentam.'");
            Console.WriteLine("Henry: 'Ouça com atenção... o rei está desesperado.'");
            Console.WriteLine();
            Console.WriteLine("Henry: 'A princesa Alice foi sequestrada por uma força sombria que ninguém ousa enfrentar um feiroz Dragão.'");
            Console.WriteLine("Henry: 'O rei prometeu recompensas inimagináveis a quem resgatá-la...'");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("• Muito ouro");
            Console.WriteLine("• Título de nobreza");
            Console.WriteLine("• E a mão da princesa Alice, conhecida por sua rara beleza");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Henry: 'Se você tiver coragem... esse pode ser o destino que mudará sua vida para sempre.'");
            Console.WriteLine("\n[0] Sair");
            Console.ReadLine();
        }
    }
}

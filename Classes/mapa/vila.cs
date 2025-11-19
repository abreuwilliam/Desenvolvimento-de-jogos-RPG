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
                Console.WriteLine("[1] Visitar o Bar do Boris");
                Console.WriteLine("[2] Ir à Loja de Armas");
                Console.WriteLine("[3] Falar com o Ancião da Vila");
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
                        using (Som.Push("anciao.mp3"))
                        {
                            FalarComAnciao();
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
            Console.WriteLine("🍺 Bar do Boris");
            Console.WriteLine("Você entra no bar e sente o cheiro forte de cerveja artesanal.");
            Console.WriteLine("O dono, Boris, limpa um copo e sorri: 'Herói! Sempre bom ver você por aqui.'");
            Console.WriteLine("\nBoris: 'Quer ouvir as novidades ou só relaxar um pouco?'");
            Console.WriteLine("[1] Ouvir as novidades");
            Console.WriteLine("[2] Pedir uma bebida (50 ouro)");
            Console.WriteLine("[0] Sair");
            Console.Write("\nEscolha: ");
            var escolha = Console.ReadLine()?.Trim();

            switch (escolha)
            {
                case "1":
                    Console.WriteLine("\nBoris: 'Dizem que criaturas estranhas andam rondando a Floresta Sombria... cuidado lá!'");
                    break;

                case "2":
                    if (_heroi.Ouro >= 50)
                    {
                        _heroi.Ouro -= 50;
                        Console.WriteLine("\nVocê bebe a cerveja artesanal de Boris. Sente-se revigorado!");
                        _heroi.Vida = Math.Min(_heroi.Vida + 20, _heroi.VidaMaxima);
                    }
                    else
                    {
                        Console.WriteLine("\nBoris: 'Haha! Parece que está sem trocados hoje, herói!'");
                    }
                    break;

                default:
                    Console.WriteLine("\nVocê sai do bar e volta para a rua principal da vila.");
                    break;
            }
        }

        private void FalarComAnciao()
        {
            Console.Clear();
            Console.WriteLine(" Ancião da Vila");
            Console.WriteLine("Você entra na casa do ancião, repleta de livros e ervas aromáticas.");
            Console.WriteLine("\nAncião: 'Ah... vejo que o destino o trouxe até mim, jovem herói.'");
            Console.WriteLine("Ancião: 'A Floresta Sombria guarda mais do que simples monstros... ela guarda memórias.'");
            Console.WriteLine("\n[1] Perguntar sobre a Floresta");
            Console.WriteLine("[2] Pedir bênção");
            Console.WriteLine("[0] Sair");
            Console.Write("\nEscolha: ");
            var escolha = Console.ReadLine()?.Trim();

            switch (escolha)
            {
                case "1":
                    Console.WriteLine("\nAncião: 'A Floresta já foi um santuário. Agora, tomada pelas trevas, esconde o caminho para um poder antigo.'");
                    break;

                case "2":
                    Console.WriteLine("\nO ancião toca sua testa. Uma luz dourada o envolve...");
                    _heroi.Vida = _heroi.VidaMaxima;
                    Console.WriteLine("Sua vida foi completamente restaurada!");
                    break;

                default:
                    Console.WriteLine("\nVocê se despede do ancião e sai de sua casa.");
                    break;
            }
        }
    }
}

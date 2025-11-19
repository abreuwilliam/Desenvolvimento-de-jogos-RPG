using System;
using System.Threading;
using RPG.Classes.Abstracts.Personagens;
using RPG.Classes.mapa;
using Rpg.Classes.Personagens;

namespace RPG.Mapa
{
    public class Navegacao
    {
        private  Personagem _heroi;
        private  Floresta _floresta;
        private Vila _vila;

        public Navegacao(Personagem heroi)
        {
            _heroi = heroi;
            _floresta = new Floresta(_heroi);
            _vila = new Vila(_heroi);
        }

        public void Executar()
        {
            var Som = new AudioPlayer();
            Som.PlayLoop("mundo.mp3");
            bool sair = false;

            while (!sair)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" MAPA DO REINO");
                Console.ResetColor();
                Console.WriteLine("------------------------------------------");
                Console.WriteLine($"Herói: {_heroi.Nome} | Nível: {_heroi.Nivel}");
                Console.WriteLine($"Vida: {_heroi.Vida}/{_heroi.VidaMaxima}");
                Console.WriteLine($"Ataque: {_heroi.Ataque} | Defesa: {_heroi.Defesa}");
                Console.WriteLine($"Ouro: {_heroi.Ouro}");
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("[1] Ir à Vila");
                Console.WriteLine("[2] Entrar na Floresta Sombria");
                Console.WriteLine("[3] Explorar o Deserto Escaldante");
                Console.WriteLine("[4] Consultar Status");
                Console.WriteLine("[0] Sair do Jogo");
                Console.Write("\nEscolha: ");

                string escolha = Console.ReadLine()?.Trim() ?? "";

                switch (escolha)
                {
                    case "1":
                        using (Som.Push("vila.mp3"))
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(" Indo para a Vila...");
                            Console.ResetColor();
                            _vila.Executar(); 
                        }
                        break;

                    case "2":
                        using (Som.Push("floresta.mp3"))
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Entrando na Floresta Sombria...");
                            Console.ResetColor();
                            _floresta.Iniciar(); 
                        }
                        break;
                    case "3":
                        using (Som.Push("deserto.mp3"))
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(" Explorando o Deserto Escaldante...");
                            Console.ResetColor();
                            var deserto = new Deserto(_heroi);
                            if(_heroi.Experiencia >= 500){deserto.EntrarNoDeserto();} 
                            else
                            {
                                Console.WriteLine("Você ainda não tem experiência suficiente para explorar o deserto. Volte quando estiver mais forte!");
                            }
                        }

                    case "4":
                        MostrarStatus();
                        break;

                    case "0":
                        Console.Write("Deseja realmente sair do jogo? (S/N): ");
                        string confirmar = Console.ReadLine()?.Trim().ToUpper() ?? "";
                        if (confirmar == "S" || confirmar == "SIM")
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Encerrando... Até a próxima aventura!");
                            Console.ResetColor();
                            sair = true;
                        }
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Opção inválida. Tente novamente.");
                        Console.ResetColor();
                        Thread.Sleep(800);
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

        private void MostrarStatus()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("STATUS DO HERÓI");
            Console.ResetColor();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"Nome: {_heroi.Nome}");
            Console.WriteLine($"Nível: {_heroi.Nivel}");
            Console.WriteLine($"Vida: {_heroi.Vida}/{_heroi.VidaMaxima}");
            Console.WriteLine($"Ataque: {_heroi.Ataque}");
            Console.WriteLine($"Defesa: {_heroi.Defesa}");
            Console.WriteLine($"Experiência: {_heroi.Experiencia}");
            Console.WriteLine($"Ouro: {_heroi.Ouro}");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine(" Dica: explore a Floresta para ganhar experiência e ouro!");
            Console.WriteLine("\nPressione ENTER para voltar ao mapa...");
            Console.ReadLine();
        }
    }
}

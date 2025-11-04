using System;
using System.Text;
using System.Threading;
using Rpg.Classes.Personagens;
using RPG.Classes.Abstracts.Personagens;

namespace RPG.Classes.mapa
{
   
    public class MenuPrincipal
    {
        private Personagem heroi;
        private AudioPlayer audioPlayer; 

        public MenuPrincipal(Personagem heroi)
        {
      
            this.heroi = heroi;
        }

        
        public void Executar()
        {
            Console.OutputEncoding = Encoding.UTF8;

            audioPlayer = new AudioPlayer();
            audioPlayer.PlayLoop("menu.mp3"); 

            while (true)
            {
                // Tela do menu principal
                Console.Clear();
                int largura = 80;
                int altura = 20;
                int left = (Console.WindowWidth - largura) / 2;
                int top = (Console.WindowHeight - altura) / 2;

                // Fragmentos da caixa
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.SetCursorPosition(left, top);
                Console.Write("╔" + new string('═', largura - 2) + "╗");
                for (int y = 1; y < altura - 1; y++)
                {
                    Console.SetCursorPosition(left, top + y);
                    Console.Write("║" + new string(' ', largura - 2) + "║");
                }
                Console.SetCursorPosition(left, top + altura - 1);
                Console.Write("╚" + new string('═', largura - 2) + "╝");

                // conteúdo do menu
                Console.ForegroundColor = ConsoleColor.Cyan;
                string titulo = "BEM-VINDO(A) AO REINO DAS LENDAS";
                Console.SetCursorPosition(left + (largura - titulo.Length) / 2, top + 2);
                Console.WriteLine(titulo);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(left + 2, top + 4);
                Console.WriteLine("Aqui você vai se divertir e navegar por um mundo de aventuras.");
                Console.SetCursorPosition(left + 2, top + 5);
                Console.WriteLine("Explore a vila, enfrente perigos na floresta e descubra missões.");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(left + 2, top + 8);
                Console.WriteLine($"Herói: {heroi.Nome}  •  Nível: {heroi.Nivel}  •  Ouro: {heroi.Ouro}");

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(left + 2, top + 11);
                Console.WriteLine("[1] Começar a jornada");
                Console.SetCursorPosition(left + 2, top + 12);
                Console.WriteLine("[2] Ver tutorial rápido");
                Console.SetCursorPosition(left + 2, top + 13);
                Console.WriteLine("[3] Créditos");
                Console.SetCursorPosition(left + 2, top + 14);
                Console.WriteLine("[0] Sair do jogo");

                Console.SetCursorPosition(left + 2, top + 16);
                Console.Write("Escolha: ");
                Console.ResetColor();
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        Console.WriteLine("Ótimo! Prepare-se... Boa sorte!");
                        Thread.Sleep(1500);
                        audioPlayer.Stop(); // Para a música manualmente
                        return; // Sai do método e continua o jogo

                    case "2":
                        // tela do tutorial
                        Console.Clear();
                        // Fragmentos caixa 2
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.SetCursorPosition(left, top);
                        Console.Write("╔" + new string('═', largura - 2) + "╗");
                        for (int y = 1; y < altura - 1; y++) { Console.SetCursorPosition(left, top + y); Console.Write("║" + new string(' ', largura - 2) + "║"); }
                        Console.SetCursorPosition(left, top + altura - 1);
                        Console.Write("╚" + new string('═', largura - 2) + "╝");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition(left + (largura - 10) / 2, top + 2);
                        Console.WriteLine("TUTORIAL");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(left + 2, top + 4);
                        Console.WriteLine("• Use os números para escolher onde ir.");
                        Console.SetCursorPosition(left + 2, top + 5);
                        Console.WriteLine("• As batalhas são por turnos.");
                        Console.SetCursorPosition(left + 2, top + 6);
                        Console.WriteLine("• Fale com pessoas na vila para missões.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break; // Volta para o loop do menu

                    case "3":
                        // Tela de cresitos
                        Console.Clear();
                        // Fragmentos caixa 3
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.SetCursorPosition(left, top);
                        Console.Write("╔" + new string('═', largura - 2) + "╗");
                        for (int y = 1; y < altura - 1; y++) { Console.SetCursorPosition(left, top + y); Console.Write("║" + new string(' ', largura - 2) + "║"); }
                        Console.SetCursorPosition(left, top + altura - 1);
                        Console.Write("╚" + new string('═', largura - 2) + "╝");

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(left + (largura - 8) / 2, top + 2);
                        Console.WriteLine("CRÉDITOS");

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(left + 2, top + 4);
                        Console.WriteLine("Jogo feito por: Um Desenvolvedor Iniciante");
                        Console.SetCursorPosition(left + 2, top + 5);
                        Console.WriteLine("Música: Da internet");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition(left + 2, top + 8);
                        Console.WriteLine("Obrigado por jogar!");

                        Console.ResetColor();
                        Console.ReadKey();
                        break; // Volta para o loop do menu

                    case "0":
                        Console.WriteLine("Tem certeza que deseja sair? (s/n)");
                        string conf = Console.ReadLine();
                        // Validação simples e case-sensitive
                        if (conf.ToLower() == "s")
                        {
                            Console.WriteLine("Até a próxima!");
                            Thread.Sleep(1000);
                            audioPlayer.Stop();
                            Environment.Exit(0); // Fecha o programa
                        }
                        break; // Se não for "s", volta para o menu

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opção inválida! Pressione qualquer tecla para tentar de novo.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break; // Volta para o loop do menu
                }
            }
        }
    }
}
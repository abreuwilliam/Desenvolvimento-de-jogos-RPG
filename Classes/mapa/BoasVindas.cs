using System;
using System.Text;
using System.Threading;
using Rpg.Classes.Personagens;
using RPG.Classes.Abstracts.Personagens;

    public class BoasVindas
    {
        private Personagem heroi;
        private AudioPlayer audioPlayer; 

        public BoasVindas(Personagem heroi)
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
                Console.Clear();
           
             
                Console.ForegroundColor = ConsoleColor.Cyan;
                string titulo = "BEM-VINDO(A) AO REINO DAS LENDAS";

                Console.WriteLine(titulo);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Aqui você vai se divertir e navegar por um mundo de aventuras.");
                Console.WriteLine("Explore a vila, enfrente perigos na floresta e descubra missões.");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Herói: {heroi.Nome}  •  Nível: {heroi.Nivel}  •  Ouro: {heroi.Ouro}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[1] Começar a jornada");
                Console.WriteLine("[2] Ver tutorial rápido");
                Console.WriteLine("[3] Créditos");
                Console.WriteLine("[0] Sair do jogo");
                Console.Write("Escolha: ");
                Console.ResetColor();
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        Console.WriteLine("Ótimo! Prepare-se... Boa sorte!");
                        Thread.Sleep(1500);
                        audioPlayer.Stop(); 
                        return; 
                    case "2":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkGray;
            
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("• Use os números para escolher onde ir.");
                        
                        Console.WriteLine("• As batalhas são por turnos.");
                  
                        Console.WriteLine("• Fale com pessoas na vila para missões.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break; 

                    case "3":
                        Console.Clear();
                        Console.WriteLine("CRÉDITOS");
                        Console.WriteLine("Desenvolvido por: William Abreu, Luis Gustavo");
                        Console.WriteLine(" Caio Espindola , Julia Cardoso, Julia Freitas");
                        Console.WriteLine("Obrigado por jogar!");
                        Console.ResetColor();
                        Console.ReadKey();
                        break; 

                    case "0":
                        Console.WriteLine("Tem certeza que deseja sair? (s/n)");
                        string conf = Console.ReadLine();
                        if (conf.ToLower() == "s")
                        {
                            Console.WriteLine("Até a próxima!");
                            Thread.Sleep(1000);
                            audioPlayer.Stop();
                            Environment.Exit(0); 
                        }
                        break; 

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opção inválida! Pressione qualquer tecla para tentar de novo.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break; 
                }
            }
        }
    }

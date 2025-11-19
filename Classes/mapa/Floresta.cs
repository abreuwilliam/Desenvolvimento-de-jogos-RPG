using System;
using System.Collections.Generic;
using Rpg.Classes.Missoes;
using Rpg.Classes.Personagens;
using RPG.Classes.Abstracts.Personagens;


namespace RPG.Classes.mapa
{
    
    public class Floresta
    {
        private Personagem heroi;
        private Random random = new Random();
        private AudioPlayer audioPlayer;

        
        private string pista1 = "Não sou um ser vivo, mas respiro e tenho uma boca.";
        private string pista2 = "Tenho um corpo, mas não tenho braços nem pernas.";
        private string pista3 = "Quando me alimento, cresço. Quando me negligenciam, morro.";
        private string pista4 = "Eu posso te dar calor e conforto, mas também posso te destruir.";

        private bool pegouPista1 = false;
        private bool pegouPista2 = false;
        private bool pegouPista3 = false;
        private bool pegouPista4 = false;
        private int pistasColetadas = 0;


        public Floresta(Personagem heroi)
        {
            this.heroi = heroi;
        }

        public void Iniciar()
        {
            audioPlayer = new AudioPlayer();
            audioPlayer.PlayLoop("floresta.mp3");

            Console.Clear();
     
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Você entra na Floresta Sombria...");
            Console.WriteLine("O ar é denso e o vento sussurra segredos antigos.");
            Console.ResetColor();
            Console.ReadKey();

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Escolha seu caminho:");
                Console.WriteLine("[1] Seguir pela direita (árvore gigante)");
                Console.WriteLine("[2] Caminho da clareira enigmática");
                Console.WriteLine("[3] Trilha do lago misterioso");
                Console.WriteLine("[4] Caminho iluminado entre árvores antigas");
                Console.WriteLine("[5] Tentar resolver o enigma final");
                Console.WriteLine("[0] Sair da Floresta");
                Console.Write("Opção: ");
                Console.ResetColor();
                string escolha = Console.ReadLine();

                switch (escolha)
                {
                    case "1":
                        Console.WriteLine("Você segue em direção à árvore gigante...");
                        Thread.Sleep(1000);
                        Console.WriteLine("De repente, um Lobo Sombrio surge!");

                     
                        Personagem lobo = new Personagem("Lobo Sombrio", 30, 8, 3);
                        audioPlayer.Stop();
                        Combate c1 = new Combate(heroi, lobo);
                        c1.Iniciar();
                        audioPlayer.PlayLoop("floresta.mp3");

                        if (heroi.EstaVivo)
                        {
                            if (!pegouPista1)
                            {
                                Console.WriteLine("Você encontrou uma pista gravada na árvore!");
                                Console.WriteLine($"PISTA: \"{pista1}\"");
                                pegouPista1 = true;
                                pistasColetadas++;
                                Console.WriteLine($"Pistas coletadas: {pistasColetadas}/4");
                            }
                            else
                            {
                                Console.WriteLine("Você já explorou este lugar. Nada de novo.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Você foi derrotado e recua para se recuperar...");
                        }
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.WriteLine("Você avança pela clareira...");
                        Thread.Sleep(1000);
                        Console.WriteLine("Uma Onça Pintada salta na sua frente!");

                        Personagem onca = new Personagem("Onça Pintada", 40, 10, 4);
                        audioPlayer.Stop();
                        Combate c2 = new Combate(heroi, onca);
                        c2.Iniciar();
                        audioPlayer.PlayLoop("floresta.mp3");

                        if (heroi.EstaVivo)
                        {
                            if (!pegouPista2)
                            {
                                Console.WriteLine("Você encontrou símbolos estranhos no chão!");
                                Console.WriteLine($"PISTA: \"{pista2}\"");
                                pegouPista2 = true;
                                pistasColetadas++;
                                Console.WriteLine($"Pistas coletadas: {pistasColetadas}/4");
                            }
                            else
                            {
                                Console.WriteLine("Nada de novo por aqui.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Você foi derrotado e recua...");
                        }
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.WriteLine("Você se aproxima do círculo de pedras...");
                        if (pistasColetadas < 4)
                        {
                            Console.WriteLine("Ainda faltam pistas para resolver o enigma!");
                            Console.WriteLine($"Você só tem {pistasColetadas} de 4 pistas.");
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            Console.WriteLine("Você tem todas as pistas:");
                            if (pegouPista1) Console.WriteLine($"- {pista1}");
                            if (pegouPista2) Console.WriteLine($"- {pista2}");
                            if (pegouPista3) Console.WriteLine($"- {pista3}");
                            if (pegouPista4) Console.WriteLine($"- {pista4}");

                            Console.WriteLine("\n'Se você me abraçar, terá calor. Que sou eu?'");
                            Console.Write("Sua resposta: ");
                            string resposta = Console.ReadLine().ToLower();

                            if (resposta.Contains("fogo")) 
                            {
                                Console.WriteLine("Correto! Uma passagem secreta se abre!");
                                Thread.Sleep(1500);
                                
                            }
                            else
                            {
                                Console.WriteLine("Errado! Você sente uma tontura e perde 10 de ouro.");
                                heroi.GastarOuro(10);
                                Thread.Sleep(1500);
                            }
                        }
                        break;

                    case "0":
                        Console.WriteLine("Você sai da floresta...");
                        audioPlayer.Stop();
                        return; 

                    default:
                        Console.WriteLine("Opção inválida.");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
    }
}
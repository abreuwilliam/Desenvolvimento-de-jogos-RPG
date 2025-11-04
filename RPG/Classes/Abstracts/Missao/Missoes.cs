using System;
using System.IO;
using System.Text;
using System.Threading;
using RPG.Classes.Abstracts.Personagens;

namespace RPG.Classes.Abstracts.Missao
{
    
    public class Missao
    {
        // Propriedades publicas para que qualqer pessoa possa editar
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Local { get; set; }
        public bool EstaCompleta { get; set; }
        public bool EstaAtiva { get; set; }
        public int ExperienciaRecompensa { get; set; }
        public int OuroRecompensa { get; set; }
        public Personagem Jogador { get; set; }

        private AudioPlayer audioPlayer;

        public Missao(string titulo, string descricao, string local, int exp, int ouro, Personagem jogador)
        {
            Id = Guid.NewGuid().ToString();
            Titulo = titulo;
            Descricao = descricao;
            Local = local;
            ExperienciaRecompensa = exp;
            OuroRecompensa = ouro;
            Jogador = jogador;
            EstaCompleta = false;
            EstaAtiva = false;
        }

       
        public void ExecutarMissao()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Inicia a música, sem parar se o programa quebrar
            audioPlayer = new AudioPlayer();
            // Caminho do arquivo "hardcoded", em caso de possivel modificação do caminho da pasta
            string caminhoMusica = "C:\\Users\\MeuUsuario\\Documents\\MeuJogo\\Assets\\missao.mp3";
            if (File.Exists(caminhoMusica))
            {
                audioPlayer.PlayLoop(caminhoMusica);
            }

            if (EstaCompleta)
            {
                Console.WriteLine("Você já completou esta missão!");
                Console.ReadKey();
                return;
            }

            EstaAtiva = true;

            
            Console.Clear();
            int largura = 70;
            int altura = 16;
            int left = (Console.WindowWidth - largura) / 2;
            int top = (Console.WindowHeight - altura) / 2;

            // fragmentos do Desenho da caixa
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

            // Console Titulo
            Console.ForegroundColor = ConsoleColor.Yellow;
            string tituloPainel = $"INICIANDO MISSÃO: {Titulo}";
            Console.SetCursorPosition(left + (largura - tituloPainel.Length) / 2, top + 1);
            Console.WriteLine(tituloPainel);

            // Console conteúdo
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left + 2, top + 3);
            Console.WriteLine($"Local: {Local}");

            Console.SetCursorPosition(left + 2, top + 5);
            // Efeito de máquina de escrever copiado aqui
            foreach (char c in Descricao)
            {
                Console.Write(c);
                Thread.Sleep(25);
            }

            Console.SetCursorPosition(left + 2, top + altura - 2);
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey(true);


            // TELA 2 HISTÓRIA 
            Console.Clear();
            // Calcula altura e largura
            largura = 70;
            altura = 16;
            left = (Console.WindowWidth - largura) / 2;
            top = (Console.WindowHeight - altura) / 2;

            // fragmentos do Desenho da caixa 2
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

            // Escreve o título
            Console.ForegroundColor = ConsoleColor.Cyan;
            tituloPainel = "HISTÓRIA";
            Console.SetCursorPosition(left + (largura - tituloPainel.Length) / 2, top + 1);
            Console.WriteLine(tituloPainel);

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left + 2, top + 3);
            string historia = $"Você está em {Local}, um lugar perigoso...";
            foreach (char c in historia)
            {
                Console.Write(c);
                Thread.Sleep(25);
            }

            Console.SetCursorPosition(left + 2, top + altura - 2);
            Console.WriteLine("Pressione qualquer tecla para começar os objetivos...");
            Console.ResetColor();
            Console.ReadKey(true);

            // Objetos
            bool sucesso = false;
            if (Titulo == "Coletar Ervas")
            {
                Console.WriteLine("Você encontrou 3 ervas medicinais!");
                Thread.Sleep(1500);
                sucesso = true;
            }
            else if (Titulo == "Matar Ratos")
            {
                Console.WriteLine("Você luta com um rato gigante...");
                Thread.Sleep(1000);
                Jogador.Vida -= 5; 
                Console.WriteLine("Você venceu, mas perdeu 5 de vida.");
                Thread.Sleep(1500);
                sucesso = Jogador.Vida > 0;
            }
            else
            {
                Console.WriteLine("Nenhum objetivo definido para esta missão.");
                Thread.Sleep(1500);
            }


            // ---- TELA 3:
            Console.Clear();
            // Recalcula 
            left = (Console.WindowWidth - largura) / 2;
            top = (Console.WindowHeight - altura) / 2;

            // fragmentos do Desenho da caixa 3
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

            if (sucesso)
            {
                EstaCompleta = true;
                Jogador.AdicionarExperiencia(ExperienciaRecompensa);
                Jogador.AdicionarOuro(OuroRecompensa);

                Console.ForegroundColor = ConsoleColor.Green;
                tituloPainel = "MISSÃO CONCLUÍDA!";
                Console.SetCursorPosition(left + (largura - tituloPainel.Length) / 2, top + 1);
                Console.WriteLine(tituloPainel);

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(left + 2, top + 3);
                Console.WriteLine($"Recompensa: +{ExperienciaRecompensa} EXP");
                Console.SetCursorPosition(left + 2, top + 4);
                Console.WriteLine($"Recompensa: +{OuroRecompensa} Ouro");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                tituloPainel = "MISSÃO FALHOU!";
                Console.SetCursorPosition(left + (largura - tituloPainel.Length) / 2, top + 1);
                Console.WriteLine(tituloPainel);
            }

            Console.SetCursorPosition(left + 2, top + altura - 2);
            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ResetColor();
            Console.ReadKey(true);

            // Stop musica final da repetição 
            audioPlayer.Stop();
            audioPlayer.Dispose();
        }
    }
}
using System;
using System.Text;
using System;
using System.IO;
using System.Text;


namespace Rpg.UI
{
    public enum Dificuldade { Facil, Medio, Dificil }



    public sealed class MenuResult
    {
        public string Nome { get; }
        public Dificuldade Dificuldade { get; }
        public bool Confirmado { get; }

        public MenuResult(string nome, Dificuldade dif, bool ok)
        {
            Nome = nome;
            Dificuldade = dif;
            Confirmado = ok;
        }
    }
    

    public static class MainMenu
    {
        // Ajuste as cores aqui
        private static readonly ConsoleColor TitleColor = ConsoleColor.Cyan;
        private static readonly ConsoleColor AccentColor = ConsoleColor.Yellow;
        private static readonly ConsoleColor BoxColor = ConsoleColor.DarkGray;
        private static readonly ConsoleColor TextColor = ConsoleColor.White;

       public static MenuResult Show()
{
    PrepararConsole();
    var nome = "Cavaleiro ";
    var dif  = Dificuldade.Facil;
    var foco = 0;

    // === Música do menu ===
    var audio = new AudioPlayer();
    var musicPath = Path.Combine(AppContext.BaseDirectory, "Assets", "menu.mp3");
    Console.WriteLine($"[audio] tentando: {musicPath}");
    if (File.Exists(musicPath))
                Console.WriteLine("tocando musica de menu: " + musicPath);
        audio.PlayLoop(musicPath);

    try
    {
        while (true)
        {
            Console.Clear();
            DesenharBanner();

            var (left, top, width, height) = DesenharJanela(60, 15);
            EscreverCentral("Crie seu aventureiro", top + 1, AccentColor, width, left);

            // Nome
            var nomeY = top + 3;
            Escrever(left + 4, nomeY, "Nome:", TextColor);
            var nomeBoxW = width - 18;
            DesenharInput(left + 12, nomeY, nome, nomeBoxW, foco == 0);

            // Dificuldade (descido)
            var difY = top + 6;
            Escrever(left + 4, difY, "Dificuldade:", TextColor);
            DesenharDificuldades(left + 16, difY, dif, foco == 1);

            // Dicas
            var maxText = width - 8;
            Escrever(left + 4, top + 10, Limitar("↑/↓ alterna campos • ←/→ troca dificuldade • Digite para editar o nome", maxText), ConsoleColor.DarkGray);
            Escrever(left + 4, top + 11, Limitar("[Enter] confirmar • [Esc] sair", maxText), ConsoleColor.DarkGray);

            // Botão
            var botY = top + 13;
            DesenharBotaoCentral("Iniciar Aventura", botY, foco == 2, width, left);

            // Input
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                audio.Stop(); // <=== para a música ao sair
                return new MenuResult(nome, dif, false);
            }

            if (key.Key == ConsoleKey.Enter)
            {
                if (foco == 2)
                {
                    audio.Stop(); // <=== para a música ao confirmar
                    return new MenuResult(string.IsNullOrWhiteSpace(nome) ? "Herói" : nome.Trim(), dif, true);
                }
                else foco = Math.Min(2, foco + 1);
            }
            else if (key.Key is ConsoleKey.UpArrow)   foco = Math.Max(0, foco - 1);
            else if (key.Key is ConsoleKey.DownArrow) foco = Math.Min(2, foco + 1);
            else if (foco == 0)                        nome = TratarEntradaTexto(nome, key);
            else if (foco == 1)
            {
                if (key.Key is ConsoleKey.LeftArrow)  dif = PrevDif(dif);
                if (key.Key is ConsoleKey.RightArrow) dif = NextDif(dif);
            }
        }
    }
    finally
    {
        // segurança extra caso algum return escape o Stop()
        audio.Stop();
        audio.Dispose();
    }
}



        // ---------- helpers visuais ----------
        private static void PrepararConsole()
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    if (Console.WindowWidth < 90) Console.SetWindowSize(Math.Max(90, Console.WindowWidth), Console.WindowHeight);
                    if (Console.WindowHeight < 30) Console.SetWindowSize(Console.WindowWidth, Math.Max(30, Console.WindowHeight));
                }
            }
            catch { /* alguns terminais não permitem redimensionar */ }
        }

      private static void DesenharBanner()
{
    Console.ForegroundColor = TitleColor;

    EscreverCentral("============================================================", 1);
    EscreverCentral("                                                            ", 2);
    EscreverCentral("                     D R A G O N   C A S T L E              ", 3);
    EscreverCentral("                                                            ", 4);
    EscreverCentral("============================================================", 5);

    Console.ResetColor();
}


        /// <summary> Desenha uma caixa centralizada e retorna (left, top, width, height) </summary>
        private static (int left, int top, int width, int height) DesenharJanela(int width, int height)
        {
            int left = (Console.WindowWidth - width) / 2;
            int top = (Console.WindowHeight - height) / 2 + 1;

            Console.ForegroundColor = BoxColor;
            // bordas ┌ ┐ └ ┘ │ ─
            Escrever(left, top, "┌" + new string('─', width - 2) + "┐");
            for (int y = 1; y < height - 1; y++)
            {
                Escrever(left, top + y, "│" + new string(' ', width - 2) + "│");
            }
            Escrever(left, top + height - 1, "└" + new string('─', width - 2) + "┘");
            Console.ResetColor();

            return (left, top, width, height);
        }
        private static string Limitar(string s, int max) =>
    string.IsNullOrEmpty(s) ? "" : (s.Length <= max ? s : s[..max]);


        private static void DesenharInput(int x, int y, string texto, int largura, bool foco)
        {
            var borda = foco ? '═' : '─';
            Console.ForegroundColor = foco ? AccentColor : BoxColor;
            Escrever(x - 1, y, "╔" + new string(borda, largura) + "╗");
            Escrever(x - 1, y + 1, "║" + Alinhar(texto, largura) + "║");
            Escrever(x - 1, y + 2, "╚" + new string(borda, largura) + "╝");
            Console.ResetColor();
        }

        private static void DesenharDificuldades(int x, int y, Dificuldade atual, bool foco)
        {
            var itens = new[] { "Fácil", "Médio", "Difícil" };
            for (int i = 0; i < itens.Length; i++)
            {
                var marcado = (int)atual == i;
                var texto = $" {(marcado ? "●" : "○")} {itens[i]}  ";
                Console.ForegroundColor = marcado ? AccentColor : (foco ? ConsoleColor.Gray : TextColor);
                Escrever(x, y + i, texto);
            }
            Console.ResetColor();
        }

        private static void DesenharBotaoCentral(string texto, int y, bool foco, int boxWidth, int boxLeft)
        {
            var content = $"[ {texto} ]";
            var x = boxLeft + (boxWidth - content.Length) / 2;
            Console.ForegroundColor = foco ? AccentColor : TextColor;
            Escrever(x, y, content);
            Console.ResetColor();
        }

        private static void EscreverCentral(string texto, int y, ConsoleColor? color = null, int width = 0, int left = 0)
        {
            int w = width == 0 ? Console.WindowWidth : width;
            int l = left == 0 ? 0 : left;
            int x = l + (w - texto.Length) / 2;
            if (color.HasValue) Console.ForegroundColor = color.Value;
            Escrever(x, y, texto);
            if (color.HasValue) Console.ResetColor();
        }

        private static void Escrever(int x, int y, string texto, ConsoleColor? color = null)
        {
            try { Console.SetCursorPosition(Math.Max(0, x), Math.Max(0, y)); } catch { }
            if (color.HasValue) Console.ForegroundColor = color.Value;
            Console.Write(texto);
            if (color.HasValue) Console.ResetColor();
        }

        private static string Alinhar(string s, int largura)
        {
            s ??= "";
            if (s.Length > largura) return s[..largura];
            return s + new string(' ', largura - s.Length);
        }

        private static string TratarEntradaTexto(string atual, ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Backspace)
                return atual.Length > 0 ? atual[..^1] : "";
            if (key.Key == ConsoleKey.Spacebar) return atual + " ";
            if (!char.IsControl(key.KeyChar)) return atual + key.KeyChar;
            return atual;
        }

        private static Dificuldade NextDif(Dificuldade d) =>
            d == Dificuldade.Dificil ? Dificuldade.Facil : (Dificuldade)((int)d + 1);
        private static Dificuldade PrevDif(Dificuldade d) =>
            d == Dificuldade.Facil ? Dificuldade.Dificil : (Dificuldade)((int)d - 1);
    }
}

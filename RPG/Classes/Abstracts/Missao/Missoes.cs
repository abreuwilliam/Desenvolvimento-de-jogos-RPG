using System;
using System.IO;
using System.Text;
using System.Threading;
using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Missoes
{
    public abstract class MissaoBase
    {
        // ---- dados da missão ----
        public string Id { get; protected set; }
        public string Titulo { get; protected set; }
        public string Descricao { get; protected set; } = string.Empty; // ✅ garante inicialização
        public string Local { get; protected set; }
        public bool EstaCompleta { get; protected set; }
        public bool EstaAtiva { get; protected set; }
        public int ExperienciaRecompensa { get; protected set; }
        public int OuroRecompensa { get; protected set; }
        public Personagem Jogador { get; protected set; }

        // ---- áudio da missão (BGM) ----
        private AudioPlayer? _bgm;
        private string? _bgmPath;
        private bool _bgmAtiva;

        // ---- layout do painel ----
        private static int _left, _top, _width, _height, _cursorY;

        // ---- estilos/velocidades ----
        protected int VelocidadeTextoMs { get; set; } = 25;
        private const int LinhaAnimMs = 12;
        private const int FadeMs = 10;

        protected MissaoBase(string titulo, string local, int expRecompensa, int ouroRecompensa, Personagem jogador)
        {
            Id = Guid.NewGuid().ToString();
            Titulo = titulo;
            Local = local;
            ExperienciaRecompensa = expRecompensa;
            OuroRecompensa = ouroRecompensa;
            EstaCompleta = false;
            EstaAtiva = false;
            Jogador = jogador;
        }

        // ========== ciclo de execução ==========
        public abstract void IniciarMissao(Personagem jogador);

        public void ExecutarMissao(Personagem jogador)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // BGM da missão
            _bgmPath = Path.Combine(AppContext.BaseDirectory, "Assets", "missao.mp3");
            _bgm = new AudioPlayer();
            if (File.Exists(_bgmPath))
            {
                _bgm.PlayLoop(_bgmPath);
                _bgmAtiva = true;
            }

            if (EstaCompleta)
            {
                Painel(() =>
                {
                    EscreverCentral("✅ Esta missão já foi completada!", 0, ConsoleColor.Green);
                    Linha();
                    EscreverLinha("Pressione qualquer tecla para voltar...");
                }, titulo: $"MISSÃO: {Titulo}");
                Console.ReadKey(true);
                return;
            }

            try
            {
                EstaAtiva = true;

                // Intro
                Painel(() =>
                {
                    EscreverCentral($"📍 Local: {Local}", 0, ConsoleColor.Yellow);
                    Linha();
                    Typewriter($"📝 {Descricao}", VelocidadeTextoMs);
                    Linha();
                }, titulo: $"🎯 INICIANDO MISSÃO: {Titulo}");

                // História
                Painel(() =>
                {
                    EscreverCentral("💬 Narrador", 0, ConsoleColor.Cyan);
                    Linha();
                    Typewriter(ContarHistoria(), VelocidadeTextoMs);
                    Linha();
                    SpinnerMensagem("Preparando objetivos", 16);
                }, titulo: "📖 HISTÓRIA");

                // Objetivos
                TransicaoFadeOutIn();
                ExecutarObjetivos(jogador);

                // Resultado
                if (VerificarConclusao())
                {
                    CompletarMissao(jogador);
                }
                else
                {
                    Painel(() =>
                    {
                        EscreverCentral("❌ Missão falhou! Tente novamente.", 0, ConsoleColor.Red);
                        Linha();
                        EscreverLinha($"👤 {Jogador.Nome} | ❤️ {Jogador.Vida}/{Jogador.VidaMaxima} | 💰 {Jogador.Ouro}");
                    }, titulo: $"MISSÃO: {Titulo}");
                }
            }
            finally
            {
                if (_bgmAtiva) _bgm?.Stop();
                _bgm?.Dispose();
                _bgm = null;
                _bgmAtiva = false;
            }
        }

        // ========== para a SUBCLASSE implementar ==========
        protected virtual string ContarHistoria()
            => $"Você avança por {Local}. O vento traz sussurros antigos... algo observa nas sombras.";

        protected abstract void ExecutarObjetivos(Personagem jogador);
        protected abstract bool VerificarConclusao();

        protected virtual void CompletarMissao(Personagem jogador)
        {
            EstaCompleta = true;
            EstaAtiva = false;

            string saida = CapturarSaidaConsole(() =>
            {
                jogador.AdicionarExperiencia(ExperienciaRecompensa);
                jogador.AdicionarOuro(OuroRecompensa);
                Console.WriteLine($"🎉 {jogador.Nome} concluiu {Titulo}!");
                Console.WriteLine($"🎁 Recompensas: +{ExperienciaRecompensa} EXP • +{OuroRecompensa} Ouro");
                DarRecompensaExtra(jogador);
            });

            // música de vitória
            PausarBgmMissao();
            var winPath = Path.Combine(AppContext.BaseDirectory, "Assets", "vitoria.mp3");
            AudioPlayer? win = null;
            try
            {
                if (File.Exists(winPath))
                {
                    win = new AudioPlayer();
                    win.PlayLoop(winPath);
                }
            }
            catch { }

            Painel(() =>
            {
                EscreverCentral("🎉 MISSÃO CONCLUÍDA!", 0, ConsoleColor.Green);
                Linha();
                foreach (var linha in saida.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    EscreverLinha(linha);
                Linha();
                EscreverLinha($"👤 {Jogador.Nome} | ❤️ {Jogador.Vida}/{Jogador.VidaMaxima} | 💰 {Jogador.Ouro}");
            }, titulo: $"MISSÃO: {Titulo}");

            if (win != null)
            {
                win.Stop();
                win.Dispose();
            }
        }

        protected virtual void DarRecompensaExtra(Personagem jogador) { }

        // ========== controle de BGM ==========
        protected void PausarBgmMissao()
        {
            if (_bgmAtiva)
            {
                _bgm?.Stop();
                _bgmAtiva = false;
            }
        }

        protected void RetomarBgmMissao()
        {
            if (!_bgmAtiva && _bgm != null && _bgmPath != null && File.Exists(_bgmPath))
            {
                _bgm.PlayLoop(_bgmPath);
                _bgmAtiva = true;
            }
        }

        // ========== helpers visuais ==========
protected void Painel(Action conteudo, string titulo, int larguraMin = 70, int alturaMin = 16)
{
    Console.Clear();
    DesenharCaixaCentral(larguraMin, alturaMin, ConsoleColor.DarkGray, ConsoleColor.Black);

    // anima o título centralizado
    EscreverCentralAnimado(titulo, 1, ConsoleColor.Yellow, invert: true, relativoAoConteudo: false);

    // linha de divisória mais lenta e com efeito
    LinhaAtAnimada(_top + 2, ConsoleColor.DarkGray, 20);

    _cursorY = _top + 3;  // conteúdo começa logo abaixo
    conteudo?.Invoke();

    LinhaAtAnimada(_cursorY++, ConsoleColor.DarkGray, 10);
    EscreverLinha("Pressione qualquer tecla para continuar...", ConsoleColor.Gray);
    Console.ReadKey(true);
}

protected void PainelNoWait(Action conteudo, string titulo, int larguraMin = 70, int alturaMin = 16)
{
    Console.Clear();
    DesenharCaixaCentral(larguraMin, alturaMin, ConsoleColor.DarkGray, ConsoleColor.Black);

    // anima o título centralizado
    EscreverCentralAnimado(titulo, 1, ConsoleColor.Yellow, invert: true, relativoAoConteudo: false);

    // linha de divisória mais lenta
    LinhaAtAnimada(_top + 2, ConsoleColor.DarkGray, 20);

    _cursorY = _top + 3;
    conteudo?.Invoke();
}



        protected string LerEntradaPainel(string prompt)
        {
            int inner = _width - 2;
            string p = prompt.Length > inner ? prompt[..inner] : prompt;
            Console.SetCursorPosition(_left + 1, _cursorY);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(PadRightSafe(p, inner));
            Console.ResetColor();
            Console.CursorVisible = true;
            Console.SetCursorPosition(_left + 1 + p.Length, _cursorY);
            string? resp = Console.ReadLine();
            Console.CursorVisible = false;
            _cursorY++;
            return resp ?? "";
        }

        private static void DesenharCaixaCentral(int width, int height, ConsoleColor borda, ConsoleColor fundo)
        {
            width = Math.Min(width, Math.Max(48, Console.WindowWidth - 4));
            height = Math.Min(height, Math.Max(12, Console.WindowHeight - 4));
            _width = width;
            _height = height;
            _left = (Console.WindowWidth - width) / 2;
            _top = (Console.WindowHeight - height) / 2;

            Console.BackgroundColor = fundo;
            Console.ForegroundColor = borda;
            Console.SetCursorPosition(_left, _top);
            Console.Write("╔" + new string('═', width - 2) + "╗");
            for (int y = 1; y < height - 1; y++)
            {
                Console.SetCursorPosition(_left, _top + y);
                Console.Write("║" + new string(' ', width - 2) + "║");
            }
            Console.SetCursorPosition(_left, _top + height - 1);
            Console.Write("╚" + new string('═', width - 2) + "╝");
            Console.ResetColor();
        }

        protected static void EscreverCentral(
            string texto,
            int yOffset,
            ConsoleColor? color = null,
            bool invert = false,
            bool relativoAoConteudo = true)
        {
            int yBase = relativoAoConteudo ? (_top + 2) : _top;
            int y = yBase + yOffset;
            int x = _left + (_width - texto.Length) / 2;
            x = Math.Max(_left + 1, x);

            if (invert) Console.BackgroundColor = ConsoleColor.Black;
            if (color.HasValue) Console.ForegroundColor = color.Value;

            Console.SetCursorPosition(x, y);
            Console.Write(texto);
            Console.ResetColor();
        }

        protected static void EscreverLinha(string texto, ConsoleColor? color = null)
        {
            int inner = _width - 2;
            string line = texto.Length > inner ? texto[..inner] : PadRightSafe(texto, inner);
            if (color.HasValue) Console.ForegroundColor = color.Value;
            Console.SetCursorPosition(_left + 1, _cursorY++);
            Console.Write(line);
            Console.ResetColor();
        }

        protected static void Linha(ConsoleColor? color = null)
        {
            if (color.HasValue) Console.ForegroundColor = color.Value;
            Console.SetCursorPosition(_left + 1, _top + 3);
            for (int i = 0; i < _width - 2; i++)
            {
                Console.Write('─');
                Thread.Sleep(LinhaAnimMs);
            }
            Console.ResetColor();
        }

        protected void Typewriter(string texto, int velocidadeMs)
        {
            int inner = _width - 2;
            int col = 0;
            foreach (char c in texto)
            {
                if (col == 0)
                    Console.SetCursorPosition(_left + 1, _cursorY);
                Console.Write(c);
                Thread.Sleep(velocidadeMs);
                col++;
                if (col >= inner || c == '\n')
                {
                    _cursorY++;
                    col = 0;
                }
            }
            _cursorY++;
        }

        protected static void SpinnerMensagem(string msg, int passos = 20)
        {
            var frames = new[] { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
            int inner = _width - 2;
            string baseTxt = (msg.Length > inner - 4) ? msg[..(inner - 4)] : msg;
            for (int i = 0; i < passos; i++)
            {
                Console.SetCursorPosition(_left + 1, _cursorY);
                string frame = frames[i % frames.Length];
                Console.Write($"{frame} {baseTxt}".PadRight(inner));
                Thread.Sleep(60);
            }
            _cursorY++;
        }

        protected static void BarraProgresso(string titulo, int atual, int total)
        {
            int inner = _width - 2;
            int barW = Math.Max(10, inner - titulo.Length - 6);
            double ratio = total == 0 ? 0 : Math.Clamp((double)atual / total, 0, 1);
            int fill = (int)Math.Round(barW * ratio);

            Console.SetCursorPosition(_left + 1, _cursorY++);
            Console.Write($"{titulo} ".PadRight(inner - barW - 2));
            Console.Write("[" + new string('■', fill) + new string(' ', barW - fill) + $"] {(int)(ratio * 100)}%");
        }

        protected static void TransicaoFadeOutIn()
        {
            for (int i = 0; i < 2; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Thread.Sleep(FadeMs);
                Console.ResetColor();
                Thread.Sleep(FadeMs);
            }
        }

        protected static string CapturarSaidaConsole(Action acao)
        {
            var original = Console.Out;
            using var sw = new StringWriter();
            try { Console.SetOut(sw); acao(); }
            finally { Console.SetOut(original); }
            return sw.ToString();
        }

        protected void DigitarTexto(string texto, int velocidadeMs = 30) => Typewriter(texto, velocidadeMs);

        protected void AtualizarProgresso(string titulo, int atual, int total, int delayMs = 300)
        {
            BarraProgresso(titulo, atual, total);
            Thread.Sleep(delayMs);
        }

        protected void StatusJogador()
        {
            EscreverLinha($"👤 {Jogador.Nome} | ❤️ {Jogador.Vida}/{Jogador.VidaMaxima} | ⚔️ {Jogador.Ataque} 🛡️ {Jogador.Defesa} | 💰 {Jogador.Ouro}");
        }

        public void MostrarStatus()
        {
            Painel(() =>
            {
                EscreverLinha($"📍 Local: {Local}");
                EscreverLinha($"📝 {Descricao}");
                Linha(ConsoleColor.DarkGray);
                EscreverLinha($"✅ Status: {(EstaCompleta ? "Concluída 🎉" : EstaAtiva ? "Em Andamento ⏳" : "Disponível 📌")}");
                EscreverLinha($"🎁 Recompensa: {ExperienciaRecompensa} EXP + {OuroRecompensa} Ouro");
                Linha(ConsoleColor.DarkGray);
                StatusJogador();
            }, titulo: $"MISSÃO: {Titulo}", larguraMin: 70, alturaMin: 16);
        }

        // helper
        private static string PadRightSafe(string s, int w) => (s ?? string.Empty).PadRight(w);
        // título animado (digitando um por um)
private static void EscreverCentralAnimado(
    string texto, int yOffset,
    ConsoleColor? color = null,
    bool invert = false,
    bool relativoAoConteudo = true,
    int delayMs = 25)
{
    int yBase = relativoAoConteudo ? (_top + 2) : _top;
    int y = yBase + yOffset;
    int x = _left + (_width - texto.Length) / 2;
    x = Math.Max(_left + 1, x);

    if (invert) Console.BackgroundColor = ConsoleColor.Black;
    if (color.HasValue) Console.ForegroundColor = color.Value;

    Console.SetCursorPosition(x, y);
    foreach (char c in texto)
    {
        Console.Write(c);
        Thread.Sleep(delayMs); // efeito “typewriter”
    }
    Console.ResetColor();
}

// divisória lenta com estilo
private static void LinhaAtAnimada(int y, ConsoleColor? color = null, int delayMs = 15)
{
    if (color.HasValue) Console.ForegroundColor = color.Value;
    Console.SetCursorPosition(_left + 1, y);

    for (int i = 0; i < _width - 2; i++)
    {
        Console.Write('─');
        Thread.Sleep(delayMs); // mais lento que o normal
    }
    Console.ResetColor();
}

        // desenha a linha horizontal na altura indicada (y)
        protected static void LinhaAt(int y, ConsoleColor? color = null)
        {
            if (color.HasValue) Console.ForegroundColor = color.Value;
            Console.SetCursorPosition(_left + 1, y);
            for (int i = 0; i < _width - 2; i++)
            {
                Console.Write('─');
                Thread.Sleep(LinhaAnimMs);
            }
            Console.ResetColor();
        }

    }
    
}

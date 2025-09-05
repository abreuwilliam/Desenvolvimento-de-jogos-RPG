using System;
using System.IO;
using System.Threading;
using Rpg.Classes.Abstracts;

public sealed class Combate
{
    private readonly Personagem _protagonista;
    private readonly Personagem _vilao;

    private static int _prLeft, _prTop, _prWidth, _prHeight;
    private static int _prCursorY;

    // configs
    private const int CustoReviver = 200;
    private const int FrameMs = 40;            // “fps” das animações (~25fps)
    private const int HitPauseMs = 250;        // pausa rápida ao acertar
    private const int StepMoveMs = 20;         // velocidade do projétil
    private const int CampoLargura = 62;       // largura do campo
    private const int CampoAltura = 17;       // altura do campo

    private static readonly string[] HeroAscii =
    {
        @"  ^  ",
        @" /%\/ ",
        @" / \ "
    };

    private static readonly string[] EnemyAscii =
    {
        @"  ^  ",
        @" /#\ ",
        @" / \ "
    };

    public Combate(Personagem protagonista, Personagem vilao)
    {
        _protagonista = protagonista ?? throw new ArgumentNullException(nameof(protagonista));
        _vilao = vilao ?? throw new ArgumentNullException(nameof(vilao));
    }

    public void Iniciar()
    {
        PrepararConsole();

        var audio = new AudioPlayer();
        var musicPath = Path.Combine(AppContext.BaseDirectory, "Assets", "combate.mp3");

        try
        {
            if (File.Exists(musicPath))
                audio.PlayLoop(musicPath);

            bool reiniciar;
            do
            {
                reiniciar = false;

                // zera a tela do campo
                DesenharCampo(out int left, out int top);

                // posições base dos bonecos
                var heroPos = (x: left + 6, y: top + 6);
                var enemyPos = (x: left + CampoLargura - 12, y: top + 6);

                // loop até alguém cair
                while (_protagonista.EstaVivo && _vilao.EstaVivo)
                {
                    // desenha tudo (HUD, bonecos)
                    RenderHUD(left, top);
                    DesenharBoneco(heroPos.x, heroPos.y, HeroAscii);
                    DesenharBoneco(enemyPos.x, enemyPos.y, EnemyAscii);

                    // turno do herói (com animação de projétil ->)
                    AnimarAtaqueProjeteis(heroPos, enemyPos, direcaoDireita: true);
                    _protagonista.AtacarAlvo(_vilao);
                    EfeitoDano(enemyPos);
                    Thread.Sleep(HitPauseMs);

                    if (!_vilao.EstaVivo) break;

                    // turno do vilão (<-)
                    AnimarAtaqueProjeteis(enemyPos, heroPos, direcaoDireita: false);
                    _vilao.AtacarAlvo(_protagonista);
                    EfeitoDano(heroPos);
                    Thread.Sleep(HitPauseMs);
                }

                // ===== resultado =====
                if (_protagonista.EstaVivo && !_vilao.EstaVivo)
                {
                    
                   Console.Clear();
PainelResultadoInicio("⚔️  RESULTADO DO COMBATE");

string recompensas = CapturarSaidaConsole(() => _vilao.ConcederRecompensa(_protagonista));

foreach (var linha in recompensas.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
{
    EscreverLinhaPainel(linha, ConsoleColor.Gray);
}

PainelResultadoStatus(_protagonista, _vilao);
PainelResultadoFim();

                }
                else if (!_protagonista.EstaVivo)
                {
                    int msgY1 = top + CampoAltura - 2;
                    int msgY2 = top + CampoAltura - 1;

                    Console.SetCursorPosition(left + 2, msgY1);
                    Console.Write("☠️  Você foi derrotado!                              ");
                    Console.SetCursorPosition(left + 2, msgY2);
                    Console.Write($"Reviver por {CustoReviver} ouro e reiniciar? (S/N): ");

                    var key = Console.ReadKey(true).Key;

                    LimparLinha(left + 2, msgY1, CampoLargura - 4);
                    LimparLinha(left + 2, msgY2, CampoLargura - 4);

                    if (key == ConsoleKey.S)
                    {
                        if (_protagonista.GastarOuro(CustoReviver))
                        {
                      
                            _protagonista.Reviver();
                            _protagonista.Curar(_protagonista.VidaMaxima);

                            if (!_vilao.EstaVivo)
                            {
                                _vilao.Reviver(); 
                                int alvo = (int)Math.Floor(_vilao.VidaMaxima * 0.40);
                                int precisa = Math.Max(0, alvo - _vilao.Vida);
                                if (precisa > 0) _vilao.Curar(precisa);
                            }

                            reiniciar = true; 
                        }
                        else
                        {
                            Console.Clear();
                            PainelResultadoInicio("⚔️  RESULTADO DO COMBATE");
                            EscreverLinhaPainel("❌ Ouro insuficiente para reviver.", ConsoleColor.Red);
                            PainelResultadoStatus(_protagonista, _vilao);
                            PainelResultadoFim();
                        }
                    }
                    else
                    {
                       
                        Console.Clear();
                        PainelResultadoInicio("⚔️  RESULTADO DO COMBATE");
                        EscreverLinhaPainel("👋 Fim do combate.", ConsoleColor.Gray);
                        PainelResultadoStatus(_protagonista, _vilao);
                        PainelResultadoFim();
                    }
                }

            } while (reiniciar);
        }
        finally
        {
            audio.Stop();
            audio.Dispose();
            Console.CursorVisible = true;
        }
    }

   

    private static void PrepararConsole()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        // garante espaço razoável
        try
        {
            if (Console.WindowWidth < 100) Console.SetWindowSize(Math.Max(100, Console.WindowWidth), Console.WindowHeight);
            if (Console.WindowHeight < 35) Console.SetWindowSize(Console.WindowWidth, Math.Max(35, Console.WindowHeight));
        }
        catch { /* alguns terminais não permitem redimensionar */}
    }

    private static void DesenharCampo(out int left, out int top)
    {
        left = (Console.WindowWidth - CampoLargura) / 2;
        top = (Console.WindowHeight - CampoAltura) / 2;

        // moldura
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.SetCursorPosition(left, top);
        Console.Write("┌" + new string('─', CampoLargura - 2) + "┐");
        for (int i = 1; i < CampoAltura - 1; i++)
        {
            Console.SetCursorPosition(left, top + i);
            Console.Write("│" + new string(' ', CampoLargura - 2) + "│");
        }
        Console.SetCursorPosition(left, top + CampoAltura - 1);
        Console.Write("└" + new string('─', CampoLargura - 2) + "┘");
        Console.ResetColor();
    }

    private void RenderHUD(int left, int top)
    {
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        EscreverCentral($"{_protagonista.Nome}  vs  {_vilao.Nome}", top + 1, left, CampoLargura);
        Console.ResetColor();

        
        DesenharBarra(left + 2, top + 3, CampoLargura - 4, _protagonista.Vida, _protagonista.VidaMaxima, ConsoleColor.Green);
        DesenharBarra(left + 2, top + 4, CampoLargura - 4, _vilao.Vida, _vilao.VidaMaxima, ConsoleColor.Red);

        Console.SetCursorPosition(left + 2, top + 5);
        Console.Write($"❤️ {_protagonista.Vida}/{_protagonista.VidaMaxima}".PadRight(CampoLargura / 2 - 2));
        Console.SetCursorPosition(left + CampoLargura / 2, top + 5);
        Console.Write($"💀 {_vilao.Vida}/{_vilao.VidaMaxima}".PadRight(CampoLargura / 2 - 2));
    }

    private static void DesenharBarra(int x, int y, int largura, int valor, int max, ConsoleColor cor)
    {
        largura = Math.Max(10, largura);
        double ratio = Math.Clamp(max == 0 ? 0.0 : (double)valor / max, 0.0, 1.0);
        int preenchido = (int)Math.Round((largura - 2) * ratio);

        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[" + new string(' ', largura - 2) + "]");
        Console.SetCursorPosition(x + 1, y);
        Console.ForegroundColor = cor;
        Console.Write(new string('■', preenchido));
        Console.ResetColor();
    }

    private static void DesenharBoneco(int x, int y, string[] sprite)
    {
        for (int i = 0; i < sprite.Length; i++)
        {
            Console.SetCursorPosition(x, y + i);
            Console.Write(sprite[i]);
        }
    }
    private static void AnimarAtaqueProjeteis((int x, int y) de, (int x, int y) para, bool direcaoDireita)
    {
        int startX = direcaoDireita ? de.x + 6 : de.x - 2;
        int endX = direcaoDireita ? para.x - 1 : para.x + 6;
        int y = de.y + 1;

        int passo = direcaoDireita ? 1 : -1;
        for (int x = startX; direcaoDireita ? x <= endX : x >= endX; x += passo)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(direcaoDireita ? "→" : "←");
            Thread.Sleep(StepMoveMs);
            Console.SetCursorPosition(x, y);
            Console.Write(' ');
        }
    }

    
    private static void EfeitoDano((int x, int y) pos)
    {
        var dx = new[] { 0, 1, -1, 0 };
        for (int i = 0; i < dx.Length; i++)
        {
            Console.SetCursorPosition(pos.x + dx[i], pos.y - 1);
            Console.Write("**");
            Thread.Sleep(FrameMs);
            Console.SetCursorPosition(pos.x + dx[i], pos.y - 1);
            Console.Write("  ");
        }
    }

    private static void EscreverCentral(string texto, int y, int left, int largura)
    {
        int x = left + (largura - texto.Length) / 2;
        if (x < left + 1) x = left + 1;
        Console.SetCursorPosition(x, y);
        Console.Write(texto);
    }

    private static void LimparLinha(int x, int y, int len)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(new string(' ', len));
    }

 

   private static void DesenharCaixaCentral(int width, int height, ConsoleColor borda, ConsoleColor fundo)
{
    width  = Math.Min(width,  Math.Max(40, Console.WindowWidth  - 4));
    height = Math.Min(height, Math.Max(10, Console.WindowHeight - 4));

    _prWidth  = width;
    _prHeight = height;
    _prLeft   = (Console.WindowWidth  - width)  / 2;
    _prTop    = (Console.WindowHeight - height) / 2;

    Console.BackgroundColor = fundo;
    Console.ForegroundColor = borda;

    Console.SetCursorPosition(_prLeft, _prTop);
    Console.Write("╔" + new string('═', width - 2) + "╗");
    for (int y = 1; y < height - 1; y++)
    {
        Console.SetCursorPosition(_prLeft, _prTop + y);
        Console.Write("│" + new string(' ', width - 2) + "│");
    }
    Console.SetCursorPosition(_prLeft, _prTop + height - 1);
    Console.Write("╚" + new string('═', width - 2) + "╝");

    Console.ResetColor();

    // 👉 conteúdo começa logo abaixo da divisória que ficará em _prTop + 2
    _prCursorY = _prTop + 3;
}

    private static void EscreverCentralPainel(string texto, int yOffsetFromTop, ConsoleColor? color = null, bool invert = false)
    {
        int y = _prTop + yOffsetFromTop;
        int x = _prLeft + (_prWidth - texto.Length) / 2;
        x = Math.Max(_prLeft + 1, x);

        if (invert) Console.BackgroundColor = ConsoleColor.Black;
        if (color.HasValue) Console.ForegroundColor = color.Value;

        Console.SetCursorPosition(x, y);
        Console.Write(texto);

        Console.ResetColor();
    }

    private static void EscreverLinhaPainel(string texto, ConsoleColor? color = null)
    {
        int innerWidth = _prWidth - 2;
        string line = texto.Length > innerWidth ? texto[..innerWidth] : texto.PadRight(innerWidth);

        if (color.HasValue) Console.ForegroundColor = color.Value;
        Console.SetCursorPosition(_prLeft + 1, _prCursorY++);
        Console.Write(line);
        Console.ResetColor();
    }

    private static void LinhaDivisoria(ConsoleColor? color = null)
    {
        if (color.HasValue) Console.ForegroundColor = color.Value;
        Console.SetCursorPosition(_prLeft + 1, _prTop + 3);
        Console.Write(new string('─', _prWidth - 2));
        Console.ResetColor();
    }


    private static void PainelResultadoInicio(string subtitulo)
{
    Console.Clear();
    int largura = Math.Max(56, subtitulo.Length + 14);

    DesenharCaixaCentral(largura, height: 13, borda: ConsoleColor.DarkGray, fundo: ConsoleColor.Black);

    EscreverCentralPainel("RESULTADO DO COMBATE", yOffsetFromTop: 1, color: ConsoleColor.Yellow, invert: true);
    EscreverCentralPainel(subtitulo,                yOffsetFromTop: 2, color: ConsoleColor.Cyan);

    // 👉 agora a divisória fica imediatamente após o subtítulo (sem “buraco”)
    LinhaDivisoriaAt(_prTop + 2, ConsoleColor.DarkGray /*, delayMs: 12 se quiser animar */);
}


    private void PainelResultadoStatus(Personagem heroi, Personagem inimigo)
    {
        EscreverLinhaPainel($"👤 {heroi.Nome}".PadRight(_prWidth / 2 - 2) + $"👹 {inimigo.Nome}");
        EscreverLinhaPainel($"❤️ {heroi.Vida}/{heroi.VidaMaxima}".PadRight(_prWidth / 2 - 2) + $"💀 {inimigo.Vida}/{inimigo.VidaMaxima}");
        EscreverLinhaPainel($"💰 Ouro: {heroi.Ouro}");
        LinhaDivisoria(ConsoleColor.DarkGray);
    }

   private static void PainelResultadoFim()
{
    // desenha uma linha onde o cursor está e então o rodapé
    LinhaDivisoriaAt(_prCursorY++, ConsoleColor.DarkGray);
    EscreverLinhaPainel("Pressione qualquer tecla para continuar...", ConsoleColor.Gray);

    Console.SetCursorPosition(_prLeft + 1, _prTop + _prHeight - 2);
    Console.ReadKey(true);
}


    private static string CapturarSaidaConsole(Action acao)
    {
        var original = Console.Out;
        using var sw = new StringWriter();
        try
        {
            Console.SetOut(sw);
            acao();
        }
        finally
        {
            Console.SetOut(original);
        }
        return sw.ToString();
    }
private static void LinhaDivisoriaAt(int y, ConsoleColor? color = null, int delayMs = 0)
{
    if (color.HasValue) Console.ForegroundColor = color.Value;
    Console.SetCursorPosition(_prLeft + 1, y);
    string seg = new string('─', _prWidth - 2);

    if (delayMs <= 0)
    {
        Console.Write(seg);
    }
    else
    {
        // desenha devagar, mais estiloso
        for (int i = 0; i < seg.Length; i++)
        {
            Console.Write('─');
            Thread.Sleep(delayMs);
        }
    }
    Console.ResetColor();
}


}

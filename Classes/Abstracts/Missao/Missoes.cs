using System;
using System.IO;
using RPG.Classes.Abstracts.Personagens;


namespace Rpg.Classes.Missoes
{
    public abstract class MissaoBase
    {
        public string Id { get; protected set; }
        public string Titulo { get; protected set; }
        public string Descricao { get; protected set; } = string.Empty;
        public string Local { get; protected set; }
        public bool EstaCompleta { get; protected set; }
        public bool EstaAtiva { get; protected set; }
        public int ExperienciaRecompensa { get; protected set; }
        public int OuroRecompensa { get; protected set; }
        public Personagem Jogador { get; protected set; }

        private AudioPlayer? _bgm;
        private string? _bgmPath;
        private bool _bgmAtiva;

        protected MissaoBase(string titulo, string local, int expRecompensa, int ouroRecompensa, Personagem jogador)
        {
            Id = Guid.NewGuid().ToString();
            Titulo = titulo;
            Local = local;
            ExperienciaRecompensa = expRecompensa;
            OuroRecompensa = ouroRecompensa;
            Jogador = jogador;
            EstaCompleta = false;
            EstaAtiva = false;
        }

        public abstract void IniciarMissao(Personagem jogador);
        protected abstract void ExecutarObjetivos(Personagem jogador);
        protected abstract bool VerificarConclusao();

        protected virtual string ContarHistoria()
            => $"Você avança por {Local}. O vento traz sussurros estranhos...";

        public void ExecutarMissao(Personagem jogador)
        {
        
            _bgmPath = Path.Combine(AppContext.BaseDirectory, "Assets", "missao.mp3");
            _bgm = new AudioPlayer();
            if (File.Exists(_bgmPath))
            {
                _bgm.PlayLoop(_bgmPath);
                _bgmAtiva = true;
            }

            if (EstaCompleta)
            {
                Console.WriteLine($"A missão '{Titulo}' já está completa.");
                Console.WriteLine("Pressione qualquer tecla para voltar...");
                Console.ReadKey(true);
                return;
            }

            EstaAtiva = true;

            Console.Clear();
            Console.WriteLine($"======== MISSÃO: {Titulo} ========");
            Console.WriteLine($"Local: {Local}");
            Console.WriteLine();
            Console.WriteLine($"Descrição: {Descricao}");
            Console.WriteLine("-----------------------------------");

            Console.WriteLine("HISTÓRIA:");
            Console.WriteLine(ContarHistoria());
            Console.WriteLine("-----------------------------------");

            ExecutarObjetivos(jogador);

            if (VerificarConclusao())
            {
                CompletarMissao(jogador);
            }
            else
            {
                Console.WriteLine("❌ A missão falhou! Tente novamente.");
                MostrarStatusJogador();
            }

            if (_bgmAtiva)
            {
                _bgm?.Stop();
                _bgm?.Dispose();
            }
        }


        protected virtual void CompletarMissao(Personagem jogador)
        {
            EstaCompleta = true;
            EstaAtiva = false;

            jogador.AdicionarExperiencia(ExperienciaRecompensa);
            jogador.AdicionarOuro(OuroRecompensa);

            if (_bgmAtiva) _bgm?.Stop();

            string winPath = Path.Combine(AppContext.BaseDirectory, "Assets", "vitoria.mp3");
            AudioPlayer? win = null;

            if (File.Exists(winPath))
            {
                win = new AudioPlayer();
                win.PlayLoop(winPath);
            }

            Console.WriteLine();
            Console.WriteLine("🎉 MISSÃO CONCLUÍDA!");
            Console.WriteLine($"Recompensas: +{ExperienciaRecompensa} EXP, +{OuroRecompensa} Ouro");
            Console.WriteLine();
            MostrarStatusJogador();

            win?.Stop();
            win?.Dispose();
        }

        protected virtual void DarRecompensaExtra(Personagem jogador) { }

        public void MostrarStatus()
        {
            Console.Clear();
            Console.WriteLine($"----- MISSÃO: {Titulo} -----");
            Console.WriteLine($"Local: {Local}");
            Console.WriteLine($"Descrição: {Descricao}");
            Console.WriteLine($"Status: {(EstaCompleta ? "Concluída" : EstaAtiva ? "Em andamento" : "Disponível")}");
            Console.WriteLine($"Recompensa: {ExperienciaRecompensa} EXP + {OuroRecompensa} Ouro");
            Console.WriteLine("---------------------------------");
            MostrarStatusJogador();
            Console.WriteLine("\nPressione qualquer tecla...");
            Console.ReadKey(true);
        }

        protected void MostrarStatusJogador()
        {
            Console.WriteLine($"Jogador: {Jogador.Nome}");
            Console.WriteLine($"Vida: {Jogador.Vida}/{Jogador.VidaMaxima}");
            Console.WriteLine($"Ataque: {Jogador.Ataque} | Defesa: {Jogador.Defesa}");
            Console.WriteLine($"Ouro: {Jogador.Ouro}");
        }

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
    }
}

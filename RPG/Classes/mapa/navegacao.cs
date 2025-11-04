using System;
using Rpg.Classes.Personagens;
using RPG.Classes.Abstracts.Personagens;

namespace RPG.Classes.mapa
{
    
    /// Menu de navegação principal — integra Vila, Floresta e outros mapas.
    

    public class Navegacao
    {
        private readonly Personagem _heroi;
        private readonly MapaDaFloresta _MapaDaFloresta;
        private readonly Vila _vila;

        public Navegacao(Personagem heroi)
        {
            _heroi = heroi ?? throw new ArgumentNullException(nameof(heroi));
            // instâncias opcionais — elas podem manter estado próprio
            _MapaDaFloresta = new MapaDaFloresta(_heroi);
            _vila = new Vila(_heroi);
        }

        public MapaDaFloresta MapaDaFloresta => _MapaDaFloresta;


        /// Executa o loop principal de navegação.

        public void Executar()
        {
            // toca a música de fundo do mundo 
            Som.PlayLoop("mundo.mp3");

            bool sair = false;
            while (!sair)
            {
                Console.Clear();

                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("🌍 Mapa do Reino", 0, ConsoleColor.Cyan);
                    Ui.Linha();
                    Ui.Typewriter($"Herói: {_heroi.Nome} — Nível {_heroi.Nivel} • Vida {_heroi.Vida}/{_heroi.VidaMaxima} • Ouro {_heroi.Ouro}", Ui.VelocidadeTextoMs, ConsoleColor.Cyan);
                    Ui.Linha();
                    Ui.EscreverCentral("Para onde deseja ir?", 0, ConsoleColor.Cyan);
                    Console.WriteLine();
                    Ui.EscreverCentral("[1] Ir à Vila", 0, ConsoleColor.Cyan);
                    Ui.EscreverCentral("[2] Entrar na Floresta Sombria", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("[3] Consultar Status", 0, ConsoleColor.Yellow);
                    Ui.EscreverCentral("[0] Sair do Jogo", 0, ConsoleColor.DarkGray);
                }, "NAVEGAÇÃO", ConsoleColor.Cyan);

                var escolha = Ui.LerEntradaPainel("Escolha: ").Trim();

                switch (escolha)
                {
                    case "1":
                        // abre a Vila — empilha tema e retorna ao menu quando sair
                        using (Som.Push("vila.mp3"))
                        {
                            // instância Vila já criada no construtor; a classe Vila faz o seu loop interno
                            _vila.GetType(); 
                            // recria instância ao entrar caso queira resetar estado:
                            var vila = new Vila(_heroi);
                        }
                        break;

                    case "2":
                        // abre a Floresta — empilha o tema e entra
                        using (Som.Push("floresta.mp3"))
                        {
                           
                            var floresta = new MapaDaFloresta(_heroi);
                            floresta.Iniciar();
                        }
                        break;

                    case "3":
                        MostrarStatusRapido();
                        break;

                    case "0":
                        Ui.Painel(() =>
                        {
                            Ui.EscreverCentral("Deseja realmente sair do jogo? (S/N)", 0, ConsoleColor.Yellow);
                        }, "SAIR", ConsoleColor.Yellow);

                        var conf = Ui.LerEntradaPainel("Confirmar (S/N): ").Trim();
                        if (string.Equals(conf, "S", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(conf, "SIM", StringComparison.OrdinalIgnoreCase))
                        {
                            Ui.Painel(() => Ui.EscreverCentral("Encerrando... Até a próxima aventura!", 0, ConsoleColor.Yellow), "TCHAU", ConsoleColor.Yellow);
                            sair = true;
                        }
                        break;

                    default:
                        Ui.Painel(() =>
                        {
                            Ui.EscreverCentral("❌ Opção inválida. Tente novamente.", 0, ConsoleColor.Red);
                        }, "ERRO", ConsoleColor.Red);
                        Thread.Sleep(700);
                        break;
                }

                if (!sair) Ui.Pausa("Pressione ENTER para continuar...");
            }

            Som.Stop();
        }

        private void MostrarStatusRapido()
        {
            Ui.Painel(() =>
            {
                Ui.EscreverCentral("📋 Status Rápido", 0, ConsoleColor.Yellow);
                Ui.Linha();
                Ui.Typewriter($"Nome: {_heroi.Nome}", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Typewriter($"Nível: {_heroi.Nivel}", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Typewriter($"Vida: {_heroi.Vida}/{_heroi.VidaMaxima}", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Typewriter($"Ataque: {_heroi.Ataque}  Defesa: {_heroi.Defesa}", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Typewriter($"EXP: {_heroi.Experiencia}", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Typewriter($"Ouro: {_heroi.Ouro}", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                Ui.Linha();
                Ui.Typewriter("Dicas: visite a Vila para equipamentos; explore a Floresta para pistas e missões.", Ui.VelocidadeTextoMs, ConsoleColor.Cyan);
            }, "STATUS", ConsoleColor.Yellow);
        }
    }
}

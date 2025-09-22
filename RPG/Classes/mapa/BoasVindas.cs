using System;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;

namespace RPG.Mapa
{
    /// <summary>
    /// Painel de boas-vindas inicial do jogo.
    /// Design e UX compatíveis com as outras telas (Ui, Som).
    /// Uso: new BoasVindas(heroi).Executar();
    /// </summary>
    public class BoasVindas
    {
        private readonly Personagem _heroi;

        public BoasVindas(Personagem heroi)
        {
            _heroi = heroi ?? throw new ArgumentNullException(nameof(heroi));
        }

        /// <summary>
        /// Mostra a tela de boas-vindas e o menu inicial.
        /// Retorna quando o jogador escolher começar / sair (não bloqueia além do necessário).
        /// </summary>
        public void Executar()
        {
            // empilha a música do menu; ao sair do using, voltará o tema anterior
            using var _ = Som.Push("menu.mp3");

            bool sair = false;
            while (!sair)
            {
                Console.Clear();

                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("🎮 BEM-VINDO(A) AO REINO DAS LENDAS 🎮", 0, ConsoleColor.Cyan);
                    Ui.Linha();

                    Ui.Typewriter("Aqui você vai se divertir e navegar por um mundo de aventuras.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                    Ui.Typewriter("Explore a vila, enfrente perigos na floresta, descubra missões e encontre tesouros.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                    Ui.Typewriter("Cada escolha abre um caminho — sua história começa agora.", Ui.VelocidadeTextoMs, ConsoleColor.Green);

                    Ui.Linha();
                    Ui.EscreverCentral($"Herói: {_heroi.Nome}  •  Nível: {_heroi.Nivel}  •  Ouro: {_heroi.Ouro}", 0, ConsoleColor.Yellow);
                    Ui.Linha();

                    Ui.EscreverCentral("O que deseja fazer agora?", 0, ConsoleColor.Cyan);
                    Ui.EscreverCentral("[1] Começar a jornada (ir ao mapa)", 0, ConsoleColor.Cyan);
                    Ui.EscreverCentral("[2] Ver tutorial rápido", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("[3] Créditos", 0, ConsoleColor.DarkGray);
                    Ui.EscreverCentral("[0] Sair do jogo", 0, ConsoleColor.DarkRed);

                }, "BEM-VINDO", ConsoleColor.Cyan);

                string opcao = Ui.LerEntradaPainel("Escolha: ").Trim();

                switch (opcao)
                {
                    case "1":
                        // começa a jornada — sai da tela de boas-vindas
                        Ui.Painel(() =>
                        {
                            Ui.Typewriter("Ótimo! Prepare-se: grandes desafios aguardam. Boa sorte, herói!", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                        }, "PRONTOS?", ConsoleColor.Yellow);
                        return; // retorna para quem chamou (por exemplo Navegação)
                    case "2":
                        MostrarTutorial();
                        break;
                    case "3":
                        MostrarCreditos();
                        break;
                    case "0":
                        Ui.Painel(() =>
                        {
                            Ui.EscreverCentral("Tem certeza que deseja sair? (S/N)", 0, ConsoleColor.Yellow);
                        }, "SAIR", ConsoleColor.Yellow);

                        var conf = Ui.LerEntradaPainel("Confirmar (S/N): ").Trim();
                        if (string.Equals(conf, "S", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(conf, "SIM", StringComparison.OrdinalIgnoreCase))
                        {
                            Ui.Painel(() => Ui.EscreverCentral("Até a próxima aventura!", 0, ConsoleColor.Yellow), "TCHAU", ConsoleColor.Yellow);
                            Environment.Exit(0);
                        }
                        break;
                    default:
                        Ui.Painel(() =>
                        {
                            Ui.EscreverCentral("❌ Opção inválida. Tente novamente.", 0, ConsoleColor.Red);
                        }, "ERRO", ConsoleColor.Red);
                        break;
                }

                Ui.Pausa("Pressione ENTER para voltar...");
            }
        }

        private void MostrarTutorial()
        {
            Ui.Painel(() =>
            {
                Ui.EscreverCentral("📜 TUTORIAL RÁPIDO", 0, ConsoleColor.Green);
                Ui.Linha();
                Ui.Typewriter("• Navegação: use os menus numerados para escolher destinos.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Typewriter("• Combates: avance e será automaticamente engajado em batalhas por turnos.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Typewriter("• Pistas & Enigmas: explore diferentes caminhos para reunir pistas antes do enigma final.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Typewriter("• Vila: compre equipamentos, fale com NPCs e receba missões.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Typewriter("• Salve sempre que puder (se o seu sistema de save suportar).", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Linha();
                Ui.Typewriter("Dica: ouça o que os NPCs dizem — pequenas frases são pistas importantes.", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
            }, "TUTORIAL", ConsoleColor.Green);
        }

        private void MostrarCreditos()
        {
            Ui.Painel(() =>
            {
                Ui.EscreverCentral("🎬 CRÉDITOS", 0, ConsoleColor.Magenta);
                Ui.Linha();
                Ui.Typewriter("Design & Código: Sua Equipe de Desenvolvimento", Ui.VelocidadeTextoMs, ConsoleColor.Magenta);
                Ui.Typewriter("Música & Efeitos: Biblioteca de Áudio (Assets)", Ui.VelocidadeTextoMs, ConsoleColor.Magenta);
                Ui.Typewriter("Testes: Aventureiros destemidos e amigos beta", Ui.VelocidadeTextoMs, ConsoleColor.Magenta);
                Ui.Linha();
                Ui.Typewriter("Obrigado por jogar — que suas histórias sejam lendárias!", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
            }, "CRÉDITOS", ConsoleColor.Magenta);
        }
    }
}

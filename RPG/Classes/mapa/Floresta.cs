using System;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;

namespace RPG.Mapa
{
    public class Floresta
    {
        private readonly Personagem _heroi;
        private readonly Random _rand = new Random();

        public Floresta(Personagem heroi)
        {
            _heroi = heroi;
        }

        public void Iniciar()
        {
            using var _ = Som.Push("floresta.mp3");

            Console.Clear();
            Ui.Painel(() =>
            {
                Ui.EscreverCentral("🗺️ Novo Território Descoberto: Bem-vindo à Floresta Sombria 🌲", 0, ConsoleColor.Green);
                Ui.Linha();
                Ui.Typewriter("As árvores bloqueiam a luz do sol, e o vento traz sussurros de segredos antigos.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Typewriter("Cada sombra parece observar você, e o ar denso carrega o cheiro de musgo e perigo.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Ui.Linha();
                Ui.EscreverCentral("Escolha seu caminho com sabedoria.", 0, ConsoleColor.Green);
            }, "FLORESTA", ConsoleColor.Green);

            Ui.Pausa("Pressione ENTER para explorar a Floresta...");

            bool sair = false;
            while (!sair)
            {
                Console.Clear();
                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("🌲 Floresta Sombria 🌲", 0, ConsoleColor.Green);
                    Ui.Linha();
                    Ui.EscreverCentral("Escolha seu caminho:", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("[1] Seguir pela direita (árvore gigante)", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("[2] Caminho da clareira enigmática", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("[3] Trilha do lago misterioso", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("[4] Caminho iluminado entre árvores antigas", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("[5] Caminho da caverna sombria", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("[6] Voltar para a Vila", 0, ConsoleColor.Yellow);
                    Ui.EscreverCentral("[0] Sair da Floresta", 0, ConsoleColor.DarkGray);
                }, "FLORESTA", ConsoleColor.Green);

                string escolha = Ui.LerEntradaPainel("Escolha: ");

                switch (escolha)
                {
                    case "1": IniciarCaminho("Lobo Sombrio", "Árvore Gigante"); break;
                    case "2": IniciarCaminho("Onça Pintada", "Clareira"); break;
                    case "3": IniciarCaminho("Planta Devora-Almas", "Lago Misterioso"); break;
                    case "4": IniciarCaminho("Urso Pardo", "Caminho Iluminado"); break;
                    case "5": IniciarCaverna(); break;
                    case "6":
                        Ui.Painel(() =>
                        {
                            Ui.Typewriter("Você retorna à Vila, com a sensação de que deixou mistérios para trás.", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                        }, "VILA", ConsoleColor.Yellow);
                        sair = true;
                        break;
                    case "0":
                        Ui.Painel(() =>
                        {
                            Ui.Typewriter("Você decide abandonar a Floresta, mas algo lhe diz que ela ainda não terminou com você...", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                        }, "SAÍDA", ConsoleColor.Yellow);
                        sair = true;
                        break;
                    default:
                        Ui.Painel(() =>
                        {
                            Ui.Typewriter("❌ Opção inválida.", Ui.VelocidadeTextoMs, ConsoleColor.Red);
                        }, "ERRO", ConsoleColor.Red);
                        break;
                }

                if (!sair)
                    Ui.Pausa("Pressione ENTER para continuar...");
            }
        }

        private void IniciarCaminho(string inimigoNome, string caminho)
        {
            string[] descricoes = caminho switch
            {
                "Árvore Gigante" => new string[]
                {
                    "A sombra da árvore gigante cobre o chão, e você sente o cheiro de folhas úmidas.",
                    "O vento faz as folhas sussurrarem segredos antigos.",
                    "Galhos baixos quase tocam seu rosto enquanto você avança."
                },
                "Clareira" => new string[]
                {
                    "A luz do sol invade a clareira, mas há algo estranho no ar.",
                    "Flores incomuns cercam a clareira, e o silêncio é quase ensurdecedor.",
                    "Você percebe símbolos gravados no solo, como um aviso antigo."
                },
                "Lago Misterioso" => new string[]
                {
                    "A água do lago reflete uma luz prateada, mas algo se mexe sob a superfície.",
                    "O som de pequenas ondas ecoa, criando uma sensação de alerta.",
                    "Plantas estranhas crescem ao redor da margem, algumas parecem se mover."
                },
                "Caminho Iluminado" => new string[]
                {
                    "Raios de sol atravessam as copas das árvores, iluminando o caminho.",
                    "O cheiro de terra molhada e flores é reconfortante, mas há perigo à espreita.",
                    "Você escuta o farfalhar de folhas e sente que está sendo observado."
                },
                _ => new string[] { "O ambiente é misterioso e silencioso, você sente um arrepio..." }
            };

            string descricao = descricoes[_rand.Next(descricoes.Length)];
            Ui.Typewriter(descricao, Ui.VelocidadeTextoMs, ConsoleColor.Green);

            Ui.Pausa("Você segue adiante... pressione ENTER para continuar.");
            Ui.Typewriter($"De repente, um {inimigoNome} surge diante de você!", Ui.VelocidadeTextoMs, ConsoleColor.Red);

            var inimigo = CriarInimigoPorNome(inimigoNome);

            var combate = new Combate(_heroi, inimigo);
            combate.Iniciar();

            if (_heroi.EstaVivo)
                ExibirEnigma(caminho);
            else
                Ui.Typewriter("Ferido, você precisa se retirar e recuperar forças.", Ui.VelocidadeTextoMs, ConsoleColor.DarkRed);
        }

        private void IniciarCaverna()
        {
            Ui.Painel(() =>
            {
                Ui.Typewriter("Você encontra a entrada de uma caverna sombria, coberta por raízes e símbolos antigos.", Ui.VelocidadeTextoMs, ConsoleColor.DarkCyan);
                Ui.Typewriter("Um frio sobrenatural percorre sua espinha...", Ui.VelocidadeTextoMs, ConsoleColor.DarkCyan);
            }, "CAVERNA", ConsoleColor.DarkCyan);

            string[] inimigos = { "Ladrão das Sombras", "Planta Devora-Almas", "Onça Pintada" };
            string inimigoNome = inimigos[_rand.Next(inimigos.Length)];

            Ui.Typewriter($"Um {inimigoNome} aparece das sombras da caverna!", Ui.VelocidadeTextoMs, ConsoleColor.Red);

            var inimigo = CriarInimigoPorNome(inimigoNome);
            var combate = new Combate(_heroi, inimigo);
            combate.Iniciar();

            if (_heroi.EstaVivo)
            {
                Ui.Painel(() =>
                {
                    Ui.Typewriter("Você encontra inscrições antigas e moedas misteriosas.", Ui.VelocidadeTextoMs, ConsoleColor.DarkYellow);
                    Ui.Typewriter("Um amuleto pulsante brilha em verde...", Ui.VelocidadeTextoMs, ConsoleColor.DarkYellow);
                }, "CAVERNA FINAL", ConsoleColor.DarkYellow);
            }
            else
            {
                Ui.Typewriter("Ferido, você decide recuar da caverna.", Ui.VelocidadeTextoMs, ConsoleColor.DarkRed);
            }
        }

        private Personagem CriarInimigoPorNome(string nome) =>
            nome switch
            {
                "Lobo Sombrio" => new Personagem("Lobo Sombrio", 30, 8, 3),
                "Onça Pintada" => new Personagem("Onça Pintada", 40, 10, 4),
                "Planta Devora-Almas" => new Personagem("Planta Devora-Almas", 28, 7, 2),
                "Urso Pardo" => new Personagem("Urso Pardo", 60, 12, 6),
                "Ladrão das Sombras" => new Personagem("Ladrão das Sombras", 35, 9, 4),
                _ => new Personagem(nome, 30, 6, 3)
            };

        private void ExibirEnigma(string caminho)
        {
            switch (caminho)
            {
                case "Árvore Gigante":
                    Ui.Painel(() =>
                    {
                        Ui.Typewriter("Um enigma surge na casca da árvore: 'O que nunca volta, mas sempre avança?'", Ui.VelocidadeTextoMs, ConsoleColor.DarkCyan);
                        string resposta = Ui.LerEntradaPainel("Digite sua resposta: ").Trim().ToLower();
                        if (resposta.Contains("tempo"))
                            Ui.Typewriter("Você encontrou moedas antigas!", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                        else
                            Ui.Typewriter("O enigma permanece indecifrável.", Ui.VelocidadeTextoMs, ConsoleColor.Red);
                    }, "ENIGMA", ConsoleColor.DarkCyan);
                    break;

                case "Clareira":
                    Ui.Painel(() =>
                    {
                        Ui.Typewriter("Inscrição na clareira: 'A chave do amanhã está em três letras de hoje.'", Ui.VelocidadeTextoMs, ConsoleColor.DarkCyan);
                        string resposta = Ui.LerEntradaPainel("Digite sua resposta: ").Trim().ToLower();
                        if (resposta == "sol")
                            Ui.Typewriter("Você encontrou a Adaga da Lua!", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                        else
                            Ui.Typewriter("O enigma permanece indecifrável.", Ui.VelocidadeTextoMs, ConsoleColor.Red);
                    }, "ENIGMA", ConsoleColor.DarkCyan);
                    break;

                case "Lago Misterioso":
                    Ui.Painel(() =>
                    {
                        Ui.Typewriter("Inscrição no lago: 'O que é leve como uma pena, mas nenhum homem pode segurar por muito tempo?'", Ui.VelocidadeTextoMs, ConsoleColor.DarkCyan);
                        string resposta = Ui.LerEntradaPainel("Digite sua resposta: ").Trim().ToLower();
                        if (resposta.Contains("resp"))
                            Ui.Typewriter("Você encontrou o Elixir da Respiração!", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                        else
                            Ui.Typewriter("O enigma permanece indecifrável.", Ui.VelocidadeTextoMs, ConsoleColor.Red);
                    }, "ENIGMA", ConsoleColor.DarkCyan);
                    break;

                case "Caminho Iluminado":
                    Ui.Painel(() =>
                    {
                        Ui.Typewriter("Símbolos gravados: 'A coragem não é ausência de medo, mas a decisão de avançar.'", Ui.VelocidadeTextoMs, ConsoleColor.DarkCyan);
                        Ui.Typewriter("Você encontra um medalhão antigo!", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                    }, "ENIGMA", ConsoleColor.DarkCyan);
                    break;
            }
        }
    }
}

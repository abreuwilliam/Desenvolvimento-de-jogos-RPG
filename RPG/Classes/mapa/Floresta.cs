using System;
using System.Collections.Generic;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Missoes;
using Rpg.Classes.Personagens;

namespace RPG.Mapa
{
    public class Floresta
    {
        private readonly Personagem _heroi;
        private readonly Random _rand = new Random();

        // pistas coletadas atualmente (persistência temporária neste objeto)
        private readonly List<string> _pistas = new();
        // mapeamento caminho -> pista (texto curto)
        private readonly Dictionary<string, string> _pistasPorCaminho = new()
        {
            { "Árvore Gigante",    "Não sou um ser vivo, mas respiro e tenho uma boca." },
            { "Clareira",          "Tenho um corpo, mas não tenho braços nem pernas." },
            { "Lago Misterioso",   "Quando me alimento, cresço. Quando me negligenciam, morro." },
            { "Caminho Iluminado", "Eu posso te dar calor e conforto, mas também posso te destruir." }
        };

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
                    Ui.EscreverCentral("[5] Caverna Sombria", 0, ConsoleColor.Cyan);
                    Ui.EscreverCentral("[0] Sair da Floresta", 0, ConsoleColor.DarkGray);
                }, "FLORESTA", ConsoleColor.Green);

                string escolha = Ui.LerEntradaPainel("Escolha: ").Trim();

                switch (escolha)
                {
                    case "1": IniciarCaminho("Lobo Sombrio", "Árvore Gigante"); break;
                    case "2": IniciarCaminho("Onça Pintada", "Clareira"); break;
                    case "3": IniciarCaminho("Planta Devora-Almas", "Lago Misterioso"); break;
                    case "4": IniciarCaminho("Urso Pardo", "Caminho Iluminado"); break;
                    case "5": TentarResolverEnigmaFinal(); break;
                    case "6":
                        Ui.Painel(() =>
                        {
                            Ui.Typewriter("Você retorna à Vila, com a sensação de que deixou mistérios para trás.", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                            MissaoBase missao = new MissaoCavernaPerdida(_heroi);
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


// para o BGM atual (usa Stop que já existe)
Som.Stop();

try
{
    var combate = new Combate(_heroi, inimigo);
    combate.Iniciar();
}
finally
{
    // retoma o BGM do início — ajuste o nome do arquivo se necessário
    Som.PlayLoop("floresta.mp3");
}


            if (_heroi.EstaVivo)
            {
                // adiciona pista se ainda não coletada
                if (_pistasPorCaminho.TryGetValue(caminho, out var pista) && !_pistas.Contains(pista))
                {
                    _pistas.Add(pista);
                    Ui.Painel(() =>
                    {
                        Ui.Typewriter("Ao explorar o local, você encontra uma pista gravada:", Ui.VelocidadeTextoMs, ConsoleColor.Cyan);
                        Ui.Typewriter($"\"{pista}\"", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                        Ui.Typewriter($"Pistas coletadas: {_pistas.Count}/4", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                    }, "PISTA", ConsoleColor.Cyan);
                }
                else
                {
                    Ui.Typewriter("Você já havia vasculhado essa área — não encontra nada novo.", Ui.VelocidadeTextoMs, ConsoleColor.DarkGray);
                }
            }
            else
            {
                Ui.Typewriter("Ferido, você precisa se retirar e recuperar forças.", Ui.VelocidadeTextoMs, ConsoleColor.DarkRed);
            }
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
            
            

        private void TentarResolverEnigmaFinal()
        {
            Ui.Painel(() =>
            {
                Ui.Typewriter("Você se posiciona em um círculo de pedras onde os quatro caminhos parecem convergir.", Ui.VelocidadeTextoMs, ConsoleColor.Cyan);
            }, "ENIGMA FINAL", ConsoleColor.Cyan);

            // verifica se já reuniu as 4 pistas
            if (_pistas.Count < 4)
            {
                Ui.Painel(() =>
                {
                    Ui.Typewriter($"Ainda faltam pistas. Você tem {_pistas.Count}/4 pistas coletadas.", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                    Ui.Typewriter("Volte aos caminhos e colete todas as pistas antes de tentar resolver o enigma final.", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                }, "ENIGMA", ConsoleColor.DarkYellow);
                return;
            }

            // mostra as pistas coletadas (opcional)
            Ui.Painel(() =>
            {
                Ui.Typewriter("As pistas reunidas:", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                int i = 1;
                foreach (var p in _pistas)
                {
                    Ui.Typewriter($"{i++}. {p}", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                }
                Ui.Linha();
                Ui.Typewriter("No centro encontra-se uma inscrição final:", Ui.VelocidadeTextoMs, ConsoleColor.Cyan);
                Ui.Typewriter("'Se você me abraçar, terá calor. Que sou eu?'", Ui.VelocidadeTextoMs, ConsoleColor.Magenta);
            }, "ENIGMA FINAL", ConsoleColor.Magenta);

            // leitura da resposta
            string resposta = Ui.LerEntradaPainel("Digite sua resposta: ").Trim().ToLower();

            // lógica de verificação — aceite variações simples
            bool acertou = resposta.Contains("tempo") || resposta == "tempo" ||
                           (resposta.Contains("sol") && resposta.Contains("tre")); // exemplo: aceita "sol" e "tre" como combinação (ajuste conforme quiser)

            if (acertou)
            {
                Ui.Painel(() =>
                {
                    Ui.Typewriter("As pedras se iluminam! Você decifrou o enigma.", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                    Ui.Typewriter("Uma passagem secreta se abre... uma nova missão se inicia.", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                }, "SUCESSO", ConsoleColor.Yellow);


                MissaoBase missao = new MissaoCavernaPerdida(_heroi);
                missao.ExecutarMissao(_heroi);
            }
            else
            {
                Ui.Painel(() =>
                {
                    Ui.Typewriter("A inscrição permanece muda. Você não decifrou o enigma.", Ui.VelocidadeTextoMs, ConsoleColor.Red);
                    Ui.Typewriter("Como punição, sente-se tonto e perde algumas moedas.", Ui.VelocidadeTextoMs, ConsoleColor.DarkRed);
                }, "FALHA", ConsoleColor.Red);

                // penalidade leve (opcional)
                int perda = Math.Min(10, _heroi.Ouro);
                if (perda > 0)
                {
                    _heroi.GastarOuro(perda);
                }
            }
        }
    }
}

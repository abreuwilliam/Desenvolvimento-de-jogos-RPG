using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Rpg.Classes.Personagens;
using Rpg.Classes.Itens;
using RPG.Classes.Abstracts.Personagens;

namespace RPG.Classes.mapa
{
    public class Vila
    {
        public VilaBar Bar { get; private set; }
        public LojaDeArmas LojaDeArmas { get; private set; }
        public LojaDeItens LojaDeItens { get; private set; } 
        public Ansiao Ansiao { get; private set; }

        private readonly Personagem _heroi;

        public Vila(Personagem heroi)
        {
            _heroi = heroi;

            // 🎵 BGM da Vila
            Som.PlayLoop("vila.mp3");

            bool sair = false;
            while (!sair)
            {
                Console.Clear();

                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("🏘️  Bem-vindo à Vila! 🏘️", 0, ConsoleColor.Cyan);
                    Ui.Linha();
                    Ui.Typewriter($"Aqui você pode descansar, comprar equipamentos e ouvir histórias, {_heroi.Nome}.", Ui.VelocidadeTextoMs, ConsoleColor.Cyan);
                    Console.WriteLine();
                    Ui.Linha();

                    Ui.EscreverCentral("Você está na vila. Escolha onde deseja ir:", 0, ConsoleColor.Cyan);
                    Console.WriteLine();
                    Ui.EscreverCentral("[1] Bar", 0, ConsoleColor.Cyan);
                    Ui.EscreverCentral("[2] Loja de Armas", 0, ConsoleColor.Cyan);
                    Ui.EscreverCentral("[3] Loja de Itens", 0, ConsoleColor.Cyan);     
                    Ui.EscreverCentral("[4] Ancião", 0, ConsoleColor.Cyan);           
                    Ui.EscreverCentral("[5] Castelo do Rei", 0, ConsoleColor.Cyan);   
                    Ui.EscreverCentral("[6] Ver Inventário", 0, ConsoleColor.Cyan);   
                    Ui.EscreverCentral("[0] Sair da Vila", 0, ConsoleColor.Cyan);
                }, "VILA", ConsoleColor.Cyan);

                var escolha = Ui.LerEntradaPainel("Escolha: ");

                switch (escolha)
                {
                    case "1":
                        {
                            using var _ = Som.Push("bar.mp3"); 
                            Bar = new VilaBar(_heroi);
                            break;
                        }

                    case "2":
                        {
                            using var _ = Som.Push("loja.mp3"); 
                            LojaDeArmas = new LojaDeArmas(_heroi);
                            break;
                        }

                    case "3": 
                        {
                            using var _ = Som.Push("loja.mp3"); 
                            LojaDeItens = new LojaDeItens(_heroi);
                            break;
                        }

                    case "4": 
                        {
                            using var _ = Som.Push("ansiao.mp3"); 
                            Ansiao = new Ansiao(_heroi);
                            Ui.Pausa("Pressione ENTER para voltar à vila...");
                            break;
                        }

                    case "5": 
                        {
                            using var _ = Som.Push("castelo.mp3");
                            CasteloDoRei(_heroi);
                            Ui.Pausa("Pressione ENTER para voltar à vila...");
                            break;
                        }

                    case "6": 
                        {
                            MostrarInventario();
                            break; 
                        }

                    case "0":
                        {
                            Ui.Painel(() =>
                            {
                                Ui.EscreverCentral("Saindo da vila...", 0, ConsoleColor.Yellow);
                            }, "SAIR", ConsoleColor.Yellow);
                            sair = true;
                            break;
                        }

                    default:
                        {
                            Ui.Painel(() =>
                            {
                                Ui.EscreverCentral("❌ Opção inválida. Tente novamente.", 0, ConsoleColor.Red);
                            }, "ERRO", ConsoleColor.Red);
                            Thread.Sleep(800);
                            break;
                        }
                }
            }
            Som.Stop();
        }

        
        private void MostrarInventario()
        {
            string escolha = "";
            while (escolha != "0")
            {
                Console.Clear();
                var itensNoInventario = _heroi.Inventario.ObterItens();

                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("🎒 INVENTÁRIO 🎒", 0, ConsoleColor.Yellow);
                    Ui.Linha();
                    if (itensNoInventario.Count == 0)
                    {
                        Ui.EscreverCentral("Seu inventário está vazio.", 0, ConsoleColor.Gray);
                    }
                    else
                    {
                        int i = 1;
                        foreach (var par in itensNoInventario)
                        {
                            var item = par.Key;
                            var quantidade = par.Value;
                            Ui.EscreverCentral($"[{i}] {item.Nome} (x{quantidade}) - {item.Descricao}", 0, ConsoleColor.Yellow);
                            i++;
                        }
                    }
                    Console.WriteLine();
                    Ui.EscreverCentral("[0] Voltar para a Vila", 0, ConsoleColor.DarkGray);
                }, "ITENS", ConsoleColor.Yellow);

                escolha = Ui.LerEntradaPainel("Digite o número do item para usar ou 0 para voltar: ");

                if (int.TryParse(escolha, out int index) && index > 0 && index <= itensNoInventario.Count)
                {
                   
                    Item itemSelecionado = itensNoInventario.Keys.ElementAt(index - 1);
                    _heroi.UsarItem(itemSelecionado); 
                    Ui.Pausa("Pressione ENTER para continuar...");
                }
                else if (escolha != "0")
                {
                    Ui.Painel(() => Ui.EscreverCentral("❌ Opção inválida.", 0, ConsoleColor.Red), "ERRO", ConsoleColor.Red);
                    Ui.Pausa();
                }
            }
        }

        private static void CasteloDoRei(Personagem heroi)
        {
            if (heroi.Experiencia >= 1000)
            {
                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("👑 Salão do Trono 👑", 0, ConsoleColor.Yellow);
                    Ui.Linha();
                    Ui.Typewriter("O rei observa você com atenção e fala em voz firme:", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                    Console.WriteLine();

                    Ui.Typewriter($"“Estou sabendo de suas aventuras e conquistas, bravo herói {heroi.Nome}.", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                    Ui.Typewriter("Se está tentando resgatar minha filha, saiba que sua coragem será recompensada.”", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                    Console.WriteLine();
                    Ui.Linha();

                    Ui.EscreverCentral("RECOMPENSAS", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("• Ouro e joias", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("• Título de nobreza", 0, ConsoleColor.Green);
                    Ui.EscreverCentral("• Terras e um exército para proteger seu domínio", 0, ConsoleColor.Green);
                    Ui.Linha();

                    Ui.EscreverCentral("PERIGO", 0, ConsoleColor.Red);
                    Ui.Typewriter("“O castelo da princesa é protegido por um dragão muito poderoso.", Ui.VelocidadeTextoMs, ConsoleColor.Red);
                    Ui.Typewriter("Muitos já tentaram, nenhum conseguiu. Traga minha filha de volta.”", Ui.VelocidadeTextoMs, ConsoleColor.Red);
                    Console.WriteLine();
                    Ui.Linha();

                    Ui.Typewriter("Você deixa o castelo determinado a seguir rumo ao norte: montanhas, floresta mística,", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                    Ui.Typewriter("um mar com piratas e monstros, e um longo deserto separam você do objetivo.", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                }, "CASTELO DO REI", ConsoleColor.Yellow);
            }
            else
            {
                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("⚠️ Você ainda não tem experiência suficiente para falar com o rei.", 0, ConsoleColor.Red);
                    Ui.EscreverCentral("Volte quando estiver mais forte.", 0, ConsoleColor.Red);
                }, "ACESSO NEGADO", ConsoleColor.Red);
            }
        }
    }

    public class VilaBar
    {
        private readonly Personagem _heroi;

        public VilaBar(Personagem heroi)
        {
            _heroi = heroi;

            bool sair = false;
            while (!sair)
            {
                Console.Clear();
                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("Bem-vindo ao Bar da Vila!", 0, ConsoleColor.Magenta);
                    Ui.Linha();
                    Ui.Typewriter("Lanternas penduradas, mesas de madeira rústica e um Guerreiro ao fundo.", Ui.VelocidadeTextoMs, ConsoleColor.Magenta);
                    Console.WriteLine();
                    Ui.Linha();
                    Ui.EscreverCentral("O que você deseja fazer?", 0, ConsoleColor.Magenta);
                    Console.WriteLine();
                    Ui.EscreverCentral("[1] Beber", 0, ConsoleColor.Magenta);
                    Ui.EscreverCentral("[2] Falar com o guerreiro", 0, ConsoleColor.Magenta);
                    Ui.EscreverCentral("[3] Sair do bar", 0, ConsoleColor.Magenta);
                }, "BAR DA VILA", ConsoleColor.Magenta);

                var opcao = Ui.LerEntradaPainel("Escolha: ").ToLower();

                if (Ui.IsOneOf(opcao, "1", "b", "beber"))
                {
                    Beber();
                }
                else if (Ui.IsOneOf(opcao, "2", "g", "guerreiro", "falar"))
                {
                    FalarComGuerreiro();
                }
                else if (Ui.IsOneOf(opcao, "3", "s", "sair"))
                {
                    Ui.Painel(() => Ui.EscreverCentral("Você se levanta, agradece e sai do bar.", 0, ConsoleColor.Magenta), "SAINDO", ConsoleColor.Magenta);
                    sair = true;
                }
                else
                {
                    Ui.Painel(() => Ui.EscreverCentral("❌ Opção inválida. Tente novamente.", 0, ConsoleColor.Red), "ERRO", ConsoleColor.Red);
                }

                if (!sair) Ui.Pausa("Pressione ENTER para continuar...");
            }
        }

        private void Beber()
        {
            Ui.Painel(() =>
            {
                Ui.Typewriter("Você pede uma bebida refrescante e sente suas energias renovadas.", Ui.VelocidadeTextoMs, ConsoleColor.Magenta);
                Console.WriteLine();
                Ui.EscreverCentral($"Seu ouro: {_heroi.Ouro}", 0, ConsoleColor.Yellow);

                if (_heroi.Ouro >= 500)
                {
                    _heroi.Ouro -= 500;
                    Ui.EscreverCentral("Você pagou 500 de ouro pela bebida.", 0, ConsoleColor.Yellow);

                    if (_heroi.Vida < _heroi.VidaMaxima)
                    {
                        _heroi.Curar(20);
                        Ui.EscreverCentral("Sua vida foi restaurada em 20 pontos!", 0, ConsoleColor.Green);
                    }
                    else
                    {
                        Ui.EscreverCentral("Sua vida já está no máximo. Aproveite a bebida!", 0, ConsoleColor.Cyan);
                    }
                }
                else
                {
                    Ui.EscreverCentral("Você não tem ouro suficiente para pagar pela bebida.", 0, ConsoleColor.Red);
                }
            }, "BEBIDA", ConsoleColor.Magenta);
        }

        // História longa, lenta, com cores e usando _heroi.Nome + 🎵 música de diálogo
        private void FalarComGuerreiro()
        {
            int slow = Math.Min(65, Ui.VelocidadeTextoMs + 25);
            string nome = string.IsNullOrWhiteSpace(_heroi.Nome) ? "Herói" : _heroi.Nome;

            // 🎵 Tema mais calmo para o diálogo (empilhado)
            using var _ = Som.Push("dialogo_guerreiro.mp3");

            // Abertura
            Ui.Painel(() =>
            {
                Ui.EscreverCentral("✦ O GUERREIRO VETERANO ✦", 0, ConsoleColor.Magenta);
                Ui.Linha();
                Ui.Typewriter("O velho guerreiro se aproxima e começa a contar uma história épica de suas aventuras passadas.", slow, ConsoleColor.Green);
                Ui.Typewriter("Ele fala sobre batalhas lendárias, tesouros escondidos e criaturas místicas.", slow, ConsoleColor.Green);
                Ui.Typewriter("Você ouve atentamente, fascinado pelas histórias do velho guerreiro.", slow, ConsoleColor.Green);
                Ui.Typewriter("Ele conta a historia de uma princesa que esta presa em um castelo muito distante com um dragão.", slow, ConsoleColor.Green);
                Ui.Typewriter("Ele diz que se voce conseguir resgata-la o rei ira te recompensar com ouro e joias.", slow, ConsoleColor.Green);
                Ui.Typewriter($"{nome}: Caminhei muito até aqui. Estou interessado o senhor conhece a estrada até o castelo da princesa.", slow, ConsoleColor.Cyan);
                Ui.Typewriter("Veterano: Conheço a estrada, os atalhos que matam e o preço de escolher o atalho errado.", slow, ConsoleColor.Yellow);
                Ui.Typewriter($"{nome}: Então me conte. O que me aguarda?", slow, ConsoleColor.Cyan);
                Ui.Typewriter("Veterano: Montanhas que sussurram avalanches, uma floresta orgulhosa, um mar que cobra histórias e um deserto que rouba memórias.", slow, ConsoleColor.Yellow);
                Ui.Typewriter("Veterano: Pergunte. Hoje, a sorte quer que eu fale… e você escute.", slow, ConsoleColor.Yellow);
            }, "ENCONTRO", ConsoleColor.Yellow);

            bool sair = false;
            bool fezPrincesa = false, fezDragao = false, fezCaminho = false, fezConselhos = false;

            while (!sair)
            {
                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("O que deseja perguntar?", 0, ConsoleColor.Cyan);
                    Console.WriteLine();
                    Ui.EscreverCentral("[1] Quem é a princesa e por que foi levada?", 0, ConsoleColor.Cyan);
                    Ui.EscreverCentral("[2] O dragão é realmente imortal? Como vencê-lo?", 0, ConsoleColor.Cyan);
                    Ui.EscreverCentral("[3] Qual é o caminho exato até o castelo?", 0, ConsoleColor.Cyan);
                    Ui.EscreverCentral("[4] Quero conselhos de sobrevivência.", 0, ConsoleColor.Cyan);
                    Ui.EscreverCentral("[0] Já ouvi o suficiente.", 0, ConsoleColor.DarkGray);
                }, "PERGUNTAS", ConsoleColor.Cyan);

                var op = Ui.LerEntradaPainel("Escolha: ");

                switch (op)
                {
                    case "1":
                        Ui.Painel(() =>
                        {
                            if (!fezPrincesa)
                            {
                                Ui.Typewriter($"{nome}: Quem é a princesa? E por que a levaram?", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Chamam-na de Rosa do Norte. Não só pela beleza, mas pelo perfume de coragem que deixa por onde passa.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter("Veterano: Não foi caçada: foi chamada. Dragões não obedecem a fome quando há feitiço de corte envolvido.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter($"{nome}: Chamado por quem?", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Inimigos que brindam na mesma mesa. Traição é arma mais afiada que lanças.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter("Veterano: Quando chegar às muralhas, lembre: o ferro abre portas, mas é a língua que as mantém abertas.", slow, ConsoleColor.Yellow);
                                fezPrincesa = true;
                            }
                            else
                            {
                                Ui.Typewriter("Veterano: A Rosa do Norte… e a mão invisível que a levou. Guarde isso no bolso da atenção.", slow, ConsoleColor.Yellow);
                            }
                        }, "A PRINCESA", ConsoleColor.Magenta);
                        break;

                    case "2":
                        Ui.Painel(() =>
                        {
                            if (!fezDragao)
                            {
                                Ui.Typewriter($"{nome}: Dizem que o dragão é imortal.", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Imortal é a história que contam sobre ele, não a carne.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter("Veterano: Procure a cicatriz sob a asa esquerda. As escamas não se encaixam lá como deveriam.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter($"{nome}: Um ponto fraco.", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Acerte quando ele inspirar fundo. O peito expande, a guarda abre.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter("Veterano: Flechas com resina incendiária ajudam. Óleo de pinheiro, tecido seco… o resto é coragem.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter($"{nome}: E se eu falhar?", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Então viva para tentar de novo. Heróis teimosos escrevem finais que a sorte não daria.", slow, ConsoleColor.Yellow);
                                fezDragao = true;
                            }
                            else
                            {
                                Ui.Typewriter("Veterano: Lembre da cicatriz… e do ritmo da respiração. É quando o impossível pisca.", slow, ConsoleColor.Yellow);
                            }
                        }, "O DRAGÃO", ConsoleColor.Red);
                        break;

                    case "3":
                        Ui.Painel(() =>
                        {
                            if (!fezCaminho)
                            {
                                Ui.Typewriter($"{nome}: Preciso do caminho exato.", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Norte até as Montanhas de Zorh. Evite o desfiladeiro estreito — avalanches dormem de olhos abertos.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter("Veterano: Vire a oeste na pedra com musgo vermelho. Entre na Floresta de Sylwen.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter($"{nome}: A floresta é viva?", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: É orgulhosa. Caminhe sem quebrar galhos. Se ouvir sussurros, responda com silêncio.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter("Veterano: Saindo dela, Porto de Miragal. Procure o barqueiro de capa azul: ele não cobra moeda, cobra histórias verdadeiras.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter($"{nome}: Histórias?", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Conte a sua, com falhas. A verdade compra travessias que ouro não compra.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter("Veterano: Depois, o Mar Cinzento e o Deserto de Kharim. Ande ao amanhecer e ao entardecer; ao meio-dia, cave sombra.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter($"{nome}: E então o castelo.", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Muralhas altas, portas mentirosas. Nem toda entrada leva para dentro.", slow, ConsoleColor.Yellow);
                                fezCaminho = true;
                            }
                            else
                            {
                                Ui.Typewriter("Veterano: Norte. Zorh. Sylwen. Miragal. Mar Cinzento. Kharim. Castelo.", slow, ConsoleColor.Yellow);
                            }
                        }, "O CAMINHO", ConsoleColor.Cyan);
                        break;

                    case "4":
                        Ui.Painel(() =>
                        {
                            if (!fezConselhos)
                            {
                                Ui.Typewriter($"{nome}: Que conselhos o senhor me daria?", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Leve água demais e orgulho de menos.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter("Veterano: Afie a lâmina quando a noite começar. Escreva, ao lado do fogo, os nomes de quem você confia.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter($"{nome}: E quando eu estiver com medo?", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Medo é bússola. Se aponta para a frente, ande. Se aponta para trás, descanse.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter($"{nome}: E se eu cair?", slow, ConsoleColor.Cyan);
                                Ui.Typewriter("Veterano: Levante-se devagar e escolha um motivo melhor para não cair de novo.", slow, ConsoleColor.Yellow);
                                Ui.Typewriter("Veterano: E lembre: ninguém atravessa um reino sozinho. Aceite ajuda quando ela doer seu orgulho.", slow, ConsoleColor.Yellow);
                                fezConselhos = true;
                            }
                            else
                            {
                                Ui.Typewriter("Veterano: Água antes da sede, lâmina antes da ferrugem e a palavra certa antes da briga errada.", slow, ConsoleColor.Yellow);
                            }
                        }, "CONSELHOS", ConsoleColor.Green);
                        break;

                    case "0":
                        sair = true;
                        break;

                    default:
                        Ui.Painel(() => Ui.EscreverCentral("❌ Opção inválida.", 0, ConsoleColor.Red), "ERRO", ConsoleColor.Red);
                        break;
                }

                if (!sair)
                {
                    Ui.Pausa("Pressione ENTER para continuar...");
                }
            }

            Ui.Painel(() =>
            {
                Ui.Typewriter($"{nome}: Obrigado por dividir o que muitos esconderiam.", slow, ConsoleColor.Cyan);
                Ui.Typewriter("Veterano: Obrigado por ouvir o que tantos apressados não ouvem.", slow, ConsoleColor.Yellow);
                Ui.Typewriter("Veterano: Se voltar vivo, traga uma nova história. É assim que sobrevivemos ao tempo.", slow, ConsoleColor.Yellow);
                Ui.Typewriter($"{nome}: Voltarei. Com a princesa… e com a minha história.", slow, ConsoleColor.Cyan);
                Ui.Typewriter("Veterano: Então vá. E quando o dragão respirar, respire com ele. É assim que se quebra o impossível.", slow, ConsoleColor.Yellow);
            }, "DESPEDIDA", ConsoleColor.Magenta);
        }
    }

    public class LojaDeArmas
    {
        private readonly Personagem _heroi;

        public LojaDeArmas(Personagem heroi)
        {
            _heroi = heroi;

            string opcao = "";
            while (opcao != "3")
            {
                Console.Clear();
                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("🛡️  Bem-vindo à Loja de Armas!", 0, ConsoleColor.Yellow);
                    Ui.Linha();
                    Ui.Typewriter("Aqui você pode comprar equipamentos para sua aventura.", Ui.VelocidadeTextoMs, ConsoleColor.Yellow);
                    Console.WriteLine();
                    Ui.EscreverCentral($"Seu ouro: {_heroi.Ouro}", 0, ConsoleColor.Yellow);
                    Console.WriteLine();
                    Ui.EscreverCentral("[1] Ver armas", 0, ConsoleColor.Yellow);
                    Ui.EscreverCentral("[3] Sair da loja", 0, ConsoleColor.Yellow);
                }, "LOJA DE ARMAS", ConsoleColor.Yellow);

                opcao = Ui.LerEntradaPainel("Escolha: ");

                switch (opcao)
                {
                    case "1":
                        MostrarArmas(_heroi);
                        break;
                    case "3":
                        Ui.Painel(() => Ui.EscreverCentral("Volte sempre!", 0, ConsoleColor.Yellow), "SAINDO", ConsoleColor.Yellow);
                        break;
                    default:
                        Ui.Painel(() => Ui.EscreverCentral("❌ Opção inválida.", 0, ConsoleColor.Red), "ERRO", ConsoleColor.Red);
                        break;
                }
            }
        }

        private static void MostrarArmas(Personagem heroi)
        {
            var armas = new Dictionary<string, (string nome, int preco, int bonusAtaque)>
            {
                ["1"] = ("Espada Longa",        1000, 10),
                ["2"] = ("Machado de Batalha",  1200, 12),
                ["3"] = ("Arco e Flecha",        800,  8),
                ["4"] = ("Cajado Mágico",       1500, 15),
                ["5"] = ("Escudo de Ferro",      900,  0), // defesa situacional
            };

            string escolha = "";
            while (escolha != "0")
            {
                Console.Clear();
                Ui.Painel(() =>
                {
                    Ui.EscreverCentral("Lista de itens:", 0, ConsoleColor.Yellow);
                    Ui.Linha();
                    foreach (var kv in armas)
                        Ui.EscreverCentral($"{kv.Key} - {kv.Value.nome} - {kv.Value.preco} Ouro (+{kv.Value.bonusAtaque} ATQ)", 0, ConsoleColor.Yellow);

                    Console.WriteLine();
                    Ui.EscreverCentral("0 - Voltar", 0, ConsoleColor.DarkGray);
                    Console.WriteLine();
                    Ui.EscreverCentral($"Seu ouro: {heroi.Ouro}", 0, ConsoleColor.Yellow);
                }, "CATÁLOGO", ConsoleColor.Yellow);

                escolha = Ui.LerEntradaPainel("Escolha a arma que deseja comprar: ");

                if (escolha == "0") break;

                if (!armas.TryGetValue(escolha, out var arma))
                {
                    Ui.Painel(() => Ui.EscreverCentral("❌ Opção inválida.", 0, ConsoleColor.Red), "ERRO", ConsoleColor.Red);
                    Ui.Pausa("Pressione ENTER para continuar...");
                    continue;
                }

                if (heroi.Ouro < arma.preco)
                {
                    Ui.Painel(() => Ui.EscreverCentral("💰 Ouro insuficiente.", 0, ConsoleColor.Red), "AVISO", ConsoleColor.Red);
                    Ui.Pausa("Pressione ENTER para continuar...");
                    continue;
                }

                heroi.Ouro -= arma.preco;
                heroi.Ataque += arma.bonusAtaque;

                Ui.Painel(() =>
                {
                    Ui.EscreverCentral($"Você comprou {arma.nome}! (+{arma.bonusAtaque} ATQ)", 0, ConsoleColor.Green);
                    Ui.EscreverCentral($"Ouro restante: {heroi.Ouro}", 0, ConsoleColor.Yellow);
                }, "COMPRA EFETUADA", ConsoleColor.Green);

                Ui.Pausa("Pressione ENTER para continuar...");
            }
        }
    }

    public class Ansiao
    {
        private readonly Personagem _heroi;

        public Ansiao(Personagem heroi)
        {
            _heroi = heroi;

            Console.Clear();
            Ui.Painel(() =>
            {
                Ui.EscreverCentral("🌳 O Ancião da Vila 🌳", 0, ConsoleColor.Green);
                Ui.Linha();
                Ui.Typewriter("Um homem idoso, cabelos brancos e olhos sábios, aproxima-se com um sorriso sereno.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                Console.WriteLine();
                Ui.Linha();

                if (_heroi.Experiencia < 500)
                {
                    Ui.Typewriter($"“{_heroi.Nome}, sua jornada está apenas começando.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                    Ui.Typewriter("A verdadeira força vem do coração e da mente.”", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                }
                else if (_heroi.Experiencia < 1000)
                {
                    Ui.Typewriter($"“{_heroi.Nome}, vejo que já enfrentou muitos desafios.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                    Ui.Typewriter("Lembre-se: a humildade é a maior virtude do herói.”", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                }
                else
                {
                    Ui.Typewriter($"“{_heroi.Nome}, agora você é um herói de verdade, com histórias para contar.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                    Ui.Typewriter("Mesmo os poderosos precisam de descanso e reflexão.”", Ui.VelocidadeTextoMs, ConsoleColor.Green);
                }

                Console.WriteLine();
                Ui.Linha();
                Ui.Typewriter("O ancião se afasta, e você sente paz e inspiração renovadas.", Ui.VelocidadeTextoMs, ConsoleColor.Green);
            }, "ANCIÃO", ConsoleColor.Green);
        }
    }

    // ------------------------------------------------------------
    // 🎨 Utilitário de UI (com suporte a cor no Typewriter)
    // ------------------------------------------------------------
    internal static class Ui
    {
        public static int VelocidadeTextoMs = 18;

        public static void Painel(Action corpo, string titulo = "", ConsoleColor? tituloCor = null)
        {
            Console.WriteLine();
            Linha();
            if (!string.IsNullOrWhiteSpace(titulo))
                EscreverCentral(titulo.ToUpper(), 0, tituloCor ?? ConsoleColor.Gray);
            else
                EscreverCentral(" ", 0, ConsoleColor.Gray);
            Linha();
            Console.WriteLine();

            corpo?.Invoke();

            Console.WriteLine();
            Linha();
            Console.WriteLine();
        }

        public static void Linha(char c = '─')
        {
            int width = Math.Max(40, Console.WindowWidth - 2);
            Console.WriteLine(new string(c, width));
        }

        public static void EscreverCentral(string texto, int delay = 0, ConsoleColor? cor = null)
        {
            if (texto == null) texto = "";
            int width = Console.WindowWidth;
            int left = Math.Max(0, (width - texto.Length) / 2);

            var prev = Console.ForegroundColor;
            if (cor.HasValue) Console.ForegroundColor = cor.Value;

            Console.SetCursorPosition(Math.Min(left, Math.Max(0, width - Math.Max(1, texto.Length))), Console.CursorTop);
            Console.WriteLine(texto);

            if (cor.HasValue) Console.ForegroundColor = prev;

            if (delay > 0) Thread.Sleep(delay);
        }

        public static void Typewriter(string texto, int velocidadeMs, ConsoleColor? cor = null)
        {
            if (texto == null) return;

            int width = Console.WindowWidth;
            int left = Math.Max(0, (width - texto.Length) / 2);
            Console.SetCursorPosition(Math.Min(left, Math.Max(0, width - Math.Max(1, texto.Length))), Console.CursorTop);

            var prev = Console.ForegroundColor;
            if (cor.HasValue) Console.ForegroundColor = cor.Value;

            foreach (var ch in texto)
            {
                Console.Write(ch);
                Thread.Sleep(velocidadeMs);
            }
            Console.WriteLine();

            if (cor.HasValue) Console.ForegroundColor = prev;
        }

        public static string LerEntradaPainel(string prompt)
        {
            EscreverCentral(prompt, 0, ConsoleColor.DarkGray);
            Console.SetCursorPosition(0, Console.CursorTop - 1); // reposiciona para ler na mesma linha
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(prompt);

            return (Console.ReadLine() ?? "").Trim();
        }

        public static void Pausa(string mensagem = "Pressione ENTER para continuar...")
        {
            EscreverCentral(mensagem, 0, ConsoleColor.DarkGray);
            Console.ReadLine();
        }

        public static bool IsOneOf(string value, params string[] options)
            => Array.Exists(options, o => o.Equals(value, StringComparison.OrdinalIgnoreCase));
    }

    // ------------------------------------------------------------
    // 🔊 Controlador simples de BGM (usa AudioPlayer já existente)
    // - Toca arquivos da pasta Assets (ex.: "vila.mp3", "bar.mp3", etc.)
    // - Suporta empilhar temas (Push/Pop) para voltar ao anterior ao sair
    // ------------------------------------------------------------
    internal static class Som
    {
        private static AudioPlayer _player;
        private static string _currentPath;
        private static readonly Stack<string> _stack = new();

        private static void Ensure()
        {
            _player ??= new AudioPlayer();
        }

        private static string FullPath(string file)
            => Path.Combine(AppContext.BaseDirectory, "Assets", file);

        public static void PlayLoop(string fileName)
        {
            var path = FullPath(fileName);
            if (!File.Exists(path)) return;

            Ensure();
            TryStop();
            try
            {
                _player.PlayLoop(path);
                _currentPath = path;
            }
            catch
            {
                // ignora erros de áudio (sem travar o jogo)
            }
        }

        public static void Stop()
        {
            TryStop();
            _currentPath = null;
            _stack.Clear();
        }

        private static void TryStop()
        {
            try { _player?.Stop(); } catch { /* ignore */ }
        }

        /// <summary>
        /// Empilha um novo tema. Ao Dispose, volta para o anterior.
        /// Uso: using var _ = Som.Push("bar.mp3");
        /// </summary>
        public static IDisposable Push(string fileName)
        {
            if (!string.IsNullOrEmpty(_currentPath))
                _stack.Push(_currentPath);

            PlayLoop(fileName);
            return new RevertToken();
        }

        private static void Pop()
        {
            TryStop();

            if (_stack.Count > 0)
            {
                var prev = _stack.Pop();
                if (File.Exists(prev))
                {
                    Ensure();
                    try
                    {
                        _player.PlayLoop(prev);
                        _currentPath = prev;
                        return;
                    }
                    catch { /* ignore */ }
                }
            }

            _currentPath = null;
        }

        private sealed class RevertToken : IDisposable
        {
            public void Dispose() => Pop();
        }
    }
}

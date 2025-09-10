using System;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes
{
    public class MissaoFlorestaSombria : MissaoBase
    {
        private bool aranhaDerrotada = false;

        public MissaoFlorestaSombria(Personagem jogador)
            : base("A Floresta Sombria", "Profundezas da Floresta de Eldor", 400, 900, jogador)
        {
            Descricao = "A floresta sombria é temida por viajantes. Há rumores sobre uma aranha colossal que tece teias envenenadas e captura qualquer um que ouse entrar.";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            Painel(() =>
            {
                Typewriter("💬 Narrador: O vento uiva por entre árvores retorcidas; a lua mal atravessa o dossel negro...", VelocidadeTextoMs);
                Typewriter("Galhos estalam sob seus passos e fios de teia brilham como lâminas prateadas.", VelocidadeTextoMs);
            }, $"🎯 Missão: {Titulo}");

            ExecutarObjetivos(jogador);

            if (VerificarConclusao())
                CompletarMissao(jogador);
            else
            {
                Painel(() =>
                {
                    EscreverCentral("❌ Missão falhou!", 0, ConsoleColor.Red);
                    Linha();
                    StatusJogador();
                }, $"MISSÃO: {Titulo}");
            }
        }

        protected override void ExecutarObjetivos(Personagem jogador)
        {
            bool missaoAbandonada = false;

            while (!aranhaDerrotada && !missaoAbandonada)
            {
                // menu de escolhas
                PainelNoWait(() =>
                {
                    EscreverCentral("🕵️ Onde você quer ir?", 0, ConsoleColor.Yellow);
                    Linha();
                    EscreverLinha("1. Explorar um tronco oco");
                    EscreverLinha("2. Seguir os fios de teia brilhantes");
                    EscreverLinha("0. Abandonar a missão");
                }, "ESCOLHA");

                string escolha = LerEntradaPainel("Digite sua escolha (1, 2 ou 0): ");

                // requisito de nível
                if (jogador.Nivel < 8)
                {
                    Painel(() =>
                    {
                        EscreverCentral("⚠️ Você sente um calafrio... ainda não está forte o suficiente para o que espreita aqui.", 0, ConsoleColor.Red);
                        Linha();
                        EscreverLinha("Volte quando alcançar o nível 8 ou superior!");
                    }, "⚠️ RESTRIÇÃO");
                    return;
                }

                if (escolha == "1")
                {
                    Painel(() =>
                    {
                        Typewriter("Você inspeciona um tronco oco coberto de musgo...", VelocidadeTextoMs);
                        Typewriter("Teias antigas, casulos ressecados e ossos — nenhum sinal de algo útil.", VelocidadeTextoMs);
                        Linha();
                        EscreverLinha("Você retorna ao caminho principal da floresta.");
                    }, "TRONCO OCO");
                }
                else if (escolha == "2")
                {
                    // encontro com a aranha
                    var aranha = PersonagemFactory.Criar(TipoPersonagem.AranhaGigante, "Aranha Gigante");

                    Painel(() =>
                    {
                        EscreverCentral($"⚔️ ENCONTRO COM {aranha.Nome.ToUpper()} ⚔️", 0, ConsoleColor.Red);
                        Linha();
                        Typewriter("Fios de teia tensionam ao seu redor; a criatura desce lentamente das copas!", VelocidadeTextoMs);
                    }, "ENCONTRO");

                    // pausa BGM e limpa a tela para o combate
                    PausarBgmMissao();
                    Console.Clear();

                    var combate = new Combate(jogador, aranha);
                    combate.Iniciar();

                    // retoma BGM e limpa de volta
                    RetomarBgmMissao();
                    Console.Clear();

                    if (!aranha.EstaVivo)
                    {
                        aranhaDerrotada = true;
                        Painel(() =>
                        {
                            EscreverCentral($"🎉 Você derrotou a {aranha.Nome}!", 0, ConsoleColor.Green);
                            Linha();
                            StatusJogador();
                        }, "VITÓRIA");
                    }
                    else
                    {
                        Painel(() =>
                        {
                            EscreverCentral($"💀 Você foi derrotado pela {aranha.Nome}.", 0, ConsoleColor.Red);
                            Linha();
                            StatusJogador();
                        }, "DERROTA");
                        return;
                    }
                }
                else if (escolha == "0")
                {
                    missaoAbandonada = true;
                    Painel(() =>
                    {
                        EscreverCentral("👋 Você decide recuar e retorna à cidade em segurança.", 0, ConsoleColor.Yellow);
                    }, "MISSÃO ABANDONADA");
                }
                else
                {
                    Painel(() =>
                    {
                        EscreverCentral("❌ Escolha inválida! Digite 1, 2 ou 0.", 0, ConsoleColor.Red);
                    }, "ERRO");
                }
            }
        }

        protected override bool VerificarConclusao() => aranhaDerrotada;

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            // Mantive a temática das Botas Élficas, mas aplico DEFESA para compatibilidade garantida
            int aumentoDefesa = 40;
            DigitarTexto($"\n🎁 Dentro de um casulo nas teias, você encontra Botas Élficas! Defesa +{aumentoDefesa}.", 30);
            jogador.Defesa += aumentoDefesa;
            jogador.Nivel += 3;
        }
    }
}

using System;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes
{
    public class MissaoPantanoSombrio : MissaoBase
    {
        private bool bruxaDerrotada = false;

        public MissaoPantanoSombrio(Personagem jogador)
            : base("O Pântano Sombrio", "Pântano Maldito de Grelth", 600, 1200, jogador)
        {
            Descricao = "Você chegou ao Pântano Sombrio, um lugar cheio de névoa e criaturas perigosas. Uma bruxa poderosa habita o local.";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            Painel(() =>
            {
                Typewriter("💬 Narrador: A névoa do pântano engole cada passo. O ar é pesado, cheio de zumbidos e lamentos distantes.", VelocidadeTextoMs);
                Typewriter("Algo o observa entre sombras líquidas, e um calafrio percorre sua espinha.", VelocidadeTextoMs);
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

            while (!bruxaDerrotada && !missaoAbandonada)
            {
                // painel de escolhas
                PainelNoWait(() =>
                {
                    EscreverCentral("🕵️ Onde você quer ir?", 0, ConsoleColor.Yellow);
                    Linha();
                    EscreverLinha("1. Seguir a trilha de lírios negros");
                    EscreverLinha("2. Entrar na cabana da bruxa");
                    EscreverLinha("0. Abandonar a missão");
                }, "ESCOLHA");

                string escolha = LerEntradaPainel("Digite sua escolha (1, 2 ou 0): ");

                // requisito de nível
                if (jogador.Nivel < 12)
                {
                    Painel(() =>
                    {
                        EscreverCentral("⚠️ Você sente a energia do pântano sufocando sua alma. Ainda não tem força para enfrentar a bruxa.", 0, ConsoleColor.Red);
                        Linha();
                        EscreverLinha("Volte quando alcançar o nível 12 ou superior!");
                    }, "⚠️ RESTRIÇÃO");
                    return;
                }

                if (escolha == "1")
                {
                    Painel(() =>
                    {
                        Typewriter("Você segue os lírios negros até um lago de água turva...", VelocidadeTextoMs);
                        Typewriter("Sapos coaxam, sombras se movem sob a superfície, mas nada de ameaçador aparece.", VelocidadeTextoMs);
                        Linha();
                        EscreverLinha("Você retorna ao ponto inicial do pântano.");
                    }, "LAGO ESCURO");
                }
                else if (escolha == "2")
                {
                    var bruxa = PersonagemFactory.Criar(TipoPersonagem.BruxaDoPantano, "Bruxa do Pântano");

                    Painel(() =>
                    {
                        EscreverCentral($"⚔️ ENCONTRO COM {bruxa.Nome.ToUpper()} ⚔️", 0, ConsoleColor.Red);
                        Linha();
                        Typewriter("Poções fervem e o cheiro de enxofre invade o ar. A bruxa ergue seu cajado e a batalha começa!", VelocidadeTextoMs);
                    }, "ENCONTRO");

                    // pausa BGM e limpa a tela
                    PausarBgmMissao();
                    Console.Clear();

                    var combate = new Combate(jogador, bruxa);
                    combate.Iniciar();

                    // retoma BGM e limpa novamente
                    RetomarBgmMissao();
                    Console.Clear();

                    if (!bruxa.EstaVivo)
                    {
                        bruxaDerrotada = true;
                        Painel(() =>
                        {
                            EscreverCentral($"🎉 Você derrotou a {bruxa.Nome}!", 0, ConsoleColor.Green);
                            Linha();
                            StatusJogador();
                        }, "VITÓRIA");
                    }
                    else
                    {
                        Painel(() =>
                        {
                            EscreverCentral($"💀 Você foi derrotado pela {bruxa.Nome}.", 0, ConsoleColor.Red);
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
                        EscreverCentral("👋 Você recua, deixando o pântano sombrio para trás e retorna à cidade em segurança.", 0, ConsoleColor.Yellow);
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

        protected override bool VerificarConclusao() => bruxaDerrotada;

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            int aumentoDefesa = 30;
            DigitarTexto($"\n🎁 Entre frascos quebrados e poções amaldiçoadas, você encontra o Cetro Místico! Defesa +{aumentoDefesa}.", 30);
            jogador.Defesa += aumentoDefesa;
            jogador.Nivel += 5;
        }
    }
}

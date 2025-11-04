using Rpg.Classes.Personagens;
using RPG.Classes.Abstracts.Missao;
using RPG.Classes.Abstracts.Personagens;
using System;

namespace Rpg.Classes.Missoes
{
    public class MissaoMarProfundo : Missao
    {
        private bool krakenDerrotado = false;

        public MissaoMarProfundo(Personagem jogador)
            : base("O Mar Profundo", "Abismo Oceânico de Thalassor", 800, 1500, jogador)
        {
            Descricao = "Você mergulhou no Mar Profundo. Lendas falam de um Kraken gigante que guarda tesouros submarinos.";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            Painel(() =>
            {
                Typewriter("💬 Narrador: As águas são escuras e frias; o peso do oceano comprime seus pulmões...", VelocidadeTextoMs);
                Typewriter("Sombras colossais dançam ao longe, e algo antigo desperta nas profundezas.", VelocidadeTextoMs);
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

            while (!krakenDerrotado && !missaoAbandonada)
            {
                // painel de escolhas
                PainelNoWait(() =>
                {
                    EscreverCentral("🕵️ O que você quer fazer?", 0, ConsoleColor.Yellow);
                    Linha();
                    EscreverLinha("1. Explorar o navio naufragado");
                    EscreverLinha("2. Mergulhar até o covil do Kraken");
                    EscreverLinha("0. Abandonar a missão");
                }, "ESCOLHA");

                string escolha = LerEntradaPainel("Digite sua escolha (1, 2 ou 0): ");

                // requisito de nível
                if (jogador.Nivel < 15)
                {
                    Painel(() =>
                    {
                        EscreverCentral("⚠️ Você não está preparado para enfrentar o Kraken.", 0, ConsoleColor.Red);
                        Linha();
                        EscreverLinha("Volte quando alcançar o nível 15 ou superior!");
                    }, "⚠️ RESTRIÇÃO");
                    return;
                }

                if (escolha == "1")
                {
                    Painel(() =>
                    {
                        Typewriter("Você vasculha as tábuas retorcidas de um navio antigo...", VelocidadeTextoMs);
                        Typewriter("Encontra pérolas menores e moedas corroídas, mas nenhuma pista do Kraken.", VelocidadeTextoMs);
                        Linha();
                        EscreverLinha("Você retorna para a área principal do abismo.");
                    }, "NAVIO NAUFRAGADO");
                }
                else if (escolha == "2")
                {
                    // encontro
                    var kraken = PersonagemFactory.Criar(TipoPersonagem.PovoKraken, "Kraken do Mar Profundo");

                    Painel(() =>
                    {
                        EscreverCentral($"⚔️ ENCONTRO COM {kraken.Nome.ToUpper()} ⚔️", 0, ConsoleColor.Red);
                        Linha();
                        Typewriter("Tentáculos colossais surgem do abismo, bloqueando toda rota de fuga!", VelocidadeTextoMs);
                    }, "ENCONTRO");

                    // pausa a música e limpa a tela para o combate
                    PausarBgmMissao();
                    Console.Clear();

                    // combate
                    var combate = new Combate(jogador, kraken);
                    combate.Iniciar();

                    // retoma música e limpa de volta
                    RetomarBgmMissao();
                    Console.Clear();

                    if (!kraken.EstaVivo)
                    {
                        krakenDerrotado = true;
                        Painel(() =>
                        {
                            EscreverCentral($"🎉 Você derrotou o {kraken.Nome}!", 0, ConsoleColor.Green);
                            Linha();
                            StatusJogador();
                        }, "VITÓRIA");
                    }
                    else
                    {
                        Painel(() =>
                        {
                            EscreverCentral($"💀 Você foi derrotado pelo {kraken.Nome}.", 0, ConsoleColor.Red);
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
                        EscreverCentral("👋 Você decide subir à superfície em segurança.", 0, ConsoleColor.Yellow);
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

        protected override bool VerificarConclusao() => krakenDerrotado;

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            int aumentoDefesa = 60;
            DigitarTexto($"\n🎁 Você saqueia o tesouro abissal e encontra a Armadura do Mar! Defesa +{aumentoDefesa}.", 30);
            jogador.Defesa += aumentoDefesa;
            jogador.Nivel += 5;
        }
    }
}

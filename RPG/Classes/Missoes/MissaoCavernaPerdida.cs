using System;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes
{
    public class MissaoCavernaPerdida : MissaoBase
    {
        private bool ogroDerrotado = false;

        public MissaoCavernaPerdida(Personagem jogador)
            : base("A Caverna Perdida", "Antiga Caverna de Zeltor", 500, 1000, jogador)
        {
            Descricao = "Você encontrou a caverna perdida, mas há uma lenda sobre um ogro terrível que a habita...";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            Painel(() =>
            {
                Typewriter("💬 Narrador: Chegando à entrada da caverna, um calafrio percorre sua espinha...", VelocidadeTextoMs);
                Typewriter("O ar denso e úmido carrega o cheiro de pedra e algo... podre.", VelocidadeTextoMs);
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

            while (!ogroDerrotado && !missaoAbandonada)
            {
                // painel de escolhas
                PainelNoWait(() => {
    EscreverCentral("🕵️ Onde você quer ir?", 0, ConsoleColor.Yellow);
    Linha();
    EscreverLinha("1. Explorar a câmara escura");
    EscreverLinha("2. Seguir a trilha de pegadas");
    EscreverLinha("0. Abandonar a missão");
}, "ESCOLHA");

string escolha = LerEntradaPainel("Digite sua escolha (1, 2 ou 0): ");

                // requisito de nível
                if (jogador.Nivel < 10)
                {
                    Painel(() =>
                    {
                        EscreverCentral("⚠️ Você não tem nível suficiente para enfrentar o ogro.", 0, ConsoleColor.Red);
                        Linha();
                        EscreverLinha("Volte quando estiver mais forte!");
                    }, "⚠️ RESTRIÇÃO");
                    return;
                }

                if (escolha == "1")
                {
                    Painel(() =>
                    {
                        Typewriter("Você se esgueira pela câmara escura...", VelocidadeTextoMs);
                        Typewriter("Encontra ossos, cheiro forte, mas nada de valor.", VelocidadeTextoMs);
                        Linha();
                        EscreverLinha("Você retorna para a entrada da caverna.");
                    }, "CÂMARA ESCURA");
                }
                else if (escolha == "2")
{
    var ogro = PersonagemFactory.Criar(TipoPersonagem.OgroCaverna, "Ogro da Caverna");

    Painel(() =>
    {
        EscreverCentral($"⚔️ ENCONTRO COM {ogro.Nome.ToUpper()} ⚔️", 0, ConsoleColor.Red);
        Linha();
        Typewriter("Um enorme Ogro desperta, rugindo e protegendo o baú!", VelocidadeTextoMs);
    }, "ENCONTRO");

    // --- só aqui: pausa a música da missão e limpa a tela ---
    PausarBgmMissao();
    Console.Clear();

    // combate
    var combate = new Combate(jogador, ogro);
    combate.Iniciar();

    // --- ao voltar do combate: retoma a música e limpa novamente ---
    RetomarBgmMissao();
    Console.Clear();

    if (!ogro.EstaVivo)
    {
        ogroDerrotado = true;
        Painel(() =>
        {
            EscreverCentral($"🎉 Você derrotou o {ogro.Nome}!", 0, ConsoleColor.Green);
            Linha();
            StatusJogador();
        }, "VITÓRIA");
    }
    else
    {
        Painel(() =>
        {
            EscreverCentral($"💀 Você foi derrotado pelo {ogro.Nome}.", 0, ConsoleColor.Red);
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
                        EscreverCentral("👋 Você abandona a missão e retorna à cidade em segurança.", 0, ConsoleColor.Yellow);
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

        protected override bool VerificarConclusao() => ogroDerrotado;

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            int aumentoDefesa = 50;
            DigitarTexto($"\n🎁 Você abre o baú e encontra a Armadura Mágica! Defesa +{aumentoDefesa}.", 30);
            jogador.Defesa += aumentoDefesa;
            jogador.Nivel += 5;
        }
    }
}

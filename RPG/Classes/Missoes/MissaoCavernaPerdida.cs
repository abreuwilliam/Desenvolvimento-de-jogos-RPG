using System;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Missoes;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes;
    public class MissaoCavernaPerdida : MissaoBase
{

    // Uma variável para controlar se a missão foi concluída com sucesso
    private bool ogroDerrotado = false;
    private object elese;

    public MissaoCavernaPerdida(Personagem jogador)
        : base("A Caverna Perdida", "Antiga Caverna de Zeltor", 500, 1000, jogador)
    {
        Descricao = "Você encontrou a caverna perdida, mas há uma lenda sobre um ogro terrível que a habita.";
    }

    // Sobrescreve o método IniciarMissao da classe base
    public override void IniciarMissao(Personagem jogador)
    {
        DigitarTexto("💬 Narrador: Chegando à entrada da caverna, um calafrio percorre sua espinha. O ar denso e úmido carrega o cheiro de pedra e algo... podre. Você decide entrar.", 40);
        Thread.Sleep(3000); 
        ExecutarObjetivos(jogador);
        if (VerificarConclusao() == true)
        {
            CompletarMissao(jogador);
        }
        else
        {
            Console.WriteLine("\n❌ Missão falhou! Tente novamente.");
        }
    }

protected override void ExecutarObjetivos(Personagem jogador)
{
   
    bool missaoAbandonada = false;

    while (!ogroDerrotado && !missaoAbandonada)
    {
        DigitarTexto("\n🕵️ Onde você quer ir?", 30);
        DigitarTexto("1. Explorar a câmara escura", 20);
        DigitarTexto("2. Seguir a trilha de pegadas", 20);
        DigitarTexto("0. Abandonar a missão", 20); // Opção para sair

        Console.Write("\nDigite sua escolha (1, 2 ou 0): ");
        string escolha = Console.ReadLine();

        if (escolha == "1")
        {
            DigitarTexto("\nVocê se esgueira pela câmara escura. Em um canto, você encontra um monte de ossos e um cheiro forte. Você se arrepia, mas não encontra nada de valor. Você volta para a entrada da caverna, mas o baú de tesouro ainda está lá, esperando por você.", 40);
            
        }
        else if (escolha == "2")
        {
            DigitarTexto("\nVocê segue as pegadas gigantes, que levam a uma câmara maior. No centro, um enorme Ogro dorme profundamente, guardando um baú de tesouro cintilante.", 40);

            LoboSombrio ogro = new LoboSombrio();

            Console.WriteLine($"\n\n⚔️ ENCONTRO COM {ogro.Nome.ToUpper()} ⚔️");

            Combate combate = new Combate(jogador, ogro);
            combate.Iniciar();

            if (!ogro.EstaVivo)
            {
                DigitarTexto($"\n🎉 Você derrotou o {ogro.Nome}!", 40);
                ogroDerrotado = true; // Define a variável para sair do loop
            }
            else
            {
                DigitarTexto($"\n💀 Você foi derrotado pelo {ogro.Nome}. A missão falhou.", 40);
                // A linha 'return;' já garante que a missão termine.
                return;
            }
        }
        else if (escolha == "0")
        {
            DigitarTexto("\nVocê decide que o perigo é grande demais e abandona a missão. Retornando à cidade em segurança.", 40);
            missaoAbandonada = true; 
        }
        else
        {
            DigitarTexto("\nEscolha inválida. Por favor, digite 1, 2 ou 0.", 40);
        }
    }
}

    protected override bool VerificarConclusao()
    {
        return ogroDerrotado;
    }

    // Sobrescreve o método para dar a recompensa extra (a armadura)
    protected override void DarRecompensaExtra(Personagem jogador)
    {
        int AumentoDefesa = 50;
        DigitarTexto($"\n🎉 Você abre o baú do tesouro e encontra a Armadura Magica! Ela foi adicionada ao seu inventário.", 40);
        jogador.Defesa += AumentoDefesa;
        jogador.Nivel += 5;
    }
}

using Rpg.Classes.Abstracts;
using System.Threading;

public class Combate
{
    private Personagem Protagonista;
    private Personagem vilao;

    public Combate(Personagem Protagonista, Personagem vilao)
    {
        this.Protagonista = Protagonista;
        this.vilao = vilao;
    }

    public void Iniciar()
    {
        while (Protagonista.EstaVivo && vilao.EstaVivo)
        {
            Protagonista.AtacarAlvo(vilao);
            if (vilao.EstaVivo)
            {
                vilao.AtacarAlvo(Protagonista);
            }
            else
            {
                vilao.ConcederRecompensa(Protagonista);
            }
            Thread.Sleep(3000); 
        }
    }
}


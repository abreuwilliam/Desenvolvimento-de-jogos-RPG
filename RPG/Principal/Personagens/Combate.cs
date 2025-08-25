using Rpg.Principal.Abstracts;
using System.Threading;

public class Combate
{
    private Personagem heroi;
    private Personagem vilao;

    public Combate(Personagem heroi, Personagem vilao)
    {
        this.heroi = heroi;
        this.vilao = vilao;
    }

    public void Iniciar()
    {
        while (heroi.EstaVivo && vilao.EstaVivo)
        {
            heroi.AtacarAlvo(vilao);
            if (vilao.EstaVivo)
            {
                vilao.AtacarAlvo(heroi);
            }
            else
            {
                vilao.ConcederRecompensa(heroi);
            }
            Thread.Sleep(3000); 
        }
    }
}


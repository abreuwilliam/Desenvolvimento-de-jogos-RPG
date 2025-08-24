using Rpg.Principal.Abstracts;
using Rpg.Principal.Personagens;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Personagem heroi = new Personagem("Herói", nivel: 10, ataque: 15, defesa: 5);
Personagem lobo = new Lobo("Lobo");
heroi.MostrarStatus();
while (heroi.EstaVivo && lobo.EstaVivo)
{
    heroi.AtacarAlvo(lobo);
    if (lobo.EstaVivo)
    {
        lobo.AtacarAlvo(heroi);
    }
    else
    {
        lobo.ConcederRecompensa(heroi);
    }
    Thread.Sleep(1000);
    // Pausa de 1 segundo entre ataques para melhor visualização
}
heroi.MostrarStatus();
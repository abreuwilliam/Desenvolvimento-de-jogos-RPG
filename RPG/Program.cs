﻿using System;
using System.Threading;
using Rpg.Principal.Abstracts;
using Rpg.Principal.Personagens;

<<<<<<< HEAD
Console.WriteLine("Hello, World!");
Personagem Protagonista = new Personagem("Herói", nivel: 2, ataque: 150, defesa: 50);
Lobo lobo = new Lobo();
Combate combate = new Combate(Protagonista, lobo);
=======
Console.WriteLine("💥 Batalha: Herói vs Vilão");
>>>>>>> 335c58431b6fa26479ab7b87d4a3b143ac94e5f5

Heroi heroi = new Heroi("Arthur", 10, 15, 5);
Vilao vilao = new Vilao("Orc", 5, 12, 4);

heroi.MostrarStatus();
vilao.MostrarStatus();

// Batalha contra o Vilão
while (heroi.EstaVivo && vilao.EstaVivo)
{
    heroi.AtacarAlvo(vilao);

    if (vilao.EstaVivo)
        vilao.AtacarAlvo(heroi);
    else
        vilao.ConcederRecompensa(heroi);

    Thread.Sleep(1000);
}

// Nemesis aparece
if (heroi.EstaVivo)
{
    Console.WriteLine("\n🌑 Um inimigo misterioso surge do nada! É o Nemesis!");
    Lobo nemesis = new Lobo("Lobo Negro");

    while (heroi.EstaVivo && nemesis.EstaVivo)
    {
        heroi.AtacarAlvo(nemesis);

        if (nemesis.EstaVivo)
            nemesis.AtacarAlvo(heroi);
        else
            nemesis.ConcederRecompensa(heroi);

        Thread.Sleep(1000);
    }
}

heroi.MostrarStatus();
Console.WriteLine("🏆 Aventura encerrada!");
Console.ReadLine();

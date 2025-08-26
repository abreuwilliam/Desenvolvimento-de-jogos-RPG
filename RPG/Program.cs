﻿using System;
using System.Threading;
using Rpg.Principal.Abstracts;
using Rpg.Principal.Personagens;

Console.WriteLine("💥 Batalha: Herói vs Vilão");

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

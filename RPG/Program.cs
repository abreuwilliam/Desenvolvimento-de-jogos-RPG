﻿using System;
using System.Threading;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;

Console.WriteLine("Hello, World!");
Personagem Protagonista = new Personagem("Herói", nivel: 2, ataque: 150, defesa: 50);
LoboSombrio lobo = new LoboSombrio();
Combate combate = new Combate(Protagonista, lobo);

Console.WriteLine("💥 Batalha: Herói vs Vilão");


Heroi heroi = new Heroi("Arthur", 10, 15, 5);
Crocodilo crocodilo = new Crocodilo();

heroi.MostrarStatus();
crocodilo.MostrarStatus();
combate.Iniciar();
heroi.MostrarStatus();
Console.WriteLine("🏆 Aventura encerrada!");
Console.ReadLine();

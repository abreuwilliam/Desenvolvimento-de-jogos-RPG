using Rpg.Principal.Abstracts;
using Rpg.Principal.Personagens;
using Rpg.Principal;

Console.WriteLine("Hello, World!");
Personagem heroi = new Personagem("Herói", nivel: 2, ataque: 150, defesa: 50);
Personagem lobo = new Lobo("Lobo");
Combate combate = new Combate(heroi, lobo);

    heroi.MostrarStatus();

    combate.Iniciar();
    heroi.MostrarStatus();



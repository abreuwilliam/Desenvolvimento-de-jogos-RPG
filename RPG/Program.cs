using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;
using Rpg.Classes;

Console.WriteLine("Hello, World!");
Personagem Protagonista = new Personagem("Herói", nivel: 2, ataque: 150, defesa: 50);
Personagem Lobo da Floresta = new Lobo da Floresta();
Combate combate = new Combate(Protagonista, lobo);

    Protagonista.MostrarStatus();

    combate.Iniciar();

    Protagonista.MostrarStatus();



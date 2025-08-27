using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{

    public class Lobo : Personagem
    {

        public Lobo(string nome = "Lobo Sombrio")
            : base(nome, nivel: 4, ataque: 60, defesa: 100)
        {
        }

    }
}
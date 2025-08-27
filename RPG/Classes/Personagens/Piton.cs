using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
      public class Piton : Personagem
    {
        public Piton(string nome = "Piton")
            : base(nome, nivel: 16, ataque: 170, defesa: 270)
        {
        }
    }
}
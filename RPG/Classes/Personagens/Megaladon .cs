using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
      public class Megaladon : Personagem
    {
        public Megaladon(string nome = "Megaladon")
            : base(nome, nivel: 30, ataque: 310, defesa: 480)
        {
        }
    }
}
using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
      public class OgroDoPantano : Personagem
    {
        public OgroDoPantano(string nome = "Ogro do Pântano")
            : base(nome, nivel: 20, ataque: 210, defesa: 330)
        {
        }
    }
}
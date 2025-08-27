using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
      public class OgroCaverna : Personagem
    {
        public OgroCaverna(string nome = "Ogro da Caverna")
            : base(nome, nivel: 4, ataque: 60, defesa: 30)
        {
        }
    }
}
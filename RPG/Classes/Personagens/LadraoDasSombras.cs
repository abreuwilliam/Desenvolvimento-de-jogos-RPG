using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
      public class LadraoDasSombras : Personagem
    {
        public LadraoDasSombras(string nome = "Ladrão das Sombras")
            : base(nome, nivel: 14, ataque: 150, defesa: 240)
        {
        }
    }
}
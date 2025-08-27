using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
       public class PovoKraken : Personagem
    {
        public PovoKraken(string nome = "Povo Kraken")
            : base(nome, nivel: 36, ataque: 370, defesa: 570)
        {
        }
    }

}
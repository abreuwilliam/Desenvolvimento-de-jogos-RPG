using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
    public class Orca : Personagem
    {
        public Orca(string nome = "Orca")
            : base(nome, nivel: 32, ataque: 330, defesa: 510)
        {
        }
    }
}
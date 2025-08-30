using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
    public class AranhaGigante : Personagem
    {
        public AranhaGigante(string nome = "Aranha Gigante")
            : base(nome, nivel: 42, ataque: 490, defesa: 1100)
        {
        }

    }
}

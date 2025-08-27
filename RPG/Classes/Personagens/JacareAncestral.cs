using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
    public class JacareAncestral : Personagem
    {
        public JacareAncestral(string nome = "Jacaré Ancestral")
            : base(nome, nivel: 18, ataque: 190, defesa: 300)
        {
        }
    }
}
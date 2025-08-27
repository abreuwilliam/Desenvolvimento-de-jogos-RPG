using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
     public class PiratasNavioBarbaNegra : Personagem
    {
        public PiratasNavioBarbaNegra(string nome = "Piratas - Navio Barba Negra")
            : base(nome, nivel: 34, ataque: 350, defesa: 540)
        {
        }
    }
}
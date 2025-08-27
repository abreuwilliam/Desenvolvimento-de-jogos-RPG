using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
     public class CacadorDeTesouros : Personagem
    {
        public CacadorDeTesouros(string nome = "Caçador de Tesouros")
            : base(nome, nivel: 42, ataque: 430, defesa: 660)
        {
        }
    }
}
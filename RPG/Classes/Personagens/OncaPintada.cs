using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
       public class OncaPintada : Personagem
    {
        public OncaPintada(string nome = "Onça Pintada")
            : base(nome, nivel: 8, ataque: 90, defesa: 150)
        {
        }
    }
}
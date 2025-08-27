using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
     public class PlantaDevoraAlmas : Personagem
    {
        public PlantaDevoraAlmas(string nome = "Planta Devora-Almas")
            : base(nome, nivel: 10, ataque: 110, defesa: 180)
        {
        }
    }

}
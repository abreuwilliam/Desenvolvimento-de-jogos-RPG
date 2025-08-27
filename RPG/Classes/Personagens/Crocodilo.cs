using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
    public class Crocodilo : Personagem
    {
        public Crocodilo(string nome = "Crocodilo")
            : base(nome, nivel: 28, ataque: 290, defesa: 450)
        {
        }
    } 
}
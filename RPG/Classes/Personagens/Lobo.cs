using Rpg.Classes.Abstracts; 

namespace Rpg.Classes.Personagens 
{
    public class Lobo da Floresta : Personagem
    {
     
        public Lobo(string nome = "Lobo")
            : base(nome, nivel: 4, ataque: 60, defesa: 100)
        {
        }  

    }
}
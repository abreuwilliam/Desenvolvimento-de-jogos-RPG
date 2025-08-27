using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
     public class EspiritoDoPantano : Personagem
    {
        public EspiritoDoPantano(string nome = "Espirito do Pântano")
            : base(nome, nivel: 22, ataque: 230, defesa: 360)
        {
        }
    }
}
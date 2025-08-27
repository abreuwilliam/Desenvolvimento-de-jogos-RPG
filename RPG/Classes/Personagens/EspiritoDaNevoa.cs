using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
      public class EspiritoDaNevoa : Personagem
    {
        public EspiritoDaNevoa(string nome = "Espirito da Névoa")
            : base(nome, nivel: 26, ataque: 270, defesa: 420)
        {
        }
    }
}
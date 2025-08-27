using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
     public class LoboDoDeserto : Personagem
    {
        public LoboDoDeserto(string nome = "Lobo do Deserto")
            : base(nome, nivel: 40, ataque: 410, defesa: 630)
        {
        }
    }
}
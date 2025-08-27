using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
     public class EscorpiaoDeFogo : Personagem
    {
        public EscorpiaoDeFogo(string nome = "Escorpião de Fogo")
            : base(nome, nivel: 38, ataque: 390, defesa: 600)
        {
        }
    }
}
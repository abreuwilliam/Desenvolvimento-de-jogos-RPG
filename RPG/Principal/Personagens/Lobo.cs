using System;
using Rpg.Principal.Abstracts;

namespace Rpg.Principal.Personagens
{
    public class Lobo : Personagem
    {
        public Lobo(string nome)
            : base(nome, 2, 20, 10)
        {
        }

        public override void ConcederRecompensa(Personagem agressor)
        {
            int expGanho = 50;
            int ouroGanho = 20;

            agressor.AdicionarExperiencia(expGanho);
            agressor.AdicionarOuro(ouroGanho);

            Console.WriteLine($"{agressor.Nome} ganhou {expGanho} de experiência e {ouroGanho} de ouro por derrotar o Lobo!");
        }
    }
}

using System;
using Rpg.Principal.Abstracts;
namespace Rpg.Principal.Personagens
{
    public class Vilao : Personagem
    {
        public Vilao(string nome, int nivel, int ataque, int defesa)
            : base(nome, nivel, ataque, defesa)
        {
            OuroBonus = 20;
        }

        public int OuroBonus { get; private set; }

        public override void ConcederRecompensa(Personagem agressor)
        {
            int expGanho = Nivel * 10;
            int ouroGanho = Nivel * 5 + OuroBonus;

            agressor.AdicionarExperiencia(expGanho);
            agressor.AdicionarOuro(ouroGanho);

            Console.WriteLine($"{agressor.Nome} derrotou o Vilão {Nome} e ganhou {expGanho} EXP e {ouroGanho} de ouro!");
        }

        public void AtaqueSombrio(Personagem alvo)
        {
            int danoExtra = 8;
            Console.WriteLine($"\n{Nome} usa ataque sombrio!");
            alvo.ReceberDano(CalcularDano() + danoExtra);
        }
    }
}

using System;
using RPG.Classes.Abstracts.Personagens;  
namespace Rpg.Classes.Personagens 
{
    public class Cavalheiro : Personagem
    {
        public Cavalheiro(string nome, int nivel, int ataque, int defesa)
            : base(nome, nivel, ataque, defesa)
        {
            this.Nome = nome;
            AtaqueBonus = 5;
        }

        public int AtaqueBonus { get; private set; }

        public override void ConcederRecompensa(Personagem agressor)
        {
            base.ConcederRecompensa(agressor);
            Console.WriteLine("O herói recebeu um bônus de coragem!");
        }

        public void AtaqueEspecial(Personagem alvo)
        {
            int danoExtra = 10;
            Console.WriteLine($"{Nome} usa ataque especial!");
            alvo.ReceberDano(CalcularDano() + danoExtra);
        }
    }
}

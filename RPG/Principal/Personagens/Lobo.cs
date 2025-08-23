using Rpg.Principal.Abstracts; // AQUI! Adicione esta linha para referenciar a classe Personagem

namespace Rpg.Principal.Personagens // Exemplo de namespace para a classe Lobo
{
    public class Lobo : Personagem
    {
        // Construtor da classe Lobo. 
        // Ele chama o construtor da classe base (Personagem) com os valores iniciais.
        public Lobo(string nome)
            : base(nome, nivel: 2, ataque: 20, defesa: 10)
        {
        }


        public void Recompensa()
        {
            Console.WriteLine("🐺 O Lobo uiva para a lua!");
        }
        public override void ConcederRecompensa(Personagem agressor)
        {
            int expGanho = 50; // Experiência fixa por derrotar o lobo
            int ouroGanho = 20; // Ouro fixo por derrotar o lobo

            agressor.AdicionarExperiencia(expGanho);
            agressor.AdicionarOuro(ouroGanho);

            Console.WriteLine($"🏆 {agressor.Nome} ganhou {expGanho} de experiência e {ouroGanho} de ouro por derrotar o Lobo!");
        }

    }
}
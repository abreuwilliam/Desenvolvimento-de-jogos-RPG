using Rpg.Principal.Abstracts; // AQUI! Adicione esta linha para referenciar a classe Personagem

namespace Rpg.Principal.Personagens // Exemplo de namespace para a classe Lobo
{
    public class Lobo : Personagem
    {
        // Construtor da classe Lobo. 
        // Ele chama o construtor da classe base (Personagem) com os valores iniciais.
        public Lobo(string nome)
            : base(nome, nivel: 4, ataque: 60, defesa: 100)
        {
        }  

    }
}
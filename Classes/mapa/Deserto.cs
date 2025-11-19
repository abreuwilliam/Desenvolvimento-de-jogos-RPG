using System;
using RPG.Classes.Abstracts.Personagens;
using Rpg.Classes.Personagens;
using Rpg.Classes.Missoes;

namespace RPG.Mapa
{
    public class Deserto
    {
        private Personagem heroi;
        private Combate combate;

        public Deserto(Personagem heroi)
        {
            this.heroi = heroi;
        }

        public void EntrarNoDeserto()
        {
            var Som = new AudioPlayer();
            Som.PlayLoop("deserto.mp3");

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Deserto Escaldante de Aridia");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"Herói: {heroi.Nome} | Ouro: {heroi.Ouro}");
            Console.WriteLine();
            Console.WriteLine("O sol abrasador queima sua pele enquanto você atravessa as dunas intermináveis do Deserto de Aridia.");
            Console.WriteLine("Cuidado com as tempestades de areia e as criaturas que espreitam sob o calor escaldante.");
            Console.WriteLine();
            Console.WriteLine("Mais adiante você avista um castelo gigante.");
            Console.WriteLine("Pressione 1 para explorar o castelo ou qualquer outra tecla para retornar ao mapa.");
            string escolha = Console.ReadLine();

            if (escolha == "1" && heroi.Experiencia >= 500)
            {
                Console.WriteLine("Você decide explorar o castelo misterioso no meio do deserto...");
                Console.WriteLine("Dentro do castelo, você encontra um Dragão adormecido guardando a porta de uma cela!");
                Console.WriteLine("Pressione qualquer tecla para enfrentá-lo.");
                Console.ReadKey();

                var dragao = PersonagemFactory.Criar(TipoPersonagem.Dragao);
                combate = new Combate(heroi, dragao);
                combate.Iniciar();

                if (heroi.Vida > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n VOCÊ DERROTOU O DRAGÃO! ");
                    Console.ResetColor();
                    Console.WriteLine(" VOCÊ VENCEU O GAME! ");
                    Console.WriteLine("Aguarde para o lançamento do RPG 2...");
                    Console.WriteLine("\nPressione qualquer tecla para retornar ao mapa.");
                    Console.ReadKey();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVocê foi derrotado pelo dragão...");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("Você decide retornar ao mapa.");
            }

            Som.Stop();
        }
    }
}

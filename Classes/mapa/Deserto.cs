using System;
using RPG.Classes.Abstracts.Personagens;
using Rpg.Classes.Personagens;

namespace RPG.Mapa
{
    public class Deserto
    {
         private Personagem heroi;

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
             Console.WriteLine("🏜️  Deserto Escaldante de Aridia");
             Console.ResetColor();
             Console.WriteLine();
             Console.WriteLine($"Herói: {heroi.Nome} | Ouro: {heroi.Ouro}");
             Console.WriteLine();
             Console.WriteLine("O sol abrasador queima sua pele enquanto você atravessa as dunas intermináveis do Deserto de Aridia.");
             Console.WriteLine("Cuidado com as tempestades de areia e as criaturas que espreitam sob o calor escaldante.");
             Console.WriteLine();
             Console.WriteLine("Mais adiante avista um castelo gigante");
             Console.WriteLine("Pressione 1 para explorar o castelo ou qualquer outra tecla para retornar ao mapa.");
             String escolha = Console.ReadLine();

                if (escolha == "1" && heroi.Experiencia >= 500)
                {
                    Console.WriteLine("Você decide explorar o castelo misterioso no meio do deserto...");
                    Console.WriteLine("Dentro do castelo, você encontra um Dragão adormecido guardando a porta de uma cela!");
                    Console.WriteLine("Pressione qualquer tecla para enfrentá-lo.");
                    Console.ReadKey();
                    var dragao = PersonagemFactory.Criar(TipoPersonagem.Dragao);
                    var batalha = new Batalha(heroi, dragao);
                    batalha.Iniciar();
                    Console.WriteLine("Após a batalha, você sai do castelo e retorna ao deserto.");
                }
                else
                {
                    Console.WriteLine("Você decide retornar ao mapa.");
                }

             Som.Stop();

         }
    }
}
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
                    Console.WriteLine("\nVOCÊ DERROTOU O DRAGÃO!");
                    Console.ResetColor();

                    Console.WriteLine("A porta da cela atrás do dragão se abre lentamente...");
                    Console.WriteLine("Você volta ao corredor principal do castelo em busca da princesa Alice.");

                    Console.WriteLine("\nPressione uma tecla para continuar...");
                    Console.ReadKey();

                    CenaTraicaoHenry();
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

        private void CenaTraicaoHenry()
        {
            Console.Clear();
            Console.WriteLine("Ao entrar na sala do trono, você vê a princesa Alice acorrentada...");
            Console.WriteLine("voce a liberta e vai com ela para a vila até o castelo do rei.");
            Console.WriteLine("Depois de dias a cavalo e muito love com a princesa, vocês chegam ao castelo.");
            Console.WriteLine("Mas, antes de chegar ao castelo faltando poucos Km, uma voz familiar ecoa pela floresta:");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nHenry: \"Muito bem, herói... exatamente como eu planejei.\"");
            Console.ResetColor();

            Console.WriteLine("\nVocê se vira e vê Henry, o velho guerreiro, agora com uma armadura negra.");
            Console.WriteLine("Ele revela a verdade: foi ele quem sequestrou a princesa!");
            Console.WriteLine("Henry queria que alguém derrotasse o dragão para abrir caminho...");
            Console.WriteLine("... e depois planejava matar o herói para roubar a recompensa do rei.");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nHenry: \"A princesa, o ouro, a nobreza... tudo será MEU!\"");
            Console.ResetColor();

            Console.WriteLine("\nPrepare-se para o combate final!");
            Console.WriteLine("Pressione qualquer tecla para lutar contra Henry.");
            Console.ReadKey();

            var henryTraidor = PersonagemFactory.Criar(TipoPersonagem.Henry);
            combate = new Combate(heroi, henryTraidor);
            combate.Iniciar();

            if (heroi.EstaVivo)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nVOCÊ DERROTOU HENRY!");
                Console.ResetColor();

                Console.WriteLine("Você liberta a princesa Alice, que agradece emocionada.");
                Console.WriteLine("O rei ficará eternamente grato por sua coragem.");
                Console.WriteLine("\nRECOMPENSAS:");
                Console.WriteLine("• 2000 de Ouro");
                Console.WriteLine("• Título de Nobreza");
                Console.WriteLine("• E a gratidão eterna da princesa Alice");

                heroi.Ouro += 2000;
                heroi.Experiencia += 500;

                Console.WriteLine("\nFIM DO GAME – obrigado por jogar!");
                Console.WriteLine("Aguarde para o lançamento do RPG 2...");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nVocê foi derrotado por Henry...");
                Console.ResetColor();
            }

            Console.WriteLine("\nPressione qualquer tecla para retornar ao mapa.");
            Console.ReadKey();
        }
    }
}

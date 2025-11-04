using Rpg.Classes.Personagens;
using RPG.Classes.Abstracts.Missao;
using RPG.Classes.Abstracts.Personagens;
using System;

namespace Rpg.Classes.Missoes
{
    public class MissaoMarProfundo : Missao
    {
        private bool krakenDerrotado = false;

        public MissaoMarProfundo(Personagem jogador)
            : base("O Mar Profundo", 
                   "Você mergulhou no Mar Profundo. Um Kraken lendário habita essas águas sombrias.",
                   "Abismo Oceânico de Thalassor",
                   800, 1500, jogador)
        {
        }

     
        public void Iniciar(Personagem jogador)
        {
            Console.Clear();
            Console.WriteLine($"=== MISSÃO INICIADA: {Titulo} ===");
            Console.WriteLine($"{Descricao}");
            Console.WriteLine($"Local: {Local}");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();

            ExecutarObjetivos(jogador);

            if (krakenDerrotado)
                Completar(jogador);
            else
            {
                Console.WriteLine("\n❌ Missão falhou!");
                Console.WriteLine($"Status do jogador: Vida {jogador.Vida}, Ouro {jogador.Ouro}");
            }
        }


        private void ExecutarObjetivos(Personagem jogador)
        {
            bool missaoAbandonada = false;

            while (!krakenDerrotado && !missaoAbandonada)
            {
                Console.Clear();
                Console.WriteLine("🌊 Você está nas profundezas do oceano...");
                Console.WriteLine("O que deseja fazer?");
                Console.WriteLine("1. Explorar o navio naufragado");
                Console.WriteLine("2. Enfrentar o Kraken");
                Console.WriteLine("0. Abandonar a missão");
                Console.Write("\nEscolha: ");

                string escolha = Console.ReadLine();

                if (jogador.Nivel < 15)
                {
                    Console.WriteLine("\n⚠️ Você precisa ser nível 15 ou superior para enfrentar o Kraken!");
                    Console.ReadKey();
                    return;
                }

                if (escolha == "1")
                {
                    Console.WriteLine("\nVocê explora o navio e encontra moedas antigas.");
                    Console.WriteLine("Nada do Kraken ainda...");
                    Console.ReadKey();
                }
                else if (escolha == "2")
                {
                    Console.WriteLine("\n⚔️ O Kraken emerge das profundezas!");
                    var kraken = PersonagemFactory.Criar(TipoPersonagem.PovoKraken, "Kraken do Mar Profundo");

                    var combate = new Combate(jogador, kraken);
                    combate.Iniciar();

                    if (!kraken.EstaVivo)
                    {
                        krakenDerrotado = true;
                        Console.WriteLine("🎉 Você derrotou o Kraken!");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("💀 Você foi derrotado pelo Kraken...");
                        return;
                    }
                }
                else if (escolha == "0")
                {
                    missaoAbandonada = true;
                    Console.WriteLine("\n👋 Você retorna à superfície.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\n❌ Opção inválida.");
                    Console.ReadKey();
                }
            }
        }


        private void Completar(Personagem jogador)
        {
            Console.WriteLine("\n✅ Missão concluída!");
            jogador.Ouro += 800;
            jogador.GanharExperiencia(1500);


            jogador.Defesa += 60;
            jogador.Nivel += 5;

            Console.WriteLine("🎁 Recompensa especial: Armadura do Mar! +60 DEF");
            Console.WriteLine("Você sobe +5 níveis!");
        }
    }
}

using System;
using RPG.Classes.Abstracts.Personagens;
using Rpg.Classes.Personagens;

namespace Rpg.Classes.Missoes
{
    public class MissaoPantanoSombrio : MissaoBase
    {
        private bool bruxaDerrotada = false;
        private AudioPlayer Som;


        public MissaoPantanoSombrio(Personagem jogador)
            : base("O Pântano Sombrio", "Pântano Maldito de Grelth", 600, 1200, jogador)
        {
            Descricao = "Você chegou ao Pântano Sombrio, um lugar cheio de névoa e criaturas perigosas. Uma bruxa poderosa habita o local.";
        }

        public override void IniciarMissao(Personagem jogador)
        {
            Console.Clear();
            var Som = new AudioPlayer();
            Som.PlayLoop("Pantano.mp3");

            Console.WriteLine(" Narrador: A névoa do pântano engole cada passo. O ar é pesado e cheio de murmúrios estranhos.");
            Console.WriteLine("Dizem que uma bruxa poderosa vive aqui, em uma velha cabana coberta de limo.");
            Console.WriteLine();

            ExecutarObjetivos(jogador);

            Som.Stop();

            if (VerificarConclusao())
                CompletarMissao(jogador);
            else
                Console.WriteLine("\n Missão falhou!");
        }

        protected override void ExecutarObjetivos(Personagem jogador)
        {
            bool missaoAbandonada = false;

            while (!bruxaDerrotada && !missaoAbandonada)
            {
                Console.WriteLine("\nO que deseja fazer?");
                Console.WriteLine("1. Seguir a trilha de lírios negros");
                Console.WriteLine("2. Entrar na cabana da bruxa");
                Console.WriteLine("0. Abandonar a missão");
                Console.Write("\nEscolha: ");
                string escolha = Console.ReadLine();

                if (jogador.Nivel < 12)
                {
                    Console.WriteLine("\n O ar do pântano é sufocante... você ainda não tem força para enfrentar a bruxa.");
                    Console.WriteLine("Volte quando alcançar o nível 12 ou superior!");
                    return;
                }

                if (escolha == "1")
                {
                    Console.WriteLine("\nVocê segue a trilha até um lago escuro coberto por névoa.");
                    Console.WriteLine("Sapos coaxam, mas nada de ameaçador aparece.");
                    Console.WriteLine("Você retorna ao ponto inicial.");
                }
                else if (escolha == "2")
                {
                    Console.WriteLine("\nVocê se aproxima da cabana. Poções fervem, corvos observam do teto...");
                    Console.WriteLine("Uma risada ecoa — a Bruxa do Pântano surge das sombras!");
                    Som.Push("Bruxa.mp3");

                    var bruxa = PersonagemFactory.Criar(TipoPersonagem.BruxaDoPantano, "Bruxa do Pântano");
                    var combate = new Combate(jogador, bruxa);
                    combate.Iniciar();

                    Som.Stop();

                    if (!bruxa.EstaVivo)
                    {
                        bruxaDerrotada = true;
                        Console.WriteLine($"\n Você derrotou a {bruxa.Nome}!");
                        Console.WriteLine("A névoa começa a se dissipar lentamente...");
                    }
                    else
                    {
                        Console.WriteLine($"\n Você foi derrotado pela {bruxa.Nome}.");
                        return;
                    }
                }
                else if (escolha == "0")
                {
                    missaoAbandonada = true;
                    Console.WriteLine("\n Você recua, deixando o pântano sombrio para trás.");
                }
                else
                {
                    Console.WriteLine("\n Escolha inválida! Digite 1, 2 ou 0.");
                }
            }
        }

        protected override bool VerificarConclusao() => bruxaDerrotada;

        protected override void DarRecompensaExtra(Personagem jogador)
        {
            int aumentoDefesa = 30;
            Console.WriteLine($"\n Entre os frascos quebrados, você encontra o Cetro das Sombras! Defesa +{aumentoDefesa}");
            jogador.Defesa += aumentoDefesa;
            jogador.Nivel += 5;
        }
    }
}

using System;
using System.Threading;
using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Missoes
{
    public abstract class MissaoBase
    {
        public string Id { get; protected set; }
        public string Titulo { get; protected set; }
        public string Descricao { get; protected set; }
        public string Local { get; protected set; }
        public bool EstaCompleta { get; protected set; }
        public bool EstaAtiva { get; protected set; }
        public int ExperienciaRecompensa { get; protected set; }
        public int OuroRecompensa { get; protected set; }

        public Personagem Jogador { get; protected set; }

        protected MissaoBase(string titulo, string local, int expRecompensa, int ouroRecompensa, Personagem jogador)
        {
            Id = Guid.NewGuid().ToString();
            Titulo = titulo;
            Local = local;
            ExperienciaRecompensa = expRecompensa;
            OuroRecompensa = ouroRecompensa;
            EstaCompleta = false;
            EstaAtiva = false;
            this.Jogador = jogador;
        }

        public abstract void IniciarMissao(Personagem jogador);

        public void ExecutarMissao(Personagem jogador)
        {
            if (EstaCompleta)
            {
                Console.WriteLine("✅ Esta missão já foi completada!");
                return;
            }

            Console.WriteLine($"\n🎯 INICIANDO MISSÃO: {Titulo}");
            Console.WriteLine(new string('=', 50));

            EstaAtiva = true;

            ContarHistoria();

            ExecutarObjetivos(jogador);

            if (VerificarConclusao())
            {
                CompletarMissao(jogador);
            }
            else
            {
                Console.WriteLine("\n❌ Missão falhou! Tente novamente.");
            }

            Console.WriteLine(new string('=', 50));
        }

        protected virtual void ContarHistoria()
        {
            Console.WriteLine($"\n📖 LOCAL: {Local}");
            Console.WriteLine($"📝 {Descricao}");

            DigitarTexto($"\n💬 Narrador: ", 50);
        }

        protected abstract void ExecutarObjetivos(Personagem jogador);

        protected abstract bool VerificarConclusao();

        protected virtual void CompletarMissao(Personagem jogador)
        {
            EstaCompleta = true;
            EstaAtiva = false;

            Console.WriteLine($"\n🎉 MISSÃO CONCLUÍDA: {Titulo}");

            jogador.AdicionarExperiencia(ExperienciaRecompensa);
            jogador.AdicionarOuro(OuroRecompensa);

            Console.WriteLine($"\n🎁 RECOMPENSAS:");
            Console.WriteLine($"⭐ +{ExperienciaRecompensa} EXP");
            Console.WriteLine($"💰 +{OuroRecompensa} Ouro");

            DarRecompensaExtra(jogador);
        }

        protected virtual void DarRecompensaExtra(Personagem jogador)
        {
            
        }

        protected void DigitarTexto(string texto, int velocidadeMs = 30)
        {
            foreach (char c in texto)
            {
                Console.Write(c);
                Thread.Sleep(velocidadeMs);
            }
            Console.WriteLine();
            //10-20 mensagens rapida passa sensao de urgencia
            // 30 - 40 padra conversa normal
            //80-100 mensagens lentas, para dar tempo de ler efeito dramatico
            // 150-200 mensagens muito lentas, para dar tempo de ler e pensar
        }

        // Método para mostrar status da missão
        public void MostrarStatus()
        {
            Console.WriteLine($"\n📋 MISSÃO: {Titulo}");//
            Console.WriteLine($"📍 Local: {Local}");
            Console.WriteLine($"📝 {Descricao}");
            Console.WriteLine($"✅ Status: {(EstaCompleta ? "Concluída 🎉" : EstaAtiva ? "Em Andamento ⏳" : "Disponível 📌")}");
            Console.WriteLine($"🎁 Recompensa: {ExperienciaRecompensa} EXP + {OuroRecompensa} Ouro");
        }
    }
}
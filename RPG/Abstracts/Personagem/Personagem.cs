using System;

namespace RpgTerminal.Principal
{
    public class Personagem
    {
        // Propriedades básicas
        public string Nome { get; private set; }
        public int Nivel { get; private set; }
        public int Vida { get; private set; }
        public int VidaMaxima { get; private set; }
        public int Ataque { get; private set; }
        public int Defesa { get; private set; }
        public int Experiencia { get; private set; }
        public int Ouro { get; private set; }
        public bool EstaVivo => Vida > 0;

        // Construtor
        public Personagem(string nome, int nivel)
        {
            Nome = nome;
            Nivel = nivel;
            Experiencia = 0;
            Ouro = 50;
            InicializarAtributos();
        }

        // Método para inicializar atributos baseados no nível
        private void InicializarAtributos()
        {
            VidaMaxima = 100 + (Nivel * 10);
            Vida = VidaMaxima;
            Ataque = 15 + (Nivel * 2);
            Defesa = 5 + Nivel;
        }
}
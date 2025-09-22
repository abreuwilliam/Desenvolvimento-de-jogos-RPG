﻿using System;
using System.Threading;
using Rpg.Classes.Abstracts;
using Rpg.Classes.Personagens;
using Rpg.Classes.Missoes;
using Rpg.UI;
using System.Security.Claims;
using System.IO;
using System.Text;
using RPG.Mapa;
Console.OutputEncoding = Encoding.UTF8;

/*
// chama o menu
MenuResult resultadoMenu = MainMenu.Show();

// se o jogador não confirmou, sai do jogo
if (!resultadoMenu.Confirmado)
{
    Console.WriteLine("👋 Saindo do jogo. Até a próxima!");
    return;
}

// pega os dados do menu
string nome = resultadoMenu.Nome;
string dificuldade = resultadoMenu.Dificuldade.ToString(); // se quiser usar em logs

Console.Clear();

Personagem Protagonista = new Personagem(nome, nivel: 10, ataque: 30, defesa: 50);
Protagonista.Ouro = 3200; 

Personagem lobo = PersonagemFactory.Criar(TipoPersonagem.LoboSombrio);

//Combate combate = new Combate(Protagonista, lobo);
//combate.Iniciar();

MissaoBase missao = new MissaoCavernaPerdida(Protagonista);
missao.ExecutarMissao(Protagonista);

/*
Console.WriteLine("Hello, World!");
Personagem Protagonista = new Personagem("Herói", nivel: 2, ataque: 150, defesa: 50);
LoboSombrio lobo = new LoboSombrio();
Combate combate = new Combate(Protagonista, lobo);

Console.WriteLine("💥 Batalha: Herói vs Vilão");


Heroi heroi = new Heroi("Arthur", 10, 15, 5);
Crocodilo crocodilo = new Crocodilo();

heroi.MostrarStatus();
crocodilo.MostrarStatus();
combate.Iniciar();
heroi.MostrarStatus();
Console.WriteLine("🏆 Aventura encerrada!");
Console.ReadLine();
*/

MenuResult resultadoMenu = MainMenu.Show();
Personagem Protagonista = new Personagem(resultadoMenu.Nome, nivel: 1000, ataque: 30, defesa: 50);
Protagonista.Ouro = 3200;

Combate combate = new Combate(Protagonista, PersonagemFactory.Criar(TipoPersonagem.LoboSombrio));
//combate.Iniciar();
Floresta floresta = new Floresta(Protagonista);
floresta.Iniciar();


    

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


MenuResult resultadoMenu = MainMenu.Show();

Personagem Protagonista = new Personagem(resultadoMenu.Nome, nivel: 5, ataque: 40, defesa: 60);
Protagonista.Ouro = 3200;

BoasVindas boasVindas = new BoasVindas(Protagonista);
boasVindas.Executar();
var navegacao = new Navegacao(Protagonista);
navegacao.Executar();



    

using System;
using System.Threading;
using Rpg.Classes.Personagens;
using Rpg.Classes.Missoes;
using System.Security.Claims;
using System.IO;
using System.Text;
using RPG.Mapa;
using RPG.Classes.Abstracts.Personagens;
using RPG.Classes.mapa;


 Menu.ExibirMenu();


Personagem Protagonista = new Personagem(Menu.Nome(), nivel: 10, ataque: 30, defesa: 50);
Protagonista.Ouro = 3200;

BoasVindas boasVindas = new BoasVindas(Protagonista);
boasVindas.Executar();
var navegacao = new Navegacao(Protagonista);
navegacao.Executar();



    

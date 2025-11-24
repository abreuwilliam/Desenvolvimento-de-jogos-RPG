using RPG.Mapa;
using RPG.Classes.Abstracts.Personagens;

Menu.ExibirMenu();

Personagem Protagonista = new Personagem(Menu.Nome(), nivel: 1, ataque: 30, defesa: 50);

BoasVindas boasVindas = new BoasVindas(Protagonista);
boasVindas.Executar();
var navegacao = new Navegacao(Protagonista);
navegacao.Executar();



    

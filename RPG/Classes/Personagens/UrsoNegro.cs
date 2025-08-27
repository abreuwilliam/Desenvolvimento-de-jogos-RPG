using Rpg.Classes.Abstracts;

namespace Rpg.Classes.Personagens
{
	public class UrsoNegro : Personagem
	{

		public UrsoNegro(string nome = "Urso Negro")
			: base(nome, nivel: 6, ataque: 80, defesa: 150)
		{
		}

	}
}
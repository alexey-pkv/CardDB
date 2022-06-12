namespace CardDB.Modules.APIModule.Models
{
	public class CardIndexSetModel
	{
		public CardIndexModel[] cards { get; init; }
		
		
		public CardIndexSetModel() {}
		public CardIndexSetModel(CardIndex[] data)
		{
			cards = new CardIndexModel[data.Length];
			
			for (var i = 0; i < data.Length; i++)
			{
				cards[i] = new CardIndexModel(data[i]);
			}
		}
	}
}
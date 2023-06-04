package Shared;

public class Card {
	public Suit suit;
	public Rank rank;

	Card(int suitNumber, int rankNumber) {
		this.suit = Suit.get(suitNumber);
		this.rank = Rank.get(rankNumber);
	}

	@Override
	public String toString() {
		return this.rank.description + " de " + this.suit.description;
	}
}

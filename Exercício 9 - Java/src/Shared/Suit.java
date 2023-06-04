package Shared;

public enum Suit {
	DIAMONDS(1, "Ouros"),
	CLUBS(2, "Paus"),
	HEARTS(3, "Copas"),
	SPADES(4, "Espadas");

	private final int number;
	public final String description;

	Suit(int number, String description) {
		this.number = number;
		this.description = description;
	}

	public static Suit get(int number) {
		for (Suit suit : Suit.values()) {
			if (suit.number == number) {
				return suit;
			}
		}
		throw new IllegalArgumentException("No suit found for number: " + number);
	}
}

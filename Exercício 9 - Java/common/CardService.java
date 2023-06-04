import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;

public class CardService extends UnicastRemoteObject implements ICardService {
	public CardService() throws RemoteException {
	}

	@Override
	public String getCardName(int suitNumber, int rankNumber) throws RemoteException {
		System.out.println("Objeto recebido!");

		Card card = new Card(suitNumber, rankNumber);
		return card.toString();
	}
}

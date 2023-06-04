import java.rmi.Remote;
import java.rmi.RemoteException;

public interface ICardService extends Remote {
	String getCardName(int suitNumber, int rankNumber) throws RemoteException;
}

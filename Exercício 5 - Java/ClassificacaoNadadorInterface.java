import java.rmi.Remote;
import java.rmi.RemoteException;

public interface ClassificacaoNadadorInterface extends Remote {
    String getClassificacao(int idade) throws RemoteException;
}
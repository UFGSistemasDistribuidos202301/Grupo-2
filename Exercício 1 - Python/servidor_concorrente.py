import socket
import threading

def reajustar_salario(cargo, salario):
    if cargo == "operador":
        return salario * 1.20
    elif cargo == "programador":
        return salario * 1.18
    
    return None

def conexao_cliente(conexao, endereco):
    print("Conexão estabelecida com", endereco)

    dados = conexao.recv(1024).decode()
    nome, cargo, salario = dados.split(";")

    salario_reajustado = reajustar_salario(cargo, float(salario))

    if salario_reajustado is not None:
        resposta = f"Nome: {nome}\nSalário reajustado: R${salario_reajustado:.2f}"
    else:
        resposta = "Cargo inválido."

    conexao.send(resposta.encode())

    conexao.close() 

    print("Conexão encerrada com", endereco)

# Iniciando o servidor
servidor = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
endereco_servidor = ('localhost', 10101)
servidor.bind(endereco_servidor)
servidor.listen(5)
print("Servidor aguardando conexões...")

while True:
    conexao, endereco_cliente = servidor.accept()

    thread_cliente = threading.Thread(target=conexao_cliente, args=(conexao, endereco_cliente))
    thread_cliente.start()
    
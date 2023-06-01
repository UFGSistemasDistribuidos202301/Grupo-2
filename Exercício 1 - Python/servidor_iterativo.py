import socket

def reajustar_salario(cargo, salario):
    if cargo == "operador":
        return salario * 1.20
    elif cargo == "programador":
        return salario * 1.18
    
    return None

# Iniciando o servidor
servidor = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
endereco_servidor = ('localhost', 10101)
servidor.bind(endereco_servidor)
servidor.listen(1)
print("Servidor aguardando conexões...")

while True:
    conexao, endereco_cliente = servidor.accept()
    print("Conexão estabelecida com", endereco_cliente)

    dados = conexao.recv(1024).decode()
    nome, cargo, salario = dados.split(";")

    salario_reajustado = reajustar_salario(cargo, float(salario))

    if salario_reajustado is not None:
        resposta = f"Nome: {nome}\nSalário reajustado: R${salario_reajustado:.2f}"
    else:
        resposta = "Cargo inválido."

    conexao.send(resposta.encode())

    conexao.close()
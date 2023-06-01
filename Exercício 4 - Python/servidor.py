from xmlrpc.server import SimpleXMLRPCServer

def calcular_peso_ideal(altura, sexo):
    if sexo == "homem":
        return (72.7 * altura) - 58
    elif sexo == "mulher":
        return (62.1 * altura) - 44.7
    else:
        raise ValueError("Sexo inválido.")

server = SimpleXMLRPCServer(("localhost", 10101))
server.register_function(calcular_peso_ideal, "calcular_peso_ideal")
print("Servidor iniciado.")
server.serve_forever()
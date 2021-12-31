# Number Divisors

Projeto criado visando implementar uma API que receba um número e retorne a todos seus divisore, informando quais desse são números primos. 
Este projeto utiliza a estrutura de `IAsyncEnumerable`, disponível a partir do .net Core 3.0.

Foram utilizadas as seguintes tecnologias:
- .Net 5 WebApi => Framework para construção de APIs
- KeyDb/Redis => Estrutura de dados chave/valor em memória, utilizado para implementação de cache distribuído.
- HAProxy => Serviço de proxy, utilizado como LB para as instâncias da WebApi
- React => Framework frontend
- Oboe => Biblioteca para consumo de APIs, realiza a desserialização do Json por stream.
- Nginx => Web server, utilizado para servir o frontend

## Como executar
Este projeto tem como dependência apenas o docker-compose. Após realizar o download, basta executar: 

`docker-compose up`

Após executar, basta acessar http://localhost:80, caso queira acessar a documentação da API, basta acessar http://localhost:5000.

Por estar utilizando HAProxy com DNS discovery, é possível escalar a webapi com o comando `docker-compose scale webapi=N`, sendo N a quantidade desejada de instâncias da webapi. 

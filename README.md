# O PROJETO

O projeto foi desenvolvido utilizando .Net 8. Suas principais funcionalidades são o cadastro de contas por usuários onde esse usuário pode realizar operações como depósito, transferências e saques em sua conta.

## TECONOLOGIAS
Além do uso de .Net também foram usados algumas outras ferramentas no projeto, como ORMs (Entity Framework core) e pacotes para auxiliarem na validação e relacionamento com o banco.

# REGAS DE NEGÓCIO:

## Cadastro de usuário
O cadastro de usuário é feito através de e-mail e senha, sendo ambos os campos obrigatórios.

 1 - E-mail e senha obrigatórios
 2 - Permitido apenas 1 e-mail por usuário.
 3 - A senha deve conter no mínimo 6 caracteres.

 ## Atualização do usuário
 Não é possível atualizar as informações do usuário no momento.

 ### Cadastro de contas
 O Cadastro de contas é protegido por autenticação. Para realizar o cadastro, o token retornado no cadastro/login de usuário deve ser enviado no header da requisição.

 1 - Deve ser informado o Cnpj e enviado 1 documento no momento da criação da conta.
 2 - Ambos os campos são de envio obrigatório.
 3 - O documento enviado deve ser uma imagem.
 4 - O Cnpj informado deve ter um formato válido.
 5 - O Cnpj informado não pode pertercer a outra conta (ativa ou inativa).
 6 - O Cnpj deve ser encontrado ReceitaWS API.
 7 - O nome e o nome fantasia são puxados na ReceitaWS API não sendo necessário que o usuário informe esses campos.
 8 - A conta é vinculado ao usuário logado (id enviado no token)
 9 - O usuário não pode possuir uma conta ativa. 

 ## Atualização de contas
 Só é possível atualizar o documento e o Cnpj da conta. Sendo aplicado as mesmas regras da criação. O usuário só pode atualizar dados da conta ativa que está vinculada a ele.

 ## Transações
 As transações se dividem em depósito, saque e transferência.

 ### Depósitos
Não é necessário estar autenticado para realizar um depósito

1 - Necessário informar os campos Número da conta, Agência, Dígito e Valor.
2 - Não é possível realizar depósitos em contas inativas. Será retornado um erro de conta não encontrada.
3 - O valor do depósito deve ser maior que zero.
4 - Os depósitos geram registros de transações do tipo entrada.

### Saques
Necessário estar autenticado para realizar o saque. O usuário só pode realizar saques da conta ativa que está vinculada a ele.

1 - Necessário informar os campos Número da conta, Agência, Dígito e Valor.
2 - A conta informada deve existir e estar ativa.
3 - A conta deve pertercer ao usuário logado.
4 - O valor do saque deve ser menor ou igual ao valor do saldo da conta.
5 - Os saques geram transações do tipo saída.

### Transferências
Necessário estar autenticado para realizar a transferência.

1 - Necessário informar os campos Número da conta, Agência, Dígito e Valor.
2 - Os dados acima não podem ser da conta origem (conta do usuário).
3 - Os dados acima devem ser de uma conta ativa.
4 - O valor da transferência não pode ser maior que o valor do saldo da conta de origem.
5 - Realizar uma transfrência geram transações do tipo saída (conta origem) e entrada (conta destino)
6 - Realizar transferências atualizam o valor do saldo das contas origem e destino

## Visualização da conta
O usuário só pode visualizar os dados de sua própria conta. Se o número da conta enviado na query da requisição pertecer a outro usuário, será retornado um erro de não encontrado.

## Extrado da conta
O usuário só pode visualizar o extrato de sua própria conta. Se os dados da conta enviado no body da requisição pertecer a outro usuário, será retornado um erro de não encontrado.
1 - Necessário informar os campos Número da conta, Agência, Dígito e Valor.
2 - Retornado o saldo da conta, total de entradas e total de saídas.
3 - Retornado todos os registros de transações dividos em entrada e saída.

# Organização do projeto 

A solução é organizada em cinco projetos: Communication, API, Domain, Application e Infrastructure. A organização segue o conceito DDD porém com algumas modificações para atender as necessidades do projeto.
Cada projeto é reponsável por configurar a injeção de dependência de seus serviços através da classe "DependencyInjectExtension".
Todas as injeções de dependência do projeto são de escopo "scoped".
As regras de negócio ficam em application, na pasta UseCases.
Os repositórios são dividos em repositórios de escrita e leitura, sendo que nos repositório de escrita ficam todas as operações de alteração no banco (insert, update e delete) enquanto que na de leitura ficam apenas operações de select.
As migrações são feitas usando o pacote FluentMigration e são organizadas por versões em infrastructure. 
 

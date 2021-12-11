para execução local com vs code

API
renomear o arquivo api/src/appsettings.Local.json.example para api/src/appsettings.Local.json e ajustar o nome da fila e se é uma fila FIFO

renomear o arquivo api/local.env.example para api/local.env e ajustar AWS_ACCESS_KEY_ID e AWS_SECRET_ACCESS_KEY com as credenciais conta aws

abrir a pasta api com o vs code e apertar F5

JOB 

renomear o arquivo job/src/appsettings.Local.json.example para job/src/appsettings.Local.json e ajustar o nome da fila e se é uma fila FIFO

renomear o arquivo job/local.env.example para job/local.env e ajustar AWS_ACCESS_KEY_ID e AWS_SECRET_ACCESS_KEY com as credenciais conta aws

abrir a pasta job com o vs code e apertar F5

para execução com docker compose

fazer os ajustes para execução local e executar o comando

docker-compose up

isso ira executar um container da api na porta 9002 e um container do job


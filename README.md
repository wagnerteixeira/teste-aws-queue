para execução local com Vs Code

API
renomear o arquivo src/Api/appsettings.Local.json.example para src/Api/appsettings.Local.json e ajustar o nome da fila e se é uma fila FIFO

renomear o arquivo src/Api/Properties/launchSettings.json.example para src/Api/Properties/launchSettings.json e ajustar AWS_ACCESS_KEY_ID e AWS_SECRET_ACCESS_KEY com as credenciais conta aws

selecionar a opção "Executar Api" no Run and Debug do Vs Code e apertar F5

JOB 

renomear o arquivo src/Job/appsettings.Local.json.example para src/Job/appsettings.Local.json e ajustar o nome da fila, nome da fila dlq, dados de conexao postgres e se é uma fila FIFO

renomear o arquivo src/Job/Properties/launchSettings.json.example para src/Job/Properties/launchSettings.json e ajustar AWS_ACCESS_KEY_ID e AWS_SECRET_ACCESS_KEY com as credenciais conta aws

selecionar a opção "Executar Job" no Run and Debug do Vs Code e apertar F5

para execução dos serviços com docker

Ctrl+Shift+P => Tasks: Run Task => Criar base e Subir os Serviços

Este comando irá subir um container com a base de dados postgres, um container com a api e um container com o job

Para parar os serviços

Ctrl+Shift+P => Tasks: Run Task => Parar Serviços

post sobre backup sqs https://stackoverflow.com/questions/57978083/aws-sqs-backup-solution-design
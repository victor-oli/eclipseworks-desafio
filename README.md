# eclipseworks-desafio

# Como iniciar o projeto via Docker:
- Primeiro é nesserário entrar na pasta raiz do projeto e rodar o comando `docker-compose up --build`;
- Uma vez que os containers foram criados, é necessário entrar no diretório da api e rodar o comando do migrations para realizar as configurações iniciais do banco. `dotnet ef migrations add initial`;
- Depois, basta rodar o comando `dotnet ef database update`;

Pronto, agora o projeto está pronto para rodar, aproveite!

Na pasta raiz do projeto existe um arquivo chamado `postman_collection` que contém todos os endpoint disponíveis na api, fique a vontado para utilizar.


# Perguntas ao PO:
- Quais relatório você acha que ajudariam no foturo?
- Você acha que ajudaria se houvesse mais níveis de hierarquia entre o projeto e as tarefas (por exemplo, micro-tarefas)?
- Faria sentido enviar e-mail e/ou outros tipos de notificações durante a gestão das tarefas?



# Pontos de melhorias:
- Aumentar a cobertura dos testes unitários englobando as outras camadas e aumentando a segurando do desenvolvimento;
- Implementar uma captura de logs bem definidos para permitir uma análise precisa durante eventuras problemas.
- Mais endpoints administrativos;
- Migrar a deleção física(feito assim em caráter didático) para deleção lógica mantendo sempre os registros antigos;
- Incluir healthcheck;
- Armezenar mais informações que podem ser úteis tanto para desenvolvimento quanto para o usuário;
- Incluir um webhook que possibilitaria por exemplo enviar feedback de quando uma tarefa é concluída para outras ferramentas;

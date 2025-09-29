docker build -t tareas:dev .

docker run -d --name tareas -p 5057:5057 tareas:dev
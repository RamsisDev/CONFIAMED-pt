docker build -t usuariosrvs:dev .

docker run -d --name usuariosrvs -p 5273:5273 usuariosrvs:dev
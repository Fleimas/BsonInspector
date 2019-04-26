//Build and package
dotnet publish -c Release -o out

//upload out folder via ftp

//build docker image on server
docker build -t bsoninspector .

//run docker image on server
docker run -d -p 8081:80 --name BsonInspector bsoninspector

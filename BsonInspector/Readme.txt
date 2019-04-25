//Build and package
dotnet publish -c Release -o out

//build docker image
docker build -t bsoninspector .

//run docker image
docker run -d -p 8081:80 --name BsonInspector bsoninspector

docker-compose up -d
dotnet tool restore --configfile .config/NuGet.config
echo "Aguardando 30s para subida do banco"
sleep 30s
dotnet dbup upgrade
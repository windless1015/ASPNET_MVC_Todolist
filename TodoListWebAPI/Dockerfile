



#Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./TodoListWebAPI.csproj" --disable-parallel
RUN dotnet publish "./TodoListWebAPI.csproj" -c release -o /app --no-restore

#Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
ENV ASPNETCORE_URLS http://+:81
WORKDIR /app
COPY --from=build /app ./


ENTRYPOINT ["dotnet", "TodoListWebAPI.dll"]
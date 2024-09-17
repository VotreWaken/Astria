# Используем базовый образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем рабочую директорию
WORKDIR /src

# Копируем .sln файл
COPY Astria.sln ./Astria.sln

# Копируем все необходимые .csproj файлы в соответствующие директории
COPY Astria.ProductManagement/ProductManagement.API/ProductManagement.API.csproj ./Astria/Astria.ProductManagement/ProductManagement.API/
COPY Astria.ProductManagement/ProductManagement.Application/ProductManagement.Application.csproj ./Astria/Astria.ProductManagement/ProductManagement.Application/
COPY Astria.ProductManagement/ProductManagement.Infrastructure/ProductManagement.Infrastructure.csproj ./Astria/Astria.ProductManagement/ProductManagement.Infrastructure/
COPY Astria.ProductManagement/ProductManagement.Domain/ProductManagement.Domain.csproj ./Astria/Astria.ProductManagement/ProductManagement.Domain/
COPY Astria.AuthenticationManagement/AuthenticationManagement.Authentication/AuthenticationManagement.Authentication.csproj ./Astria/Astria.AuthenticationManagement/AuthenticationManagement.Authentication/
COPY Astria.Rabbitmq/Astria.Rabbitmq.csproj ./Astria/Astria.Rabbitmq/
COPY Astria.SharedKernel/Astria.SharedKernel.csproj ./Astria/Astria.SharedKernel/
COPY Astria.QueryRepository/Astria.QueryRepository.csproj ./Astria/Astria.QueryRepository/
COPY Astria.EventSourcingRepository/Astria.EventSourcingRepository.csproj ./Astria/Astria.EventSourcingRepository/

# Восстанавливаем зависимости
RUN dotnet restore ./Astria/Astria.ProductManagement/ProductManagement.API/ProductManagement.API.csproj

# Копируем все исходные файлы в соответствующие директории
COPY Astria.ProductManagement/ProductManagement.API/. ./Astria/Astria.ProductManagement/ProductManagement.API/
COPY Astria.ProductManagement/ProductManagement.Application/. ./Astria/Astria.ProductManagement/ProductManagement.Application/
COPY Astria.ProductManagement/ProductManagement.Infrastructure/. ./Astria/Astria.ProductManagement/ProductManagement.Infrastructure/
COPY Astria.ProductManagement/ProductManagement.Domain/. ./Astria/Astria.ProductManagement/ProductManagement.Domain/
COPY Astria.AuthenticationManagement/. ./Astria/Astria.AuthenticationManagement/
COPY Astria.Rabbitmq/. ./Astria/Astria.Rabbitmq/
COPY Astria.SharedKernel/. ./Astria/Astria.SharedKernel/
COPY Astria.QueryRepository/. ./Astria/Astria.QueryRepository/
COPY Astria.EventSourcingRepository/. ./Astria/Astria.EventSourcingRepository/

# Сборка проекта
WORKDIR /src/Astria/Astria.ProductManagement/ProductManagement.API
RUN dotnet publish -c Release -o /out

# Создание финального образа для выполнения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /src
COPY --from=build /out .

ENTRYPOINT ["dotnet", "ProductManagement.API.dll"]

# Открываем необходимые порты
EXPOSE 5197
EXPOSE 7185

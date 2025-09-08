# Imagen base con runtime (producción)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagen con SDK (compilación)
FROM mcr.microsoft.com/dotnet/sdk:8.0.100-rc.2 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar proyectos y restaurar
COPY ["src/CleanArchitecture/CleanArchitecture.Api/CleanArchitecture.Api.csproj", "src/CleanArchitecture/CleanArchitecture.Api/"]
COPY ["src/CleanArchitecture/CleanArchitecture.Application/CleanArchitecture.Application.csproj", "src/CleanArchitecture/CleanArchitecture.Application/"]
COPY ["src/CleanArchitecture/CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "src/CleanArchitecture/CleanArchitecture.Domain/"]
COPY ["src/CleanArchitecture/CleanArchitecture.Infraestructure/CleanArchitecture.Infraestructure.csproj", "src/CleanArchitecture/CleanArchitecture.Infraestructure/"]

RUN dotnet restore "./src/CleanArchitecture/CleanArchitecture.Api/CleanArchitecture.Api.csproj"

# Copiar el resto del código
COPY . .
WORKDIR "/src/src/CleanArchitecture/CleanArchitecture.Api"
RUN dotnet build "./CleanArchitecture.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar la app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CleanArchitecture.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# -------------------
# ETAPA FINAL - DEBUG
# -------------------
FROM build AS debug
WORKDIR /app
COPY --from=publish /app/publish .
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV DOTNET_RUNNING_IN_CONTAINER=1
ENTRYPOINT ["dotnet", "CleanArchitecture.Api.dll"]

# -----------------------
# ETAPA FINAL - PRODUCCIÓN
# -----------------------
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Api.dll"]
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SguScheduleAPNs.DevicesManager/SguScheduleAPNs.DevicesManager.csproj", "SguScheduleAPNs.DevicesManager/"]
COPY ["Core/Core.csproj", "Core/"]

RUN dotnet restore "SguScheduleAPNs.DevicesManager/SguScheduleAPNs.DevicesManager.csproj"

COPY ["SguScheduleAPNs.DevicesManager/", "SguScheduleAPNs.DevicesManager/"]
COPY ["Core/", "Core/"]

RUN dotnet publish "SguScheduleAPNs.DevicesManager/SguScheduleAPNs.DevicesManager.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SguScheduleAPNs.DevicesManager.dll"]
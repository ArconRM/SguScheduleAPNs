FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SguScheduleAPNs.NotificationServer/SguScheduleAPNs.NotificationServer.csproj", "SguScheduleAPNs.NotificationServer/"]
COPY ["Core/Core.csproj", "Core/"]

RUN dotnet restore "SguScheduleAPNs.NotificationServer/SguScheduleAPNs.NotificationServer.csproj"

COPY ["SguScheduleAPNs.NotificationServer/", "SguScheduleAPNs.NotificationServer/"]
COPY ["Core/", "Core/"]

RUN dotnet publish "SguScheduleAPNs.NotificationServer/SguScheduleAPNs.NotificationServer.csproj" -c Release -o /app/publish --no-restore

FROM build AS final

WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SguScheduleAPNs.NotificationServer.dll"]

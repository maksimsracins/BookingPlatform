# Local development setup

The database password and JWT signing key are not stored in tracked configuration.
Set them in the current PowerShell session before starting the database and API:

```powershell
function New-RandomSecret {
    $generator = [Security.Cryptography.RandomNumberGenerator]::Create()
    try {
        $bytes = New-Object byte[] 32
        $generator.GetBytes($bytes)
        [Convert]::ToBase64String($bytes)
    }
    finally {
        $generator.Dispose()
    }
}

$env:POSTGRES_PASSWORD = New-RandomSecret
docker compose -f .\src\docker-compose.yml up -d

$env:Jwt__SecretKey = New-RandomSecret
$env:ConnectionStrings__Default = "Host=localhost;Port=5432;Database=booking;Username=postgres;Password=$env:POSTGRES_PASSWORD"
dotnet run --project .\src\BookingPlatform.Api\BookingPlatform.Api.csproj
```

Compose requires `POSTGRES_PASSWORD` and publishes PostgreSQL only on the local
machine. Configure `Jwt__SecretKey` with a secret of at least 32 bytes; the API
refuses to start if the key is missing or too short. Use a secret manager or
environment variables for deployed environments, and never commit real secrets.
For an existing PostgreSQL data volume, changing `POSTGRES_PASSWORD` does not
change the password of the already-initialized database role; rotate that
password in PostgreSQL to match the new environment value.

The same `ConnectionStrings__Default` environment variable can be supplied when
running the Telegram worker.

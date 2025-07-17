# Entity Framework Migration Commands

param(
    [Parameter(Mandatory=$false)]
    [string]$MigrationName
)

function Add-Migration {
    param([string]$Name)
    if (-not $Name) {
        Write-Host "Usage: .\ef-commands.ps1 -MigrationName 'YourMigrationName'" -ForegroundColor Red
        return
    }
    dotnet ef migrations add $Name --project BE.Data --startup-project .
}

function Remove-Migration {
    dotnet ef migrations remove --project BE.Data --startup-project .
}

function Update-Database {
    dotnet ef database update --project BE.Data --startup-project .
}

function Drop-Database {
    dotnet ef database drop --project BE.Data --startup-project .
}

# Execute based on parameters
if ($MigrationName) {
    Add-Migration -Name $MigrationName
} else {
    Write-Host "Available commands:" -ForegroundColor Green
    Write-Host "  Add Migration:    .\scripts\ef-commands.ps1 -MigrationName 'YourMigrationName'" -ForegroundColor Yellow
    Write-Host "  Remove Migration: Remove-Migration" -ForegroundColor Yellow
    Write-Host "  Update Database:  Update-Database" -ForegroundColor Yellow
    Write-Host "  Drop Database:    Drop-Database" -ForegroundColor Yellow
}

# PowerShell Build Script - Windows equivalent of Makefile
param(
    [Parameter(Position=0, Mandatory=$true)]
    [string]$Command,
    
    [Parameter(Position=1)]
    [string]$Name
)

switch ($Command) {
    "migration-add" {
        if (-not $Name) {
            Write-Host "Usage: .\make.ps1 migration-add YourMigrationName" -ForegroundColor Red
            exit 1
        }
        dotnet ef migrations add $Name --project BE.Data --startup-project .
    }
    
    "migration-remove" {
        dotnet ef migrations remove --project BE.Data --startup-project .
    }
    
    "database-update" {
        dotnet ef database update --project BE.Data --startup-project .
    }
    
    "database-drop" {
        dotnet ef database drop --project BE.Data --startup-project .
    }
    
    "build" {
        dotnet build
    }
    
    "clean" {
        dotnet clean
    }
    
    "restore" {
        dotnet restore
    }
    
    "run" {
        dotnet run
    }
    
    "test" {
        dotnet test
    }
    
    "dev" {
        dotnet watch run
    }
    
    "seed-data" {
        Write-Host "Importing all geographical data..." -ForegroundColor Green
        dotnet run --project BE.Data/DataImporter/DataImporter.csproj
    }
    
    default {
        Write-Host "Available commands:" -ForegroundColor Green
        Write-Host "  .\make.ps1 migration-add <name>   - Add a new migration" -ForegroundColor Yellow
        Write-Host "  .\make.ps1 migration-remove       - Remove last migration" -ForegroundColor Yellow
        Write-Host "  .\make.ps1 database-update        - Update database" -ForegroundColor Yellow
        Write-Host "  .\make.ps1 database-drop          - Drop database" -ForegroundColor Yellow
        Write-Host "  .\make.ps1 build                  - Build project" -ForegroundColor Yellow
        Write-Host "  .\make.ps1 clean                  - Clean project" -ForegroundColor Yellow
        Write-Host "  .\make.ps1 restore                - Restore packages" -ForegroundColor Yellow
        Write-Host "  .\make.ps1 run                    - Run project" -ForegroundColor Yellow
        Write-Host "  .\make.ps1 test                   - Run tests" -ForegroundColor Yellow
        Write-Host "  .\make.ps1 dev                    - Run with hot reload" -ForegroundColor Yellow
        Write-Host "  .\make.ps1 seed-data              - Import all CSV data to database" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Examples:" -ForegroundColor Cyan
        Write-Host "  .\make.ps1 migration-add AddUserTable" -ForegroundColor Gray
        Write-Host "  .\make.ps1 build" -ForegroundColor Gray
        Write-Host "  .\make.ps1 dev" -ForegroundColor Gray
    }
}

# Database Configuration Switcher for Geographical Data API
# Usage: .\switch-database.ps1 <provider>
# Providers: sqlite, localdb, sqlserver, inmemory

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("sqlite", "localdb", "sqlserver", "inmemory")]
    [string]$Provider
)

$appsettingsPath = "appsettings.Development.json"

switch ($Provider.ToLower()) {
    "sqlite" {
        $config = @{
            "Logging" = @{
                "LogLevel" = @{
                    "Default" = "Information"
                    "Microsoft.AspNetCore" = "Warning"
                    "Microsoft.EntityFrameworkCore.Database.Command" = "Information"
                }
            }
            "ConnectionStrings" = @{
                "DefaultConnection" = "Data Source=./BE.Data/Data/geodata.db"
            }
            "DatabaseProvider" = "SQLite"
            "EnableHttpsRedirection" = $false
            "Development" = @{
                "AllowHttpTesting" = $true
                "UseSwaggerOnRoot" = $true
            }
        }
        Write-Host "✅ Configured for SQLite database" -ForegroundColor Green
        Write-Host "   File: ./BE.Data/Data/geodata.db" -ForegroundColor Gray
    }
    "localdb" {
        $config = @{
            "Logging" = @{
                "LogLevel" = @{
                    "Default" = "Information"
                    "Microsoft.AspNetCore" = "Warning" 
                    "Microsoft.EntityFrameworkCore.Database.Command" = "Information"
                }
            }
            "ConnectionStrings" = @{
                "DefaultConnection" = "Server=(localdb)\MSSQLLocalDB;Database=GeographicalDataDB;Trusted_Connection=true;TrustServerCertificate=true;"
            }
            "DatabaseProvider" = "LocalDB"
            "EnableHttpsRedirection" = $false
            "Development" = @{
                "AllowHttpTesting" = $true
                "UseSwaggerOnRoot" = $true
            }
        }
        Write-Host "✅ Configured for SQL Server LocalDB" -ForegroundColor Green
        Write-Host "   Database: GeographicalDataDB on (localdb)\MSSQLLocalDB" -ForegroundColor Gray
        Write-Host "   Note: Run 'dotnet ef database update' to create the database" -ForegroundColor Yellow
    }
    "sqlserver" {
        $config = @{
            "Logging" = @{
                "LogLevel" = @{
                    "Default" = "Information"
                    "Microsoft.AspNetCore" = "Warning"
                    "Microsoft.EntityFrameworkCore.Database.Command" = "Information"
                }
            }
            "ConnectionStrings" = @{
                "DefaultConnection" = "Server=localhost;Database=GeographicalDataDB;Trusted_Connection=true;TrustServerCertificate=true;"
            }
            "DatabaseProvider" = "SqlServer"
            "EnableHttpsRedirection" = $false
            "Development" = @{
                "AllowHttpTesting" = $true
                "UseSwaggerOnRoot" = $true
            }
        }
        Write-Host "✅ Configured for SQL Server Express" -ForegroundColor Green
        Write-Host "   Database: GeographicalDataDB on localhost" -ForegroundColor Gray
        Write-Host "   Note: Ensure SQL Server Express is installed and running" -ForegroundColor Yellow
    }
    "inmemory" {
        $config = @{
            "Logging" = @{
                "LogLevel" = @{
                    "Default" = "Information"
                    "Microsoft.AspNetCore" = "Warning"
                    "Microsoft.EntityFrameworkCore.Database.Command" = "Warning"
                }
            }
            "ConnectionStrings" = @{
                "DefaultConnection" = ""
            }
            "DatabaseProvider" = "InMemory"
            "EnableHttpsRedirection" = $false
            "Development" = @{
                "AllowHttpTesting" = $true
                "UseSwaggerOnRoot" = $true
            }
        }
        Write-Host "✅ Configured for In-Memory database" -ForegroundColor Green
        Write-Host "   Note: Data will be lost when application stops" -ForegroundColor Yellow
    }
}

# Write the configuration to appsettings.Development.json
$configJson = $config | ConvertTo-Json -Depth 10
$configJson | Set-Content $appsettingsPath -Encoding UTF8

Write-Host ""
Write-Host "🔄 Next steps:" -ForegroundColor Cyan
if ($Provider -eq "localdb" -or $Provider -eq "sqlserver") {
    Write-Host "   1. dotnet ef database update" -ForegroundColor White
    Write-Host "   2. dotnet run" -ForegroundColor White
} else {
    Write-Host "   1. dotnet run" -ForegroundColor White
}
Write-Host ""

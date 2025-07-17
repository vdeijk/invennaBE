# Entity Framework Commands
migration-add:
	dotnet ef migrations add $(name) --project BE.Data --startup-project .

migration-remove:
	dotnet ef migrations remove --project BE.Data --startup-project .

database-update:
	dotnet ef database update --project BE.Data --startup-project .

database-drop:
	dotnet ef database drop --project BE.Data --startup-project .

# Build Commands
build:
	dotnet build

clean:
	dotnet clean

restore:
	dotnet restore

# Run Commands
run:
	dotnet run

test:
	dotnet test

# Development Commands
dev:
	dotnet watch run

# Usage Examples:
# make migration-add name=InitialCreate
# make migration-remove
# make database-update
# make build
# make run

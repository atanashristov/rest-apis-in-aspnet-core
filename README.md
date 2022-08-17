# rest-apis-in-aspnet-core

Code and notes from studying [Rest Api's in Asp.Net Core and C# 2022 Edition](https://www.udemy.com/course/rest-apis-in-aspnet-core)

Develompment:

```bash
cd code/MusicApi
dotnet watch
```

## Notes

### EF: Migrations

Install globally EF command line tools:

```bash
dotnet tool install --global dotnet-ef
```

Add package to the project:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Verify installation:

```bash
dotnet ef
```

Create migration:

```bash
dotnet ef migrations add <Name>
```

Run migrations:

```bash
dotnet ef database update
```

See [DotNet EF CLI Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

### Azure

Storage account:

- Resource group "musicgroup"
- Storage account "musicstorageaccount321"
- Public container "songcovers"

Example over image: [Image](https://musicstorageaccount321.blob.core.windows.net/songcovers/Depeche_Mode-101.jpg)

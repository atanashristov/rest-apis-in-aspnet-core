# rest-apis-in-aspnet-core

Code and notes from studying [Rest Api's in Asp.Net Core and C# 2022 Edition](https://www.udemy.com/course/rest-apis-in-aspnet-core)

Development:

```bash
cd code/MusicApi
dotnet watch
```

## Notes

### Postgres DB privileges

Before using migrations, create user and database in Postgres.

Run below as postgres user (admin user).

Create user "musicuser":

```sql
create user musicuser with encrypted password 'musicpass';
```

Create database:

```sql
create database musicdb_dev;
```

Change owner and privileges:

```sql
alter database musicdb_dev owner to musicuser;
-- not needed:
-- grant all privileges on database musicdb_dev to musicuser;
```

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
- Storage account "musicstorageaccount322"
- Public container "musiccover"

Example over image: [Image](https://musicstorageaccount322.blob.core.windows.net/musiccover/ACDC.jpg)

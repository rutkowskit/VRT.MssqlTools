# VRT.MssqlTools.Decrypt App

Application dumps sql server objects as sql `create` commands to the standard output.
Encrypted sql objects will be decrypted first.

## Enabling DAC

The application uses the `Dedicated Administrator Connection (DAC)`.

It is required to properly decrypt encrypted sql objects.

Follow the steps described in [this](https://www.mssqltips.com/sqlservertip/5364/troubleshooting-the-sql-server-dedicated-administrator-connection/) article in order to enable `DAC`.


## Building and publishing
To build the app, run the following command: 

### Normal build
```dotnet publish -c Release```

### Build self contained release

```dotnet publish -r win-x64 -c Release -p:PublishSingleFile=true /p:PublishTrimmed=true /p:EnableCompressionInSingleFile=true /p:ReadyToRun=true --self-contained```


### Build framework dependent release

```dotnet publish -c Release -p:PublishSingleFile=true /p:PublishTrimmed=true /p:EnableCompressionInSingleFile=true /p:ReadyToRun=true```

## Execution

```bash
Usage: MsSqlDecrypt [--sql-server <String>] [--sql-database <String>] [--sql-user-name <String>] [--sql-password <String>] [--object-name-filter <String>] [--help] [--version]

MsSqlDecrypt

Options:
  -s, --sql-server <String>            Sql server name. Name format: [server]\[instance] (Required)
  -d, --sql-database <String>          Sql server database name (Required)
  -u, --sql-user-name <String>         Sql server user name. If this value is empty, windows authentication will be used.
  -p, --sql-password <String>          Sql server user password. Ignored if SqlUserName is empty.
  -n, --object-name-filter <String>    Object name filter pattern. No case sensitive.
  -h, --help                           Show help message
  --version                            Show version
```

### Example with sql user authentication

```bash
MsSqlDecrypt -s "MyServer\sql1" -d "TestDatabase" -u sa -p "very_complicated_password"
```

### Example with windows authentication

In order to authenticate using `windows` credentials, just omit `sql-user-name` parameter.

```bash
MsSqlDecrypt -s "MyServer\sql1" -d "TestDatabase"
```


### Filter object by name

The application has `filter by name` option.
The below command will filter all objects with name containing `person` word.
Matching is NOT case sensitive.

```bash
MsSqlDecrypt -s "MyServer\sql1" -d "TestDatabase" -n "person"
```

# PROJECT_CNPM

## Khởi tạo hệ thống

### Dựng container và tạo database

```bash
docker-compose up -d
docker exec -it project_cnpm /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Kh@ng09102005" -C -i /SQL.sql
```

## Chạy dự án

Mở hai terminal riêng.

### Terminal 1: Chạy backend Python

```bash
py .\src\app.py
```

### Terminal 2: Chạy ứng dụng .NET

```bash
dotnet watch run
```

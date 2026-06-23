# Project_CNPM

Huong dan khoi dong va chay du an.

## 1. Khoi dong database

Mo terminal tai thu muc goc du an va chay:

```powershell
docker-compose up -d
```

Tao database va import du lieu:

```powershell
docker exec -it cnpm_server /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Kh@ng09102005" -C -i /SQL.sql
```

## 2. Cai thu vien Python

```powershell
pip install -r requirement.txt
```

## 3. Chay backend Python

Mo terminal thu nhat:

```powershell
python .\src\app.py
```

Backend Python chay tai:

```text
http://localhost:5000
```

## 4. Chay ung dung .NET

Mo terminal thu hai:

```powershell
dotnet restore
dotnet watch run
```

Sau khi chay thanh cong, mo duong dan hien tren terminal de vao website.

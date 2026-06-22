Lệnh dựng container và tạo database
- docker-compose up -d
- docker exec -it project_cnpm  /otp/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Kh@ng09102005" -C -i /SQL.sql

Lệnh chạy thử (mở 2 terminal)
- py .\src\app.py   
- dotnet watch run
dotnet ef dbcontext scaffold "Server=localhost;Database=BillsProject;User Id=login;Password=login;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Entities

dotnet ef migrations add AtualizaBancoPorFavorFunciona --project ../Bills.Domain/Bills.Domain.csproj --startup-project ../Bills.Api/Bills.Api.csproj

dotnet ef database update --project ../Bills.Domain/Bills.Domain.csproj --startup-project ../Bills.Api/Bills.Api.csproj

dotnet ef dbcontext scaffold "Server=localhost;Database=BillsProject;User Id=login;Password=login;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Entities --project ../Bills.Domain/Bills.Domain.csproj --startup-project ../Bills.Api/Bills.Api.csproj


dotnet ef dbcontext scaffold "Server=localhost;Database=BillsProject;User Id=login;Password=login;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Entities --context BillsProjectContext --force --project ../Bills.Domain/Bills.Domain.csproj --startup-project ../Bills.Api/Bills.Api.csproj


create table InstallmentBills (
    id int identity(1,1),
    billsId bigint,
    installment_number int,
    amount money,
    dueDate date,
    status bit
)

ALTER TABLE installmentbills
ADD CONSTRAINT PK_Installmentbills_id
PRIMARY KEY (id)
 
ALTER TABLE installmentbills
ADD CONSTRAINT FK_Installmentbills_BillsId
FOREIGN KEY (BillsId) REFERENCES Bills(Id)


drop table UserPasswordToken
CREATE TABLE UserPasswordToken (
    idPasswordToken BIGINT NOT NULL identity(1,1) primary key,
    userId int NOT NULL,
    token VARCHAR(256) NOT NULL,
    expiresIn DATETIME NOT NULL,
    isUsed BIT NOT NULL DEFAULT 0
)

ALTER TABLE UserPasswordToken
ADD CONSTRAINT FK_UserPasswordToken_userId
FOREIGN KEY (userId) REFERENCES Users(Id)

testsendluizao
mOnitor@@301199


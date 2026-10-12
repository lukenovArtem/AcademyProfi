CREATE DATABASE AcademyProfiDb;
GO
USE AcademyProfiDb;
GO

-- Справочник статусов
CREATE TABLE StatusDict (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE,
    DisplayName NVARCHAR(50) NOT NULL
);

INSERT INTO StatusDict (Name, DisplayName) VALUES 
('New', N'Новая'),
('InProgress', N'В работе'),
('Completed', N'Завершена'),
('Rejected', N'Отклонена');

-- Курсы
CREATE TABLE Courses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    Price DECIMAL(18,2) NOT NULL CHECK (Price >= 0)
);

INSERT INTO Courses (Title, Description, Price) VALUES 
(N'C# Разработка', N'Базовый курс по C#', 15000),
(N'ASP.NET Core', N'Веб-разработка на .NET', 25000),
(N'SQL для начинающих', N'Основы баз данных', 10000);

-- Заявки
CREATE TABLE Applications (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    CourseId INT NOT NULL,
    StatusId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Applications_Courses FOREIGN KEY (CourseId) REFERENCES Courses(Id),
    CONSTRAINT FK_Applications_Status FOREIGN KEY (StatusId) REFERENCES StatusDict(Id)
);

INSERT INTO Applications (StudentName, Email, CourseId, StatusId) VALUES 
(N'Иван Иванов', 'ivan@mail.ru', 1, 1),
(N'Петр Петров', 'petr@mail.ru', 2, 2),
(N'Анна Сидорова', 'anna@mail.ru', 1, 3);
GO
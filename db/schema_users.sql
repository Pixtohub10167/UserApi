-- Схема БД для части 3 (SQLite)
CREATE TABLE Users (
    Id        INTEGER PRIMARY KEY AUTOINCREMENT,
    Login     TEXT NOT NULL,
    PassHash  TEXT NOT NULL,
    CreatedAt TEXT NOT NULL
);

-- Уникальность логина на уровне СУБД
CREATE UNIQUE INDEX IX_Users_Login ON Users(Login);

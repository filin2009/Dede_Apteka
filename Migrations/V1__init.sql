-- Начальная схема: препараты и пользователи.

CREATE TABLE drugs (
    id           SERIAL PRIMARY KEY,
    name         VARCHAR(200)  NOT NULL,
    manufacturer VARCHAR(200)  NOT NULL,
    price        NUMERIC(10,2) NOT NULL,
    quantity     INTEGER       NOT NULL
);

CREATE TABLE users (
    id            SERIAL PRIMARY KEY,
    username      VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(64)  NOT NULL,
    role          VARCHAR(50)  NOT NULL DEFAULT 'user'
);

-- Сид-пользователь для получения токена: admin / admin123
-- password_hash = SHA-256('admin123') в hex.
INSERT INTO users (username, password_hash, role)
VALUES ('admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'admin');

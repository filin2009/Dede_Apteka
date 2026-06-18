-- Таблица для замеров производительности (п.2 пятницы).
-- insert_time — timestamp with timezone (PostgreSQL timestamptz).

CREATE TABLE benchmark (
    id          SERIAL PRIMARY KEY,
    message     VARCHAR(30)              NOT NULL,
    insert_time TIMESTAMP WITH TIME ZONE NOT NULL
);

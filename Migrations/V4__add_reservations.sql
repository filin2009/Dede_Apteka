-- Брони препаратов — для «сложного» POST с валидацией даты (п.3.2 пятницы).

CREATE TABLE reservations (
    id               SERIAL PRIMARY KEY,
    customer_name    VARCHAR(100)             NOT NULL,
    reservation_date TIMESTAMP WITH TIME ZONE NOT NULL
);

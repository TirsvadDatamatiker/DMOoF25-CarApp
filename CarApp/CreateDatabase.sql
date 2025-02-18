CREATE TABLE FuelTypes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Price REAL NOT NULL
);

CREATE TABLE Cars (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Brand TEXT NOT NULL,
    Model TEXT NOT NULL,
    Year INTEGER NOT NULL,
    GearType TEXT NOT NULL,
    FuelTypeId INTEGER,
    FuelEfficiency REAL NOT NULL,
    Mileage INTEGER NOT NULL,
    Description TEXT,
    FOREIGN KEY (FuelTypeId) REFERENCES FuelTypes(Id)
);

INSERT INTO FuelTypes (Name, Price) VALUES ('Benzin', 13.49);
INSERT INTO FuelTypes (Name, Price) VALUES ('Diesel', 12.49);
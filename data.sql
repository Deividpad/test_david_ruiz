ALTER SEQUENCE users.country_id_country_seq RESTART WITH 1;
ALTER SEQUENCE users.department_id_department_seq RESTART WITH 1;
ALTER SEQUENCE users.municipality_id_municipality_seq RESTART WITH 1;
ALTER SEQUENCE users.address_id_address_seq RESTART WITH 1;

---------------COLOMBIA---------------
-- Insert Colombia into country table
INSERT INTO users.country (id_country,country_name) 
VALUES (1,'Colombia');


-- Insert departments for Colombia (assuming the country id is 1)
INSERT INTO users.department (id_department, department_name, country_id) 
VALUES 
(1,'Antioquia', 1),
(2,'Cundinamarca', 1),
(3,'Valle del Cauca', 1),
(4,'Atlántico', 1);


-- Insert municipalities for each department
-- Antioquia (department_id = 1)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('Medellín', 1),
('Envigado', 1),
('Itagüí', 1);

-- Cundinamarca (department_id = 2)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('Bogotá', 2),
('Soacha', 2),
('Chía', 2);

-- Valle del Cauca (department_id = 3)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('Cali', 3),
('Palmira', 3),
('Buenaventura', 3);

-- Atlántico (department_id = 4)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('Barranquilla', 4),
('Soledad', 4),
('Malambo', 4);


---------------ARGENTINA---------------
-- Insert Argentina into country table
INSERT INTO users.country (id_country,country_name) 
VALUES (2,'Argentina');

-- Insert departments for Argentina (assuming the country_id is 2)
INSERT INTO users.department (id_department,department_name, country_id) 
VALUES 
(5,'Buenos Aires', 2),
(6,'Córdoba', 2),
(7,'Santa Fe', 2),
(8,'Mendoza', 2);

-- Insert municipalities for Buenos Aires (department_id = 1)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('La Plata', 5),
('Mar del Plata', 5),
('Bahía Blanca', 5);

-- Insert municipalities for Córdoba (department_id = 2)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('Córdoba', 6),
('Villa María', 6),
('Río Cuarto', 6);

-- Insert municipalities for Santa Fe (department_id = 3)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('Santa Fe', 7),
('Rosario', 7),
('Rafaela', 7);

-- Insert municipalities for Mendoza (department_id = 4)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('Mendoza', 8),
('San Rafael', 8),
('Luján de Cuyo', 8);

---------------BRASIL---------------
-- Insert Brazil into country table
INSERT INTO users.country (id_country,country_name) 
VALUES (3,'Brazil');

-- Insert departments (states) for Brazil (assuming the country_id is 3)
INSERT INTO users.department (id_department,department_name, country_id) 
VALUES 
(9,'São Paulo', 3),
(10,'Rio de Janeiro', 3),
(11,'Minas Gerais', 3),
(12,'Bahia', 3);

-- Insert municipalities for São Paulo (department_id = 1)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('São Paulo', 9),
('Campinas', 9),
('Santos', 9);

-- Insert municipalities for Rio de Janeiro (department_id = 2)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('Rio de Janeiro', 10),
('Niterói', 10),
('Petrópolis', 10);

-- Insert municipalities for Minas Gerais (department_id = 3)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('Belo Horizonte', 11),
('Uberlândia', 11),
('Ouro Preto', 11);

-- Insert municipalities for Bahia (department_id = 4)
INSERT INTO users.municipality (municipality_name, department_id)
VALUES 
('Salvador', 12),
('Feira de Santana', 12),
('Ilhéus', 12);

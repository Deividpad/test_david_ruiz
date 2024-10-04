--CREATE DATABASE db_user;
CREATE SCHEMA users;
--ALTER USER postgres PASSWORD 'postgres';

--DROP TABLE IF EXISTS users.country
CREATE TABLE users.country
(
    id_country SERIAL PRIMARY KEY,
	country_name VARCHAR(60) not null
);

--DROP TABLE IF EXISTS users.department
CREATE TABLE users.department
(
    id_department SERIAL PRIMARY KEY,
	department_name VARCHAR(60) not null,
	country_id INT not null,
	FOREIGN KEY (country_id) REFERENCES users.country(id_country) ON DELETE CASCADE
);

--DROP TABLE IF EXISTS users.municipality
CREATE TABLE users.municipality
(
    id_municipality SERIAL PRIMARY KEY,
	municipality_name VARCHAR(60) not null,
	department_id INT not null,
	FOREIGN KEY (department_id) REFERENCES users.department(id_department) ON DELETE CASCADE
);

--DROP TABLE IF EXISTS users.address
CREATE TABLE users.address
(
    id_address SERIAL PRIMARY KEY,
	neighborhood VARCHAR(50) not null,
	address_description VARCHAR(150) not null,
	municipality_id INT not null,
	FOREIGN KEY (municipality_id) REFERENCES users.municipality(id_municipality) ON DELETE CASCADE	
);

--DROP TABLE IF EXISTS users.user
CREATE TABLE users.user
(
    id_user SERIAL PRIMARY KEY,
	user_name VARCHAR(60) not null,
	phone VARCHAR(20) not null,
	address_id INT not null,
	FOREIGN KEY (address_id) REFERENCES users.address(id_address) ON DELETE CASCADE
);

--GETUSERS()
--DROP FUNCTION IF EXISTS users.getUsers();

CREATE OR REPLACE FUNCTION users.getUsers()
RETURNS TABLE(user_id INT, user_name VARCHAR, phone VARCHAR, 
			  address_id INT, neighborhood VARCHAR, address_description VARCHAR,
			  municipality_id INT, municipality_name VARCHAR,
			  department_id INT, department_name VARCHAR, 
			  country_id INT, country_name VARCHAR) 
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY 
	SELECT u.id_user, u.user_name, u.phone, 
		ad.id_address, ad.neighborhood, ad.address_description,
		mun.id_municipality, mun.municipality_name,
		dep.id_department, dep.department_name, 
		ct.id_country, ct.country_name
	FROM users.user AS u
	INNER JOIN users.address AS ad ON u.address_id = ad.id_address
	INNER JOIN users.municipality AS mun ON ad.municipality_id = mun.id_municipality
	INNER JOIN users.department AS dep ON mun.department_id = dep.id_department
	INNER JOIN users.country AS ct ON dep.country_id = ct.id_country;
END;
$$;

--SELECT * FROM users.getusers();

--GETUSERBYID(ID)
--DROP FUNCTION IF EXISTS users.getUserById;

CREATE OR REPLACE FUNCTION users.getUserById(p_id_user INT)
RETURNS TABLE(user_id INT, user_name VARCHAR, phone VARCHAR, 
			  address_id INT, neighborhood VARCHAR, address_description VARCHAR,
			  municipality_id INT, municipality_name VARCHAR,
			  department_id INT, department_name VARCHAR, 
			  country_id INT, country_name VARCHAR) 
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY 
	SELECT u.id_user, u.user_name, u.phone, 
		ad.id_address, ad.neighborhood, ad.address_description,
		mun.id_municipality as municipality_id, mun.municipality_name,
		dep.id_department, dep.department_name, 
		ct.id_country, ct.country_name
	FROM users.user AS u
	INNER JOIN users.address AS ad ON u.address_id = ad.id_address
	INNER JOIN users.municipality AS mun ON ad.municipality_id = mun.id_municipality
	INNER JOIN users.department AS dep ON mun.department_id = dep.id_department
	INNER JOIN users.country AS ct ON dep.country_id = ct.id_country
	WHERE u.id_user = p_id_user;
END;
$$;

--SELECT * FROM users.getUserById(2);

--INSERT USER
--DROP PROCEDURE IF EXISTS users.insertUser;
CREATE OR REPLACE PROCEDURE users.insertUser(
    IN _user_name VARCHAR(60),
    IN _phone VARCHAR(20),
    IN _address_id INT,
    new_user_id INOUT INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO users.user (user_name, phone, address_id)
    VALUES (_user_name, _phone, _address_id)
    RETURNING id_user INTO new_user_id;
END;
$$;

--CALL users.insertUser('John Doe'::VARCHAR, '555-1234'::VARCHAR, 1::INT, null);

--INSERT ADDRESS
--DROP PROCEDURE IF EXISTS users.insertUser;
CREATE OR REPLACE PROCEDURE users.insertAddress(
    IN _neighborhood VARCHAR(50),
    IN _address_description VARCHAR(150),
    IN _municipality_id INT,
    new_address_id INOUT INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO users.address (neighborhood, address_description, municipality_id)
    VALUES (_neighborhood, _address_description, _municipality_id)
    RETURNING id_address INTO new_address_id;
END;
$$;
--CALL users.insertAddress('ng2'::VARCHAR, 'add desc'::VARCHAR, 1::INT, null);

--GETCOUNTRIES
--DROP FUNCTION IF EXISTS users.getCountries
CREATE OR REPLACE FUNCTION users.getCountries()
RETURNS TABLE(id_country INT, country_name VARCHAR, 
			  id_department INT, department_name VARCHAR,
			  id_municipality INT, municipality_name VARCHAR, mun_department_id INT) 
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY 
	SELECT ct.id_country, ct.country_name,
		dep.id_department as department_id, dep.department_name,
		mun.id_municipality as municipality_id, mun.municipality_name, mun.department_id as mun_department_id
	FROM users.country AS ct
	INNER JOIN users.department AS dep ON ct.id_country = dep.country_id
	INNER JOIN users.municipality AS mun ON dep.id_department = mun.department_id
	ORDER BY ct.country_name, mun.department_id;
END;
$$;

--SELECT * FROM users.getCountries();

--UPDATE USER AND ADDRESS
CREATE OR REPLACE PROCEDURE users.updateUserAndAddress(
    p_id_user INT,
    p_user_name VARCHAR,
    p_phone VARCHAR,
	p_neighborhood VARCHAR,
    p_address_description VARCHAR,
	p_municipality_id INT
) 
LANGUAGE plpgsql
AS $$
DECLARE 
	v_address_id INT;
BEGIN

	SELECT address_id INTO v_address_id
	FROM users.user
	WHERE id_user = p_id_user;
	
	UPDATE users.address 
    SET neighborhood = p_neighborhood,
        address_description = p_address_description,
		municipality_id = p_municipality_id
    WHERE id_address = v_address_id;
	
    UPDATE users.user 
    SET user_name = p_user_name,
        phone = p_phone
    WHERE id_user = p_id_user;
END;
$$;

--DELETE USER AND ADDRESS
CREATE OR REPLACE PROCEDURE users.deleteUserAndAddress(
    p_id_user INT,
	OUT rows_affected INT
) 
LANGUAGE plpgsql
AS $$
DECLARE 
	v_address_id INT;
	v_user_deleted INT := 0;
    v_address_deleted INT := 0;
BEGIN

	SELECT address_id INTO v_address_id
	FROM users.user
	WHERE id_user = p_id_user;
	
	DELETE
	FROM users.user 
    WHERE id_user = p_id_user;
	GET DIAGNOSTICS v_user_deleted = ROW_COUNT;
	
	DELETE
	FROM users.address 
    WHERE id_address = v_address_id;
	GET DIAGNOSTICS v_address_deleted = ROW_COUNT;
	rows_affected := v_user_deleted + v_address_deleted;

END;
$$;

--CALL users.deleteUserAndAddress(9,null);

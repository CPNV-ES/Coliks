-- Date: Feb 2019
-- Author: X. Carrel
-- Goal: Creates the Coliks DB as ASP project material

USE master
GO

-- First delete the database if it exists
IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'Coliks'))
BEGIN
	USE master
	ALTER DATABASE Coliks SET SINGLE_USER WITH ROLLBACK IMMEDIATE; -- Disconnect users the hard way (we cannot drop the db if someone's connected)
	DROP DATABASE Coliks -- Destroy it
END
GO

-- Second ensure we have the proper directory structure
SET NOCOUNT ON;
GO
CREATE TABLE #ResultSet (Directory varchar(200)) -- Temporary table (name starts with #) -> will be automatically destroyed at the end of the session

INSERT INTO #ResultSet EXEC master.sys.xp_subdirs 'c:\' -- Stored procedure that lists subdirectories

IF NOT EXISTS (Select * FROM #ResultSet where Directory = 'DATA')
	EXEC master.sys.xp_create_subdir 'C:\DATA\' -- create DATA

DELETE FROM #ResultSet -- start over for MSSQL subdir
INSERT INTO #ResultSet EXEC master.sys.xp_subdirs 'c:\DATA'

IF NOT EXISTS (Select * FROM #ResultSet where Directory = 'MSSQL')
	EXEC master.sys.xp_create_subdir 'C:\DATA\MSSQL'

DROP TABLE #ResultSet -- Explicitely delete it because the script may be executed multiple times during the same session
GO

-- Everything is ready, we can create the db
CREATE DATABASE Coliks ON  PRIMARY 
( NAME = 'Coliks_data', FILENAME = 'C:\DATA\MSSQL\Coliks.mdf' , SIZE = 20480KB , MAXSIZE = 51200KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = 'Coliks_log', FILENAME = 'C:\DATA\MSSQL\Coliks.ldf' , SIZE = 10240KB , MAXSIZE = 20480KB , FILEGROWTH = 1024KB )

GO

-- Create tables 

USE Coliks
GO

CREATE TABLE categories (
  id int NOT NULL IDENTITY PRIMARY KEY,
  code varchar(5) NOT NULL,
  description varchar(45) NOT NULL)

CREATE TABLE cities (
  id int NOT NULL IDENTITY PRIMARY KEY,
  name varchar(100) NOT NULL)
  
CREATE TABLE contracts (
  id int NOT NULL IDENTITY PRIMARY KEY,
  creationdate datetime DEFAULT NULL,
  effectivereturn datetime DEFAULT NULL,
  plannedreturn datetime DEFAULT NULL,
  customer_id int NOT NULL DEFAULT '0',
  notes text,
  total float DEFAULT '0',
  takenon datetime DEFAULT NULL,
  paidon datetime DEFAULT NULL,
  insurance tinyint DEFAULT NULL,
  goget tinyint DEFAULT NULL,
  help_staff_id int DEFAULT NULL,
  tune_staff_id int DEFAULT NULL)

CREATE TABLE customers (
  id int NOT NULL IDENTITY PRIMARY KEY,
  lastname varchar(50) NOT NULL,
  firstname varchar(50) NOT NULL,
  address varchar(50) DEFAULT NULL,
  city_id int DEFAULT NULL,
  phone varchar(50) DEFAULT NULL,
  email varchar(50) DEFAULT NULL,
  mobile varchar(50) DEFAULT NULL)

CREATE TABLE durations (
  id int NOT NULL IDENTITY PRIMARY KEY,
  code varchar(5) NOT NULL,
  details varchar(45) NOT NULL)

CREATE TABLE geartypes (
  id int NOT NULL IDENTITY PRIMARY KEY,
  name varchar(45) NOT NULL)
  
CREATE TABLE items (
  id int NOT NULL IDENTITY PRIMARY KEY,
  itemnb varchar(50) NOT NULL,
  brand varchar(50) DEFAULT NULL,
  model varchar(50) DEFAULT NULL,
  size int DEFAULT '0',
  category_id int NOT NULL DEFAULT '0',
  cost int DEFAULT '0',
  returned int DEFAULT '0',
  type varchar(50) DEFAULT NULL,
  stock int DEFAULT '0',
  serialnumber varchar(50) DEFAULT NULL)

CREATE TABLE logs (
  id int NOT NULL IDENTITY PRIMARY KEY,
  timestamp datetime DEFAULT NULL,
  text text)
  
CREATE TABLE purchases (
  id int NOT NULL IDENTITY PRIMARY KEY,
  customer_id int DEFAULT '0',
  date datetime DEFAULT NULL,
  description varchar(50) DEFAULT NULL,
  amount float DEFAULT '0')

CREATE TABLE renteditems (
  id int NOT NULL IDENTITY PRIMARY KEY,
  item_id int NOT NULL,
  contract_id int NOT NULL,
  duration_id int NOT NULL,
  category_id int NOT NULL DEFAULT '1',
  price int DEFAULT '0',
  description varchar(255) DEFAULT NULL,
  linenb int DEFAULT '0',
  partialreturn tinyint DEFAULT NULL)

CREATE TABLE rentprices (
  id int NOT NULL IDENTITY PRIMARY KEY,
  category_id int NOT NULL,
  duration_id int NOT NULL,
  geartype_id int NOT NULL,
  price int DEFAULT '0')

CREATE TABLE staffs (
  id int NOT NULL IDENTITY PRIMARY KEY,
  Nom varchar(255) DEFAULT NULL)

ALTER TABLE contracts WITH CHECK ADD 
  CONSTRAINT contract_customer FOREIGN KEY (customer_id) REFERENCES customers (id) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT contract_help FOREIGN KEY (help_staff_id) REFERENCES staffs (id) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT contract_tune FOREIGN KEY (tune_staff_id) REFERENCES staffs (id) ON DELETE NO ACTION ON UPDATE NO ACTION

ALTER TABLE customers WITH CHECK ADD 
  CONSTRAINT customer_city FOREIGN KEY (city_id) REFERENCES cities (id) ON DELETE NO ACTION ON UPDATE NO ACTION

ALTER TABLE items WITH CHECK ADD 
  CONSTRAINT item_category FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE NO ACTION ON UPDATE NO ACTION

ALTER TABLE purchases WITH CHECK ADD 
  CONSTRAINT purchase_customer FOREIGN KEY (customer_id) REFERENCES customers (id) ON DELETE NO ACTION ON UPDATE NO ACTION

ALTER TABLE renteditems WITH CHECK ADD 
  CONSTRAINT renteditem_category FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT renteditem_contract FOREIGN KEY (contract_id) REFERENCES contracts (id) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT renteditem_duration FOREIGN KEY (duration_id) REFERENCES durations (id) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT renteditem_item FOREIGN KEY (item_id) REFERENCES items (id) ON DELETE NO ACTION ON UPDATE NO ACTION

ALTER TABLE rentprices WITH CHECK ADD 
  CONSTRAINT rentprice_category FOREIGN KEY (category_id) REFERENCES categories (id) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT rentprice_duration FOREIGN KEY (duration_id) REFERENCES durations (id) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT rentprice_type FOREIGN KEY (geartype_id) REFERENCES geartypes (id) ON DELETE NO ACTION ON UPDATE NO ACTION

SET NOCOUNT ON;
GO


create view NbRecords
as
SELECT 
    t.NAME AS TableName,
    p.[Rows]
FROM 
    sys.tables t
INNER JOIN      
    sys.indexes i ON t.OBJECT_ID = i.object_id
INNER JOIN 
    sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
INNER JOIN 
    sys.allocation_units a ON p.partition_id = a.container_id
WHERE 
    t.NAME NOT LIKE 'dt%' AND
    i.OBJECT_ID > 255 AND   
    i.index_id <= 1
GROUP BY 
    t.NAME, i.object_id, i.index_id, i.name, p.[Rows]

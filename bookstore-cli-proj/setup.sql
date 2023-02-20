DROP DATABASE IF EXISTS myBookStoredb;
CREATE DATABASE myBookStoredb;
USE myBookStoredb;

CREATE TABLE books (
	id INT PRIMARY KEY AUTO_INCREMENT,
    title VARCHAR(255),
    author VARCHAR(255),  
    price VARCHAR(30),
    qty INT
);

INSERT INTO books (title, author, price, qty) 
VALUES ("LOTR", "JRR Tolkein", 19.99,  100);

INSERT INTO books (title, author, price, qty) 
VALUES ("The Great Gatsby", "F Scott Fitzgerald", 19.99,  100);   
DROP DATABASE IF EXISTS crmtestapi;
CREATE DATABASE crmtestapi 
	DEFAULT CHARACTER SET utf8
	DEFAULT COLLATE utf8_general_ci;

CREATE USER IF NOT EXISTS crmtestapiuser;
GRANT ALL ON crmtestapi.* to 'crmtestapiuser'@'localhost' IDENTIFIED BY 'crmtestapiuserpassword';

CREATE TABLE crmtestapi.user(
	id VARCHAR(36),
	email VARCHAR(50) NOT NULL UNIQUE,
	password VARCHAR(100) NOT NULL,
	name VARCHAR(100) NOT NULL,
	surname VARCHAR(100) NOT NULL,
	deleted_at DATETIME,
	PRIMARY KEY(id)
);


CREATE TABLE crmtestapi.role(
	id VARCHAR(36),
	type VARCHAR(50) NOT NULL,
	user VARCHAR(36) NOT NULL,
	PRIMARY KEY(id),
	UNIQUE(type, user),
	FOREIGN KEY(user) REFERENCES user(id) ON DELETE CASCADE
);

CREATE TABLE crmtestapi.customer(
	id VARCHAR(36),
	name VARCHAR(100) NOT NULL,
	surname VARCHAR(100) NOT NULL,
	photo_name VARCHAR(50),
	created_by VARCHAR(36) NOT NULL,
	last_updated_by VARCHAR(36),
	PRIMARY KEY(id),
	FOREIGN KEY(created_by) REFERENCES user(id) ON DELETE CASCADE, -- ¿ON DELETE?
	FOREIGN KEY(last_updated_by) REFERENCES user(id) ON DELETE CASCADE -- ¿ON DELETE?
);

-- -----------------------------------------------------------------

INSERT INTO `crmtestapi`.`user` (`id`, `email`, `password`, `name`, `surname`) VALUES 
	('11111111-1111-1111-1111-111111111111', 'admin@admin.es', 'AQAAAAEAACcQAAAAEHkiukovlvPkbNFKa8DjhmjuIXaz4GXMm51nadhhKIqTuBcErHzqB7wv6EoAi6CvjQ==', 'John', 'Admin Doe'),
	('22222222-2222-2222-2222-222222222222', 'user@user.es', 'AQAAAAEAACcQAAAAEHkiukovlvPkbNFKa8DjhmjuIXaz4GXMm51nadhhKIqTuBcErHzqB7wv6EoAi6CvjQ==', 'John', 'Doe');

INSERT INTO `crmtestapi`.`role` (`id`, `type`, `user`) VALUES 
	('11111111-1111-1111-1111-111111111111', 'admin', '11111111-1111-1111-1111-111111111111');
	
INSERT INTO `crmtestapi`.`customer` (`id`, `name`, `surname`, `created_by`, `last_updated_by`) VALUES 
	('11111111-1111-1111-1111-111111111111', 'John', 'Doe', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111'),
	('22222222-2222-2222-2222-222222222222', 'John', 'Doe The Second', '22222222-2222-2222-2222-222222222222', NULL);

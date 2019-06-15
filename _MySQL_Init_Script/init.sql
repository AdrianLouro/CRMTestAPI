DROP DATABASE IF EXISTS crmtestapi;
CREATE DATABASE crmtestapi 
	DEFAULT CHARACTER SET utf8
	DEFAULT COLLATE utf8_general_ci;

CREATE USER IF NOT EXISTS crmtestapiuser;
GRANT ALL ON crmtestapi.* to 'crmtestapiuser'@'localhost' IDENTIFIED BY 'crmtestapiuserpassword';

CREATE TABLE crmtestapi.user(
	id VARCHAR(36),
	name VARCHAR(100) NOT NULL,
	surname VARCHAR(100) NOT NULL,
	PRIMARY KEY(id)
);


CREATE TABLE crmtestapi.role(
	id VARCHAR(36),
	type VARCHAR(50) NOT NULL,
	user VARCHAR(36) NOT NULL,
	PRIMARY KEY(id),
	FOREIGN KEY(user) REFERENCES user(id) ON DELETE CASCADE
);

-- -----------------------------------------------------------------

INSERT INTO `crmtestapi`.`user` (`id`, `name`, `surname`) VALUES 
	('11111111-1111-1111-1111-111111111111', 'John', 'Admin Doe'),
	('22222222-2222-2222-2222-222222222222', 'John', 'Doe');

INSERT INTO `crmtestapi`.`role` (`id`, `type`, `user`) VALUES 
	('11111111-1111-1111-1111-111111111111', 'admin', '11111111-1111-1111-1111-111111111111');

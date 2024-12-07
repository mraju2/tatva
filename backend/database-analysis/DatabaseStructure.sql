CREATE TABLE sakila.actor (
  actor_id smallint,
  first_name varchar(45),
  last_name varchar(45),
  last_update timestamp
);
CREATE TABLE sakila.actor_info (
  actor_id smallint,
  first_name varchar(45),
  last_name varchar(45),
  film_info text(65535)
);
CREATE TABLE sakila.address (
  address_id smallint,
  address varchar(50),
  address2 varchar(50),
  district varchar(20),
  city_id smallint,
  postal_code varchar(10),
  phone varchar(20),
  location geometry,
  last_update timestamp
);
CREATE TABLE sakila.category (
  category_id tinyint,
  name varchar(25),
  last_update timestamp
);
CREATE TABLE sakila.city (
  city_id smallint,
  city varchar(50),
  country_id smallint,
  last_update timestamp
);
CREATE TABLE sakila.country (
  country_id smallint,
  country varchar(50),
  last_update timestamp
);
CREATE TABLE sakila.customer (
  customer_id smallint,
  store_id tinyint,
  first_name varchar(45),
  last_name varchar(45),
  email varchar(50),
  address_id smallint,
  active tinyint,
  create_date datetime,
  last_update timestamp
);
CREATE TABLE sakila.customer_list (
  ID smallint,
  name varchar(91),
  address varchar(50),
  zip code varchar(10),
  phone varchar(20),
  city varchar(50),
  country varchar(50),
  notes varchar(6),
  SID tinyint
);
CREATE TABLE sakila.film (
  film_id smallint,
  title varchar(255),
  description text(65535),
  release_year year,
  language_id tinyint,
  original_language_id tinyint,
  rental_duration tinyint,
  rental_rate decimal,
  length smallint,
  replacement_cost decimal,
  rating enum(5),
  special_features set(54),
  last_update timestamp
);
CREATE TABLE sakila.film_actor (
  actor_id smallint,
  film_id smallint,
  last_update timestamp
);
CREATE TABLE sakila.film_category (
  film_id smallint,
  category_id tinyint,
  last_update timestamp
);
CREATE TABLE sakila.film_list (
  FID smallint,
  title varchar(255),
  description text(65535),
  category varchar(25),
  price decimal,
  length smallint,
  rating enum(5),
  actors text(65535)
);
CREATE TABLE sakila.film_text (
  film_id smallint,
  title varchar(255),
  description text(65535)
);
CREATE TABLE sakila.inventory (
  inventory_id mediumint,
  film_id smallint,
  store_id tinyint,
  last_update timestamp
);
CREATE TABLE sakila.language (
  language_id tinyint,
  name char(20),
  last_update timestamp
);
CREATE TABLE sakila.nicer_but_slower_film_list (
  FID smallint,
  title varchar(255),
  description text(65535),
  category varchar(25),
  price decimal,
  length smallint,
  rating enum(5),
  actors text(65535)
);
CREATE TABLE sakila.payment (
  payment_id smallint,
  customer_id smallint,
  staff_id tinyint,
  rental_id int,
  amount decimal,
  payment_date datetime,
  last_update timestamp
);
CREATE TABLE sakila.rental (
  rental_id int,
  rental_date datetime,
  inventory_id mediumint,
  customer_id smallint,
  return_date datetime,
  staff_id tinyint,
  last_update timestamp
);
CREATE TABLE sakila.sales_by_film_category (
  category varchar(25),
  total_sales decimal
);
CREATE TABLE sakila.sales_by_store (
  store varchar(101),
  manager varchar(91),
  total_sales decimal
);
CREATE TABLE sakila.staff (
  staff_id tinyint,
  first_name varchar(45),
  last_name varchar(45),
  address_id smallint,
  picture blob(65535),
  email varchar(50),
  store_id tinyint,
  active tinyint,
  username varchar(16),
  password varchar(40),
  last_update timestamp
);
CREATE TABLE sakila.staff_list (
  ID tinyint,
  name varchar(91),
  address varchar(50),
  zip code varchar(10),
  phone varchar(20),
  city varchar(50),
  country varchar(50),
  SID tinyint
);
CREATE TABLE sakila.store (
  store_id tinyint,
  manager_staff_id tinyint,
  address_id smallint,
  last_update timestamp
);

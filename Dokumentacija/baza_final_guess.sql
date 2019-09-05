DROP TABLE IF EXISTS korisnik;
DROP TABLE IF EXISTS prostorija;
DROP TABLE IF EXISTS spremnik;
DROP TABLE IF EXISTS oznaka;
DROP TABLE IF EXISTS spremnik_oznaka;
DROP TABLE IF EXISTS stavka;
DROP TABLE IF EXISTS stavka_oznaka;
DROP TABLE IF EXISTS dnevnik;


CREATE TABLE korisnik (
    id_korisnik int primary key IDENTITY(1,1),
    korisnicko_ime varchar(50) not null,
    lozinka varchar(50) not null
);

CREATE TABLE prostorija (
    id_prostorija int primary key IDENTITY(1,1),
    naziv_prostorije varchar(50) not null,
    datum_kreiranja datetime not null,
    opis varchar(5000),
    posebne_napomene varchar(1000),
    aktivna varchar(10) not null,
    korisnik_id int,
    foreign key(korisnik_id) references korisnik(id_korisnik)
);

CREATE TABLE spremnik(
    id_spremnik int primary key IDENTITY(1,1),
    naziv_spremnika varchar(50) not null unique,
    datum_kreiranja datetime not null,
    zapremnina float not null,
    zauzeće float DEFAULT 0,
    opis varchar(5000),
    korisnik_id int,
    prostorija_id int references prostorija(id_prostorija) ON DELETE CASCADE,
    foreign key(korisnik_id) references korisnik(id_korisnik)
);



CREATE TABLE oznaka (
    id_oznaka int primary key IDENTITY(1,1),
    naziv varchar(50) not null unique,
    kvarljivost varchar(2) not null,
    aktivna varchar(2) DEFAULT 'da' 
);

CREATE TABLE spremnik_oznaka (
    spremnik_id int,
    oznaka_id int,
    PRIMARY KEY(spremnik_id,oznaka_id),
    foreign key (spremnik_id) references spremnik(id_spremnik),
    foreign key (oznaka_id) references oznaka(id_oznaka)

);

CREATE TABLE stavka(
    id_stavka int primary key IDENTITY(1,1),
    naziv varchar(50) not null,
    datum_kreiranja datetime not null,
    datum_roka date,
    zauzeće float not null,
    korisnik_id int,
    spremnik_id int references spremnik(id_spremnik)  ON DELETE CASCADE,
    foreign key(korisnik_id) references korisnik(id_korisnik)
);

CREATE TABLE stavka_oznaka (
    stavka_id int,
    oznaka_id int,
    primary key (stavka_id,oznaka_id),
    foreign key (stavka_id) references stavka(id_stavka) ON DELETE CASCADE,
    foreign key (oznaka_id) references oznaka(id_oznaka) ON DELETE CASCADE
);

CREATE TABLE dnevnik (
    id_dnevnik int primary key IDENTITY(1,1),
    radnja varchar(15),
    datum datetime not null,
    kolicina float not null,
    stavka_id int,
    korisnik_id int,
    foreign key(stavka_id) references stavka(id_stavka),
    foreign key(korisnik_id) references korisnik(id_korisnik)
);


INSERT INTO korisnik (korisnicko_ime, lozinka)
VALUES ('domagoj', 'domagojjesuper');

INSERT INTO korisnik (korisnicko_ime, lozinka)
VALUES ('filip', 'nou');

INSERT INTO korisnik (korisnicko_ime, lozinka)
VALUES ('dominik', 'domagojnijesuper');

INSERT INTO spremnik(naziv_spremnika, datum_kreiranja, zapremnina, opis, prostorija_id, tip_id)
VALUES ('Spremnik 1', '2017-06-16', '15.5', 'spremnik u kojem je nekaj', '1', '1');


INSERT INTO oznaka(naziv, kvarljivost)
VALUES ('Mlijeko', 'da');

INSERT INTO oznaka(naziv, kvarljivost)
VALUES ('Kruh', 'da');

INSERT INTO oznaka(naziv, kvarljivost, aktivna)
VALUES ('Drvo', 0 ,'ne');
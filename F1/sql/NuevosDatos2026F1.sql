INSERT INTO RACE (GP_NAME, CIRCUIT_NAME, COUNTRY, GP_DATE_START, GP_DATE_END, GP_LENGTH, GP_DISTANCE, TURN, LAP, LAST_WINNER, FAST_LAP, GP_IMG) VALUES
('Gran Premio de Australia', 'Albert Park', 'Australia', '2026-03-06','2026-03-08', 5300, 307.4, 16, 58, 'Lando Norris', 'Charles Leclerc 1:19.813', 'AU.png'),
('Gran Premio de China', 'Shanghai International Circuit', 'China', '2026-03-13','2026-03-15', 5450, 305.2, 16, 56, 'Oscar Piastri', 'Michael Schumacher 1:32.238', 'CN.png'),
('Gran Premio de Japón', 'Suzuka Circuit', 'Japan', '2026-03-27','2026-03-29', 5800, 307.4, 18, 53, 'Max Verstappen', 'Kimi Antonelli 1:30.965', 'JP.png'),
('Gran Premio de Baréin', 'Sakhir', 'Bahrain', '2026-04-10','2026-04-12', 5400, 307.8, 15, 57, 'Oscar Piastri', 'Pedro de la Rosa 1:31.447', 'BH.png'),
('Gran Premio de Arabia Saudí', 'Jeddah Corniche', 'Saudi Arabia', '2026-04-17','2026-04-19', 6160, 308.0, 27, 50, 'Oscar Piastri', 'Lewis Hamilton 1:30.734', 'SA.png'),
('Gran Premio de Miami', 'Miami Gardens', 'USA', '2026-05-01','2026-05-03', 5410, 308.4, 19, 57, 'Oscar Piastri', 'Max Verstappen 1:29.708', 'USM.png'),
('Gran Premio de Canadá', 'Circuit Gilles Villeneuve', 'Canada', '2026-05-22','2026-05-24', 4361, 305.3, 12, 70, 'George Russell', 'Michael Schumacher 1:13.078', 'CA.png'),
('Gran Premio de Mónaco', 'Circuit de Monaco', 'Monaco', '2026-06-05','2026-06-07', 3340, 260.5, 19, 78, 'Lando Norris', 'Lewis Hamilton 1:12.909', 'MC.png'),
('Gran Premio de España', 'Barcelona-Catalunya', 'Spain', '2026-06-12','2026-06-14', 4655, 307.2, 16, 66, 'Oscar Piastri', 'Oscar Piastri 1:15.743', 'ES.png'),
('Gran Premio de Austria', 'Red Bull Ring', 'Austria', '2026-06-26','2026-06-28', 4326, 298.5, 10, 69, 'Lando Norris', 'Oscar Piastri 1:07.924', 'AT.png'),
('Gran Premio de Gran Bretaña', 'Silverstone', 'United Kingdom', '2026-07-03','2026-07-05', 5891, 306.0, 18, 52, 'Lando Norris', 'Lewis Hamilton 1:24.303', 'UK.png'),
('Gran Premio de Bélgica', 'Spa-Francorchamps', 'Belgium', '2026-07-17','2026-07-19', 7004, 308.0, 19, 44, 'Oscar Piastri', 'Lewis Hamilton 1:44.000', 'BE.png'),
('Gran Premio de Hungría', 'Hungaroring', 'Hungary', '2026-07-24','2026-07-26', 4381, 307.0, 14, 70, 'Lando Norris', 'Lewis Hamilton 1:13.447', 'HU.png'),
('Gran Premio de Países Bajos', 'Zandvoort', 'Netherlands', '2026-08-21','2026-08-23', 4259, 306.5, 14, 72, 'Oscar Piastri', 'Max Verstappen 1:08.885', 'NL.png'),
('Gran Premio de Italia', 'Monza', 'Italy', '2026-09-04','2026-09-06', 5793, 305.0, 11, 53, 'Max Verstappen', 'Lewis Hamilton 1:18.887', 'IT.png'),
('Gran Premio de España – Madrid', 'Circuito de Madrid', 'Spain', '2026-09-11','2026-09-13', 5000, 290.0, 16, 58, '', '', 'MD.png'),
('Gran Premio de Azerbaiyán', 'Baku City Circuit', 'Azerbaijan', '2026-09-24','2026-09-26', 6003, 260.1, 20, 51, 'Max Verstappen', 'Max Verstappen 1:43.388', 'AZ.png'),
('Gran Premio de Singapur', 'Marina Bay', 'Singapore', '2026-10-09','2026-10-11', 5073, 308.7, 23, 61, 'George Russell', 'Lewis Hamilton 1:33.808', 'SG.png'),
('Gran Premio de Estados Unidos', 'Circuit of The Americas', 'USA', '2026-10-23','2026-10-25', 5513, 308.7, 20, 56, 'Max Verstappen', 'Kimi Antonelli 1:37.577', 'US.png'),
('Gran Premio de México', 'Autódromo Hermanos Rodríguez', 'Mexico', '2026-10-30','2026-11-01', 4421, 306.0, 16, 69, 'Lando Norris', 'George Russell 1:20.052', 'MX.png'),
('Gran Premio de Brasil', 'Interlagos', 'Brazil', '2026-11-06','2026-11-08', 4309, 305.8, 15, 71, 'Oscar Piastri', 'Alex Albon 1:12.400', 'BR.png'),
('Gran Premio de Las Vegas', 'Las Vegas Street Circuit', 'USA', '2026-11-19','2026-11-21', 6000, 360.0, 14, 60, 'Max Verstappen', 'Max Verstappen 1:33.365', 'LV.png'),
('Gran Premio de Qatar', 'Lusail International Circuit', 'Qatar', '2026-11-27','2026-11-29', 5380, 307.7, 16, 57, 'Oscar Piastri', 'Oscar Piastri 1:22.996', 'QA.png'),
('Gran Premio de Abu Dhabi', 'Yas Marina Circuit', 'UAE', '2026-12-04','2026-12-06', 5554, 340.6, 21, 58, 'Max Verstappen', 'Charles Leclerc 1:26.725', 'AE.png');

-- Equipos 2026
INSERT INTO TEAM (TEAM_NAME, PRICE, TOTAL_POINTS, TEAM_IMG) VALUES
('Mercedes-AMG', 28.0, 0, 'mercedes.png'),
('Oracle Red Bull Racing', 29.0, 0, 'redbull.png'),
('Scuderia Ferrari', 27.5, 0, 'ferrari.png'),
('McLaren', 18.0, 0, 'mclaren.png'),
('Williams F1 Team', 9.5, 0, 'williams.png'),
('Aston Martin Aramco F1 Team', 15.2, 0, 'astonmartin.png'),
('TGR Haas F1 Team', 8.9, 0, 'haas.png'),
('Alpine F1 Team', 10.1, 0, 'alpine.png'),
('Audi F1 Team', 11.3, 0, 'audi.png'),
('Cadillac F1 Team', 7.8, 0, 'cadillac.png'),
('Visa Cash App Racing Bulls', 12.5, 0, 'racingbulls.png');

-- Pilotos 2026
INSERT INTO DRIVER (DRIVER_NAME, NACIONALITY, PRICE, TOTAL_POINTS, ID_TEAM, DRIVER_IMG) VALUES
('George Russell','United Kingdom',27.0,0,1,'russell.png'),
('Kimi Antonelli','Italy',12.5,0,1,'antonelli.png'),

('Max Verstappen','Netherlands',29.5,0,2,'verstappen.png'),
('Isack Hadjar','France',18.0,0,2,'hadjar.png'),

('Charles Leclerc','Monaco',28.0,0,3,'leclerc.png'),
('Lewis Hamilton','United Kingdom',28.5,0,3,'hamilton.png'),

('Lando Norris','United Kingdom',20.0,0,4,'norris.png'),
('Oscar Piastri','Australia',19.0,0,4,'piastri.png'),

('Alexander Albon','Thailand',9.8,0,5,'albon.png'),
('Carlos Sainz Jr.','Spain',9.7,0,5,'sainz.png'),

('Fernando Alonso','Spain',17.5,0,6,'alonso.png'),
('Lance Stroll','Canada',15.8,0,6,'stroll.png'),

('Esteban Ocon','France',8.7,0,7,'ocon.png'),
('Oliver Bearman','United Kingdom',8.9,0,7,'bearman.png'),

('Pierre Gasly','France',11.0,0,8,'gasly.png'),
('Franco Colapinto','Argentina',10.5,0,8,'colapinto.png'),

('Nico Hülkenberg','Germany',10.0,0,9,'hulkenberg.png'),
('Gabriel Bortoleto','Brazil',9.2,0,9,'bortoleto.png'),

('Sergio Pérez','Mexico',16.8,0,10,'perez.png'),
('Valtteri Bottas','Finland',14.2,0,10,'bottas.png'),

('Liam Lawson','New Zealand',12.0,0,11,'lawson.png'),
('Arvid Lindblad','United Kingdom',8.0,0,11,'lindblad.png');

-- AUSTRALIA (1)
INSERT INTO SCHEDULE (SCHEDULE_NAME, SCHEDULE_DAY, SCHEDULE_TIME, ID_RACE) VALUES
('Free Practice 1','Friday','12:30:00',1),
('Free Practice 2','Friday','16:00:00',1),
('Free Practice 3','Saturday','12:30:00',1),
('Qualifying','Saturday','16:00:00',1),
('Race','Sunday','15:00:00',1);

-- CHINA (2) — Sprint weekend
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','11:30:00',2),
('Sprint Qualifying','Friday','15:30:00',2),
('Sprint','Saturday','11:00:00',2),
('Qualifying','Saturday','15:00:00',2),
('Race','Sunday','15:00:00',2);

-- JAPÓN (3)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','11:30:00',3),
('Free Practice 2','Friday','15:00:00',3),
('Free Practice 3','Saturday','11:30:00',3),
('Qualifying','Saturday','15:00:00',3),
('Race','Sunday','14:00:00',3);

-- BAHREIN (4)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','14:30:00',4),
('Free Practice 2','Friday','18:00:00',4),
('Free Practice 3','Saturday','15:30:00',4),
('Qualifying','Saturday','19:00:00',4),
('Race','Sunday','18:00:00',4);

-- ARABIA SAUDÍ (5)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','16:30:00',5),
('Free Practice 2','Friday','20:00:00',5),
('Free Practice 3','Saturday','16:30:00',5),
('Qualifying','Saturday','20:00:00',5),
('Race','Sunday','20:00:00',5);

-- MIAMI (6) — Sprint weekend
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','12:30:00',6),
('Sprint Qualifying','Friday','16:30:00',6),
('Sprint','Saturday','12:00:00',6),
('Qualifying','Saturday','16:00:00',6),
('Race','Sunday','16:00:00',6);

-- CANADÁ (7) — Sprint weekend
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','12:30:00',7),
('Sprint Qualifying','Friday','16:30:00',7),
('Sprint','Saturday','12:00:00',7),
('Qualifying','Saturday','16:00:00',7),
('Race','Sunday','16:00:00',7);

-- MÓNACO (8)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','13:30:00',8),
('Free Practice 2','Friday','17:00:00',8),
('Free Practice 3','Saturday','12:30:00',8),
('Qualifying','Saturday','16:00:00',8),
('Race','Sunday','15:00:00',8);

-- BARCELONA (9)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','13:30:00',9),
('Free Practice 2','Friday','17:00:00',9),
('Free Practice 3','Saturday','12:30:00',9),
('Qualifying','Saturday','16:00:00',9),
('Race','Sunday','15:00:00',9);

-- AUSTRIA (10)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','13:30:00',10),
('Free Practice 2','Friday','17:00:00',10),
('Free Practice 3','Saturday','12:30:00',10),
('Qualifying','Saturday','16:00:00',10),
('Race','Sunday','15:00:00',10);

-- GRAN BRETAÑA (11) — Sprint weekend
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','12:30:00',11),
('Sprint Qualifying','Friday','16:30:00',11),
('Sprint','Saturday','12:00:00',11),
('Qualifying','Saturday','16:00:00',11),
('Race','Sunday','15:00:00',11);

-- BÉLGICA (12)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','13:30:00',12),
('Free Practice 2','Friday','17:00:00',12),
('Free Practice 3','Saturday','12:30:00',12),
('Qualifying','Saturday','16:00:00',12),
('Race','Sunday','15:00:00',12);

-- HUNGRÍA (13)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','13:30:00',13),
('Free Practice 2','Friday','17:00:00',13),
('Free Practice 3','Saturday','12:30:00',13),
('Qualifying','Saturday','16:00:00',13),
('Race','Sunday','15:00:00',13);

-- PAÍSES BAJOS (14) — Sprint weekend
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','12:30:00',14),
('Sprint Qualifying','Friday','16:30:00',14),
('Sprint','Saturday','12:00:00',14),
('Qualifying','Saturday','16:00:00',14),
('Race','Sunday','15:00:00',14);

-- ITALIA (15)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','12:30:00',15),
('Free Practice 2','Friday','16:00:00',15),
('Free Practice 3','Saturday','12:30:00',15),
('Qualifying','Saturday','16:00:00',15),
('Race','Sunday','15:00:00',15);

-- MADRID (16)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','12:30:00',16),
('Free Practice 2','Friday','16:00:00',16),
('Free Practice 3','Saturday','12:30:00',16),
('Qualifying','Saturday','16:00:00',16),
('Race','Sunday','15:00:00',16);

-- AZERBAIYÁN (17)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','12:00:00',17),
('Free Practice 2','Friday','16:00:00',17),
('Free Practice 3','Saturday','11:30:00',17),
('Qualifying','Saturday','15:00:00',17),
('Race','Sunday','15:00:00',17);

-- SINGAPUR (18)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','17:00:00',18),
('Free Practice 2','Friday','21:00:00',18),
('Free Practice 3','Saturday','17:00:00',18),
('Qualifying','Saturday','21:00:00',18),
('Race','Sunday','20:00:00',18);

-- ESTADOS UNIDOS (19)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','13:00:00',19),
('Free Practice 2','Friday','17:00:00',19),
('Free Practice 3','Saturday','12:30:00',19),
('Qualifying','Saturday','16:00:00',19),
('Race','Sunday','15:00:00',19);

-- MÉXICO (20)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','12:30:00',20),
('Free Practice 2','Friday','16:30:00',20),
('Free Practice 3','Saturday','12:00:00',20),
('Qualifying','Saturday','15:00:00',20),
('Race','Sunday','14:00:00',20);

-- BRASIL (21)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','12:30:00',21),
('Free Practice 2','Friday','16:30:00',21),
('Free Practice 3','Saturday','12:00:00',21),
('Qualifying','Saturday','15:00:00',21),
('Race','Sunday','14:00:00',21);

-- LAS VEGAS (22)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','17:00:00',22),
('Free Practice 2','Friday','21:00:00',22),
('Free Practice 3','Saturday','17:00:00',22),
('Qualifying','Saturday','21:00:00',22),
('Race','Sunday','20:00:00',22);

-- QATAR (23)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','18:00:00',23),
('Free Practice 2','Friday','22:00:00',23),
('Free Practice 3','Saturday','18:00:00',23),
('Qualifying','Saturday','22:00:00',23),
('Race','Sunday','19:00:00',23);

-- ABU DHABI (24)
INSERT INTO SCHEDULE VALUES
('Free Practice 1','Friday','17:00:00',24),
('Free Practice 2','Friday','20:30:00',24),
('Free Practice 3','Saturday','17:00:00',24),
('Qualifying','Saturday','18:00:00',24),
('Race','Sunday','17:00:00',24);

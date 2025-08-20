-- MySQL dump 10.13  Distrib 8.0.42, for Linux (x86_64)
--
-- Host: poliedro-cluster-mysql-u44828.vm.elestio.app    Database: eds
-- ------------------------------------------------------
-- Server version	8.0.42


CREATE DATABASE IF NOT EXISTS eds_new;
USE eds_new;


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `business`
--

DROP TABLE IF EXISTS `business`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `business` (
  `id_business` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `context` varchar(100) NOT NULL,
  PRIMARY KEY (`id_business`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `capacity`
--

DROP TABLE IF EXISTS `capacity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `capacity` (
  `id_capacity` int NOT NULL AUTO_INCREMENT,
  `code` varchar(45) NOT NULL,
  `height` double NOT NULL,
  `gallon` double NOT NULL,
  `liters` int NOT NULL,
  `date` date NOT NULL DEFAULT (curdate()),
  PRIMARY KEY (`id_capacity`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `category`
--

DROP TABLE IF EXISTS `category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `category` (
  `idcategory` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`idcategory`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `compartiment`
--

DROP TABLE IF EXISTS `compartiment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compartiment` (
  `id_compartiment` int NOT NULL AUTO_INCREMENT,
  `number` int NOT NULL,
  `nominal` double NOT NULL,
  `operative` double NOT NULL,
  `stock` double NOT NULL,
  `height` double NOT NULL,
  `id_tank` int NOT NULL,
  `date` date NOT NULL DEFAULT (curdate()),
  PRIMARY KEY (`id_compartiment`),
  KEY `fk_compartiment_tank1_idx` (`id_tank`) USING BTREE,
  CONSTRAINT `fk_compartiment_tank1` FOREIGN KEY (`id_tank`) REFERENCES `tank` (`id_tank`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `compartiment_capacity`
--

DROP TABLE IF EXISTS `compartiment_capacity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compartiment_capacity` (
  `id_compartiment_capacity` int NOT NULL AUTO_INCREMENT,
  `id_compartiment` int NOT NULL,
  `id_capacity` int NOT NULL,
  `is_default` tinyint NOT NULL,
  PRIMARY KEY (`id_compartiment_capacity`),
  KEY `fk_compartiment_capacity_compartiment1` (`id_compartiment`),
  KEY `fk_compartiment_capacity_capacity1` (`id_capacity`),
  CONSTRAINT `fk_compartiment_capacity_capacity1` FOREIGN KEY (`id_capacity`) REFERENCES `capacity` (`id_capacity`),
  CONSTRAINT `fk_compartiment_capacity_compartiment1` FOREIGN KEY (`id_compartiment`) REFERENCES `compartiment` (`id_compartiment`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `court`
--

DROP TABLE IF EXISTS `court`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `court` (
  `id_court` int NOT NULL AUTO_INCREMENT,
  `id_islander` int NOT NULL,
  `date_starttime` date NOT NULL,
  `starttime` time NOT NULL,
  `date_endtime` date NOT NULL,
  `endtime` time NOT NULL,
  `consecutive` int NOT NULL,
  `id_eds` int NOT NULL,
  `descripcion` varchar(100) DEFAULT NULL,
  `distintic` double DEFAULT NULL,
  PRIMARY KEY (`id_court`),
  KEY `fk_court_islander_idx` (`id_islander`),
  KEY `fk_court_eds1_idx` (`id_eds`),
  CONSTRAINT `fk_court_eds1` FOREIGN KEY (`id_eds`) REFERENCES `eds` (`id_eds`),
  CONSTRAINT `fk_court_islander` FOREIGN KEY (`id_islander`) REFERENCES `islander` (`id_islander`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `court_dispensers`
--

DROP TABLE IF EXISTS `court_dispensers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `court_dispensers` (
  `id_court_dispensers` int NOT NULL AUTO_INCREMENT,
  `id_court` int NOT NULL,
  `accumulated_amount` double NOT NULL,
  `accumulated_gallons` double NOT NULL,
  `id_product` int NOT NULL,
  `id_compartiment` int NOT NULL,
  `id_hose` int NOT NULL,
  PRIMARY KEY (`id_court_dispensers`),
  KEY `fk_court_has_dispensers_court1_idx` (`id_court`),
  KEY `fk_court_dispensers_product1_idx` (`id_product`),
  KEY `fk_court_dispensers_compartiment1_idx` (`id_compartiment`),
  KEY `fk_court_dispensers_hose1_idx` (`id_hose`),
  CONSTRAINT `fk_court_dispensers_compartiment1` FOREIGN KEY (`id_compartiment`) REFERENCES `compartiment` (`id_compartiment`),
  CONSTRAINT `fk_court_dispensers_hose1` FOREIGN KEY (`id_hose`) REFERENCES `hose` (`id_hose`),
  CONSTRAINT `fk_court_dispensers_product1` FOREIGN KEY (`id_product`) REFERENCES `product` (`id_product`),
  CONSTRAINT `fk_court_has_dispensers_court1` FOREIGN KEY (`id_court`) REFERENCES `court` (`id_court`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`%`*/ /*!50003 TRIGGER `trg_insert_hose_history` AFTER INSERT ON `court_dispensers` FOR EACH ROW BEGIN
    DECLARE court_date DATE;
    DECLARE v_id_dispensers INT;

    SELECT date_starttime INTO court_date
    FROM court
    WHERE id_court = NEW.id_court;

    SELECT id_dispensers INTO v_id_dispensers
    FROM hose
    WHERE id_hose = NEW.id_hose;

    INSERT INTO hose_history (
        id_hose,
        id_dispensers,
        accumulated_amount,
        accumulated_gallons,
        date
    ) VALUES (
        NEW.id_hose,
        v_id_dispensers,
        NEW.accumulated_amount,
        NEW.accumulated_gallons,
        court_date
    );
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`%`*/ /*!50003 TRIGGER `trg_update_hose_accumulated` AFTER INSERT ON `court_dispensers` FOR EACH ROW BEGIN
    UPDATE hose
    SET 
        accumulated_amount = NEW.accumulated_amount,
        accumulated_gallons = NEW.accumulated_gallons
    WHERE id_hose = NEW.id_hose;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `court_dispensers_inventory`
--

DROP TABLE IF EXISTS `court_dispensers_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `court_dispensers_inventory` (
  `id_court_dispensers_inventorycol` int NOT NULL AUTO_INCREMENT,
  `id_court_dispensers` int NOT NULL,
  `id_inventory` int NOT NULL,
  PRIMARY KEY (`id_court_dispensers_inventorycol`),
  KEY `fk_court_dispensers_has_inventory_inventory1_idx` (`id_inventory`),
  KEY `fk_court_dispensers_has_inventory_court_dispensers1_idx` (`id_court_dispensers`),
  CONSTRAINT `fk_court_dispensers_has_inventory_court_dispensers1` FOREIGN KEY (`id_court_dispensers`) REFERENCES `court_dispensers` (`id_court_dispensers`),
  CONSTRAINT `fk_court_dispensers_has_inventory_inventory1` FOREIGN KEY (`id_inventory`) REFERENCES `inventory` (`id_inventory`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `court_document`
--

DROP TABLE IF EXISTS `court_document`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `court_document` (
  `idcourt_document` int NOT NULL AUTO_INCREMENT,
  `descripcion` text NOT NULL,
  `id_court` int NOT NULL,
  PRIMARY KEY (`idcourt_document`),
  KEY `fk_court_document_court1_idx` (`id_court`),
  CONSTRAINT `fk_court_document_court1` FOREIGN KEY (`id_court`) REFERENCES `court` (`id_court`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `court_expenditures`
--

DROP TABLE IF EXISTS `court_expenditures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `court_expenditures` (
  `id_court_expenditure` int NOT NULL AUTO_INCREMENT,
  `id_court` int NOT NULL,
  `id_expenditures` int NOT NULL,
  `amount` double NOT NULL,
  `description` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id_court_expenditure`),
  KEY `fk_court_has_expenditures_expenditures1_idx` (`id_expenditures`),
  KEY `fk_court_has_expenditures_court1_idx` (`id_court`),
  CONSTRAINT `fk_court_has_expenditures_court1` FOREIGN KEY (`id_court`) REFERENCES `court` (`id_court`),
  CONSTRAINT `fk_court_has_expenditures_expenditures1` FOREIGN KEY (`id_expenditures`) REFERENCES `expenditures` (`id_expenditures`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `court_type_of_collection`
--

DROP TABLE IF EXISTS `court_type_of_collection`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `court_type_of_collection` (
  `id_court_type_of_collection` int NOT NULL AUTO_INCREMENT,
  `id_court` int NOT NULL,
  `id_type_of_collection` int NOT NULL,
  `amount` double NOT NULL,
  `description` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id_court_type_of_collection`),
  KEY `fk_court_has_type_of_collection_type_of_collection1_idx` (`id_type_of_collection`),
  KEY `fk_court_has_type_of_collection_court1_idx` (`id_court`),
  CONSTRAINT `fk_court_has_type_of_collection_court1` FOREIGN KEY (`id_court`) REFERENCES `court` (`id_court`),
  CONSTRAINT `fk_court_has_type_of_collection_type_of_collection1` FOREIGN KEY (`id_type_of_collection`) REFERENCES `type_of_collection` (`id_type_of_collection`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dispenser_type`
--

DROP TABLE IF EXISTS `dispenser_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dispenser_type` (
  `id_dispenser_type` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`id_dispenser_type`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dispensers`
--

DROP TABLE IF EXISTS `dispensers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dispensers` (
  `id_dispensers` int NOT NULL AUTO_INCREMENT,
  `code` varchar(45) NOT NULL,
  `number` int NOT NULL,
  `id_dispenser_type` int NOT NULL,
  `id_eds` int NOT NULL,
  `idisland` int NOT NULL,
  `number_hose` int NOT NULL,
  PRIMARY KEY (`id_dispensers`),
  KEY `fk_dispensers_dispenser_type1_idx` (`id_dispenser_type`),
  KEY `fk_dispensers_eds1_idx` (`id_eds`),
  KEY `fk_dispensers_island1_idx` (`idisland`),
  CONSTRAINT `fk_dispensers_dispenser_type1` FOREIGN KEY (`id_dispenser_type`) REFERENCES `dispenser_type` (`id_dispenser_type`),
  CONSTRAINT `fk_dispensers_eds1` FOREIGN KEY (`id_eds`) REFERENCES `eds` (`id_eds`),
  CONSTRAINT `fk_dispensers_island1` FOREIGN KEY (`idisland`) REFERENCES `island` (`idisland`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `eds`
--

DROP TABLE IF EXISTS `eds`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `eds` (
  `id_eds` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `nit` varchar(45) NOT NULL,
  `address` varchar(100) NOT NULL,
  `sicom` varchar(45) NOT NULL,
  `idbusiness` int NOT NULL,
  `date` date NOT NULL DEFAULT (curdate()),
  PRIMARY KEY (`id_eds`),
  KEY `fk_eds_business1_idx` (`idbusiness`),
  CONSTRAINT `fk_eds_business1` FOREIGN KEY (`idbusiness`) REFERENCES `business` (`id_business`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `eds_tank`
--

DROP TABLE IF EXISTS `eds_tank`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `eds_tank` (
  `id_eds_tank` int NOT NULL AUTO_INCREMENT,
  `id_eds` int NOT NULL,
  `id_tank` int NOT NULL,
  PRIMARY KEY (`id_eds_tank`),
  KEY `fk_eds_has_tank_tank1_idx` (`id_tank`),
  KEY `fk_eds_has_tank_eds1_idx` (`id_eds`),
  CONSTRAINT `fk_eds_has_tank_eds1` FOREIGN KEY (`id_eds`) REFERENCES `eds` (`id_eds`),
  CONSTRAINT `fk_eds_has_tank_tank1` FOREIGN KEY (`id_tank`) REFERENCES `tank` (`id_tank`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `expenditures`
--

DROP TABLE IF EXISTS `expenditures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `expenditures` (
  `id_expenditures` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`id_expenditures`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `hose`
--

DROP TABLE IF EXISTS `hose`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hose` (
  `id_hose` int NOT NULL AUTO_INCREMENT,
  `id_dispensers` int NOT NULL,
  `number` int NOT NULL,
  `accumulated_amount` double NOT NULL,
  `accumulated_gallons` double NOT NULL,
  `id_product_type` int NOT NULL,
  PRIMARY KEY (`id_hose`),
  KEY `fk_house_dispensers1_idx` (`id_dispensers`),
  KEY `fk_hose_product_type1_idx` (`id_product_type`),
  CONSTRAINT `fk_hose_product_type1` FOREIGN KEY (`id_product_type`) REFERENCES `product_type` (`id_product_type`),
  CONSTRAINT `fk_house_dispensers1` FOREIGN KEY (`id_dispensers`) REFERENCES `dispensers` (`id_dispensers`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `hose_history`
--

DROP TABLE IF EXISTS `hose_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hose_history` (
  `id_hose_hose_history` int NOT NULL AUTO_INCREMENT,
  `id_hose` int NOT NULL,
  `id_dispensers` int NOT NULL,
  `accumulated_amount` double NOT NULL,
  `accumulated_gallons` double NOT NULL,
  `date` date NOT NULL,
  PRIMARY KEY (`id_hose_hose_history`),
  KEY `fk_hose_has_hose_history_hose1_idx` (`id_hose`),
  KEY `fk_hose_history_dispensers1_idx` (`id_dispensers`),
  CONSTRAINT `fk_hose_has_hose_history_hose1` FOREIGN KEY (`id_hose`) REFERENCES `hose` (`id_hose`),
  CONSTRAINT `fk_hose_history_dispensers1` FOREIGN KEY (`id_dispensers`) REFERENCES `dispensers` (`id_dispensers`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `inventory`
--

DROP TABLE IF EXISTS `inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventory` (
  `id_inventory` int NOT NULL AUTO_INCREMENT,
  `date` date NOT NULL,
  `reference_type` varchar(20) DEFAULT NULL,
  `reference_id` int DEFAULT NULL,
  PRIMARY KEY (`id_inventory`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`%`*/ /*!50003 TRIGGER `trg_insert_court_dispensers_inventory_from_inventory` AFTER INSERT ON `inventory` FOR EACH ROW BEGIN
    DECLARE v_id_court INT;

    SELECT id_court INTO v_id_court
    FROM court
    WHERE id_court = NEW.reference_id
    AND 'court' = NEW.reference_type;

    INSERT INTO court_dispensers_inventory (
        id_court_dispensers,
        id_inventory
    )
    SELECT id_court_dispensers, NEW.id_inventory
    FROM court_dispensers
    WHERE id_court = v_id_court;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `island`
--

DROP TABLE IF EXISTS `island`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `island` (
  `idisland` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`idisland`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `islander`
--

DROP TABLE IF EXISTS `islander`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `islander` (
  `id_islander` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `id_eds` int NOT NULL,
  `password` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  PRIMARY KEY (`id_islander`),
  KEY `fk_islander_eds1_idx` (`id_eds`),
  CONSTRAINT `fk_islander_eds1` FOREIGN KEY (`id_eds`) REFERENCES `eds` (`id_eds`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product` (
  `id_product` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `id_product_type` int NOT NULL,
  `price` double NOT NULL,
  `date` date NOT NULL DEFAULT (curdate()),
  PRIMARY KEY (`id_product`),
  KEY `fk_product_product_type1_idx` (`id_product_type`),
  CONSTRAINT `fk_product_product_type1` FOREIGN KEY (`id_product_type`) REFERENCES `product_type` (`id_product_type`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `product_compartiment`
--

DROP TABLE IF EXISTS `product_compartiment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_compartiment` (
  `id_product_compartiment` int NOT NULL AUTO_INCREMENT,
  `id_product` int NOT NULL,
  `id_compartiment` int NOT NULL,
  `stock` double NOT NULL,
  PRIMARY KEY (`id_product_compartiment`),
  KEY `fk_product_has_compartiment_compartiment1_idx` (`id_compartiment`),
  KEY `fk_product_has_compartiment_product1_idx` (`id_product`),
  CONSTRAINT `fk_product_has_compartiment_compartiment1` FOREIGN KEY (`id_compartiment`) REFERENCES `compartiment` (`id_compartiment`),
  CONSTRAINT `fk_product_has_compartiment_product1` FOREIGN KEY (`id_product`) REFERENCES `product` (`id_product`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`%`*/ /*!50003 TRIGGER `after_insert_product_compartiment` AFTER INSERT ON `product_compartiment` FOR EACH ROW BEGIN
  DECLARE tankId INT;
  -- Obtener el id_tank relacionado con el nuevo compartimiento
  SELECT id_tank INTO tankId FROM compartiment WHERE id_compartiment = NEW.id_compartiment;
  UPDATE tank
    SET stock = (
      SELECT IFNULL(SUM(pc.stock),0)
      FROM product_compartiment pc
      JOIN compartiment c ON c.id_compartiment = pc.id_compartiment
      WHERE c.id_tank = tank.id_tank
    )
    WHERE id_tank = tankId;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`%`*/ /*!50003 TRIGGER `after_update_product_compartiment` AFTER UPDATE ON `product_compartiment` FOR EACH ROW BEGIN
  DECLARE tankIdNew INT;
  DECLARE tankIdOld INT;

  -- Nuevo tanque relacionado
  SELECT id_tank INTO tankIdNew FROM compartiment WHERE id_compartiment = NEW.id_compartiment;
  UPDATE tank
    SET stock = (
      SELECT IFNULL(SUM(pc.stock),0)
      FROM product_compartiment pc
      JOIN compartiment c ON c.id_compartiment = pc.id_compartiment
      WHERE c.id_tank = tank.id_tank
    )
    WHERE id_tank = tankIdNew;

  -- Si el compartimiento cambió de tanque, actualizar el anterior también
  IF (OLD.id_compartiment <> NEW.id_compartiment) THEN
    SELECT id_tank INTO tankIdOld FROM compartiment WHERE id_compartiment = OLD.id_compartiment;
    UPDATE tank
      SET stock = (
        SELECT IFNULL(SUM(pc.stock),0)
        FROM product_compartiment pc
        JOIN compartiment c ON c.id_compartiment = pc.id_compartiment
        WHERE c.id_tank = tank.id_tank
      )
      WHERE id_tank = tankIdOld;
  END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`%`*/ /*!50003 TRIGGER `after_delete_product_compartiment` AFTER DELETE ON `product_compartiment` FOR EACH ROW BEGIN
  DECLARE tankId INT;
  -- Obtener el id_tank relacionado con el compartimiento eliminado
  SELECT id_tank INTO tankId FROM compartiment WHERE id_compartiment = OLD.id_compartiment;
  UPDATE tank
    SET stock = (
      SELECT IFNULL(SUM(pc.stock),0)
      FROM product_compartiment pc
      JOIN compartiment c ON c.id_compartiment = pc.id_compartiment
      WHERE c.id_tank = tank.id_tank
    )
    WHERE id_tank = tankId;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `product_type`
--

DROP TABLE IF EXISTS `product_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_type` (
  `id_product_type` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  PRIMARY KEY (`id_product_type`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `provider`
--

DROP TABLE IF EXISTS `provider`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `provider` (
  `id_provider` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `date` date NOT NULL DEFAULT (curdate()),
  PRIMARY KEY (`id_provider`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `shopping`
--

DROP TABLE IF EXISTS `shopping`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shopping` (
  `id_shopping` int NOT NULL AUTO_INCREMENT,
  `invoice` varchar(45) NOT NULL,
  `date` date DEFAULT NULL,
  `id_provider` int NOT NULL,
  `id_category` int NOT NULL,
  `amount` double NOT NULL,
  PRIMARY KEY (`id_shopping`,`amount`),
  KEY `fk_shopping_provider1_idx` (`id_provider`),
  KEY `fk_shopping_category1_idx` (`id_category`),
  CONSTRAINT `fk_shopping_category1` FOREIGN KEY (`id_category`) REFERENCES `category` (`idcategory`),
  CONSTRAINT `fk_shopping_provider1` FOREIGN KEY (`id_provider`) REFERENCES `provider` (`id_provider`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `shopping_produc_inventory`
--

DROP TABLE IF EXISTS `shopping_produc_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shopping_produc_inventory` (
  `id_shopping_produc_inventorycol` int NOT NULL AUTO_INCREMENT,
  `id_shopping_product` int NOT NULL,
  `idinventory` int NOT NULL,
  PRIMARY KEY (`id_shopping_produc_inventorycol`),
  KEY `fk_shopping_product_has_inventory_inventory1_idx` (`idinventory`),
  KEY `fk_shopping_product_has_inventory_shopping_product1_idx` (`id_shopping_product`),
  CONSTRAINT `fk_shopping_product_has_inventory_inventory1` FOREIGN KEY (`idinventory`) REFERENCES `inventory` (`id_inventory`),
  CONSTRAINT `fk_shopping_product_has_inventory_shopping_product1` FOREIGN KEY (`id_shopping_product`) REFERENCES `shopping_product` (`id_shopping_product`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `shopping_product`
--

DROP TABLE IF EXISTS `shopping_product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shopping_product` (
  `id_shopping_product` int NOT NULL AUTO_INCREMENT,
  `id_shopping` int NOT NULL,
  `id_product` int NOT NULL,
  `quantity` double NOT NULL,
  `price` double NOT NULL,
  `id_compartiment` int NOT NULL,
  `date` date NOT NULL DEFAULT (curdate()),
  PRIMARY KEY (`id_shopping_product`),
  KEY `fk_shopping_has_product_product1_idx` (`id_product`),
  KEY `fk_shopping_has_product_shopping1_idx` (`id_shopping`),
  KEY `fk_shopping_product_compartiment1_idx` (`id_compartiment`),
  CONSTRAINT `fk_shopping_has_product_product1` FOREIGN KEY (`id_product`) REFERENCES `product` (`id_product`),
  CONSTRAINT `fk_shopping_has_product_shopping1` FOREIGN KEY (`id_shopping`) REFERENCES `shopping` (`id_shopping`),
  CONSTRAINT `fk_shopping_product_compartiment1` FOREIGN KEY (`id_compartiment`) REFERENCES `compartiment` (`id_compartiment`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`%`*/ /*!50003 TRIGGER `after_insert_shopping_product` AFTER INSERT ON `shopping_product` FOR EACH ROW BEGIN
  UPDATE product_compartiment
  SET stock = stock + NEW.quantity
  WHERE id_product = NEW.id_product
    AND id_compartiment = NEW.id_compartiment;

  UPDATE compartiment
  SET stock = stock + NEW.quantity
  WHERE id_compartiment = NEW.id_compartiment;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `tank`
--

DROP TABLE IF EXISTS `tank`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tank` (
  `id_tank` int NOT NULL AUTO_INCREMENT,
  `number` varchar(45) NOT NULL,
  `ability` double NOT NULL,
  `compartment` int NOT NULL,
  `stock` double NOT NULL,
  PRIMARY KEY (`id_tank`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `type_of_collection`
--

DROP TABLE IF EXISTS `type_of_collection`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `type_of_collection` (
  `id_type_of_collection` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  `date` date NOT NULL DEFAULT (curdate()),
  PRIMARY KEY (`id_type_of_collection`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `v_business`
--

DROP TABLE IF EXISTS `v_business`;
/*!50001 DROP VIEW IF EXISTS `v_business`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_business` AS SELECT 
 1 AS `id_business`,
 1 AS `name`,
 1 AS `eds`,
 1 AS `tank_number`,
 1 AS `compartiment`,
 1 AS `id_product`,
 1 AS `date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_capacity`
--

DROP TABLE IF EXISTS `v_capacity`;
/*!50001 DROP VIEW IF EXISTS `v_capacity`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_capacity` AS SELECT 
 1 AS `id_capacity`,
 1 AS `code`,
 1 AS `height`,
 1 AS `gallon`,
 1 AS `liters`,
 1 AS `date`,
 1 AS `id_business`,
 1 AS `id_product`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_compartiment`
--

DROP TABLE IF EXISTS `v_compartiment`;
/*!50001 DROP VIEW IF EXISTS `v_compartiment`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_compartiment` AS SELECT 
 1 AS `id_compartiment`,
 1 AS `number`,
 1 AS `nominal`,
 1 AS `operative`,
 1 AS `stock`,
 1 AS `height`,
 1 AS `id_tank`,
 1 AS `id_business`,
 1 AS `id_product`,
 1 AS `date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_court`
--

DROP TABLE IF EXISTS `v_court`;
/*!50001 DROP VIEW IF EXISTS `v_court`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_court` AS SELECT 
 1 AS `id`,
 1 AS `consecutive`,
 1 AS `id_eds`,
 1 AS `eds`,
 1 AS `bussiness`,
 1 AS `islander`,
 1 AS `date_starttime`,
 1 AS `starttime`,
 1 AS `date_endtime`,
 1 AS `endtime`,
 1 AS `distinc`,
 1 AS `total_accumulated_amount`,
 1 AS `total_accumulated_gallons`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_court_collection`
--

DROP TABLE IF EXISTS `v_court_collection`;
/*!50001 DROP VIEW IF EXISTS `v_court_collection`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_court_collection` AS SELECT 
 1 AS `id`,
 1 AS `court`,
 1 AS `date`,
 1 AS `collection`,
 1 AS `amount`,
 1 AS `description`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_court_dispenser`
--

DROP TABLE IF EXISTS `v_court_dispenser`;
/*!50001 DROP VIEW IF EXISTS `v_court_dispenser`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_court_dispenser` AS SELECT 
 1 AS `id`,
 1 AS `business`,
 1 AS `id_eds`,
 1 AS `eds`,
 1 AS `dispenser`,
 1 AS `number_hose`,
 1 AS `last_accumulated_amount`,
 1 AS `last_accumulated_gallons`,
 1 AS `code_court`,
 1 AS `islander`,
 1 AS `date_starttime`,
 1 AS `starttime`,
 1 AS `date_endtime`,
 1 AS `endtime`,
 1 AS `distinc`,
 1 AS `product`,
 1 AS `price`,
 1 AS `product_typr`,
 1 AS `accumulated_amount`,
 1 AS `accumulated_gallons`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_court_document`
--

DROP TABLE IF EXISTS `v_court_document`;
/*!50001 DROP VIEW IF EXISTS `v_court_document`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_court_document` AS SELECT 
 1 AS `id`,
 1 AS `court`,
 1 AS `descripcion`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_court_expenditure`
--

DROP TABLE IF EXISTS `v_court_expenditure`;
/*!50001 DROP VIEW IF EXISTS `v_court_expenditure`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_court_expenditure` AS SELECT 
 1 AS `id`,
 1 AS `court`,
 1 AS `date`,
 1 AS `expenditure`,
 1 AS `amount`,
 1 AS `description`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_eds`
--

DROP TABLE IF EXISTS `v_eds`;
/*!50001 DROP VIEW IF EXISTS `v_eds`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_eds` AS SELECT 
 1 AS `id_eds`,
 1 AS `name`,
 1 AS `nit`,
 1 AS `address`,
 1 AS `sicom`,
 1 AS `id_business`,
 1 AS `id_product`,
 1 AS `date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_hose`
--

DROP TABLE IF EXISTS `v_hose`;
/*!50001 DROP VIEW IF EXISTS `v_hose`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_hose` AS SELECT 
 1 AS `id`,
 1 AS `number_hose`,
 1 AS `accumulated_amount`,
 1 AS `accumulated_gallons`,
 1 AS `product_type`,
 1 AS `price`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_hose_history`
--

DROP TABLE IF EXISTS `v_hose_history`;
/*!50001 DROP VIEW IF EXISTS `v_hose_history`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_hose_history` AS SELECT 
 1 AS `id`,
 1 AS `id_hose`,
 1 AS `numer_hose`,
 1 AS `id_dispenser`,
 1 AS `dispenser_number`,
 1 AS `id_eds`,
 1 AS `eds`,
 1 AS `accumulated_amount`,
 1 AS `accumulated_gallons`,
 1 AS `date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_inventory`
--

DROP TABLE IF EXISTS `v_inventory`;
/*!50001 DROP VIEW IF EXISTS `v_inventory`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_inventory` AS SELECT 
 1 AS `id_business`,
 1 AS `business`,
 1 AS `id_eds`,
 1 AS `eds`,
 1 AS `id_tank`,
 1 AS `tank`,
 1 AS `tank_capacity`,
 1 AS `id_compartment`,
 1 AS `compartment`,
 1 AS `id_product`,
 1 AS `product`,
 1 AS `stock`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_product`
--

DROP TABLE IF EXISTS `v_product`;
/*!50001 DROP VIEW IF EXISTS `v_product`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_product` AS SELECT 
 1 AS `id_product`,
 1 AS `name`,
 1 AS `id_product_type`,
 1 AS `price`,
 1 AS `id_business`,
 1 AS `date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_providers`
--

DROP TABLE IF EXISTS `v_providers`;
/*!50001 DROP VIEW IF EXISTS `v_providers`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_providers` AS SELECT 
 1 AS `id_provider`,
 1 AS `name`,
 1 AS `id_product`,
 1 AS `id_business`,
 1 AS `date`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_shopping_product`
--

DROP TABLE IF EXISTS `v_shopping_product`;
/*!50001 DROP VIEW IF EXISTS `v_shopping_product`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_shopping_product` AS SELECT 
 1 AS `id_shopping_product`,
 1 AS `id_shopping`,
 1 AS `id_product`,
 1 AS `fecha`,
 1 AS `nombre_producto`,
 1 AS `quantity`,
 1 AS `price`,
 1 AS `id_compartiment`,
 1 AS `total_precio`,
 1 AS `id_business`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_type_of_collection`
--

DROP TABLE IF EXISTS `v_type_of_collection`;
/*!50001 DROP VIEW IF EXISTS `v_type_of_collection`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_type_of_collection` AS SELECT 
 1 AS `id_type_of_collection`,
 1 AS `description`,
 1 AS `id_product`,
 1 AS `id_business`,
 1 AS `date`*/;
SET character_set_client = @saved_cs_client;

--
-- Dumping routines for database 'eds'
--

--
-- Final view structure for view `v_business`
--

/*!50001 DROP VIEW IF EXISTS `v_business`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_business` AS select `b`.`id_business` AS `id_business`,`b`.`name` AS `name`,`e`.`name` AS `eds`,`t`.`number` AS `tank_number`,`c`.`number` AS `compartiment`,`p`.`id_product` AS `id_product`,`p`.`date` AS `date` from ((((`business` `b` left join `eds` `e` on((`e`.`id_eds` = `b`.`id_business`))) left join `product` `p` on((`p`.`id_product` = `b`.`id_business`))) left join `tank` `t` on((`t`.`id_tank` = `b`.`id_business`))) left join `compartiment` `c` on((`c`.`id_compartiment` = `b`.`id_business`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_capacity`
--

/*!50001 DROP VIEW IF EXISTS `v_capacity`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_capacity` AS select `c`.`id_capacity` AS `id_capacity`,`c`.`code` AS `code`,`c`.`height` AS `height`,`c`.`gallon` AS `gallon`,`c`.`liters` AS `liters`,`c`.`date` AS `date`,`b`.`id_business` AS `id_business`,`p`.`id_product` AS `id_product` from ((`capacity` `c` left join `business` `b` on((`b`.`id_business` = `c`.`id_capacity`))) left join `product` `p` on((`p`.`id_product` = `c`.`id_capacity`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_compartiment`
--

/*!50001 DROP VIEW IF EXISTS `v_compartiment`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_compartiment` AS select `c`.`id_compartiment` AS `id_compartiment`,`c`.`number` AS `number`,`c`.`nominal` AS `nominal`,`c`.`operative` AS `operative`,`c`.`stock` AS `stock`,`c`.`height` AS `height`,`c`.`id_tank` AS `id_tank`,`b`.`id_business` AS `id_business`,`p`.`id_product` AS `id_product`,`c`.`date` AS `date` from ((`compartiment` `c` left join `business` `b` on((`b`.`id_business` = `c`.`id_compartiment`))) left join `product` `p` on((`p`.`id_product` = `c`.`id_compartiment`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_court`
--

/*!50001 DROP VIEW IF EXISTS `v_court`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_court` AS select `c`.`id_court` AS `id`,`c`.`consecutive` AS `consecutive`,`e`.`id_eds` AS `id_eds`,`e`.`name` AS `eds`,`b`.`name` AS `bussiness`,`i`.`name` AS `islander`,`c`.`date_starttime` AS `date_starttime`,`c`.`starttime` AS `starttime`,`c`.`date_endtime` AS `date_endtime`,`c`.`endtime` AS `endtime`,`c`.`distintic` AS `distinc`,`ca`.`total_accumulated_amount` AS `total_accumulated_amount`,`cg`.`total_accumulated_gallons` AS `total_accumulated_gallons` from (((((`court` `c` join `islander` `i` on((`i`.`id_islander` = `c`.`id_islander`))) join `eds` `e` on((`e`.`id_eds` = `c`.`id_eds`))) left join (select `court_dispensers`.`id_court` AS `id_court`,sum(`court_dispensers`.`accumulated_amount`) AS `total_accumulated_amount` from `court_dispensers` group by `court_dispensers`.`id_court`) `ca` on((`ca`.`id_court` = `c`.`id_court`))) left join (select `court_dispensers`.`id_court` AS `id_court`,sum(`court_dispensers`.`accumulated_gallons`) AS `total_accumulated_gallons` from `court_dispensers` group by `court_dispensers`.`id_court`) `cg` on((`cg`.`id_court` = `c`.`id_court`))) join `business` `b` on((`b`.`id_business` = `e`.`idbusiness`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_court_collection`
--

/*!50001 DROP VIEW IF EXISTS `v_court_collection`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_court_collection` AS select `cc`.`id_court_type_of_collection` AS `id`,`c`.`id_court` AS `court`,`c`.`date_starttime` AS `date`,`tc`.`description` AS `collection`,`cc`.`amount` AS `amount`,`cc`.`description` AS `description` from ((`court_type_of_collection` `cc` left join `court` `c` on((`c`.`id_court` = `cc`.`id_court`))) left join `type_of_collection` `tc` on((`tc`.`id_type_of_collection` = `cc`.`id_type_of_collection`))) order by `c`.`id_court` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_court_dispenser`
--

/*!50001 DROP VIEW IF EXISTS `v_court_dispenser`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_court_dispenser` AS select `cd`.`id_court_dispensers` AS `id`,`b`.`name` AS `business`,`e`.`id_eds` AS `id_eds`,`e`.`name` AS `eds`,`d`.`number` AS `dispenser`,`h`.`number` AS `number_hose`,`h`.`accumulated_amount` AS `last_accumulated_amount`,`h`.`accumulated_gallons` AS `last_accumulated_gallons`,`c`.`id_court` AS `code_court`,`i`.`name` AS `islander`,`c`.`date_starttime` AS `date_starttime`,`c`.`starttime` AS `starttime`,`c`.`date_endtime` AS `date_endtime`,`c`.`endtime` AS `endtime`,`c`.`distintic` AS `distinc`,`p`.`name` AS `product`,`p`.`price` AS `price`,`pt`.`description` AS `product_typr`,`cd`.`accumulated_amount` AS `accumulated_amount`,`cd`.`accumulated_gallons` AS `accumulated_gallons` from ((((((((`court_dispensers` `cd` left join `court` `c` on((`c`.`id_court` = `cd`.`id_court`))) left join `product` `p` on((`p`.`id_product` = `cd`.`id_product`))) left join `hose` `h` on((`h`.`id_hose` = `cd`.`id_hose`))) left join `dispensers` `d` on((`d`.`id_dispensers` = `h`.`id_dispensers`))) left join `product_type` `pt` on((`p`.`id_product_type` = `pt`.`id_product_type`))) left join `eds` `e` on((`e`.`id_eds` = `d`.`id_eds`))) left join `business` `b` on((`b`.`id_business` = `e`.`idbusiness`))) left join `islander` `i` on((`i`.`id_islander` = `c`.`id_islander`))) order by `c`.`id_court` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_court_document`
--

/*!50001 DROP VIEW IF EXISTS `v_court_document`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_court_document` AS select `cd`.`idcourt_document` AS `id`,`c`.`id_court` AS `court`,`cd`.`descripcion` AS `descripcion` from (`court_document` `cd` left join `court` `c` on((`c`.`id_court` = `cd`.`id_court`))) order by `c`.`id_court` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_court_expenditure`
--

/*!50001 DROP VIEW IF EXISTS `v_court_expenditure`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_court_expenditure` AS select `ce`.`id_court_expenditure` AS `id`,`c`.`id_court` AS `court`,`c`.`date_starttime` AS `date`,`e`.`description` AS `expenditure`,`ce`.`amount` AS `amount`,`ce`.`description` AS `description` from ((`court_expenditures` `ce` left join `court` `c` on((`c`.`id_court` = `ce`.`id_court`))) left join `expenditures` `e` on((`e`.`id_expenditures` = `ce`.`id_expenditures`))) order by `c`.`id_court` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_eds`
--

/*!50001 DROP VIEW IF EXISTS `v_eds`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_eds` AS select `e`.`id_eds` AS `id_eds`,`e`.`name` AS `name`,`e`.`nit` AS `nit`,`e`.`address` AS `address`,`e`.`sicom` AS `sicom`,`b`.`id_business` AS `id_business`,`p`.`id_product` AS `id_product`,`e`.`date` AS `date` from ((`eds` `e` left join `business` `b` on((`b`.`id_business` = `e`.`id_eds`))) left join `product` `p` on((`p`.`id_product` = `e`.`id_eds`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_hose`
--

/*!50001 DROP VIEW IF EXISTS `v_hose`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_hose` AS select `h`.`id_hose` AS `id`,`h`.`number` AS `number_hose`,`h`.`accumulated_amount` AS `accumulated_amount`,`h`.`accumulated_gallons` AS `accumulated_gallons`,`pt`.`description` AS `product_type`,`p`.`price` AS `price` from (((`hose` `h` left join `dispensers` `d` on((`d`.`id_dispensers` = `h`.`id_dispensers`))) left join `product_type` `pt` on((`pt`.`id_product_type` = `h`.`id_product_type`))) left join `product` `p` on((`pt`.`id_product_type` = `p`.`id_product_type`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_hose_history`
--

/*!50001 DROP VIEW IF EXISTS `v_hose_history`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_hose_history` AS select `hh`.`id_hose_hose_history` AS `id`,`h`.`id_hose` AS `id_hose`,`h`.`number` AS `numer_hose`,`d`.`id_dispensers` AS `id_dispenser`,`d`.`number` AS `dispenser_number`,`e`.`id_eds` AS `id_eds`,`e`.`name` AS `eds`,`hh`.`accumulated_amount` AS `accumulated_amount`,`hh`.`accumulated_gallons` AS `accumulated_gallons`,`hh`.`date` AS `date` from (((`hose_history` `hh` left join `dispensers` `d` on((`d`.`id_dispenser_type` = `hh`.`id_dispensers`))) left join `hose` `h` on((`h`.`id_hose` = `hh`.`id_hose`))) left join `eds` `e` on((`e`.`id_eds` = `d`.`id_eds`))) order by `hh`.`date` desc */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_inventory`
--

/*!50001 DROP VIEW IF EXISTS `v_inventory`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_inventory` AS select `business`.`id_business` AS `id_business`,`business`.`name` AS `business`,`eds`.`id_eds` AS `id_eds`,`eds`.`name` AS `eds`,`tank`.`id_tank` AS `id_tank`,`tank`.`number` AS `tank`,`tank`.`ability` AS `tank_capacity`,`compartiment`.`id_compartiment` AS `id_compartment`,`compartiment`.`number` AS `compartment`,`product`.`id_product` AS `id_product`,`product`.`name` AS `product`,`compartiment`.`stock` AS `stock` from ((((((`business` join `eds` on((`business`.`id_business` = `eds`.`idbusiness`))) join `eds_tank` on((`eds`.`id_eds` = `eds_tank`.`id_eds`))) join `tank` on((`eds_tank`.`id_tank` = `tank`.`id_tank`))) join `compartiment` on((`tank`.`id_tank` = `compartiment`.`id_tank`))) join `product_compartiment` on((`compartiment`.`id_compartiment` = `product_compartiment`.`id_compartiment`))) join `product` on((`product_compartiment`.`id_product` = `product`.`id_product`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_product`
--

/*!50001 DROP VIEW IF EXISTS `v_product`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_product` AS select `p`.`id_product` AS `id_product`,`p`.`name` AS `name`,`p`.`id_product_type` AS `id_product_type`,`p`.`price` AS `price`,`b`.`id_business` AS `id_business`,`p`.`date` AS `date` from (`product` `p` left join `business` `b` on((`b`.`id_business` = `p`.`id_product`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_providers`
--

/*!50001 DROP VIEW IF EXISTS `v_providers`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_providers` AS select `p`.`id_provider` AS `id_provider`,`p`.`name` AS `name`,`pd`.`id_product` AS `id_product`,`b`.`id_business` AS `id_business`,`p`.`date` AS `date` from ((`provider` `p` left join `business` `b` on((`b`.`id_business` = `p`.`id_provider`))) left join `product` `pd` on((`pd`.`id_product` = `p`.`id_provider`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_shopping_product`
--

/*!50001 DROP VIEW IF EXISTS `v_shopping_product`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_shopping_product` AS select `sp`.`id_shopping_product` AS `id_shopping_product`,`sp`.`id_shopping` AS `id_shopping`,`sp`.`id_product` AS `id_product`,`sp`.`date` AS `fecha`,`p`.`name` AS `nombre_producto`,`sp`.`quantity` AS `quantity`,`sp`.`price` AS `price`,`sp`.`id_compartiment` AS `id_compartiment`,(`sp`.`quantity` * `sp`.`price`) AS `total_precio`,`b`.`id_business` AS `id_business` from ((((((`shopping_product` `sp` join `product` `p` on((`sp`.`id_product` = `p`.`id_product`))) join `compartiment` `c` on((`c`.`id_compartiment` = `sp`.`id_compartiment`))) join `tank` `t` on((`t`.`id_tank` = `c`.`id_tank`))) join `eds_tank` `et` on((`et`.`id_tank` = `t`.`id_tank`))) join `eds` `e` on((`e`.`id_eds` = `et`.`id_eds`))) join `business` `b` on((`b`.`id_business` = `e`.`idbusiness`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_type_of_collection`
--

/*!50001 DROP VIEW IF EXISTS `v_type_of_collection`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`%` SQL SECURITY DEFINER */
/*!50001 VIEW `v_type_of_collection` AS select `toc`.`id_type_of_collection` AS `id_type_of_collection`,`toc`.`description` AS `description`,`p`.`id_product` AS `id_product`,`b`.`id_business` AS `id_business`,`p`.`date` AS `date` from ((`type_of_collection` `toc` left join `business` `b` on((`b`.`id_business` = `toc`.`id_type_of_collection`))) left join `product` `p` on((`p`.`id_product` = `toc`.`id_type_of_collection`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-07-16  9:13:00

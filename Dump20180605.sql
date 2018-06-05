-- MySQL dump 10.13  Distrib 8.0.11, for Win64 (x86_64)
--
-- Host: localhost    Database: store
-- ------------------------------------------------------
-- Server version	8.0.11

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `address`
--

DROP TABLE IF EXISTS `address`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `address` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ProvinceName` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CityName` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CountyName` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `DetailInfo` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `TelNumber` varchar(20) NOT NULL,
  `UserID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `address`
--

LOCK TABLES `address` WRITE;
/*!40000 ALTER TABLE `address` DISABLE KEYS */;
INSERT INTO `address` VALUES (1,'张文彬','北京','北京','丰台区','西罗园二区6号楼1602','15311508135',1);
/*!40000 ALTER TABLE `address` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cart`
--

DROP TABLE IF EXISTS `cart`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `cart` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `SKUID` int(11) NOT NULL,
  `Count` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cart`
--

LOCK TABLES `cart` WRITE;
/*!40000 ALTER TABLE `cart` DISABLE KEYS */;
INSERT INTO `cart` VALUES (4,1,1,2);
/*!40000 ALTER TABLE `cart` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order`
--

DROP TABLE IF EXISTS `order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `order` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `AddressID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order`
--

LOCK TABLES `order` WRITE;
/*!40000 ALTER TABLE `order` DISABLE KEYS */;
INSERT INTO `order` VALUES (1,1,'2018-05-28 22:30:32',1),(2,1,'2018-05-28 22:36:58',1);
/*!40000 ALTER TABLE `order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ordersku`
--

DROP TABLE IF EXISTS `ordersku`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `ordersku` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `SKUID` int(11) NOT NULL,
  `Count` int(11) NOT NULL,
  `OrderID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ordersku`
--

LOCK TABLES `ordersku` WRITE;
/*!40000 ALTER TABLE `ordersku` DISABLE KEYS */;
INSERT INTO `ordersku` VALUES (1,1,1,1),(2,1,1,2);
/*!40000 ALTER TABLE `ordersku` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `product` (
  `ID` int(10) NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `Title` varchar(100) DEFAULT NULL,
  `Img` varchar(200) NOT NULL,
  `Imgs` varchar(500) NOT NULL,
  `Detial` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID_UNIQUE` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` VALUES (1,'红心火龙果',NULL,'https://img10.360buyimg.com/n7/jfs/t3118/197/229364904/200600/37296076/57ab28ceNf76fe40f.jpg','https://img10.360buyimg.com/n7/jfs/t3118/197/229364904/200600/37296076/57ab28ceNf76fe40f.jpg',NULL),(2,'芒果',NULL,'https://img10.360buyimg.com/n7/jfs/t3832/361/3992685655/451734/4d12e581/58a6bfa1N56a8232f.jpg','https://img10.360buyimg.com/n7/jfs/t3832/361/3992685655/451734/4d12e581/58a6bfa1N56a8232f.jpg',NULL),(3,'脐橙',NULL,'https://img10.360buyimg.com/n7/jfs/t17740/327/925846464/279298/4e2c0a11/5ab0d753N246cf77e.jpg','https://img10.360buyimg.com/n7/jfs/t17740/327/925846464/279298/4e2c0a11/5ab0d753N246cf77e.jpg',NULL),(4,'草莓',NULL,'https://img11.360buyimg.com/n7/jfs/t15994/184/2167604929/554815/b4f234/5a93bd8cN7f388e57.jpg','https://img11.360buyimg.com/n7/jfs/t15994/184/2167604929/554815/b4f234/5a93bd8cN7f388e57.jpg',NULL),(5,'百香果',NULL,'https://img12.360buyimg.com/n7/jfs/t13057/81/825407133/1439835/58674273/5a14e701N139506ac.jpg','https://img12.360buyimg.com/n7/jfs/t13057/81/825407133/1439835/58674273/5a14e701N139506ac.jpg',NULL),(6,'香梨','爱奇果 新疆库尔勒香梨 总重量约2.5kg 精选特级大果 新鲜水果','https://img11.360buyimg.com/n7/jfs/t8212/335/867296173/346941/42857e6f/59b0afceNd7a1adfa.jpg','https://img11.360buyimg.com/n1/jfs/t8938/30/873811715/423513/cea961e0/59b0afd9Nbbdc5f09.jpg,https://img11.360buyimg.com/n1/jfs/t7972/286/2511692290/402163/9c3f34c2/59b0afddN9dabe22c.jpg,https://img11.360buyimg.com/n1/jfs/t7399/265/2507137150/453229/dbe9e1cf/59b0afe0Nfbe8f936.jpg','https://img11.360buyimg.com/cms/jfs/t9133/17/896207706/99566/2a3cdabe/59b0aa16Nbdff4c7e.jpg');
/*!40000 ALTER TABLE `product` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sku`
--

DROP TABLE IF EXISTS `sku`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `sku` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Price` decimal(5,2) unsigned NOT NULL,
  `ProductID` int(10) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sku`
--

LOCK TABLES `sku` WRITE;
/*!40000 ALTER TABLE `sku` DISABLE KEYS */;
INSERT INTO `sku` VALUES (1,'500g',34.90,6);
/*!40000 ALTER TABLE `sku` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `OpenID` varchar(45) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-06-05 22:04:28

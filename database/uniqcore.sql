/*
 Navicat Premium Data Transfer

 Source Server         : test
 Source Server Type    : MySQL
 Source Server Version : 100428
 Source Host           : localhost:3306
 Source Schema         : uniqcore

 Target Server Type    : MySQL
 Target Server Version : 100428
 File Encoding         : 65001

 Date: 12/08/2023 13:15:51
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for __efmigrationshistory
-- ----------------------------
DROP TABLE IF EXISTS `__efmigrationshistory`;
CREATE TABLE `__efmigrationshistory`  (
  `MigrationId` varchar(95) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  PRIMARY KEY (`MigrationId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of __efmigrationshistory
-- ----------------------------
INSERT INTO `__efmigrationshistory` VALUES ('20210301132143_Initial', '3.1.9');
INSERT INTO `__efmigrationshistory` VALUES ('20210315214823_Phone_TaxiOrders', '3.1.9');
INSERT INTO `__efmigrationshistory` VALUES ('20210323125324_Phone-InviteCodes', '3.1.9');
INSERT INTO `__efmigrationshistory` VALUES ('20210404064213_Phone-RemoveUniqueOnTargetNumberIndex', '3.1.9');
INSERT INTO `__efmigrationshistory` VALUES ('20210415121853_Phone-AddBlockeByAtAccountsToChatsw', '3.1.9');
INSERT INTO `__efmigrationshistory` VALUES ('20210424154416_Phone-BankTransactions', '3.1.9');
INSERT INTO `__efmigrationshistory` VALUES ('20210504132448_Phone-News', '3.1.9');
INSERT INTO `__efmigrationshistory` VALUES ('20210504133212_Fractions-Access', '3.1.9');
INSERT INTO `__efmigrationshistory` VALUES ('20210810153848_AddPhoneBankAccount', '3.1.9');

-- ----------------------------
-- Table structure for fraction_access
-- ----------------------------
DROP TABLE IF EXISTS `fraction_access`;
CREATE TABLE `fraction_access`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FractionId` int NOT NULL,
  `FractionRank` int NOT NULL,
  `AccessList` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of fraction_access
-- ----------------------------

-- ----------------------------
-- Table structure for phones
-- ----------------------------
DROP TABLE IF EXISTS `phones`;
CREATE TABLE `phones`  (
  `CharacterUuid` int NOT NULL AUTO_INCREMENT,
  `InstalledAppsIds` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `SimCardId` int NULL DEFAULT NULL,
  `AccountId` int NULL DEFAULT NULL,
  PRIMARY KEY (`CharacterUuid`) USING BTREE,
  INDEX `IX_Phones_AccountId`(`AccountId` ASC) USING BTREE,
  INDEX `IX_Phones_SimCardId`(`SimCardId` ASC) USING BTREE,
  CONSTRAINT `FK_Phones_phones_msg_accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `phones_msg_accounts` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_Phones_phones_simcards_SimCardId` FOREIGN KEY (`SimCardId`) REFERENCES `phones_simcards` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 981 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones
-- ----------------------------

-- ----------------------------
-- Table structure for phones_bank_transact
-- ----------------------------
DROP TABLE IF EXISTS `phones_bank_transact`;
CREATE TABLE `phones_bank_transact`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `From` int NOT NULL,
  `FromType` int NOT NULL,
  `To` int NOT NULL,
  `ToType` int NOT NULL,
  `Amount` int NOT NULL,
  `Date` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 340254 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_bank_transact
-- ----------------------------

-- ----------------------------
-- Table structure for phones_blocks
-- ----------------------------
DROP TABLE IF EXISTS `phones_blocks`;
CREATE TABLE `phones_blocks`  (
  `SimCardId` int NOT NULL,
  `TargetNumber` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`SimCardId`, `TargetNumber`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_blocks
-- ----------------------------

-- ----------------------------
-- Table structure for phones_callhistory
-- ----------------------------
DROP TABLE IF EXISTS `phones_callhistory`;
CREATE TABLE `phones_callhistory`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FromSimCardId` int NOT NULL,
  `TargetNumber` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `CallStatus` int NOT NULL,
  `Duration` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_phones_callhistory_FromSimCardId`(`FromSimCardId` ASC) USING BTREE,
  INDEX `IX_phones_callhistory_TargetNumber`(`TargetNumber` ASC) USING BTREE,
  CONSTRAINT `FK_phones_callhistory_phones_simcards_FromSimCardId` FOREIGN KEY (`FromSimCardId`) REFERENCES `phones_simcards` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 100 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_callhistory
-- ----------------------------

-- ----------------------------
-- Table structure for phones_contacts
-- ----------------------------
DROP TABLE IF EXISTS `phones_contacts`;
CREATE TABLE `phones_contacts`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `HolderSimCardId` int NOT NULL,
  `TargetNumber` int NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_phones_contacts_HolderSimCardId`(`HolderSimCardId` ASC) USING BTREE,
  INDEX `IX_phones_contacts_TargetNumber`(`TargetNumber` ASC) USING BTREE,
  CONSTRAINT `FK_phones_contacts_phones_simcards_HolderSimCardId` FOREIGN KEY (`HolderSimCardId`) REFERENCES `phones_simcards` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 30 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_contacts
-- ----------------------------

-- ----------------------------
-- Table structure for phones_msg_accounts
-- ----------------------------
DROP TABLE IF EXISTS `phones_msg_accounts`;
CREATE TABLE `phones_msg_accounts`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Username` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `SimCardId` int NOT NULL,
  `DisplayedName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `IsNumberHided` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `LastVisit` datetime(6) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_phones_msg_accounts_SimCardId`(`SimCardId` ASC) USING BTREE,
  CONSTRAINT `FK_phones_msg_accounts_phones_simcards_SimCardId` FOREIGN KEY (`SimCardId`) REFERENCES `phones_simcards` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 11131 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_msg_accounts
-- ----------------------------

-- ----------------------------
-- Table structure for phones_msg_accountstochat
-- ----------------------------
DROP TABLE IF EXISTS `phones_msg_accountstochat`;
CREATE TABLE `phones_msg_accountstochat`  (
  `AccountId` int NOT NULL,
  `ChatId` int NOT NULL,
  `IsLeaved` tinyint(1) NOT NULL,
  `IsMuted` tinyint(1) NOT NULL,
  `LastReadMessageId` int NULL DEFAULT NULL,
  `AdminLvl` int NOT NULL,
  `IsBlocked` tinyint(1) NOT NULL,
  `Permissions` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `BlockedById` int NULL DEFAULT NULL,
  PRIMARY KEY (`AccountId`, `ChatId`) USING BTREE,
  INDEX `IX_phones_msg_accountsToChat_ChatId`(`ChatId` ASC) USING BTREE,
  INDEX `IX_phones_msg_accountsToChat_LastReadMessageId`(`LastReadMessageId` ASC) USING BTREE,
  INDEX `IX_phones_msg_accountsToChat_BlockedById`(`BlockedById` ASC) USING BTREE,
  CONSTRAINT `FK_phones_msg_accountsToChat_phones_msg_accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `phones_msg_accounts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_phones_msg_accountsToChat_phones_msg_accounts_BlockedById` FOREIGN KEY (`BlockedById`) REFERENCES `phones_msg_accounts` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_phones_msg_accountsToChat_phones_msg_chats_ChatId` FOREIGN KEY (`ChatId`) REFERENCES `phones_msg_chats` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_phones_msg_accountsToChat_phones_msg_messages_LastReadMessag~` FOREIGN KEY (`LastReadMessageId`) REFERENCES `phones_msg_messages` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_msg_accountstochat
-- ----------------------------

-- ----------------------------
-- Table structure for phones_msg_chats
-- ----------------------------
DROP TABLE IF EXISTS `phones_msg_chats`;
CREATE TABLE `phones_msg_chats`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `Type` int NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Avatar` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `InviteCode` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_phones_msg_chats_InviteCode`(`InviteCode` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 273 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_msg_chats
-- ----------------------------

-- ----------------------------
-- Table structure for phones_msg_contacts
-- ----------------------------
DROP TABLE IF EXISTS `phones_msg_contacts`;
CREATE TABLE `phones_msg_contacts`  (
  `ContactId` int NOT NULL AUTO_INCREMENT,
  `HolderAccountId` int NOT NULL,
  `TargetAccountId` int NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  PRIMARY KEY (`ContactId`) USING BTREE,
  INDEX `IX_phones_msg_contacts_HolderAccountId`(`HolderAccountId` ASC) USING BTREE,
  INDEX `IX_phones_msg_contacts_TargetAccountId`(`TargetAccountId` ASC) USING BTREE,
  CONSTRAINT `FK_phones_msg_contacts_phones_msg_accounts_HolderAccountId` FOREIGN KEY (`HolderAccountId`) REFERENCES `phones_msg_accounts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_phones_msg_contacts_phones_msg_accounts_TargetAccountId` FOREIGN KEY (`TargetAccountId`) REFERENCES `phones_msg_accounts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 79 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_msg_contacts
-- ----------------------------

-- ----------------------------
-- Table structure for phones_msg_messages
-- ----------------------------
DROP TABLE IF EXISTS `phones_msg_messages`;
CREATE TABLE `phones_msg_messages`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Text` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `SenderId` int NOT NULL,
  `ChatId` int NOT NULL,
  `IsRead` tinyint(1) NOT NULL,
  `Attachments` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `Discriminator` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `Photo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_phones_msg_messages_ChatId`(`ChatId` ASC) USING BTREE,
  INDEX `IX_phones_msg_messages_SenderId`(`SenderId` ASC) USING BTREE,
  CONSTRAINT `FK_phones_msg_messages_phones_msg_accounts_SenderId` FOREIGN KEY (`SenderId`) REFERENCES `phones_msg_accounts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_phones_msg_messages_phones_msg_chats_ChatId` FOREIGN KEY (`ChatId`) REFERENCES `phones_msg_chats` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 1211 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_msg_messages
-- ----------------------------

-- ----------------------------
-- Table structure for phones_news_advert
-- ----------------------------
DROP TABLE IF EXISTS `phones_news_advert`;
CREATE TABLE `phones_news_advert`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SenderUUID` int NOT NULL,
  `EditorUUID` int NOT NULL,
  `DateCreate` datetime(6) NOT NULL,
  `DateCompleate` datetime(6) NOT NULL,
  `PhoneNumber` int NOT NULL,
  `MessengerLogin` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `Text` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `PrimeAdvert` tinyint(1) NOT NULL,
  `ImageUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NULL,
  `Status` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_news_advert
-- ----------------------------

-- ----------------------------
-- Table structure for phones_simcards
-- ----------------------------
DROP TABLE IF EXISTS `phones_simcards`;
CREATE TABLE `phones_simcards`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Number` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `BankNumber` int NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_phones_simcards_Number`(`Number` ASC) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 19277 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_simcards
-- ----------------------------

-- ----------------------------
-- Table structure for phones_taxi_orders
-- ----------------------------
DROP TABLE IF EXISTS `phones_taxi_orders`;
CREATE TABLE `phones_taxi_orders`  (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DriverUuid` int NOT NULL,
  `Date` datetime(6) NOT NULL,
  `TotalPrice` int NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_bin ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of phones_taxi_orders
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;

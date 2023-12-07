-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: Dec 05, 2023 at 09:48 AM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `uniqlogs`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE `accounts` (
  `socialclub` text DEFAULT NULL,
  `login` varchar(155) NOT NULL,
  `hwid` varchar(155) DEFAULT NULL,
  `redbucks` int(11) DEFAULT 0,
  `ip` varchar(155) NOT NULL,
  `character1` int(11) NOT NULL,
  `character2` int(11) NOT NULL,
  `character3` int(11) NOT NULL,
  `lastCharacter` int(11) DEFAULT -1,
  `email` varchar(155) NOT NULL,
  `password` varchar(155) NOT NULL,
  `promocodes` varchar(155) DEFAULT NULL,
  `present` tinyint(1) DEFAULT 0,
  `idkey` int(11) NOT NULL,
  `lang` varchar(4) NOT NULL DEFAULT 'ru',
  `realsocialclub` text DEFAULT NULL,
  `mcoins` int(11) NOT NULL DEFAULT 0,
  `bonusbegine` datetime DEFAULT NULL,
  `totalplayed` int(11) NOT NULL DEFAULT 0,
  `bonuscompleete` tinyint(1) NOT NULL DEFAULT 0,
  `promoused` varchar(45) DEFAULT NULL,
  `promoreceived` tinyint(1) NOT NULL DEFAULT 0,
  `usedbonuses` text DEFAULT NULL,
  `socialclubid` bigint(20) UNSIGNED NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `achievements`
--

CREATE TABLE `achievements` (
  `uuid` int(11) NOT NULL,
  `achieveName` int(11) NOT NULL,
  `currentLevel` int(11) NOT NULL DEFAULT 0,
  `givenReward` tinyint(4) NOT NULL DEFAULT 0,
  `dateCompleted` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `adminaccess`
--

CREATE TABLE `adminaccess` (
  `minrank` int(11) NOT NULL,
  `command` varchar(155) NOT NULL,
  `isadmin` tinyint(1) NOT NULL,
  `idkey` int(11) NOT NULL,
  `istech` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `adminlog`
--

CREATE TABLE `adminlog` (
  `time` datetime NOT NULL,
  `admin` text NOT NULL,
  `action` text NOT NULL,
  `player` text NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `advertised`
--

CREATE TABLE `advertised` (
  `ID` int(10) UNSIGNED NOT NULL,
  `Author` varchar(40) NOT NULL,
  `AuthorSIM` int(11) NOT NULL,
  `AD` varchar(150) NOT NULL,
  `Editor` varchar(40) DEFAULT NULL,
  `EditedAD` varchar(150) DEFAULT NULL,
  `Opened` datetime NOT NULL,
  `Closed` datetime DEFAULT NULL,
  `Status` tinyint(4) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `alcobars`
--

CREATE TABLE `alcobars` (
  `id` int(11) NOT NULL,
  `position` text NOT NULL,
  `radius` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `alcoclubs`
--

CREATE TABLE `alcoclubs` (
  `id` int(11) NOT NULL,
  `alco1` int(11) NOT NULL,
  `alco2` int(11) NOT NULL,
  `alco3` int(11) NOT NULL,
  `pricemod` varchar(155) NOT NULL,
  `mats` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `armorpoints`
--

CREATE TABLE `armorpoints` (
  `id` int(11) NOT NULL,
  `fractionid` int(11) NOT NULL,
  `dimension` text NOT NULL,
  `position` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `arrestlog`
--

CREATE TABLE `arrestlog` (
  `time` datetime NOT NULL,
  `player` text NOT NULL,
  `target` text NOT NULL,
  `reason` text NOT NULL,
  `stars` text NOT NULL,
  `pnick` text NOT NULL,
  `tnick` text NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `arrests`
--

CREATE TABLE `arrests` (
  `id` int(11) NOT NULL,
  `uuid` int(11) NOT NULL,
  `detaineduuid` int(11) NOT NULL,
  `arrestdate` datetime NOT NULL,
  `releasedate` datetime NOT NULL,
  `strippedlicenses` text DEFAULT NULL,
  `canbeissue` tinyint(1) NOT NULL,
  `bailamount` int(11) NOT NULL DEFAULT 0,
  `bailplayeruuid` int(11) NOT NULL DEFAULT -1,
  `reason` text DEFAULT NULL,
  `wantedlevel` int(11) NOT NULL DEFAULT 0,
  `phone` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `bankpoints`
--

CREATE TABLE `bankpoints` (
  `id` int(11) NOT NULL,
  `dimension` int(10) UNSIGNED NOT NULL,
  `position` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `banlog`
--

CREATE TABLE `banlog` (
  `time` datetime NOT NULL,
  `admin` text NOT NULL,
  `player` text NOT NULL,
  `until` datetime NOT NULL,
  `reason` text NOT NULL,
  `ishard` text NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `banned`
--

CREATE TABLE `banned` (
  `uuid` int(11) NOT NULL,
  `name` text NOT NULL,
  `account` text NOT NULL,
  `time` varchar(155) NOT NULL,
  `until` varchar(155) NOT NULL,
  `ishard` bigint(20) NOT NULL,
  `ip` varchar(155) NOT NULL,
  `socialclub` text NOT NULL,
  `hwid` varchar(155) NOT NULL,
  `reason` text NOT NULL,
  `byadmin` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `bizsettings`
--

CREATE TABLE `bizsettings` (
  `biztype` int(11) NOT NULL,
  `settings` text DEFAULT NULL,
  `bliptype` int(11) NOT NULL DEFAULT 0,
  `blipcolor` int(11) NOT NULL DEFAULT 0,
  `name` text DEFAULT NULL,
  `minimumpercentproduct` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `blacklist`
--

CREATE TABLE `blacklist` (
  `id` int(11) NOT NULL,
  `serial` text NOT NULL,
  `socialclub` text NOT NULL,
  `admin` text NOT NULL,
  `date` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `bonuscodes`
--

CREATE TABLE `bonuscodes` (
  `id` int(11) NOT NULL,
  `bonusname` varchar(45) NOT NULL,
  `money` int(11) NOT NULL DEFAULT 0,
  `coins` int(11) NOT NULL DEFAULT 0,
  `prime` int(11) NOT NULL DEFAULT 0,
  `dateoff` datetime NOT NULL,
  `countuse` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `boxlog`
--

CREATE TABLE `boxlog` (
  `id` int(11) NOT NULL,
  `time` datetime NOT NULL,
  `boxId` int(11) NOT NULL DEFAULT -1,
  `uuid` int(11) NOT NULL DEFAULT -1,
  `item` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `businesses`
--

CREATE TABLE `businesses` (
  `id` int(11) NOT NULL,
  `owneruuid` int(11) NOT NULL DEFAULT -5,
  `owner` text DEFAULT NULL,
  `sellprice` text NOT NULL,
  `type` text NOT NULL,
  `products` text NOT NULL,
  `enterpoint` text NOT NULL,
  `unloadpoint` text NOT NULL,
  `additionalpos` text DEFAULT NULL,
  `money` text DEFAULT NULL,
  `mafia` text NOT NULL,
  `orders` text NOT NULL,
  `name` text NOT NULL,
  `peds` text NOT NULL,
  `colshapeRange` int(11) NOT NULL DEFAULT 1,
  `blipPosition` text DEFAULT NULL,
  `dimension` int(10) UNSIGNED DEFAULT 0,
  `family` int(11) NOT NULL DEFAULT -1,
  `takeoverdate` datetime DEFAULT NULL,
  `daywithoutproducts` datetime DEFAULT NULL,
  `banknew` int(11) NOT NULL DEFAULT -1,
  `bankacc` int(11) NOT NULL DEFAULT -1,
  `pledged` tinyint(1) NOT NULL DEFAULT 0,
  `camposition` text DEFAULT NULL,
  `profitData` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `cartradecars`
--

CREATE TABLE `cartradecars` (
  `carid` int(11) NOT NULL,
  `bizid` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `components` text DEFAULT NULL,
  `position` varchar(255) DEFAULT NULL,
  `rotation` varchar(255) DEFAULT NULL,
  `number` varchar(255) DEFAULT NULL,
  `model` varchar(255) DEFAULT NULL,
  `isplaced` tinyint(1) DEFAULT NULL,
  `componentsnew` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `cartradelogs`
--

CREATE TABLE `cartradelogs` (
  `operation` varchar(50) NOT NULL,
  `money` int(11) NOT NULL,
  `playername` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `cartrades`
--

CREATE TABLE `cartrades` (
  `bizId` int(11) NOT NULL,
  `amount` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `casino`
--

CREATE TABLE `casino` (
  `id` int(11) NOT NULL,
  `bizId` int(11) NOT NULL,
  `amount` int(11) NOT NULL,
  `minusDate` datetime DEFAULT NULL,
  `stateTax` int(11) NOT NULL,
  `casinoTax` int(11) NOT NULL,
  `maxWinOfBet` int(11) NOT NULL DEFAULT 5000000
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `casinobetlog`
--

CREATE TABLE `casinobetlog` (
  `id` int(11) NOT NULL,
  `time` varchar(100) DEFAULT NULL,
  `name` varchar(100) DEFAULT NULL,
  `uuid` int(11) DEFAULT NULL,
  `zero` varchar(3) DEFAULT NULL,
  `red` varchar(3) DEFAULT NULL,
  `black` varchar(3) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `casinoendlog`
--

CREATE TABLE `casinoendlog` (
  `id` int(11) NOT NULL,
  `time` varchar(100) DEFAULT NULL,
  `name` varchar(100) DEFAULT NULL,
  `uuid` int(11) DEFAULT NULL,
  `state` varchar(100) DEFAULT NULL,
  `type` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `casinologs`
--

CREATE TABLE `casinologs` (
  `operation` varchar(50) NOT NULL,
  `money` int(11) NOT NULL,
  `playername` varchar(50) DEFAULT NULL,
  `datetime` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `casinowinloselog`
--

CREATE TABLE `casinowinloselog` (
  `id` int(11) NOT NULL,
  `name` varchar(100) DEFAULT NULL,
  `uuid` int(11) DEFAULT NULL,
  `time` varchar(100) DEFAULT NULL,
  `wonbet` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `characters`
--

CREATE TABLE `characters` (
  `uuid` bigint(20) NOT NULL,
  `adminlvl` int(11) NOT NULL,
  `money` int(11) NOT NULL,
  `firstname` text NOT NULL,
  `lastname` text NOT NULL,
  `fraction` int(11) NOT NULL,
  `fractionlvl` int(11) NOT NULL,
  `warns` int(11) NOT NULL,
  `biz` text DEFAULT NULL,
  `hotel` int(11) DEFAULT NULL,
  `hotelleft` int(11) DEFAULT NULL,
  `sim` int(11) DEFAULT NULL,
  `PetName` mediumtext DEFAULT NULL,
  `demorgan` int(11) NOT NULL,
  `arrest` int(11) NOT NULL,
  `unwarn` datetime NOT NULL,
  `unmute` int(11) DEFAULT NULL,
  `unmutedate` datetime NOT NULL,
  `bank` int(11) DEFAULT NULL,
  `wanted` text DEFAULT NULL,
  `lvl` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `gender` tinyint(1) NOT NULL,
  `health` int(11) NOT NULL,
  `armor` int(11) DEFAULT NULL,
  `licenses` text NOT NULL,
  `lastveh` text DEFAULT NULL,
  `onduty` tinyint(1) NOT NULL,
  `lasthour` int(11) NOT NULL DEFAULT 0,
  `lastmin` int(11) NOT NULL DEFAULT 0,
  `contacts` text DEFAULT NULL,
  `achiev` text DEFAULT NULL,
  `createdate` datetime NOT NULL,
  `pos` text NOT NULL,
  `work` int(11) NOT NULL,
  `idkey` int(11) NOT NULL,
  `friends` text NOT NULL,
  `arrestiligalTime` int(11) NOT NULL DEFAULT 0,
  `timerMiss` datetime NOT NULL DEFAULT current_timestamp(),
  `arrestID` int(11) NOT NULL DEFAULT 0,
  `courttime` int(11) NOT NULL DEFAULT 0,
  `family` int(11) NOT NULL DEFAULT 0,
  `familylvl` int(11) NOT NULL DEFAULT 0,
  `mulct` int(11) NOT NULL DEFAULT 0,
  `rouletteinventory` text DEFAULT NULL,
  `newrouletteinventory` text DEFAULT NULL,
  `hasfreespin` int(11) NOT NULL DEFAULT 0,
  `lastdayplayedhours` int(11) NOT NULL DEFAULT 0,
  `chips` text DEFAULT NULL,
  `partner` int(11) NOT NULL DEFAULT -1,
  `callHistory` text DEFAULT NULL,
  `hungerlevel` int(11) DEFAULT 100,
  `thirstlevel` int(11) DEFAULT 100,
  `equipId` int(11) NOT NULL,
  `inventoryId` int(11) NOT NULL,
  `lastvehicle` int(11) NOT NULL DEFAULT -1,
  `imp_job_state` text DEFAULT NULL,
  `numberofratings` int(11) NOT NULL DEFAULT 0,
  `amountofratings` int(11) NOT NULL DEFAULT 0,
  `numberofadminratings` int(11) NOT NULL DEFAULT 0,
  `amountofadminratings` int(11) NOT NULL DEFAULT 0,
  `queststage` int(11) DEFAULT 1,
  `promoused` varchar(45) DEFAULT '',
  `voiceupdate` tinyint(1) NOT NULL DEFAULT 1,
  `arena_points` int(11) NOT NULL DEFAULT 0,
  `media` int(11) NOT NULL DEFAULT 0,
  `mediahelper` int(11) NOT NULL DEFAULT 0,
  `lastvote` int(11) NOT NULL DEFAULT -1,
  `usedTips` text DEFAULT NULL,
  `deleted` tinyint(1) NOT NULL DEFAULT 0,
  `deletedAt` datetime DEFAULT NULL,
  `owner` int(11) NOT NULL DEFAULT -1,
  `familypoints` int(11) DEFAULT 0,
  `familypointsadd` int(11) DEFAULT 0,
  `familypointssub` int(11) DEFAULT 0,
  `familypointlastupdate` datetime DEFAULT NULL,
  `iconoverhead` text DEFAULT NULL,
  `rbrating` int(11) NOT NULL DEFAULT 0,
  `donateInventoryId` int(11) DEFAULT NULL,
  `customizationid` int(11) DEFAULT -1,
  `imagelink` text DEFAULT NULL,
  `banknew` int(11) NOT NULL DEFAULT -1,
  `respectPoints` int(11) NOT NULL DEFAULT 0,
  `bonusPoints` int(11) NOT NULL DEFAULT 0,
  `mypromocode` text DEFAULT NULL,
  `countUseMyPromocode` int(11) NOT NULL DEFAULT 0,
  `primeLeftFraction` datetime DEFAULT NULL,
  `vipdate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `chatlogs`
--

CREATE TABLE `chatlogs` (
  `idkey` int(11) NOT NULL,
  `uuid` int(11) NOT NULL,
  `typechat` int(11) NOT NULL,
  `message` text NOT NULL,
  `datetime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `colshapes`
--

CREATE TABLE `colshapes` (
  `ID` varchar(155) NOT NULL,
  `Rank` varchar(155) NOT NULL,
  `FPosX` varchar(155) NOT NULL,
  `FPosY` varchar(155) NOT NULL,
  `FPosZ` varchar(155) NOT NULL,
  `FPosDim` varchar(155) NOT NULL,
  `TPosX` varchar(155) NOT NULL,
  `TPosY` varchar(155) NOT NULL,
  `TPosZ` varchar(155) NOT NULL,
  `TPosDim` varchar(155) NOT NULL,
  `Revers` varchar(155) NOT NULL,
  `ForVeh` varchar(155) NOT NULL,
  `Interact` varchar(155) NOT NULL,
  `Fraction` varchar(155) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `complaints`
--

CREATE TABLE `complaints` (
  `id` int(11) NOT NULL,
  `applicantuuid` int(11) NOT NULL,
  `fractionid` int(11) NOT NULL,
  `employeeuuid` int(11) NOT NULL,
  `text` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `connlog`
--

CREATE TABLE `connlog` (
  `uuid` text NOT NULL,
  `in` datetime NOT NULL,
  `out` datetime DEFAULT NULL,
  `sclub` text NOT NULL,
  `hwid` text DEFAULT NULL,
  `ip` text NOT NULL,
  `null` varchar(155) DEFAULT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `contracts`
--

CREATE TABLE `contracts` (
  `ownerid` int(11) NOT NULL,
  `ownerType` int(11) NOT NULL,
  `contractName` int(11) NOT NULL,
  `countCompleted` int(11) NOT NULL DEFAULT 0,
  `currentLevel` int(11) NOT NULL DEFAULT 0,
  `inProgress` tinyint(4) NOT NULL DEFAULT 0,
  `expirationDate` datetime NOT NULL,
  `countPlayersProgress` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `customization`
--

CREATE TABLE `customization` (
  `idkey` int(11) NOT NULL,
  `uuid` bigint(20) NOT NULL,
  `gender` text NOT NULL,
  `parents` text DEFAULT NULL,
  `features` text DEFAULT NULL,
  `appearance` text DEFAULT NULL,
  `hair` text DEFAULT NULL,
  `clothes` text DEFAULT NULL,
  `accessory` text DEFAULT NULL,
  `tattoos` text DEFAULT NULL,
  `eyebrowc` text DEFAULT NULL,
  `beardc` text DEFAULT NULL,
  `eyec` text DEFAULT NULL,
  `blushc` text DEFAULT NULL,
  `lipstickc` text DEFAULT NULL,
  `chesthairc` text DEFAULT NULL,
  `iscreated` tinyint(1) DEFAULT NULL,
  `makeup` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `deletelog`
--

CREATE TABLE `deletelog` (
  `time` datetime NOT NULL,
  `uuid` text NOT NULL,
  `name` text NOT NULL,
  `account` text NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `donate`
--

CREATE TABLE `donate` (
  `id` int(11) NOT NULL,
  `unitpayid` int(11) NOT NULL DEFAULT 0,
  `login` varchar(45) NOT NULL,
  `type` varchar(15) NOT NULL,
  `value` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `sum` int(11) DEFAULT 0,
  `promo` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `donateitems`
--

CREATE TABLE `donateitems` (
  `id` int(11) NOT NULL,
  `type` int(11) NOT NULL,
  `name` varchar(64) NOT NULL,
  `price` int(11) NOT NULL,
  `count` int(11) NOT NULL DEFAULT 0,
  `maxcount` int(11) NOT NULL,
  `status` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `donateroulettelog`
--

CREATE TABLE `donateroulettelog` (
  `idkey` int(11) NOT NULL,
  `time` datetime DEFAULT NULL,
  `name` varchar(60) DEFAULT NULL,
  `uuid` int(11) DEFAULT NULL,
  `droprarity` int(11) DEFAULT NULL,
  `droptype` int(11) DEFAULT NULL,
  `dropvalue` varchar(60) DEFAULT NULL,
  `isfree` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `donate_errors`
--

CREATE TABLE `donate_errors` (
  `id` int(11) NOT NULL,
  `orderid` varchar(11) NOT NULL,
  `error` text NOT NULL,
  `date` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `donate_history`
--

CREATE TABLE `donate_history` (
  `id` int(11) NOT NULL,
  `name` varchar(90) NOT NULL,
  `operation` text NOT NULL,
  `sum` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `login` varchar(90) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `donate_inventories`
--

CREATE TABLE `donate_inventories` (
  `id` int(11) NOT NULL,
  `items` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `donate_items_log`
--

CREATE TABLE `donate_items_log` (
  `id` int(11) NOT NULL,
  `login` text NOT NULL,
  `uuid` int(11) NOT NULL,
  `itemid` int(11) NOT NULL,
  `itemname` text NOT NULL,
  `sum` int(11) NOT NULL,
  `operation` text NOT NULL,
  `date` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `donate_roulettes`
--

CREATE TABLE `donate_roulettes` (
  `id` int(11) NOT NULL,
  `rouletteid` int(11) NOT NULL,
  `bank` int(11) NOT NULL,
  `total_game` int(11) NOT NULL,
  `total_spend` bigint(20) NOT NULL,
  `total_drop` bigint(20) NOT NULL,
  `rarity_data` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `dooraccess`
--

CREATE TABLE `dooraccess` (
  `id` int(11) NOT NULL,
  `uuid` int(11) NOT NULL,
  `accessname` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `efcore_bank_account`
--

CREATE TABLE `efcore_bank_account` (
  `ID` int(11) NOT NULL,
  `UUID` int(11) NOT NULL,
  `BankId` int(11) NOT NULL,
  `Number` bigint(20) NOT NULL,
  `OwnerType` int(11) NOT NULL,
  `Balance` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `efcore_bank_credit`
--

CREATE TABLE `efcore_bank_credit` (
  `ID` int(11) NOT NULL,
  `UUID` int(11) NOT NULL,
  `BankId` int(11) NOT NULL,
  `TypePayment` int(11) NOT NULL,
  `Indebtedness` int(11) NOT NULL,
  `LeftPayments` int(11) NOT NULL,
  `PledgeId` int(11) NOT NULL,
  `PledgeType` int(11) NOT NULL,
  `PayedAmount` int(11) NOT NULL,
  `Percents` int(11) NOT NULL,
  `ClosedCredit` tinyint(1) NOT NULL,
  `HistoryPayment` longtext DEFAULT NULL,
  `Create` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `InterestRate` float NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `efcore_bank_deposit`
--

CREATE TABLE `efcore_bank_deposit` (
  `ID` int(11) NOT NULL,
  `UUID` int(11) NOT NULL,
  `BankId` int(11) NOT NULL,
  `Amount` int(11) NOT NULL,
  `Profit` int(11) NOT NULL,
  `DepositMoney` int(11) NOT NULL,
  `AddedMoney` int(11) NOT NULL,
  `DepositTypes` int(11) NOT NULL,
  `HoursInInterval` int(11) NOT NULL,
  `DepositFullTime` int(11) NOT NULL,
  `ClosedDeposit` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `efcore_bank_transact`
--

CREATE TABLE `efcore_bank_transact` (
  `Id` int(11) NOT NULL,
  `From` int(11) NOT NULL,
  `FromType` int(11) NOT NULL,
  `To` int(11) NOT NULL,
  `ToType` int(11) NOT NULL,
  `Amount` int(11) NOT NULL,
  `Tax` int(11) NOT NULL,
  `Comment` longtext DEFAULT NULL,
  `Date` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `equips`
--

CREATE TABLE `equips` (
  `id` int(11) NOT NULL,
  `clothes` text NOT NULL,
  `weapons` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `errorlogs`
--

CREATE TABLE `errorlogs` (
  `id` int(11) NOT NULL,
  `log` text DEFAULT NULL,
  `time` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `eventcfg`
--

CREATE TABLE `eventcfg` (
  `RewardLimit` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `eventslog`
--

CREATE TABLE `eventslog` (
  `AdminStarted` text NOT NULL,
  `EventName` text NOT NULL,
  `MembersLimit` text NOT NULL,
  `Started` text NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `e_candidates`
--

CREATE TABLE `e_candidates` (
  `ID` int(11) NOT NULL,
  `Votes` text NOT NULL,
  `Election` text NOT NULL,
  `Name` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `e_points`
--

CREATE TABLE `e_points` (
  `Election` int(11) NOT NULL,
  `X` varchar(11) NOT NULL,
  `Y` varchar(11) NOT NULL,
  `Z` varchar(11) NOT NULL,
  `Dimension` int(11) NOT NULL,
  `Opened` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `e_voters`
--

CREATE TABLE `e_voters` (
  `Election` int(11) NOT NULL,
  `Login` text NOT NULL,
  `TimeVoted` text NOT NULL,
  `VotedFor` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `families`
--

CREATE TABLE `families` (
  `f_id` int(11) NOT NULL,
  `f_name` varchar(255) NOT NULL,
  `f_owner` int(11) DEFAULT NULL,
  `f_ranks` text DEFAULT NULL,
  `f_money` bigint(20) NOT NULL DEFAULT 0,
  `f_biography` text NOT NULL,
  `f_nation` text DEFAULT NULL,
  `f_chaticon` int(11) NOT NULL DEFAULT 1,
  `f_chatcolor` text DEFAULT NULL,
  `f_elo` int(11) NOT NULL DEFAULT 1000,
  `f_cntbattles` int(11) NOT NULL DEFAULT 0,
  `f_taboo` text DEFAULT NULL,
  `f_rules` text DEFAULT NULL,
  `f_clothespoint` text DEFAULT NULL,
  `f_clothesdim` int(10) UNSIGNED NOT NULL DEFAULT 0,
  `f_moneylimit` int(11) NOT NULL DEFAULT 0,
  `f_typefam` int(11) NOT NULL DEFAULT 0,
  `f_points` int(11) NOT NULL DEFAULT 0,
  `f_respectPoints` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `familybattles`
--

CREATE TABLE `familybattles` (
  `id` int(11) NOT NULL,
  `business` int(11) NOT NULL DEFAULT -1,
  `familydef` int(11) NOT NULL DEFAULT -1,
  `familyattack` int(11) NOT NULL DEFAULT -1,
  `location` int(11) NOT NULL DEFAULT -1,
  `weapon` int(11) NOT NULL DEFAULT -1,
  `time` datetime NOT NULL,
  `comment` text DEFAULT NULL,
  `status` int(11) NOT NULL DEFAULT 0,
  `familywinner` int(11) DEFAULT -1,
  `famdefpoint` int(11) DEFAULT 0,
  `famattackpoint` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `familycompanies`
--

CREATE TABLE `familycompanies` (
  `id` int(11) NOT NULL,
  `position` text NOT NULL,
  `key` int(11) NOT NULL,
  `ownerid` int(11) NOT NULL DEFAULT 0,
  `datecapt` datetime DEFAULT NULL,
  `ownertype` int(11) NOT NULL DEFAULT 1,
  `rotation` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `familymp`
--

CREATE TABLE `familymp` (
  `id` int(11) NOT NULL,
  `date` text NOT NULL,
  `location` int(11) NOT NULL DEFAULT 1,
  `winner` int(11) NOT NULL DEFAULT 0,
  `finished` tinyint(4) NOT NULL DEFAULT 0,
  `type` int(11) NOT NULL DEFAULT 1,
  `kills` text DEFAULT NULL,
  `business` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `farms`
--

CREATE TABLE `farms` (
  `id` int(11) NOT NULL,
  `gangOwnerId` int(11) NOT NULL,
  `seedsOnStock` int(11) NOT NULL,
  `productsOnStock` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `farms_plants`
--

CREATE TABLE `farms_plants` (
  `id` int(11) NOT NULL,
  `farmId` int(11) NOT NULL,
  `seedAt` datetime DEFAULT NULL,
  `growingStage` int(11) DEFAULT NULL,
  `plantType` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `fisher_data`
--

CREATE TABLE `fisher_data` (
  `id` int(11) NOT NULL,
  `socialname` varchar(45) NOT NULL,
  `lvl` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `license` int(11) NOT NULL,
  `map_expires` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `fishing_spots`
--

CREATE TABLE `fishing_spots` (
  `id` int(11) NOT NULL,
  `position` text NOT NULL,
  `rotation` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `fractionaccess`
--

CREATE TABLE `fractionaccess` (
  `idkey` int(11) NOT NULL,
  `fraction` int(11) NOT NULL,
  `commands` text NOT NULL,
  `weapons` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `fractionmoney`
--

CREATE TABLE `fractionmoney` (
  `id` int(11) NOT NULL,
  `fractionId` int(11) NOT NULL,
  `operation` varchar(3) NOT NULL,
  `sum` int(11) NOT NULL,
  `date` datetime(2) NOT NULL,
  `description` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `fractionranks`
--

CREATE TABLE `fractionranks` (
  `idkey` int(11) NOT NULL,
  `fraction` int(11) NOT NULL,
  `rank` int(11) NOT NULL,
  `payday` int(11) NOT NULL,
  `name` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `fractions`
--

CREATE TABLE `fractions` (
  `id` int(11) NOT NULL,
  `money` int(11) NOT NULL,
  `fuellimit` int(11) NOT NULL,
  `fuelleft` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `fractionstock`
--

CREATE TABLE `fractionstock` (
  `id` int(11) NOT NULL,
  `inventoryid` int(11) NOT NULL,
  `fractionid` int(11) NOT NULL,
  `dimension` int(10) UNSIGNED NOT NULL DEFAULT 0,
  `position` text NOT NULL,
  `password` text DEFAULT NULL,
  `typeowner` int(11) NOT NULL DEFAULT 2,
  `size` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `fractionvehicles`
--

CREATE TABLE `fractionvehicles` (
  `fraction` int(11) NOT NULL,
  `number` text NOT NULL,
  `components` text DEFAULT NULL,
  `model` text NOT NULL,
  `position` text NOT NULL,
  `rotation` text NOT NULL,
  `rank` int(11) NOT NULL,
  `colorprim` int(11) NOT NULL,
  `colorsec` int(11) NOT NULL,
  `idkey` int(11) NOT NULL,
  `power` double NOT NULL DEFAULT 1,
  `torque` double NOT NULL DEFAULT 1,
  `items` text NOT NULL,
  `componentsnew` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `fraction_access`
--

CREATE TABLE `fraction_access` (
  `Id` int(11) NOT NULL,
  `FractionId` int(11) NOT NULL,
  `FractionRank` int(11) NOT NULL,
  `AccessList` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `furniture`
--

CREATE TABLE `furniture` (
  `uuid` varchar(155) NOT NULL,
  `furniture` text NOT NULL,
  `data` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `gangspoints`
--

CREATE TABLE `gangspoints` (
  `id` int(11) NOT NULL,
  `gangid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `garages`
--

CREATE TABLE `garages` (
  `id` int(11) NOT NULL,
  `type` int(11) NOT NULL,
  `position` text NOT NULL,
  `rotation` text NOT NULL,
  `typeowner` double NOT NULL DEFAULT 0,
  `nativeType` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `houses`
--

CREATE TABLE `houses` (
  `id` int(11) NOT NULL,
  `owner` text DEFAULT NULL,
  `type` varchar(11) NOT NULL,
  `position` text NOT NULL,
  `price` text NOT NULL,
  `locked` tinyint(4) NOT NULL,
  `garage` text NOT NULL,
  `bank` text DEFAULT NULL,
  `roommates` text DEFAULT NULL,
  `isAdmin` int(11) NOT NULL DEFAULT 0,
  `typeowner` double NOT NULL DEFAULT 0,
  `owneruuid` int(11) NOT NULL DEFAULT -1,
  `roommatesuuid` text DEFAULT NULL,
  `inventoryId` int(11) NOT NULL DEFAULT -1,
  `rentCost` int(11) NOT NULL DEFAULT 0,
  `furnitures` text DEFAULT NULL,
  `banknew` int(11) NOT NULL DEFAULT -1,
  `pledged` tinyint(1) NOT NULL DEFAULT 0,
  `camposition` text DEFAULT NULL,
  `occupiers` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `idlog`
--

CREATE TABLE `idlog` (
  `in` datetime NOT NULL,
  `out` datetime DEFAULT NULL,
  `id` text NOT NULL,
  `name` text NOT NULL,
  `uuid` text NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `inventories`
--

CREATE TABLE `inventories` (
  `id` int(11) NOT NULL,
  `maxWeight` int(11) NOT NULL,
  `size` int(11) NOT NULL,
  `items` text NOT NULL,
  `type` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `inventory`
--

CREATE TABLE `inventory` (
  `items` text NOT NULL,
  `uuid` int(11) NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `itemslog`
--

CREATE TABLE `itemslog` (
  `idkey` int(11) NOT NULL,
  `time` datetime NOT NULL,
  `from` text NOT NULL,
  `to` text NOT NULL,
  `type` int(11) NOT NULL,
  `amount` int(11) NOT NULL,
  `data` text NOT NULL,
  `id` int(11) NOT NULL,
  `action` int(11) NOT NULL DEFAULT 0,
  `player` int(11) NOT NULL DEFAULT -1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `killog`
--

CREATE TABLE `killog` (
  `id` int(11) NOT NULL,
  `killer` varchar(45) NOT NULL,
  `target` varchar(45) NOT NULL,
  `clientweapon` text NOT NULL,
  `serverweapon` text NOT NULL,
  `date` datetime(2) NOT NULL,
  `distance` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `main`
--

CREATE TABLE `main` (
  `Fraction` int(11) NOT NULL DEFAULT 0,
  `Rank` double NOT NULL DEFAULT 0,
  `FPosX` double NOT NULL DEFAULT 0,
  `FPosY` double NOT NULL DEFAULT 0,
  `FPosZ` double NOT NULL DEFAULT 0,
  `FPosDim` double NOT NULL DEFAULT 0,
  `TPosX` double NOT NULL DEFAULT 0,
  `TPosY` double NOT NULL DEFAULT 0,
  `TPosZ` double NOT NULL DEFAULT 0,
  `TPosDim` double NOT NULL DEFAULT 0,
  `Revers` int(11) NOT NULL DEFAULT 0,
  `ForVeh` int(11) NOT NULL DEFAULT 0,
  `Interact` int(11) NOT NULL DEFAULT 0,
  `SID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `money`
--

CREATE TABLE `money` (
  `id` varchar(155) NOT NULL,
  `holder` varchar(155) DEFAULT '',
  `balance` int(11) NOT NULL,
  `type` varchar(155) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `moneylog`
--

CREATE TABLE `moneylog` (
  `time` datetime NOT NULL,
  `from` text NOT NULL,
  `to` text NOT NULL,
  `amount` int(11) NOT NULL,
  `comment` text NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `namelog`
--

CREATE TABLE `namelog` (
  `time` datetime NOT NULL,
  `uuid` text NOT NULL,
  `old` text NOT NULL,
  `new` text NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `newcustomization`
--

CREATE TABLE `newcustomization` (
  `id` int(11) NOT NULL,
  `gender` tinyint(1) NOT NULL,
  `eyecolor` int(11) NOT NULL,
  `headoverlays` text NOT NULL,
  `headblend` text NOT NULL,
  `facefeatures` text NOT NULL,
  `hairs` text NOT NULL,
  `tattoos` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `newmoneylog`
--

CREATE TABLE `newmoneylog` (
  `id` int(11) NOT NULL,
  `time` datetime NOT NULL,
  `fromtype` int(11) NOT NULL,
  `from` int(11) NOT NULL,
  `totype` int(11) NOT NULL,
  `to` int(11) NOT NULL,
  `amount` int(11) NOT NULL,
  `tax` int(11) NOT NULL,
  `comment` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `nicknames`
--

CREATE TABLE `nicknames` (
  `srv` varchar(155) NOT NULL,
  `name` varchar(155) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `oldhouses`
--

CREATE TABLE `oldhouses` (
  `id` int(11) NOT NULL,
  `owner` text NOT NULL,
  `type` varchar(11) NOT NULL,
  `position` text NOT NULL,
  `price` text NOT NULL,
  `locked` tinyint(4) NOT NULL,
  `garage` text NOT NULL,
  `bank` text NOT NULL,
  `roommates` text NOT NULL,
  `isAdmin` int(11) NOT NULL DEFAULT 0,
  `typeowner` double NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `othervehicles`
--

CREATE TABLE `othervehicles` (
  `type` varchar(155) NOT NULL,
  `number` text DEFAULT NULL,
  `model` text NOT NULL,
  `position` text DEFAULT NULL,
  `rotation` text DEFAULT NULL,
  `color1` int(11) NOT NULL,
  `color2` int(11) NOT NULL,
  `price` int(11) NOT NULL,
  `idkey` int(11) NOT NULL,
  `businessID` int(11) DEFAULT NULL,
  `uuid` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `parliament_sitting`
--

CREATE TABLE `parliament_sitting` (
  `id` int(11) NOT NULL,
  `choices` text NOT NULL,
  `speakeruuid` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `topic` text NOT NULL,
  `desc` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `philanthropists`
--

CREATE TABLE `philanthropists` (
  `id` int(11) NOT NULL,
  `uuid` int(11) NOT NULL,
  `amount` int(11) NOT NULL,
  `time` datetime NOT NULL,
  `target` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones`
--

CREATE TABLE `phones` (
  `CharacterUuid` int(11) NOT NULL,
  `InstalledAppsIds` longtext DEFAULT NULL,
  `SimCardId` int(11) DEFAULT NULL,
  `AccountId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_bank_transact`
--

CREATE TABLE `phones_bank_transact` (
  `Id` int(11) NOT NULL,
  `From` int(11) NOT NULL,
  `FromType` int(11) NOT NULL,
  `To` int(11) NOT NULL,
  `ToType` int(11) NOT NULL,
  `Amount` int(11) NOT NULL,
  `Date` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_blocks`
--

CREATE TABLE `phones_blocks` (
  `SimCardId` int(11) NOT NULL,
  `TargetNumber` int(11) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_callhistory`
--

CREATE TABLE `phones_callhistory` (
  `Id` int(11) NOT NULL,
  `FromSimCardId` int(11) NOT NULL,
  `TargetNumber` int(11) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `CallStatus` int(11) NOT NULL,
  `Duration` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_contacts`
--

CREATE TABLE `phones_contacts` (
  `Id` int(11) NOT NULL,
  `HolderSimCardId` int(11) NOT NULL,
  `TargetNumber` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_msg_accounts`
--

CREATE TABLE `phones_msg_accounts` (
  `Id` int(11) NOT NULL,
  `Username` longtext DEFAULT NULL,
  `SimCardId` int(11) NOT NULL,
  `DisplayedName` longtext DEFAULT NULL,
  `IsNumberHided` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `LastVisit` datetime(6) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_msg_accountstochat`
--

CREATE TABLE `phones_msg_accountstochat` (
  `AccountId` int(11) NOT NULL,
  `ChatId` int(11) NOT NULL,
  `IsLeaved` tinyint(1) NOT NULL,
  `IsMuted` tinyint(1) NOT NULL,
  `LastReadMessageId` int(11) DEFAULT NULL,
  `AdminLvl` int(11) NOT NULL,
  `IsBlocked` tinyint(1) NOT NULL,
  `Permissions` longtext DEFAULT NULL,
  `BlockedById` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_msg_chats`
--

CREATE TABLE `phones_msg_chats` (
  `Id` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `Type` int(11) NOT NULL,
  `Description` longtext DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Avatar` longtext DEFAULT NULL,
  `InviteCode` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_msg_contacts`
--

CREATE TABLE `phones_msg_contacts` (
  `ContactId` int(11) NOT NULL,
  `HolderAccountId` int(11) NOT NULL,
  `TargetAccountId` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_msg_messages`
--

CREATE TABLE `phones_msg_messages` (
  `Id` int(11) NOT NULL,
  `Text` longtext DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `SenderId` int(11) NOT NULL,
  `ChatId` int(11) NOT NULL,
  `IsRead` tinyint(1) NOT NULL,
  `Attachments` longtext DEFAULT NULL,
  `Discriminator` longtext NOT NULL,
  `Title` longtext DEFAULT NULL,
  `Photo` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_news_advert`
--

CREATE TABLE `phones_news_advert` (
  `Id` int(11) NOT NULL,
  `SenderUUID` int(11) NOT NULL,
  `EditorUUID` int(11) NOT NULL,
  `DateCreate` datetime(6) NOT NULL,
  `DateCompleate` datetime(6) NOT NULL,
  `PhoneNumber` int(11) NOT NULL,
  `MessengerLogin` longtext DEFAULT NULL,
  `Text` longtext DEFAULT NULL,
  `PrimeAdvert` tinyint(1) NOT NULL,
  `ImageUrl` longtext DEFAULT NULL,
  `Status` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_simcards`
--

CREATE TABLE `phones_simcards` (
  `Id` int(11) NOT NULL,
  `Number` int(11) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `BankNumber` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `phones_taxi_orders`
--

CREATE TABLE `phones_taxi_orders` (
  `Id` int(11) NOT NULL,
  `DriverUuid` int(11) NOT NULL,
  `Date` datetime(6) NOT NULL,
  `TotalPrice` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `prime`
--

CREATE TABLE `prime` (
  `id` int(11) NOT NULL,
  `prime_id` int(11) NOT NULL DEFAULT 0,
  `login` varchar(45) NOT NULL,
  `type` varchar(15) NOT NULL,
  `value` int(11) NOT NULL,
  `sum` int(11) DEFAULT 0,
  `promo` varchar(45) DEFAULT 'noref',
  `date` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `prime_errors`
--

CREATE TABLE `prime_errors` (
  `id` int(11) NOT NULL,
  `orderid` varchar(11) NOT NULL,
  `error` text NOT NULL,
  `date` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `prime_history`
--

CREATE TABLE `prime_history` (
  `id` int(11) NOT NULL,
  `name` varchar(90) NOT NULL,
  `operation` text NOT NULL,
  `sum` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `login` varchar(90) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `promos`
--

CREATE TABLE `promos` (
  `name` text NOT NULL,
  `money` int(11) NOT NULL,
  `mcoins` int(11) NOT NULL,
  `usages` int(11) DEFAULT 0,
  `owneruuid` int(11) NOT NULL,
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `propsprices`
--

CREATE TABLE `propsprices` (
  `id` int(11) NOT NULL,
  `name` text NOT NULL,
  `price` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `questions`
--

CREATE TABLE `questions` (
  `ID` int(10) UNSIGNED NOT NULL,
  `Author` varchar(40) NOT NULL,
  `Question` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `Respondent` varchar(40) DEFAULT NULL,
  `Response` varchar(240) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL,
  `Opened` datetime NOT NULL,
  `Closed` datetime DEFAULT NULL,
  `Status` tinyint(4) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `rentcarpoint`
--

CREATE TABLE `rentcarpoint` (
  `id` int(11) NOT NULL,
  `positionped` text NOT NULL,
  `rotationped` double NOT NULL,
  `positioncar` text NOT NULL,
  `rotationcar` double NOT NULL,
  `categories` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `reportmessages`
--

CREATE TABLE `reportmessages` (
  `id` int(11) NOT NULL,
  `reportid` int(11) NOT NULL,
  `senderuuid` int(11) NOT NULL,
  `message` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `reports`
--

CREATE TABLE `reports` (
  `id` int(11) NOT NULL,
  `authoruuid` int(11) NOT NULL,
  `opendate` varchar(45) NOT NULL,
  `closedate` varchar(45) DEFAULT NULL,
  `rating` int(11) NOT NULL DEFAULT -1,
  `adminuuid` int(11) NOT NULL DEFAULT -1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `resetcharacters`
--

CREATE TABLE `resetcharacters` (
  `id` int(11) NOT NULL,
  `compleete` int(11) DEFAULT NULL,
  `hours` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `roulette_statics`
--

CREATE TABLE `roulette_statics` (
  `idkey` int(11) NOT NULL,
  `spend_overall` bigint(20) NOT NULL DEFAULT 0,
  `droped_overall` bigint(20) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `safes`
--

CREATE TABLE `safes` (
  `minamount` int(11) NOT NULL,
  `maxamount` int(11) NOT NULL,
  `pos` text NOT NULL,
  `address` text NOT NULL,
  `rotation` int(11) NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `stocklog`
--

CREATE TABLE `stocklog` (
  `time` datetime NOT NULL,
  `frac` text NOT NULL,
  `uuid` text NOT NULL,
  `type` text NOT NULL,
  `amount` text NOT NULL,
  `in` text NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `teleports`
--

CREATE TABLE `teleports` (
  `id` int(11) NOT NULL,
  `enterPoint` text DEFAULT NULL,
  `enterDimension` int(10) UNSIGNED DEFAULT NULL,
  `exitPoint` text DEFAULT NULL,
  `exitDimension` int(10) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `ticketlog`
--

CREATE TABLE `ticketlog` (
  `time` datetime NOT NULL,
  `player` text NOT NULL,
  `target` text NOT NULL,
  `sum` text NOT NULL,
  `reason` text NOT NULL,
  `pnick` text NOT NULL,
  `tnick` text NOT NULL,
  `idkey` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `vehicles`
--

CREATE TABLE `vehicles` (
  `idkey` int(11) NOT NULL,
  `holderuuid` int(11) NOT NULL DEFAULT -1,
  `number` varchar(155) NOT NULL DEFAULT '',
  `model` varchar(155) NOT NULL,
  `componentsnew` text DEFAULT NULL,
  `position` varchar(255) DEFAULT NULL,
  `rotation` varchar(255) DEFAULT NULL,
  `typeowner` int(11) NOT NULL DEFAULT 0,
  `power` double NOT NULL DEFAULT 0,
  `torque` double NOT NULL DEFAULT 1,
  `price` int(11) DEFAULT NULL,
  `keynum` int(11) DEFAULT 0,
  `streamer` tinyint(1) DEFAULT 0,
  `donatecar` tinyint(1) DEFAULT 0,
  `isdeath` tinyint(1) DEFAULT 0,
  `items` text DEFAULT NULL,
  `fuel` int(11) DEFAULT 0,
  `dirt` float DEFAULT 0,
  `doorbreak` int(11) DEFAULT 0,
  `mileage` int(11) DEFAULT 0,
  `mileageoilchange` int(11) DEFAULT 0,
  `mileagebrakepadschange` int(11) DEFAULT 0,
  `mileagetransmissionservice` int(11) DEFAULT 0,
  `enginehealth` float DEFAULT 0,
  `rank` int(11) DEFAULT NULL,
  `holder` varchar(155) DEFAULT NULL,
  `dimension` int(10) UNSIGNED DEFAULT 0,
  `inventoryId` int(11) NOT NULL DEFAULT -1,
  `wantedlevel` text DEFAULT NULL,
  `saveposition` varchar(255) DEFAULT NULL,
  `saverotation` varchar(255) DEFAULT NULL,
  `isdeleted` tinyint(1) NOT NULL DEFAULT 0,
  `tradepoint` int(11) NOT NULL DEFAULT -1,
  `dirtclear` datetime DEFAULT NULL,
  `pledged` tinyint(1) NOT NULL DEFAULT 0,
  `buytype` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `vehicletrading`
--

CREATE TABLE `vehicletrading` (
  `id` int(11) NOT NULL,
  `position` text NOT NULL,
  `rotation` text NOT NULL,
  `currentveh` int(11) NOT NULL DEFAULT -1,
  `price` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `vehicle_configurations`
--

CREATE TABLE `vehicle_configurations` (
  `model` int(10) UNSIGNED NOT NULL,
  `modelName` varchar(150) DEFAULT NULL,
  `displayName` varchar(150) DEFAULT NULL,
  `slots` int(11) DEFAULT NULL,
  `maxWeight` int(11) DEFAULT NULL,
  `maxFuel` int(11) DEFAULT NULL,
  `fuelConsumption` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `weapondamageconfigs`
--

CREATE TABLE `weapondamageconfigs` (
  `id` int(11) NOT NULL,
  `weaponHash` int(10) UNSIGNED NOT NULL,
  `baseDamage` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `weapondamagemodifiers`
--

CREATE TABLE `weapondamagemodifiers` (
  `id` int(11) NOT NULL,
  `value` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `weapons`
--

CREATE TABLE `weapons` (
  `id` varchar(155) NOT NULL,
  `lastserial` varchar(155) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `weedfarm`
--

CREATE TABLE `weedfarm` (
  `id` int(11) NOT NULL,
  `ownerId` int(11) NOT NULL DEFAULT 0,
  `occupationDate` datetime(4) NOT NULL DEFAULT '2021-01-01 00:00:00.0000',
  `components` text NOT NULL,
  `onDrying` int(11) NOT NULL DEFAULT 0,
  `onPacking` int(11) NOT NULL DEFAULT 0,
  `onDelivery` int(11) NOT NULL DEFAULT 0,
  `irrigationSystem` int(11) NOT NULL DEFAULT 100,
  `lightSystem` int(11) NOT NULL DEFAULT 100,
  `dyringSystem` int(11) NOT NULL DEFAULT 100,
  `ventilationSystem` int(11) DEFAULT 100,
  `enterPosition` text DEFAULT NULL,
  `vehPosition` text DEFAULT NULL,
  `vehRotation` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `whitelist`
--

CREATE TABLE `whitelist` (
  `id` int(11) NOT NULL,
  `socialclub` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `workers`
--

CREATE TABLE `workers` (
  `id` int(11) NOT NULL,
  `uuid` int(11) NOT NULL,
  `exp` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `achievements`
--
ALTER TABLE `achievements`
  ADD PRIMARY KEY (`uuid`,`achieveName`) USING BTREE;

--
-- Indexes for table `adminaccess`
--
ALTER TABLE `adminaccess`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `adminlog`
--
ALTER TABLE `adminlog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `advertised`
--
ALTER TABLE `advertised`
  ADD PRIMARY KEY (`ID`) USING BTREE;

--
-- Indexes for table `alcobars`
--
ALTER TABLE `alcobars`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `alcoclubs`
--
ALTER TABLE `alcoclubs`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `armorpoints`
--
ALTER TABLE `armorpoints`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `arrestlog`
--
ALTER TABLE `arrestlog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `arrests`
--
ALTER TABLE `arrests`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `bankpoints`
--
ALTER TABLE `bankpoints`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `banlog`
--
ALTER TABLE `banlog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `banned`
--
ALTER TABLE `banned`
  ADD PRIMARY KEY (`uuid`) USING BTREE;

--
-- Indexes for table `bizsettings`
--
ALTER TABLE `bizsettings`
  ADD PRIMARY KEY (`biztype`) USING BTREE,
  ADD UNIQUE KEY `biztype_UNIQUE` (`biztype`) USING BTREE;

--
-- Indexes for table `blacklist`
--
ALTER TABLE `blacklist`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `bonuscodes`
--
ALTER TABLE `bonuscodes`
  ADD PRIMARY KEY (`id`) USING BTREE,
  ADD UNIQUE KEY `bonusname_UNIQUE` (`bonusname`) USING BTREE;

--
-- Indexes for table `boxlog`
--
ALTER TABLE `boxlog`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `businesses`
--
ALTER TABLE `businesses`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `cartradecars`
--
ALTER TABLE `cartradecars`
  ADD PRIMARY KEY (`carid`) USING BTREE;

--
-- Indexes for table `casino`
--
ALTER TABLE `casino`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `casinobetlog`
--
ALTER TABLE `casinobetlog`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `casinoendlog`
--
ALTER TABLE `casinoendlog`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `casinowinloselog`
--
ALTER TABLE `casinowinloselog`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `characters`
--
ALTER TABLE `characters`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `chatlogs`
--
ALTER TABLE `chatlogs`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `colshapes`
--
ALTER TABLE `colshapes`
  ADD PRIMARY KEY (`ID`) USING BTREE;

--
-- Indexes for table `complaints`
--
ALTER TABLE `complaints`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `connlog`
--
ALTER TABLE `connlog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `contracts`
--
ALTER TABLE `contracts`
  ADD PRIMARY KEY (`ownerid`,`ownerType`,`contractName`) USING BTREE;

--
-- Indexes for table `customization`
--
ALTER TABLE `customization`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `deletelog`
--
ALTER TABLE `deletelog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `donate`
--
ALTER TABLE `donate`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `donateitems`
--
ALTER TABLE `donateitems`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `donateroulettelog`
--
ALTER TABLE `donateroulettelog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `donate_errors`
--
ALTER TABLE `donate_errors`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `donate_history`
--
ALTER TABLE `donate_history`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `donate_inventories`
--
ALTER TABLE `donate_inventories`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `donate_items_log`
--
ALTER TABLE `donate_items_log`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `donate_roulettes`
--
ALTER TABLE `donate_roulettes`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `dooraccess`
--
ALTER TABLE `dooraccess`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `efcore_bank_account`
--
ALTER TABLE `efcore_bank_account`
  ADD PRIMARY KEY (`ID`) USING BTREE;

--
-- Indexes for table `efcore_bank_credit`
--
ALTER TABLE `efcore_bank_credit`
  ADD PRIMARY KEY (`ID`) USING BTREE;

--
-- Indexes for table `efcore_bank_deposit`
--
ALTER TABLE `efcore_bank_deposit`
  ADD PRIMARY KEY (`ID`) USING BTREE;

--
-- Indexes for table `efcore_bank_transact`
--
ALTER TABLE `efcore_bank_transact`
  ADD PRIMARY KEY (`Id`) USING BTREE;

--
-- Indexes for table `equips`
--
ALTER TABLE `equips`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `errorlogs`
--
ALTER TABLE `errorlogs`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `eventslog`
--
ALTER TABLE `eventslog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `e_candidates`
--
ALTER TABLE `e_candidates`
  ADD PRIMARY KEY (`ID`) USING BTREE;

--
-- Indexes for table `e_points`
--
ALTER TABLE `e_points`
  ADD PRIMARY KEY (`Election`) USING BTREE;

--
-- Indexes for table `families`
--
ALTER TABLE `families`
  ADD PRIMARY KEY (`f_id`) USING BTREE;

--
-- Indexes for table `familybattles`
--
ALTER TABLE `familybattles`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `familycompanies`
--
ALTER TABLE `familycompanies`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `familymp`
--
ALTER TABLE `familymp`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `farms`
--
ALTER TABLE `farms`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `farms_plants`
--
ALTER TABLE `farms_plants`
  ADD PRIMARY KEY (`id`,`farmId`) USING BTREE;

--
-- Indexes for table `fisher_data`
--
ALTER TABLE `fisher_data`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `fishing_spots`
--
ALTER TABLE `fishing_spots`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `fractionaccess`
--
ALTER TABLE `fractionaccess`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `fractionmoney`
--
ALTER TABLE `fractionmoney`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `fractionranks`
--
ALTER TABLE `fractionranks`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `fractions`
--
ALTER TABLE `fractions`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `fractionstock`
--
ALTER TABLE `fractionstock`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `fractionvehicles`
--
ALTER TABLE `fractionvehicles`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `fraction_access`
--
ALTER TABLE `fraction_access`
  ADD PRIMARY KEY (`Id`) USING BTREE;

--
-- Indexes for table `furniture`
--
ALTER TABLE `furniture`
  ADD PRIMARY KEY (`uuid`) USING BTREE;

--
-- Indexes for table `gangspoints`
--
ALTER TABLE `gangspoints`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `garages`
--
ALTER TABLE `garages`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `houses`
--
ALTER TABLE `houses`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `idlog`
--
ALTER TABLE `idlog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `inventories`
--
ALTER TABLE `inventories`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `inventory`
--
ALTER TABLE `inventory`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `itemslog`
--
ALTER TABLE `itemslog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `killog`
--
ALTER TABLE `killog`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `main`
--
ALTER TABLE `main`
  ADD PRIMARY KEY (`SID`) USING BTREE;

--
-- Indexes for table `money`
--
ALTER TABLE `money`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `moneylog`
--
ALTER TABLE `moneylog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `namelog`
--
ALTER TABLE `namelog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `newcustomization`
--
ALTER TABLE `newcustomization`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `newmoneylog`
--
ALTER TABLE `newmoneylog`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `oldhouses`
--
ALTER TABLE `oldhouses`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `othervehicles`
--
ALTER TABLE `othervehicles`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `parliament_sitting`
--
ALTER TABLE `parliament_sitting`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `philanthropists`
--
ALTER TABLE `philanthropists`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `phones`
--
ALTER TABLE `phones`
  ADD PRIMARY KEY (`CharacterUuid`) USING BTREE,
  ADD KEY `IX_Phones_AccountId` (`AccountId`) USING BTREE,
  ADD KEY `IX_Phones_SimCardId` (`SimCardId`) USING BTREE;

--
-- Indexes for table `phones_bank_transact`
--
ALTER TABLE `phones_bank_transact`
  ADD PRIMARY KEY (`Id`) USING BTREE;

--
-- Indexes for table `phones_blocks`
--
ALTER TABLE `phones_blocks`
  ADD PRIMARY KEY (`SimCardId`,`TargetNumber`) USING BTREE;

--
-- Indexes for table `phones_callhistory`
--
ALTER TABLE `phones_callhistory`
  ADD PRIMARY KEY (`Id`) USING BTREE,
  ADD KEY `IX_phones_callhistory_FromSimCardId` (`FromSimCardId`) USING BTREE,
  ADD KEY `IX_phones_callhistory_TargetNumber` (`TargetNumber`) USING BTREE;

--
-- Indexes for table `phones_contacts`
--
ALTER TABLE `phones_contacts`
  ADD PRIMARY KEY (`Id`) USING BTREE,
  ADD KEY `IX_phones_contacts_HolderSimCardId` (`HolderSimCardId`) USING BTREE,
  ADD KEY `IX_phones_contacts_TargetNumber` (`TargetNumber`) USING BTREE;

--
-- Indexes for table `phones_msg_accounts`
--
ALTER TABLE `phones_msg_accounts`
  ADD PRIMARY KEY (`Id`) USING BTREE,
  ADD KEY `IX_phones_msg_accounts_SimCardId` (`SimCardId`) USING BTREE;

--
-- Indexes for table `phones_msg_accountstochat`
--
ALTER TABLE `phones_msg_accountstochat`
  ADD PRIMARY KEY (`AccountId`,`ChatId`) USING BTREE,
  ADD KEY `IX_phones_msg_accountsToChat_ChatId` (`ChatId`) USING BTREE,
  ADD KEY `IX_phones_msg_accountsToChat_LastReadMessageId` (`LastReadMessageId`) USING BTREE,
  ADD KEY `IX_phones_msg_accountsToChat_BlockedById` (`BlockedById`) USING BTREE;

--
-- Indexes for table `phones_msg_chats`
--
ALTER TABLE `phones_msg_chats`
  ADD PRIMARY KEY (`Id`) USING BTREE,
  ADD UNIQUE KEY `IX_phones_msg_chats_InviteCode` (`InviteCode`) USING BTREE;

--
-- Indexes for table `phones_msg_contacts`
--
ALTER TABLE `phones_msg_contacts`
  ADD PRIMARY KEY (`ContactId`) USING BTREE,
  ADD KEY `IX_phones_msg_contacts_HolderAccountId` (`HolderAccountId`) USING BTREE,
  ADD KEY `IX_phones_msg_contacts_TargetAccountId` (`TargetAccountId`) USING BTREE;

--
-- Indexes for table `phones_msg_messages`
--
ALTER TABLE `phones_msg_messages`
  ADD PRIMARY KEY (`Id`) USING BTREE,
  ADD KEY `IX_phones_msg_messages_ChatId` (`ChatId`) USING BTREE,
  ADD KEY `IX_phones_msg_messages_SenderId` (`SenderId`) USING BTREE;

--
-- Indexes for table `phones_news_advert`
--
ALTER TABLE `phones_news_advert`
  ADD PRIMARY KEY (`Id`) USING BTREE;

--
-- Indexes for table `phones_simcards`
--
ALTER TABLE `phones_simcards`
  ADD PRIMARY KEY (`Id`) USING BTREE,
  ADD UNIQUE KEY `IX_phones_simcards_Number` (`Number`) USING BTREE;

--
-- Indexes for table `phones_taxi_orders`
--
ALTER TABLE `phones_taxi_orders`
  ADD PRIMARY KEY (`Id`) USING BTREE;

--
-- Indexes for table `prime`
--
ALTER TABLE `prime`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `prime_errors`
--
ALTER TABLE `prime_errors`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `prime_history`
--
ALTER TABLE `prime_history`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `promos`
--
ALTER TABLE `promos`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `propsprices`
--
ALTER TABLE `propsprices`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `questions`
--
ALTER TABLE `questions`
  ADD PRIMARY KEY (`ID`) USING BTREE;

--
-- Indexes for table `rentcarpoint`
--
ALTER TABLE `rentcarpoint`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `reportmessages`
--
ALTER TABLE `reportmessages`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `reports`
--
ALTER TABLE `reports`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `resetcharacters`
--
ALTER TABLE `resetcharacters`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `roulette_statics`
--
ALTER TABLE `roulette_statics`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `safes`
--
ALTER TABLE `safes`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `stocklog`
--
ALTER TABLE `stocklog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `teleports`
--
ALTER TABLE `teleports`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `ticketlog`
--
ALTER TABLE `ticketlog`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `vehicles`
--
ALTER TABLE `vehicles`
  ADD PRIMARY KEY (`idkey`) USING BTREE;

--
-- Indexes for table `vehicletrading`
--
ALTER TABLE `vehicletrading`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `vehicle_configurations`
--
ALTER TABLE `vehicle_configurations`
  ADD PRIMARY KEY (`model`) USING BTREE;

--
-- Indexes for table `weapondamageconfigs`
--
ALTER TABLE `weapondamageconfigs`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `weapondamagemodifiers`
--
ALTER TABLE `weapondamagemodifiers`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `weapons`
--
ALTER TABLE `weapons`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `weedfarm`
--
ALTER TABLE `weedfarm`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `whitelist`
--
ALTER TABLE `whitelist`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `workers`
--
ALTER TABLE `workers`
  ADD PRIMARY KEY (`id`) USING BTREE;

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`) USING BTREE;

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `accounts`
--
ALTER TABLE `accounts`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `adminaccess`
--
ALTER TABLE `adminaccess`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `adminlog`
--
ALTER TABLE `adminlog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `advertised`
--
ALTER TABLE `advertised`
  MODIFY `ID` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `alcobars`
--
ALTER TABLE `alcobars`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `armorpoints`
--
ALTER TABLE `armorpoints`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `arrestlog`
--
ALTER TABLE `arrestlog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `arrests`
--
ALTER TABLE `arrests`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `bankpoints`
--
ALTER TABLE `bankpoints`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `banlog`
--
ALTER TABLE `banlog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `blacklist`
--
ALTER TABLE `blacklist`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `bonuscodes`
--
ALTER TABLE `bonuscodes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `boxlog`
--
ALTER TABLE `boxlog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `cartradecars`
--
ALTER TABLE `cartradecars`
  MODIFY `carid` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `casinobetlog`
--
ALTER TABLE `casinobetlog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `casinoendlog`
--
ALTER TABLE `casinoendlog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `casinowinloselog`
--
ALTER TABLE `casinowinloselog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `characters`
--
ALTER TABLE `characters`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `chatlogs`
--
ALTER TABLE `chatlogs`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `complaints`
--
ALTER TABLE `complaints`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `connlog`
--
ALTER TABLE `connlog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `customization`
--
ALTER TABLE `customization`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `deletelog`
--
ALTER TABLE `deletelog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `donate`
--
ALTER TABLE `donate`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `donateitems`
--
ALTER TABLE `donateitems`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `donateroulettelog`
--
ALTER TABLE `donateroulettelog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `donate_errors`
--
ALTER TABLE `donate_errors`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `donate_history`
--
ALTER TABLE `donate_history`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `donate_inventories`
--
ALTER TABLE `donate_inventories`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `donate_items_log`
--
ALTER TABLE `donate_items_log`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `donate_roulettes`
--
ALTER TABLE `donate_roulettes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `dooraccess`
--
ALTER TABLE `dooraccess`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `efcore_bank_account`
--
ALTER TABLE `efcore_bank_account`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `efcore_bank_credit`
--
ALTER TABLE `efcore_bank_credit`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `efcore_bank_deposit`
--
ALTER TABLE `efcore_bank_deposit`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `efcore_bank_transact`
--
ALTER TABLE `efcore_bank_transact`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `equips`
--
ALTER TABLE `equips`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `errorlogs`
--
ALTER TABLE `errorlogs`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `eventslog`
--
ALTER TABLE `eventslog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `e_candidates`
--
ALTER TABLE `e_candidates`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `e_points`
--
ALTER TABLE `e_points`
  MODIFY `Election` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `families`
--
ALTER TABLE `families`
  MODIFY `f_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `familybattles`
--
ALTER TABLE `familybattles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `familycompanies`
--
ALTER TABLE `familycompanies`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `familymp`
--
ALTER TABLE `familymp`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `fisher_data`
--
ALTER TABLE `fisher_data`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `fishing_spots`
--
ALTER TABLE `fishing_spots`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `fractionaccess`
--
ALTER TABLE `fractionaccess`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `fractionmoney`
--
ALTER TABLE `fractionmoney`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `fractionranks`
--
ALTER TABLE `fractionranks`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `fractionstock`
--
ALTER TABLE `fractionstock`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `fractionvehicles`
--
ALTER TABLE `fractionvehicles`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `fraction_access`
--
ALTER TABLE `fraction_access`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `idlog`
--
ALTER TABLE `idlog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `inventories`
--
ALTER TABLE `inventories`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `inventory`
--
ALTER TABLE `inventory`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `itemslog`
--
ALTER TABLE `itemslog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `killog`
--
ALTER TABLE `killog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `moneylog`
--
ALTER TABLE `moneylog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `namelog`
--
ALTER TABLE `namelog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `newcustomization`
--
ALTER TABLE `newcustomization`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `newmoneylog`
--
ALTER TABLE `newmoneylog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `othervehicles`
--
ALTER TABLE `othervehicles`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `parliament_sitting`
--
ALTER TABLE `parliament_sitting`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `philanthropists`
--
ALTER TABLE `philanthropists`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones`
--
ALTER TABLE `phones`
  MODIFY `CharacterUuid` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones_bank_transact`
--
ALTER TABLE `phones_bank_transact`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones_callhistory`
--
ALTER TABLE `phones_callhistory`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones_contacts`
--
ALTER TABLE `phones_contacts`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones_msg_accounts`
--
ALTER TABLE `phones_msg_accounts`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones_msg_chats`
--
ALTER TABLE `phones_msg_chats`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones_msg_contacts`
--
ALTER TABLE `phones_msg_contacts`
  MODIFY `ContactId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones_msg_messages`
--
ALTER TABLE `phones_msg_messages`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones_news_advert`
--
ALTER TABLE `phones_news_advert`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones_simcards`
--
ALTER TABLE `phones_simcards`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `phones_taxi_orders`
--
ALTER TABLE `phones_taxi_orders`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `prime`
--
ALTER TABLE `prime`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `prime_errors`
--
ALTER TABLE `prime_errors`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `prime_history`
--
ALTER TABLE `prime_history`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `promos`
--
ALTER TABLE `promos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `propsprices`
--
ALTER TABLE `propsprices`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `questions`
--
ALTER TABLE `questions`
  MODIFY `ID` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `rentcarpoint`
--
ALTER TABLE `rentcarpoint`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `reportmessages`
--
ALTER TABLE `reportmessages`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `reports`
--
ALTER TABLE `reports`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `resetcharacters`
--
ALTER TABLE `resetcharacters`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `roulette_statics`
--
ALTER TABLE `roulette_statics`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `safes`
--
ALTER TABLE `safes`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `stocklog`
--
ALTER TABLE `stocklog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `teleports`
--
ALTER TABLE `teleports`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `ticketlog`
--
ALTER TABLE `ticketlog`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `vehicles`
--
ALTER TABLE `vehicles`
  MODIFY `idkey` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `vehicletrading`
--
ALTER TABLE `vehicletrading`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `weapondamageconfigs`
--
ALTER TABLE `weapondamageconfigs`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `weedfarm`
--
ALTER TABLE `weedfarm`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `whitelist`
--
ALTER TABLE `whitelist`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `workers`
--
ALTER TABLE `workers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `phones`
--
ALTER TABLE `phones`
  ADD CONSTRAINT `FK_Phones_phones_msg_accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `phones_msg_accounts` (`Id`),
  ADD CONSTRAINT `FK_Phones_phones_simcards_SimCardId` FOREIGN KEY (`SimCardId`) REFERENCES `phones_simcards` (`Id`);

--
-- Constraints for table `phones_callhistory`
--
ALTER TABLE `phones_callhistory`
  ADD CONSTRAINT `FK_phones_callhistory_phones_simcards_FromSimCardId` FOREIGN KEY (`FromSimCardId`) REFERENCES `phones_simcards` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `phones_contacts`
--
ALTER TABLE `phones_contacts`
  ADD CONSTRAINT `FK_phones_contacts_phones_simcards_HolderSimCardId` FOREIGN KEY (`HolderSimCardId`) REFERENCES `phones_simcards` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `phones_msg_accounts`
--
ALTER TABLE `phones_msg_accounts`
  ADD CONSTRAINT `FK_phones_msg_accounts_phones_simcards_SimCardId` FOREIGN KEY (`SimCardId`) REFERENCES `phones_simcards` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `phones_msg_accountstochat`
--
ALTER TABLE `phones_msg_accountstochat`
  ADD CONSTRAINT `FK_phones_msg_accountsToChat_phones_msg_accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `phones_msg_accounts` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_phones_msg_accountsToChat_phones_msg_accounts_BlockedById` FOREIGN KEY (`BlockedById`) REFERENCES `phones_msg_accounts` (`Id`),
  ADD CONSTRAINT `FK_phones_msg_accountsToChat_phones_msg_chats_ChatId` FOREIGN KEY (`ChatId`) REFERENCES `phones_msg_chats` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_phones_msg_accountsToChat_phones_msg_messages_LastReadMessag~` FOREIGN KEY (`LastReadMessageId`) REFERENCES `phones_msg_messages` (`Id`);

--
-- Constraints for table `phones_msg_contacts`
--
ALTER TABLE `phones_msg_contacts`
  ADD CONSTRAINT `FK_phones_msg_contacts_phones_msg_accounts_HolderAccountId` FOREIGN KEY (`HolderAccountId`) REFERENCES `phones_msg_accounts` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_phones_msg_contacts_phones_msg_accounts_TargetAccountId` FOREIGN KEY (`TargetAccountId`) REFERENCES `phones_msg_accounts` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `phones_msg_messages`
--
ALTER TABLE `phones_msg_messages`
  ADD CONSTRAINT `FK_phones_msg_messages_phones_msg_accounts_SenderId` FOREIGN KEY (`SenderId`) REFERENCES `phones_msg_accounts` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_phones_msg_messages_phones_msg_chats_ChatId` FOREIGN KEY (`ChatId`) REFERENCES `phones_msg_chats` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

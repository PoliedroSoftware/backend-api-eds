-- phpMyAdmin SQL Dump
-- version 5.2.2
-- https://www.phpmyadmin.net/
--
-- Servidor: mysql:3306
-- Tiempo de generación: 05-07-2025 a las 12:59:19
-- Versión del servidor: 8.0.42
-- Versión de PHP: 8.2.28

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `eds`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `business`
--

CREATE TABLE `business` (
  `id_business` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `context` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `capacity`
--

CREATE TABLE `capacity` (
  `id_capacity` int NOT NULL,
  `code` varchar(45) NOT NULL,
  `height` double NOT NULL,
  `gallon` double NOT NULL,
  `liters` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `category`
--

CREATE TABLE `category` (
  `idcategory` int NOT NULL,
  `description` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `compartiment`
--

CREATE TABLE `compartiment` (
  `id_compartiment` int NOT NULL,
  `number` int NOT NULL,
  `nominal` double NOT NULL,
  `operative` double NOT NULL,
  `stock` double NOT NULL,
  `height` double NOT NULL,
  `id_tank` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `compartiment_capacity`
--

CREATE TABLE `compartiment_capacity` (
  `id_compartiment_capacity` int NOT NULL,
  `id_compartiment` int NOT NULL,
  `id_capacity` int NOT NULL,
  `default` tinyint NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `court`
--

CREATE TABLE `court` (
  `id_court` int NOT NULL,
  `id_islander` int NOT NULL,
  `date_starttime` date NOT NULL,
  `starttime` time NOT NULL,
  `date_endtime` date NOT NULL,
  `endtime` time NOT NULL,
  `consecutive` int NOT NULL,
  `id_eds` int NOT NULL,
  `descripcion` varchar(100) DEFAULT NULL,
  `distintic` double DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `court_dispensers`
--

CREATE TABLE `court_dispensers` (
  `id_court_dispensers` int NOT NULL,
  `id_court` int NOT NULL,
  `accumulated_amount` double NOT NULL,
  `accumulated_gallons` double NOT NULL,
  `id_product` int NOT NULL,
  `id_compartiment` int NOT NULL,
  `id_hose` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

--
-- Disparadores `court_dispensers`
--
DELIMITER $$
CREATE TRIGGER `trg_insert_hose_history` AFTER INSERT ON `court_dispensers` FOR EACH ROW BEGIN
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
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_update_hose_accumulated` AFTER INSERT ON `court_dispensers` FOR EACH ROW BEGIN
    UPDATE hose
    SET 
        accumulated_amount = NEW.accumulated_amount,
        accumulated_gallons = NEW.accumulated_gallons
    WHERE id_hose = NEW.id_hose;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `court_dispensers_inventory`
--

CREATE TABLE `court_dispensers_inventory` (
  `id_court_dispensers_inventorycol` int NOT NULL,
  `id_court_dispensers` int NOT NULL,
  `id_inventory` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `court_document`
--

CREATE TABLE `court_document` (
  `idcourt_document` int NOT NULL,
  `descripcion` text NOT NULL,
  `id_court` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `court_expenditures`
--

CREATE TABLE `court_expenditures` (
  `id_court_expenditure` int NOT NULL,
  `id_court` int NOT NULL,
  `id_expenditures` int NOT NULL,
  `amount` double NOT NULL,
  `decription` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `court_type_of_collection`
--

CREATE TABLE `court_type_of_collection` (
  `id_court_type_of_collection` int NOT NULL,
  `id_court` int NOT NULL,
  `id_type_of_collection` int NOT NULL,
  `amount` double NOT NULL,
  `description` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `dispensers`
--

CREATE TABLE `dispensers` (
  `id_dispensers` int NOT NULL,
  `code` varchar(45) NOT NULL,
  `number` int NOT NULL,
  `id_dispenser_type` int NOT NULL,
  `id_eds` int NOT NULL,
  `idisland` int NOT NULL,
  `number_hose` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `dispenser_type`
--

CREATE TABLE `dispenser_type` (
  `id_dispenser_type` int NOT NULL,
  `description` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `eds`
--

CREATE TABLE `eds` (
  `id_eds` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `nit` varchar(45) NOT NULL,
  `address` varchar(100) NOT NULL,
  `sicom` varchar(45) NOT NULL,
  `idbusiness` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `eds_tank`
--

CREATE TABLE `eds_tank` (
  `id_eds_tank` int NOT NULL,
  `id_eds` int NOT NULL,
  `id_tank` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `expenditures`
--

CREATE TABLE `expenditures` (
  `id_expenditures` int NOT NULL,
  `description` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `hose`
--

CREATE TABLE `hose` (
  `id_hose` int NOT NULL,
  `id_dispensers` int NOT NULL,
  `number` int NOT NULL,
  `accumulated_amount` double NOT NULL,
  `accumulated_gallons` double NOT NULL,
  `id_product_type` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `hose_history`
--

CREATE TABLE `hose_history` (
  `id_hose_hose_history` int NOT NULL,
  `id_hose` int NOT NULL,
  `id_dispensers` int NOT NULL,
  `accumulated_amount` double NOT NULL,
  `accumulated_gallons` double NOT NULL,
  `date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inventory`
--

CREATE TABLE `inventory` (
  `id_inventory` int NOT NULL,
  `date` date NOT NULL,
  `reference_type` varchar(20) DEFAULT NULL,
  `reference_id` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

--
-- Disparadores `inventory`
--
DELIMITER $$
CREATE TRIGGER `trg_insert_court_dispensers_inventory_from_inventory` AFTER INSERT ON `inventory` FOR EACH ROW BEGIN
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
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `island`
--

CREATE TABLE `island` (
  `idisland` int NOT NULL,
  `description` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `islander`
--

CREATE TABLE `islander` (
  `id_islander` int NOT NULL,
  `name` varchar(100) NOT NULL,
  `id_eds` int NOT NULL,
  `password` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `first_name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `product`
--

CREATE TABLE `product` (
  `id_product` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `id_product_type` int NOT NULL,
  `price` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `product_compartiment`
--

CREATE TABLE `product_compartiment` (
  `id_product_compartiment` int NOT NULL,
  `id_product` int NOT NULL,
  `id_compartiment` int NOT NULL,
  `stock` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

--
-- Disparadores `product_compartiment`
--
DELIMITER $$
CREATE TRIGGER `after_delete_product_compartiment` AFTER DELETE ON `product_compartiment` FOR EACH ROW BEGIN
  DECLARE tankId INT;
  -- Obtener el id_tank relacionado con el compartimiento eliminado
  SELECT id_tank INTO tankId FROM compartment WHERE id_compartiment = OLD.id_compartiment;
  UPDATE tank
    SET stock = (
      SELECT IFNULL(SUM(pc.stock),0)
      FROM product_compartiment pc
      JOIN compartment c ON c.id_compartiment = pc.id_compartiment
      WHERE c.id_tank = tank.id_tank
    )
    WHERE id_tank = tankId;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `after_insert_product_compartiment` AFTER INSERT ON `product_compartiment` FOR EACH ROW BEGIN
  DECLARE tankId INT;
  -- Obtener el id_tank relacionado con el nuevo compartimiento
  SELECT id_tank INTO tankId FROM compartment WHERE id_compartiment = NEW.id_compartiment;
  UPDATE tank
    SET stock = (
      SELECT IFNULL(SUM(pc.stock),0)
      FROM product_compartiment pc
      JOIN compartment c ON c.id_compartiment = pc.id_compartiment
      WHERE c.id_tank = tank.id_tank
    )
    WHERE id_tank = tankId;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `after_update_product_compartiment` AFTER UPDATE ON `product_compartiment` FOR EACH ROW BEGIN
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
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `product_type`
--

CREATE TABLE `product_type` (
  `id_product_type` int NOT NULL,
  `description` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `provider`
--

CREATE TABLE `provider` (
  `id_provider` int NOT NULL,
  `name` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `shopping`
--

CREATE TABLE `shopping` (
  `id_shopping` int NOT NULL,
  `invoice` varchar(45) NOT NULL,
  `date` date DEFAULT NULL,
  `id_provider` int NOT NULL,
  `id_category` int NOT NULL,
  `amount` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `shopping_product`
--

CREATE TABLE `shopping_product` (
  `id_shopping_product` int NOT NULL,
  `id_shopping` int NOT NULL,
  `id_product` int NOT NULL,
  `quantity` double NOT NULL,
  `price` double NOT NULL,
  `id_compartiment` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

--
-- Disparadores `shopping_product`
--
DELIMITER $$
CREATE TRIGGER `after_insert_shopping_product` AFTER INSERT ON `shopping_product` FOR EACH ROW BEGIN
  UPDATE product_compartiment
  SET stock = stock + NEW.quantity
  WHERE id_product = NEW.id_product
    AND id_compartiment = NEW.id_compartiment;

  UPDATE compartiment
  SET stock = stock + NEW.quantity
  WHERE id_compartiment = NEW.id_compartiment;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `shopping_produc_inventory`
--

CREATE TABLE `shopping_produc_inventory` (
  `id_shopping_produc_inventorycol` int NOT NULL,
  `id_shopping_product` int NOT NULL,
  `idinventory` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tank`
--

CREATE TABLE `tank` (
  `id_tank` int NOT NULL,
  `number` varchar(45) NOT NULL,
  `ability` double NOT NULL,
  `compartment` int NOT NULL,
  `stock` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `type_of_collection`
--

CREATE TABLE `type_of_collection` (
  `id_type_of_collection` int NOT NULL,
  `description` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_court`
-- (Véase abajo para la vista actual)
--
CREATE TABLE `v_court` (
`bussiness` varchar(45)
,`consecutive` int
,`date_endtime` date
,`date_starttime` date
,`distinc` double
,`eds` varchar(45)
,`endtime` time
,`id` int
,`id_eds` int
,`islander` varchar(100)
,`starttime` time
,`total_accumulated_amount` double
,`total_accumulated_gallons` double
);

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_court_collection`
-- (Véase abajo para la vista actual)
--
CREATE TABLE `v_court_collection` (
`amount` double
,`collection` varchar(45)
,`court` int
,`date` date
,`description` varchar(100)
,`id` int
);

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_court_dispenser`
-- (Véase abajo para la vista actual)
--
CREATE TABLE `v_court_dispenser` (
`accumulated_amount` double
,`accumulated_gallons` double
,`business` varchar(45)
,`code_court` int
,`date_endtime` date
,`date_starttime` date
,`dispenser` int
,`distinc` double
,`eds` varchar(45)
,`endtime` time
,`id` int
,`id_eds` int
,`islander` varchar(100)
,`last_accumulated_amount` double
,`last_accumulated_gallons` double
,`number_hose` int
,`price` double
,`product` varchar(45)
,`product_typr` varchar(45)
,`starttime` time
);

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_court_document`
-- (Véase abajo para la vista actual)
--
CREATE TABLE `v_court_document` (
`court` int
,`descripcion` text
,`id` int
);

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_court_expenditure`
-- (Véase abajo para la vista actual)
--
CREATE TABLE `v_court_expenditure` (
`amount` double
,`court` int
,`date` date
,`description` varchar(100)
,`expenditure` varchar(45)
,`id` int
);

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_hose`
-- (Véase abajo para la vista actual)
--
CREATE TABLE `v_hose` (
`accumulated_amount` double
,`accumulated_gallons` double
,`id` int
,`number_hose` int
,`price` double
,`product_type` varchar(45)
);

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_hose_history`
-- (Véase abajo para la vista actual)
--
CREATE TABLE `v_hose_history` (
`accumulated_amount` double
,`accumulated_gallons` double
,`date` date
,`dispenser_number` int
,`eds` varchar(45)
,`id` int
,`id_dispenser` int
,`id_eds` int
,`id_hose` int
,`numer_hose` int
);

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_inventory`
-- (Véase abajo para la vista actual)
--
CREATE TABLE `v_inventory` (
`business` varchar(45)
,`compartment` int
,`eds` varchar(45)
,`id_business` int
,`id_compartment` int
,`id_eds` int
,`id_product` int
,`id_tank` int
,`product` varchar(45)
,`stock` double
,`tank` varchar(45)
,`tank_capacity` double
);

-- --------------------------------------------------------

--
-- Estructura Stand-in para la vista `v_shopping_product`
-- (Véase abajo para la vista actual)
--
CREATE TABLE `v_shopping_product` (
`id_compartiment` int
,`id_product` int
,`id_shopping` int
,`id_shopping_product` int
,`nombre_producto` varchar(45)
,`price` double
,`quantity` double
,`total_precio` double
);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `business`
--
ALTER TABLE `business`
  ADD PRIMARY KEY (`id_business`);

--
-- Indices de la tabla `capacity`
--
ALTER TABLE `capacity`
  ADD PRIMARY KEY (`id_capacity`);

--
-- Indices de la tabla `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`idcategory`);

--
-- Indices de la tabla `compartiment`
--
ALTER TABLE `compartiment`
  ADD PRIMARY KEY (`id_compartiment`),
  ADD KEY `fk_compartiment_tank1_idx` (`id_tank`) USING BTREE;

--
-- Indices de la tabla `compartiment_capacity`
--
ALTER TABLE `compartiment_capacity`
  ADD PRIMARY KEY (`id_compartiment_capacity`),
  ADD KEY `fk_compartiment_capacity_compartiment1` (`id_compartiment`),
  ADD KEY `fk_compartiment_capacity_capacity1` (`id_capacity`);

--
-- Indices de la tabla `court`
--
ALTER TABLE `court`
  ADD PRIMARY KEY (`id_court`),
  ADD KEY `fk_court_islander_idx` (`id_islander`),
  ADD KEY `fk_court_eds1_idx` (`id_eds`);

--
-- Indices de la tabla `court_dispensers`
--
ALTER TABLE `court_dispensers`
  ADD PRIMARY KEY (`id_court_dispensers`),
  ADD KEY `fk_court_has_dispensers_court1_idx` (`id_court`),
  ADD KEY `fk_court_dispensers_product1_idx` (`id_product`),
  ADD KEY `fk_court_dispensers_compartiment1_idx` (`id_compartiment`),
  ADD KEY `fk_court_dispensers_hose1_idx` (`id_hose`);

--
-- Indices de la tabla `court_dispensers_inventory`
--
ALTER TABLE `court_dispensers_inventory`
  ADD PRIMARY KEY (`id_court_dispensers_inventorycol`),
  ADD KEY `fk_court_dispensers_has_inventory_inventory1_idx` (`id_inventory`),
  ADD KEY `fk_court_dispensers_has_inventory_court_dispensers1_idx` (`id_court_dispensers`);

--
-- Indices de la tabla `court_document`
--
ALTER TABLE `court_document`
  ADD PRIMARY KEY (`idcourt_document`),
  ADD KEY `fk_court_document_court1_idx` (`id_court`);

--
-- Indices de la tabla `court_expenditures`
--
ALTER TABLE `court_expenditures`
  ADD PRIMARY KEY (`id_court_expenditure`),
  ADD KEY `fk_court_has_expenditures_expenditures1_idx` (`id_expenditures`),
  ADD KEY `fk_court_has_expenditures_court1_idx` (`id_court`);

--
-- Indices de la tabla `court_type_of_collection`
--
ALTER TABLE `court_type_of_collection`
  ADD PRIMARY KEY (`id_court_type_of_collection`),
  ADD KEY `fk_court_has_type_of_collection_type_of_collection1_idx` (`id_type_of_collection`),
  ADD KEY `fk_court_has_type_of_collection_court1_idx` (`id_court`);

--
-- Indices de la tabla `dispensers`
--
ALTER TABLE `dispensers`
  ADD PRIMARY KEY (`id_dispensers`),
  ADD KEY `fk_dispensers_dispenser_type1_idx` (`id_dispenser_type`),
  ADD KEY `fk_dispensers_eds1_idx` (`id_eds`),
  ADD KEY `fk_dispensers_island1_idx` (`idisland`);

--
-- Indices de la tabla `dispenser_type`
--
ALTER TABLE `dispenser_type`
  ADD PRIMARY KEY (`id_dispenser_type`);

--
-- Indices de la tabla `eds`
--
ALTER TABLE `eds`
  ADD PRIMARY KEY (`id_eds`),
  ADD KEY `fk_eds_business1_idx` (`idbusiness`);

--
-- Indices de la tabla `eds_tank`
--
ALTER TABLE `eds_tank`
  ADD PRIMARY KEY (`id_eds_tank`),
  ADD KEY `fk_eds_has_tank_tank1_idx` (`id_tank`),
  ADD KEY `fk_eds_has_tank_eds1_idx` (`id_eds`);

--
-- Indices de la tabla `expenditures`
--
ALTER TABLE `expenditures`
  ADD PRIMARY KEY (`id_expenditures`);

--
-- Indices de la tabla `hose`
--
ALTER TABLE `hose`
  ADD PRIMARY KEY (`id_hose`),
  ADD KEY `fk_house_dispensers1_idx` (`id_dispensers`),
  ADD KEY `fk_hose_product_type1_idx` (`id_product_type`);

--
-- Indices de la tabla `hose_history`
--
ALTER TABLE `hose_history`
  ADD PRIMARY KEY (`id_hose_hose_history`),
  ADD KEY `fk_hose_has_hose_history_hose1_idx` (`id_hose`),
  ADD KEY `fk_hose_history_dispensers1_idx` (`id_dispensers`);

--
-- Indices de la tabla `inventory`
--
ALTER TABLE `inventory`
  ADD PRIMARY KEY (`id_inventory`);

--
-- Indices de la tabla `island`
--
ALTER TABLE `island`
  ADD PRIMARY KEY (`idisland`);

--
-- Indices de la tabla `islander`
--
ALTER TABLE `islander`
  ADD PRIMARY KEY (`id_islander`),
  ADD KEY `fk_islander_eds1_idx` (`id_eds`);

--
-- Indices de la tabla `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`id_product`),
  ADD KEY `fk_product_product_type1_idx` (`id_product_type`);

--
-- Indices de la tabla `product_compartiment`
--
ALTER TABLE `product_compartiment`
  ADD PRIMARY KEY (`id_product_compartiment`),
  ADD KEY `fk_product_has_compartiment_compartiment1_idx` (`id_compartiment`),
  ADD KEY `fk_product_has_compartiment_product1_idx` (`id_product`);

--
-- Indices de la tabla `product_type`
--
ALTER TABLE `product_type`
  ADD PRIMARY KEY (`id_product_type`);

--
-- Indices de la tabla `provider`
--
ALTER TABLE `provider`
  ADD PRIMARY KEY (`id_provider`);

--
-- Indices de la tabla `shopping`
--
ALTER TABLE `shopping`
  ADD PRIMARY KEY (`id_shopping`,`amount`),
  ADD KEY `fk_shopping_provider1_idx` (`id_provider`),
  ADD KEY `fk_shopping_category1_idx` (`id_category`);

--
-- Indices de la tabla `shopping_product`
--
ALTER TABLE `shopping_product`
  ADD PRIMARY KEY (`id_shopping_product`),
  ADD KEY `fk_shopping_has_product_product1_idx` (`id_product`),
  ADD KEY `fk_shopping_has_product_shopping1_idx` (`id_shopping`),
  ADD KEY `fk_shopping_product_compartiment1_idx` (`id_compartiment`);

--
-- Indices de la tabla `shopping_produc_inventory`
--
ALTER TABLE `shopping_produc_inventory`
  ADD PRIMARY KEY (`id_shopping_produc_inventorycol`),
  ADD KEY `fk_shopping_product_has_inventory_inventory1_idx` (`idinventory`),
  ADD KEY `fk_shopping_product_has_inventory_shopping_product1_idx` (`id_shopping_product`);

--
-- Indices de la tabla `tank`
--
ALTER TABLE `tank`
  ADD PRIMARY KEY (`id_tank`);

--
-- Indices de la tabla `type_of_collection`
--
ALTER TABLE `type_of_collection`
  ADD PRIMARY KEY (`id_type_of_collection`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `business`
--
ALTER TABLE `business`
  MODIFY `id_business` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `capacity`
--
ALTER TABLE `capacity`
  MODIFY `id_capacity` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `category`
--
ALTER TABLE `category`
  MODIFY `idcategory` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `compartiment`
--
ALTER TABLE `compartiment`
  MODIFY `id_compartiment` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `court`
--
ALTER TABLE `court`
  MODIFY `id_court` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `court_dispensers`
--
ALTER TABLE `court_dispensers`
  MODIFY `id_court_dispensers` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `court_dispensers_inventory`
--
ALTER TABLE `court_dispensers_inventory`
  MODIFY `id_court_dispensers_inventorycol` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `court_document`
--
ALTER TABLE `court_document`
  MODIFY `idcourt_document` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `court_expenditures`
--
ALTER TABLE `court_expenditures`
  MODIFY `id_court_expenditure` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `court_type_of_collection`
--
ALTER TABLE `court_type_of_collection`
  MODIFY `id_court_type_of_collection` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `dispensers`
--
ALTER TABLE `dispensers`
  MODIFY `id_dispensers` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `dispenser_type`
--
ALTER TABLE `dispenser_type`
  MODIFY `id_dispenser_type` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `eds`
--
ALTER TABLE `eds`
  MODIFY `id_eds` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `eds_tank`
--
ALTER TABLE `eds_tank`
  MODIFY `id_eds_tank` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `expenditures`
--
ALTER TABLE `expenditures`
  MODIFY `id_expenditures` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `hose`
--
ALTER TABLE `hose`
  MODIFY `id_hose` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `hose_history`
--
ALTER TABLE `hose_history`
  MODIFY `id_hose_hose_history` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inventory`
--
ALTER TABLE `inventory`
  MODIFY `id_inventory` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `island`
--
ALTER TABLE `island`
  MODIFY `idisland` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `islander`
--
ALTER TABLE `islander`
  MODIFY `id_islander` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `product`
--
ALTER TABLE `product`
  MODIFY `id_product` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `product_compartiment`
--
ALTER TABLE `product_compartiment`
  MODIFY `id_product_compartiment` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `product_type`
--
ALTER TABLE `product_type`
  MODIFY `id_product_type` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `provider`
--
ALTER TABLE `provider`
  MODIFY `id_provider` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `shopping`
--
ALTER TABLE `shopping`
  MODIFY `id_shopping` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `shopping_product`
--
ALTER TABLE `shopping_product`
  MODIFY `id_shopping_product` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `shopping_produc_inventory`
--
ALTER TABLE `shopping_produc_inventory`
  MODIFY `id_shopping_produc_inventorycol` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `tank`
--
ALTER TABLE `tank`
  MODIFY `id_tank` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `type_of_collection`
--
ALTER TABLE `type_of_collection`
  MODIFY `id_type_of_collection` int NOT NULL AUTO_INCREMENT;

-- --------------------------------------------------------

--
-- Estructura para la vista `v_court`
--
DROP TABLE IF EXISTS `v_court`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `v_court`  AS SELECT `c`.`id_court` AS `id`, `c`.`consecutive` AS `consecutive`, `e`.`id_eds` AS `id_eds`, `e`.`name` AS `eds`, `b`.`name` AS `bussiness`, `i`.`name` AS `islander`, `c`.`date_starttime` AS `date_starttime`, `c`.`starttime` AS `starttime`, `c`.`date_endtime` AS `date_endtime`, `c`.`endtime` AS `endtime`, `c`.`distintic` AS `distinc`, `ca`.`total_accumulated_amount` AS `total_accumulated_amount`, `cg`.`total_accumulated_gallons` AS `total_accumulated_gallons` FROM (((((`court` `c` join `islander` `i` on((`i`.`id_islander` = `c`.`id_islander`))) join `eds` `e` on((`e`.`id_eds` = `c`.`id_eds`))) left join (select `court_dispensers`.`id_court` AS `id_court`,sum(`court_dispensers`.`accumulated_amount`) AS `total_accumulated_amount` from `court_dispensers` group by `court_dispensers`.`id_court`) `ca` on((`ca`.`id_court` = `c`.`id_court`))) left join (select `court_dispensers`.`id_court` AS `id_court`,sum(`court_dispensers`.`accumulated_gallons`) AS `total_accumulated_gallons` from `court_dispensers` group by `court_dispensers`.`id_court`) `cg` on((`cg`.`id_court` = `c`.`id_court`))) join `business` `b` on((`b`.`id_business` = `e`.`idbusiness`))) ;

-- --------------------------------------------------------

--
-- Estructura para la vista `v_court_collection`
--
DROP TABLE IF EXISTS `v_court_collection`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `v_court_collection`  AS SELECT `cc`.`id_court_type_of_collection` AS `id`, `c`.`id_court` AS `court`, `c`.`date_starttime` AS `date`, `tc`.`description` AS `collection`, `cc`.`amount` AS `amount`, `cc`.`description` AS `description` FROM ((`court_type_of_collection` `cc` left join `court` `c` on((`c`.`id_court` = `cc`.`id_court`))) left join `type_of_collection` `tc` on((`tc`.`id_type_of_collection` = `cc`.`id_type_of_collection`))) ORDER BY `c`.`id_court` DESC ;

-- --------------------------------------------------------

--
-- Estructura para la vista `v_court_dispenser`
--
DROP TABLE IF EXISTS `v_court_dispenser`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `v_court_dispenser`  AS SELECT `cd`.`id_court_dispensers` AS `id`, `b`.`name` AS `business`, `e`.`id_eds` AS `id_eds`, `e`.`name` AS `eds`, `d`.`number` AS `dispenser`, `h`.`number` AS `number_hose`, `h`.`accumulated_amount` AS `last_accumulated_amount`, `h`.`accumulated_gallons` AS `last_accumulated_gallons`, `c`.`id_court` AS `code_court`, `i`.`name` AS `islander`, `c`.`date_starttime` AS `date_starttime`, `c`.`starttime` AS `starttime`, `c`.`date_endtime` AS `date_endtime`, `c`.`endtime` AS `endtime`, `c`.`distintic` AS `distinc`, `p`.`name` AS `product`, `p`.`price` AS `price`, `pt`.`description` AS `product_typr`, `cd`.`accumulated_amount` AS `accumulated_amount`, `cd`.`accumulated_gallons` AS `accumulated_gallons` FROM ((((((((`court_dispensers` `cd` left join `court` `c` on((`c`.`id_court` = `cd`.`id_court`))) left join `product` `p` on((`p`.`id_product` = `cd`.`id_product`))) left join `hose` `h` on((`h`.`id_hose` = `cd`.`id_hose`))) left join `dispensers` `d` on((`d`.`id_dispensers` = `h`.`id_dispensers`))) left join `product_type` `pt` on((`p`.`id_product_type` = `pt`.`id_product_type`))) left join `eds` `e` on((`e`.`id_eds` = `d`.`id_eds`))) left join `business` `b` on((`b`.`id_business` = `e`.`idbusiness`))) left join `islander` `i` on((`i`.`id_islander` = `c`.`id_islander`))) ORDER BY `c`.`id_court` DESC ;

-- --------------------------------------------------------

--
-- Estructura para la vista `v_court_document`
--
DROP TABLE IF EXISTS `v_court_document`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `v_court_document`  AS SELECT `cd`.`idcourt_document` AS `id`, `c`.`id_court` AS `court`, `cd`.`descripcion` AS `descripcion` FROM (`court_document` `cd` left join `court` `c` on((`c`.`id_court` = `cd`.`id_court`))) ORDER BY `c`.`id_court` DESC ;

-- --------------------------------------------------------

--
-- Estructura para la vista `v_court_expenditure`
--
DROP TABLE IF EXISTS `v_court_expenditure`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `v_court_expenditure`  AS SELECT `ce`.`id_court_expenditure` AS `id`, `c`.`id_court` AS `court`, `c`.`date_starttime` AS `date`, `e`.`description` AS `expenditure`, `ce`.`amount` AS `amount`, `ce`.`decription` AS `description` FROM ((`court_expenditures` `ce` left join `court` `c` on((`c`.`id_court` = `ce`.`id_court`))) left join `expenditures` `e` on((`e`.`id_expenditures` = `ce`.`id_expenditures`))) ORDER BY `c`.`id_court` DESC ;

-- --------------------------------------------------------

--
-- Estructura para la vista `v_hose`
--
DROP TABLE IF EXISTS `v_hose`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `v_hose`  AS SELECT `h`.`id_hose` AS `id`, `h`.`number` AS `number_hose`, `h`.`accumulated_amount` AS `accumulated_amount`, `h`.`accumulated_gallons` AS `accumulated_gallons`, `pt`.`description` AS `product_type`, `p`.`price` AS `price` FROM (((`hose` `h` left join `dispensers` `d` on((`d`.`id_dispensers` = `h`.`id_dispensers`))) left join `product_type` `pt` on((`pt`.`id_product_type` = `h`.`id_product_type`))) left join `product` `p` on((`pt`.`id_product_type` = `p`.`id_product_type`))) ;

-- --------------------------------------------------------

--
-- Estructura para la vista `v_hose_history`
--
DROP TABLE IF EXISTS `v_hose_history`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `v_hose_history`  AS SELECT `hh`.`id_hose_hose_history` AS `id`, `h`.`id_hose` AS `id_hose`, `h`.`number` AS `numer_hose`, `d`.`id_dispensers` AS `id_dispenser`, `d`.`number` AS `dispenser_number`, `e`.`id_eds` AS `id_eds`, `e`.`name` AS `eds`, `hh`.`accumulated_amount` AS `accumulated_amount`, `hh`.`accumulated_gallons` AS `accumulated_gallons`, `hh`.`date` AS `date` FROM (((`hose_history` `hh` left join `dispensers` `d` on((`d`.`id_dispenser_type` = `hh`.`id_dispensers`))) left join `hose` `h` on((`h`.`id_hose` = `hh`.`id_hose`))) left join `eds` `e` on((`e`.`id_eds` = `d`.`id_eds`))) ORDER BY `hh`.`date` DESC ;

-- --------------------------------------------------------

--
-- Estructura para la vista `v_inventory`
--
DROP TABLE IF EXISTS `v_inventory`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `v_inventory`  AS SELECT `business`.`id_business` AS `id_business`, `business`.`name` AS `business`, `eds`.`id_eds` AS `id_eds`, `eds`.`name` AS `eds`, `tank`.`id_tank` AS `id_tank`, `tank`.`number` AS `tank`, `tank`.`ability` AS `tank_capacity`, `compartiment`.`id_compartiment` AS `id_compartment`, `compartiment`.`number` AS `compartment`, `product`.`id_product` AS `id_product`, `product`.`name` AS `product`, `compartiment`.`stock` AS `stock`from ((((((`eds`.`business`join `eds`.`eds` on ((`eds`.`business`.`id_business` = `eds`.`eds`.`idbusiness`)))join `eds`.`eds_tank` on ((`eds`.`eds`.`id_eds` = `eds`.`eds_tank`.`id_eds`)))join `eds`.`tank` on ((`eds`.`eds_tank`.`id_tank` = `eds`.`tank`.`id_tank`)))join `eds`.`compartiment` on ((`eds`.`tank`.`id_tank` = `eds`.`compartiment`.`id_tank`)))join `eds`.`product_compartiment` on ((`eds`.`compartiment`.`id_compartiment` = `eds`.`product_compartiment`.`id_compartiment`)))join `eds`.`product` on ((`eds`.`product_compartiment`.`id_product` = `eds`.`product`.`id_product`)));

-- --------------------------------------------------------

--
-- Estructura para la vista `v_shopping_product`
--
DROP TABLE IF EXISTS `v_shopping_product`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `v_shopping_product`  AS SELECT `sp`.`id_shopping_product` AS `id_shopping_product`, `sp`.`id_shopping` AS `id_shopping`, `sp`.`id_product` AS `id_product`, `p`.`name` AS `nombre_producto`, `sp`.`quantity` AS `quantity`, `sp`.`price` AS `price`, `sp`.`id_compartiment` AS `id_compartiment`, (`sp`.`quantity` * `sp`.`price`) AS `total_precio` FROM (`shopping_product` `sp` join `product` `p` on((`sp`.`id_product` = `p`.`id_product`))) ;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `compartiment`
--
ALTER TABLE `compartiment`
  ADD CONSTRAINT `fk_compartiment_tank1` FOREIGN KEY (`id_tank`) REFERENCES `tank` (`id_tank`);

--
-- Filtros para la tabla `compartiment_capacity`
--
ALTER TABLE `compartiment_capacity`
  ADD CONSTRAINT `fk_compartiment_capacity_capacity1` FOREIGN KEY (`id_capacity`) REFERENCES `capacity` (`id_capacity`),
  ADD CONSTRAINT `fk_compartiment_capacity_compartiment1` FOREIGN KEY (`id_compartiment`) REFERENCES `compartiment` (`id_compartiment`);

--
-- Filtros para la tabla `court`
--
ALTER TABLE `court`
  ADD CONSTRAINT `fk_court_eds1` FOREIGN KEY (`id_eds`) REFERENCES `eds` (`id_eds`),
  ADD CONSTRAINT `fk_court_islander` FOREIGN KEY (`id_islander`) REFERENCES `islander` (`id_islander`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `court_dispensers`
--
ALTER TABLE `court_dispensers`
  ADD CONSTRAINT `fk_court_dispensers_compartiment1` FOREIGN KEY (`id_compartiment`) REFERENCES `compartiment` (`id_compartiment`),
  ADD CONSTRAINT `fk_court_dispensers_hose1` FOREIGN KEY (`id_hose`) REFERENCES `hose` (`id_hose`),
  ADD CONSTRAINT `fk_court_dispensers_product1` FOREIGN KEY (`id_product`) REFERENCES `product` (`id_product`),
  ADD CONSTRAINT `fk_court_has_dispensers_court1` FOREIGN KEY (`id_court`) REFERENCES `court` (`id_court`);

--
-- Filtros para la tabla `court_dispensers_inventory`
--
ALTER TABLE `court_dispensers_inventory`
  ADD CONSTRAINT `fk_court_dispensers_has_inventory_court_dispensers1` FOREIGN KEY (`id_court_dispensers`) REFERENCES `court_dispensers` (`id_court_dispensers`),
  ADD CONSTRAINT `fk_court_dispensers_has_inventory_inventory1` FOREIGN KEY (`id_inventory`) REFERENCES `inventory` (`id_inventory`);

--
-- Filtros para la tabla `court_document`
--
ALTER TABLE `court_document`
  ADD CONSTRAINT `fk_court_document_court1` FOREIGN KEY (`id_court`) REFERENCES `court` (`id_court`);

--
-- Filtros para la tabla `court_expenditures`
--
ALTER TABLE `court_expenditures`
  ADD CONSTRAINT `fk_court_has_expenditures_court1` FOREIGN KEY (`id_court`) REFERENCES `court` (`id_court`),
  ADD CONSTRAINT `fk_court_has_expenditures_expenditures1` FOREIGN KEY (`id_expenditures`) REFERENCES `expenditures` (`id_expenditures`);

--
-- Filtros para la tabla `court_type_of_collection`
--
ALTER TABLE `court_type_of_collection`
  ADD CONSTRAINT `fk_court_has_type_of_collection_court1` FOREIGN KEY (`id_court`) REFERENCES `court` (`id_court`),
  ADD CONSTRAINT `fk_court_has_type_of_collection_type_of_collection1` FOREIGN KEY (`id_type_of_collection`) REFERENCES `type_of_collection` (`id_type_of_collection`);

--
-- Filtros para la tabla `dispensers`
--
ALTER TABLE `dispensers`
  ADD CONSTRAINT `fk_dispensers_dispenser_type1` FOREIGN KEY (`id_dispenser_type`) REFERENCES `dispenser_type` (`id_dispenser_type`),
  ADD CONSTRAINT `fk_dispensers_eds1` FOREIGN KEY (`id_eds`) REFERENCES `eds` (`id_eds`),
  ADD CONSTRAINT `fk_dispensers_island1` FOREIGN KEY (`idisland`) REFERENCES `island` (`idisland`);

--
-- Filtros para la tabla `eds`
--
ALTER TABLE `eds`
  ADD CONSTRAINT `fk_eds_business1` FOREIGN KEY (`idbusiness`) REFERENCES `business` (`id_business`);

--
-- Filtros para la tabla `eds_tank`
--
ALTER TABLE `eds_tank`
  ADD CONSTRAINT `fk_eds_has_tank_eds1` FOREIGN KEY (`id_eds`) REFERENCES `eds` (`id_eds`),
  ADD CONSTRAINT `fk_eds_has_tank_tank1` FOREIGN KEY (`id_tank`) REFERENCES `tank` (`id_tank`);

--
-- Filtros para la tabla `hose`
--
ALTER TABLE `hose`
  ADD CONSTRAINT `fk_hose_product_type1` FOREIGN KEY (`id_product_type`) REFERENCES `product_type` (`id_product_type`),
  ADD CONSTRAINT `fk_house_dispensers1` FOREIGN KEY (`id_dispensers`) REFERENCES `dispensers` (`id_dispensers`);

--
-- Filtros para la tabla `hose_history`
--
ALTER TABLE `hose_history`
  ADD CONSTRAINT `fk_hose_has_hose_history_hose1` FOREIGN KEY (`id_hose`) REFERENCES `hose` (`id_hose`),
  ADD CONSTRAINT `fk_hose_history_dispensers1` FOREIGN KEY (`id_dispensers`) REFERENCES `dispensers` (`id_dispensers`);

--
-- Filtros para la tabla `islander`
--
ALTER TABLE `islander`
  ADD CONSTRAINT `fk_islander_eds1` FOREIGN KEY (`id_eds`) REFERENCES `eds` (`id_eds`);

--
-- Filtros para la tabla `product`
--
ALTER TABLE `product`
  ADD CONSTRAINT `fk_product_product_type1` FOREIGN KEY (`id_product_type`) REFERENCES `product_type` (`id_product_type`);

--
-- Filtros para la tabla `product_compartiment`
--
ALTER TABLE `product_compartiment`
  ADD CONSTRAINT `fk_product_has_compartiment_compartiment1` FOREIGN KEY (`id_compartiment`) REFERENCES `compartiment` (`id_compartiment`),
  ADD CONSTRAINT `fk_product_has_compartiment_product1` FOREIGN KEY (`id_product`) REFERENCES `product` (`id_product`);

--
-- Filtros para la tabla `shopping`
--
ALTER TABLE `shopping`
  ADD CONSTRAINT `fk_shopping_category1` FOREIGN KEY (`id_category`) REFERENCES `category` (`idcategory`),
  ADD CONSTRAINT `fk_shopping_provider1` FOREIGN KEY (`id_provider`) REFERENCES `provider` (`id_provider`);

--
-- Filtros para la tabla `shopping_product`
--
ALTER TABLE `shopping_product`
  ADD CONSTRAINT `fk_shopping_has_product_product1` FOREIGN KEY (`id_product`) REFERENCES `product` (`id_product`),
  ADD CONSTRAINT `fk_shopping_has_product_shopping1` FOREIGN KEY (`id_shopping`) REFERENCES `shopping` (`id_shopping`),
  ADD CONSTRAINT `fk_shopping_product_compartiment1` FOREIGN KEY (`id_compartiment`) REFERENCES `compartiment` (`id_compartiment`);

--
-- Filtros para la tabla `shopping_produc_inventory`
--
ALTER TABLE `shopping_produc_inventory`
  ADD CONSTRAINT `fk_shopping_product_has_inventory_inventory1` FOREIGN KEY (`idinventory`) REFERENCES `inventory` (`id_inventory`),
  ADD CONSTRAINT `fk_shopping_product_has_inventory_shopping_product1` FOREIGN KEY (`id_shopping_product`) REFERENCES `shopping_product` (`id_shopping_product`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

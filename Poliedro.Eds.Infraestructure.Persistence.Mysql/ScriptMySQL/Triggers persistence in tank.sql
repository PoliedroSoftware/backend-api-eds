DELIMITER $$
CREATE TRIGGER after_update_product_compartiment
AFTER UPDATE ON product_compartiment
FOR EACH ROW
BEGIN
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
END $$
DELIMITER ;


DROP TRIGGER IF EXISTS after_insert_product_compartiment;
DELIMITER $$
CREATE TRIGGER after_insert_product_compartiment
AFTER INSERT ON product_compartiment
FOR EACH ROW
BEGIN
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
END $$
DELIMITER ;



DROP TRIGGER IF EXISTS after_delete_product_compartiment;
DELIMITER $$
CREATE TRIGGER after_delete_product_compartiment
AFTER DELETE ON product_compartiment
FOR EACH ROW
BEGIN
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
END $$
DELIMITER ;


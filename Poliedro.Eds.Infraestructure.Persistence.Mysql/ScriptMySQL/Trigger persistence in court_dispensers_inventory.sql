DROP TRIGGER IF EXISTS trg_insert_court_dispensers_inventory_from_inventory;
DELIMITER $$

CREATE TRIGGER trg_insert_court_dispensers_inventory_from_inventory
AFTER INSERT ON inventory
FOR EACH ROW
BEGIN
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
END$$

DELIMITER ;
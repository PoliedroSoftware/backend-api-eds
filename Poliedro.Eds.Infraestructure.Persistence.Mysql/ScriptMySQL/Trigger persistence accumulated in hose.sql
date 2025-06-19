DROP TRIGGER IF EXISTS trg_update_hose_accumulated;
DELIMITER $$

CREATE TRIGGER trg_update_hose_accumulated
AFTER INSERT ON court_dispensers
FOR EACH ROW
BEGIN
    UPDATE hose
    SET 
        accumulated_amount = NEW.accumulated_amount,
        accumulated_gallons = NEW.accumulated_gallons
    WHERE id_hose = NEW.id_hose;
END$$

DELIMITER ;

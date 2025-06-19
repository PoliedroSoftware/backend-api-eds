DROP TRIGGER IF EXISTS trg_insert_hose_history;
DELIMITER $$

CREATE TRIGGER trg_insert_hose_history
AFTER INSERT ON court_dispensers
FOR EACH ROW
BEGIN
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
END$$

DELIMITER ;

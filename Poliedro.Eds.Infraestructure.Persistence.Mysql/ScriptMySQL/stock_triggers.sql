DELIMITER $$
DROP TRIGGER after_delete_product_compartiment $$
DROP TRIGGER after_insert_product_compartiment $$
DROP TRIGGER after_update_product_compartiment $$


CREATE TRIGGER trg_product_compartiment_after_insert_update_stock_consistency
AFTER INSERT ON product_compartiment
FOR EACH ROW
BEGIN
    -- 1. Actualizar el stock en la tabla 'compartiment'
    UPDATE compartiment
    SET stock = NEW.stock
    WHERE id_compartiment = NEW.id_compartiment;

    -- 2. Actualizar el stock en la tabla 'tank'
    UPDATE tank
    SET stock = (
        SELECT SUM(c.stock)
        FROM compartiment c
        WHERE c.id_tank = (
            SELECT cmp.id_tank
            FROM compartiment cmp
            WHERE cmp.id_compartiment = NEW.id_compartiment
        )
    )
    WHERE id_tank = (
        SELECT cmp.id_tank
        FROM compartiment cmp
        WHERE cmp.id_compartiment = NEW.id_compartiment
    );
END$$


CREATE TRIGGER trg_product_compartiment_after_update_update_stock_consistency
AFTER UPDATE ON product_compartiment
FOR EACH ROW
BEGIN
    -- Solo ejecutar si el stock realmente ha cambiado
    IF NEW.stock <> OLD.stock THEN
        -- 1. Actualizar el stock en la tabla 'compartiment'
        UPDATE compartiment
        SET stock = NEW.stock
        WHERE id_compartiment = NEW.id_compartiment;

        -- 2. Actualizar el stock en la tabla 'tank'
        UPDATE tank
        SET stock = (
            SELECT SUM(c.stock)
            FROM compartiment c
            WHERE c.id_tank = (
                SELECT cmp.id_tank
                FROM compartiment cmp
                WHERE cmp.id_compartiment = NEW.id_compartiment
            )
        )
        WHERE id_tank = (
            SELECT cmp.id_tank
            FROM compartiment cmp
            WHERE cmp.id_compartiment = NEW.id_compartiment
        );
    END IF;
END$$


CREATE TRIGGER trg_product_compartiment_after_delete_update_stock_consistency
AFTER DELETE ON product_compartiment
FOR EACH ROW
BEGIN
    -- 1. Setear el stock del compartimento a 0
    UPDATE compartiment
    SET stock = 0
    WHERE id_compartiment = OLD.id_compartiment;

    -- 2. Actualizar el stock en la tabla 'tank'
    UPDATE tank
    SET stock = (
        SELECT SUM(c.stock)
        FROM compartiment c
        WHERE c.id_tank = (
            SELECT cmp.id_tank
            FROM compartiment cmp
            WHERE cmp.id_compartiment = OLD.id_compartiment
        )
    )
    WHERE id_tank = (
        SELECT cmp.id_tank
        FROM compartiment cmp
        WHERE cmp.id_compartiment = OLD.id_compartiment
    );
END$$
DELIMITER ;
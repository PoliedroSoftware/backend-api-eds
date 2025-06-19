DROP TRIGGER IF EXISTS after_insert_shopping_product;
DELIMITER $$
CREATE TRIGGER after_insert_shopping_product
AFTER INSERT ON shopping_product
FOR EACH ROW
BEGIN
  UPDATE product_compartiment
  SET stock = stock + NEW.quantity
  WHERE id_product = NEW.id_product
    AND id_compartiment = NEW.id_compartiment;

  UPDATE compartiment
  SET stock = stock + NEW.quantity
  WHERE id_compartiment = NEW.id_compartiment;
END $$
DELIMITER ;

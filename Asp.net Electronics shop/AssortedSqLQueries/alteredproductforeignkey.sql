﻿ALTER TABLE Product
ADD FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID);
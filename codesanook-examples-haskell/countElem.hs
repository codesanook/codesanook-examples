countElem :: [a] -> String
countElem [] = "Empty"
countElem (_:[]) = "one"
countElem (_:_:[]) = "two"
countElem _ = "Many"
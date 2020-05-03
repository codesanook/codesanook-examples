

length' :: [a] -> int length' [] = 0 length' (_:xs) = 1 + length' xs 

reverse' :: [a -]
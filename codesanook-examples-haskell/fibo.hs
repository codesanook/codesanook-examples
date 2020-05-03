fibo :: Int -> Int
fibo 0 = 0
fibo 1 = 1
fibo 2 =  fibo 1  + fibo 0
fibo n =  fibo (n-2) + fibo (n-1)
{-
0, 0 f(0)
1, 1 f(1)
2, 1 f(2)
3, 2
4, 3
5, 5
6, 8
7, 13
-}

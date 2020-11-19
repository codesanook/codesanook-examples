#load "BallModel.fs"
open Codesanook.Examples.LearnFSharp

type Result = 
  | Success                // no string needed for success state
  | ErrorMessage of string // error message needed

let result = Success
let erroResult= ErrorMessage


let genericBallTest name x y shouldBounceX shouldBounceY = 
    let ball = Ball.New(x, y, 1., 1.)

    let ball' = ball.Move(0., 0., 100., 100.)
    let xOp = if shouldBounceX then (-) else (+)
    let yOp = if shouldBounceY then (-) else (+)

    if xOp ball.X ball.XVel <> ball'.X then 
        failwith "X value not updated correctly"

    if yOp ball.Y ball.YVel <> ball'.Y then 
        failwith "Y value not updated correctly"

    printfn "passed - %s ball`.X %f ball`.Y %f" name ball'.X ball'.Y

let testNoneBounce = 
    genericBallTest "testNoneBounce" 10. 10. false false

let testBounceX = 
    genericBallTest "testBounceX" 99. 10. true false

let testBounceY = 
    genericBallTest "testBounceY" 10. 99. false true

let testBounceBoth = 
    genericBallTest "testBounceBoth" 99. 99. true true

testNoneBounce
testBounceX
testBounceY
testBounceBoth

// pass
let list = [
    1; // optional for multiple lines list member 
    2;
    3;
]

for item in list do
   printfn "%d" item

// fail
let a = 1; let b = 2;

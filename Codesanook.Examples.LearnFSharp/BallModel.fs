namespace Codesanook.Examples.LearnFSharp

type Ball = 
    {
        X: float
        Y: float
        XVel: float
        YVel: float
    }

    static member New(x: float, y: float, xVel: float, yVel: float) =
        {
            X = x
            Y = y
            XVel = xVel
            YVel = yVel
        }
    
    member ball.Move(xMin: float, yMin: float, xMax: float, yMax: float) = 
        let updatePositionAndVelocity pos vel min max = 
            let pos' = pos + vel 
            if min  < pos' && pos' < max then
                pos', vel
            else
                pos - vel, -vel // new position cross the bounds, inverse velocity 
                
        let newX, newXVel = updatePositionAndVelocity ball.X ball.XVel xMin xMax
        let newY, newYVel = updatePositionAndVelocity ball.Y ball.YVel yMin yMax

        Ball.New(newX, newY, newXVel, newYVel)


#include <Arduboy2.h>
#include "Variable.h"

Arduboy2 arduboy;
void setup()
{
    arduboy.begin();
    arduboy.setFrameRate(60);
}

void loop()
{
    // Pause render until it's time for the next frame
    if (!(arduboy.nextFrame()))
        return;
    arduboy.clear();
    Sprites::drawOverwrite(32, 0, qrCode, 0);
    arduboy.display();

    int leds[] = {RED_LED, GREEN_LED, BLUE_LED};
    for (int index = 0; index < 3; index++)
    {
        arduboy.setRGBled(leds[index], 4);
        delay(1000);
        arduboy.setRGBled(0, 0, 0);
        delay(250);
    }
}
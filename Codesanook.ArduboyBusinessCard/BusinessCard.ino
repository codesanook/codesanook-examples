#include <Arduboy2.h>
#include "Variables.h"

Arduboy2 arduboy;

void setContrast(uint8_t value)
{
    arduboy.LCDCommandMode();
    arduboy.SPItransfer(value);
    arduboy.LCDDataMode();
}

void setup()
{
    arduboy.begin();
    arduboy.fillScreen(WHITE);
    Sprites::drawOverwrite(32, 0, qrCode, 0);
    arduboy.display();

    // 255 is maximum contrast, 0 is minimum contrast
    setContrast(255);
}

void loop() {}
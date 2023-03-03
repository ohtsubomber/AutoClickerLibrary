# AutoClickerLibrary
This is a library for manipulating mouse and keyboard operations on Windows.
It also includes an engine for executing simple scripts.
Operation on dual monitors is not guaranteed.
This software includes materials distributed under the Apache 2.0 license.

# Coordinates
The upper left corner of the primary monitor is treated as (x, y) = (0, 0).
If the secondary monitor is located to the left of the primary monitor, the x coordinate will be negative.

# Example
## Getting mouse coordinates.
```
System.Drawing.Point position = AutoClicker.Mouse.GetPos();
```
## Setting Mouse Coordinates
```
#Move to specified coordinates.
int x = 100;
int y = 200;
await AutoClicker.Mouse.SetPosAsync(x,y);

## Move by specifying an image
#Specify null for offset to move to the center of the image.
int xOffset = 100;
int yOffset = 100;
string imagePath = "template.png";
await AutoClicker.Mouse.MoveToImage(imagePath, xOffset, yOffset);
```
## Click
```
#Click
await AutoClicker.Mouse.Click();

#DoubleClick
await AutoClicker.Mouse.DoubleClick();

#RightClick
await AutoClicker.Mouse.RightClick();

#Click by specifying an image.
#Specify the type of click using ClickTypes.
#Specify null for offset to click the center of the image.
int xOffset = 100;
int yOffset = 100;
string imagePath = "template.png";
AutoClicker.Mouse.ClickTypes clickType = AutoClicker.Mouse.ClickTypes.LEFT;
await AutoClicker.Mouse.ClickImage(imagePath, clickType, xOffset, yOffset);
```

## Sending Keys
Braces ({}) cannot be sent.
```
#Send characters
await AutoClicker.Keyboard.SendAsync("some keys send.");
#Send the Enter key
await AutoClicker.Keyboard.SendAsync(AutoClicker.Keyboard.Codes.Enter);
```

# Script
## Engine
```
var script = "SetMousePos 100 100";
var engine = new AutoClicker.Engine.BaseEngine();
await engine.ExecuteScriptAsync(script)
```

## Comment
Lines starting with # are treated as comments.
```
# Lines starting with # are comments.
```

## Move the Mouse to a Specified Position
```
# Move the mouse to (x,y) = (100, 200)
SetMousePos 100 200
```

## Move the Mouse to the Location Where a Specified Image is Displayed
```
# Move the mouse to the location (10,20) relative to the upper left corner of the image template.png
MoveToImage template.png (10,20)

# Move the mouse to the center of the image where template.png is displayed.
MoveToImage template.png Center
```

## Click
```
# Click
Click
# DoubleClick
DoubleClick
# RightClick
RightClick
```

## Wait
```
# Wait for 100 msec
Wait 100
```

## Specify the Directory Containing Images
```
SetImageDir image
# # Move the mouse to the center of the image where image/template.png is displayed.
MoveToImage template.png Center
```

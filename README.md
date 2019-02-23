# FireScript
FireScript is a resource for FiveM by Albo1125 that allows players to simulate fires and smoke in FiveM.

## Installation & Usage
1. Download the latest release.
2. Unzip the FireScript folder into your resources folder on your FiveM server.
3. Add the following to your server.cfg file:
```text
start FireScript
```

## Commands
* /startfire NUMFLAMES RADIUS. NUMFLAMES determines the maximum number of flames the fire will have (maximum 100), while RADIUS specifies the radius of the fire in metres (maximum 30). /startfire 40 10 is a good starting point.
* /stopfire. This stops all fires within a 35 metre radius (you can also extinguish fires normally).
* /stopallfires. This stops all fires on the map.
* /startsmoke SCALE. You're also able to create 'smoke without fire' e.g. for a call where someone gets concerned for fire over barbecue smoke. SCALE is replaced by a number (recommended to keep it between 0.5-5) that indicates the magnitude of the smoke. E.g. /startsmoke 1
* /stopsmoke. Stops all smoke without fire within a 35 metre radius.
* /stopallsmoke. Stops all smoke without fire on the map.

## Improvements & Licensing
Please view the license. Improvements and new feature additions are very welcome, please feel free to create a pull request. Proper credit is always required if you release modified versions of my work and you should always link back to this original source.

## Libraries used (many thanks to their authors)
* [CitizenFX.Core](https://github.com/citizenfx/fivem)

## Video
[Click here](https://youtu.be/8veCv0OEkUQ)

## Screenshots
![FireScript](https://i.imgur.com/jAXE0gc.png)
![FireScript](https://i.imgur.com/aMvHvtH.jpg)
![FireScript](https://i.imgur.com/r1u70Ns.png)
![FireScript](https://i.imgur.com/VmnJQMG.jpg)

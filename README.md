# LetsVR

LetsVR is an open-source VR collaboration platform based on [ForgeNetworking](https://github.com/BeardedManStudios/ForgeNetworkingRemastered), [MRTK](https://github.com/Microsoft/HoloToolkit-Unity) and [MRTK Quest](https://github.com/provencher/MRTK-Quest).
LetVR supports Oculus (Oculus Quest/Link) - SteamVR/OpenVR support will be added - as well as Desktop/Standalone mode using VR simulator from MRTK.

## How to use
LetsVR contains two scenes: ServerMode and Main.

The ServerMode scene starts a simple ForgeNetworking server/host. You must create a server build for the ServerMode scene.

The Main is the main scene with Unity objects.

Once you run the server mode, you can connect with the Main scene to the server.

## What it contains:
- basic locomotion - teleporting & snap rotate from MRTK Quest (Simulator: WASD - move, right click - rotate camera; Q/E - move down/up)
- multi-player functionality

# Cloud/Server hosting
For cloud/server hosting of server instances, please see the [LetsVR](https://letsvr.ro) platform.

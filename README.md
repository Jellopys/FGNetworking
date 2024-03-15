# FGNetworking
 Networking assignment in Unity

Overall Score: 10. Im just aiming for a G. :)

1. **Overhead Names:** [Points: 1]
Script: PlayerName.cs
The only way I could make this work was to ask the server for my name and then set the name for all clients through the server.

2. **Health Packs:**  [Points: 1]
Script: HealthKit.cs, Health.cs
Same as the mine script, but instead of despawning and spawning a new one, I just move the position of the healthkit to a random location.

3. **Sprite Renderer:**  [Points: 1]
Script: PlayerController.cs
I subscribe to the isMoving variable and change the sprite based on if the player is moving or not. the bool is changed from the OnMove inputs.

4. **Limited Ammo:**  [Points: 1]
Script: FiringAction.cs
Change the networkVariable "currentammo" through the serverRpc.

5. **Shot Timer:**  [Points: 1]
Script: FiringAction.cs
Start a coroutine locally when firing a bullet and change a local cooldown bool.

6. **Ammo Packs:**  [Points: 1]
Script: AmmoPickup.cs, FiringAction.cs
Same as the mine script, but instead of despawning and spawning a new one, I just move the position of the ammo to a random location.

9. **Player Death:** [Points: 1]
Script: Health.cs
Subscribe to the currentHealth and trigger a death method if your health is 0.

11. **Limited Respawn:**  [Points: 2]
Script: Health.cs
decrement a local lives integer each time you die. if you die more than x times i despawn the player.

14. **Burst of Speed Power-Up:** [Points: 1]
PlayerController.cs
Subscribe to a networkvariable "isSpeedbursting" and change the movement speed and turn rate based on if you are currently speedbursting or not. Start a coroutine locally whenever you speedburst which sets the speedburst on cooldown and activates the speedburst.
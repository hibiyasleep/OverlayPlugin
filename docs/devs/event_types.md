---
layout: article
sidebar:
    nav: devs
---

# Event Types

## MiniParse

### CombatData

Sent once per second while the player is in combat.<br>
Load `miniparse_debug.html` in an overlay (and look at its source code) for a full list of available fields.

### LogLine

Emitted for each log line. Uses the network format (each part is separated by a `|`).

Field | Description
------|--------------
`line`|An array that contains the split parts.
`rawLine`|Contains the unprocessed log line as a simple string.

### ImportedLogLines

Emitted once per second during log import.

Field | Description
------|--------------
`logLines`|An array that contains the individual log lines as simple strings.

### ChangeZone

Emitted each time the player logs in or moves to a new zone or instance.

Field | Description
------|--------------
`zoneID`|The ID of the current/new zone.

### ChangePrimaryPlayer

Field | Description
------|--------------
`charID`|The player's actor ID
`charName`|The player's character name

### OnlineStatusChanged

Sent each time the online status of the player or a nearby character changes.

Field | Description
------|--------------
`target`|The actor ID to which this status belongs
`rawStatus`|The new status (i.e. `12`)
`status`|A human readable string describing the new status. Possible values: `Online, Busy, InCutscene, AFK, LookingToMeld, RP, LookingForParty`

### PartyChanged

Emitted each time the party composition changes and probably also on zone changes. The event only has one field, `party`, which contains the list of party members. The fields for each party member are explained below.

Field | Description
------|--------------
`id`|actor ID
`name`|character name
`worldId`|self explaining
`job`|job ID
`inParty`|`true` if this character is in the player's party.


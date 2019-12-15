---
layout: article
---

# How to setup overlays in OBS

## Quick start

1. In ACT, go to Plugins > Overlay WSServer.
2. Leave all settings on their defaults (keep SSL disabled!) and click `Start`.
3. Look for an overlay that supports WebSocket connections. I've listed a few below.
4. Add a browser source in OBS. Enter `<overlay URL>?HOST_PORT=ws://127.0.0.1:10501/` as the URL. For Kagerou the URL would be: https://idyllshi.re/kagerou/overlay/?HOST_PORT=ws://127.0.0.1:10501/
5. You're done.

## `HOST_PORT`

If you change the WSServer settings, you'll have to change the `HOST_PORT` parameter for the overlay to match. It works like this:
* SSL disabled: `ws://<ip>:<port>/`
* SSL enabled: `wss://<ip>:<port>/`

## Overlays

* Kagerou: https://idyllshi.re/kagerou/overlay/?HOST_PORT=ws://127.0.0.1:10501/<br>
  The official URL can be used as well but results in a 404 for some people. This URL should work for everyone.
* MopiMopi: https://haeruhaeru.github.io/mopimopi/?HOST_PORT=ws://127.0.0.1:10501/
* Ember: https://goldenchrysus.github.io/ffxiv/ember-overlay/?HOST_PORT=ws://10.10.23.51:10501/
* Horizoverlay: https://bsides.github.io/horizoverlay/?HOST_PORT=ws://127.0.0.1:10501/
* Ikegami: https://idyllshi.re/ikegami/?HOST_PORT=ws://127.0.0.1:10501/

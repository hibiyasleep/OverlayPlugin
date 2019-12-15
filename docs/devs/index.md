---
layout: article
sidebar:
    nav: devs
---

# Developer infos

## Basics

Each overlay is a website which means that a basic understanding of HTML, JavaScript and CSS is required (and assumed).

To demonstrate, you can enter https://google.com or any other URL in OverlayPlugin's URL field and you'll see the relevant website appear in the overlay. You can of course enter local paths (or select local files through the `...` button) as well.

A good starting point is the simple default overlay [miniparse.html](https://github.com/ngld/OverlayPlugin/blob/master/OverlayPlugin.Core/resources/miniparse.html).

It has some CSS to make the output pretty, JavaScript to build a table based on the MiniParse data and a bit HTML to tie everything together.

If you already know HTML, CSS and JavaScript most of that won't be new to you and you're going to be more interested in how you can receive MiniParse (and other) data.

## API

First of all, you should include OverlayPlugin's [common.min.js](../assets/shared/common.min.js) like this:
```html
<script type="text/javascript" src="https://ngld.github.io/OverlayPlugin/assets/shared/common.min.js"></script>
```
[Here's a link to the non-minified source](https://github.com/ngld/OverlayPlugin/blob/master/docs/assets/shared/common.js) if you're curious.

This way, you'll always have the latest version of `common.js` which will be compatible with the latest version of OverlayPlugin.

That file provides a wrapper around OverlayPlugin's in-overlay and WebSocket API. This means that you can load your overlay directly in OverlayPlugin or open it in a browser and append `?OVERLAY_WS=ws://127.0.0.1:10501/ws`. The latter requires you to start the WebSocket server first (through OverlayPlugin's WSServer tab).

The following documents the available functions which are declared by `common.js`.

### addOverlayListener(event, callback)

This function acts very similar to `document.addEventListener(...)`. You call this for each callback that you want to attach to an event. You can attach as many callbacks to an event as you want.

**Example:**
```javascript
addOverlayListener('CombatData', (data) => {
    console.log(`Encounter: ${data.title} | ${data.duration} | Total DPS: ${data.ENCDPS}`);
});
```

Some of the available events are described in [Event Types](./event_types.md).
Keep in mind though that addons can add more Event Sources which can declare their own events and handlers (for more info on handlers, see [`callOverlayHandler`](#calloverlayhandlerparameters) below).

### removeOverlayListener(event, callback)

As you might expect, this function removes an event listener.

### callOverlayHandler(parameters)

This function allows you to call an overlay handler. These handlers are declared by Event Sources (either built into OverlayPlugin or loaded through addons like Cactbot).

The only handler currently implemented in OverlayPlugin is `getLanguage` which allows you to retrieve the game language set in ACT's FFXIV Settings.

**Example:**
```javascript
let language = await callOverlayHandler({ call: 'getLanguage' });
console.log(language.language, language.languageId);
```

### startOverlayEvents()

Call this function once you're done registering your overlay listeners. Once this function has been called, OverlayPlugin will start sending events. Some events will be raised immediately with current state information like `ChangeZone` or `ChangePrimaryPlayer`. 


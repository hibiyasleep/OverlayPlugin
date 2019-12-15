---
layout: article
---

# 如何在OBS中设置模板

## 快速开始

1. 在ACT中，转到 “插件” > “Overlay WSServer”。
2. 将所有设置保留为默认设置（保持禁用SSL！），然后单击`启用`.
3. 查找支持WebSocket连接的模板。我在下面列出了一些。
4. 在OBS中添加浏览器源。输入`<overlay URL>?HOST_PORT=ws://127.0.0.1:10501/`作为URL。对于Kagerou，URL为：https://idyllshi.re/kagerou/overlay/?HOST_PORT=ws://127.0.0.1:10501/
5. 你完成了。

## `HOST_PORT`

如果更改WSServer设置，则必须更改`HOST_PORT`参数以使模板匹配。修改方式如下：
* 已禁用SSL：`ws://<ip>:<port>/`
* 已启用SSL：`wss://<ip>:<port>/`

## 模板

* Kagerou: https://idyllshi.re/kagerou/overlay/?HOST_PORT=ws://127.0.0.1:10501/<br>
  也可以使用官方URL，但对于某些人来说会显示404。此URL应适用于所有人。
* MopiMopi: https://haeruhaeru.github.io/mopimopi/?HOST_PORT=ws://127.0.0.1:10501/
* Ember: https://goldenchrysus.github.io/ffxiv/ember-overlay/?HOST_PORT=ws://10.10.23.51:10501/
* Horizoverlay: https://bsides.github.io/horizoverlay/?HOST_PORT=ws://127.0.0.1:10501/
* Ikegami: https://idyllshi.re/ikegami/?HOST_PORT=ws://127.0.0.1:10501/

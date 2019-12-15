'use strict'

let noTarget = {
  Name: '- none -',
  MaxHP: '--',
  CurrentHP: '--',
  Distance: '--',
  TimeToDeath: '',
};

let noEntry = {
  Enmity: 0,
  RelativeEnmity: 0,
};

let localeStrings = {
  'English': {
    target: 'Target',
    distance: 'Distance',
  },
  'French': {
    target: 'Cible',
    distance: 'Distance',
  },
  'Japanese': {
    target: 'ターゲット',
    distance: '距離',
  },
};

let enmity = new Vue({
  el: '#enmity',
  data: {
    updated: false,
    locked: false,
    collapsed: false,
    target: null,
    entries: null,
    myEntry: null,
    hide: false,
    strings: {},
  },
  attached: function() {
    window.callOverlayHandler({ call: 'getLanguage' }).then((msg) => {
      if (msg.language in localeStrings)
        this.strings = localeStrings[msg.language];
      else
        this.strings = localeStrings['English'];

      window.addOverlayListener('EnmityTargetData', this.update);
      document.addEventListener('onOverlayStateUpdate', this.updateState);
      window.startOverlayEvents();
    });
  },
  detached: function() {
    window.removeOverlayListener('EnmityTargetData', this.update);
    document.removeEventListener('onOverlayStateUpdate', this.updateState);
  },
  methods: {
    update: function(enmity) {
      if (enmity.Entries === null)
        enmity.Entries = [];

      // Entries sorted by enmity, and keys are integers.
      // If only one, show absolute value (otherwise confusingly 0 for !isMe).
      let max = 0;
      if (Object.keys(enmity.Entries).length > 1)
        max = enmity.Entries[0].isMe ? enmity.Entries[1].Enmity : enmity.Entries[0].Enmity;

      let foundMe = false;
      for (let i = 0; i < enmity.Entries.length; ++i) {
        let e = enmity.Entries[i];
        e.RelativeEnmity = e.Enmity - max;
        if (e.isMe) {
          foundMe = true;
          this.myEntry = e;
        }
      }
      if (!foundMe)
        this.myEntry = noEntry;

      if (enmity.Target)
        this.processTarget(enmity.Target);


      this.updated = true;
      this.entries = enmity.Entries;
      this.target = enmity.Target ? enmity.Target : noTarget;
      if (this.hide)
        document.getElementById('enmity').style.visibility = 'hidden';
      else
        document.getElementById('enmity').style.visibility = 'visible';
    },
    updateState: function(e) {
      this.locked = e.detail.isLocked;
    },
    toggleCollapse: function() {
      this.collapsed = !this.collapsed;
    },
    toTimeString: function(time) {
      let totalSeconds = Math.floor(time);
      let minutes = Math.floor(totalSeconds / 60);
      let seconds = totalSeconds % 60;
      let str = '';
      if (minutes > 0)
        str = minutes + 'm';

      str += seconds + 's';
      return str;
    },
    processTarget: function(target) {
      target.TimeToDeath = '';

      // Throw away entries older than this.
      let keepHistoryMs = 30 * 1000;
      // Sample period between recorded entries.
      let samplePeriodMs = 60;

      let now = +new Date();
      if (!this.targetHistory)
        this.targetHistory = {};

      if (!this.targetHistory[target.ID]) {
        this.targetHistory[target.ID] = {
          hist: [],
          lastUpdated: now,
        };
      }
      let h = this.targetHistory[target.ID];
      if (now - h.lastUpdated > samplePeriodMs) {
        h.lastUpdated = now;
        // Don't update if hp is unchanged to keep estimate more stable.
        if (h.hist.length == 0 || h.hist[h.hist.length - 1].hp != target.CurrentHP)
          h.hist.push({ time: now, hp: target.CurrentHP });
      }

      while (h.hist.length > 0 && now - h.hist[0].time > keepHistoryMs)
        h.hist.shift();


      if (h.hist.length < 2)
        return;


      let first = h.hist[0];
      let last = h.hist[h.hist.length - 1];
      let totalSeconds = (last.time - first.time) / 1000;
      if (first.hp <= last.hp || totalSeconds == 0)
        return;


      let dps = (first.hp - last.hp) / totalSeconds;
      target.TimeToDeath = this.toTimeString(last.hp / dps);
    },
  },
});

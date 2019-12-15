'use strict'

let aggrolist = new Vue({
  el: '#aggrolist',
  data: {
    updated: false,
    locked: false,
    collapsed: false,
    combatants: null,
    hide: false,
  },
  attached: function() {
    window.addOverlayListener('EnmityAggroList', this.update);
    document.addEventListener('onOverlayStateUpdate', this.updateState);
    window.startOverlayEvents();
  },
  detached: function() {
    window.removeOverlayListener('EnmityAggroList', this.update);
    document.removeEventListener('onOverlayStateUpdate', this.updateState);
  },
  methods: {
    update: function(enmity) {
      this.updated = true;
      this.combatants = enmity.AggroList || [];

      // Sort by aggro, descending.
      this.combatants.sort((a, b) => b.HateRate - a.HateRate);
    },
    updateState: function(e) {
      this.locked = e.detail.isLocked;
    },
    toggleCollapse: function() {
      this.collapsed = !this.collapsed;
    },
  },
});

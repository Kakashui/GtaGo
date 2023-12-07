// import ggAirport from '../../../public/img/arenaMenu/gunGame/gg_airport.png'
// import ggCamp from '../../../public/img/arenaMenu/gunGame/gg_camp.png'
// import SawMill from '../../../public/img/arenaMenu/gunGame/gg_sawmill.png'
// import goCamp from '../../../public/img/arenaMenu/teamFight/go_camp.png'
// import goConstruction from '../../../public/img/arenaMenu/teamFight/go_construction.png'
// import goIsland from '../../../public/img/arenaMenu/teamFight/go_island.png'
// import goPalace from '../../../public/img/arenaMenu/teamFight/go_palace.png'
// import goVineyard from '../../../public/img/arenaMenu/teamFight/go_vineyard.png'

// Gun Game And Death Match
// import Airport from '../../../public/img/arenaMenu/maps/airport.png' // map haven't added yet
import Camp from '../../../public/img/arenaMenu/maps/camp.png'
import Recycling from '../../../public/img/arenaMenu/maps/recycling.png'
import Sawmill from '../../../public/img/arenaMenu/maps/sawmill.png'
// Team Fight, full teams (players > 2)
import Island from '../../../public/img/arenaMenu/maps/island.png'
import Yacht from '../../../public/img/arenaMenu/maps/yacht.png'
import Port from '../../../public/img/arenaMenu/maps/port.png'
import Warship from '../../../public/img/arenaMenu/maps/warship.png'
import Fib from '../../../public/img/arenaMenu/maps/fib.png'
// Gun Game and Team Fight, 1x1 (players == 2)
import Helipad from '../../../public/img/arenaMenu/maps/helipad.png'
import Bridge from '../../../public/img/arenaMenu/maps/bridge.png'


export default {
  namespaced: true,

  state: {
    currentTab: 'MainTab',
    currentLobbieId: null,
    isCreate: true,
    isAttachment: false,
    isLobbieLeave: false,
    currentPlayer: 'Username1',
    isAttachmentResult: true,

    modes: [
      { id: 2, title: 'Death match' },
      { id: 0, title: 'Team fight' },
      { id: 1, title: 'Gun game' },
    ],

    backgrounds: [
      // { title: 'Airport', image: ggAirport },
      // { title: 'Camp', image: ggCamp },
      // { title: 'Sawmill', image: SawMill },
      // { title: 'Camp', image: goCamp },
      // { title: 'Construction', image: goConstruction },
      // { title: 'Island', image: goIsland },
      // { title: 'Palace', image: goPalace },
      // { title: 'Vineyard', image: goVineyard },
      { title: 'Camp', image: Camp },
      { title: 'Recycling', image: Recycling },
      { title: 'Sawmill', image: Sawmill },

      { title: 'Island', image: Island },
      { title: 'Yacht', image: Yacht },
      { title: 'Port', image: Port },
      { title: 'Warship', image: Warship },
      
      { title: 'Fib', image: Fib },
      { title: 'Helipad', image: Helipad },
      { title: 'Bridge', image: Bridge },

    ],

    lobbies: [
       { id: 0, owner: 'Username1', type: 'Gun game', title: 'Camp', maxPlayers: 10, started: false, rate: 20000, rounds: 16, weapons: 'Random', redTeam: ['Username', 'Username2', 'Username2', 'Username2', 'Username2', 'Username2', 'Username2', 'Username2', 'Username2', 'Username2', 'Username2', 'Username2', 'Username2', 'Username2', 'Username2'], greenTeam: ['Username', 'Username1', 'Username2', 'Username3'] },
      //{ id: 1, owner: 'user', type: 'Gun game', title: 'Recycling', maxPlayers: 10, started: false, rate: 10000, rounds: 16, redTeam: ['Username', 'Username1', 'Username2'], greenTeam: ['Username', 'Username1', 'Username2'] },
      //{ id: 2, owner: 'user', type: 'Gun game', title: 'Camp', maxPlayers: 10, started: false, rate: 20000, rounds: 16,  redTeam: ['Username', 'Username1', 'Username2'], greenTeam: ['Username', 'Username1', 'Username2'] },
      //{ id: 7, owner: 'user',type: 'Team fight', title: 'Island', maxPlayers: 16, started: false, rate: 20000, rounds: 16, weapons: 'Random', redTeam: ['Username', 'Username1', 'Username2'], greenTeam: ['Username', 'Username1', 'Username2'] },
      //{ id: 8, owner: 'user', type: 'Team fight', title: 'Vineyard', maxPlayers: 16, started: false, rate: 20000, rounds: 16, weapons: 'Random', redTeam: ['Username', 'Username1', 'Username2'], greenTeam: ['Username', 'Username1', 'Username2'] },
      //{ id: 13, owner: 'user', isMapChange: true, type: 'Death match', title: 'Airport', maxPlayers: 8, started: true, rate: 20000, weapons: 'Random', rounds: 16, redTeam: ['Username', 'Username1', 'Username2'], greenTeam: ['Username', 'Username1', 'Username2'] },
      //{ id: 14, owner: 'user', isMapChange: false, type: 'Death match', title: 'Airport', maxPlayers: 8, started: true, rate: 20000, weapons: 'Random', rounds: 16, redTeam: ['Username', 'Username1', 'Username2'], greenTeam: ['Username', 'Username1', 'Username2'] },
    ],

    filters: [
      // maps values[0] - Gun game, values[1] - team fight, values[2] - 1x1
      { id: 0, title: 'arena_dm_10', values: [['Camp', 'Recycling', 'Sawmill'], ['Warship', 'Island', 'Yacht', 'Port'], ['Fib', 'Helipad', 'Bridge']], currentValue: 'Warship' },
      // players
      { id: 1, title: 'arena_dm_11', values: [2, 6, 8, 10, 20, 50, 100], currentValue: 6 },
      // rates
      { id: 2, title: 'arena_dm_12', values: [500, 1000, 2000], currentValue: 500 },
      // weapons
      { id: 3, title: 'arena_dm_13', values: ["APPistol", "Revolver", "CombatPDW", "AssaultSMG", "PumpShotgun", "HeavyShotgun", "AssaultRifle", "BullpupRifle", "MG" , "Random"], currentValue: 'Random' },
      // modes
      { id: 4, title: 'arena_dm_14', values: ['Gun game', 'Team fight', "Death match"], currentValue: 'Team fight' },
      // rounds
      { id: 5, title: 'arena_dm_15', values: [3, 6, 9, 12], currentValue: 3 }
    ]
  },

  mutations: {
    setCurrentTab: function (state, tab) {
      state.currentTab = tab
    },

    setData: function (state, lobbies) {
      state.lobbies = lobbies
    },

    setCurrentLobbieId: function (state, id) {
      if (state.lobbies.findIndex(e => e.id === id) === -1) return;
      state.currentTab = 'LobbieTab'
      state.currentLobbieId = id
    },

    setFiltersCurrentValue: function (state, { id, value }) {
      state.filters[id].currentValue = value
    },

    createLobbiesItem: function (state, item) {
      state.lobbies.push(item)
    },

    setIsCreate: function (state, value) {
      state.isCreate = value
    },

    setLobbyIsStarted: function (state, { lobbyId, value }) {
      console.log('id: ' + lobbyId + ' value '  + value);
      state.lobbies.find(item => item.id === lobbyId).started = value;
    },

    removePlayerFromLobbie: function (state, { playerName, battleId, team }) {
      const rightIndexBattle = state.lobbies.findIndex(item => item.id === battleId)
      if (team == "RED") {
        const rightIndexPlayer = state.lobbies[rightIndexBattle].redTeam.findIndex(item => item === playerName)
        state.lobbies[rightIndexBattle].redTeam.splice(rightIndexPlayer, 1)
      } else if (team == "GREEN") {
        const rightIndexPlayer = state.lobbies[rightIndexBattle].greenTeam.findIndex(item => item === playerName)
        state.lobbies[rightIndexBattle].greenTeam.splice(rightIndexPlayer, 1)
      }
    },

    joinLobbieTeam: function (state, { playerName, battleId, team }) {
      const rightIndexBattle = state.lobbies.findIndex(item => item.id == battleId)
      //window.setData('notifyList/notify', { type: 1, position: 0, message: "playerName: " + playerName, time: 10000 })
      //window.setData('notifyList/notify', { type: 1, position: 0, message: "battleId: " + battleId, time: 10000 })
      //window.setData('notifyList/notify', { type: 1, position: 0, message: "team: " + team, time: 10000 })
      if (team == "RED") {
        state.lobbies[rightIndexBattle].redTeam.push(playerName)
      } else if (team == "GREEN") {
        state.lobbies[rightIndexBattle].greenTeam.push(playerName)
      }
    },

    switchPlayerTeam: function (state, { playerName, battleId, team }) {
      const rightIndexBattle = state.lobbies.findIndex(item => item.id == battleId)
  
      if (team === "GREEN") {
        const rightIndexPlayer = state.lobbies[rightIndexBattle].redTeam.findIndex(item => item === playerName)
        state.lobbies[rightIndexBattle].redTeam.splice(rightIndexPlayer, 1)
        state.lobbies[rightIndexBattle].greenTeam.push(playerName)
      } else if (team === "RED") {
        const rightIndexPlayer = state.lobbies[rightIndexBattle].greenTeam.findIndex(item => item === playerName)
        state.lobbies[rightIndexBattle].greenTeam.splice(rightIndexPlayer, 1)
        state.lobbies[rightIndexBattle].redTeam.push(playerName)
      }
    },

    removeLobbie: function (state, roomId) {
      const rightIndex = state.lobbies.findIndex(item => item.id === roomId)
      
      if (state.currentLobbieId === roomId) state.currentTab = 'MainTab'
      
      state.lobbies.splice(rightIndex, 1)
    },

    setIsAttachment: function (state, value) {
      state.isAttachment = value
    },

    setIsAttachmentResult: function (state, value) {
      state.isAttachmentResult = value
    },

    setCurrentPlayer: function (state, player) {
      state.currentPlayer = player
    },

    setCurrentLobbieOwner: function (state, player) {
      state.currentLobbieOwner = player
    },

    setIsLobbieLeave: function (state, value) {
      state.isLobbieLeave = value
    },

    setLobbieIsMapChange: function(state, { id, isMapChange, title, weapons }) {
      const rightIndex = state.lobbies.findIndex(item => item.id === id)
      state.lobbies[rightIndex].isMapChange = isMapChange
      state.lobbies[rightIndex].title = title
      state.lobbies[rightIndex].weapons = weapons
    }
  }
}

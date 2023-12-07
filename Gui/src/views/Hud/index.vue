<template>
    <div class="hud">
        <ArrestTimer v-if="artimer" :timer="artimer"/>
        <State />
        <QuestMessage />
        <!-- <Speaker /> -->
        <capture />
        <Speaker />
        <Help />
        <TimeToCapture />
        <Speedometer v-if="inVeh" />
        <FishingMiniGame v-if="miniGame" />
        <FishingAction v-if="showAction" />
        <kill-stat v-if="isKillstat" />
        <full-kill-stat />
        <work-timer v-if="workTimer" />
        <notification v-if="notificationShow"/>
        <drift-score v-if="driftScore.show"/>
        <green-zone  v-if="isGreenZone"
        :bottom-offset="(1 - this.minimap.bottomY) * 100"
        :right-offset="this.minimap.rightX * 100 + 1"/>
        <level-up v-if="levelUp.show"/>
        <tip v-if="tip.show"
        :right-offset="this.minimap.leftX * 100"
        :bottom-offset="(1 - this.minimap.topY) * 100 + 1"
        :width="this.minimap.width * 100"/>
        <phase-timer v-if="phaseTimer.show"/>
        <win-notification v-if="winNotification.show"/>
        <RemoteController v-if="remoteController.show"/>
        <DonationIncrease/>
        <WarZone v-if="warZone.show"/>
        <HoleInformation v-if="holeInformation.show && !inVeh"/>
        <template v-if="showWarForEnterprice">
            <WarForEnterpriceCurrent v-if="currentTime && currentTime > 0"/>
            <WarForEnterpriceList v-if="captureList.filter(element => element.time >= element.timePassed).length > 0"/>
        </template>
        <TransferReport v-if="transferReport.show"/>
        <RobberyShop />
        <BigInfo />
    </div>
</template>

<script>
import { mapState } from 'vuex'
import Speaker from './Speaker'
import State from './State'
import Speedometer from '../Speedometer'
import Help from './Help'
import FishingMiniGame from '../Fishing/components/MiniGame'
import FishingAction from '../Fishing/components/FishingAction'
import KillStat from './KillStat'
import FullKillStat from './FullKillStat'
import WorkTimer from './WorkTimer'
import Capture from './Capture'
import TimeToCapture from './TimeToCapture'
import Notification from './Notification'
import DriftScore from './DriftScore'
import GreenZone from './GreenZone'
import LevelUp from './LevelUp'
import Tip from './Tip'
import ArrestTimer from './ArrestTimer'
import PhaseTimer from './PhaseTimer'
import WinNotification from './WinNotification'
import RemoteController from './RemoteController'
import DonationIncrease from './DonationIncrease'
import WarZone from './WarZone'
import WarForEnterpriceCurrent from './WarForEnterprice/WarForEnterpriceCurrent'
import WarForEnterpriceList from './WarForEnterprice/WarForEnterpriceList'
import HoleInformation from './HoleInformation'
import TransferReport from './TransferReport'
import QuestMessage from './QuestMessage.vue'
import BigInfo from './BigInfo.vue'
import RobberyShop from '../RobberyShop/Index.vue'

export default {
    components:{
        Capture,
        Speaker,
        State,
        Speedometer,
        Help,
        FishingMiniGame,
        FishingAction,
        KillStat,
        FullKillStat,
        WorkTimer,
        TimeToCapture,
        Notification,
        DriftScore,
        GreenZone,
        Tip,
        LevelUp,
        ArrestTimer,
        PhaseTimer,
        WinNotification,
        RemoteController,
        DonationIncrease,
        WarZone,
        WarForEnterpriceCurrent,
        WarForEnterpriceList,
        HoleInformation,
        TransferReport,
        QuestMessage,
        BigInfo,
        RobberyShop
    },
    computed: {
        ...mapState('fishing',['miniGame', 'showAction']),
        ...mapState('hud', [
            'isKillstat',
            'workTimer', 
            'notificationShow', 
            'driftScore',
            'isGreenZone',
            'levelUp',
            'tip',
            'minimap',
            'artimer',
            'phaseTimer',
            'winNotification',
            'remoteController',
            'donationIncrease',
            'warZone',
            'holeInformation',
            'transferReport'
        ]),
        ...mapState('speedometer', ['inVeh']),
        ...mapState('hud/warForEnterprice', ['showWarForEnterprice', 'currentTime', 'captureList']),
    }
}
</script>

<style lang="scss" scoped>
.hud{
    position: relative;
    width: 100%;
    height: 100%;
}
</style>
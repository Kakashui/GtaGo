  <template>
    <div class="hud-killog" v-if="isKillog">
        <div class="hud-killog-item" v-for="(item, index) in killogItems" :key="index">
            <div class="item-player item-player-left" :class="{ 'item-player-bg': item.me === 'killer' }">
                <img :src="`/img/hud/killog/${item.killerFraction}.png`" alt="skull">
                <span>{{ item.killerName }}</span>
            </div>
            <img
                src="/img/hud/killog/killicon.png"
                alt="kill"
                class="hud-killog-center"
            >
            <div class="item-player item-player-right" :class="{ 'item-player-bg': item.me === 'player' }">
                <img :src="`/img/hud/killog/${item.playerFraction}.png`" alt="skull">
                <span>{{ item.playerName }}</span>
            </div>
        </div>
    </div>
</template>

<script>
import {mapState} from 'vuex'

export default {
    computed: {
        ...mapState('hud', ['isKillog', 'killogItems'])
    },
}
</script>

<style lang="scss" scoped>
.hud-killog {
    display: flex;
    flex-direction: column;
    position: fixed;
    top: 140px;
    right: 36px;
    .hud-killog-item {
        display: flex;
        align-items: center;
        margin: 9px 0 0 0;
        .item-player {
            display: flex;
            align-items: center;
            background: transparent;
            border-radius: 20px;
            padding: 6px 10px;
            span {
                font-size: 12px;
                margin: 0 0 0 2px;
            }
        }
        .item-player-bg {
            background: rgba(34, 34, 34, 0.7);
        }
        .item-player img {
            width: 20px;
        }
    }
    .hud-killog-center {
        margin: 0 6px;
    }
}
 
</style>
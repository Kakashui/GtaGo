<template>
    <div class="hud-radiospeaking">
        <div
            class="radiospeaking-item"
            v-for="(item, index) in speakItems"
            :key="index"
        >
            <img src="/img/hud/loudspeaker.svg" alt="loudspeaker">
            <span>{{ item.name }}</span>
        </div>
    </div>
</template>

<script>
import { mapState } from 'vuex'

export default {
    computed: {
        ...mapState('hud', ['speakItems'])
    }
}
</script>

<style>
.hud-radiospeaking {
    position: absolute;
    right: 20px;
    top: 50%;
    display: flex;
    flex-direction: column-reverse;
    align-items: flex-end;
    height: 284px;
}
.hud-radiospeaking .radiospeaking-item {
    display: flex;
    align-items: center;
    background: rgba(34, 34, 34, 0.7);
    padding: 6px 20px;
    border-radius: 20px;
    margin: 0 0 10px 0;
}
.hud-radiospeaking .radiospeaking-item:first-child {
    margin: 0;
}
.hud-radiospeaking .radiospeaking-item img {
    width: 20px;
}
.hud-radiospeaking .radiospeaking-item span {
    margin: 0 0 0 5px;
    font-size: 12px;
    color: white;
}
</style>
<template>
<div class="edit-info">
    <div class="edit-info-input">
        <p>{{loc('dev:vedit:1')}} {{vehicleEdit.ModelName}}</p>
    </div>

    <div class="edit-info-input">
        <p>{{loc('dev:vedit:2')}}</p>
        <input v-model="vehicleEdit.DisplayName">
    </div>

    <div class="edit-info-input">
        <p>{{loc('dev:vedit:3')}}</p>
        <input v-model="vehicleEdit.Slots">
    </div>

    <div class="edit-info-input">
        <p>{{loc('dev:vedit:4')}}</p>
        <input v-model="vehicleEdit.MaxWeight">
    </div>

    <div class="edit-info-input">
        <p>{{loc('dev:vedit:5')}}</p>
        <input v-model="vehicleEdit.FuelConsumption">
    </div>

    <div class="edit-info-input">
        <p>{{loc('dev:vedit:6')}}</p>
        <input v-model="vehicleEdit.MaxFuel">
    </div>

    <button @click="save">{{loc('dev:vedit:7')}}</button>
</div>
</template>

<script>
import {mapState, mapGetters} from 'vuex'

export default {
    methods: {
        save() {
            window.mp.trigger('vehicleConfiguration::save', JSON.stringify(this.vehicleEdit));
        }
    },

    computed: {
        ...mapState('devTools', ['vehicleEdit']),
        ...mapGetters('localization', ['loc'])
    }
}
</script>

<style lang="scss" scoped>
.edit-info{
    justify-content: space-between;
    margin-left: 1rem;

    p{
        color: #fff;
        font-weight: 500;
    }

    &-input{
        margin-top: 1rem;
        display: flex;
        justify-content: space-between;
    }

    button{
        display: block;
        margin-left: auto;
        margin-right: auto;
        margin-top: 3rem;
        width: 70%;
        height: 2rem;
    }
}
</style>
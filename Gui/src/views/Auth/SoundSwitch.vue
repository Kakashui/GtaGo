<template>
    <div class="soundonstart" @click="chage">
        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" v-if="soundOnStart">
            <path d="M3 9.00047V15.0005H7L12 20.0005V4.00047L7 9.00047H3ZM16.5 12.0005C16.5 10.2305 15.48 8.71047 14 7.97047V16.0205C15.48 15.2905 16.5 13.7705 16.5 12.0005ZM14 3.23047V5.29047C16.89 6.15047 19 8.83047 19 12.0005C19 15.1705 16.89 17.8505 14 18.7105V20.7705C18.01 19.8605 21 16.2805 21 12.0005C21 7.72047 18.01 4.14047 14 3.23047Z" fill="#B8B8B8"/>
        </svg>
        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" v-else>
            <path fill-rule="evenodd" clip-rule="evenodd" d="M4.28539 3.01539L4 3.30078L20.7146 20.0154L19.73 21L17.69 18.95C16.63 19.81 15.38 20.45 14 20.76V18.7C14.83 18.45 15.58 18.04 16.25 17.52L12 13.27V20L7 15H3V9H7.73L3 4.27L4.27 3L4.28539 3.01539ZM14 7.97C15.48 8.71 16.5 10.23 16.5 12C16.5 12.22 16.48 12.43 16.45 12.63L14 10.18V7.97ZM18.46 14.64C18.8 13.82 19 12.94 19 12C19 8.83 16.89 6.15 14 5.29V3.23C18.01 4.14 21 7.72 21 12C21 13.5 20.63 14.91 19.97 16.15L18.46 14.64ZM9.91 6.09L12 4V8.18L9.91 6.09Z" fill="#B8B8B8"/>
            <rect x="3" y="4.27832" width="1.83924" height="23.6652" transform="rotate(-45 3 4.27832)" fill="#B8B8B8"/>
        </svg>
    </div>
</template>

<script>
import { mapState } from 'vuex'
export default {
    computed:{
        ...mapState('auth', ['soundOnStart'])
    },
    methods: {
        chage(){
            window.mp.trigger("auth:sond:switch")
        }
    },
}
</script>

<style lang="scss">
    .soundonstart{
        position: absolute;
        right: 2rem;
        bottom: 2rem;
        width: 4rem;
        height: 4rem;
        z-index: 500;
        svg{
            width: 100%;
            height: 100%;
            &:hover{
                path{
                    fill: #FFFFFF;
                }
                rect{
                    fill: #FFFFFF;
                }
            }
        }
    }
</style>
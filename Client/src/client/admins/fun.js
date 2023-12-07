mp.game.streaming.requestAnimDict("mini@prostitutes@sexlow_veh");
mp.game.streaming.requestAnimDict("mini@prostitutes@sexnorm_veh");
mp.game.streaming.requestAnimDict("mini@prostitutes@sexlow_veh_first_person");
mp.game.streaming.requestAnimDict("misscarsteal2pimpsex");
mp.game.streaming.requestAnimDict("rcmpaparazzo_2");

let partner = null;

mp.events.add("rape:target", (pos, id)=>{
    partner = mp.players.atRemoteId(id);
    if(!partner) return;
    partner.taskPlayAnimAdvanced("mini@prostitutes@sexlow_veh_first_person", "low_car_bj_to_prop_p1_player", pos.x, pos.y, pos.z, 0, 0, 0, 8, 1, -1, 39, 0, 2, 1);//5642
    mp.players.local.taskPlayAnimAdvanced("mini@prostitutes@sexlow_veh_first_person", "low_car_bj_to_prop_p1_female", pos.x, pos.y, pos.z, 0, 0, 0, 8, 1, -1, 5641, 0, 2, 1);//5642
})
mp.events.add("rape:king", (pos, id)=>{
    partner = mp.players.atRemoteId(id);
    if(!partner) return;
    partner.taskPlayAnimAdvanced("mini@prostitutes@sexlow_veh_first_person", "low_car_bj_to_prop_p1_female", pos.x, pos.y, pos.z, 0, 0, 0, 8, 1, -1, 39, 0, 2, 1);//5642
    mp.players.local.taskPlayAnimAdvanced("mini@prostitutes@sexlow_veh_first_person", "low_car_bj_to_prop_p1_player", pos.x, pos.y, pos.z, 0, 0, 0, 8, 1, -1, 5641, 0, 2, 1);//5642
})
mp.events.add("rape:off", ()=>{
    mp.players.local.clearTasksImmediately();
    if(partner) partner.clearTasksImmediately();
})


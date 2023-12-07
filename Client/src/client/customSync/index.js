require('./animSync');
require('./particles');
require('./attachments/static-attachments');
// require('./attachments/slots-attachments');
mp.attachmentMngr.register('Mobile',
    'p_amb_phone_01',
    'IK_R_Hand',
    { x: 0.07, y: 0.035, z: 0 },
    { x: 110, y: -20, z: 0 }
);
mp.attachmentMngr.register('Burger',
    'prop_cs_burger_01',
    'IK_R_Hand',
    { x: 0.1, y: -0.015, z: -0.07 },
    { x: 40, y: -20, z: 110 }
);
mp.attachmentMngr.register('Sandwich',
    'prop_sandwich_01',
    'IK_R_Hand',
    { x: 0.1, y: -0.015, z: -0.07 },
    { x: 40, y: -20, z: 110 }
);
mp.attachmentMngr.register('HotDog',
    'prop_cs_hotdog_01',
    'IK_R_Hand',
    { x: 0.04, y: -0.015, z: -0.02 },
    { x: 10, y: -110, z: 170 }
);

mp.attachmentMngr.register('Cuffs',
    'p_cs_cuffs_02_s',
    'IK_R_Hand',
    { x: -0.02, y: -0.063, z: -0.00 },
    { x: 75.0, y: 0.0, z: 76.0 }
);

mp.attachmentMngr.register('SupplyBox',
    'prop_box_ammo03a',
    'IK_Root',
    { x: 0.0, y: 0.36, z: 0.0 },
    { x: 0.0, y: 0.0, z: 0.0 }
);

mp.attachmentMngr.register('RobberyBox',
    'prop_box_tea01a',
    'IK_Root',
    { x: 0.0, y: 0.36, z: 0.0 },
    { x: 0.0, y: 0.0, z: 0.0 }
);

mp.attachmentMngr.register('Tablet',
    'prop_cs_tablet',
    60309,
    { x: 0.115, y: 0.001, z: 0.125 },
    { x: -150.001, y: 9.99, z: 55.001 }
);

mp.attachmentMngr.register('Guitar',
    'prop_acc_guitar_01',
    60309,
    { x: 0.015, y: 0.001, z: 0.05 },
    { x: -0.01, y: -5.009, z: 5 }
);

mp.attachmentMngr.register('Microphone',
    'p_ing_microphonel_01',
    28422,
    { x: 0.009, y: 0.0, z: 0.001 },
    { x: 4.989, y: -0.09, z: 0.0 }
);

mp.attachmentMngr.register('Camera',
    'prop_v_cam_01',
    28422,
    { x: -0.01, y: 0.001, z: 0.001 },
    { x: -0.1, y: 4.99, z: -5 }
);

mp.attachmentMngr.register('Drink',
    'ng_proc_sodacan_01a',
    'IK_R_Hand',
    { x: 0.07, y: 0.085, z: -0.02 },
    { x: 40, y: -100, z: 110 }
);

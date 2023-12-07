module.exports = {
  "1": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_ecola_can",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": true
      }
    ],
    "enter": {
      "dictionary": "mini@sprunk",
      "name": "plyr_buy_drink_pt1",
      "flag": 48,
      "start": 0.5,
      "stop": 1.0
    },
    "enterEffects": null,
    "action": {
      "dictionary": "mini@sprunk",
      "name": "plyr_buy_drink_pt2",
      "flag": 48,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "mini@sprunk",
      "name": "plyr_buy_drink_pt1",
      "flag": 50,
      "start": 0.9,
      "stop": 1.0
    },
    "exit": {
      "dictionary": "mini@sprunk",
      "name": "plyr_buy_drink_pt3",
      "flag": 48,
      "start": 0.3,
      "stop": 1.0
    },
    "isLooped": false,
    "canCancel": false
  },
  "2": {
    "sequence": [
      {
        "dictionary": "anim@safehouse@bong",
        "name": "bong_stage1",
        "flag": 34,
        "start": 0.15,
        "stop": 0.7
      }
    ],
    "attachs": [
      {
        "model": "p_cs_lighter_01",
        "boneId": 28422,
        "offset": {
          "x": 0.0025,
          "y": 0.005,
          "z": -0.0575
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      },
      {
        "model": "prop_bong_01",
        "boneId": 60309,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": [
      {
        "asset": "scr_safehouse",
        "name": "scr_sh_bong_smoke",
        "startAt": 0.5,
        "duration": 3000,
        "scale": 2.0,
        "boneId": 12844,
        "offset": {
          "x": 0.0,
          "y": 0.1515,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        }
      },
      {
        "asset": "scr_safehouse",
        "name": "scr_sh_bong_smoke",
        "startAt": 0.6,
        "duration": 3000,
        "scale": 2.0,
        "boneId": 12844,
        "offset": {
          "x": 0.0,
          "y": 0.1515,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        }
      },
      {
        "asset": "scr_safehouse",
        "name": "scr_sh_lighter_flame",
        "startAt": 0.3,
        "duration": 2300,
        "scale": 1.0,
        "boneId": 6286,
        "offset": {
          "x": 0.07,
          "y": 0.0025,
          "z": 0.095
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        }
      }
    ],
    "base": null,
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "3": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_cs_ciggy_01",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": {
      "dictionary": "AMB@WORLD_HUMAN_SMOKING@MALE@MALE_A@ENTER",
      "name": "ENTER",
      "flag": 48,
      "start": 0.0,
      "stop": 1.0
    },
    "enterEffects": [
      {
        "asset": "scr_safehouse",
        "name": "scr_sh_bong_smoke",
        "startAt": 0.5,
        "duration": 3000,
        "scale": 2.0,
        "boneId": 12844,
        "offset": {
          "x": 0.0,
          "y": 0.1515,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        }
      },
      {
        "asset": "scr_safehouse",
        "name": "scr_sh_bong_smoke",
        "startAt": 0.85,
        "duration": 3000,
        "scale": 2.0,
        "boneId": 12844,
        "offset": {
          "x": 0.0,
          "y": 0.1515,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        }
      }
    ],
    "action": {
      "dictionary": "AMB@WORLD_HUMAN_SMOKING@MALE@MALE_A@IDLE_A",
      "name": "IDLE_B",
      "flag": 48,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": [
      {
        "asset": "scr_safehouse",
        "name": "scr_sh_bong_smoke",
        "startAt": 0.47,
        "duration": 3000,
        "scale": 2.0,
        "boneId": 12844,
        "offset": {
          "x": 0.0,
          "y": 0.1515,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        }
      },
      {
        "asset": "scr_safehouse",
        "name": "scr_sh_bong_smoke",
        "startAt": 0.65,
        "duration": 5000,
        "scale": 2.5,
        "boneId": 12844,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        }
      }
    ],
    "base": {
      "dictionary": "AMB@WORLD_HUMAN_SMOKING@MALE@MALE_A@IDLE_A",
      "name": "IDLE_A",
      "flag": 50,
      "start": 0.89,
      "stop": 1.0
    },
    "exit": {
      "dictionary": "AMB@WORLD_HUMAN_SMOKING@MALE@MALE_A@EXIT",
      "name": "EXIT",
      "flag": 48,
      "start": 0.35,
      "stop": 1.0
    },
    "isLooped": false,
    "canCancel": false
  },
  "4": {
    "sequence": null,
    "attachs": [
      {
        "model": "p_cs_joint_01",
        "boneId": 6286,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "safe@franklin@ig_13",
      "name": "blunt_idle_a",
      "flag": 48,
      "start": 0.0,
      "stop": 0.55
    },
    "actionEffects": null,
    "base": {
      "dictionary": "safe@franklin@ig_13",
      "name": "blunt_idle_a",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "8": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_acc_guitar_01",
        "boneId": 60309,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "amb@world_human_musician@guitar@male@idle_a",
      "name": "idle_c",
      "flag": 48,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "amb@world_human_musician@guitar@male@base",
      "name": "base",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": true
  },
  "9": {
    "sequence": null,
    "attachs": [
      {
        "model": "p_ing_microphonel_01",
        "boneId": 6286,
        "offset": {
          "x": 0.05,
          "y": 0.06,
          "z": 0.0
        },
        "rotate": {
          "x": -72.0,
          "y": 14.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": {
      "dictionary": "missmic4premiere",
      "name": "interview_short_lazlow",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": true
  },
  "10": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_v_cam_01",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": {
      "dictionary": "missmic4premiere",
      "name": "interview_short_camman",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": true
  },
  "5": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_cs_burger_01",
        "boneId": 60309,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "mp_player_inteat@burger",
      "name": "mp_player_int_eat_burger",
      "flag": 48,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "mp_player_inteat@burger",
      "name": "mp_player_int_eat_burger_enter",
      "flag": 50,
      "start": 0.8,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "6": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_sandwich_01",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "amb@world_human_seat_wall_eating@male@both_hands@idle_a",
      "name": "idle_c",
      "flag": 48,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "amb@world_human_seat_wall_eating@male@both_hands@idle_a",
      "name": "idle_a",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "7": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_cs_hotdog_01",
        "boneId": 60309,
        "offset": {
          "x": 0.05,
          "y": 0.01,
          "z": -0.01
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 75.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "mp_player_inteat@burger",
      "name": "mp_player_int_eat_burger",
      "flag": 48,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "mp_player_inteat@burger",
      "name": "mp_player_int_eat_burger_enter",
      "flag": 50,
      "start": 0.8,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "11": {
    "sequence": null,
    "attachs": [
      {
        "model": "p_amb_clipboard_01",
        "boneId": 60309,
        "offset": {
          "x": -0.005,
          "y": 0.035,
          "z": 0.0175
        },
        "rotate": {
          "x": -84.0,
          "y": 4.0,
          "z": -12.0
        },
        "deleteBefore": false
      },
      {
        "model": "prop_pencil_01",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "anim@amb@board_room@supervising@",
      "name": "dissaproval_01_lo_amy_skater_01",
      "flag": 48,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "anim@amb@board_room@supervising@",
      "name": "dissaproval_01_lo_amy_skater_01",
      "flag": 50,
      "start": 0.8,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": true
  },
  "12": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_beer_logopen",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": -0.15
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "amb@code_human_wander_drinking@beer@male@idle_a",
      "name": "idle_c",
      "flag": 48,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "amb@code_human_wander_drinking@beer@male@base",
      "name": "static",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "13": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_binoc_01",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": {
      "dictionary": "amb@lo_res_idles@",
      "name": "world_human_binoculars_lo_res_base",
      "flag": 33,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": true
  },
  "14": {
    "sequence": [
      {
        "dictionary": "anim@arena@amb@seat_drone_tablet@male@var_a@",
        "name": "tablet_enter",
        "flag": 48,
        "start": 0.2,
        "stop": 1.0
      },
      {
        "dictionary": "anim@arena@amb@seat_drone_tablet@male@var_a@",
        "name": "tablet_idle_l",
        "flag": 49,
        "start": 0.0,
        "stop": 1.0
      }
    ],
    "attachs": [
      {
        "model": "prop_cs_tablet",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": null,
    "exit": {
      "dictionary": "anim@arena@amb@seat_drone_tablet@male@var_a@",
      "name": "tablet_exit",
      "flag": 48,
      "start": 0.0,
      "stop": 0.6
    },
    "isLooped": true,
    "canCancel": false
  },
  "15": {
    "sequence": [
      {
        "dictionary": "anim@cellphone@in_car@ds",
        "name": "cellphone_call_listen_base",
        "flag": 48,
        "start": 0.0,
        "stop": 1.0
      }
    ],
    "attachs": [
      {
        "model": "p_amb_phone_01",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": null,
    "exit": null,
    "isLooped": true,
    "canCancel": false
  },
  "16": {
    "sequence": [
      {
        "dictionary": "amb@code_human_wander_texting_fat@male@enter",
        "name": "enter",
        "flag": 50,
        "start": 0.3,
        "stop": 1.0
      },
      {
        "dictionary": "amb@code_human_wander_texting_fat@male@idle_a",
        "name": "idle_b",
        "flag": 50,
        "start": 0.0,
        "stop": 1.0
      },
      {
        "dictionary": "amb@code_human_wander_texting_fat@male@exit",
        "name": "exit",
        "flag": 50,
        "start": 0.0,
        "stop": 0.5
      }
    ],
    "attachs": [
      {
        "model": "prop_ld_health_pack",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0925,
          "z": -0.005
        },
        "rotate": {
          "x": 180.0,
          "y": 84.0,
          "z": -82.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": null,
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "17": {
    "sequence": [
      {
        "dictionary": "oddjobs@bailbond_hobotwitchy",
        "name": "base",
        "flag": 50,
        "start": 0.0,
        "stop": 1.0
      },
      {
        "dictionary": "oddjobs@bailbond_hobotwitchy",
        "name": "base",
        "flag": 50,
        "start": 0.0,
        "stop": 1.0
      },
      {
        "dictionary": "oddjobs@bailbond_hobotwitchy",
        "name": "base",
        "flag": 50,
        "start": 0.0,
        "stop": 1.0
      }
    ],
    "attachs": [
      {
        "model": "prop_gaffer_arm_bind",
        "boneId": 60309,
        "offset": {
          "x": 0.09,
          "y": 0.005,
          "z": 0.0075
        },
        "rotate": {
          "x": -67.0,
          "y": -2.0,
          "z": 114.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": null,
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "20": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_drink_whisky",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": -0.05
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "amb@world_human_drinking@coffee@male@idle_a",
      "name": "idle_b",
      "flag": 50,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "amb@world_human_drinking@coffee@male@base",
      "name": "base",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "19": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_champ_01b",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": -0.175
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": {
      "dictionary": "anim@amb@nightclub@mini@drinking@champagne_drinking@base@",
      "name": "intro",
      "flag": 50,
      "start": 0.7,
      "stop": 1.0
    },
    "enterEffects": null,
    "action": {
      "dictionary": "anim@amb@nightclub@mini@drinking@champagne_drinking@base@",
      "name": "outro",
      "flag": 50,
      "start": 0.0,
      "stop": 0.5
    },
    "actionEffects": null,
    "base": {
      "dictionary": "anim@amb@nightclub@mini@drinking@champagne_drinking@base@",
      "name": "bottle_hold_idle",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "18": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_drink_redwine",
        "boneId": 28422,
        "offset": {
          "x": 0.035,
          "y": 0.0325,
          "z": 0.0
        },
        "rotate": {
          "x": -5.0,
          "y": 1.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "anim@heists@heist_safehouse_intro@wine@window",
      "name": "wine_window_part_one",
      "flag": 50,
      "start": 0.0,
      "stop": 0.9
    },
    "actionEffects": null,
    "base": {
      "dictionary": "anim@heists@heist_safehouse_intro@wine@window",
      "name": "wine_window_idle",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "21": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_tequsunrise",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": -0.1275
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "amb@world_human_drinking@coffee@male@idle_a",
      "name": "idle_b",
      "flag": 50,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "amb@world_human_drinking@coffee@male@base",
      "name": "base",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "22": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_tequila",
        "boneId": 28422,
        "offset": {
          "x": 0.005,
          "y": 0.01,
          "z": -0.0225
        },
        "rotate": {
          "x": -14.0,
          "y": -7.0,
          "z": -88.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "anim@heists@heist_safehouse_intro@wine@window",
      "name": "wine_window_part_one",
      "flag": 50,
      "start": 0.0,
      "stop": 0.9
    },
    "actionEffects": null,
    "base": {
      "dictionary": "anim@heists@heist_safehouse_intro@wine@window",
      "name": "wine_window_idle",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "23": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_drink_whtwine",
        "boneId": 28422,
        "offset": {
          "x": 0.025,
          "y": 0.015,
          "z": 0.0
        },
        "rotate": {
          "x": -9.0,
          "y": 6.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "anim@heists@heist_safehouse_intro@wine@window",
      "name": "wine_window_part_one",
      "flag": 50,
      "start": 0.0,
      "stop": 0.9
    },
    "actionEffects": null,
    "base": {
      "dictionary": "anim@heists@heist_safehouse_intro@wine@window",
      "name": "wine_window_idle",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "24": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_daiquiri",
        "boneId": 28422,
        "offset": {
          "x": 0.005,
          "y": 0.01,
          "z": -0.0225
        },
        "rotate": {
          "x": -14.0,
          "y": -7.0,
          "z": -88.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "anim@heists@heist_safehouse_intro@wine@window",
      "name": "wine_window_part_one",
      "flag": 50,
      "start": 0.0,
      "stop": 0.9
    },
    "actionEffects": null,
    "base": {
      "dictionary": "anim@heists@heist_safehouse_intro@wine@window",
      "name": "wine_window_idle",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "25": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_mojito",
        "boneId": 28422,
        "offset": {
          "x": 0.0,
          "y": 0.0,
          "z": -0.14
        },
        "rotate": {
          "x": 0.0,
          "y": 0.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "amb@world_human_drinking@coffee@male@idle_a",
      "name": "idle_b",
      "flag": 50,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "amb@world_human_drinking@coffee@male@base",
      "name": "base",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "26": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_shot_glass",
        "boneId": 6286,
        "offset": {
          "x": 0.0675,
          "y": -0.0025,
          "z": -0.0525
        },
        "rotate": {
          "x": -67.0,
          "y": 1.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "anim@amb@nightclub@mini@drinking@drinking_shots@ped_c@normal",
      "name": "drink",
      "flag": 50,
      "start": 0.0,
      "stop": 0.4
    },
    "actionEffects": null,
    "base": {
      "dictionary": "anim@amb@nightclub@mini@drinking@drinking_shots@ped_c@normal",
      "name": "glass_hold",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "27": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_rum_bottle",
        "boneId": 60309,
        "offset": {
          "x": -0.03,
          "y": -0.03,
          "z": -0.1875
        },
        "rotate": {
          "x": -5.0,
          "y": 16.0,
          "z": -1.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "anim@amb@casino@hangout@ped_female@stand_withdrink@01a@idles",
      "name": "idle_a",
      "flag": 50,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "anim@amb@casino@hangout@ped_female@stand_withdrink@01a@base",
      "name": "base",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "28": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_rum_bottle",
        "boneId": 60309,
        "offset": {
          "x": 0.0025,
          "y": -0.005,
          "z": -0.225
        },
        "rotate": {
          "x": 2.0,
          "y": 7.0,
          "z": 1.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "anim@amb@casino@hangout@ped_female@stand_withdrink@01a@idles",
      "name": "idle_a",
      "flag": 50,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "anim@amb@casino@hangout@ped_female@stand_withdrink@01a@base",
      "name": "base",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "29": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_tequila_bottle",
        "boneId": 60309,
        "offset": {
          "x": -0.0275,
          "y": -0.0075,
          "z": -0.28
        },
        "rotate": {
          "x": -1.0,
          "y": 10.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "anim@amb@casino@hangout@ped_female@stand_withdrink@01a@idles",
      "name": "idle_a",
      "flag": 50,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "anim@amb@casino@hangout@ped_female@stand_withdrink@01a@base",
      "name": "base",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "30": {
    "sequence": null,
    "attachs": [
      {
        "model": "p_cs_papers_01",
        "boneId": 6286,
        "offset": {
          "x": 0.0375,
          "y": 0.005,
          "z": -0.06
        },
        "rotate": {
          "x": -150.0,
          "y": -20.0,
          "z": 0.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "amb@world_human_gardener_plant@female@idle_a",
      "name": "idle_a_female",
      "flag": 34,
      "start": 0.0,
      "stop": 0.5
    },
    "actionEffects": null,
    "base": {
      "dictionary": "special_ped@griff@trevor_1@trevor_1a",
      "name": "convo_trevor_whatareyoudoing_0",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "31": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_wateringcan",
        "boneId": 6286,
        "offset": {
          "x": 0.2225,
          "y": -0.19,
          "z": -0.145
        },
        "rotate": {
          "x": -178.0,
          "y": -60.0,
          "z": -66.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "weapon@w_sp_jerrycan",
      "name": "fire",
      "flag": 50,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "switch@trevor@digging",
      "name": "001433_01_trvs_26_digging_idle",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "32": {
    "sequence": null,
    "attachs": [
      {
        "model": "prop_cs_sack_01",
        "boneId": 6286,
        "offset": {
          "x": 0.02,
          "y": -0.065,
          "z": -0.1225
        },
        "rotate": {
          "x": 166.0,
          "y": -276.0,
          "z": 41.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "amb@world_human_gardener_plant@female@idle_a",
      "name": "idle_a_female",
      "flag": 50,
      "start": 0.0,
      "stop": 0.1
    },
    "actionEffects": null,
    "base": {
      "dictionary": "special_ped@griff@trevor_1@trevor_1a",
      "name": "convo_trevor_whatareyoudoing_0",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "33": {
    "sequence": null,
    "attachs": [
      {
        "model": "ng_proc_crate_03a",
        "boneId": 6286,
        "offset": {
          "x": 0.1125,
          "y": -0.365,
          "z": -0.215
        },
        "rotate": {
          "x": -65.0,
          "y": -28.0,
          "z": -31.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "anim@heists@money_grab@briefcase",
      "name": "put_down_case",
      "flag": 50,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "gestures@m@car@truck@casual@ds",
      "name": "gesture_shrug_soft",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "34": {
    "sequence": [
      {
        "dictionary": "amb@world_human_gardener_plant@female@idle_a",
        "name": "idle_a_female",
        "flag": 34,
        "start": 0.0,
        "stop": 0.6
      }
    ],
    "attachs": null,
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": null,
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "35": {
    "sequence": [
      {
        "dictionary": "anim@amb@business@weed@weed_inspecting_lo_med_hi@",
        "name": "weed_crouch_checkingleaves_idle_01_inspector",
        "flag": 34,
        "start": 0.0,
        "stop": 1.0
      }
    ],
    "attachs": null,
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": null,
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "36": {
    "sequence": [
      {
        "dictionary": "anim@narcotics@trash",
        "name": "drop_front",
        "flag": 34,
        "start": 0.0,
        "stop": 1.0
      }
    ],
    "attachs": null,
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": null,
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "37": {
    "sequence": null,
    "attachs": [
      {
        "model": "stt_prop_c4_stack",
        "boneId": 64097,
        "offset": {
          "x": 0.1375,
          "y": -0.075,
          "z": -0.035
        },
        "rotate": {
          "x": -27.02,
          "y": -8.02,
          "z": -56.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": {
      "dictionary": "weapons@first_person@aim_rng@generic@projectile@thermal_charge@",
      "name": "plant_floor",
      "flag": 34,
      "start": 0.0,
      "stop": 1.0
    },
    "actionEffects": null,
    "base": {
      "dictionary": "gestures@m@car@truck@casual@ds",
      "name": "gesture_shrug_soft",
      "flag": 49,
      "start": 0.0,
      "stop": 1.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  },
  "38": {
    "sequence": null,
    "attachs": [
      {
        "model": "w_am_digiscanner",
        "boneId": 6286,
        "offset": {
          "x": 0.115,
          "y": 0.0625,
          "z": -0.0075
        },
        "rotate": {
          "x": -74.02,
          "y": -54.02,
          "z": -120.0
        },
        "deleteBefore": false
      }
    ],
    "enter": null,
    "enterEffects": null,
    "action": null,
    "actionEffects": null,
    "base": {
      "dictionary": "move_characters@sandy@texting",
      "name": "sandy_text_loop_base",
      "flag": 49,
      "start": 0.0,
      "stop": 0.0
    },
    "exit": null,
    "isLooped": false,
    "canCancel": false
  }
}
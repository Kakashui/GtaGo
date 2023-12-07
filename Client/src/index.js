require('./client/global');
require('./client/keys');
require('./client/camera');
require('./client/gui');
require('./client/actionHandler');
require('./client/chat');
require('./client/common');
require('./client/customSync');
require('./client/doors');
require('./client/render');
require('./client/hud');
require('./client/gametags');
require('./client/globalMenu');
require('./client/customization/Customization');  

require('./client/voice');
require('./client/main');
require('./client/bizsettings');
require('./client/checkpoints');
require('./client/inventory');

require('./client/shops/furnitureStore');
require('./client/vehiclesync');
require('./client/gangzones');
require('./client/enviroment');
require('./client/radiosync');
require('./client/speedcheck');
require('./client/fishing');
require('./client/casino');

require('./client/marriage'); 
require('./client/fingerpointing');
require('./client/judge');
require('./client/lscustomsNew');
require('./client/deathScreen');
require('./client/arena/index')
require('./client/technicianWork')
require('./client/carThiefWork')
require('./client/transporteur')

require('./client/configs');
require('./client/clothing');
require('./client/props');
require('./client/businesses/index');
require('./client/works/index');
require('./client/pets/index');

require('./client/farm');
require('./client/busWaysCreator');
require('./client/houses');
require('./client/interactionShapes/index');
require('./client/captureUI');
require('./client/farms/index');
require('./client/fractions');
require('./client/fractions/index');
require('./client/cigarettes');
require('./client/lifeActivity/index');
require('./client/admins');
require('./client/school');
require('./client/autoRepair');
require('./client/greenscreen');
require('./client/ui/index');
require('./client/animationsMenu');
require('./client/weaponSystem');
require('./client/fastAccess')
require('./client/illegalShop');
require('./client/carRoom');
require('./client/startQuest/index');
require('./client/phone/index');
require('./client/mainMenu');
require('./client/personalDigitalAssistant');
require('./client/lsnews');
require('./client/sound');
require('./client/families/family');
require('./client/families/updateData');
require('./client/families/cefEvents');
require('./client/families/syncMemberBlip');
require('./client/families/familyBattle');

require('./client/cityhall/menu');
require('./client/cityhall/cefEvents');
require('./client/cityhall/updateData');
require('./client/royalBattle/index');

require('./client/reports/admin');
require('./client/reports/player');
require('./client/reports/transferMoney');

require('./client/lift');
require('./client/parliament');
require('./client/antiafk');
require('./client/docs');
require('./client/vehicleTrading');
require('./client/zoneSystem/zones');
require('./client/islandCapture');
require('./client/vehicleRent');
require('./client/moneySystem/index');
require('./client/priceMenu');
require('./client/personalEvents');

require('./client/customColShapes');
require('./client/steelMaking');

//shops
require('./client/shops/tattoShop');
require('./client/shops/barberShop');
require('./client/shops/clothisngShop');
require('./client/shops/maskShop');
require('./client/shops/weaponShop');
require('./client/shops/shop24');
require('./client/shops/burgerShot');
require('./client/shops/alcoBar');
require('./client/shops/carWash');
require('./client/newDonateShop');
require('./client/shops/handlingModShop');


require('./client/docks/dock');
require('./client/docks/dockLoaderJob');
require('./client/questPeds');
require('./client/interactionMenu');
require('./client/authorization');
require('./client/world');
require('./client/anticheat/index');
//require('./client/inventory/items/itemsAnimator');
require('./client/tablet');
require('./client/tip');
require('./client/miniGames');
require('./client/scenes');
require('./client/binocular');
require('./client/interiorsCheck');
require('./client/weedFarm');
require('./client/hudQuestMessage');
require('./client/bigInfo');
require('./client/costumeMenu');

global.effectManager = require('./client/EffectManager');
global.controlsManager = require('./client/ControlsManager');
global.enviromentManager = require('./client/EnviromentActions');

if(global.debug) require('./client/malboro');
//require('./client/malboro/parseValidClothes');
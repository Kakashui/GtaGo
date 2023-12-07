const SerializeBase = 16;

// by ragempdev
// https://rage.mp/files/file/144-efficient-attachment-sync/

function checkEntityAttachment(entity) {
	if (!entity.__attachmentObjects) {
		entity.__attachmentObjects = {};
	}
}

mp.attachmentMngr = 
{
	attachments: {},
	
	addFor: async function(entity, id)
	{
		if(this.attachments.hasOwnProperty(id))
		{
			checkEntityAttachment(entity);

			if(!entity.__attachmentObjects.hasOwnProperty(id))
			{
				let attInfo = this.attachments[id];
				
				let object = mp.objects.new(attInfo.model, entity.position, {dimension: entity.dimension || 0});
				while (!object.doesExist()) {
					await mp.game.waitAsync(0);
				}
				object.setCollision(false, false);
				object.attachTo(entity.handle,
					(typeof(attInfo.boneName) === 'string') ? entity.getBoneIndexByName(attInfo.boneName) : entity.getBoneIndex(attInfo.boneName),
					attInfo.offset.x, attInfo.offset.y, attInfo.offset.z, 
					attInfo.rotation.x, attInfo.rotation.y, attInfo.rotation.z, 
					false, false, false, false, 2, true);
				// object.setCollision(false, false);
					
				entity.__attachmentObjects[id] = object;
			}
		}
		else
		{
			mp.game.graphics.notify(`Static Attachments Error: ~r~Unknown Attachment Used: ~w~0x${id.toString(16)}`);
		}
	},
	
	removeFor: function(entity, id)
	{
		checkEntityAttachment(entity);

		if(entity.__attachmentObjects.hasOwnProperty(id))
		{
			let obj = entity.__attachmentObjects[id];
			delete entity.__attachmentObjects[id];
			obj.destroy();
		}
	},
	
	initFor: function(entity)
	{
		for(let attachment of entity.__attachments)
		{
			mp.attachmentMngr.addFor(entity, attachment);
		}
	},
	
	shutdownFor: function(entity)
	{
		checkEntityAttachment(entity);
		
		for(let attachment in entity.__attachmentObjects)
		{
			mp.attachmentMngr.removeFor(entity, attachment);
		}
	},
	
	register: function(id, model, boneName, offset, rotation)
	{
		if(typeof(id) === 'string')
		{
			id = mp.game.joaat(id);
		}
		
		if(typeof(model) === 'string')
		{
			model = mp.game.joaat(model);
		}
		
		if(!this.attachments.hasOwnProperty(id))
		{
			if(mp.game.streaming.isModelInCdimage(model))
			{
				this.attachments[id] =
				{
					id: id,
					model: model,
					offset: offset,
					rotation: rotation,
					boneName: boneName
				};
			}
			else
			{
				mp.game.graphics.notify(`Static Attachments Error: ~r~Invalid Model (0x${model.toString(16)})`);
			}
		}
		else
		{
			mp.game.graphics.notify("Static Attachments Error: ~r~Duplicate Entry");
		}
	},
	
	unregister: function(id) 
	{
		if(typeof(id) === 'string')
		{
			id = mp.game.joaat(id);
		}
		
		if(this.attachments.hasOwnProperty(id))
		{
			delete this.attachments[id];
		}
	},
	
	addLocal: function(attachmentName)
	{
		if(typeof(attachmentName) === 'string')
		{
			attachmentName = mp.game.joaat(attachmentName);
		}
		
		let entity = mp.players.local;
		
		if(!entity.__attachments || entity.__attachments.indexOf(attachmentName) === -1)
		{
			mp.events.callRemote("staticAttachments.Add", attachmentName.toString(SerializeBase));
		}
	},
	
	removeLocal: function(attachmentName)
	{
		if(typeof(attachmentName) === 'string')
		{
			attachmentName = mp.game.joaat(attachmentName);
		}
		
		let entity = mp.players.local;
		
		if(entity.__attachments && entity.__attachments.indexOf(attachmentName) !== -1)
		{
			mp.events.callRemote("staticAttachments.Remove", attachmentName.toString(SerializeBase));
		}
	},
	
	getAttachments: function()
	{
		return Object.assign({}, this.attachments);
	}
};

mp.events.add("entityStreamIn", (entity) =>
{
	try {
		
		if(entity.__attachments)
		{
			mp.attachmentMngr.initFor(entity);
		}
	} catch (e) {
		if(global.sendException) mp.serverLog(`static-attachments.entityStreamIn: ${e.name}\n${e.message}\n${e.stack}`);
	}
});

mp.events.add("entityStreamOut", (entity) =>
{
	try {		
		if(entity.__attachmentObjects)
		{
			mp.attachmentMngr.shutdownFor(entity);
		}
	} catch (e) {		
		if(global.sendException) mp.serverLog(`static-attachments.entityStreamOut: ${e.name}\n${e.message}\n${e.stack}`);
	}
});


mp.events.add("onChangeDimension", (oldDim, newDim) =>
{
	try {		
		if(mp.players.local.__attachmentObjects)
		{
			mp.attachmentMngr.shutdownFor(mp.players.local);
			mp.attachmentMngr.initFor(mp.players.local);
		}
	} catch (e) {		
		if(global.sendException) mp.serverLog(`static-attachments.entityStreamOut: ${e.name}\n${e.message}\n${e.stack}`);
	}
});

mp.events.addDataHandler("attachmentsData", (entity, data) =>
{
	try {
		if (!entity || entity.type !== 'player' || !mp.players.exists(entity)) return;
		
		let newAttachments = (data.length > 0) ? data.split('|').map(att => parseInt(att, SerializeBase)) : [];

		if(entity.handle !== 0)
		{
			let oldAttachments = entity.__attachments;	
			
			if(!oldAttachments)
			{
				oldAttachments = [];
				entity.__attachmentObjects = {};
			}
			
			// process outdated first
			for(let attachment of oldAttachments)
			{
				if(newAttachments.indexOf(attachment) === -1)
				{
					mp.attachmentMngr.removeFor(entity, attachment);
				}
			}
			
			// then new attachments
			for(let attachment of newAttachments)
			{
				if(oldAttachments.indexOf(attachment) === -1)
				{
					mp.attachmentMngr.addFor(entity, attachment);
				}
			}
		}

		entity.__attachments = newAttachments;
	} catch (e) {
		if(global.sendException) mp.serverLog(`static-attachments.attachmentsData: ${e.name}\n${e.message}\n${e.stack}`);		
	}
});

function InitAttachmentsOnJoin()
{
	mp.players.forEach(_player =>
	{
		let data = _player.getVariable("attachmentsData");
		
		if(data && data.length > 0)
		{
			let atts = data.split('|').map(att => parseInt(att, SerializeBase));
			_player.__attachments = atts;
			_player.__attachmentObjects = {};
		}
	});
}

InitAttachmentsOnJoin();
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Fishing
{
	public enum AnimationFlags
	{
		Loop = 1 << 0,
		StopOnLastFrame = 1 << 1,
		OnlyAnimateUpperBody = 1 << 4,
		AllowPlayerControl = 1 << 5,
		Cancellable = 1 << 7
	};

	public enum FishingActions
	{
		NoAction = -1,
		GearShop = 0,
		FishShop = 1,
		FishSpot = 2
	}


	public enum Fish
	{
		Herring = 0,
		Bass = 1,
		Eel = 2,
		Pike = 3,
		Sterlet = 4,
		Salmon = 5,
		Sturgeon = 6,
		Amur = 7,
		Stingray = 8,
		Tuna = 9,
		Trout = 10,
		PerfectHerring = 11,
		PerfectBass = 12,
		PerfectEel = 13,
		PerfectPike = 14,
		PerfectSterlet = 15,
		PerfectSalmon = 16,
		PerfectSturgeon = 17,
		PerfectAmur = 18,
		PerfectStingray = 19,
		PerfectTuna = 20,
		PerfectTrout = 21,
		GoldFish = 22
	}
}

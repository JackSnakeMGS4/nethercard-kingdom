﻿/**
 * Description: Enums used in the game.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

public enum ConflicSide
{
	Player,
	Enemy,
	All
}

public enum CardType
{
	Unit,
	DirectOffensiveSpell,
	DirectDefensiveSpell,
	AoeSpell,
	None
}

public enum CardSelectionMode
{
	InHand,
	InCollection,
	InDeck,
	InUpgrade
}

public enum CardLevel
{
	Level1,
	Level2,
	Level3
}

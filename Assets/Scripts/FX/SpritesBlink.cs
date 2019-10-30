﻿/**
 * Description: Blinks sprites, for example in response to taken damage.
 * Authors: Kornel
 * Copyright: © 2018-2019 Kornel. All rights reserved. For license see: 'LICENSE' file.
 **/

using UnityEngine;
using UnityEngine.Assertions;

enum BlinkMode
{
	Change,
	Hide
}

public class SpritesBlink : MonoBehaviour
{
	[Header("External objects")]
	[SerializeField] private SpriteRenderer[] spritesToBlink = null;
	[SerializeField] private Material normalMaterial = null;
	[SerializeField] private Material blinkMaterial = null;
	[SerializeField] private int blinkAmount = 1;
	[SerializeField] private BlinkMode mode = BlinkMode.Change;
	[SerializeField] private bool changeColor = false;
	[SerializeField] private Color blinkColor = Color.red;

	[Header("Tweakable")]
	[SerializeField] private float blinkTime = 0.1f;

	private Color originalColor;
	private bool blinking = false;
	private int blinksLeft = 0;

	void Start( )
	{
		if ( spritesToBlink.Length == 0 )
			spritesToBlink = GetComponentsInChildren<SpriteRenderer>( );

		Assert.IsNotNull( spritesToBlink, "You need to add sprites." );
		Assert.AreNotEqual( spritesToBlink.Length, 0, "You need to add sprites." );

		if ( mode == BlinkMode.Change )
		{
			Assert.IsNotNull( normalMaterial, "Normal material can not be empty." );
			Assert.IsNotNull( blinkMaterial, "Blink material can not be empty." );
		}

		if ( changeColor )
			originalColor = spritesToBlink[0].color;
	}

	/// <summary>
	/// Do a blink.
	/// </summary>
	public void Blink( )
	{
		blinksLeft = blinkAmount;
		Blink( blinkTime );
	}

	private void ReBlink( )
	{
		Blink( blinkTime );
	}

	/// <summary>
	/// Do a blink and overwrite the blink time from the inspector.
	/// </summary>
	/// <param name="time">Time the blink will last.</param>
	public void Blink( float time )
	{
		if ( blinking )
			return;

		blinking = true;
		blinksLeft--;

		if ( mode == BlinkMode.Change )
			SwapMaterial( blinkMaterial, blinkColor );
		else
			Hide( true );

		Invoke( "Unblink", time );
	}

	private void Unblink( )
	{
		blinking = false;

		if ( mode == BlinkMode.Change )
			SwapMaterial( normalMaterial, originalColor );
		else
			Hide( false );

		if ( blinksLeft > 0 )
			Invoke( "ReBlink", blinkTime );
	}

	private void SwapMaterial( Material material, Color color )
	{
		foreach ( var sprite in spritesToBlink )
		{
			sprite.material = material;
			if ( changeColor )
				sprite.color = color;
		}
	}

	private void Hide( bool state )
	{
		foreach ( var sprite in spritesToBlink )
			sprite.enabled = !state;
	}
}

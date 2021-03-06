﻿/**
 * Description: Manages summoning of instances.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SummoningManager : MonoBehaviour
{
	public static SummoningManager Instance { get; private set; }

	public CardType SummoningCardType { get; private set; } = CardType.Undefined;
	public Targetable LastTarget { get; private set; } = null;
	public bool CanSummon { get; set; } = true;
	public bool TrashCard { get; set; } = false;

	[Header("Objects")]
	[SerializeField] private PlaySound manaSound = null;
	[SerializeField] private TextMeshProUGUI manaCounter = null;
	[SerializeField] private Image manaImage = null;

	[Header("Areas")]
	[SerializeField] private GameObject summoningAreaUnits = null;
	[SerializeField] private GameObject summoningAreaAoe = null;

	[Header("Cursor")]
	[SerializeField] private GameObject good = null;
	[SerializeField] private GameObject bad = null;

	[Header("Mana")]
	[SerializeField] private float manaIncrease = 1f;
	[SerializeField] private int startMana = 0;
	[SerializeField] private int maxMana = 100;

	private List<Targetable> targetables = new List<Targetable>();
	private CardType currentSummoningType = CardType.Undefined;
	private bool overValidTarget = false;
	private float currentMana = 0;

	private void Awake( )
	{
		if ( Instance != null && Instance != this )
			Destroy( this );
		else
			Instance = this;
	}

	private void OnDestroy( ) { if ( this == Instance ) { Instance = null; } }

	void Start ()
	{
		Assert.IsNotNull( manaSound, $"Please assign <b>{nameof( manaSound )}</b> field on <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		Assert.IsNotNull( manaCounter, $"Please assign <b>{nameof( manaCounter )}</b> field on <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		Assert.IsNotNull( manaImage, $"Please assign <b>{nameof( manaImage )}</b> field on <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		Assert.IsNotNull( summoningAreaUnits, $"Please assign <b>{nameof( summoningAreaUnits )}</b> field on <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		Assert.IsNotNull( summoningAreaAoe, $"Please assign <b>{nameof( summoningAreaAoe )}</b> field on <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		Assert.IsNotNull( good, $"Please assign <b>{nameof( good )}</b> field on <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );
		Assert.IsNotNull( bad, $"Please assign <b>{nameof( bad )}</b> field on <b>{GetType( ).Name}</b> script on <b>{name}</b> object" );

		AddMana( startMana, false );
	}

	void Update ()
	{
		UpdareIndicatorsPos( );

		if ( !CanSummon )
			return;

		ManaProgress( );
	}

	private void UpdareIndicatorsPos( )
	{
		Vector2 endPoint = Camera.main.ScreenToWorldPoint( Input.mousePosition );

		good.transform.position = endPoint;
		bad.transform.position = endPoint;
	}

	public void AddTargetable( Targetable targetable )
	{
		targetables.Add( targetable );
		targetable.SetActiveState( currentSummoningType );
	}

	public void RemoveTargetable( Targetable targetable ) => targetables.Remove( targetable );

	public bool Summoning( CardType type, bool started )
	{
		currentSummoningType = started ? type : CardType.Undefined;
		foreach ( var t in targetables )
			t.SetActiveState( currentSummoningType );

		if ( !CanSummon )
		{
			bad.SetActive( started );
			good.SetActive( false );

			return false;
		}

		if ( type == CardType.Unit )
			summoningAreaUnits.SetActive( started );

		if ( type == CardType.AoeSpell )
			summoningAreaAoe.SetActive( started );

		SummoningCardType = started ? type : CardType.Undefined;
		bad.SetActive( started );

		if ( !started )
		{
			good.SetActive( false );
			bad.SetActive( false );
		}

		if ( !started && overValidTarget )
		{
			overValidTarget = false;
			return true;
		}

		return false;
	}

	public bool EnoughMana( int amount ) => currentMana >= amount;

	public void AddMana( float amount, bool playSound = true )
	{
		//if ( playSound )
			//manaSound.Play( );

		currentMana += amount;
		currentMana = currentMana < maxMana ? currentMana : maxMana;

		manaCounter.text = currentMana.ToString( "0" );
		manaImage.fillAmount = currentMana / maxMana;
	}

	public void RemoveMana( int amount )
	{
		currentMana -= amount;
		manaCounter.text = currentMana.ToString( "0" );
	}

	public void MouseOverTarget( Targetable target, CardType targetableBy, bool isOver, bool toTrash = false )
	{
		if ( !CanSummon )
			return;

		TrashCard = toTrash;
		LastTarget = target;

		if ( targetableBy.HasFlag( SummoningCardType ) )
		{
			overValidTarget = isOver;
			good.SetActive( isOver );
			bad.SetActive( !isOver );

			return;
		}
		else if ( SummoningCardType != CardType.Undefined )
		{
			overValidTarget = false;
			good.SetActive( false );
			bad.SetActive( true );
		}

		overValidTarget = false;
		good.SetActive( false );
		bad.SetActive( false );
	}

	private void ManaProgress( )
	{
		AddMana( manaIncrease * Time.deltaTime );
	}
}

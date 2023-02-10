using System;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : Singleton<GameAssets>
{
	[field: SerializeField] public GameObject FollowersPF { get; private set; }

	protected override void Awake()
	{
		base.Awake();
	}
}
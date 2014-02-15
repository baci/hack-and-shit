using UnityEngine;
using System.Collections;

public class Virus : File
{
	private bool directionReversed = false;
	public bool DirectionReversed { get {return directionReversed;} }

	protected override void Start ()
	{
		base.Start ();

		GameTime.Instance.OnGameEnded += OnGameEnded;
	}

	protected override void Update ()
	{
		base.Update ();

		if(Target == null)
		{
			if(mPrevNode == null)
				DestroyJuicyVirus();
			else
				DestroyJuicy();
		}
	}

	private void OnGameEnded()
	{
		DestroyJuicyVirus();
	}

	public void DestroyJuicyVirus()
	{
		// destroy for realzz
		GameTime.Instance.OnGameEnded -= OnGameEnded;
		Destroy(gameObject);
	}

	public override void DestroyJuicy(bool willGivePoints = true)
	{
		// virus cries: NO!!!
		// I'm just gonna do a juicy direction change here huehuehue

		directionReversed = !directionReversed;
		if(mPrevNode != null)
			mPrevNode.HandleFile(this, Target);
		else if(Target != null)
			Target.HandleFile(this, mPrevNode);
	}
}


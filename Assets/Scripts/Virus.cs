using UnityEngine;
using System.Collections;

public class Virus : File
{
	private bool directionReversed = false;
	public bool DirectionReversed { get {return directionReversed;} }

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

	public void DestroyJuicyVirus()
	{
		// destroy for realzz
		Destroy(gameObject);
	}

	public override void DestroyJuicy()
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


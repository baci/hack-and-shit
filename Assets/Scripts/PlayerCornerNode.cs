using UnityEngine;
using System.Collections;

public class PlayerCornerNode : NetworkNode {

	private int curFilesInRow = 0;
	public Transform pointsDisplay;
	public float pointFlyingSpeed;

	override public void RecieveFile(File aFile, NetworkNode aFromNode)
    {
    }

    override public void HandleFile(File aFile, NetworkNode aFromNode)
    {
        if(aFile is Virus)
		{
			transform.parent.GetComponent<PlayerCorner>().RemoveScore(10);
			(aFile as Virus).DestroyJuicyVirus();
		}
		else if (aFile.DidPoint == false)
		{
			aFile.DidPoint = true;
			transform.parent.GetComponent<PlayerCorner>().AddScore(1);
			StartCoroutine(DoPointEffect(aFile));
		}
    }

	private IEnumerator DoPointEffect(File file)
	{
		curFilesInRow++;

		Hashtable ht = new Hashtable();
		ht.Add("position", pointsDisplay.position);
		ht.Add("time", pointFlyingSpeed);
		ht.Add("EaseType", "easeInQuad");
		iTween.MoveTo(file.gameObject, ht);
		yield return new WaitForSeconds(pointFlyingSpeed);
		file.DestroyJuicy(); 

		curFilesInRow--;
	}
}

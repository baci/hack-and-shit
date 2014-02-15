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
		else
		{
			transform.parent.GetComponent<PlayerCorner>().AddScore(1);
			StartCoroutine(DoPointEffect(aFile));
		}
    }

	private IEnumerator DoPointEffect(File file)
	{
		curFilesInRow++;

		iTween.MoveTo(file.gameObject, pointsDisplay.position, pointFlyingSpeed);
		yield return new WaitForSeconds(pointFlyingSpeed);
		file.DestroyJuicy(); 

		curFilesInRow--;
	}
}

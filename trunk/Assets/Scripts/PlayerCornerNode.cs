using UnityEngine;
using System.Collections;

public class PlayerCornerNode : NetworkNode {

	private int curFilesInRow = 0;
	public Transform pointsDisplay;
	public float pointFlyingSpeed;
	public float pointRotationFactor = 50;
	public float pointScaleFactor = 3;

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
		iTween.ScaleTo(file.gameObject, file.transform.localScale*pointScaleFactor, pointFlyingSpeed);
		iTween.ColorTo(file.gameObject, new Color(1,1,1,0), pointFlyingSpeed/3);
		iTween.RotateBy(file.gameObject, new Vector3(0,0,Random.Range(-pointRotationFactor,pointRotationFactor)), 
		                									pointFlyingSpeed);

		yield return new WaitForSeconds(pointFlyingSpeed);
		file.DestroyJuicy(); 

		curFilesInRow--;
	}
}

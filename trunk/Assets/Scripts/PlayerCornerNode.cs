using UnityEngine;
using System.Collections;

public class PlayerCornerNode : NetworkNode {

	private int curFilesInRow = 0;
	public float cooldownTime;

	public Transform pointsDisplay;
	public float pointFlyingSpeed;
	public float pointRotationFactor = 50;
	public float pointScaleFactor = 3;

	Color mOriginalColor;
	public AudioClip comboFx;

    protected override void Start()
    {
        base.Start();
        mOriginalColor = renderer.material.color;
    }

	override public void RecieveFile(File aFile, NetworkNode aFromNode)
    {
        if (aFile is Virus)
        {
            transform.parent.GetComponent<PlayerCorner>().RemoveScore(10);
            (aFile as Virus).DestroyJuicyVirus();

            curFilesInRow = 0;

            DoVirusEffect(aFile as Virus);
        }
        else if (aFile.DidPoint == false)
        {
            aFile.DidPoint = true;
            StartCoroutine(DoPointEffect(aFile));
        }
    }

    override public void HandleFile(File aFile, NetworkNode aFromNode)
    {
    }

	private IEnumerator DoPointEffect(File file)
	{
		AudioController.instance.PlaySfx(comboFx);

        var args = new Hashtable(){
				{"scale", Vector3.one*1.05f},
				{"time", 0.15f},
				{"easetype", "easeOutQuad"}
			};
        iTween.ScaleTo(transform.parent.gameObject, args);
        args = new Hashtable(){
				{"scale", Vector3.one},
				{"time", 0.15f},
				{"delay", 0.15f},
				{"easetype", "easeOutQuad"}
			};
        iTween.ScaleTo(transform.parent.gameObject, args);

		curFilesInRow++;

		transform.parent.GetComponent<PlayerCorner>().AddScore(1);
		if(curFilesInRow > 3)
		{
			transform.parent.GetComponent<PlayerCorner>().AddScore(2);
			iTween.ShakePosition(Game.Instance.gameObject, new Vector3(0.15f,0.15f,0.15f), 1f);
		}
		if(curFilesInRow > 5)
		{
			transform.parent.GetComponent<PlayerCorner>().AddScore(2);
			iTween.ShakePosition(Game.Instance.gameObject, new Vector3(0.7f,0.7f,0.7f), 1f);
		}

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
		file.DestroyJuicy(true, curFilesInRow); 

		yield return new WaitForSeconds(3f);
		curFilesInRow--;
	}


    private void DoVirusEffect(Virus aVirus)
    {
        var particles = aVirus.transform.FindChild("VirusAttack");
        particles.parent = null;
        particles.position = aVirus.transform.position;
        particles.up = Vector3.Normalize(transform.position - particles.position);
        particles.active = true;

        iTween.ShakePosition(transform.parent.gameObject, new Vector3(0.2f, 0.2f, 0), 0.5f);
        var args = new Hashtable(){
				{"scale", Vector3.one*1.05f},
				{"time", 0.15f},
				{"easetype", "easeOutQuad"}
			};
        iTween.ScaleTo(transform.parent.gameObject, args);
        args = new Hashtable(){
				{"scale", Vector3.one},
				{"time", 0.15f},
				{"delay", 0.15f},
				{"easetype", "easeOutQuad"}
			};
        iTween.ScaleTo(transform.parent.gameObject, args);

        args = new Hashtable(){
				{"color", Color.red * 0.75f},
				{"time", 0.3f},
				{"easetype", "easeOutQuad"}
			};
        iTween.FadeTo(transform.parent.gameObject, args);

        args = new Hashtable(){
				{"color", mOriginalColor},
				{"time", 0.7f},
				{"delay", 0.3f},
				{"easetype", "easeOutQuad"}
			};
        iTween.FadeTo(transform.parent.gameObject, args);
    }
}

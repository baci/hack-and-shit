using UnityEngine;
using System.Collections;

public abstract class NetworkNode : MonoBehaviour 
{
    protected enum ConnectionDir
    {
        up,
        down,
        left,
        right,
    }

	// Use this for initialization
	protected virtual void Start () 
    {
		GameTime.Instance.OnGameEnded += OnGameEnded;
	}
	
	private void OnGameEnded()
	{
		GameTime.Instance.OnGameEnded -= OnGameEnded;
		GetComponent<Touchable>().onTouchBegin = null;
		GetComponent<Touchable>().onTouchEnd = null;
		GetComponent<Touchable>().onTouchMove = null;
	}

    abstract public void RecieveFile(File aFile, NetworkNode aFromNode);
    abstract public void HandleFile(File aFile, NetworkNode aFromNode);
}

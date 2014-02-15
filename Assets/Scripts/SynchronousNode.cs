using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SynchronousNode : MonoBehaviour {

	public List<SynchronousNode> otherNodes;

	private void Start () 
	{
		GetComponent<Touchable>().onTouchBegin += OnTouchBegin;
		GetComponent<Touchable>().onTouchMove += OnTouchMove;
		GetComponent<Touchable>().onTouchEnd += OnTouchEnd;
	}
	
	private void OnTouchBegin(Touch touch, Vector3 hitPos)
	{
		foreach(SynchronousNode node in otherNodes)
		{
			node.GetComponent<ConnectionNode>().OnTouchBegin(touch, hitPos);
		}
	}

	private void OnTouchMove(Touch touch)
	{
		foreach(SynchronousNode node in otherNodes)
		{
			node.GetComponent<ConnectionNode>().OnTouchMove(touch);
		}
	}

	private void OnTouchEnd(Touch touch)
	{
		foreach(SynchronousNode node in otherNodes)
		{
			node.GetComponent<ConnectionNode>().OnTouchEnd(touch);
		}
	}
}

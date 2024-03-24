using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TreeHandlerCanvas : MonoBehaviour
{
	public TreeHandler3D treeHandler;

	[Serializable]
	public struct BranchHandler
	{
		public TMP_InputField Length;
		public TMP_InputField OrientationX;
		public TMP_InputField OrientationY;
		public TMP_InputField OrientationZ;
	}
	public BranchHandler[] BranchHandlers;

	public (float[], Vector3[]) GetCanvasContent()
	{
		List<float> lengths = new();
		List<Vector3> orientations = new();

		foreach (BranchHandler branch in BranchHandlers)
		{
			lengths.Add(float.Parse(branch.Length.text));
			orientations.Add(new Vector3(
				float.Parse(branch.OrientationX.text),
				float.Parse(branch.OrientationY.text),
				float.Parse(branch.OrientationZ.text)
			));
		}

		return (lengths.ToArray(), orientations.ToArray());
	}
}

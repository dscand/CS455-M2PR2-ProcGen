using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeHandler3D : MonoBehaviour
{
	public TreeHandlerCanvas treeCanvas;
	public TreeBuilder3D[] TreeBuilders;


	public class Rule_EqualSplit_Custom : TreeBuilder3D.Rule_EqualSplit
	{
		public int branchCount;
		public float[] lengths;
		public Vector3[] orientations;

		public Rule_EqualSplit_Custom(float[] lengths, Vector3[] orientations)
		{
			branchCount = lengths.Length;
			this.lengths = lengths;
			this.orientations = orientations;
		}

		public override bool matches(TreeBuilder3D.Branch branch) {return true;}
		public override TreeBuilder3D.Branch[] rhs(TreeBuilder3D.Branch branch)
		{
			List<TreeBuilder3D.Branch> branches = new();

			for (int i = 0; i < branchCount; i++)
			{
				branches.Add(new(
					branch.end(),
					branch.orientation * Quaternion.Euler(orientations[i]),
					branch.length * lengths[i] / 100f
				));
			}

			return branches.ToArray();
		}
	}


	public void BuildTrees()
	{
		(float[] lengths, Vector3[] orientations) = treeCanvas.GetCanvasContent();
		Debug.Log($"[{string.Join(", ", lengths)}]");
		Debug.Log($"[{string.Join(", ", orientations)}]");

		TreeBuilder3D.Rule[] rules = {
			new TreeBuilder3D.Rule_ShortTerminate(),
			new TreeBuilder3D.Rule_DownTerminate(),
			new Rule_EqualSplit_Custom(lengths, orientations),
		};

		foreach (TreeBuilder3D treeBuilder in TreeBuilders)
		{
			TreeBuilder3D.Branch rootBranch = new(
				treeBuilder.transform.position,
				treeBuilder.RootBranch.transform.rotation,
				treeBuilder.RootBranch.GetComponent<Renderer>().bounds.size.y
			);

			treeBuilder.StopBuild();
			treeBuilder.DestroyTree();
			treeBuilder.StartBuild(rootBranch, rules);
		}
	}

	public void StopBuild()
	{
		foreach (TreeBuilder3D treeBuilder in TreeBuilders)
		{
			treeBuilder.StopBuild();
		}
	}

	public void DestroyTrees()
	{
		foreach (TreeBuilder3D treeBuilder in TreeBuilders)
		{
			treeBuilder.DestroyTree();
		}
	}
}

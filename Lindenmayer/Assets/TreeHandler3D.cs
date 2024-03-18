using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHandler3D : MonoBehaviour
{
	public TreeBuilder3D[] TreeBuilders;

	public void BuildTrees()
	{
		TreeBuilder3D.Rule[] rules = {
			new TreeBuilder3D.Rule_ShortTerminate(),
			new TreeBuilder3D.Rule_DownTerminate(),
			new TreeBuilder3D.Rule_EqualSplit(),
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

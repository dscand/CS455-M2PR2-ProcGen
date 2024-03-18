using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TreeBuilder3D : TreeBuilder
{
	public new class Rule_EqualSplit : Rule
	{
		public override bool matches(Branch branch) {return true;}
		public override Branch[] rhs(Branch branch)
		{
			Branch a = new(
				branch.end(),
				branch.orientation * Quaternion.Euler(45,0,0),
				branch.length * 2 / 3
			);
			
			Branch b = new(
				branch.end(),
				branch.orientation * Quaternion.Euler(45,120,0),
				branch.length * 2 / 3
			);
			
			Branch c = new(
				branch.end(),
				branch.orientation * Quaternion.Euler(45,240,0),
				branch.length * 2 / 3
			);

			return new Branch[] {a, b, c};
		}
	}


	/*void Start()
	{
		Rule[] rules = {
			new Rule_ShortTerminate(),
			new Rule_DownTerminate(),
			new Rule_EqualSplit(),
		};

		Branch rootBranch = new(
			transform.position,
			RootBranch.transform.rotation,
			RootBranch.GetComponent<Renderer>().bounds.size.y
		);

		StartCoroutine(BuildTree(rootBranch, rules));
	}*/
}
